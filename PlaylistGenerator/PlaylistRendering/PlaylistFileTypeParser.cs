using System;
using System.Linq;
using System.Xml.Serialization;

namespace PlaylistGenerator.PlaylistRendering
{
	public interface IPlaylistFileTypeParser
	{
		PlaylistFileType Parse(string fileType);
	}

	public class PlaylistFileTypeParser : IPlaylistFileTypeParser
	{
		public PlaylistFileType Parse(string fileType)
		{
			return Enum.GetValues(typeof (PlaylistFileType))
				.Cast<PlaylistFileType>()
				.FirstOrDefault(value => GetXmlEnumValueForFileType(value).Equals(fileType, StringComparison.CurrentCultureIgnoreCase));
		}

		private string GetXmlEnumValueForFileType(PlaylistFileType playlistFileType)
		{
			var memInfo = typeof(PlaylistFileType).GetMember(playlistFileType.ToString());
			var attributes = memInfo[0].GetCustomAttributes(typeof(XmlEnumAttribute), false);
			return ((XmlEnumAttribute) attributes[0]).Name;
		}
	}
}