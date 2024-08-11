using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;

namespace Carbon.Desktop.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {

        [ObservableProperty]
        private ViewModelBase? _selectedViewModel = new PluginDisplayViewModel();

        [ObservableProperty]
        public ListItemTemplate? _selectedListItem;


        public ObservableCollection<ListItemTemplate> Items { get; } = new ObservableCollection<ListItemTemplate>
        {
            new ListItemTemplate("Home", typeof(PluginDisplayViewModel), "home_regular"),
            new ListItemTemplate("Settings", typeof(SettingsViewModel), "home_regular"),
        };

        partial void OnSelectedListItemChanged(ListItemTemplate? value)
        {
            if (value is null)
                return;
            var instance = Activator.CreateInstance(value.Type);
            if (instance is null)
                return;
            SelectedViewModel = (ViewModelBase)instance;
        }

        [RelayCommand]
        private void SelectPluginsView()
        {
            SelectedListItem = Items[0];
        }

        [RelayCommand]
        private void SelectSettingsView()
        {
            SelectedListItem = Items[1];
        }
    }


    public class ListItemTemplate
    {
        public StreamGeometry Icon { get; set; }
        public string Label { get; set; }
        public Type Type { get; set; }

        public ListItemTemplate(string label, Type type, string iconKey)
        {
            Label = label;
            Type = type;
            Application.Current!.TryFindResource(iconKey, out var res);
            Icon = (StreamGeometry)res!;
        }
    }
}
