using System;
using System.Xml.Serialization;

namespace PlaylistGenerator.Configuration.Blacklists
{
	[Serializable, XmlType("File")]
	public class BlacklistFile
	{
		[XmlAttribute("location")]
		public string Location;
	}
}