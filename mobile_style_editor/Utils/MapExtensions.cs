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
		public static string DefaultSourceId = "carto.streets";

        public static void Update(this MapView MapView, bool withListener, byte[] data, Action completed, Action<string> failed = null)
        {
            System.Threading.Tasks.Task.Run(delegate
            {
                var sourceId = Parser.GetSourceFromData(data);

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
                    if (sourceId == null)
                    {
                        sourceId = DefaultSourceId;
                    }

                    var source = new CartoOnlineTileDataSource(sourceId);

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

                    if (withListener) {
                        var popupSource = new LocalVectorDataSource(MapView.Options.BaseProjection);
                        var popupLayer = new VectorLayer(popupSource);
                        MapView.Layers.Add(popupLayer);
                        layer.VectorTileEventListener = new VectorEventListener(popupSource);
                    }

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
                    var decoder = (MBVectorTileDecoder)(MapView.Layers[MapView.Layers.Count -1] as VectorTileLayer).TileDecoder;

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
