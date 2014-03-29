using System;

namespace PlaylistGenerator
{
	[Serializable]
	public class PlaylistGeneratorException : Exception
	{
		public PlaylistGeneratorException(string message) : base(message) { }
		public PlaylistGeneratorException(string message, Exception exception) : base(message, exception) { }
	}
}