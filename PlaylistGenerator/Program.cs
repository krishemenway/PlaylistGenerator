using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using PlaylistGenerator.Data;
using PlaylistGenerator.Data.Grouping;

namespace PlaylistGenerator
{
	public class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				var commandLineArguments = GetArgs(args.ToList());

				if (!commandLineArguments.ContainsKey(strings.ArgumentXMLPath) || string.IsNullOrEmpty(commandLineArguments[strings.ArgumentXMLPath]))
					throw new PlaylistGeneratorException(strings.NoXmlDataError);

				var playlist = Playlist.CreateInstance(commandLineArguments[strings.ArgumentXMLPath]);

				var blacklist = GetBlacklist();
				playlist.GlobalBlacklist = blacklist ?? new Blacklist();

				playlist.Groupings = GetGroupings();

				var eligibleFileTypes = GetEligibleFileTypes() ?? PlaylistBuilder.DefaultEligibileFileTypes;

				playlist.SetupPlaylist(eligibleFileTypes);
				playlist.Title = commandLineArguments.ContainsKey("name") ? commandLineArguments["name"] : string.Empty;

				if (commandLineArguments.ContainsKey(strings.ArgumentFileType) && !string.IsNullOrEmpty(commandLineArguments[strings.ArgumentFileType]))
					playlist.FileType = GetFileTypeFromArg(commandLineArguments[strings.ArgumentFileType]);

				string playlistString = eligibleFileTypes == null
						? new PlaylistBuilder().BuildPlaylist(playlist, IsPreviewMode(commandLineArguments))
						: new PlaylistBuilder(eligibleFileTypes).BuildPlaylist(playlist, IsPreviewMode(commandLineArguments));

				Console.Write(playlistString);
			}
			catch (PlaylistGeneratorException e)
			{
				Console.WriteLine(e.Message);
				PrintUsage(args);
				return;
			}
		}

		private static bool IsPreviewMode(Dictionary<string, string> argVars)
		{
			if (argVars.ContainsKey("preview"))
			{
				return true;
			}

			return false;
		}

		private static Blacklist GetBlacklist()
		{
			var blacklistpath = GetApplicationVariable("BlacklistPath");
			if(string.IsNullOrEmpty(blacklistpath))
				return null;

			return Blacklist.LoadBlacklist(blacklistpath);
		}

		private static VideoGrouping GetGroupings()
		{
			var groupingPath = GetApplicationVariable("Groups");
			if (string.IsNullOrEmpty(groupingPath))
				return null;

			return VideoGrouping.LoadGroups(groupingPath);
		}

		private static List<string> GetEligibleFileTypes()
		{
			var eligibleFileTypes = GetApplicationVariable("EligibleFileTypes");
			if(string.IsNullOrEmpty(eligibleFileTypes))
			{
				return null;
			}

			return eligibleFileTypes.Split(new[] { ',' }).ToList();
		}

		private static string GetApplicationVariable(string name)
		{
			var appReader = new AppSettingsReader();
			string value;
			try
			{
				value = (string)appReader.GetValue(name, typeof(string));
			}
			catch (InvalidOperationException)
			{
				value = string.Empty;
			}

			return value;
		}

		private static FileType GetFileTypeFromArg(string argVar)
		{
			switch(argVar)
			{
				case "pls":
					return FileType.VLanPlaylist;
				case "mpcpl":
					return FileType.MediaPlayerClassicPlaylist;
				case "wpl":
					return FileType.WplPlaylist;
				case "m3u":
					return FileType.M3UPlaylist;
				default:
					throw new PlaylistGeneratorException("Invalid FileType in parameter");
			}
		}

		private static void PrintUsage(string[] args)
		{
			Console.WriteLine(strings.Usage1);
			Console.WriteLine(strings.Usage2);
			Console.WriteLine(strings.Usage3);
			Console.WriteLine(string.Format("{0} -mode Mode [-max num] -out FileType [-paths \"D:\\Directory|D:\\Directory2\"] [-xml \"D:\\Directories.xml\"]",args[0]));
		}

		private static Dictionary<string, string> GetArgs(IList<string> args)
		{
			Dictionary<string, string> argVars = new Dictionary<string, string>();
			for (int i = 0; i < args.Count; i = i + 2)
			{
				if (args.Count <= i + 1 || string.IsNullOrEmpty(args[i + 1]))
				{
					argVars.Add(args[i].Trim('-'), string.Empty);
				}
				else if (args[i + 1].StartsWith("-"))
				{
					argVars.Add(args[i].Trim('-'), string.Empty);
					i--;
				}
				else
				{
					argVars.Add(args[i].Trim('-'), args[i + 1]);
				}
			}
			return argVars;
		}
	}

	public class PlaylistGeneratorException : Exception
	{
		public PlaylistGeneratorException(string message) : base(message)
		{
			
		}

		public PlaylistGeneratorException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
