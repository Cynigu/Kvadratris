using System;
using System.Windows;
using System.Windows.Input;
using lab3.View;

namespace lab3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Properties.Settings.Default.count_open++;
            Properties.Settings.Default.Save();
            InfoWindow info = new InfoWindow();
            if (Properties.Settings.Default.splash_screen) {
                info.Show();
            }
            InitializeComponent();
            info.Close();
        }

        #region PreviewTextInput
        private void R_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == ".") || (e.Text == "-")
               && (!R.Text.Contains(".")
               && R.Text.Length != 0)))
            {
                e.Handled = true;
            }
        }
        private void Border1y_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == ".") || (e.Text == "-")
               && (!R.Text.Contains(".")
               && R.Text.Length != 0)))
            {
                e.Handled = true;
            }
        }
        private void Border2y_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == ".") || (e.Text == "-")
               && (!R.Text.Contains(".")
               && R.Text.Length != 0)))
            {
                e.Handled = true;
            }
        }
        private void Step_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == ".")
               && (!R.Text.Contains(".")
               && R.Text.Length != 0)))
            {
                e.Handled = true;
            }
        }
        #endregion
    }
    // Добавлен ReactiveUI, NLog, Autofac, LivaCharts...
}
