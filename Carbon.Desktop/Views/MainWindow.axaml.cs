using Avalonia.Controls;
using Carbon.Core.Plugins;
using Carbon.Desktop.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Carbon.Desktop.Views
{
    public partial class MainWindow : Window
    {
        private List<IPlugin> plugins = new List<IPlugin>();
        public MainWindow()
        {
            InitializeComponent();
            LoadPlugins();
            DisplayPlugins();
        }
        private void LoadPlugins()
        {
            string pluginPath = "Plugins"; // Directory where plugins are stored

            if (!Directory.Exists(pluginPath))
                return;

            foreach (string file in Directory.GetFiles(pluginPath, "*.dll"))
            {
                Assembly assembly = Assembly.LoadFrom(file);
                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                        plugins.Add(plugin);
                    }
                }
            }
        }

        private void DisplayPlugins()
        {
            PluginGrid.Children.Clear(); // Clear existing controls
            PluginGrid.Rows = (int)Math.Ceiling((double)plugins.Count / 3); // Adjust the number of rows
            PluginGrid.Columns = Math.Min(5, plugins.Count); // Adjust the number of columns
            PluginGrid.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
            PluginGrid.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;

            foreach (IPlugin plugin in plugins)
            {
                plugin.Load();

                UserControl control = plugin.GetControl();
                PluginGrid.Children.Add(control);
            }
        }
    }
}