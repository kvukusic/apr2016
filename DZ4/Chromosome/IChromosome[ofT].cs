using System;

namespace APR.DZ4
{
    public interface IChromosome<TValue> : IChromosome
    {
        TValue[] GetValues();

        TValue GetValue(int index);

        void SetValue(int index, TValue value);

        string GetValueString(int index);
    }
}