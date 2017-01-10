using System;
using System.Collections;
using System.Linq;

namespace APR.DZ4
{
    public class BinaryChromosome : AbstractChromosome<BitArray>
    {
        public BinaryChromosome(IBinaryProblem problem) : base(problem)
        {
            // Initialize randomly
            for (int i = 0; i < Dimension; i++)
            {
                BitArray gene = BitArrayHelper.CreateRandom(problem.GetNumberOfBits(i));
                SetValue(i, gene);
            }
        }

        public BinaryChromosome(BinaryChromosome chromosome) : base(chromosome)
        {
        }

        public override IChromosome Copy()
        {
            return new BinaryChromosome(this);
        }

        public override string GetValueString(int index)
        {
            return GetValue(index).ToBinaryString();
        }

        public override string ToString()
        {
            return string.Join("|", Enumerable.Range(0, Dimension).Select(index => GetValueString(index)));
        }
    }
}