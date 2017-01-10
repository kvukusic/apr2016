using System;
using System.Collections.Generic;
using System.Linq;

namespace APR.DZ4
{
    public abstract class AbstractGeneticAlgorithm<T> : IGeneticAlgorithm<T> where T : IChromosome
    {
        private IGeneticAlgorithmState<T> _currentState;
        protected int _populationSize;
        protected IList<T> _population;
        protected IProblem<T> _problem;
        protected ISelectionOperator<T> _selectionOperator;
        protected ICrossoverOperator<T> _crossoverOperator;
        protected IMutationOperator<T> _mutationOperator;
        protected ITerminationCondition _terminationCondition;

        protected AbstractGeneticAlgorithm(
            IProblem<T> problem,
            int populationSize,
            ISelectionOperator<T> selectionOperator, 
            ICrossoverOperator<T> crossoverOperator,
            IMutationOperator<T> mutationOperator,
            ITerminationCondition terminationCondition)
        {
            _problem = problem;
            _populationSize = populationSize;
            _selectionOperator = selectionOperator;
            _crossoverOperator = crossoverOperator;
            _mutationOperator = mutationOperator;
            _terminationCondition = terminationCondition;

            CurrentGeneration = 1;
            EvolutionTime = TimeSpan.Zero;

            _population = CreateInitialPopulation();
            _population = EvaluatePopulation(_population);
        }

        public ISelectionOperator<T> SelectionOperator
        {
            get { return _selectionOperator; } 
        }

        public ICrossoverOperator<T> CrossoverOperator 
        {
            get { return _crossoverOperator; } 
        }

        public IMutationOperator<T> MutationOperator
        {
            get { return _mutationOperator; }
        }

        public IList<T> Population
        {
            get { return _population; }
        }

        public T BestIndividual { get; private set; }

        public int CurrentGeneration { get; private set; }

        public int FitnessEvaluations { get { return _problem.FitnessFunctionEvaluations; } }

        public TimeSpan EvolutionTime { get; private set; }

        protected bool IsTerminationConditionReached()
        {
            if (_terminationCondition.HasReached(this))
            {
                RaiseTerminated(_terminationCondition.Description);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Evolves a single generation. Can be used to continue execution of
        /// the GA after the specified stopping condition have been reached.
        /// </summary>
        public abstract void Evolve();

        public abstract void Run();

        protected virtual IList<T> CreateInitialPopulation()
        {
            var population = new List<T>(_populationSize);
            for (int i = 0; i < _populationSize; i++)
            {
                population.Add(_problem.CreateChromosome());
            }

            return population;
        }

        protected virtual IList<T> EvaluatePopulation(IList<T> population)
        {
            for (int i = 0; i < population.Count; i++)
            {
                _problem.Evaluate(population[i]);
            }

            return population;
        }

        /// <summary>
        /// This method needs to be called at the start of the <see cref="Run"/>
        /// method.
        /// </summary>
        protected virtual void InitState()
        {
            // Find the best individual (the population must be already initialized)
            BestIndividual = _population.OrderByDescending(c => c, _problem.FitnessComparer).First();

            // Initialize the evolution time
            var startDateTime = DateTime.Now;
            EvolutionTime = DateTime.Now - startDateTime;

            // Initialize the current generation number
            CurrentGeneration = 1;
        }

        /// <summary>
        /// This method needs to be called at the end of the <see cref="Evolve"/>
        /// method.
        /// </summary>
        protected virtual void UpdateState()
        {
            // Update the best individual with the best fitness
            BestIndividual = _population.OrderByDescending(c => c, _problem.FitnessComparer).First();

            // Update generation counter
            CurrentGeneration++;

            // Update evolution time
            var startDateTime = DateTime.Now;
            EvolutionTime += DateTime.Now - startDateTime;

            var oldState = _currentState;
            _currentState = new GeneticAlgorithmState<T>()
            {
                BestIndividual = BestIndividual,
                Generation = CurrentGeneration,
                Evaluations = _problem.FitnessFunctionEvaluations,
                ElapsedTime = EvolutionTime
            };

            RaiseStateChanged(oldState, _currentState);

            UpdateStateAwareComponents();
        }

        protected virtual void UpdateStateAwareComponents()
        {
            object[] possibleStateAwares = new object[]
            {
                _selectionOperator,
                _mutationOperator,
                _crossoverOperator
            };

            foreach (var item in possibleStateAwares)
            {
                IGeneticAlgorithmStateAware stateAware = item as IGeneticAlgorithmStateAware;
                if (stateAware != null)
                {
                    stateAware.CurrentState = _currentState;
                }
            }
        }

        public event EventHandler<GeneticAlgorithmStateChangedEventArgs<T>> StateChanged;

        private void RaiseStateChanged(
            IGeneticAlgorithmState<T> oldState, IGeneticAlgorithmState<T> newState)
        {
            if (StateChanged != null)
            {
                StateChanged(this, new GeneticAlgorithmStateChangedEventArgs<T>(oldState, newState));
            }
        }

        public event EventHandler<GeneticAlgorithmTerminatedEventArgs> Terminated;

        private void RaiseTerminated(string reason)
        {
            if (Terminated != null)
            {
                Terminated(this, new GeneticAlgorithmTerminatedEventArgs(reason));
            }
        }
    }
}