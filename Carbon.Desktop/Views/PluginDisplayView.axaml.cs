using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Carbon.Core.Plugins;
using System.IO;
using System.Reflection;
using System;
using System.Collections.Generic;
using Carbon.Desktop.ViewModels;
using System.Linq;

namespace Carbon.Desktop.Views;

public partial class PluginDisplayView : UserControl
{
    private List<IPlugin> plugins = new List<IPlugin>();
    public PluginDisplayView()
    {
        InitializeComponent();
        LoadPlugins();
        DisplayPlugins();
    }


    private void DisplayPlugins()
    {
        PluginPanel.Children.Clear(); // Clear existing controls

        foreach (IPlugin plugin in plugins)
        {
            plugin.Load();

            UserControl control = plugin.GetControl();
            control.MaxHeight = 300;
            control.MaxWidth = 300; // Set MaxWidth if needed

            PluginPanel.Children.Add(control);
        }
    }
    private void LoadPlugins()
    {
        string pluginPath = "Plugins"; // Directory where plugins are stored
        var svm = new SettingsViewModel();
        if (!Directory.Exists(pluginPath))
            return;

        foreach (string file in Directory.GetFiles(pluginPath, "*.dll"))
        {
            Assembly assembly = Assembly.LoadFrom(file);
            foreach (Type type in assembly.GetTypes())
            {
                // Check if type is selected in the vm
                if (!svm.SelectedPlugins.Any(p => p.Name == type.Name))
                    continue;

                if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                    plugins.Add(plugin);
                }
            }

            // Order plugins to meet the same order as in the vm
            plugins = plugins.OrderBy(p => svm.SelectedPlugins.IndexOf(svm.SelectedPlugins.FirstOrDefault(sp => sp.Name == p.GetType().Name))).ToList();
        }
    }
}