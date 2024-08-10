using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Carbon.Core.Plugins;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace TestLib
{

    public class WeatherPlugin : IPlugin
    {
        private WeatherPluginControl _control;

        public void Load()
        {
            Console.WriteLine("Weather plugin loaded");
        }

        public UserControl GetControl()
        {
            _control = new WeatherPluginControl();
            // Simulate updating weather information
            SimulateWeatherUpdate();
            return _control;
        }

        private void SimulateWeatherUpdate()
        {
            var random = new Random();
            // Generate a random weather forecast
            var weatherData = $"Temperature: {random.Next(-20, 40)}°C\n" +
                              $"Wind Speed: {random.Next(0, 30)} km/h\n" +
                              $"Humidity: {random.Next(0, 100)}%\n";

            // Update the weather information on the control
            _control.UpdateWeather(weatherData);
        }
    }
    public class WeatherPluginControl : UserControl
    {
        private TextBlock _weatherInfo;

        public WeatherPluginControl()
        {
            // Create and configure UI elements in code
            _weatherInfo = new TextBlock
            {
                FontSize = 16,
                FontWeight = FontWeight.Bold,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                Margin = new Thickness(10)
            };

            var border = new Border
            {
                BorderBrush = new SolidColorBrush(Colors.Gray),
                BorderThickness = new Thickness(1),
                Padding = new Thickness(10),
                Child = _weatherInfo
            };

            // Set the content of the UserControl
            Content = border;
        }

        public void UpdateWeather(string weatherData)
        {
            _weatherInfo.Text = weatherData;
        }
    }
}
