using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Ijw.Math;
using IjwFramework.Types;

namespace Ijw.Skeletal
{
	public class CoreMesh
	{
		Pair<Vertex,CoreBone>[] vertices;
		ushort[] indices;

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
	}
}
