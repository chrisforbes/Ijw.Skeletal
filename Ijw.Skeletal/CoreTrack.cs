using System.Collections.Generic;
using System.Linq;
using System.Xml;
using IjwFramework.Types;

namespace Ijw.Skeletal
{
	public class CoreTrack
	{
		readonly CoreBone bone;
		readonly List<Pair<float, Transform>> keys = new List<Pair<float, Transform>>();

		public CoreTrack(XmlElement e, CoreSkeleton skeleton)
		{
			int boneId = int.Parse(e.GetAttribute("BONEID"));
			bone = skeleton.GetBone(boneId);

			foreach (XmlElement f in e.SelectNodes("./KEYFRAME"))
				keys.Add(new Pair<float, Transform>(float.Parse(f.GetAttribute("TIME")),
				new Transform(
					Util.ReadQuaternion(f.SelectSingleNode("./ROTATION")),
					Util.ReadVector(f.SelectSingleNode("./TRANSLATION")))));
		}

		public CoreBone Bone { get { return bone; } }
		public IEnumerable<Pair<float, Transform>> Keys { get { return keys; } }

		public Transform GetTransformAt(float time)
		{
			if (keys.Count == 1)
				return keys[0].Second;

			var before = keys.Last(x => x.First <= time);
			var after = keys.First(x => x.First >= time);

			float t = (time - before.First) / (after.First - before.First);

			return Transform.Blend(t, before.Second, after.Second);
		}
	}
}
