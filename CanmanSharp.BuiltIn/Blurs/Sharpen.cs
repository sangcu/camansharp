using CanmanSharp.Core;

namespace CanmanSharp.BuiltIn.Blurs
{
    public class Sharpen : FilterBase
    {
        public Sharpen(double Amt)
        {
            Amt = Amt/100;

            Type = FilterTypes.Kernel;
            Adjust = new double[3, 3]; // { 0, -Amt, 0, -Amt, 4 * Amt + 1, -Amt, 0, -Amt, 0 };
            Adjust[0, 0] = 0;
            Adjust[0, 1] = -Amt;
            Adjust[0, 2] = 0;
            Adjust[1, 0] = -Amt;
            Adjust[1, 1] = 4*Amt + 1;
            Adjust[1, 2] = -Amt;
            Adjust[2, 0] = 0;
            Adjust[2, 1] = -Amt;
            Adjust[2, 2] = 0;
            Bias = 0;
        }
    }
}