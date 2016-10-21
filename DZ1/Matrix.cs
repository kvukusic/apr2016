using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DZ1
{
    public class Matrix
    {
        public static readonly int PRECISION = 6;
        public static readonly double EPSILON = Math.Pow(10, -PRECISION);

        private int _rows;
        private int _cols;
        private double[][] _elements;

        public Matrix(int rows, int columns)
        {
            if (rows == 0 || columns == 0)
            {
                throw new ArgumentException("Zero number of rows/columns not allowed");
            }

            _rows = rows;
            _cols = columns;
            _elements = new double[rows][];
            for (int r = 0; r < rows; r++)
            {
                _elements[r] = new double[columns];
            }
        }

        public Matrix(double[,] values) : this(values.GetLength(0), values.GetLength(1))
        {
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    this[r][c] = values[r, c];
                }
            }
        }

        public Matrix(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName is null", fileName);
            }

            int rows = 0;
            int cols = 0;
            var values = new List<double[]>();

            using (var streamReader = File.OpenText(fileName))
            {
                string currentLine = null;
                while ((currentLine = streamReader.ReadLine()) != null)
                {
                    string[] valueStrings = currentLine.Split('\t');
                    double[] valueNumbers = new double[valueStrings.Length];
                    for (int i = 0; i < valueStrings.Length; i++)
                    {
                        valueNumbers[i] = Double.Parse(valueStrings[i]);
                    }

                    values.Add(valueNumbers);
                }
            }

            // Check whether the matrix was specified correctly
            rows = values.Count;
            if (rows == 0)
            {
                throw new ArgumentException("Invalid matrix specified in file.");
            }

            // Check whether all rows have same number of columns
            cols = values[0].Length;
            if (cols > 1)
            {
                for (int i = 1; i < rows; i++)
                {
                    if (values[i].Length != cols)
                    {
                        throw new ArgumentException("Invalid matrix specified in file.");
                    }
                }
            }

            // Initialize fields
            if (rows == 0 || cols == 0)
            {
                throw new ArgumentException("Zero rows/columns specified.");
            }

            _rows = rows;
            _cols = cols;
            _elements = new double[rows][];
            for (int r = 0; r < rows; r++)
            {
                _elements[r] = new double[cols];

                for (int c = 0; c < cols; c++)
                {
                    _elements[r][c] = values[r][c];
                }
            }
        }

        public int Rows
        {
            get { return (int) _rows; }
        }

        public int Columns
        {
            get { return (int) _cols; }
        }

        public Matrix Transpose()
        {
            Matrix result = new Matrix(_cols, _rows);
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    result[c][r] = this[r][c];
                }
            }
            return result;
        }

        public double GetValue(int row, int column)
        {
            return _elements[row][column];
        }

        public void SetValue(int row, int column, double value)
        {
            _elements[row][column] = value;
        }

        public Matrix GetRow(int row)
        {
            var retval = new Matrix(1, _cols);
            for (int c = 0; c < _cols; c++)
            {
                retval[0][c] = this[row][c];
            }
            return retval;
        }

        public void SetRow(int row, Matrix rowMatrix)
        {
            if (!rowMatrix.IsRowVector())
            {
                throw new ArgumentException("rowMatrix is not actually a row vector");
            }

            if (_cols != rowMatrix._cols)
            {
                throw new ArgumentException("rowMatrix is not compatible with this matrix");
            }

            for (int c = 0; c < _cols; c++)
            {
                this[row][c] = rowMatrix[0][c];
            }
        }

        public Matrix GetColumn(int column)
        {
            var retval = new Matrix(_rows, 1);
            for (int r = 0; r < _rows; r++)
            {
                retval[r][0] = this[r][column];
            }
            return retval;
        }

        public void SetColumn(int column, Matrix columnMatrix)
        {
            if (!columnMatrix.IsColumnVector())
            {
                throw new ArgumentException("columnMatrix is not actually a column vector");
            }

            if (_rows != columnMatrix._rows)
            {
                throw new ArgumentException("columnMatrix is not compatible with this matrix");
            }

            for (int r = 0; r < _rows; r++)
            {
                this[r][column] = columnMatrix[r][0];
            }
        }

        public bool IsSquareMatrix()
        {
            return _rows == _cols;
        }

        public bool IsIdentityMatrix()
        {
            if (!IsSquareMatrix())
            {
                return false;
            }

            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    if (r != c && Math.Abs(this[r][c]) > EPSILON ||
                        r == c && Math.Abs(this[r][c] - 1.0) > EPSILON)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool IsUnitMatrix()
        {
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    if(Math.Abs(this[r][c] - 1.0) > EPSILON)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool IsZeroMatrix()
        {
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    if(Math.Abs(this[r][c]) > EPSILON)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool IsColumnVector()
        {
            return _cols == 1;
        }

        public bool IsRowVector()
        {
            return _rows == 1;
        }

        public Matrix ToLowerTriangularMatrix()
        {
            if (!IsSquareMatrix())
            {
                throw new ArgumentException("Square matrix required.");
            }

            Matrix result = Matrix.Identity(_rows);

            for (int r = 1; r < _rows; r++)
            {
                for (int c = 0; c < r; c++)
                {
                    result[r][c] = this[r][c];
                }
            }

            return result;
        }

        public Matrix ToUpperTriangularMatrix()
        {
            if (!IsSquareMatrix())
            {
                throw new ArgumentException("Square matrix required.");
            }

            Matrix result = new Matrix(_rows, _cols);

            for (int r = 0; r < _rows; r++)
            {
                for (int c = r; c < _cols; c++)
                {
                    result[r][c] = this[r][c];
                }
            }

            return result;
        }

        #region Static Factory
            
            public static Matrix Identity(int dim)
            {
                Matrix result = new Matrix(dim, dim);
                for (int r = 0; r < dim; r++)
                {
                    result[r][r] = 1.0;
                }
                return result;
            }

        #endregion

        #region Write
            
        public void Write(TextWriter writer)
        {
            using(StringReader sr = new StringReader(this.ToString()))
            {
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    writer.WriteLine(line);
                }
            }
        }

        public void WriteToConsole()
        {
            Write(Console.Out);
        }

        public void WriteToFile(string filePath)
        {
            using(FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using(StreamWriter sw = new StreamWriter(fs))
                {
                    Write(sw);
                }
            }

            Console.Out.WriteLine("Matrix successfully written to file: " + filePath);
        }

        #endregion

        #region Operators

        public static bool operator ==(Matrix lhs, Matrix rhs)
        {
            return Equals(lhs, rhs);
        }

        public static bool operator !=(Matrix lhs, Matrix rhs)
        {
            return !Equals(lhs, rhs);
        }

        public static Matrix operator *(Matrix lhs, Matrix rhs)
        {
            if (lhs._cols != rhs._rows)
            {
                throw new ArgumentException("Matrices are incomaptible for multiplication.");
            }

            Matrix result = new Matrix(lhs._rows, rhs._cols);

            for (int r = 0; r < result._rows; r++)
            {
                for (int c = 0; c < result._cols; c++)
                {
                    Matrix lr = lhs.GetRow(r);
                    Matrix rc = rhs.GetColumn(c);
                    double sum = 0.0;
                    for (int k = 0; k < lr._cols; k++)
                    {
                        sum += lr[0][k] * rc[k][0];
                    }

                    result[r][c] = sum;
                }
            }

            return result;
        }

        public static Matrix operator *(double lhs, Matrix rhs)
        {
            Matrix result = rhs.Copy();
            for (int r = 0; r < result._rows; r++)
            {
                for (int c = 0; c < result._cols; c++)
                {
                    result[r][c] *= lhs;
                }
            }

            return result;
        }

        public static Matrix operator *(Matrix lhs, double rhs)
        {
            Matrix result = lhs.Copy();
            for (int r = 0; r < result._rows; r++)
            {
                for (int c = 0; c < result._cols; c++)
                {
                    result[r][c] *= rhs;
                }
            }

            return result;
        }

        public static Matrix operator /(Matrix lhs, double rhs)
        {
            Matrix result = lhs.Copy();
            for (int r = 0; r < result._rows; r++)
            {
                for (int c = 0; c < result._cols; c++)
                {
                    result[r][c] /= rhs;
                }
            }

            return result;
        }

        public static Matrix operator +(Matrix lhs, Matrix rhs)
        {
            if (!(lhs._rows == rhs._rows && lhs._cols == rhs._cols))
            {
                throw new ArgumentException("Matrices are incomaptible");
            }

            Matrix result = lhs.Copy();
            for (int r = 0; r < result._rows; r++)
            {
                for (int c = 0; c < result._cols; c++)
                {
                    result[r][c] += rhs[r][c];
                }
            }

            return result;
        }

        public static Matrix operator -(Matrix lhs, Matrix rhs)
        {
            if (!(lhs._rows == rhs._rows && lhs._cols == rhs._cols))
            {
                throw new ArgumentException("Matrices are incomaptible");
            }

            Matrix result = lhs.Copy();
            for (int r = 0; r < result._rows; r++)
            {
                for (int c = 0; c < result._cols; c++)
                {
                    result[r][c] -= rhs[r][c];
                }
            }

            return result;
        }

        public static Matrix operator ~(Matrix m)
        {
            return m.Transpose();
        }

        #endregion

        #region Copy

        public Matrix Copy()
        {
            Matrix result = new Matrix(_rows, _cols);
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    result[r][c] = this[r][c];
                }
            }
            return result;
        }

        #endregion

        #region Indexer

        public double[] this[int row]
        {
            get
            {
                return _elements[row];
            }
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    sb.Append(String.Format("{0:F" + PRECISION + "}", _elements[r][c]));
                    if (c < _cols - 1)
                    {
                        sb.Append("\t");
                    }
                }

                if (r < _rows - 1)
                {
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        #endregion

        #region Equals & HashCode

        protected bool Equals(Matrix other)
        {
            if (_rows != other._rows) return false;
            if (_cols != other._cols) return false;

            for (int r = 0; r < _rows; r++)
            {
                for (int c = 0; c < _cols; c++)
                {
                    if (Math.Abs(_elements[r][c] - other._elements[r][c]) > EPSILON)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Matrix)obj);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    hash = hash * 31 + (int)_elements[i][j];
                }
            }
            return hash;
        }

        #endregion

    }
}