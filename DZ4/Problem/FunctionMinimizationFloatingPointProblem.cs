using System;
using System.Collections.Generic;
using System.Linq;
using APR.DZ2.Functions;

namespace APR.DZ4
{
    public class FunctionMinimizationFloatingPointProblem : IFloatingPointProblem
    {
        protected class FloatingPointFitnessComparer : IComparer<FloatingPointChromosome>
        {
            public int Compare(FloatingPointChromosome x, FloatingPointChromosome y)
            {
                if (x.Fitness == y.Fitness) return 0;
                if (x.Fitness < y.Fitness) return 1;
                return -1;
            }
        }

        private Function _function;
        private int _dimension;
        private double[] _lowerBounds;
        private double[] _upperBounds;

        public FunctionMinimizationFloatingPointProblem(Function function, int dimension) 
            : this (function, dimension, Double.MinValue, Double.MaxValue)
        {
        }

        public FunctionMinimizationFloatingPointProblem(
            Function function,
            int dimension,
            double uniformLowerBound,
            double uniformUpperBound)
        {
            _function = function;
            _dimension = dimension;
            
            _lowerBounds = Enumerable.Repeat(uniformLowerBound, dimension).ToArray();
            _upperBounds = Enumerable.Repeat(uniformUpperBound, dimension).ToArray();
        }

        public int Dimension 
        { 
            get { return _dimension; } 
        }

        public IComparer<FloatingPointChromosome> FitnessComparer
        {
            get { return new FloatingPointFitnessComparer(); }
        }

        public int FitnessFunctionEvaluations
        {
            get { return _function.Evaluations; }
        }

        public void Evaluate(FloatingPointChromosome chromosome)
        {
            double[] x = chromosome.GetValues();
            chromosome.Fitness = _function.Value(x);
        }

        public double GetLowerBound(int index)
        {
            return _lowerBounds[index];
        }

        public double GetUpperBound(int index)
        {
            return _upperBounds[index];
        }

        public FloatingPointChromosome CreateChromosome()
        {
            return new FloatingPointChromosome(this);
        }
    }
}