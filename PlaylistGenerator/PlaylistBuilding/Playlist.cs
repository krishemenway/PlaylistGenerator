using System.Collections.Generic;

namespace PlaylistGenerator.PlaylistBuilding
{
	public class Playlist
	{
		public string Title { get; set; }
		public IList<Video> Videos = new List<Video>();
	}
}
