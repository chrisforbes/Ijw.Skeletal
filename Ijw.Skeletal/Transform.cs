using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ijw.Math;
using IjwFramework.Types;

namespace Ijw.Skeletal
{
	using Influence = Pair<float, Transform>;

	struct Transform
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
					result = Transform.Blend(i.First, reference, i.Second);
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

			return new Matrix(
				1 - yy2 - zz2, xy2 - zw2, xz2 + yw2, 0,
				xy2 + zw2, 1 - xx2 - zz2, yz2 - xw2, 0,
				xz2 - yw2, yz2 + xw2, 1 - xx2 - yy2, 0,
				translation.x, translation.y, translation.z, 1 );
		}
	}
}
