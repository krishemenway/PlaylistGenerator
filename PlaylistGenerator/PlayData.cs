using System;
using System.Collections.Generic;
using System.IO;

namespace PlaylistGenerator
{
	public class PlayData
	{
		protected const string PlayDataFileName = "save.data";
		protected Stream PlayDataFile;
		private Dictionary<string, int> _playData;

		public PlayData()
		{
			PlayDataFile = File.Open(PlayDataFileName, FileMode.OpenOrCreate);
			_playData = new Dictionary<string, int>();
			LoadPlayData();
		}

		private void LoadPlayData()
		{
			var playDataReader = new StreamReader(PlayDataFile);

			while(!playDataReader.EndOfStream)
			{
				var line = playDataReader.ReadLine();

				try
				{
					var lineArray = line.Split(new[] {'\t'});
					var curfile = lineArray[0];
					var curcount = Convert.ToInt32(lineArray[1]);

					_playData.Add(curfile, curcount);
				} catch { }
			}

			playDataReader.Close();
		}

		public void SavePlayData()
		{
			PlayDataFile = File.Create(PlayDataFileName);
			var playDataWriter = new StreamWriter(PlayDataFile);

			foreach(KeyValuePair<string, int> video in _playData)
			{
				playDataWriter.WriteLine(string.Format("{0}\t{1}",video.Key,video.Value));
			}

			playDataWriter.Close();
		}

		public int GetPlayCountForVideo(string file)
		{
			if(!_playData.ContainsKey(file))
			{
				return 0;
			}

			return _playData[file];
		}

		public void AddCountForVideo(string file,int count)
		{
			if(!_playData.ContainsKey(file))
			{
				_playData.Add(file,0);
			}

			_playData[file] += count;
		}
	}
}
