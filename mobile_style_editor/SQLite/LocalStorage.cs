
using System;
using System.Collections.Generic;
using SQLite;

namespace mobile_style_editor
{
	public class LocalStorage
	{
		public static LocalStorage Instance = new LocalStorage();

		SQLiteConnection db;

		public List<StoredStyle> Styles { get { return db.Query<StoredStyle>("select * from StoredStyle"); } }

        LocalStorage()
        {
#if __UWP__
            string folder = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
#else
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#endif
            const string file = "mobile_styles.db";

            try
            {
                db = new SQLiteConnection(System.IO.Path.Combine(folder, file));
            }
            catch (Exception e)
            {
                // TODO UWP Throws:
                // Unable to load DLL 'sqlite3': The specified module could not be found. (Exception from HRESULT: 0x8007007E)
                
                // Probably LocalStorage isn't necessary at this point anyway.
                // Should fix so it wouldn't save style urls, but rather read all files from stored styles directory
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            db.CreateTable<StoredStyle>();
        }

		public void AddStyle(string name, string path)
		{
			db.Insert(new StoredStyle { Name = name, Path = path });	
		}
	}
}
