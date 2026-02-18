using System.Windows;
using System.ComponentModel;

namespace Практическая_Работа_4_Андреев_Маринин
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Pages.Page1()); // стартовая страница
        }

        private void BtnPage1_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new Pages.Page1());
        private void BtnPage2_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new Pages.Page2());
        private void BtnPage3_Click(object sender, RoutedEventArgs e) => MainFrame.Navigate(new Pages.Page3());

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var res = MessageBox.Show("Выйти из приложения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res != MessageBoxResult.Yes) e.Cancel = true;
        }


    }
}
