using System.Xml.Serialization;

namespace PlaylistGenerator.PlaylistRendering
{
	public enum PlaylistFileType
	{
		[XmlEnum("")]
		None,

		[XmlEnum("pls")]
		PLSPlaylist = 1,

		[XmlEnum("mpcpl")]
		MediaPlayerClassicPlaylist,

		[XmlEnum("wpl")]
		WPLPlaylist,

		[XmlEnum("m3u")]
		M3UPlaylist
	}
}
