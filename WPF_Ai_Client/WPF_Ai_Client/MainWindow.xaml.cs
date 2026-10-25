using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Media.Animation;

namespace WPF_Ai_Client
{
    public partial class MainWindow : Window
    {
        private bool isPanelOpen = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                To = isPanelOpen ? -200 : 0,
                Duration = TimeSpan.FromMilliseconds(200),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };

            PanelTransform.BeginAnimation(System.Windows.Media.TranslateTransform.XProperty, animation);

            isPanelOpen = !isPanelOpen;
            ToggleButton.Content = isPanelOpen ? "->" : "<-";
        }

        private async void AnalyzeButton_Click(object sender, RoutedEventArgs e)
        {
            using HttpClient client = new HttpClient();
            var data = new { message = NoteInput.Text, prompt = "analyse" };
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            try
            {
                // Отправляем запрос асинхронно, чтобы окно не зависало
                var response = await client.PostAsync("http://localhost:5137/api/Ai/analyze", content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    new ResultWindow(result).ShowDialog();
                }
                else
                {
                    MessageBox.Show($"Ошибка сервера: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}