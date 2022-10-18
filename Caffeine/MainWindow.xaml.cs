using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Caffeine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CaffeineManager _manager;

        public MainWindow(CaffeineManager manager)
        {
            _manager = manager;
            InitializeComponent();

            KeyCheckbox.IsChecked = manager.KeyboardEnabled;
            SilenceCheckbox.IsChecked = manager.SoundEnabled;
        }


        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SilenceCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            _manager.KeyboardEnabled = (bool)SilenceCheckbox.IsChecked;
        }

        private void KeyCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            _manager.KeyboardEnabled = (bool)KeyCheckbox.IsChecked;
        }
    }
}
