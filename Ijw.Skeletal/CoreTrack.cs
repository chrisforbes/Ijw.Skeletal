using System.Collections.Generic;
using System.Linq;
using System.Xml;
using IjwFramework.Types;

namespace Ijw.Skeletal
{
	public class CoreTrack
	{
		readonly CoreBone bone;
		readonly List<Pair<float, Transform>> keys;

		public CoreTrack(XmlElement e, CoreSkeleton skeleton)
		{
			int boneId = int.Parse(e.GetAttribute("BONEID"));
			bone = skeleton.GetBone(boneId);

			keys = e.SelectElements("./KEYFRAME").Select(
				x => new Pair<float, Transform>(
					float.Parse(x.GetAttribute("TIME")),
					new Transform(
						Util.ReadQuaternion(x.SelectSingleNode("./ROTATION")),
						Util.ReadVector3(x.SelectSingleNode("./TRANSLATION"))))).ToList();
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
