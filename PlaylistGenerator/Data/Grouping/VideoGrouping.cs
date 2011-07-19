using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace PlaylistGenerator.Data.Grouping
{
	[Serializable, XmlType("Groups")]
	public class VideoGrouping : List<Group>
	{
		public static VideoGrouping LoadGroups(string path)
		{
			if (File.Exists(path))
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(VideoGrouping));

				var file = new FileStream(path, FileMode.Open);
				var groups = (VideoGrouping)xmlSerializer.Deserialize(file);
				file.Close();

				return groups;
			}

			throw new Exception(string.Format("Could not find group file at path {0}", path));
		}

		public Group FindGroupWithFile(string video)
		{
			return this.FirstOrDefault(group => group.Files.Contains(video));
		}
	}

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
