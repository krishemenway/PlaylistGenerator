using System.IO;

namespace PlaylistGenerator.PlaylistBuilding
{
	public interface IDirectoryScanner
	{
		bool DirectoryExists(string path);
		string[] GetAllFiles(string path);
	}

	public class DirectoryScanner : IDirectoryScanner
	{
		public bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		public string[] GetAllFiles(string path)
		{
			return Directory.GetFiles(path, AllFiles, SearchOption.AllDirectories);
		}

		private const string AllFiles = "*";
	}
}
