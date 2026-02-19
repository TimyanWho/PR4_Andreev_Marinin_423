using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Media.Imaging;

namespace Практическая_Работа_4_Андреев_Маринин.Pages
{
    public partial class Page2 : Page
    {
        private Chart _chart;

        private const string ImgPath = @"C:\Users\Timyan\source\repos\Практическая_Работа_4_Андреев_Маринин\Практическая_Работа_4_Андреев_Маринин\f2.png";

        public Page2()
        {
            InitializeComponent();
            InitializeChart();
            LoadTopImage();
            this.Loaded += (s, e) => UpdateFormulaText();
        }

        private void InitializeChart()
        {
            _chart = new Chart();
            _chart.Dock = System.Windows.Forms.DockStyle.Fill;
            _chart.ChartAreas.Add(new ChartArea("Main"));
            var s = new Series("f")
            {
                ChartType = SeriesChartType.Line,
                IsValueShownAsLabel = false
            };
            _chart.Series.Add(s);
            WfhChart2.Child = _chart;
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

        private void CmbFunc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateFormulaText();
        }

        private void UpdateFormulaText()
        {
            if (TbFormula == null || CmbFunc == null) return;
            var it = CmbFunc.SelectedItem as ComboBoxItem;
            if (it == null) { TbFormula.Text = "Формула: —"; return; }
            var tag = it.Tag as string ?? "";
            switch (tag)
            {
                case "sinh": TbFormula.Text = "Формула: sh(x) = sinh(x)"; break;
                case "square": TbFormula.Text = "Формула: x^2"; break;
                case "exp": TbFormula.Text = "Формула: e^x"; break;
                default: TbFormula.Text = "Формула: —"; break;
            }
        }

        private void BtnPlot_Click(object sender, RoutedEventArgs e)
        {
            if (!TryParse(TbFrom.Text, out double from) || !TryParse(TbTo.Text, out double to) || !TryParse(TbStep.Text, out double step))
            {
                MessageBox.Show("Введите корректные числа для диапазона и шага.");
                return;
            }
            if (Math.Abs(step) < 1e-12) { MessageBox.Show("Шаг не может быть ноль."); return; }

            string tag = ((ComboBoxItem)CmbFunc.SelectedItem).Tag as string;

            _chart.Series["f"].Points.Clear();
            TbList2.Clear();

            bool asc = step > 0;
            if (asc && from > to) { MessageBox.Show("xFrom должен быть <= xTo при положительном шаге."); return; }
            if (!asc && from < to) { MessageBox.Show("xFrom должен быть >= xTo при отрицательном шаге."); return; }

            for (double x = from; asc ? x <= to + 1e-9 : x >= to - 1e-9; x += step)
            {
                double y = 0;
                try
                {
                    switch (tag)
                    {
                        case "sinh": y = Math.Sinh(x); break;
                        case "square": y = x * x; break;
                        case "exp": y = Math.Exp(x); break;
                    }
                }
                catch { y = double.NaN; }

                if (double.IsNaN(y) || double.IsInfinity(y)) continue;

                _chart.Series["f"].Points.AddXY(x, y);
                TbList2.AppendText($"x={x:G6}   y={y:G6}\r\n");
            }

            _chart.ChartAreas["Main"].RecalculateAxesScale();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TbList2.Clear();
            _chart.Series["f"].Points.Clear();
        }

        private bool TryParse(string s, out double v)
        {
            return double.TryParse((s ?? "").Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out v);
        }
    }
}
