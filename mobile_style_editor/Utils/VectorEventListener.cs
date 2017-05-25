
using System;
using Carto.DataSources;
using Carto.Geometry;
using Carto.Graphics;
using Carto.Layers;
using Carto.Styles;
using Carto.VectorElements;

namespace mobile_style_editor
{
    public class VectorEventListener : VectorTileEventListener
    {
        LocalVectorDataSource source;

        public VectorEventListener(LocalVectorDataSource source)
        {
            this.source = source;
        }

        public override bool OnVectorTileClicked(Carto.Ui.VectorTileClickInfo clickInfo)
        {
            source.Clear();

            Color color = new Color(0, 100, 200, 150);

            Feature feature = clickInfo.Feature;
            Geometry geometry = feature.Geometry;

            PointStyleBuilder pointBuilder = new PointStyleBuilder();
            pointBuilder.Color = color;

            LineStyleBuilder lineBuilder = new LineStyleBuilder();
            lineBuilder.Color = color;

            PolygonStyleBuilder polygonBuilder = new PolygonStyleBuilder();
            polygonBuilder.Color = color;

            if (geometry is PointGeometry)
            {
                source.Add(new Point((PointGeometry)geometry, pointBuilder.BuildStyle()));
            }
            else if (geometry is LineGeometry)
            {
                source.Add(new Line((LineGeometry)geometry, lineBuilder.BuildStyle()));
            }
            else if (geometry is PolygonGeometry)
            {
                source.Add(new Polygon((PolygonGeometry)geometry, polygonBuilder.BuildStyle()));
            }
            else if (geometry is MultiGeometry)
            {
                GeometryCollectionStyleBuilder collectionBuilder = new GeometryCollectionStyleBuilder();
                collectionBuilder.PointStyle = pointBuilder.BuildStyle();
                collectionBuilder.LineStyle = lineBuilder.BuildStyle();
                collectionBuilder.PolygonStyle = polygonBuilder.BuildStyle();

                source.Add(new GeometryCollection((MultiGeometry)geometry, collectionBuilder.BuildStyle()));
            }

            BalloonPopupStyleBuilder builder = new BalloonPopupStyleBuilder();

            // Set a higher placement priority so it would always be visible
            builder.PlacementPriority = 10;

            string id = clickInfo.FeatureId.ToString();

            string name = "Info (id: " + id + ")";

            if (feature.Properties.GetObjectElement("name").String != "null") {
                name += " about " + feature.Properties.GetObjectElement("name").String;
            }

            string description = feature.Properties.ToString();

            BalloonPopup popup = new BalloonPopup(clickInfo.ClickPos, builder.BuildStyle(), name, description);

            source.Add(popup);

            return true;
        }
    }
}
