using System;
using System.Windows;
using System.Windows.Controls;

namespace Практическая_Работа_4_Андреев_Маринин.Pages
{
    public partial class Page1 : Page
    {
        public Page1() { InitializeComponent(); }

        private void BtnCalc_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetDouble(TbX.Text, out double x) ||
                !TryGetDouble(TbY.Text, out double y) ||
                !TryGetDouble(TbZ.Text, out double z))
            {
                MessageBox.Show("Проверьте ввод: требуется число.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // формула (интерпретация):
            // t = (2 * cos(x - π/6)) / (0.5 + sin^2(y)) * (1 + z^2 / (3 - z^2/5))
            double numerator = 2.0 * Math.Cos(x - Math.PI / 6.0);
            double denom = 0.5 + Math.Pow(Math.Sin(y), 2);
            double inside = 1.0 + (z * z) / (3.0 - (z * z) / 5.0);
            double t = numerator / denom * inside;

            TbResult.Text = t.ToString("G6");
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TbX.Clear();
            TbY.Clear();
            TbZ.Clear();
            TbResult.Clear();
        }

        private bool TryGetDouble(string s, out double value) => double.TryParse(s, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out value);
    }
}
