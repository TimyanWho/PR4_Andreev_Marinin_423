using System;
using System.Windows;
using System.Windows.Controls;

namespace Практическая_Работа_4_Андреев_Маринин.Pages
{
    public partial class Page2 : Page
    {
        public Page2() { InitializeComponent(); }

        private void BtnCalc_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(TbX.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double x))
            {
                MessageBox.Show("Введите число для x.");
                return;
            }

            double result;
            if (RbSinh.IsChecked == true) result = Math.Sinh(x);
            else if (RbSquare.IsChecked == true) result = x * x;
            else result = Math.Exp(x);

            TbResult.Text = result.ToString("G6");
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TbX.Clear(); TbResult.Clear(); RbSinh.IsChecked = true;
        }
    }
}
