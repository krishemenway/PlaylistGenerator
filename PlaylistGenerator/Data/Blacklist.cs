using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace PlaylistGenerator.Data
{
	[Serializable, XmlType("Blacklist")]
	public class Blacklist : List<BlacklistFile>
	{
		public static Blacklist LoadBlacklist(string path)
		{
			if(File.Exists(path))
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(Blacklist));

				var file = new FileStream(path, FileMode.Open);
				var blacklist = (Blacklist) xmlSerializer.Deserialize(file);
				file.Close();

				return blacklist;
			}

			throw new Exception(string.Format("Could not find file at path {0}", path));
		}
	}

	[Serializable, XmlType("File")]
	public class BlacklistFile
	{
		[XmlAttribute("location")]
		public string Location;
	}
}
