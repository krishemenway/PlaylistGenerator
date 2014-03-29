using System;
using System.Xml.Serialization;

namespace PlaylistGenerator.Configuration.Pools
{
	[Serializable,XmlType("VideoConfiguration")]
	public class VideoConfiguration : VideoDirectoryConfiguration
	{
	}
}
