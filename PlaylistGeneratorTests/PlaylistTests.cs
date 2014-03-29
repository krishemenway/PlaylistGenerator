using System.Collections.Generic;
using PlaylistGenerator;
using PlaylistGenerator.Configuration.Blacklists;
using PlaylistGenerator.PlaylistBuilding;
using PlaylistGeneratorTests.Configuration;
using Rhino.Mocks;

namespace PlaylistGeneratorTests
{
	public class PlaylistTests
	{
		protected void GivenPathYieldNRandomVideos(string path, int numberOfVideos)
		{
			var videos = new List<Video>();
			var trimmedPath = path.TrimEnd(new[] {'\\'});

			for (var videoNumber = 0; videoNumber < numberOfVideos; videoNumber++ )
			{
				videos.Add(new Video(string.Format("{0}\\TestVideo{1}.mp4", trimmedPath, videoNumber)));
			}

			GivenPathYieldsVideos(path, videos);
		}

		protected void GivenPathYieldsVideos(string path, IList<Video> videos)
		{
			_videoScanner.Stub(x => x.GetVideosInDirectory(path)).Return(videos);
		}

		protected void WhenBuildingPlaylist()
		{
			ThenPlaylist = new PlaylistCreator(_blacklistStore, _videoScannerProvider, _randomProvider)
				.Build(GivenPlaylistConfiguration.Build());
		}

		protected void ThenAllVideosShouldNotBeNull()
		{
			ThenPlaylist.Videos.AllObjectsShould(x => x != null, "One or more videos was null!");
		}

		public virtual void Setup()
		{
			_blacklistStore = MockRepository.GenerateStub<IBlacklistStore>();
			_videoScannerProvider = MockRepository.GenerateStub<IVideoScannerProvider>();
			_videoScanner = MockRepository.GenerateStub<IVideoScanner>();

			_randomProvider = new RandomProvider(RandomSeed);

			_videoScannerProvider.Stub(x => x.GetVideoScanner(Arg<VideoScannerOptions>.Is.Anything)).Return(_videoScanner);

			GivenPlaylistConfiguration = new PlaylistConfigurationBuilder();
		}

		protected PlaylistConfigurationBuilder GivenPlaylistConfiguration { get; set; }
		protected Playlist ThenPlaylist { get; set; }

		private const int RandomSeed = 1565415465;

		private IRandomProvider _randomProvider;
		private IVideoScanner _videoScanner;
		private IBlacklistStore _blacklistStore;
		private IVideoScannerProvider _videoScannerProvider;
	}
}
