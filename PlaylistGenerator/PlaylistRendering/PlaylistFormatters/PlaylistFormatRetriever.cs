using System;

namespace PlaylistGenerator.PlaylistRendering.PlaylistFormatters
{
	public interface IPlaylistFormatRetriever
	{
		IPlaylistFormatter GetPlaylistFormatter(PlaylistFileType playlistFileType);
	}

	public class PlaylistFormatRetriever : IPlaylistFormatRetriever
	{
		public IPlaylistFormatter GetPlaylistFormatter(PlaylistFileType playlistFileType)
		{
			switch (playlistFileType)
			{
				case PlaylistFileType.MediaPlayerClassicPlaylist:
					return new MediaPlayerClassicPlaylistFormatter();
				case PlaylistFileType.PLSPlaylist:
					return new VideoLanPlaylistFormatter();
				case PlaylistFileType.WPLPlaylist:
					return new WPLPlaylistFormatter();
				case PlaylistFileType.M3UPlaylist:
					return new M3UPlaylistFormatter();
			}

			throw new Exception(string.Format("Could not determine an output type for {0}", playlistFileType.ToString()));
		}
	}
}
