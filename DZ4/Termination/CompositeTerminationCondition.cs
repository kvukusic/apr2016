using System;
using System.Collections.Generic;
using System.Linq;

namespace APR.DZ4
{
    public class CompositeTerminationCondition : ITerminationCondition
    {
        private IList<ITerminationCondition> _terminationConditions;

        public CompositeTerminationCondition(params ITerminationCondition[] conditions)
        {
            _terminationConditions = new List<ITerminationCondition>();

            if (conditions != null)
            {
                foreach (var c in conditions)
                {
                    AddTerminationCondition(c);
                }
            }
        }

        public void AddTerminationCondition(ITerminationCondition condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException("Condition cannot be null");
            }

            _terminationConditions.Add(condition);
        }

        public string Description
        {
            get 
            {
                return string.Join("|", _terminationConditions.Select(c => c.Description));
            }
        }

        public bool HasReached<T>(IGeneticAlgorithm<T> geneticAlgorithm) where T : IChromosome
        {
            return _terminationConditions.Any(c => c.HasReached(geneticAlgorithm));
        }
    }
}