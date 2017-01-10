using System;
using System.Linq;
using APR.DZ1.Extensions;
using APR.DZ4.Extensions;

namespace APR.DZ4
{
    public class FloatingPointChromosome : AbstractChromosome<double>
    {
        private IFloatingPointProblem _problem;

        public FloatingPointChromosome(IFloatingPointProblem problem) : base(problem)
        {
            _problem = problem;

            // Initialize randomly
            for (int i = 0; i < Dimension; i++)
            {
                double lowerBound = _problem.GetLowerBound(i);
                double upperBound = _problem.GetUpperBound(i);

                SetValue(i, RandomNumberGeneratorProvider.Instance.NextDouble(lowerBound, upperBound));
            }
        }

        public FloatingPointChromosome(FloatingPointChromosome chromosome) : base(chromosome)
        {
            _problem = chromosome._problem;
        }

        public override IChromosome Copy()
        {
            return new FloatingPointChromosome(this);
        }

        public override string GetValueString(int index)
        {
            return GetValue(index).ToString("F6");
        }

        public double GetLowerBound(int index)
        {
            return _problem.GetLowerBound(index);
        }

        public double GetUpperBound(int index)
        {
            return _problem.GetUpperBound(index);
        }

        public new IFloatingPointProblem Problem
        {
            get { return _problem; }
        }

        public override string ToString()
        {
            return string.Join("|", Enumerable.Range(0, Dimension).Select(index => GetValueString(index)));
        }
    }
}