using Avalonia.Controls;

namespace Carbon.Core.Plugins
{
    public interface IPlugin
    {
        void Load();
        UserControl GetControl();
    }
}
