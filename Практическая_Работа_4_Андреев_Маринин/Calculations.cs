using System;

namespace Практическая_Работа_4_Андреев_Маринин
{
    /// <summary>
    /// Набор вычислительных методов, независимых от UI.
    /// Методы имеют сигнатуру TryCompute... (bool + out double),
    /// чтобы unit-тесты могли проверять успешность вычисления.
    /// </summary>
    public static class Calculations
    {
        /// <summary>
        /// Вычисляет t = (2 * cos(x - π/6)) / (0.5 + sin^2(y)) * (1 + z^2 / (3 - z^2/5))
        /// Возвращает true если получилось корректное значение (не NaN/Infinity), иначе false.
        /// </summary>
        public static bool TryComputeT(double x, double y, double z, out double t)
        {
            t = double.NaN;
            try
            {
                double numerator = 2.0 * Math.Cos(x - Math.PI / 6.0);
                double denom = 0.5 + Math.Pow(Math.Sin(y), 2);
                double denomInner = 3.0 - (z * z) / 5.0;

                // защита от деления на ноль в внутреннем выражении
                if (Math.Abs(denomInner) < 1e-15)
                    return false;

                double inside = 1.0 + (z * z) / denomInner;
                t = numerator / denom * inside;

                if (double.IsNaN(t) || double.IsInfinity(t)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Вычисляет 'a' по формуле из задания (вариант 1)
        /// a = { (f+y)^2 - sqrt(f*y) , if xy>0
        ///       (f+y)^2 + sqrt(|f*y|) , if xy<0
        ///       (f+y)^2 + 1            , if xy==0 }
        /// functionType: 0 = sinh(x), 1 = x^2, 2 = exp(x)
        /// Возвращает true если вычисление успешно (и не NaN/Infinity); false при sqrt отрицательного аргумента в ветке xy>0, или других ошибках.
        /// </summary>
        public static bool TryComputeA(double x, double y, int functionType, out double a)
        {
            a = double.NaN;
            try
            {
                double f;
                switch (functionType)
                {
                    case 0: f = Math.Sinh(x); break;
                    case 1: f = x * x; break;
                    case 2: f = Math.Exp(x); break;
                    default: return false;
                }

                double prod = x * y;
                if (prod > 0)
                {
                    double arg = f * y;
                    if (arg < 0) return false; // sqrt отрицательного в этой ветке -> ошибка по заданию
                    a = Math.Pow(f + y, 2) - Math.Sqrt(arg);
                }
                else if (prod < 0)
                {
                    double arg = f * y;
                    a = Math.Pow(f + y, 2) + Math.Sqrt(Math.Abs(arg));
                }
                else // prod == 0
                {
                    a = Math.Pow(f + y, 2) + 1.0;
                }

                if (double.IsNaN(a) || double.IsInfinity(a)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Вычисляет y для третьей страницы:
        /// y = (10 - 2*b*c)/x + cos( sqrt(a^3 * x) )
        /// Возвращает false при делении на ноль (x==0) или при NaN/Infinity.
        /// </summary>
        public static bool TryComputeY(double x, double aParam, double b, double c, out double y)
        {
            y = double.NaN;
            try
            {
                if (Math.Abs(x) < 1e-15) return false; // защита от деления на ноль
                double part1 = (10.0 - 2.0 * b * c) / x;
                double inside = aParam * aParam * aParam * x; // a^3 * x
                double part2;
                if (inside >= 0) part2 = Math.Cos(Math.Sqrt(inside));
                else part2 = Math.Cos(Math.Sqrt(Math.Abs(inside)));
                y = part1 + part2;
                if (double.IsNaN(y) || double.IsInfinity(y)) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}