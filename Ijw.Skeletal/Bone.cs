using System.Collections.Generic;
using System.Linq;
using Ijw.Math;

namespace Ijw.Skeletal
{
	public class Bone
	{
		readonly CoreBone coreBone;
		readonly Skeleton skeleton;

		Transform transform;
		Transform absTransform;
		Matrix matrix;

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

				if (Parent == null)
					absTransform = transform;
				else
					absTransform = transform * Parent.absTransform;

				matrix = (CoreBone.BoneSpace * absTransform).ToMatrix();
			}
		}
	}
}
