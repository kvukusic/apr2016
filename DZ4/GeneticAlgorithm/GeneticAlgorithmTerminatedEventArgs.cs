using System;

namespace APR.DZ4
{
    public class GeneticAlgorithmTerminatedEventArgs : EventArgs
    {
        public GeneticAlgorithmTerminatedEventArgs(string reason)
        {
            Reason = reason;
        }

        public string Reason { get; private set; }
    }
}