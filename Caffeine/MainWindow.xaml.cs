using System.Windows;

namespace Caffeine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool ShuttingDown { get; set; }
        public bool EnableKeys { get; set; }
        public bool EnableSound { get; set; }

        public MainWindow(CaffeineManager manager)
        {
            InitializeComponent();

            ShuttingDown = false;

            EnableKeys = manager.KeyboardEnabled;
            EnableSound = manager.SoundEnabled;
        }


        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            //_manager.Visible = false;
            Close();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SilenceCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            //_manager.KeyboardEnabled = (bool)SilenceCheckbox.IsChecked;
        }

        private void KeyCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            //_manager.KeyboardEnabled = (bool)KeyCheckbox.IsChecked;
        }
    }
}
