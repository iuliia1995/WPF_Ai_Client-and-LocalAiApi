using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace WPF_Ai_Client
{
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        private async void AnalyzeButton_Click(object sender, RoutedEventArgs e)
        {
            using HttpClient client = new HttpClient();
            var data = new { message = NoteInput.Text, prompt = "analyse" };
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("http://localhost:5137/api/Ai/analyze", content);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    using JsonDocument doc = JsonDocument.Parse(result);
                    string aiText = doc.RootElement.GetProperty("response").GetString();

                    new ResultWindow(aiText).ShowDialog();
                }
            }
            catch { }
        }
    }
}