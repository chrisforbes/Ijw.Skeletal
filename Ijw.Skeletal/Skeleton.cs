using System;
using System.Collections.Generic;
using System.Linq;
using Ijw.Math;

namespace Ijw.Skeletal
{
	public class Skeleton
	{
		readonly CoreSkeleton coreSkeleton;
		readonly Dictionary<CoreBone, Bone> bones;

		public Skeleton(CoreSkeleton coreSkeleton)
		{
			this.coreSkeleton = coreSkeleton;

			bones = coreSkeleton.Bones.Select(x => new Bone(x, this)).
				ToDictionary(x => x.CoreBone);
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

		public IEnumerable<Bone> Bones { get { return bones.Values; } }

		public IEnumerable<Vector3> GetBoneLines() { return Bones.SelectMany(x => x.AsLine()); }
		public IEnumerable<Vector3> GetBonePoints() { return Bones.Select(x => x.Position); }
	}
}
