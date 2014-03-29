using System.Collections.Generic;
using NUnit.Framework;
using PlaylistGenerator.PlaylistBuilding;
using PlaylistGenerator.PlaylistRendering;
using PlaylistGenerator.PlaylistRendering.PlaylistFormatters;
using PlaylistGeneratorTests.PlaylistBuilding;
using Rhino.Mocks;

namespace PlaylistGeneratorTests.PlaylistRendering
{
	[TestFixture]
	public class PlaylistRendererTests
	{
		[Test]
		public void ShouldRenderHeaderAndFooterOnlyWithNoVideos()
		{
			WhenRenderingPlaylist();
			ThenHeaderWasRendered();
			ThenNoVideosWereRendered();
			ThenFooterWasRendered();
		}

		[Test]
		public void ShouldRenderAllVideosInPlaylistWhenProvided()
		{
			GivenPlaylist
				.WithVideos(_allVideos);

			WhenRenderingPlaylist();
			ThenHeaderWasRendered();
			ThenVideosWereRendered();
			ThenFooterWasRendered();
		}

		private void ThenVideosWereRendered()
		{
			for (var videoIndex = 0; videoIndex < _allVideos.Count; videoIndex++)
			{
				int index = videoIndex;
				_playlistFormatter.AssertWasCalled(x => x.GetPlaylistVideo(_allVideos[index].Location, index));
			}
		}

		private void ThenFooterWasRendered()
		{
			_playlistFormatter.AssertWasCalled(x => x.GetFooter(Playlist));
		}

		private void ThenNoVideosWereRendered()
		{
			_playlistFormatter.AssertWasNotCalled(x => x.GetPlaylistVideo(Arg<string>.Is.Anything, Arg<int>.Is.Anything));
		}

		private void ThenHeaderWasRendered()
		{
			_playlistFormatter.AssertWasCalled(x => x.GetHeader(Playlist));
		}

		private void WhenRenderingPlaylist()
		{
			new PlaylistRenderer(_playlistFormatRetriever)
				.Render(Playlist, _playlistFileType, RenderToResult);
		}

		[SetUp]
		public void Setup()
		{
			_playlistFormatRetriever = MockRepository.GenerateStub<IPlaylistFormatRetriever>();
			_playlistFormatter = MockRepository.GenerateStub<IPlaylistFormatter>();
			_playlistFormatRetriever.Stub(x => x.GetPlaylistFormatter(_playlistFileType)).Return(_playlistFormatter);

			_playlistFormatter.Stub(x => x.GetHeader(Arg<Playlist>.Is.Anything)).Return("Header");
			_playlistFormatter.Stub(x => x.GetFooter(Arg<Playlist>.Is.Anything)).Return("Footer");
			_playlistFormatter.Stub(x => x.GetPlaylistVideo(Arg<string>.Is.Anything, Arg<int>.Is.Anything)).Return("Video");

			_playlistFileType = PlaylistFileType.None;

			GivenPlaylist = new PlaylistBuilder();

			_allVideos = new List<Video>
			{
				new Video("Test Location"),
				new Video("Test Location 2")
			};
		}

		private void RenderToResult(string stringToRender)
		{
			_renderedResult += stringToRender;
		}

		private string _renderedResult;

		private IList<Video> _allVideos;
		private PlaylistFileType _playlistFileType;

		private Playlist Playlist { get { return GivenPlaylist.Build(); } }

		protected PlaylistBuilder GivenPlaylist;
		private IPlaylistFormatRetriever _playlistFormatRetriever;
		private IPlaylistFormatter _playlistFormatter;
	}
}
