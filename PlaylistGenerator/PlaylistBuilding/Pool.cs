using System.Collections.Generic;
using System.Linq;
using PlaylistGenerator.Configuration.Pools;

namespace PlaylistGenerator.PlaylistBuilding
{
	public class Pool
	{
		public Pool(PoolConfiguration poolConfiguration, IVideoScanner videoScanner, IRandomProvider randomProvider)
		{
			VideoDirectories = new List<VideoDirectory>();
			Videos = new List<Video>();

			_randomProvider = randomProvider;

			CurrentVideoDirectoryIndex = 0;
			_videoScanner = videoScanner;
			PoolConfiguration = poolConfiguration;
			InitializePool();
		}

		public VideoDirectory GetNextVideoDirectory()
		{
			return VideoDirectories[GetIndexForNextVideoDirectory()];
		}

		private int GetIndexForNextVideoDirectory()
		{
			var nextVideoDirectoryIndex = -1;
			while (nextVideoDirectoryIndex == -1 || VideoDirectories.Count == nextVideoDirectoryIndex || !VideoDirectories[nextVideoDirectoryIndex].HasNextVideo)
			{
				nextVideoDirectoryIndex = PoolConfiguration.IsLinear ? CurrentVideoDirectoryIndex++ : RandomPositionInVideoDirectories;

				if (CurrentVideoDirectoryIndex >= VideoDirectories.Count)
				{
					nextVideoDirectoryIndex = CurrentVideoDirectoryIndex = 0;
				}

				if(!VideoDirectories[nextVideoDirectoryIndex].HasNextVideo)
				{
					VideoDirectories.RemoveAt(nextVideoDirectoryIndex);
				}
			}

			return nextVideoDirectoryIndex;
		}

		private int RandomPositionInVideoDirectories
		{
			get { return _randomProvider.Get().Next(VideoDirectories.Count); }
		}

		public void ReinitializePool()
		{
			if (ShouldReload && !HasVideosRemaining)
			{
				InitializePool();
			}
		}

		private void InitializePool()
		{
			VideoDirectories = PoolConfiguration
				.VideoDirectories
				.Select(directoryConfiguration => new VideoDirectory(directoryConfiguration, _videoScanner, _randomProvider))
				.Where(directory => directory.HasNextVideo)
				.ToList();

			Videos = PoolConfiguration
				.Videos
				.Select(s => new Video(s.Location))
				.ToList();
		}

		private int CurrentVideoDirectoryIndex { get; set; }

		public bool HasVideosRemaining { get { return Videos.Any() || VideoDirectories.Any(dir => dir.HasNextVideo); } }

		public bool ShouldReload { get { return PoolConfiguration.ReloadWhenEmpty; } }

		private IList<Video> Videos { get; set; }
		private IList<VideoDirectory> VideoDirectories { get; set; }
		private PoolConfiguration PoolConfiguration { get; set; }

		private readonly IVideoScanner _videoScanner;
		private readonly IRandomProvider _randomProvider;
	}
}
