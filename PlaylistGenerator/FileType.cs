using System.Xml.Serialization;

namespace PlaylistGenerator
{
	public enum FileType
	{
		[XmlEnum("")]
		None,

		[XmlEnum("pls")]
		VLanPlaylist = 1,

		[XmlEnum("mpcpl")]
		MediaPlayerClassicPlaylist,

		[XmlEnum("wpl")]
		WplPlaylist,

		[XmlEnum("m3u")]
		M3UPlaylist
	}
}
