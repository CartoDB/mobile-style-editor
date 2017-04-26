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

		public static List<StoredStyle> ToStoredStyles(this string[] files)
		{
		List<StoredStyle> list = new List<StoredStyle>();

			foreach (string file in files)
			{
				string[] split = file.Split('/');

				string name = split[split.Length - 1];
				string path = file.Replace(name, "");

				list.Add(new StoredStyle { Name = name, Path = path });
			}
			return list;
		}
	}
}
