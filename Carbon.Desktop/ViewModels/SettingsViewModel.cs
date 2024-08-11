using Avalonia.Collections;
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
                        AvailablePlugins.Add(new PluginItem { Name = plugin.GetType().Name, Plugin = plugin });
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
                foreach (var name in settings.PluginNames)
                {
                    var pluginItem = AvailablePlugins.FirstOrDefault(p => p.Name == name);
                    if (pluginItem != null)
                    {
                        SelectedPlugins.Add(pluginItem);
                    }
                }
            }
        }

        public void SavePluginSettings()
        {
            var settings = new PluginSettings
            {
                PluginNames = SelectedPlugins.Select(p => p.Name).ToList()
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
            if (!SelectedPlugins.Contains(plugin))
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
        public string Name { get; set; }
        public IPlugin Plugin { get; set; }
    }

    public class PluginSettings
    {
        public List<string> PluginNames { get; set; } = new List<string>();
    }
}
