using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ijw.Math;
using IjwFramework.Types;

namespace Ijw.Skeletal
{
	using Influence = Pair<float, Transform>;

	public struct Transform
	{
		public Quaternion rotation;
		public Vector3 translation;

		public Transform(Quaternion r, Vector3 t)
		{
			rotation = r;
			translation = t;
		}

		public static Transform Blend(float t, Transform a, Transform b)
		{
			return new Transform(
				Quaternion.Blend(t, a.rotation, b.rotation),
				Vector3.Lerp(t, a.translation, b.translation));
		}

		public static Transform BlendMany(Transform reference, IEnumerable<Influence> t)
		{
			Transform result = reference;
			float w = 0;

			foreach (var i in t)
			{
				if (w == 0)
				{
					result = i.Second;
					w = i.First;
				}
				else
				{
					float factor = i.First / (w + i.First);
					result = Transform.Blend(factor, result, i.Second);
					w += factor;
				}
			}

			return result;
		}

		public static Transform operator *(Transform a, Transform b)
		{
			return new Transform(a.rotation * b.rotation,
				a.translation * b.rotation + b.translation);
		}

		public Matrix ToMatrix()
		{
			float xx2 = rotation.x * rotation.x * 2;
			float yy2 = rotation.y * rotation.y * 2;
			float zz2 = rotation.z * rotation.z * 2;
			float xy2 = rotation.x * rotation.y * 2;
			float zw2 = rotation.z * rotation.w * 2;
			float xz2 = rotation.x * rotation.z * 2;
			float yw2 = rotation.y * rotation.w * 2;
			float yz2 = rotation.y * rotation.z * 2;
			float xw2 = rotation.x * rotation.w * 2;

			Matrix m;
			m.M11 = 1 - yy2 - zz2;
			m.M21 = xy2 + zw2;
			m.M31 = xz2 - yw2;

			m.M12 = xy2 - zw2;
			m.M22 = 1 - xx2 - zz2;
			m.M32 = yz2 + xw2;

			m.M13 = xz2 + yw2;
			m.M23 = yz2 - xw2;
			m.M33 = 1 - xx2 - yy2;

			m.M14 = m.M24 = m.M34 = 0;

			m.M41 = translation.x;
			m.M42 = translation.y;
			m.M43 = translation.z;
			m.M44 = 1;
			return m;
		}
	}
}
