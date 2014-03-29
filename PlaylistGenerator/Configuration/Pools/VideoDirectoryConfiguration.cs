using System;
using System.Xml.Serialization;

namespace PlaylistGenerator.Configuration.Pools
{
	[Serializable, XmlType("Directory")]
	public class VideoDirectoryConfiguration
	{
		public VideoDirectoryConfiguration()
		{
		}

		[XmlAttribute("reload")]
		public bool ReloadWhenEmpty;

		[XmlAttribute("linear")]
		public bool IsLinear;

		[XmlAttribute("random-start")]
		public bool RandomStartPosition;

		[XmlAttribute("location")]
		public string Location;

		[XmlAttribute("regex")]
		public string MatchRegex;
	}
}
