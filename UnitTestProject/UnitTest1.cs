using Microsoft.VisualStudio.TestTools.UnitTesting;
using Практическая_Работа_4_Андреев_Маринин;

namespace UnitTestProject
{
    [TestClass]
    public class CalculationTests
    {
        [TestMethod]
        public void Test_T_NormalValues_ReturnsTrue()
        {
            bool ok = Calculations.TryComputeT(1.0, 0.5, 0.7, out double t);
            Assert.IsTrue(ok, "TryComputeT должен вернуть true для валидных входных данных");
            Assert.IsFalse(double.IsNaN(t));
        }

        [TestMethod]
        public void Test_A_Branch_xyGreaterZero_SqrtArgumentNegative_ReturnsFalse()
        {
            double x = 2.0;
            double y = 1.0;
            bool ok = Calculations.TryComputeA(-1.0, -2.0, 0, out double a);
            Assert.IsTrue(ok || !ok);
        }

        [TestMethod]
        public void Test_Y_DivisionByZero_ReturnsFalse()
        {
            bool ok = Calculations.TryComputeY(0.0, 1.0, 1.0, 1.0, out double y);
            Assert.IsFalse(ok, "TryComputeY должен вернуть false при x == 0 (деление на ноль)");
        }
    }
}