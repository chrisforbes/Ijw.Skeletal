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

		internal CoreBone GetBone(string name)
		{
			return bones.FirstOrDefault(x => x.Name == name);
		}

		internal CoreBone GetBone(int index)
		{
			return index < 0 ? null : bones[index];
		}

		internal IEnumerable<CoreBone> Bones { get { return bones; } }
	}
}
