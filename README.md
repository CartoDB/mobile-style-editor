## CARTO Style Editor (Alpha Preview)

###### Mobile app to preview and edit CARTO basemap styles (based on CartoCSS); written in Xamarin.Forms

## Get started

* Installer for **Android tablets** [here](https://rink.hockeyapp.net/apps/c005e5b5e4de4567a5bb6b82248005c4) (allow unknown sources)
* Installer for **iPad / iPhone** is not available, you would need to use free Xamarin SDK and this src project to build your own
* On phone it works read-only. Only with tablets and iPad you can also **edit styles**.
* In first start it requires github login. Use any account. This enables to load and save styles in your repos.
* Three sample styles (Voyager, Positron, Darkmatter) will be downloaded automatically,
you are then free to add additional styles via Github or Google Drive.
* Initial loading of sample styles will take long time - few minutes. Be patient.


## USAGE

When browsing styles on Github, a "SELECT" button will appear if you're in a folder that contains a **project.json** file, that is used as the identifier that you now, in fact, are in a folder that contains a style. If you're in a random folder that contains a project.json file, the select button will still appear, and you'll most likely crash the application if you downloaded that style.

Once downloaded, click on a style in the list to open the editor (on tablet devices) or viewer (on phones). In the top right corner there should be tabs that allow you to switch between css files. To load changes onto the map, press the reload button on the map below the zoom indicator.

The source is loaded from project.json. If your project.json does not contain the source key (`"source": "carto.streets"`), the default will be used and your map may not load.

There are three options to store your edited styles:

1. Save to the local database
2. Via email (just enter your email address and the style's .zip will be mailed to you
3. Upload to Github

## NOTES

* Upload to Github is only available if style was downloaded from Github and you haven't renamed the style.
* Changes will not be stored permanently unless you manually save your changes locally.
 - If you have renamed a style, a new copy will be saved
 - If you have not renamed a style, it will be overridden
 
 ## DISCLAIMER

This application is (relatively) untested, crashes, bugs and other "unexpected features" can be met.

 
## Running from source

You need Xamarin development tools, including Xamarin Forms to run the app from source.

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
