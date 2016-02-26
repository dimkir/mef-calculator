using System.ComponentModel.Composition;
using CalculatorContract;

namespace Subtraction
{
    [Export(typeof(ICalculator))]
    [ExportMetadata("CalciMetaData", "Subtract")]
    public class Subtract : ICalculator
    {
        #region Interface members
        public int GetNumber(int num1, int num2)
        {
            return num1 - num2;
        }
        #endregion
    }
}
