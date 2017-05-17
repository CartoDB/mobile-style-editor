
using System;
using System.Collections.Generic;
#if __UWP__
#else
using SQLite;
#endif
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

        #if __UWP__
#else
        SQLiteConnection db;
#endif
        LocalStorage()
        {
#if __UWP__
            //string folder = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
#else
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            const string file = "carto_style_editor.db";

            db = new SQLiteConnection(System.IO.Path.Combine(folder, file));
            db.CreateTable<StoredStyle>();
#endif

        }

    }
}
