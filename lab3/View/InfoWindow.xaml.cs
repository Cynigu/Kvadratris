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
using System.Windows.Shapes;

namespace lab3.View
{
    /// <summary>
    /// Логика взаимодействия для InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {
        public InfoWindow()
        {
            
            InitializeComponent();
            if (Properties.Settings.Default.splash_screen)
            {
                checkBox.IsChecked = true;
            }
            else
            {
                checkBox.IsChecked = false;
            }
            textCountOpen.Text = "Количество открытий программы: " + Properties.Settings.Default.count_open.ToString();
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.splash_screen = true;
            Properties.Settings.Default.Save();
        }
        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.splash_screen = false;
            Properties.Settings.Default.Save();
        }
    }
}
