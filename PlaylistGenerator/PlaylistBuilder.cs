using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlaylistGenerator.Data;

namespace PlaylistGenerator
{
	public class PlaylistBuilder
	{
		public static List<string> DefaultEligibileFileTypes = new List<string> { "avi", "wmv", "flv", "mpg", "mov", "mpeg", "ogv", "mp4" };
		public List<string> EligibleFileTypes;
		public PlayData Data;
		private const int DEFAULT_SEED = 20;

		public PlaylistBuilder() : this(DefaultEligibileFileTypes) { }

		public PlaylistBuilder(List<string> eligibileFileTypes)
		{
			EligibleFileTypes = eligibileFileTypes;
		}

		public string BuildPlaylist(Playlist playlist, bool previewMode = false)
		{
			Data = new PlayData();

			StringBuilder videoListBuilder = new StringBuilder();
			var currentVideoCount = 0;
			while (currentVideoCount < playlist.MaxVideos && playlist.RemainingVideoCount > 0)
			{
				foreach (var sequenceItem in playlist.Sequence)
				{
					var pool = playlist.GetPoolWithName(sequenceItem.Name);

					if (currentVideoCount >= playlist.MaxVideos) break;
					if (pool.VideoDirectories.Count == 0) continue;
					if (pool.VideoDirectories.Sum(dir => dir.Files.Count) == 0) continue;

					string currentVideo = previewMode
						? pool.GetVideoDirectory(DEFAULT_SEED).GetVideo(DEFAULT_SEED)
						: pool.GetVideoDirectory().GetVideo();

					Data.AddCountForVideo(currentVideo, 1);
					videoListBuilder.AppendLine(playlist.Formatter.GetLineForVideo(currentVideo, currentVideoCount));
					currentVideoCount++;
				}
			}

			StringBuilder playlistBuilder = new StringBuilder();
			playlistBuilder.AppendLine(playlist.Formatter.GetHeaderLine(playlist.Title, currentVideoCount));
			playlistBuilder.Append(videoListBuilder);
			Data.SavePlayData();
			playlistBuilder.AppendLine(playlist.Formatter.GetFooterLine(currentVideoCount));

			return playlistBuilder.ToString();
		}
	}
}
