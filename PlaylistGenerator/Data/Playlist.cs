using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using PlaylistGenerator.Data.Grouping;
using PlaylistGenerator.Data.Pools;
using PlaylistGenerator.Data.Sequence;
using PlaylistGenerator.Formatters;

namespace PlaylistGenerator.Data
{
	[Serializable, XmlType("Playlist")]
	public class Playlist
	{
		public Playlist() { }

		public static Playlist CreateInstance(string xmlConfigurationFile)
		{
			var serializer = new XmlSerializer(typeof(Playlist));
			var stream = new FileStream(xmlConfigurationFile, FileMode.Open);
			var playlist = (Playlist)serializer.Deserialize(stream);
			stream.Close();
			return playlist;
		}

		[XmlAttribute("mode")]
		public BuildMethod Method;

		[XmlElement("Pools",typeof(List<Pool>))]
		public List<Pool> Pools;

		[XmlElement("Sequence",typeof(List<SequenceItem>))] 
		public List<SequenceItem> Sequence;

		[XmlAttribute("max")]
		public int MaxVideos;

		private RefillAfterCompleteMode _RefillAfterComplete;
		[XmlAttribute("refillaftercomplete")] 
		public RefillAfterCompleteMode RefillAfterComplete
		{
			get 
			{
				return _RefillAfterComplete != RefillAfterCompleteMode.None
						? _RefillAfterComplete : RefillAfterCompleteMode.NoRefillAfterComplete;
			} 
			set
			{
				_RefillAfterComplete = value;
			}
		}

		public Blacklist BlackListedFiles;
		private string _BlackListPath;
		[XmlAttribute("blacklist")] 
		public string BlackListPath
		{
			get { return _BlackListPath; }
			set
			{
				_BlackListPath = value;

				if(BlackListedFiles == null)
				{
					BlackListedFiles = new Blacklist();
				}

				BlackListedFiles = Blacklist.LoadBlacklist(_BlackListPath);
			}
		}

		public List<string> GetBlacklistedFiles()
		{
			return BlackListedFiles.Select(file => file.Location)
				.Union(GlobalBlacklist.Select(file => file.Location)).ToList();
		}

		public Blacklist GlobalBlacklist;

		public VideoGrouping Groupings;

		private FileType _FileType;
		[XmlAttribute("FileType")]
		public FileType FileType
		{
			get
			{
				return _FileType;
			}
			set
			{
				_FileType = value;

				SetFormatter(_FileType);
			}
		}

		public int RemainingVideoCount
		{
			get
			{
				return Pools.Sum(x => x.RemainingVideoCount);
			}
		}

		[XmlIgnore]
		public IPlaylistFormatter Formatter;
		public string Title;

		public void SetupPlaylist(List<string> eligibleFileTypes)
		{
			if (FileType == FileType.None)
				throw new PlaylistGeneratorException(strings.NoFileTypeSpecifiedError);

			SetFormatter(_FileType);

			foreach(var pool in Pools)
			{
				pool.Playlist = this;
				pool.SetupPool(eligibleFileTypes);
			}

			if (Pools.Count == 0) throw new PlaylistGeneratorException(strings.NoPoolDataError);

			if(MaxVideos <= 0)
			{
				MaxVideos = RemainingVideoCount;
			}

			if(Groupings == null)
				Groupings = new VideoGrouping();
		}

		public static BuildMethod GetMethodFromAttribute(XmlAttribute xmlAttribute)
		{
			if (string.IsNullOrEmpty(xmlAttribute.Value))
				return BuildMethod.None;

			switch (xmlAttribute.Value)
			{
				case "SingleRandom":
					return BuildMethod.SingleRandom;
				case "MultiRandom":
					return BuildMethod.MultiRandom;
				case "LinearPlay":
					return BuildMethod.LinearPlay;
				default:
					throw new PlaylistGeneratorException("Bad Build Method specified");
			}
		}

		public void SetFormatter(XmlAttribute xmlAttribute)
		{
			SetFormatter(xmlAttribute.Value);
		}

		public void SetFormatter(string extension)
		{
			switch (extension)
			{
				case "mpcpl":
					SetFormatter(FileType.MediaPlayerClassicPlaylist);
					break;
				case "pls":
					SetFormatter(FileType.VLanPlaylist);
					break;
				case "wpl":
					SetFormatter(FileType.WplPlaylist);
					break;
				case "m3u":
					SetFormatter(FileType.M3UPlaylist);
					break;
				default:
					SetFormatter(FileType.None);
					break;
			}
		}

		public Pool GetPoolWithName(string poolName)
		{
			var pools = Pools.Where(pool => pool.Name.Equals(poolName));

			if (pools.Count() > 1)
				throw new Exception("You created two pools with the same name...which I don't want you to do");

			if(pools.Count() == 0)
				throw new Exception("You tried to use a name of a pool in sequence that doesn't exist");

			return pools.FirstOrDefault();
		}

		public void SetFormatter(FileType fileType)
		{
			switch(fileType)
			{
				case FileType.MediaPlayerClassicPlaylist:
					Formatter = new MediaPlayerClassicPlaylistFormatter();
					break;
				case FileType.VLanPlaylist:
					Formatter = new VideoLanPlaylistFormatter();
					break;
				case FileType.WplPlaylist:
					Formatter = new WplPlaylistFormatter();
					break;
				case FileType.M3UPlaylist:
					Formatter = new M3UPlaylistFormatter();
					break;
				default:
					Formatter = null;
					break;
			}
		}
	}
}
