using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ijw.Skeletal
{
	public class Skeleton
	{
		readonly CoreSkeleton coreSkeleton;
		readonly Dictionary<CoreBone, Bone> bones = new Dictionary<CoreBone, Bone>();

		public Skeleton(CoreSkeleton coreSkeleton)
		{
			this.coreSkeleton = coreSkeleton;
			foreach (var coreBone in coreSkeleton.Bones)
				bones.Add(coreBone, new Bone(coreBone, this));
		}

		internal Bone GetBone(CoreBone bone)
		{
			return bones[bone];
		}

		public IEnumerable<Bone> RootBones
		{
			get { return bones.Values.Where(x => x.CoreBone.Parent == null); }
		}

		public void VisitBones(Action<Bone> action)
		{
			var open = new Stack<Bone>(RootBones);
			while (open.Count > 0)
			{
				var bone = open.Pop();
				action(bone);
				foreach (var child in bone.Children)
					open.Push(child);
			}
		}

		public void Animate(Mixer mixer)
		{
			VisitBones(x =>
			{
				var influences = mixer.GetInfluencesFor(x);
				x.Transform = Transform.BlendMany(x.CoreBone.Transform,
					influences);
			});
		}
	}
}
