using System.ComponentModel.Composition;
using CalculatorContract;

namespace Addition
{
    [Export(typeof(ICalculator))]
    [ExportMetadata("CalciMetaData", "Add")]
    public class Add : ICalculator
    {
        #region Interface members
        public int GetNumber(int num1, int num2)
        {
            return num1 + num2;
        }
        #endregion
    }
}
