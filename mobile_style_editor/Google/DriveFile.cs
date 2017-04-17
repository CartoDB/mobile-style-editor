
using System;

namespace mobile_style_editor
{
	public class DriveFile
	{
		public string Id { get; set; }

		public string Name { get; set; }

#if __IOS__
		public static DriveFile FromGoogleApiDriveFile(Google.Apis.Drive.v3.Data.File file)
		{
			return new DriveFile { Id = file.Id, Name = file.Name };
		}
#endif
	}
}
