using System.Windows;
using System.Text.Json;

namespace WPF_Ai_Client
{
    public partial class ResultWindow : Window
    {
        public ResultWindow(string text)
        {
            InitializeComponent();
            ResultTextBlock.Text = FormatText(text);
        }

        private string FormatText(string text)
        {
            try
            {
                text = text.Replace("```json", "").Replace("```", "").Trim();
                int start = text.IndexOf('{');
                int end = text.LastIndexOf('}');
                if (start >= 0 && end > start)
                {
                    using JsonDocument doc = JsonDocument.Parse(text.Substring(start, end - start + 1));
                    var root = doc.RootElement;
                    return $"Краткий вывод:\n{root.GetProperty("pt1").GetString()}\n\n" +
                           $"Название заметки:\n{root.GetProperty("pt2").GetString()}\n\n" +
                           $"Советы:\n{root.GetProperty("pt3").GetString()}\n\n" +
                           $"Предупреждения:\n{root.GetProperty("pt4").GetString()}";
                }
            }
            catch { }
            return text;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
    }
}