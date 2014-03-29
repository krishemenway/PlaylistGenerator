using System;
using PlaylistGenerator.PlaylistBuilding;

namespace PlaylistGenerator.PlaylistRendering.PlaylistFormatters
{
	public class VideoLanPlaylistFormatter : IPlaylistFormatter
	{
		public string GetHeader(Playlist playlist)
		{
			return string.Format("[playlist]{0}", Environment.NewLine);
		}

		public string GetPlaylistVideo(string path, int videoNumber)
		{
			return string.Format("File{0}={1}{2}", videoNumber, path, Environment.NewLine);
		}

		public string GetFooter(Playlist playlist)
		{
			return string.Format("NumberOfEntries={0}{1}Version=2{1}", playlist.Videos.Count, Environment.NewLine);
		}
	}
}
