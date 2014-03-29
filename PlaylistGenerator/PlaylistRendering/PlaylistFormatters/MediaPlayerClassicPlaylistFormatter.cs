using System;
using PlaylistGenerator.PlaylistBuilding;

namespace PlaylistGenerator.PlaylistRendering.PlaylistFormatters
{
	public class MediaPlayerClassicPlaylistFormatter : IPlaylistFormatter
	{
		public string GetHeader(Playlist playlist)
		{
			return "MPCPLAYLIST" + Environment.NewLine;
		}

		public string GetPlaylistVideo(string path, int videoNumber)
		{
			return string.Format("{0},type,0\n{0},filename,{1}{2}", videoNumber + 1, path, Environment.NewLine);
		}

		public string GetFooter(Playlist playlist)
		{
			return string.Empty;
		}
	}

}
