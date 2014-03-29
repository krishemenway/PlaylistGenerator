using System.Collections.Generic;
using NUnit.Framework;
using PlaylistGenerator.PlaylistBuilding;
using PlaylistGeneratorTests.Configuration;

namespace PlaylistGeneratorTests
{
	[TestFixture]
	public class LinearTest : PlaylistTests
	{
		[Test]
		public void ShouldGetVideosInOrderWhenVideoDirectoryIsLinear()
		{
			var testPool = new PoolConfigurationBuilder()
				.WithName("Test")
				.WithVideoDirectory(isLinear: true, path: "C:\\Test\\");

			GivenPlaylistConfiguration
				.WithPool(testPool)
				.WithSequenceItem("Test");

			GivenPathYieldsVideos("C:\\Test\\", new List<Video>
			{
				new Video("F:\\Test4.mp4"),
				new Video("F:\\Test2.mp4"),
				new Video("F:\\Test3.mp4")
			});

			WhenBuildingPlaylist();

			ThenPlaylist.Videos.ShouldEqual(new List<Video>
			{
				new Video("F:\\Test2.mp4"),
				new Video("F:\\Test3.mp4"),
				new Video("F:\\Test4.mp4")
			});
		}

		[SetUp]
		public override void Setup()
		{
			base.Setup();
		}
	}
}
