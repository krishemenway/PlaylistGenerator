using System.Collections.Generic;
using PlaylistGenerator.Configuration.Pools;

namespace PlaylistGeneratorTests.Configuration
{
	public class PoolConfigurationBuilder
	{
		public PoolConfigurationBuilder()
		{
			_instance = new PoolConfiguration
				{
					Name = "Default Name",
					IsLinear = false,
					ReloadWhenEmpty = false,
					VideoDirectories = new List<VideoDirectoryConfiguration>(),
					Videos = new List<VideoConfiguration>()
				};
		}

		public PoolConfiguration Build()
		{
			return _instance;
		}

		private readonly PoolConfiguration _instance;

		public PoolConfigurationBuilder WithVideoDirectory(string path = "", bool isLinear = false, string matchRegex = "", bool reloadWhenEmpty = false)
		{
			_instance.VideoDirectories.Add(
				new VideoDirectoryConfiguration
				{
					Location = path,
					IsLinear = isLinear,
					MatchRegex = matchRegex,
					ReloadWhenEmpty = reloadWhenEmpty
				});
			return this;
		}

		public PoolConfigurationBuilder WithName(string name)
		{
			_instance.Name = name;
			return this;
		}
	}
}