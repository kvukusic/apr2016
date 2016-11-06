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

        public Vector Copy()
        {
            var retval = new Vector(_values);
            return retval;
        }

        public double this[int index]
        {
            get { return _values[index]; }
            set { _values[index] = value; }
        }

#region Operators
    public static bool operator ==(Vector lhs, Vector rhs)
        {
            return Equals(lhs, rhs);
        }

        public static bool operator !=(Vector lhs, Vector rhs)
        {
            return !Equals(lhs, rhs);
        }

        public static Vector operator *(double lhs, Vector rhs)
        {
            Vector result = rhs.Copy();
            for (int i = 0; i < rhs.Dimension; i++)
            {
                result[i] *= lhs;
            }

            return result;
        }

        public static Vector operator *(Vector lhs, double rhs)
        {
            Vector result = lhs.Copy();
            for (int i = 0; i < lhs.Dimension; i++)
            {
                result[i] *= rhs;
            }

            return result;
        }

        public static Vector operator /(Vector lhs, double rhs)
        {
            Vector result = lhs.Copy();
            for (int i = 0; i < lhs.Dimension; i++)
            {
                result[i] /= rhs;
            }

            return result;
        }

        public static Vector operator +(Vector lhs, Vector rhs)
        {
            if (lhs.Dimension != rhs.Dimension)
            {
                throw new ArgumentException("Vertices are incomaptible");
            }

            Vector result = lhs.Copy();
            for (int i = 0; i < lhs.Dimension; i++)
            {
                result[i] += rhs[i];
            }

            return result;
        }

        public static Vector operator -(Vector lhs, Vector rhs)
        {
            if (lhs.Dimension != rhs.Dimension)
            {
                throw new ArgumentException("Vertices are incomaptible");
            }

            Vector result = lhs.Copy();
            for (int i = 0; i < result.Dimension; i++)
            {
                result[i] -= rhs[i];
            }

            return result;
        }
        
#endregion

#region Equals & HashCode
        protected bool Equals(Vector other)
        {
            if(_values.Length != other._values.Length) return false;

            for (int i = 0; i < _values.Length; i++)
            {
                if (Math.Abs(_values[i] - other._values[i]) > EPSILON)
                    {
                        return false;
                    }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vector)obj);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            for (int i = 0; i < _values.Length; i++)
            {
                hash = hash * 31 + (int)_values[i];
            }
            return hash;
        }
#endregion

#region ToString
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
#endregion
    }
}