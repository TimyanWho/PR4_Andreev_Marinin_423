using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Media.Imaging;

namespace Практическая_Работа_4_Андреев_Маринин.Pages
{
    public partial class Page3 : Page
    {
        private Chart _chart;

        private const string ImgPath = @"C:\Users\Timyan\source\repos\Практическая_Работа_4_Андреев_Маринин\Практическая_Работа_4_Андреев_Маринин\f3.png";

        public Page3()
        {
            InitializeComponent();
            InitializeChart();
            LoadTopImage();
        }

        private void InitializeChart()
        {
            _chart = new Chart();
            _chart.Dock = System.Windows.Forms.DockStyle.Fill;
            _chart.ChartAreas.Add(new ChartArea("Main"));
            var s = new Series("y")
            {
                ChartType = SeriesChartType.Line,
                IsValueShownAsLabel = false
            };
            _chart.Series.Add(s);
            WfhChart.Child = _chart;
        }

        private void LoadTopImage()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ImgPath) || !File.Exists(ImgPath))
                {
                    ImgTop3.Source = null;
                    return;
                }
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.UriSource = new Uri(ImgPath, UriKind.Absolute);
                bi.EndInit();
                ImgTop3.Source = bi;
            }
            catch
            {
                ImgTop3.Source = null;
            }
        }

        private void BtnCalc_Click(object sender, RoutedEventArgs e)
        {
            if (!TryParseAll(out double x0, out double xk, out double dx, out double a, out double b, out double c)) return;
            if (Math.Abs(dx) < 1e-15) { MessageBox.Show("dx не может быть 0", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

            _chart.Series["y"].Points.Clear();
            TbList.Clear();

            for (double x = x0; (dx > 0) ? x <= xk + 1e-12 : x >= xk - 1e-12; x += dx)
            {
                if (Math.Abs(x) < 1e-15)
                {
                    TbList.AppendText($"x={x:G6}   y=undefined (деление на 0)\r\n");
                    continue;
                }

                double part1 = (10.0 - 2.0 * b * c) / x;
                double insideSqrt = a * a * a * x; // a^3 * x
                double part2;

                if (insideSqrt >= 0) part2 = Math.Cos(Math.Sqrt(insideSqrt));
                else part2 = Math.Cos(Math.Sqrt(Math.Abs(insideSqrt)));

                double yVal = part1 + part2;

                if (double.IsNaN(yVal) || double.IsInfinity(yVal) || Math.Abs(yVal) > 1e150)
                {
                    TbList.AppendText($"x={x:G6}   y=invalid ({yVal})\r\n");
                    continue;
                }

                _chart.Series["y"].Points.AddXY(x, yVal);
                TbList.AppendText($"x={x:G6}   y={yVal:G6}\r\n");
            }

            try
            {
                _chart.ChartAreas["Main"].RecalculateAxesScale();
            }
            catch
            {

            }
        }

        private bool TryParseAll(out double x0, out double xk, out double dx, out double a, out double b, out double c)
        {
            x0 = xk = dx = a = b = c = 0;
            bool ok = double.TryParse(TbX0.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out x0)
                   && double.TryParse(TbXk.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out xk)
                   && double.TryParse(TbDx.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out dx)
                   && double.TryParse(TbA.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out a)
                   && double.TryParse(TbB.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out b)
                   && double.TryParse(TbC.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out c);

            if (!ok) MessageBox.Show("Проверьте все поля — требуется число.");
            return ok;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TbX0.Clear(); TbXk.Clear(); TbDx.Clear();
            TbA.Clear(); TbB.Clear(); TbC.Clear();
            TbList.Clear();
            _chart.Series["y"].Points.Clear();
        }
    }
}
