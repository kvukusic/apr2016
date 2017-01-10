using System;

namespace APR.DZ4
{
    public class GeneticAlgorithmRunner<T>
    {
        private IGeneticAlgorithm<T> _geneticAlgorithm;

        public GeneticAlgorithmRunner(IGeneticAlgorithm<T> geneticAlgorithm)
        {
            _geneticAlgorithm = geneticAlgorithm;
        }

        public void Run()
        {
            _geneticAlgorithm.StateChanged += OnGeneticAlgorithmStateChanged;
            _geneticAlgorithm.Terminated += OnGeneticAlgorithmTerminated;
            _geneticAlgorithm.Run();
        }

        private void OnGeneticAlgorithmStateChanged(object sender, GeneticAlgorithmStateChangedEventArgs<T> args)
        {

        }

        private void OnGeneticAlgorithmTerminated(object sender, GeneticAlgorithmTerminatedEventArgs args)
        {
            
        }
    }
}