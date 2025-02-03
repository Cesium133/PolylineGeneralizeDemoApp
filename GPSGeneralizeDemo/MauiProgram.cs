using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Maui;
using GPSGeneralizeDemo.ViewModels;
using GPSGeneralizeDemo.Pages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace GPSGeneralizeDemo
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("GPSGeneralizeDemo.settings.json");

            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();

            var apiKey = config["ARCGIS_API_KEY"]; // Get value from config

            builder.UseArcGISRuntime(config => config.UseApiKey(apiKey));

            builder.Configuration.AddConfiguration(config);

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<MapViewModel>();
            builder.Services.AddSingleton<MainPage>();

            return builder.Build();
        }
    }

    public class Settings
    {
        public string ARCGIS_API_KEY { get; set; }
    }
}
