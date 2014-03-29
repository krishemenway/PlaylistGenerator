using NUnit.Framework;
using PlaylistGenerator;
using PlaylistGenerator.PlaylistRendering;

namespace PlaylistGeneratorTests
{
	[TestFixture]
	public class PlaylistFileTypeParserTests
	{
		[Test]
		[TestCase("pls", PlaylistFileType.PLSPlaylist)]
		[TestCase("m3u", PlaylistFileType.M3UPlaylist)]
		[TestCase("mpcpl", PlaylistFileType.MediaPlayerClassicPlaylist)]
		[TestCase("wpl", PlaylistFileType.WPLPlaylist)]
		public void ShouldGetCorrectFileTypeWithParameter(string givenFileType, PlaylistFileType expectedFileType)
		{
			Assert.AreEqual(expectedFileType, new PlaylistFileTypeParser().Parse(givenFileType.ToLower()));
			Assert.AreEqual(expectedFileType, new PlaylistFileTypeParser().Parse(givenFileType.ToUpper()));
		}
	}
}
