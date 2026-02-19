using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Media.Imaging;

namespace Практическая_Работа_4_Андреев_Маринин.Pages
{
    public partial class Page1 : Page
    {
        private Chart _chart;


        private const string ImgPath = @"C:\Users\Timyan\source\repos\Практическая_Работа_4_Андреев_Маринин\Практическая_Работа_4_Андреев_Маринин\f1.png";

        public Page1()
        {
            InitializeComponent();
            InitializeChart();
            LoadTopImage(); // загрузка картинки из кода (пользователь менять не может)
        }

        private void InitializeChart()
        {
            _chart = new Chart();
            _chart.Dock = System.Windows.Forms.DockStyle.Fill;
            _chart.ChartAreas.Add(new ChartArea("Main"));
            var s = new Series("t")
            {
                ChartType = SeriesChartType.Line,
                IsValueShownAsLabel = false
            };
            _chart.Series.Add(s);
            WfhChart1.Child = _chart;
        }

        private void LoadTopImage()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ImgPath) || !File.Exists(ImgPath))
                {
                    // файл не найден — не показываем картинку, но приложение работает
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

        // --- остальной код как раньше (ComputeT, BtnCalc_Click, BtnPlot_Click, TryParse и т.д.) ---
        // Для краткости оставляю реализацию прежней, скопируйте сюда ваш рабочий код вычислений/построения.
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
                MessageBox.Show("Введите корректные числа для x, y и z.");
                return;
            }
            double t = ComputeT(x, y, z);
            TbResult.Text = t.ToString("G6");
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TbX.Clear(); TbY.Clear(); TbZ.Clear(); TbResult.Clear();
        }

        private void BtnPlot_Click(object sender, RoutedEventArgs e)
        {
            if (!TryParse(TbXFrom.Text, out double xFrom) || !TryParse(TbXTo.Text, out double xTo) || !TryParse(TbXStep.Text, out double step))
            {
                MessageBox.Show("Введите корректные значения диапазона для x.");
                return;
            }
            if (Math.Abs(step) < 1e-12) { MessageBox.Show("Шаг не может быть ноль."); return; }

            TryParse(TbY.Text, out double yVal);
            TryParse(TbZ.Text, out double zVal);

            _chart.Series["t"].Points.Clear();
            TbList1.Clear();

            bool ascending = step > 0;
            if (ascending && xFrom > xTo) { MessageBox.Show("xFrom должен быть <= xTo при положительном шаге."); return; }
            if (!ascending && xFrom < xTo) { MessageBox.Show("xFrom должен быть >= xTo при отрицательном шаге."); return; }

            for (double x = xFrom; ascending ? x <= xTo + 1e-9 : x >= xTo - 1e-9; x += step)
            {
                double t;
                try { t = ComputeT(x, yVal, zVal); }
                catch { t = double.NaN; }

                if (double.IsNaN(t) || double.IsInfinity(t)) continue;
                _chart.Series["t"].Points.AddXY(x, t);
                TbList1.AppendText($"x={x:G6}   t={t:G6}\r\n");
            }

            _chart.ChartAreas["Main"].RecalculateAxesScale();
        }

        private bool TryParse(string s, out double v)
        {
            return double.TryParse((s ?? "").Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out v);
        }
    }
}
