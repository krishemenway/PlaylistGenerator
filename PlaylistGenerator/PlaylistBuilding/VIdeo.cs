namespace PlaylistGenerator.PlaylistBuilding
{
	public class Video
	{
		public Video(string location)
		{
			Location = location;
		}

		public int Length { get; set; }
		public string Location { get; set; }

		public override bool Equals(object obj)
		{
			return Location == ((Video)obj).Location;
		}

		public override string ToString()
		{
			return Location;
		}
	}
}
