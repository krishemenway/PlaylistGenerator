using System;

namespace PlaylistGenerator
{
	public interface IRandomProvider
	{
		Random Get();
	}

	public class RandomProvider : IRandomProvider
	{
		public RandomProvider(int? seed = null)
		{
			_seed = seed ?? DateTime.Now.Millisecond;
			_random = new Lazy<Random>(() => new Random(_seed));
		}

		public Random Get()
		{
			return _random.Value;
		}

		private int _seed;
		private readonly Lazy<Random> _random;
	}
}
