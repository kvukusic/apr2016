using System;

namespace APR.DZ4
{
    public abstract class Chromosome : IComparable<Chromosome>
    {
        protected Chromosome(int dimension)
        {
            Dimension = dimension;
        }

        protected int Dimension { get; }

        public double Fitness { get; set; } = double.PositiveInfinity;

        public abstract Chromosome Copy();

        public static bool operator <(Chromosome lhs, Chromosome rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }

        public static bool operator >(Chromosome lhs, Chromosome rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }

        public static bool operator <=(Chromosome lhs, Chromosome rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }

        public static bool operator >=(Chromosome lhs, Chromosome rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }

        public int CompareTo(Chromosome other)
        {
            if (other == null) return -1;
            if (Fitness < other.Fitness) return -1;
            else if (Fitness > other.Fitness) return 1;
            else return 0;
            //return Fitness.CompareTo(other.Fitness);
        }
    }

    public class BinaryChromosome : Chromosome
    {
        private readonly int[] _sizeByDimension;
        private char[][] _genomes;

        public BinaryChromosome(int dimension, int[] sizeByDimension) : base(dimension)
        {
            _sizeByDimension = sizeByDimension;
            _genomes = new char[dimension][];
            for (int i = 0; i < dimension; i++)
            {
                _genomes[i] = new char[sizeByDimension[i]];
            }
        }

        public int Size(int dimension)
        {
            return _sizeByDimension[dimension];
        }
        public char[][] GetGenomes()
        {
            var retval = new char[_genomes.Length][];
            for (int i = 0; i < _genomes.Length; i++)
            {
                retval[i] = _genomes[i].Copy();
            }

            return retval;
        }

        public char[] GetGenomes(int dimension)
        {
            return _genomes[dimension].Copy();
        }

        public char GetGenome(int dimension, int index)
        {
            return _genomes[dimension][index];
        }

        public void SetGenome(int dimension, int index, char value)
        {
            if (value != '0' && value != '1')
            {
                throw new ArgumentException();
            }

            _genomes[dimension][index] = value;
        }

        public override string ToString()
        {
            return string.Join("|", _genomes.Select(c => new string(c)));
        }

        public override Chromosome Copy()
        {
            var retval = new BinaryChromosome(Dimension, _sizeByDimension.Copy());
            retval._genomes = _genomes.Copy();
            retval.Fitness = Fitness;
            return retval;
        }
    }

    public class FloatingPointChromosome : Chromosome
    {
        private double[] _values;

        public FloatingPointChromosome(int dimension) : base(dimension)
        {
            _values = new double[dimension];
        }

        public double[] GetValues()
        {
            return _values.Copy();
        }

        public double GetValue(int dimension)
        {
            return _values[dimension];
        }

        public void SetValue(int dimension, double value)
        {
            _values[dimension] = value;
        }

        public override string ToString()
        {
            return string.Join("|", _values.Select(d => d.ToString("F5")));
        }

        public override Chromosome Copy()
        {
            var retval = new FloatingPointChromosome(Dimension);
            retval._values = _values.Copy();
            retval.Fitness = Fitness;
            return retval;
        }
    }
}