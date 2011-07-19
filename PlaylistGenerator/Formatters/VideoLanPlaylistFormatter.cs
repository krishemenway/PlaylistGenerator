using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaylistGenerator.Formatters
{
	public class VideoLanPlaylistFormatter : IPlaylistFormatter
	{
		public string GetHeaderLine(string title, int totalCount)
		{
			return "[playlist]";
		}

		public string GetLineForVideo(string path, int videoNumber)
		{
			return string.Format("File{0}={1}", videoNumber, path);
		}

		public string GetFooterLine(int totalCount)
		{
			return string.Format("NumberOfEntries={0}\nVersion=2", totalCount);
		}
	}
}
