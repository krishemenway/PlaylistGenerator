using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace PlaylistGenerator.Data.Pools
{
	[Serializable, XmlType("Pool")]
	public class Pool
	{
		[XmlElement("Directory")]
		public List<VideoDirectory> VideoDirectories;

		[XmlElement("Video")]
		public List<Video> Videos;
		private BuildMethod _BuildMethod;
		public PullDirectoryMethod PullDirectoryMethod;
		public Playlist Playlist;
		private int _CurrentDirectory;
		private int _Counter;

		[XmlAttribute("name")]
		public string Name;

		public Pool()
		{
			Videos = new List<Video>();
			VideoDirectories = new List<VideoDirectory>();
		}

		public void SetupPool(List<string> eligibleFileTypes)
		{
			_Counter = 0;

			VideoDirectories.AddRange(Videos);
			Videos.Clear();

			List<VideoDirectory> directoriesToClean = new List<VideoDirectory>();
			foreach(var directory in VideoDirectories)
			{
				directory.Pool = this;
				directory.SetupVideoDirectory(eligibleFileTypes);

				if(directory.RemainingVideoCount == 0)
				{
					directoriesToClean.Add(directory);
				}
			}
			
			foreach(var directory in directoriesToClean)
			{
				VideoDirectories.Remove(directory);
			}
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

		public List<string> GetBlacklistedFiles()
		{
			var files = Playlist.GetBlacklistedFiles();
			files.AddRange(BlackListedFiles.Select(file => file.Location));

			return files;
		}

		private RefillAfterCompleteMode _RefillDirectoriesAfterComplete;
		[XmlAttribute("refillaftercomplete")]
		public RefillAfterCompleteMode RefillDirectoriesAfterComplete
		{
			get 
			{
				return _RefillDirectoriesAfterComplete != RefillAfterCompleteMode.None
						? _RefillDirectoriesAfterComplete : Playlist.RefillAfterComplete;
			}
			set
			{
				_RefillDirectoriesAfterComplete = value;
			}
		}

		[XmlAttribute("mode")]
		public BuildMethod Method
		{
			get { return _BuildMethod != BuildMethod.None ? _BuildMethod : Playlist.Method; }
			set { _BuildMethod = value; }
		}

		public int RemainingVideoCount
		{
			get { return VideoDirectories.Sum(x => x.RemainingVideoCount); }
		}

		public VideoDirectory GetVideoDirectory()
		{
			return GetVideoDirectory(Seed);
		}

		protected int Seed
		{
			get { return DateTime.Now.Millisecond % Int32.MaxValue; }
		}

		public VideoDirectory GetVideoDirectory(int seed)
		{
			_Counter++;
			switch (PullDirectoryMethod)
			{
				case PullDirectoryMethod.Linear:
					if (++_CurrentDirectory >= VideoDirectories.Count) _CurrentDirectory = 0;
					return VideoDirectories[_CurrentDirectory];
				case PullDirectoryMethod.Random:
					int directoryIndex = new Random(seed + _Counter).Next() % VideoDirectories.Count;
					return VideoDirectories[directoryIndex];
			}

			throw new PlaylistGeneratorException("No Pool Method Defined");
		}
	}
}
