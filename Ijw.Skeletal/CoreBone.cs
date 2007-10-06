using System;
using System.Collections.Generic;
using System.Xml;

namespace Ijw.Skeletal
{
	public class CoreBone
	{
		readonly CoreBone parent;
		readonly string name;

		readonly Transform boneSpace;
		readonly Transform transform;

		List<CoreBone> children = new List<CoreBone>();

		internal CoreBone(XmlElement e, Func<int, CoreBone> boneLookup)
		{
			int parentId = int.Parse(e.SelectSingleNode("./PARENTID").InnerText);
			parent = boneLookup(parentId);

			name = e.GetAttribute("NAME");
			boneSpace = new Transform(
				Util.ReadQuaternion(e.SelectSingleNode("./ROTATION")),
				Util.ReadVector(e.SelectSingleNode("./TRANSLATION")));

			transform = new Transform(
				Util.ReadQuaternion(e.SelectSingleNode("./LOCALROTATION")),
				Util.ReadVector(e.SelectSingleNode("./LOCALTRANSLATION")));
		}

		internal void AddChild(CoreBone bone)
		{
			children.Add(bone);
		}

		public string Name { get { return name; } }
		public CoreBone Parent { get { return parent; } }
		public IEnumerable<CoreBone> Children { get { return children; } }

		public Transform Transform { get { return transform; } }
		public Transform BoneSpace { get { return boneSpace; } }
	}
}
