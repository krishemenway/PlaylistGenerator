using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistGenerator.PlaylistBuilding
{
	public interface IVideoScannerProvider
	{
		IVideoScanner GetVideoScanner(VideoScannerOptions videoScannerOptions);
	}

	public class VideoScannerProvider : IVideoScannerProvider
	{
		public IVideoScanner GetVideoScanner(VideoScannerOptions videoScannerOptions)
		{
			return new VideoScanner(videoScannerOptions);
		}
	}
}
