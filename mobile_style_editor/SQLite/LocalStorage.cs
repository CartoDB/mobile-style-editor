
#if __UWP__
#else
using System;
using System.Collections.Generic;
using SQLite;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public class LocalStorage
	{
		const string ACCESSTOKEN = "access_token";

		public bool HasAccessToken
		{ 
			get { return Application.Current.Properties.ContainsKey(ACCESSTOKEN); }
		}

		public string AccessToken 
		{ 
			get { return (string)Application.Current.Properties[ACCESSTOKEN]; } 
			set 
			{
				if (!HasAccessToken)
				{
					Application.Current.Properties.Add(ACCESSTOKEN, value);
				}
				else
				{
					Application.Current.Properties[ACCESSTOKEN] = value;
				}

				Application.Current.SavePropertiesAsync();
			}
		}

		public static LocalStorage Instance = new LocalStorage();

		SQLiteConnection db;

        LocalStorage()
        {
#if __UWP__
            string folder = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
#else
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#endif
            const string file = "carto_style_editor.db";

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

	}
}
#endif