## CARTO Style Studio Alpha Preview

###### Mobile app to Preview & Fix CARTO basemap styles (based on CartoCSS); written in Xamarin.Forms

#### IMPORTANT

Github authentication logic is not stored in the repository, you need to create your own `github_info.json` with the format:

```
{
	"username": "<your-username>",
	"pa_token": "<your-public-access-token>",
	"client_id": "<your-client-id>",
	"client_secret": "<your-client-secret>"
}
```

`client_id` and `client_secret` are the only required keys used in production mode, other keys are/were for debugging and development purposes.

Additional infomation available in Hubclient.cs: https://github.com/CartoDB/mobile-style-editor/blob/master/mobile_style_editor/Github/HubClient.cs

Additionally, the iOS and UWP versions require that you also create your own `drive_client_ids.json` with the following format:

```
{
	"user": "<your-user-name>",
	"description": "Identifiers to request data from Google Drive for com.carto.style.editor via Google Drive API for user {user} (refresh_token is user-specific",
	"client_id": "<your-client-id>",
	"client_secret": "<your-client-secret",
	"refresh_token": "<your-refresh-token>"
}
```
Additional information available in DriveClientiOS.cs: https://github.com/CartoDB/mobile-style-editor/blob/master/mobile_style_editor/Google/DriveClientiOS.cs

#### DISCLAIMER

This application is (relatively) untested, crashes, bugs and other "unexpected features" are bound to arise, expected even.

##### USAGE
Log in to your Github account, three smaple styles (Voyager, Positron, Darkmatter) will be downloaded automatically,
you are then free to add additional styles via Github or Google Drive.

When browsing styles on Github, a "SELECT" button will appear if you're in a folder that contains a **project.json** file, that is used as the identifier that you now, in fact, are in a folder that contains a style. If you're in a random folder that contains a project.json file, the select button will still appear, and you'll most likely crash the application if you downloaded that style.

Once downloaded, click on a style in the list to open the editor (on tablet devices) or viewer (on phones). In the top right corner there should be tabs that allow you to switch between css files. To load changes onto the map, press the reload button on the map below the zoom indicator.

The source is loaded from project.json. If your project.json does not contain the source key (`"source": "nutiteq.osm"`), the default will be used and your map may not load.

There are three options to store your edited styles:

1. Save to the local database
2. Via email (just enter your email address and the style's .zip will be mailed to you
3. Upload to Github

##### NOTES

* The third option is only available if it's downloaded gtom Github and you haven't renamed the style.
* Changes will not be stored permanently unless you manually save your changes locally.
 - If you have renamed a style, a new copy will be saved
 - If you have not renamed a style, it will be overridden
