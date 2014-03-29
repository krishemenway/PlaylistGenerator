using System;
using System.IO;
using System.Xml.Serialization;

namespace PlaylistGenerator.Configuration.Blacklists
{
	public interface IBlacklistStore
	{
		Blacklist Load(string blacklistPath);
	}

	public class BlacklistStore : IBlacklistStore
	{
		public Blacklist Load(string blacklistPath)
		{
			if (File.Exists(blacklistPath))
			{
				var xmlSerializer = new XmlSerializer(typeof(Blacklist));
				using (var file = new FileStream(blacklistPath, FileMode.Open))
				{
					return xmlSerializer.Deserialize(file) as Blacklist;
				}
			}

			throw new Exception(string.Format("Could not find file at path {0}", blacklistPath));
		}
	}
}
