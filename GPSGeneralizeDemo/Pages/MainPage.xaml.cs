using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Geometry;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Map = Esri.ArcGISRuntime.Mapping.Map;
using GPSGeneralizeDemo.ViewModels;
using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Symbology;

namespace GPSGeneralizeDemo.Pages;

public partial class MainPage : ContentPage
{
    public Map Map = new();
    public PolylineBuilder polylineBuilder = new PolylineBuilder(SpatialReferences.WebMercator);
    public GraphicsOverlay markerGraphicsOverlay = new();
    public GraphicsOverlay polylineGraphicsOverlay = new();
    private MapViewModel mapViewModel;
    private SimpleMarkerSymbol markerSymbol = new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, System.Drawing.Color.Red, 10);
    private SimpleLineSymbol polylineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.CornflowerBlue, 5);

	public MainPage(MapViewModel mapViewModel)
	{
        this.mapViewModel = mapViewModel;
        InitializeComponent();
        SetupMap();
	}

    private void MainMapView_GeoViewTapped(object? sender, GeoViewInputEventArgs e)
    {
        if (mapViewModel.mapDrawModeOn)
        {
            EpsilonSlider.Value = 0;
            MapPoint droppedPoint = new MapPoint(e.Location.X, e.Location.Y, SpatialReferences.WebMercator);
            DrawPolylineOnMap(droppedPoint);
        }
        else
        {
            // do nothing
        }
    }

    private async void DrawPolylineOnMap(MapPoint droppedPoint)
    {
        try
        {
            Graphic droppedPointGraphic = new Graphic(droppedPoint, markerSymbol);
            markerGraphicsOverlay.Graphics.Add(droppedPointGraphic);
            polylineBuilder.AddPoint(droppedPoint);
            Graphic polylineGraphic = new Graphic
            {
                Geometry = polylineBuilder.ToGeometry(),
                Symbol = polylineSymbol
            };
            polylineGraphicsOverlay.Graphics.Clear();
            polylineGraphicsOverlay.Graphics.Add(polylineGraphic);
            ClearPolyline_Button.IsEnabled = true;
            if (polylineBuilder.Parts.FirstOrDefault().PointCount > 1)
            {
                UndoPolylineSegment_Button.IsEnabled = true;
            }
        } catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"{ex.Message}", "OK");
        }

    }

    private void SetupMap()
    {
        Map = new Map(BasemapStyle.ArcGISTopographic);

        var mapCenterPoint = new MapPoint(-118.805, 34.027, SpatialReferences.Wgs84);
        Map.InitialViewpoint = new Viewpoint(mapCenterPoint, 100000);
        MainMapView.Map = Map;
        MainMapView.GraphicsOverlays.Add(markerGraphicsOverlay);
        MainMapView.GraphicsOverlays.Add(polylineGraphicsOverlay);

        GeneralizeMethodPicker.ItemsSource = mapViewModel.LineGeneralizeMethodNamesArr;
        EpsilonSlider.Maximum = mapViewModel.MaxSliderVal;
        MainMapView.GeoViewTapped += MainMapView_GeoViewTapped;
    }

    private void EpsilonSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {

    }

    private void GeneralizeMethodPicker_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void DrawPolyline_Button_Clicked(object sender, EventArgs e)
    {
        mapViewModel.MapDrawModeOn = !mapViewModel.MapDrawModeOn;
        DrawPolyline_Button.IsEnabled = !mapViewModel.MapDrawModeOn;
        EpsilonSlider.Value = 0;

    }

    private void UndoPolylineSegment_Button_Clicked(object sender, EventArgs e)
    {
        int pointCount = polylineBuilder.Parts.Count() > 0 ? polylineBuilder.Parts.FirstOrDefault().PointCount : 0;
        if (pointCount > 0)
        {
            polylineBuilder.Parts.FirstOrDefault().RemovePoint(pointCount - 1);

            markerGraphicsOverlay.Graphics.RemoveAt(pointCount - 1);
            Graphic polylineGraphic = new Graphic
            {
                Geometry = polylineBuilder.ToGeometry(),
                Symbol = polylineSymbol
            };
            polylineGraphicsOverlay.Graphics.Clear();
            polylineGraphicsOverlay.Graphics.Add(polylineGraphic);

        } else { }

    }

    private void ClearPolyline_Button_Clicked(object sender, EventArgs e)
    {
        markerGraphicsOverlay.Graphics.Clear();
        polylineGraphicsOverlay.Graphics.Clear();
        polylineBuilder.Parts.Clear();

        DrawPolyline_Button.IsEnabled = true;
        mapViewModel.MapDrawModeOn = false;

        ClearPolyline_Button.IsEnabled = false;
        UndoPolylineSegment_Button.IsEnabled = false;
        ResetSliderValue();

    }

    private void ResetSliderValue()
    {
        EpsilonSlider.Value = 0;
    }
}