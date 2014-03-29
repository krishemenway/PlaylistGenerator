using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace PlaylistGenerator.Configuration.Grouping
{
	[Serializable, XmlType("Groups")]
	public class VideoGrouping : List<Group>
	{
		public Group FindGroupWithFile(string video)
		{
			return this.FirstOrDefault(group => group.Files.Contains(video));
		}
	}
}
