using System;
using System.Threading.Tasks;
using Android.Gms.Common.Apis;
using Android.Gms.Drive;
using Android.Gms.Drive.Query;
using Android.Runtime;

namespace mobile_style_editor
{
	public static class GoogleExtensions
	{
		/* From:
		 * https://github.com/xamarin/GooglePlayServicesComponents/blob/master/drive/source/Additions/IPendingResultExtensions.cs
		 */
		public static async Task<IDriveApiMetadataBufferResult> QueryAsync(this IDriveApi api, GoogleApiClient apiClient, QueryClass query)
		{
			return (await api.Query(apiClient, query)).JavaCast<IDriveApiMetadataBufferResult>();
		}
	}
}
