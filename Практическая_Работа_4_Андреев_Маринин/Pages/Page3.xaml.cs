using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Forms.DataVisualization.Charting; // add reference
using System.Linq;

namespace Практическая_Работа_4_Андреев_Маринин.Pages
{
    public partial class Page3 : Page
    {
        private Chart _chart;

        public Page3()
        {
            InitializeComponent();
            InitializeChart();
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

        private void BtnCalc_Click(object sender, RoutedEventArgs e)
        {
            if (!TryParseAll(out double x0, out double xk, out double dx, out double a, out double b, out double c)) return;
            if (dx == 0) { MessageBox.Show("dx не может быть 0"); return; }

            _chart.Series["y"].Points.Clear();
            TbList.Clear();

            // y = (10 - 2*b*c) / x + cos( sqrt(a^3 * x) )
            for (double x = x0; (dx > 0) ? x <= xk + 1e-9 : x >= xk - 1e-9; x += dx)
            {
                double denomX = x;
                double part1 = (10.0 - 2.0 * b * c) / denomX;
                double insideSqrt = a * a * a * x; // a^3 * x
                double part2 = double.NaN;
                if (insideSqrt >= 0) part2 = Math.Cos(Math.Sqrt(insideSqrt));
                else part2 = Math.Cos(Math.Sqrt(Math.Abs(insideSqrt))); // если отрицательно — берем abs
                double y = part1 + part2;

                _chart.Series["y"].Points.AddXY(x, y);
                TbList.AppendText($"x={x:G6}   y={y:G6}\r\n");
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

            if (!ok) MessageBox.Show("Проверьте все поля — требуется число (используйте точку или запятую).");
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
