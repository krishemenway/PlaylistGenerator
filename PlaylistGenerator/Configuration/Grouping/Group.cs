using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PlaylistGenerator.Configuration.Grouping
{
	[Serializable, XmlType("Group")]
	public class Group
	{
		[XmlAttribute("name")]
		public string Name;

		[XmlElement("File")]
		public List<string> Files;

		public string this[int i]
		{
			get { return Files[i]; }
		}
	}
}