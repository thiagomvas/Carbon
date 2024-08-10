using Carbon.Core.Plugins;

namespace TestLib
{
    public class WeatherPlugin : IPlugin
    {
        public void Load()
        {
            Console.WriteLine("Weather plugin loaded");
        }
    }
}
