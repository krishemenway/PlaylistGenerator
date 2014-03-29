using System;
using System.IO;
using System.Xml.Serialization;
using PlaylistGenerator.Configuration.Grouping;

namespace PlaylistGenerator.Configuration.Playlist
{
	internal interface IPlaylistRepository
	{
		PlaylistConfiguration Find(PlaylistGeneratorSettings playlistGeneratorSettings);
	}

	internal class PlaylistConfigurationRepository : IPlaylistRepository
	{
		public PlaylistConfiguration Find(PlaylistGeneratorSettings playlistGeneratorSettings)
		{
			using (var stream = new FileStream(playlistGeneratorSettings.ConfigurationXMLPath, FileMode.Open))
			{
				var serializer = new XmlSerializer(typeof(PlaylistConfiguration));
				var playlistConfiguration = (PlaylistConfiguration)serializer.Deserialize(stream);
				playlistConfiguration.PlaylistGeneratorSettings = playlistGeneratorSettings;

				return playlistConfiguration;
			}
		}

		public static VideoGrouping GetEpisodeGroups(string episodeGroupPath)
		{
			if (File.Exists(episodeGroupPath))
			{
				var xmlSerializer = new XmlSerializer(typeof(VideoGrouping));
				using(var file = new FileStream(episodeGroupPath, FileMode.Open))
				{
					return xmlSerializer.Deserialize(file) as VideoGrouping;
				}
			}

			throw new Exception(string.Format("Could not find group file at path {0}", episodeGroupPath));
		}
	}
}
