using System;
using APR.DZ1.Extensions;

namespace APR.DZ4
{
    public abstract class AbstractChromosome<T> : IChromosome<T>
    {
        private IProblem _problem;
        private double _fitness;
        private T[] _values;

        public AbstractChromosome(IProblem problem)
        {
            _problem = problem;
            _values = new T[_problem.Dimension];
        }

        public AbstractChromosome(AbstractChromosome<T> chromosome)
        {
            _problem = chromosome._problem;
            _fitness = chromosome._fitness;
            _values = chromosome._values.Copy<T>();
        }

        public int Dimension { get { return _problem.Dimension; } }
        
        public double Fitness
        {
            get { return _fitness; }
            set { _fitness = value; }
        }

        public abstract IChromosome Copy();

        public T GetValue(int index)
        {
            return _values[index];
        }

        public T[] GetValues()
        {
            return _values.Copy<T>();
        }

        public void SetValue(int index, T value)
        {
            _values[index] = value;
        }

        public abstract string GetValueString(int index);

        public IProblem Problem { get { return _problem; } }

        public T this[int index]
        {
            get { return _values[index]; }
            set { _values[index] = value; }
        }
    }
}