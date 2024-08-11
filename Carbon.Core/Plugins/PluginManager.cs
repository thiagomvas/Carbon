using System.Reflection;

namespace Carbon.Core.Plugins
{
    public class PluginManager
    {
        public List<IPlugin> Plugins = new();
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
                        Plugins.Add(plugin);
                    }
                }
            }
        }

        public void InitializePlugins()
        {
            foreach (IPlugin plugin in Plugins)
            {
                plugin.Load();
            }
        }
    }
}
