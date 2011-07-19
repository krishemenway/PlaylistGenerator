using System;
using System.Xml.Serialization;

namespace PlaylistGenerator.Data
{
	[Serializable]
	public enum RefillAfterCompleteMode
	{
		[XmlEnum("")]
		None = 0,

		[XmlEnum("false")]
		NoRefillAfterComplete,

		[XmlEnum("true")]
		RefillDirectory
	}
}
