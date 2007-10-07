using System.Collections.Generic;
using System.Linq;
using Ijw.Math;
using System;

namespace Ijw.Skeletal
{
	public class Bone
	{
		readonly CoreBone coreBone;
		readonly Skeleton skeleton;

		Transform transform;
		Transform absTransform;
		Matrix matrix = Matrix.Identity;

		public Bone(CoreBone coreBone, Skeleton skeleton)
		{
			this.coreBone = coreBone;
			this.skeleton = skeleton;
			this.transform = coreBone.Transform;
		}

		public Bone Parent
		{
			get
			{
				if (coreBone.Parent == null)
					return null;
				return skeleton.GetBone(coreBone.Parent);
			}
		}

		public IEnumerable<Bone> Children
		{
			get { return coreBone.Children.Select(x => skeleton.GetBone(x)); }
		}

		public CoreBone CoreBone { get { return coreBone; } }

		public Transform Transform
		{
			get { return transform; }
			set
			{
				transform = value;

				absTransform = (Parent == null) 
					? transform 
					: transform * Parent.absTransform;

				matrix = Matrix.Identity;

				//matrix = (CoreBone.BoneSpace * absTransform).ToMatrix();
			}
		}

		public Matrix Matrix { get { return matrix; } }

		public IEnumerable<Vector3> AsLine()
		{
			yield return absTransform.translation;

			yield return (Parent == null)
				? Parent.absTransform.translation
				: Vector3.Zero;
		}

		public Vector3 Position { get { return absTransform.translation; } }
	}
}
