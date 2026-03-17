using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Практическая_Работа_4_Андреев_Маринин.Pages
{
    public partial class Page2 : Page
    {
        private const string ImgPath = "../../f2.png";

        public Page2()
        {
            InitializeComponent();
            LoadTopImage();
        }

        private void LoadTopImage()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ImgPath) || !File.Exists(ImgPath))
                {
                    ImgTop2.Source = null;
                    return;
                }

                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.UriSource = new Uri(ImgPath, UriKind.RelativeOrAbsolute);
                bi.EndInit();
                ImgTop2.Source = bi;
            }
            catch
            {
                ImgTop2.Source = null;
            }
        }

        private void BtnCalc_Click(object sender, RoutedEventArgs e)
        {
            if (!TryParse(TbX.Text, out double x) || !TryParse(TbY.Text, out double y))
            {
                MessageBox.Show("Введите корректные числовые значения x и y.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int functionType = RbSinh.IsChecked == true ? 0 : (RbSquare.IsChecked == true ? 1 : 2);

            if (Calculations.TryComputeA(x, y, functionType, out double aValue))
            {
                TbResult.Text = aValue.ToString("G8");
            }
            else
            {
                TbResult.Text = "invalid";
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TbX.Clear();
            TbY.Clear();
            TbResult.Clear();
            RbSinh.IsChecked = true;
        }

        private bool TryParse(string s, out double v)
        {
            return double.TryParse((s ?? "").Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out v);
        }
    }
}