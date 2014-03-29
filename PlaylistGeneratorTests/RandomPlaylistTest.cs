using System.Collections.Generic;
using NUnit.Framework;
using PlaylistGenerator.PlaylistBuilding;
using PlaylistGeneratorTests.Configuration;

namespace PlaylistGeneratorTests
{
	[TestFixture]
	public class RandomPlaylistTest : PlaylistTests
	{
		[Test]
		public void ShouldGetVideosInRandomOrder()
		{
			var testPool = new PoolConfigurationBuilder()
				.WithName("Test")
				.WithVideoDirectory(isLinear: false, path: "C:\\Test\\")
				.WithVideoDirectory(isLinear: false, path: "C:\\Test2\\");

			GivenPlaylistConfiguration
				.WithPool(testPool)
				.WithSequenceItem("Test");

			GivenPathYieldsVideos("C:\\Test\\", new List<Video>
			{
				new Video("F:\\Test4.mp4"),
				new Video("F:\\Test2.mp4"),
				new Video("F:\\Test3.mp4")
			});

			GivenPathYieldsVideos("C:\\Test2\\", new List<Video>
			{
				new Video("F:\\Test5.mp4"),
				new Video("F:\\Test6.mp4"),
				new Video("F:\\Test7.mp4")
			});

			WhenBuildingPlaylist();

			ThenPlaylist.Videos.ShouldEqual(new List<Video>
			{
				new Video("F:\\Test6.mp4"),
				new Video("F:\\Test7.mp4"),
				new Video("F:\\Test4.mp4"),
				new Video("F:\\Test5.mp4"),
				new Video("F:\\Test2.mp4"),
				new Video("F:\\Test3.mp4")
			});
		}

		[Test]
		public void StressTest()
		{
			var testPool = new PoolConfigurationBuilder()
				.WithName("Test")
				.WithVideoDirectory(isLinear: false, path: "C:\\Test\\")
				.WithVideoDirectory(isLinear: false, path: "C:\\Test2\\");

			GivenPlaylistConfiguration
				.WithPool(testPool)
				.WithSequenceItem("Test");

			GivenPathYieldNRandomVideos("C:\\Test\\", 100);
			GivenPathYieldNRandomVideos("C:\\Test2\\", 150);

			WhenBuildingPlaylist();

			ThenPlaylist.Videos.ShouldHaveCount(250);
			ThenAllVideosShouldNotBeNull();
		}

		[SetUp]
		public override void Setup()
		{
			base.Setup();
		}
	}
}
