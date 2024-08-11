using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using Carbon.Core.Plugins;
using System.Collections.ObjectModel;

namespace TestLib
{
    public class ClockPlugin : IPlugin
    {
        private ClockPluginControl _control;

        public void Load()
        {
            Console.WriteLine("Clock plugin loaded");
        }

        public UserControl GetControl()
        {
            _control = new ClockPluginControl() { Width = 300 };
            return _control;
        }
    }

    public class ClockPluginControl : UserControl
    {
        private readonly TextBlock _timeTextBlock;
        private readonly DispatcherTimer _timer;

        public ClockPluginControl()
        {
            _timeTextBlock = new TextBlock
            {
                FontSize = 24,
                FontWeight = Avalonia.Media.FontWeight.Bold,
                Foreground = new SolidColorBrush(Colors.Black),
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
            };

            var border = new Border
            {
                Background = new SolidColorBrush(Colors.LightGray),
                BorderBrush = new SolidColorBrush(Colors.Gray),
                BorderThickness = new Thickness(1),
                Padding = new Thickness(10),
                Child = _timeTextBlock
            };

            Content = border;

            // Initialize timer
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (sender, e) => _timeTextBlock.Text = DateTime.Now.ToString("HH:mm:ss");
            _timer.Start();
        }
    }

    public class AsyncPlugin : IPlugin
    {
        private AsyncPluginControl _control;
        private readonly HttpClient _httpClient = new HttpClient();
        private Task _loadingTask;

        public void Load()
        {
            Console.WriteLine("Async plugin loading...");

            // Start the data loading asynchronously
            _loadingTask = LoadDataAsync();

            Console.WriteLine("Async plugin loading initiated.");
        }

        private async Task LoadDataAsync()
        {
            try
            {
                // Simulate async data loading
                var data = await _httpClient.GetStringAsync("https://example.com/");

                // Update the control once data is loaded
                if (_control != null)
                {
                    _control.UpdateContent(data);
                }
            }
            catch (Exception ex)
            {
                if (_control != null)
                {
                    _control.UpdateContent($"Error: {ex.Message}");
                }
            }
        }

        public UserControl GetControl()
        {
            _control = new AsyncPluginControl();
            return _control;
        }
    }

    public class AsyncPluginControl : UserControl
    {
        private TextBlock _textBlock;

        public AsyncPluginControl()
        {
            _textBlock = new TextBlock
            {
                Text = "Loading..."
            };

            Content = _textBlock;
        }

        public void UpdateContent(string data)
        {
            _textBlock.Text = data;
        }
    }
    public class RandomQuotePlugin : IPlugin
    {
        public void Load()
        {
            Console.WriteLine("Random Quote plugin loaded");
        }

        public UserControl GetControl()
        {
            return new RandomQuotePluginControl();
        }
    }

    public class RandomQuotePluginControl : UserControl
    {
        private readonly TextBlock _quoteTextBlock;

        public RandomQuotePluginControl()
        {
            var quotes = new[]
            {
            "The best way to predict the future is to invent it.",
            "Success is not final, failure is not fatal: It is the courage to continue that counts.",
            "Act as if what you do makes a difference. It does."
        };

            var random = new Random();
            var quote = quotes[random.Next(quotes.Length)];

            _quoteTextBlock = new TextBlock
            {
                Text = quote,
                FontSize = 18,
                FontWeight = FontWeight.Bold,
                Foreground = new SolidColorBrush(Colors.DarkBlue),
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                TextWrapping = TextWrapping.Wrap,  // Ensure text wraps within the container
                MaxWidth = 300  // Adjust to fit your desired maximum width
            };

            var border = new Border
            {
                Background = new SolidColorBrush(Colors.White),
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(1),
                Padding = new Thickness(10),
                Child = _quoteTextBlock,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,  // Center the border horizontally
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center  // Center the border vertically
            };

            Content = border;
        }
    }
    public class TodoListPlugin : IPlugin
    {
        public void Load()
        {
            Console.WriteLine("Todo List plugin loaded");
        }

        public UserControl GetControl()
        {
            return new TodoListPluginControl();
        }
    }

    public class TodoListPluginControl : UserControl
    {
        private readonly ListBox _listBox;
        private readonly TextBox _inputBox;

        public TodoListPluginControl()
        {
            var stackPanel = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Vertical,
                Margin = new Thickness(10)
            };

            var title = new TextBlock
            {
                Text = "Todo List",
                FontSize = 22,
                FontWeight = FontWeight.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            };

            var todoItems = new ObservableCollection<string>
            {
                "Buy groceries",
                "Walk the dog",
                "Read a book"
            };

            _listBox = new ListBox
            {
                Margin = new Thickness(0, 0, 0, 10),
                ItemsSource = todoItems
            };

            _inputBox = new TextBox
            {
                Watermark = "Enter new todo item...",
                Margin = new Thickness(0, 0, 0, 10)
            };

            var addButton = new Button
            {
                Content = "Add",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right
            };

            addButton.Click += (sender, args) =>
            {
                if (!string.IsNullOrWhiteSpace(_inputBox.Text))
                {
                    todoItems.Add(_inputBox.Text);
                    _inputBox.Clear();
                }
            };

            stackPanel.Children.Add(title);
            stackPanel.Children.Add(_listBox);
            stackPanel.Children.Add(_inputBox);
            stackPanel.Children.Add(addButton);

            Content = stackPanel;
        }
    }
}
