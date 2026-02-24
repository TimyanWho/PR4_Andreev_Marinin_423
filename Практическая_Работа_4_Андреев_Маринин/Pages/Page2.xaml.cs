using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Практическая_Работа_4_Андреев_Маринин.Pages
    {
        public partial class Page2 : Page
        {
            private const string ImgPath = @"C:\Users\Timyan\source\repos\Практическая_Работа_4_Андреев_Маринин\Практическая_Работа_4_Андреев_Маринин\f2.png";

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
                    bi.UriSource = new Uri(ImgPath, UriKind.Absolute);
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

                // вычисляем f(x) в зависимости от выбранного варианта
                double f;
                try
                {
                    if (RbSinh.IsChecked == true) f = Math.Sinh(x);
                    else if (RbSquare.IsChecked == true) f = x * x;
                    else f = Math.Exp(x);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при вычислении f(x): " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                double prodXY = x * y;
                double aValue;
                try
                {
                    if (prodXY > 0)
                    {
                        double arg = f * y;
                        if (arg < 0)
                        {
                            // корень отрицательного числа недопустим по формуле — сообщаем и не падаем
                            TbResult.Text = "undefined (sqrt<0)";
                            return;
                        }
                        aValue = Math.Pow(f + y, 2) - Math.Sqrt(arg);
                    }
                    else if (prodXY < 0)
                    {
                        double arg = f * y;
                        aValue = Math.Pow(f + y, 2) + Math.Sqrt(Math.Abs(arg));
                    }
                    else // prodXY == 0
                    {
                        aValue = Math.Pow(f + y, 2) + 1.0;
                    }

                    if (double.IsNaN(aValue) || double.IsInfinity(aValue))
                    {
                        TbResult.Text = "invalid";
                        return;
                    }

                    TbResult.Text = aValue.ToString("G8");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при вычислении a: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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