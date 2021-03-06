﻿using System;
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
			var m = rotation.ToMatrix();
			m.TranslationVector = translation;
			return m;
		}
	}
}
