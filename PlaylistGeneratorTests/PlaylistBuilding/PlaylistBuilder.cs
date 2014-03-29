using System.Collections.Generic;
using PlaylistGenerator.PlaylistBuilding;

namespace PlaylistGeneratorTests.PlaylistBuilding
{
	public class PlaylistBuilder
	{
		public PlaylistBuilder()
		{
			_playlist = new Playlist();
		}

		public PlaylistBuilder WithVideos(IList<Video> videos)
		{
			_playlist.Videos = videos;
			return this;
		}

		public PlaylistBuilder WithTitle(string title)
		{
			_playlist.Title = title;
			return this;
		}

		public Playlist Build()
		{
			return _playlist;
		}

		private Playlist _playlist;
	}
}
