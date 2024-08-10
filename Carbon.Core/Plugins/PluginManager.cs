using System.Reflection;

namespace Carbon.Core.Plugins
{
    public class PluginManager
    {
        private List<IPlugin> plugins = new();
        public void LoadPlugins(string path)
        {
            if(!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"The path {path} does not exist.");
            }

            foreach (string file in Directory.GetFiles(path, "*.dll"))
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

        public void InitializePlugins()
        {
            foreach (IPlugin plugin in plugins)
            {
                plugin.Load();
            }
        }
    }
}
