namespace PlaylistGenerator.Formatters
{
	public class WplPlaylistFormatter : IPlaylistFormatter
	{
		public const string HeaderData = @"<?wpl version=""1.0""?>
<smil>
	<head>
		<meta name=""Generator"" content=""PlaylistGenerator""/>
		<meta name=""IsNetworkFeed"" content=""0""/>
		<meta name=""ItemCount"" content=""{1}""/>
		<title>{0}</title>
	</head>
	<body>
		<seq>";

		public const string FooterData = "\t\t</seq>\n\t</body>\n</smil>";

		public string GetHeaderLine(string title, int totalCount)
		{
			return string.Format(HeaderData, title, totalCount);
		}

		public string GetLineForVideo(string path, int videoNumber)
		{
			return string.Format(@"			<media src=""{0}"" />", path.Replace("&","&amp;"));
		}

		public string GetFooterLine(int totalCount)
		{
			return FooterData;
		}
	}
}
