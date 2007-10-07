using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Ijw.Math;

namespace Ijw.Skeletal
{
	static class Util
	{
		public static Quaternion ReadQuaternion(XmlNode n)
		{
			var parts = ReadValues(n);
			return new Quaternion(parts[0], parts[1], parts[2], parts[3]);
		}

		public static Vector3 ReadVector3(XmlNode n)
		{
			var parts = ReadValues(n);
			return new Vector3(parts[0], parts[1], parts[2]);
		}

		public static Vector2 ReadVector2(XmlNode n)
		{
			var parts = ReadValues(n);
			return new Vector2(parts[0], parts[1]);
		}

		static float[] ReadValues(XmlNode n)
		{
			return n.InnerText.Split(' ').Select(x => float.Parse(x)).ToArray();
		}

		public static IEnumerable<XmlElement> SelectElements(this XmlNode n, string xpath)
		{
			return n.SelectNodes(xpath).Cast<XmlElement>();
		}

		public static IEnumerable<XmlAttribute> SelectAttributes(this XmlNode n, string xpath)
		{
			return n.SelectNodes(xpath).Cast<XmlAttribute>();
		}
	}
}
