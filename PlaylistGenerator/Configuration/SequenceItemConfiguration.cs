using System;
using System.Xml.Serialization;

namespace PlaylistGenerator.Configuration
{
	[Serializable, XmlType("SequenceItem")]
	public class SequenceItemConfiguration
	{
		public SequenceItemConfiguration() { }

		[XmlAttribute("name")]
		public string Name;
	}
}
