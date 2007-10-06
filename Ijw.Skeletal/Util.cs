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

		public static Vector3 ReadVector(XmlNode n)
		{
			var parts = ReadValues(n);
			return new Vector3(parts[0], parts[1], parts[2]);
		}

		static float[] ReadValues(XmlNode n)
		{
			return n.InnerText.Split(' ').Select(x => float.Parse(x)).ToArray();
		}
	}
}
