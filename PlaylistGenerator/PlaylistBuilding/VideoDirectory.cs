using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PlaylistGenerator.Configuration.Pools;

namespace PlaylistGenerator.PlaylistBuilding
{
	public class VideoDirectory
	{
		public VideoDirectory(VideoDirectoryConfiguration videoDirectoryConfiguration, IVideoScanner videoScanner, IRandomProvider randomProvider)
		{
			Videos = new List<Video>();

			_videoDirectoryConfiguration = videoDirectoryConfiguration;
			_videoScanner = videoScanner;
			_randomProvider = randomProvider;

			ReinitializeVideoDirectory();
		}

		public bool HasNextVideo
		{
			get
			{
				return Videos.Any() && (!_videoDirectoryConfiguration.RandomStartPosition || HasRandomStartPositionVideo());
			}
		}

		public Video GetNextVideo()
		{
			if (!HasNextVideo)
			{
				if(_videoDirectoryConfiguration.ReloadWhenEmpty)
				{
					ReinitializeVideoDirectory();
				}

				throw new Exception("What the fuck");
			}

			var videoIndex = GetNextVideoIndex();
			var nextVideo = Videos[videoIndex];

			Videos.RemoveAt(videoIndex);
			return nextVideo;
		}

		private int GetNextVideoIndex()
		{
			if(_videoDirectoryConfiguration.IsLinear)
			{
				if (_videoDirectoryConfiguration.RandomStartPosition)
				{
					return RandomStartPosition;
				}

				return 0;
			}
			
			return RandomPositionInVideos;
		}

		private bool VideoExists(int index)
		{
			return Videos.Count > index && Videos[index] != null;
		}

		private int RandomStartPosition
		{
			get
			{
				if (!_randomStartPosition.HasValue)
				{
					_randomStartPosition = RandomPositionInVideos / 2;
				}

				return _randomStartPosition.Value;
			}
		}

		private int RandomPositionInVideos
		{
			get { return _randomProvider.Get().Next(Videos.Count); }
		}

		private void ReinitializeVideoDirectory()
		{
			Videos = _videoScanner
				.GetVideosInDirectory(_videoDirectoryConfiguration.Location)
				.Where(DoesNotHaveRegexOrMatchesRegex)
				.OrderBy(x => x.Location)
				.ToList();

			_randomStartPosition = null;
		}

		private bool DoesNotHaveRegexOrMatchesRegex(Video video, int index)
		{
			return !HasRegex || Regex.Match(video.Location, _videoDirectoryConfiguration.MatchRegex).Success;
		}

		private bool HasRandomStartPositionVideo()
		{
			return VideoExists(RandomStartPosition);
		}

		private bool HasRegex { get { return !string.IsNullOrEmpty(_videoDirectoryConfiguration.MatchRegex); } }

		private IList<Video> Videos { get; set; }
		private int? _randomStartPosition;

		private readonly VideoDirectoryConfiguration _videoDirectoryConfiguration;
		private readonly IVideoScanner _videoScanner;
		private readonly IRandomProvider _randomProvider;
	}
}
