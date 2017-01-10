using System;
using System.Collections.Generic;

namespace APR.DZ4.Demo
{
    public class GAParameterCrossoverOperator : ICrossoverOperator<GAParametersChromosome>
    {
        private double _crossoverProbability;

        public GAParameterCrossoverOperator(double crossoverProbability)
        {
            _crossoverProbability = crossoverProbability;
        }

        public int NumberOfParents
        {
            get { return 2; }
        }

        public IList<GAParametersChromosome> Execute(IList<GAParametersChromosome> parents)
        {
            GAParametersChromosome par1 = (GAParametersChromosome)parents[0].Copy();
            GAParametersChromosome par2 = (GAParametersChromosome)parents[1].Copy();
            

            IList<GAParametersChromosome> result = new List<GAParametersChromosome>();

            if (RandomNumberGeneratorProvider.Instance.NextDouble() > _crossoverProbability)
            {
                par1.IsEvaluated = true;
                par2.IsEvaluated = true;

                result.Add(par1);
                result.Add(par2);
                return result;
            }

            Tuple<GAParameters, GAParameters> child = GAParameters.Cross(par1.GetValue(0), par2.GetValue(0));
            
            par1.SetValue(0, child.Item1);
            par1.IsEvaluated = false;

            par2.SetValue(0, child.Item2);
            par2.IsEvaluated = false;

            result.Add(par1);

            return result;
        }
    }
}