using System;
using System.Collections.Generic;
using System.Xml;
using IjwFramework.Types;

namespace Ijw.Skeletal
{
	public class CoreBone
	{
		readonly int parentId;
		readonly string name;

		readonly Transform boneSpace;
		readonly Transform transform;

		readonly CoreSkeleton skeleton;

		readonly Lazy<CoreBone> parent;

		List<CoreBone> children = new List<CoreBone>();

		internal CoreBone(XmlElement e, CoreSkeleton skeleton)
		{
			parentId = int.Parse(e.SelectSingleNode("./PARENTID").InnerText);

			name = e.GetAttribute("NAME");
			boneSpace = new Transform(
				Util.ReadQuaternion(e.SelectSingleNode("./LOCALROTATION")),
				Util.ReadVector3(e.SelectSingleNode("./LOCALTRANSLATION")));

			transform = new Transform(
				Util.ReadQuaternion(e.SelectSingleNode("./ROTATION")),
				Util.ReadVector3(e.SelectSingleNode("./TRANSLATION")));

			this.skeleton = skeleton;

			parent = Lazy.New(() => 
			{ 
				var bone = skeleton.GetBone(parentId); 
				if (bone != null)
					bone.AddChild(this);
				return bone;
			});
		}

		internal void AddChild(CoreBone bone)
		{
			children.Add(bone);
		}

		public string Name { get { return name; } }

		public CoreBone Parent { get { return parent.Value; } }

		public IEnumerable<CoreBone> Children { get { return children; } }

		public Transform Transform { get { return transform; } }
		public Transform BoneSpace { get { return boneSpace; } }
	}
}
