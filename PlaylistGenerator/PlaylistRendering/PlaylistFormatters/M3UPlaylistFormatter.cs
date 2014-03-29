using System;
using System.Globalization;
using PlaylistGenerator.PlaylistBuilding;

namespace PlaylistGenerator.PlaylistRendering.PlaylistFormatters
{
	public class M3UPlaylistFormatter : IPlaylistFormatter
	{
		public M3UPlaylistFormatter()
		{
		}

		public string GetHeader(Playlist playlist)
		{
			return "#EXTM3U" + Environment.NewLine;
		}

		public string GetPlaylistVideo(string path, int videoNumber)
		{
			if(path == null)
				throw new ArgumentNullException("path");

			return string.Format(CultureInfo.CurrentCulture, "#EXTINF:{0},{1}{2}", 0, path, Environment.NewLine)
				+ string.Format(CultureInfo.CurrentCulture, "{0}{1}", path, Environment.NewLine);
		}

		public string GetFooter(Playlist playlist)
		{
			return string.Empty;
		}
	}
}
