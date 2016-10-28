using APR.DZ2.Functions;

namespace APR.DZ2
{
    public interface IMinimizer
    {
        double[] Minimize(Function f, double[] start);
        bool IsOutputEnabled { get; set; }
    }
}
