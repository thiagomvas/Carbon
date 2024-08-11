using Avalonia.Controls;
using Carbon.Core.Plugins;
using Carbon.Desktop.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Carbon.Desktop.Views;

public partial class PluginDisplayView : UserControl
{
    private readonly PluginManager pluginManager = new();
    private readonly List<IPlugin> plugins = new();
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

        pluginManager.LoadPlugins(pluginPath);
        var plugs = pluginManager.Plugins;

        foreach (var selected in svm.SelectedPlugins)
        {
            var plugin = plugs.FirstOrDefault(p => p.GetType() == selected.PluginType);
            if (plugin != null)
            {
                plugins.Add(plugin);
            }
        }
    }
}