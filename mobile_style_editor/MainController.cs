
using System;
using System.IO;
using System.Threading.Tasks;
using Carto.Core;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class MainController : BaseController
    {
        MainView ContentView;

        ZipData data;

        string folder, filename;

        string TemporaryName { get { return "temporary-" + data.Filename; } }

        bool IsTemplateFolder { get { return folder.Contains(StyleListController.TemplateFolder); } }

        bool ContainsUnsavedChanged { get; set; }

        string CalculatedPath
        {
            get
            {
                if (currentWorkingName == null)
                {
                    return Path.Combine(folder, filename);
                }

                return Path.Combine(Parser.ApplicationFolder, TemporaryName);
            }
        }

        readonly SaveTimer timer = new SaveTimer();

        string owner { get { return GithubData.Owner; } }

        string repository { get { return GithubData.Name; } }

        string path { get { return GithubData.RepositoryPath; } }

        string branch { get { return GithubData.Branch; } }

        RepositoryData GithubData;

        public MainController(string folder, string filename)
        {
            NavigationPage.SetHasNavigationBar(this, false);

            this.folder = folder;
            this.filename = filename;

            ContentView = new MainView(IsTemplateFolder);
            Content = ContentView;

            ContentView.NavigationBar.Title.Text = filename.Replace(Parser.ZipExtension, "").ToUpper();

            string[] split = folder.Split('/');
            string styleFolder = split[split.Length - 1];
            var localPath = Path.Combine(styleFolder, filename.Replace(Parser.ZipExtension, ""));

            GithubData = LocalStorage.Instance.GetRepositoryData(localPath);


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ContentView.ShowLoading();

            Task.Run(delegate
            {
                data = Parser.GetZipData(folder, filename);

                Device.BeginInvokeOnMainThread(delegate
                {
                    ContentView.Initialize(data);
                });

                byte[] zipBytes = FileUtils.PathToByteData(data.DecompressedPath + Parser.ZipExtension);

                Device.BeginInvokeOnMainThread(delegate
                {
                    ContentView.UpdateMap(zipBytes, delegate
                    {
                        ContentView.HideLoading();
                    });
                });

            });

            ContentView.NavigationBar.Back.Click += OnBackButtonPressed;

            ContentView.FileTabs.OnTabTap += OnTabTapped;
            ContentView.Toolbar.Tabs.OnTabTap += OnTabTapped;

            ContentView.Toolbar.ExpandButton.Click += OnFileTabExpand;
            ContentView.Toolbar.UploadButton.Click += OnUploadButtonClicked;
            ContentView.Toolbar.SaveButton.Click += OnSaveButtonClicked;
            ContentView.Toolbar.EmailButton.Click += OnEmailButtonClicked;

            ContentView.MapView.RefreshButton.Click += OnRefresh;

            ContentView.GithubUpload.Content.Commit.Clicked += OnGithubCommitButtonClicked;

            ContentView.MapView.SourceLabel.Done += OnSourceChanged;

#if __ANDROID__
			DriveClientDroid.Instance.UploadComplete += OnUploadComplete;
#elif __IOS__
            DriveClientiOS.Instance.UploadComplete += OnUploadComplete;
#elif __UWP__
            ContentView.Zoom.In.Click += ZoomIn;
            ContentView.Zoom.Out.Click += ZoomOut;
#endif

            timer.Initialize(ContentView);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ContentView.NavigationBar.Back.Click -= OnBackButtonPressed;

            ContentView.FileTabs.OnTabTap -= OnTabTapped;
            ContentView.Toolbar.Tabs.OnTabTap -= OnTabTapped;

            ContentView.Toolbar.ExpandButton.Click -= OnFileTabExpand;
            ContentView.Toolbar.UploadButton.Click -= OnUploadButtonClicked;
            ContentView.Toolbar.SaveButton.Click -= OnSaveButtonClicked;
            ContentView.Toolbar.EmailButton.Click = OnEmailButtonClicked;

            ContentView.MapView.RefreshButton.Click -= OnRefresh;

            ContentView.GithubUpload.Content.Commit.Clicked -= OnGithubCommitButtonClicked;

            ContentView.MapView.SourceLabel.Done -= OnSourceChanged;

#if __ANDROID__
			DriveClientDroid.Instance.UploadComplete -= OnUploadComplete;
#elif __IOS__
            DriveClientiOS.Instance.UploadComplete -= OnUploadComplete;
#elif __UWP__
            ContentView.Zoom.In.Click -= ZoomIn;
            ContentView.Zoom.Out.Click -= ZoomOut;
#endif

            timer.Dispose();
        }

        async void OnBackButtonPressed(object sender, EventArgs e)
        {
            if (ContainsUnsavedChanged)
            {
                string message = "If you go back now, you will lose unsaved changes";
                Alert("Attention!", message, null, async delegate
                {
                    await Navigation.PopAsync(true);
                });
            }
            else
            {
                await Navigation.PopAsync(true);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (ContainsUnsavedChanged)
            {
                HandleUnsavedChanges();
                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }

        void HandleUnsavedChanges()
        {
            string message = "If you go back now, you will lose unsaved changes";
            Alert("Attention!", message, null, async delegate
            {
                await Navigation.PopAsync(true);
            });
        }

        void OnSourceChanged(object sender, EventArgs e)
        {
            ContentView.ShowLoading(); ;

            string osm = (sender as SourceLabel).Text;
            MapExtensions.SourceId = osm;

            ContentView.UpdateMap(delegate
            {
                ContentView.HideLoading();
            });
        }

        void ZoomIn(object sender, EventArgs e)
        {
            ContentView.MapView.SetZoom(ContentView.MapView.Zoom + 1, 0f);
        }

        void ZoomOut(object sender, EventArgs e)
        {
            ContentView.MapView.SetZoom(ContentView.MapView.Zoom - 1, 0f);
        }

        void NormalizeView(string text)
        {
            Device.BeginInvokeOnMainThread(delegate
            {
                ContentView.HideLoading();
                ContentView.GithubUpload.Hide();
                Toast.Show(text, ContentView);
            });
        }

        void OnFileTabExpand(object sender, EventArgs e)
        {
            ContentView.ToggleTabs();
        }

        void OnUploadComplete(object sender, EventArgs e)
        {
            NormalizeView("Upload of " + (string)sender + " complete");
        }

        async void OnUploadButtonClicked(object sender, EventArgs e)
        {
            /*
             * TODO
             * 1. Style location & name change logic
             * 2. "Create new branch" logic
             */
            ContentView.GithubUpload.Content.Update(owner, repository, path, branch);
            ContentView.GithubUpload.Show();
        }

        void OnGithubCommitButtonClicked(object sender, EventArgs e)
		{
            string message = "Are you sure you wish to commit your changes to Github?";
            Alert("Warning!", message, null, async delegate {

				ContentView.GithubUpload.ShowLoading();

				string comment = ContentView.GithubUpload.Content.Comment.Text;

				string error = await HubClient.Instance.Update(owner, repository, path, branch, ContentView.Data, comment);

				if (error != null)
				{
					Alert("Whoops!", error, null);
                } else
                {
                    ContentView.GithubUpload.Hide();

                    message = "Changes committed to " + repository + "/" + path + " (" + branch + ")";
                    Alert("Great success!", message, null);
                }

				ContentView.GithubUpload.HideLoading();
            });
		}

		void OnSaveButtonClicked(object sender, EventArgs e)
		{
            string message = "If you overwrite a saved style, the original style will be lost forever";
            Alert("Warning!", message, null, delegate {

                ContentView.ShowLoading();
                Task.Run(delegate
                {
                    string source = Path.Combine(Parser.ApplicationFolder, currentWorkingName);
                    string destination = Path.Combine(folder, filename);
                    File.Copy(source, destination, true);

                    Device.BeginInvokeOnMainThread(delegate {
                        ContainsUnsavedChanged = false;    
                        ContentView.HideLoading();
                    });

                });
            });
		}

        void OnEmailButtonClicked(object sender, EventArgs e)
        {
            Email.OpenSender(CalculatedPath, delegate {
                
                Device.BeginInvokeOnMainThread(delegate {
                    ContainsUnsavedChanged = false;   
                });

            },(obj) => {
                Alert("Whoops", obj, null);
            });
        }

		string currentWorkingName;
		MemoryStream currentWorkingStream;

		void OnRefresh(object sender, EventArgs e)
		{
			int index = ContentView.ActiveIndex;

            if (ContentView.Toolbar.Tabs.IsVisible)
            {
                // If Tool Tabs are visible, get that index instead of Popup tabs index,
                // cf Toolbar.cs line 67 for a more detailed explanation
                index = ContentView.Toolbar.Tabs.ActiveIndex;    
            }

			string text = ContentView.Editor.Text;

			if (index == -1)
			{
				System.Diagnostics.Debug.WriteLine("Couldn't find a single active tab");
				return;
			}

            UpdateDataAndMap(text, index);
		}

        void UpdateDataAndMap(string text, int index)
        {
            ContentView.ShowLoading();

            Task.Run(delegate
            {
                string path = data.StyleFilePaths[index];

                // Update file content in ZipData as well, in addition to saving it,
                data.DecompressedFiles[index] = text;

                FileUtils.OverwriteFileAtPath(path, text);
                
                string name = TemporaryName;

                string zipPath = Parser.Compress(data.DecompressedPath, name);

                // Get bytes to update style
                byte[] zipBytes = FileUtils.PathToByteData(zipPath);

                Device.BeginInvokeOnMainThread(delegate
                {
                    // Save current working data (name & bytes as stream) to conveniently upload
                    // Doing this on the main thread to assure thread safety

                    currentWorkingName = name;
                    currentWorkingStream = new MemoryStream(zipBytes);

                    ContentView.UpdateMap(zipBytes, delegate
                    {
                        ContentView.HideLoading();
                    });

                    ContainsUnsavedChanged = true;
                });
            });

        }

		void OnTabTapped(object sender, EventArgs e)
		{
			FileTab tab = (FileTab)sender;

			ContentView.Editor.Update(tab.Index);

            /*
             * If parent is FileTabs, all tabs are visible, ExpandButton is hidden
             * If ExpandButton is visible, tab.Parent will be FileTabPopup
             */
            if (tab.Parent is FileTabs)
            {
                // Do nothing. No UI-updates to handle
            }
            else
            {
                ContentView.FileTabs.Toggle();
                ContentView.Toolbar.ExpandButton.Update(tab.Text);
            }
		}

	}
}
