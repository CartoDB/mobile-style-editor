
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Ui;
using Carto.Utils;
using Carto.VectorTiles;
using Xamarin.Forms;

#if __IOS__
using Xamarin.Forms.Platform.iOS;
#elif __ANDROID__
using Xamarin.Forms.Platform.Android;
#endif

namespace mobile_style_editor
{
	public class MainView : BaseView
	{
		public ActivityIndicator Loader { get; private set; }

		public Toolbar Toolbar { get; private set; }

		public MapView MapView { get; private set; }

		public CSSEditorView Editor { get; private set; }

		public MainView()
		{
			Loader = new ActivityIndicator();

			Toolbar = new Toolbar();
#if __IOS__
			MapView = new MapView();
#elif __ANDROID__
			MapView = new MapView(Forms.Context);
#endif
			Editor = new CSSEditorView();
		}

		public override void LayoutSubviews()
		{
			int platformPadding = Device.OnPlatform(20, 0, 0);

			double x = 0;
			double y = platformPadding;
			double w = Width;
			double h = Height / 7;

			AddSubview(Toolbar, x, y, w, h);

			y += h;
			w = Width / 3 * 1.9;
			h = Height - (h + platformPadding);

			AddSubview(MapView.ToView(), new Rectangle(x, y, w, h));

			x += w;
			w = Width - w;

			AddSubview(Editor, new Rectangle(x, y, w, h));

			if (Data != null)
			{
				Editor.Initialize(Data);
				Toolbar.Initialize(Data);
			}

			w = 50;
			h = 50;
			x = Width / 2 - w / 2;
			y = Height / 2 - h / 2;

			AddSubview(Loader, x, y, w, h);
		}

		public void ShowLoading()
		{
			Loader.IsRunning = true;
		}

		public void HideLoading()
		{
			Loader.IsRunning = false;
		}

		ZipData Data;

		public void Initialize(ZipData data)
		{
			Data = data;
			Editor.Initialize(Data);
			Toolbar.Initialize(Data);
		}

		const string OSM = "nutiteq.osm";

		public void UpdateMap(byte[] data)
		{
			MapView.Layers.Clear();

			System.Threading.Tasks.Task.Run(delegate
			{
				Console.WriteLine("Update map: starting");
				BinaryData styleAsset = new BinaryData(data);

				var package = new ZippedAssetPackage(styleAsset);
				var styleSet = new CompiledStyleSet(package);

				var source = new CartoOnlineTileDataSource(OSM);
				var decoder = new MBVectorTileDecoder(styleSet);

				var layer = new VectorTileLayer(source, decoder);
				Device.BeginInvokeOnMainThread(delegate
				{
					//MapView.Layers.Add(layer);
					Console.WriteLine("Update map: complete");
				});
			});
		}
	}
}
