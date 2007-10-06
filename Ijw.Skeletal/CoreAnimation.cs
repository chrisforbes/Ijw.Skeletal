using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Ijw.Skeletal
{
	public class CoreAnimation
	{
		readonly float duration;
		readonly List<CoreTrack> tracks = new List<CoreTrack>();

		public float Duration { get { return duration; } }

		public CoreAnimation(string filename, CoreSkeleton skeleton)
		{
			var doc = new XmlDocument();
			doc.Load(filename);

			duration = float.Parse(doc.SelectSingleNode("/ANIMATION/@DURATION").Value);
		}

		public CoreTrack GetTrack(CoreBone bone)
		{
			return tracks.FirstOrDefault(x => x.Bone == bone);
		}
	}
}
