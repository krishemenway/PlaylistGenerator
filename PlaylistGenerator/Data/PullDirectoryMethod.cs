using System;
using System.Xml.Serialization;

namespace PlaylistGenerator.Data
{
	[Serializable]
	public enum PullDirectoryMethod
	{
		[XmlEnum("Random")]
		Random,

		[XmlEnum("Linear")]
		Linear
	}
}
