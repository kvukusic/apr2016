using System;
using System.Collections.Generic;
using System.Linq;
using APR.DZ2.Functions;

namespace APR.DZ4
{
    public class FunctionMinimizationBinaryProblem : IBinaryProblem
    {
        protected class BinaryFitnessComparer : IComparer<BinaryChromosome>
        {
            public int Compare(BinaryChromosome x, BinaryChromosome y)
            {
                if (x.Fitness == y.Fitness) return 0;
                if (x.Fitness < y.Fitness) return 1;
                return -1;
            }
        }

        private Function _function;
        private int _dimension;
        private int _precision;
        private double[] _lowerBounds;
        private double[] _upperBounds;
        private int[] _geneLengths;

        public FunctionMinimizationBinaryProblem(Function function, int dimension, int precision)
            : this(function, dimension, precision, Double.MinValue, Double.MaxValue)
        {
        }

        public FunctionMinimizationBinaryProblem(
            Function function,
            int dimension,
            int precision,
            double uniformLowerBound,
            double uniformUpperBound)
        {
            _function = function;
            _dimension = dimension;
            _precision = precision;

            _lowerBounds = Enumerable.Repeat(uniformLowerBound, dimension).ToArray();
            _upperBounds = Enumerable.Repeat(uniformUpperBound, dimension).ToArray();

            _geneLengths = DetermineNumberOfBits();

            if (_geneLengths.Any(length => length > 64))
            {
                throw new ArgumentException("Too large number of bits in a gene. Try lowering the"
                    + " precision or the search interval.");
            }
        }

        private int[] DetermineNumberOfBits()
        {
            int[] lengths = new int[_dimension];
            for (int i = 0; i < _dimension; i++)
            {
                var min = _lowerBounds[i];
                var max = _upperBounds[i];

                // Calculate needed number of bits
                var n =
                    (int)Math.Ceiling(Math.Log10(Math.Floor(1 + (max - min) * Math.Pow(10, _precision))) / Math.Log10(2));

                lengths[i] = n;
            }

            return lengths;
        }

        public int Dimension
        {
            get { return _dimension; }
        }

        public IComparer<BinaryChromosome> FitnessComparer
        {
            get { return new BinaryFitnessComparer(); }
        }

        public int FitnessFunctionEvaluations
        {
            get { return _function.Evaluations; }
        }

        public void Evaluate(BinaryChromosome chromosome)
        {
            var binaryValues = chromosome.GetValues().Select(gene => Convert.ToInt64(gene.ToBinaryString(), 2)).ToArray();
            var floatValues =
                binaryValues.Select(
                    (i, index) =>
                        _lowerBounds[index] +
                        i * (_upperBounds[index] - _lowerBounds[index]) / (Math.Pow(2, chromosome.GetValue(index).Length) - 1))
                    .ToArray();

            chromosome.Fitness = _function.Value(floatValues);
        }

        public int GetNumberOfBits(int index)
        {
            return _geneLengths[index];
        }

        public BinaryChromosome CreateChromosome()
        {
            return new BinaryChromosome(this);
        }
    }
}