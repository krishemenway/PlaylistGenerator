using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaylistGenerator.Formatters
{
	public class MediaPlayerClassicPlaylistFormatter : IPlaylistFormatter
	{
		public string GetHeaderLine(string title, int totalCount)
		{
			return "MPCPLAYLIST";
		}

		public string GetLineForVideo(string path, int videoNumber)
		{
			return string.Format("{0},type,0\n{0},filename,{1}", videoNumber + 1, path);
		}

		public string GetFooterLine(int totalCount)
		{
			return string.Empty;
		}
	}

}
