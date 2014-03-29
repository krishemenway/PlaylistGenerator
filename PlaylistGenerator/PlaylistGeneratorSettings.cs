using System.Collections.Generic;
using System.Configuration;
using CommandLine;
using PlaylistGenerator.PlaylistRendering;

namespace PlaylistGenerator
{
	public interface IPlaylistGeneratorSettings
	{
		IEnumerable<string> EligibleFileTypes { get; }
		string BlacklistPath { get; }
	}

	public class PlaylistGeneratorSettings : IPlaylistGeneratorSettings
	{
		public PlaylistGeneratorSettings()
			: this(new AppSettingsReader(), new PlaylistFileTypeParser()) { }

		public PlaylistGeneratorSettings(AppSettingsReader appSettingsReader, IPlaylistFileTypeParser playlistFileTypeParser)
		{
			_appSettingsReader = appSettingsReader;
			_playlistFileTypeParser = playlistFileTypeParser;

			RawEligibileFileTypes = GetApplicationVariable("EligibleFileTypes");
			BlacklistPath = GetApplicationVariable("BlacklistPath");
			GroupsPath = GetApplicationVariable("Groups");
		}

		[Option('m', "max")]
		public int MaxVideoCount { get; set; }

		[Option('n', "name")]
		public string Title { get; set; }

		[Option('x', "xml", Required = true)]
		public string ConfigurationXMLPath { get; set; }

		[Option('o', "output-path", Required = true)]
		public string OutputFilePath { get; set; }

		[Option('p', "playlist-format", Required = true)]
		public string RawPlaylistFormat { get; set; }

		public PlaylistFileType PlaylistFormat
		{
			get { return _playlistFileTypeParser.Parse(RawPlaylistFormat); }
		}

		[Option('f', "fileTypes")]
		public string RawEligibileFileTypes { get; set; }

		public IEnumerable<string> EligibleFileTypes
		{
			get 
			{
				return !string.IsNullOrWhiteSpace(RawEligibileFileTypes)
					? RawEligibileFileTypes.ToLower().Split(new[] { ',' })
					: DefaultEligibileFileTypes;
			}
		}

		[Option('b', "blacklist")]
		public string BlacklistPath { get; set; }

		[Option('g', "groups")]
		public string GroupsPath { get; set; }

		private string GetApplicationVariable(string settingName)
		{
			return _appSettingsReader.GetValue(settingName, typeof(string)) as string;
		}

		private readonly AppSettingsReader _appSettingsReader;
		private readonly IPlaylistFileTypeParser _playlistFileTypeParser;

		private static readonly IEnumerable<string> DefaultEligibileFileTypes
			= new List<string> { "avi", "wmv", "flv", "mpg", "mov", "mpeg", "ogv", "mp4", "mkv" };
	}
}
