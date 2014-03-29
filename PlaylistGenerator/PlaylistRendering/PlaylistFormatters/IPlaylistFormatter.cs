using PlaylistGenerator.PlaylistBuilding;

namespace PlaylistGenerator.PlaylistRendering.PlaylistFormatters
{
	public interface IPlaylistFormatter
	{
		string GetHeader(Playlist playlist);
		string GetPlaylistVideo(string path, int videoNumber);
		string GetFooter(Playlist playlist);
	}
}
