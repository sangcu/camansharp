namespace CanmanSharp.Core
{
    public abstract class JobFactory
    {
        public FilterTypes Type { get; set; }

        public string Src { get; set; }

        public double[,] Adjust { get; set; }

        public double Bias { get; set; }

        public string Name { get; set; }

        public double Divisor { get; set; }
    }
}