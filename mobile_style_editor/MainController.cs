
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace mobile_style_editor
{
    public class MainController : BaseController
    {
        MainView ContentView;

        ZipData data;

        string folder, filename;

        string TemporaryName { get { return "temporary-" + data.Filename; } }

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

        public MainController(string folder, string filename)
        {
            NavigationPage.SetHasNavigationBar(this, false);

            this.folder = folder;
            this.filename = filename;

            ContentView = new MainView();
            Content = ContentView;

            ContentView.NavigationBar.Title.Text = filename.ToUpper();
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

            ContentView.Popup.Content.Confirm.Clicked += OnConfirmButtonClicked;

            ContentView.MapView.SourceLabel.Done += OnSourceChanged;

#if __ANDROID__
			DriveClientDroid.Instance.UploadComplete += OnUploadComplete;
#elif __IOS__
            DriveClientiOS.Instance.UploadComplete += OnUploadComplete;
#elif __UWP__
            ContentView.Zoom.In.Click += ZoomIn;
            ContentView.Zoom.Out.Click += ZoomOut;
#endif
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

            ContentView.Popup.Content.Confirm.Clicked -= OnConfirmButtonClicked;

            ContentView.MapView.SourceLabel.Done -= OnSourceChanged;

#if __ANDROID__
			DriveClientDroid.Instance.UploadComplete -= OnUploadComplete;
#elif __IOS__
            DriveClientiOS.Instance.UploadComplete -= OnUploadComplete;
#elif __UWP__
            ContentView.Zoom.In.Click -= ZoomIn;
            ContentView.Zoom.Out.Click -= ZoomOut;
#endif
        }

        async void OnBackButtonPressed(object sender, EventArgs e)
        {
            if (ContainsUnsavedChanged)
            {
				string message = "If you go back now, you will lose unsaved changes";
				Alert("Attention!", message, null, async delegate {
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
            ContentView.ShowLoading();;

            string osm = (sender as SourceLabel).Text;
            MapExtensions.SourceId = osm;

            ContentView.UpdateMap(delegate {
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
				ContentView.Popup.Hide();
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

		void OnUploadButtonClicked(object sender, EventArgs e)
		{
			ShowPopup(PopupType.Upload);
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
            string path = CalculatedPath;
            Email.OpenSender(path, delegate {
                
                Device.BeginInvokeOnMainThread(delegate {
                    ContainsUnsavedChanged = false;   
                });

            },(obj) => {
                Alert("Whoops", obj, null);
            });
        }

		void ShowPopup(PopupType type)
		{
			if (currentWorkingName == null)
			{
				Toast.Show("You don't seem to have made any changes", ContentView);
				return;
			}

			ContentView.Popup.Show(type);
		}

		void OnConfirmButtonClicked(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				ContentView.ShowLoading();

				Task.Run(delegate
				{
					string name = ContentView.Popup.Content.Text;

					if (string.IsNullOrWhiteSpace(name))
					{
						Toast.Show("Please provide a name for your style", ContentView);
						return;
					}

					name += Parser.ZipExtension;

					if (ContentView.Popup.Type == PopupType.Upload)
					{
#if __ANDROID__
						DriveClientDroid.Instance.Upload(name, currentWorkingStream);
#elif __IOS__

						DriveClientiOS.Instance.Upload(name, currentWorkingStream);
#endif
					}
					else
					{
						if (!Directory.Exists(Parser.LocalStyleLocation))
						{
							Directory.CreateDirectory(Parser.LocalStyleLocation);
						}

						string source = Path.Combine(Parser.ApplicationFolder, currentWorkingName);
						string destination = Path.Combine(Parser.LocalStyleLocation, name);
						File.Copy(source, destination);

						NormalizeView(name + " saved to local database");
					}
				});
			});

		}

		string currentWorkingName;
		MemoryStream currentWorkingStream;

		void OnRefresh(object sender, EventArgs e)
		{
			int index = ContentView.FileTabs.ActiveIndex;

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
