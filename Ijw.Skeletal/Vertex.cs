using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ijw.Math;
using System.Xml;

namespace Ijw.Skeletal
{
	public struct Vertex
	{
		public readonly Vector3 position;
		public readonly Vector3 normal;
		public readonly Vector2 tex;

		public Vertex(XmlNode e)
		{
			position = Util.ReadVector3(e.SelectSingleNode("./POS"));
			normal = Util.ReadVector3(e.SelectSingleNode("./NORM"));
			tex = Util.ReadVector2(e.SelectSingleNode("./TEXCOORD"));
		}

		public Vertex(Vector3 pos, Vector3 norm, Vector2 t)
		{
			position = pos;
			normal = norm;
			tex = t;
		}

		internal Vertex Transform(Matrix m)
		{
			return new Vertex(
				position.TransformAsCoordinate(m),
				normal.TransformAsNormal(m),
				tex);
		}
	}
}
