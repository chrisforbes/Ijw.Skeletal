using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Ijw.Skeletal
{
	public class CoreSkeleton
	{
		readonly List<CoreBone> bones;

		public CoreSkeleton(string filename)
		{
			var doc = new XmlDocument();
			doc.Load(filename);

			bones = doc.SelectElements("//BONE").Select(
				x => new CoreBone(x, this)).ToList();
		}

		public int NumBones { get { return bones.Count; } }

		public CoreBone GetBone(string name)
		{
			return bones.FirstOrDefault(x => x.Name == name);
		}

		public CoreBone GetBone(int index)
		{
			return index < 0 ? null : bones[index];
		}

		public IEnumerable<CoreBone> Bones { get { return bones; } }
	}
}
