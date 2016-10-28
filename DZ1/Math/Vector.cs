using System;
using System.Text;
using APR.DZ1.Extensions;

namespace APR.DZ1
{
    public class Vector
    {
        public static readonly int PRECISION = 6;
        public static readonly double EPSILON = Math.Pow(10, -PRECISION);

        private double[] _values;
        private bool _isTransposed;

        public Vector (int dimension)
        {
            if(dimension <= 0)
            {
                throw new ArgumentException("Negative or zero dimension.");
            }
        }

        public Vector(params double[] values)
        {
            if(values == null)
            {
                throw new ArgumentNullException("values is null");
            }

            _values = values.Copy();
        }

        public int Dimension { get { return _values.Length; } }

        public double Norm()
        {
            return _values.Norm();
        }

        public void Transpose()
        {
            _isTransposed = !_isTransposed;
        }

        public double this[int index]
        {
            get { return _values[index]; }
            set { _values[index] = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('[');
            for (int i = 0; i < _values.Length; i++)
            {
                sb.Append(String.Format("{0:F" + PRECISION + "}", _values[i]));
                if (i < _values.Length - 1)
                {
                    sb.Append(',');
                }
            }
            sb.Append(']');
            return sb.ToString();
        }
    }
}