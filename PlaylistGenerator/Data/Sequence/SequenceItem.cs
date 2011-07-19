using System;
using System.Xml.Serialization;

namespace PlaylistGenerator.Data.Sequence
{
	[Serializable, XmlType("SequenceItem")]
	public class SequenceItem
	{
		[XmlAttribute("name")]
		public string Name;

		public SequenceItem() {}

		public SequenceItem(string name)
		{
			Name = name;
		}
	}
}
