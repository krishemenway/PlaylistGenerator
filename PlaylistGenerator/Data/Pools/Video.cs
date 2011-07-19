using System;
using System.Xml.Serialization;

namespace PlaylistGenerator.Data.Pools
{
	[Serializable,XmlType("Video")]
	public class Video : VideoDirectory
	{
	}
}
