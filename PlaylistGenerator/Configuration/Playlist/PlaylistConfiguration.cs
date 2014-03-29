using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using PlaylistGenerator.Configuration.Pools;

namespace PlaylistGenerator.Configuration.Playlist
{
	[Serializable, XmlType("Playlist")]
	public class PlaylistConfiguration
	{
		public PlaylistConfiguration()
		{
		}

		[XmlAttribute("guid")]
		public Guid PlaylistGuid { get; set; }

		[XmlElement("Pools", typeof(List<PoolConfiguration>))]
		public List<PoolConfiguration> PoolConfigurations { get; set; }

		[XmlElement("Sequence", typeof(List<SequenceItemConfiguration>))]
		public List<SequenceItemConfiguration> Sequence { get; set; }

		[XmlAttribute("max")]
		public int MaxVideos { get; set; }

		[XmlAttribute("blacklist")]
		public string BlackListPath { get; set; }

		[XmlAttribute("title")]
		public string Title { get; set; }

		[XmlIgnore]
		public IPlaylistGeneratorSettings PlaylistGeneratorSettings { get; set; }

		public override string ToString() { return Title; }
	}
}
