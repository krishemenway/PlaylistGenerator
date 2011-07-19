namespace PlaylistGenerator.Formatters
{
	public interface IPlaylistFormatter
	{
		string GetHeaderLine(string title, int totalCount);
		string GetLineForVideo(string path, int videoNumber);
		string GetFooterLine(int totalCount);
	}
}
