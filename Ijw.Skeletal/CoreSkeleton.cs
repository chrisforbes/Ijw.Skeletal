using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Ijw.Skeletal
{
	public class CoreSkeleton
	{
		List<CoreBone> bones = new List<CoreBone>();

		public CoreSkeleton(string filename)
		{
			var doc = new XmlDocument();
			doc.Load(filename);

			foreach (XmlElement e in doc.SelectNodes("/SKELETON/BONE"))
				bones.Add(new CoreBone(e, GetBone));
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
