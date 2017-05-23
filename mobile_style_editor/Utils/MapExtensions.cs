using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Ui;
using Carto.Utils;
using Carto.VectorTiles;
using Xamarin.Forms;

namespace mobile_style_editor
{
	public static class MapExtensions
	{
		const string OSM = "nutiteq.osm";

        public static void Update(this MapView MapView, byte[] data, Action completed, Action<string> failed = null)
        {
            System.Threading.Tasks.Task.Run(delegate
            {
                BinaryData styleAsset = new BinaryData(data);

                var package = new ZippedAssetPackage(styleAsset);
                var styleSet = new CompiledStyleSet(package);

                // UWP doesn't have a version released where simply changing the style set is supported,
                // need to clear layers and recreate the entire thing
#if __UWP__
				MapView.Layers.Clear();
		
				var source = new CartoOnlineTileDataSource(OSM);
				var decoder = new MBVectorTileDecoder(styleSet);
                
				var layer = new VectorTileLayer(source, decoder);
				Device.BeginInvokeOnMainThread(delegate
				{
					MapView.Layers.Add(layer);
                    if (completed != null)
                    {
                        completed();
                    }
				});
#else
                if (MapView.Layers.Count == 0)
                {
                    var source = new CartoOnlineTileDataSource(OSM);

                    MBVectorTileDecoder decoder = null;

                    try
                    {
                        decoder = new MBVectorTileDecoder(styleSet);
                    }
                    catch (System.ApplicationException e)
                    {
                        if (failed != null)
                        {
                            failed(e.Message);
                            return;
                        }
                    }

                    var layer = new VectorTileLayer(source, decoder);
                    Device.BeginInvokeOnMainThread(delegate
                    {
                        MapView.Layers.Add(layer);
                        if (completed != null)
                        {
                            completed();
                        }
                    });
                }
                else
                {
                    var decoder = (MBVectorTileDecoder)(MapView.Layers[0] as VectorTileLayer).TileDecoder;

                    Device.BeginInvokeOnMainThread(delegate
                    {
                        decoder.CompiledStyle = styleSet;

                        if (completed != null)
                        {
                            completed();
                        }
                    });
                }
#endif
            });
        }
	}
}
