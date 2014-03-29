using System;
using System.Collections.Generic;
using PlaylistGenerator;
using PlaylistGenerator.Configuration;
using PlaylistGenerator.Configuration.Playlist;
using PlaylistGenerator.Configuration.Pools;
using Rhino.Mocks;

namespace PlaylistGeneratorTests.Configuration
{
	public class PlaylistConfigurationBuilder
	{
		public PlaylistConfigurationBuilder()
		{
			_instance = new PlaylistConfiguration
			{
				MaxVideos = int.MaxValue,
				BlackListPath = string.Empty,
				PlaylistGeneratorSettings = MockRepository.GenerateStub<IPlaylistGeneratorSettings>(),
				Sequence = new List<SequenceItemConfiguration>(),
				PoolConfigurations = new List<PoolConfiguration>(),
				PlaylistGuid = Guid.NewGuid(),
				Title = "Default Title"
			};

			_instance.PlaylistGeneratorSettings.Stub(x => x.EligibleFileTypes).Return(new List<string> {"mp4", "avi", "test"});
			_instance.PlaylistGeneratorSettings.Stub(x => x.BlacklistPath).Return(string.Empty);
		}

		public PlaylistConfigurationBuilder WithTitle(string title)
		{
			_instance.Title = title;
			return this;
		}

		public PlaylistConfigurationBuilder WithPool(PoolConfiguration poolConfiguration)
		{
			_instance.PoolConfigurations.Add(poolConfiguration);
			return this;
		}

		public PlaylistConfigurationBuilder WithPool(PoolConfigurationBuilder poolConfigurationBuilder)
		{
			return WithPool(poolConfigurationBuilder.Build());
		}

		public PlaylistConfigurationBuilder WithSequenceItem(string name)
		{
			_instance.Sequence.Add(new SequenceItemConfiguration { Name = name });
			return this;
		}

		public PlaylistConfiguration Build()
		{
			return _instance;
		}

		private readonly PlaylistConfiguration _instance;
	}
}
