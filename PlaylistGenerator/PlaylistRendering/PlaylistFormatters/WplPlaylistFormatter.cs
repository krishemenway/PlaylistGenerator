using System;
using System.Globalization;
using PlaylistGenerator.PlaylistBuilding;

namespace PlaylistGenerator.PlaylistRendering.PlaylistFormatters
{
	internal class WPLPlaylistFormatter : IPlaylistFormatter
	{
		public string GetHeader(Playlist playlist)
		{
			if(playlist == null)
				throw new ArgumentNullException("playlist");

			return string.Format(CultureInfo.CurrentCulture, HeaderData, playlist.Title, playlist.Videos.Count);
		}

		public string GetPlaylistVideo(string path, int videoNumber)
		{
			if(path == null)
				throw new ArgumentNullException("path");

			return string.Format(CultureInfo.CurrentCulture, @"<media src=""{0}"" />", path.Replace("&", "&amp;"));
		}

		public string GetFooter(Playlist playlist)
		{
			return FooterData;
		}

		private const string HeaderData = @"<?wpl version=""1.0""?>
<smil>
	<head>
		<meta name=""Generator"" content=""PlaylistGenerator""/>
		<meta name=""IsNetworkFeed"" content=""0""/>
		<meta name=""ItemCount"" content=""{1}""/>
		<title>{0}</title>
	</head>
	<body>
		<seq>";

		private const string FooterData = "\t\t</seq>\n\t</body>\n</smil>";
	}
}
