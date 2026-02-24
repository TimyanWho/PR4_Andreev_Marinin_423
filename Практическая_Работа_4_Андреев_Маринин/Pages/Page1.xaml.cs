using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Практическая_Работа_4_Андреев_Маринин.Pages
{
    public partial class Page1 : Page
    {
        private const string ImgPath = @"C:\Users\Timyan\source\repos\Практическая_Работа_4_Андреев_Маринин\Практическая_Работа_4_Андреев_Маринин\f1.png";

        public Page1()
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
                    ImgTop1.Source = null;
                    return;
                }
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.UriSource = new Uri(ImgPath, UriKind.Absolute);
                bi.EndInit();
                ImgTop1.Source = bi;
            }
            catch
            {
                ImgTop1.Source = null;
            }
        }

        private double ComputeT(double x, double y, double z)
        {
            double numerator = 2.0 * Math.Cos(x - Math.PI / 6.0);
            double denom = 0.5 + Math.Pow(Math.Sin(y), 2);
            double inside = 1.0 + (z * z) / (3.0 - (z * z) / 5.0);
            return numerator / denom * inside;
        }

        private void BtnCalc_Click(object sender, RoutedEventArgs e)
        {
            if (!TryParse(TbX.Text, out double x) || !TryParse(TbY.Text, out double y) || !TryParse(TbZ.Text, out double z))
            {
                MessageBox.Show("Введите корректные числовые значения для x, y и z.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                double t = ComputeT(x, y, z);
                TbResult.Text = t.ToString("G6");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при вычислении: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TbX.Clear();
            TbY.Clear();
            TbZ.Clear();
            TbResult.Clear();
        }

        private bool TryParse(string s, out double v)
        {
            return double.TryParse((s ?? "").Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out v);
        }
    }
}