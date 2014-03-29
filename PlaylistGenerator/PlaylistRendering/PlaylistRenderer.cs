using System;
using PlaylistGenerator.PlaylistBuilding;
using PlaylistGenerator.PlaylistRendering.PlaylistFormatters;

namespace PlaylistGenerator.PlaylistRendering
{
	internal interface IPlaylistRenderer
	{
		void Render(Playlist playlist, PlaylistFileType playlistFileType, PlaylistOutput playlistOutput);
	}

	internal class PlaylistRenderer : IPlaylistRenderer
	{
		public PlaylistRenderer(IPlaylistFormatRetriever playlistFormatRetriever = null)
		{
			_playlistFormatRetriever = playlistFormatRetriever ?? new PlaylistFormatRetriever();
		}

		public void Render(Playlist playlist, PlaylistFileType playlistFileType, PlaylistOutput playlistOutput)
		{
			var playlistFormatter = _playlistFormatRetriever.GetPlaylistFormatter(playlistFileType);

			playlistOutput(playlistFormatter.GetHeader(playlist));

			for (var playlistIndex = 0; playlistIndex < playlist.Videos.Count; playlistIndex++)
			{
				var location = playlist.Videos[playlistIndex].Location;
				playlistOutput(playlistFormatter.GetPlaylistVideo(location, playlistIndex));
			}

			playlistOutput(playlistFormatter.GetFooter(playlist));
		}

		private readonly IPlaylistFormatRetriever _playlistFormatRetriever;
	}
}