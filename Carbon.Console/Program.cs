// See https://aka.ms/new-console-template for more information
using Carbon.Core.Plugins;

string path = @"C:\Users\Thiago\source\repos\Carbon\Carbon.Console\bin\Debug\net8.0";

PluginManager pluginManager = new PluginManager();
pluginManager.LoadPlugins(path); // Directory where plugin DLLs are stored
pluginManager.InitializePlugins();

Console.WriteLine("Plugins have been loaded.");