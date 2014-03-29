using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistGeneratorTests
{
	public static class Rand
	{
		static Rand()
		{
			_random = new Random();
		}

		public static int GetRandom(int minValue, int maxValue)
		{
			return _random.Next(minValue, maxValue);
		}

		private static Random _random;
	}
}
