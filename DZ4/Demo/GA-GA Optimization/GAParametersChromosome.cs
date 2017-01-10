using System;

namespace APR.DZ4.Demo
{
    public class GAParametersChromosome : AbstractChromosome<GAParameters>
    {
        private IProblem<GAParametersChromosome> _problem;

        public GAParametersChromosome(IProblem<GAParametersChromosome> problem) : base (problem)
        {
            _problem = problem;
        }

        public GAParametersChromosome(IProblem<GAParametersChromosome> problem, GAParameters parameters) : base (problem)
        {
            _problem = problem;
            SetValue(0, parameters);
        }

        public override IChromosome Copy()
        {
            var result = new GAParametersChromosome(_problem);

            result.SetValue(0, GetValue(0).Copy());

            return result;
        }

        public override string GetValueString(int index)
        {
            return GetValue(index).ToString();
        }

        public bool IsEvaluated { get; set; }

        public override string ToString()
        {
            return GetValue(0).ToString();
        }
    }
}