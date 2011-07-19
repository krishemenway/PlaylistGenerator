using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaylistGenerator.Formatters
{
	public class M3UPlaylistFormatter : IPlaylistFormatter
	{
		public string GetHeaderLine(string title, int totalCount)
		{
			return "#EXTM3U";
		}

		public string GetLineForVideo(string path, int videoNumber)
		{
			return path;
		}

		public string GetFooterLine(int totalCount)
		{
			return string.Empty;
		}
	}
}
