using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Geometry;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Map = Esri.ArcGISRuntime.Mapping.Map;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GPSGeneralizeDemo.ViewModels
{
    public partial class MapViewModel : ObservableObject
    {
        [ObservableProperty]
        public bool mapDrawModeOn = false; // MapDrawModeOn

        [ObservableProperty]
        public int maxSliderVal = 20; // MaxSliderVal


        [ObservableProperty]
        public List<string> lineGeneralizeMethodNamesArr = new List<string>
        {
            "Ramer-Douglas-Peucker"
        }; // LineGeneralizeMethodNamesArr
        public MapViewModel()
        {
        }

    }
}
