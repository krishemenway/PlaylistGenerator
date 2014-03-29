using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PlaylistGenerator.Configuration.Blacklists
{
	[Serializable, XmlType("Blacklist")]
	public class Blacklist : List<BlacklistFile>
	{
	}
}
