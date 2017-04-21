using System;
using System.Collections.Generic;
using System.Linq;

namespace mobile_style_editor
{
	public static class Extensions
	{
		public static List<object> ToObjects(this List<DriveFile> files)
		{
			return files.Cast<object>().ToList();
		}

		public static List<object> ToObjects(this List<StoredStyle> styles)
		{
			return styles.Cast<object>().ToList();
		}

	}
}
