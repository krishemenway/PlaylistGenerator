using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PlaylistGenerator.Configuration.Blacklists;

namespace PlaylistGenerator.PlaylistBuilding
{
	public interface IVideoScanner
	{
		IList<Video> GetVideosInDirectory(string directory);
	}

	public class VideoScanner : IVideoScanner
	{
		public VideoScanner(VideoScannerOptions videoScannerOptions, IDirectoryScanner directoryScanner = null)
		{
			_videoScannerOptions = videoScannerOptions;
			_directoryScanner = directoryScanner ?? new DirectoryScanner();
		}

		public IList<Video> GetVideosInDirectory(string directory)
		{
			if (_directoryScanner.DirectoryExists(directory))
			{
				return _directoryScanner
					.GetAllFiles(directory)
					.Where(
						file =>
							EligibleFileTypes.Contains(new FileInfo(file).Extension.Trim('.').ToLower())
							&& !Blacklist.Any(b => b.Location.Equals(file, StringComparison.CurrentCultureIgnoreCase)))
					.Select(file => new Video(file))
					.ToList();
			}

			throw new Exception(string.Format("Could not find directory {0}", directory));
		}

		private IEnumerable<string> EligibleFileTypes
		{
			get { return _videoScannerOptions.EligibleFileTypes; }
		}

		private IEnumerable<BlacklistFile> Blacklist
		{
			get { return _videoScannerOptions.Blacklist; }
		}

		private readonly VideoScannerOptions _videoScannerOptions;
		private readonly IDirectoryScanner _directoryScanner;
	}

	public class VideoScannerOptions
	{
		public IEnumerable<string> EligibleFileTypes { get; set; }
		public Blacklist Blacklist { get; set; }
	}
}
