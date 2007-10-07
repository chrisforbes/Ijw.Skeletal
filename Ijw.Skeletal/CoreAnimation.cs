using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Ijw.Skeletal
{
	public class CoreAnimation
	{
		readonly float duration;
		readonly List<CoreTrack> tracks;

		public float Duration { get { return duration; } }

		public CoreAnimation(string filename, CoreSkeleton skeleton)
		{
			var doc = new XmlDocument();
			doc.Load(filename);

			duration = float.Parse(doc.SelectSingleNode("/ANIMATION/@DURATION").Value);

			tracks = doc.SelectElements("//TRACK").Select(
				e => new CoreTrack(e, skeleton)).ToList();
		}

		public CoreTrack GetTrack(CoreBone bone)
		{
			return tracks.FirstOrDefault(x => x.Bone == bone);
		}
	}
}
