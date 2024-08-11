# Carbon Dashboard
Carbon is a highly customizable personal dashboard where you can create your own plugins for it using Avalonia User Controls.

## Creating custom plugins
To create a custom plugin, follow these steps:

1- Clone the Repository 
2- Add a new Class Lib project, this will be where your custom plugins will be
3- Add the  ``Avalonia``  package
4- Copy the following "Template" into your new plugin

```cs

    public class MyPlugin : IPlugin
    {
        public void Load()
        {
            Console.WriteLine("MyPlugin loaded");
        }

        public UserControl GetControl()
        {
            return new MyPluginControl();
        }
    }

    public class RandomQuotePluginControl : UserControl
    {
        private readonly TextBlock _textBlock;

        public RandomQuotePluginControl()
        {
            // This is an example, for your plugin control, add styling and more children via code


            _quoteTextBlock = new TextBlock
            {
                Text = quote,
                FontSize = 18,
                FontWeight = FontWeight.Bold,
                Foreground = new SolidColorBrush(Colors.DarkBlue),
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            };

            Content = _textBlock;
        }
    }
```
Modify the template however you want.
5- Build the class library. Include any dependencies' DLLs as well.
6- Copy the DLLs into the Plugin folder for your dashboard.
7- Run the dashboard and enable your new plugin

