using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace PlaylistGenerator.Data.Pools
{
	[Serializable, XmlType("Directory")]
	public class VideoDirectory
	{
		public List<string> Files;
		public List<string> BackupFiles;
		public Pool Pool;

		private int _CurVideo;
		private bool _IsFirstVideo = true;
		public string OnDeck;

		private BuildMethod _Method;
		[XmlAttribute("mode")]
		public BuildMethod Method
		{
			get { return _Method != BuildMethod.None ? _Method : Pool.Method; }
			set { _Method = value; }
		}

		private RefillAfterCompleteMode _RefillListAfterComplete;
		[XmlAttribute("refillaftercomplete")]
		public RefillAfterCompleteMode RefillListAfterComplete
		{
			get { return _RefillListAfterComplete != RefillAfterCompleteMode.None ? _RefillListAfterComplete : Pool.RefillDirectoriesAfterComplete; }
			set { _RefillListAfterComplete = value; }
		}

		public int RemainingVideoCount
		{
			get { return Files.Count; }
		}

		public Blacklist BlackListedFiles;
		private string _BlackListPath;
		[XmlAttribute("blacklist")]
		public string BlackListPath
		{
			get { return _BlackListPath; }
			set
			{
				_BlackListPath = value;

				if (BlackListedFiles == null)
				{
					BlackListedFiles = new Blacklist();
				}

				BlackListedFiles = Blacklist.LoadBlacklist(_BlackListPath);
			}
		}

		[XmlAttribute("location")]
		public string Location;

		[XmlAttribute("regex")]
		public string MatchRegex;

		public VideoDirectory()
		{
			Files = new List<string>();
		}

		public void SetupVideoDirectory(List<string> eligibleFileTypes)
		{
			if (!File.Exists(Location) && !Directory.Exists(Location)) return;

			_CurVideo = 0;

			if (MatchRegex == null) MatchRegex = string.Empty;

			Files = new List<string>();
			BackupFiles = new List<string>();
			LoadFiles(Location, eligibleFileTypes, MatchRegex);

			SetupBackup();
		}

		public VideoDirectory(string path, Pool pool, List<string> eligibleFileTypes)
		{
			if (!File.Exists(path) && !Directory.Exists(path)) return;
			Pool = pool;
			_CurVideo = 0;

			SetupVideoDirectory(eligibleFileTypes);
			SetupBackup();
		}

		protected void LoadFiles(string path, List<string> eligibileFileTypes, string regex)
		{
			if(Directory.Exists(path))
			{
				var files = Directory.GetFiles(path);
				var blacklistFiles = GetBlacklistedFiles();

				if (files != null)
				{
					foreach (var file in files)
					{
						if (eligibileFileTypes.Contains(file.Substring(file.LastIndexOf(".") + 1))
						    && Regex.IsMatch(file, regex) && !blacklistFiles.Contains(file))
						{
							Files.Add(file);
							BackupFiles.Add(file);
						}
					}
				}

				var directories = Directory.GetDirectories(path);

				if (directories != null)
				{
					foreach (var directory in directories)
					{
						LoadFiles(directory, eligibileFileTypes, regex);
					}
				}
			} else if (File.Exists(path))
			{
				Files.Add(path);
				BackupFiles.Add(path);
			}
		}

		protected List<string> GetBlacklistedFiles()
		{
			var files = Pool.GetBlacklistedFiles();
			files.AddRange(BlackListedFiles.Select(x => x.Location));

			return files;
		}

		private void SetupBackup()
		{
			string[] backupFiles = new string[Files.Count];
			Files.CopyTo(backupFiles);
			BackupFiles = backupFiles.ToList();
		}

		public void LoadNextGroupVideo(string currentVideo)
		{
			var group = Pool.Playlist.Groupings.FindGroupWithFile(currentVideo);
			if (group != null)
			{
				var nextVideoLocation = group.Files.IndexOf(currentVideo) + 1;
				if (nextVideoLocation < group.Files.Count)
				{
					OnDeck = group[nextVideoLocation];
				}
			}
		}

		public string GetVideo()
		{
			return GetVideo(Seed);
		}

		public string GetVideo(int seed)
		{
			string video;

			if(HasOnDeckVideo())
			{
				video = OnDeck;
				OnDeck = string.Empty;
				LoadNextGroupVideo(video);
			}
			else
			{
				if (Method == BuildMethod.RandomLinear && _IsFirstVideo)
				{
					video = GetRandomVideo(seed);
				} else if(Method == BuildMethod.LinearPlay || Method == BuildMethod.RandomLinear)
				{
					video = GetNextVideo();
				} else
				{
					video = GetRandomVideo(seed);
				}
			}

			_IsFirstVideo = false;
			RemoveVideoIfNeccesary(video);
			RefillIfNeccesary();
			return video;
		}

		protected bool HasOnDeckVideo()
		{
			return !string.IsNullOrEmpty(OnDeck);
		}

		public void RemoveVideoIfNeccesary(string video)
		{
			var n = Files.IndexOf(video);

			if (Method == BuildMethod.SingleRandom)
			{
				Files.RemoveAt(n);
			}
		}

		public void RefillIfNeccesary()
		{
			if (Files.Count == 0)
			{
				if (RefillListAfterComplete == RefillAfterCompleteMode.RefillDirectory)
				{
					BackupFiles.ForEach(x => Files.Add(x));
				}
				else
				{
					Pool.VideoDirectories.Remove(this);
				}
			}
		}

		public int Seed
		{
			get { return DateTime.Now.Millisecond % Int32.MaxValue; }
		}

		public string GetRandomVideo(int seed)
		{
			int n = new Random(seed + Files.Count).Next(Files.Count);
			_CurVideo = n + 1;

			return Files[n];
		}

		public string GetNextVideo()
		{
			if (_CurVideo == Files.Count)
				_CurVideo = 0;

			return Files[_CurVideo++];
		}
	}
}
