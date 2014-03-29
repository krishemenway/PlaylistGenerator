using System;
using System.IO;
using CommandLine;

namespace PlaylistGenerator
{
	public class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				var playlistGeneratorSettings = new PlaylistGeneratorSettings();
				Parser.Default.ParseArguments(args, playlistGeneratorSettings);

				var playlistGenerator = new PlaylistGenerator(playlistGeneratorSettings);
				using (var streamWriter = new StreamWriter(playlistGeneratorSettings.OutputFilePath))
				{
					playlistGenerator.Generate(streamWriter.Write);
				}
			}
			catch (PlaylistGeneratorException e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}
