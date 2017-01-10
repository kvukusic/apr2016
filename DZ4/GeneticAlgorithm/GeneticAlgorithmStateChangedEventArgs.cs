using System;

namespace APR.DZ4
{
    public class GeneticAlgorithmStateChangedEventArgs : EventArgs
    {
        public GeneticAlgorithmStateChangedEventArgs(
            IGeneticAlgorithmState oldState, IGeneticAlgorithmState newState)
        {
            OldState = oldState;
            NewState = newState;
        }

        public IGeneticAlgorithmState OldState { get; private set; }
        public IGeneticAlgorithmState NewState { get; private set; }
    }

    public class GeneticAlgorithmStateChangedEventArgs<T> : EventArgs
    {
        public GeneticAlgorithmStateChangedEventArgs(
            IGeneticAlgorithmState<T> oldState, IGeneticAlgorithmState<T> newState)
        {
            OldState = oldState;
            NewState = newState;
        }

        public IGeneticAlgorithmState<T> OldState { get; private set; }
        public IGeneticAlgorithmState<T> NewState { get; private set; }
    }
}