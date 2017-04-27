
using System;
using Carto.Core;

namespace mobile_style_editor
{
	public class DriveFile
	{
		public string Id { get; set; }

		public string Name { get; set; }

#if __ANDROID__
#elif __UWP__
#else
        public static DriveFile FromGoogleApiDriveFile(Google.Apis.Drive.v3.Data.File file)
		{
			return new DriveFile { Id = file.Id, Name = file.Name };
		}
#endif

		public static DriveFile FromVariant(Variant file)
		{
			string id = file.GetObjectElement("id").String;
			string name = file.GetObjectElement("name").String;

			return new DriveFile { Id = id, Name = name };
		}
	}
}
