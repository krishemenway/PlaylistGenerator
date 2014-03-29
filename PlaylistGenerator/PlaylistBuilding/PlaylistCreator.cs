using System.Linq;
using PlaylistGenerator.Configuration.Blacklists;
using PlaylistGenerator.Configuration.Playlist;

namespace PlaylistGenerator.PlaylistBuilding
{
	public interface IPlaylistBuilder
	{
		Playlist Build(PlaylistConfiguration playlistConfiguration);
	}

	public class PlaylistCreator : IPlaylistBuilder
	{
		public PlaylistCreator(IBlacklistStore blacklistStore = null, IVideoScannerProvider videoScannerProvider = null, IRandomProvider randomProvider = null)
		{
			_blacklistStore = blacklistStore ?? new BlacklistStore();
			_videoScannerProvider = videoScannerProvider ?? new VideoScannerProvider();
			_randomProvider = randomProvider ?? new RandomProvider();
		}

		public Playlist Build(PlaylistConfiguration playlistConfiguration)
		{
			var videoScannerOptions = new VideoScannerOptions
			{
				Blacklist = _blacklistStore.Load(GetBlacklistPath(playlistConfiguration)),
				EligibleFileTypes = playlistConfiguration.PlaylistGeneratorSettings.EligibleFileTypes
			};

			var videoScanner = _videoScannerProvider.GetVideoScanner(videoScannerOptions);

			var videoPoolsByName = playlistConfiguration
				.PoolConfigurations
				.ToDictionary(configuration => configuration.Name, configuration => new Pool(configuration, videoScanner, _randomProvider));

			var playlist = new Playlist();

			while (playlist.Videos.Count < playlistConfiguration.MaxVideos && videoPoolsByName.Any(x => x.Value.HasVideosRemaining))
			{
				foreach (var poolName in playlistConfiguration.Sequence)
				{
					if (playlist.Videos.Count >= playlistConfiguration.MaxVideos)
						break;

					var pool = videoPoolsByName[poolName.Name];
					if (!pool.HasVideosRemaining)
					{
						if(pool.ShouldReload)
							pool.ReinitializePool();
						else
							continue;
					}

					playlist.Videos.Add(pool.GetNextVideoDirectory().GetNextVideo());
				}
			}

			return playlist;
		}

		private static string GetBlacklistPath(PlaylistConfiguration playlistConfiguration)
		{
			return playlistConfiguration.BlackListPath ?? playlistConfiguration.PlaylistGeneratorSettings.BlacklistPath;
		}

		private readonly IBlacklistStore _blacklistStore;
		private readonly IVideoScannerProvider _videoScannerProvider;
		private IRandomProvider _randomProvider;
	}
}
