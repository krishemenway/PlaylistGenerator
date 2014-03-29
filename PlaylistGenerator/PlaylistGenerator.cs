using PlaylistGenerator.Configuration.Playlist;
using PlaylistGenerator.PlaylistBuilding;
using PlaylistGenerator.PlaylistRendering;

namespace PlaylistGenerator
{
	public class PlaylistGenerator
	{
		internal PlaylistGenerator(PlaylistGeneratorSettings settings, 
			IPlaylistRepository playlistRepository = null, 
			IPlaylistBuilder playlistBuilder = null, 
			IPlaylistRenderer playlistRenderer = null)
		{
			Settings = settings;

			_playlistConfigurationRepository = playlistRepository ?? new PlaylistConfigurationRepository();
			_playlistBuilder = playlistBuilder ?? new PlaylistCreator();
			_playlistRenderer = playlistRenderer ?? new PlaylistRenderer();
		}

		public void Generate(PlaylistOutput output)
		{
			var playlistConfiguration = _playlistConfigurationRepository.Find(Settings);
			var playlist = _playlistBuilder.Build(playlistConfiguration);
			_playlistRenderer.Render(playlist, Settings.PlaylistFormat, output);
		}

		private PlaylistGeneratorSettings Settings { get; set; }

		private readonly IPlaylistRepository _playlistConfigurationRepository;
		private readonly IPlaylistRenderer _playlistRenderer;
		private readonly IPlaylistBuilder _playlistBuilder;
	}

	public delegate void PlaylistOutput(string stringToWrite);
}
