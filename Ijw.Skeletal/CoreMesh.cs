using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Ijw.Math;
using IjwFramework.Types;
using System.IO;

namespace Ijw.Skeletal
{
	public class CoreMesh
	{
		readonly Pair<Vertex,CoreBone>[] vertices;
		readonly ushort[] indices;
		readonly string textureName;

		public CoreMesh(string filename, CoreSkeleton skeleton)
		{
			var doc = new XmlDocument();
			doc.Load(filename);

			vertices = doc.SelectElements("//VERTEX").Select(
				e => new Pair<Vertex, CoreBone>(
					new Vertex(e),
					skeleton.GetBone(int.Parse( e.SelectSingleNode("./INFLUENCE/@ID" ).Value )))).ToArray();

			indices = doc.SelectAttributes("//FACE/@VERTEXID").SelectMany(
				e => e.Value.Split(' ').Select(x => ushort.Parse(x))).ToArray();

			textureName = CoreMaterial.GetTextureFilename(
				Path.ChangeExtension(filename, ".xrf"));
		}

		public ushort[] GetIndices() { return indices; }

		public Vertex[] GetTransformedVertices(Skeleton skeleton)
		{
			var boneMatrices = skeleton.Bones.ToDictionary(
				x => x.CoreBone,
				x => x.Matrix);

			return vertices.Select(
				x => x.First.Transform(boneMatrices[x.Second])).ToArray();
		}

		public string TextureName { get { return textureName; } }
	}

	static class CoreMaterial
	{
		public static string GetTextureFilename(string filename)
		{
			var doc = new XmlDocument();
			doc.Load(filename);
			return doc.SelectSingleNode("//MAP").InnerText;
		}
	}
}
