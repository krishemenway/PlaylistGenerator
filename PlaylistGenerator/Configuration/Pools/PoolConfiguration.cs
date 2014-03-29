using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PlaylistGenerator.Configuration.Pools
{
	[Serializable, XmlType("Pool")]
	public class PoolConfiguration
	{
		public PoolConfiguration()
		{
			Videos = new List<VideoConfiguration>();
			VideoDirectories = new List<VideoDirectoryConfiguration>();
		}

		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlElement("VideoConfiguration")]
		public List<VideoConfiguration> Videos { get; set; }

		[XmlElement("Directory")]
		public List<VideoDirectoryConfiguration> VideoDirectories { get; set; }

		[XmlAttribute("linear")]
		public bool IsLinear;

		[XmlAttribute("reload")]
		public bool ReloadWhenEmpty { get; set; }
	}
}
