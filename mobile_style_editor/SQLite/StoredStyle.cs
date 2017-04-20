using System;
namespace mobile_style_editor
{
	public class StoredStyle
	{
		[SQLite.PrimaryKey, SQLite.AutoIncrement]
		public int Id { get; set; }

		public string Name { get; set; }

		public string Path { get; set; }
	}
}
