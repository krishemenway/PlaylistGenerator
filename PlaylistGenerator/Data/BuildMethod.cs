using System;
using System.Xml.Serialization;

namespace PlaylistGenerator.Data
{
	[Serializable]
	public enum BuildMethod
	{
		[XmlEnum("")]
		None = 0,

		[XmlEnum("SingleRandom")]
		SingleRandom = 1,

		[XmlEnum("MultiRandom")]
		MultiRandom = 2,

		[XmlEnum("Linear")]
		LinearPlay = 3,

		[XmlEnum("RandomLinear")]
		RandomLinear = 4
	}
}
