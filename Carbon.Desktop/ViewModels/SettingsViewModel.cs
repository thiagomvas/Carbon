using Carbon.Core.Plugins;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Carbon.Desktop.ViewModels
{
    internal partial class SettingsViewModel : ViewModelBase
    {
        public ObservableCollection<PluginItem> AvailablePlugins { get; } = new ObservableCollection<PluginItem>();
        public ObservableCollection<PluginItem> SelectedPlugins { get; } = new ObservableCollection<PluginItem>();

        private const string SettingsFilePath = "plugin_settings.json";

        public SettingsViewModel()
        {
            LoadAvailablePlugins();
            LoadPluginSettings();
        }

        private void LoadAvailablePlugins()
        {
            string pluginPath = "Plugins";
            if (!Directory.Exists(pluginPath)) return;

            foreach (string file in Directory.GetFiles(pluginPath, "*.dll"))
            {
                Assembly assembly = Assembly.LoadFrom(file);
                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                        AvailablePlugins.Add(new PluginItem { PluginType = type, Plugin = plugin });
                    }
                }
            }
        }

        private void LoadPluginSettings()
        {
            if (File.Exists(SettingsFilePath))
            {
                string json = File.ReadAllText(SettingsFilePath);
                var settings = JsonConvert.DeserializeObject<PluginSettings>(json);
                foreach (var typeName in settings.PluginTypeNames)
                {
                    Type pluginType = Type.GetType(typeName);
                    if (pluginType != null)
                    {
                        var pluginItem = AvailablePlugins.FirstOrDefault(p => p.PluginType == pluginType);
                        if (pluginItem != null)
                        {
                            SelectedPlugins.Add(pluginItem);
                        }
                    }
                }
            }
        }

        public void SavePluginSettings()
        {
            var settings = new PluginSettings
            {
                PluginTypeNames = SelectedPlugins.Select(p => p.PluginType.AssemblyQualifiedName).ToList()
            };

            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(SettingsFilePath, json);
        }

        [RelayCommand]
        public void Save()
        {
            SavePluginSettings();
        }

        [RelayCommand]
        public void Reset()
        {
            SelectedPlugins.Clear();
            LoadPluginSettings();
        }

        [RelayCommand]
        public void AddPlugin(PluginItem plugin)
        {
            if (SelectedPlugins.Contains(plugin))
            {
                // Add new instance
                var newPlugin = new PluginItem { PluginType = plugin.PluginType, Plugin = (IPlugin)Activator.CreateInstance(plugin.PluginType) };
                SelectedPlugins.Add(newPlugin);
            }
            else
            {
                SelectedPlugins.Add(plugin);
            }
        }

        [RelayCommand]
        public void RemovePlugin(PluginItem plugin)
        {
            SelectedPlugins.Remove(plugin);
        }

        [RelayCommand]
        public void MoveUpPlugin(PluginItem plugin)
        {
            int index = SelectedPlugins.IndexOf(plugin);
            if (index > 0)
            {
                SelectedPlugins.Move(index, index - 1);
            }
        }

        [RelayCommand]
        public void MoveDownPlugin(PluginItem plugin)
        {
            int index = SelectedPlugins.IndexOf(plugin);
            if (index < SelectedPlugins.Count - 1)
            {
                SelectedPlugins.Move(index, index + 1);
            }
        }
    }

    public class PluginItem
    {
        public Type PluginType { get; set; }
        public IPlugin Plugin { get; set; }
    }

    public class PluginSettings
    {
        public List<string> PluginTypeNames { get; set; } = new List<string>();
    }
}
