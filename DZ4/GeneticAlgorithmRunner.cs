using System;

namespace APR.DZ4
{
    public class GeneticAlgorithmRunner<T> where T : IChromosome
    {
        private IGeneticAlgorithm<T> _geneticAlgorithm;

        public GeneticAlgorithmRunner(IGeneticAlgorithm<T> geneticAlgorithm)
        {
            _geneticAlgorithm = geneticAlgorithm;
        }

        public void Run()
        {
            ConsoleEx.WriteLineGreen("Genetic algorithm started...\n");

            try
            {
                _geneticAlgorithm.StateChanged += OnGeneticAlgorithmStateChanged;
                _geneticAlgorithm.Terminated += OnGeneticAlgorithmTerminated;
                _geneticAlgorithm.Run();
            }
            catch (Exception ex)
            {
                ConsoleEx.WriteLineRed(ex.StackTrace);
            }
        }

        private void OnGeneticAlgorithmStateChanged(object sender, GeneticAlgorithmStateChangedEventArgs<T> args)
        {
            if (args.NewState.BestIndividual.Fitness < args.NewState.BestIndividual.Fitness) {
                // TODO write out the best individual change
            }

            string timeString = "";
            if (args.NewState.ElapsedTime < TimeSpan.FromSeconds(1))
                timeString = args.NewState.ElapsedTime.TotalMilliseconds + " ms";
            else if (args.NewState.ElapsedTime < TimeSpan.FromMinutes(1))
                timeString = args.NewState.ElapsedTime.TotalSeconds + " s";
            else
                timeString = (int)args.NewState.ElapsedTime.TotalMinutes + " m " + args.NewState.ElapsedTime.Seconds + " s";

            Console.Write(string.Format(
                "\rTime: {0, -10} Generation: {1, -8} Evaluations: {2, -10} Best fitness: {3, -20} Best individual: {4}",
                timeString, args.NewState.Generation, args.NewState.Evaluations, args.NewState.BestIndividual.Fitness, args.NewState.BestIndividual
            ));
        }

        private void OnGeneticAlgorithmTerminated(object sender, GeneticAlgorithmTerminatedEventArgs args)
        {
            ConsoleEx.WriteLineGreen("\n\nGenetic algorithm finished because one of the stopping criteria!");
            ConsoleEx.WriteLine("The final result: ");
            
            IGeneticAlgorithm<T> ga = sender as IGeneticAlgorithm<T>;
            if (ga != null)
            {
                string timeString = "";
            if (ga.EvolutionTime < TimeSpan.FromSeconds(1))
                timeString = ga.EvolutionTime.TotalMilliseconds + " ms";
            else if (ga.EvolutionTime < TimeSpan.FromMinutes(1))
                timeString = ga.EvolutionTime.TotalSeconds + " s";
            else
                timeString = (int)ga.EvolutionTime.TotalMinutes + " m " + ga.EvolutionTime.Seconds + " s";

            Console.Write(string.Format(
                "Time: {0, -10} Generations: {1, -8} Evaluations: {2, -10} Best fitness: {3, -20} Best individual: {4}",
                timeString, ga.CurrentGeneration, ga.FitnessEvaluations, ga.BestIndividual.Fitness, ga.BestIndividual
            ));
            }
        }
    }
}