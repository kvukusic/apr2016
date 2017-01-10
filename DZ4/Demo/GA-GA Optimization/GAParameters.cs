using System;
using System.Text;

namespace APR.DZ4.Demo
{
    public class GAParameters
    {
        public enum ChromosomeRepresentationType { FloatingPoint, Binary }
        public enum SelectionOperatorType { Tournament, RouletteWheel }
        public enum CrossoverOperatorType { Arithmetic, Heuristic, /* Binary */ OnePoint, TwoPoint }
        public enum MutationOperatorType { FloatingPointBoundary, Gaussian, Uniform, /* Binary */ BitFlip, BinaryBoundary, Simple}

        public static ChromosomeRepresentationType[] ChromosomeRepresentation = {ChromosomeRepresentationType.FloatingPoint, ChromosomeRepresentationType.Binary};
        public static int[] PopulationSize = {10, 20, 30, 40, 50, 100, 200, 300, 500, 1000,/* 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000*/};
        public static double[] CrossoverRate = {0.05, 0.1, 0.15, 0.2, 0.25, 0.3, 0.35, 0.4, 0.45, 0.5, 0.55, 0.6, 0.66, 0.7, 0.75, 0.8, 0.85, 0.9, 0.95, 1.0};
        public static double[] MutationRate = {0.05, 0.1, 0.15, 0.2, 0.25, 0.3, 0.35, 0.4, 0.45, 0.5, 0.55, 0.6, 0.66, 0.7, 0.75, 0.8, 0.85, 0.9, 0.95, 1.0};
        public static double[] ElitismRate = {0.0001, 0.0005, 0.001, 0.005, 0.01, 0.05, 0.1};
        public static double[] GenerationGap = {0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9};
        public static GeneticAlgorithmVariant[] GAVariant = {GeneticAlgorithmVariant.SteadyState, GeneticAlgorithmVariant.Generational};
        public static SelectionOperatorType[] SelectionOperator = { SelectionOperatorType.Tournament, SelectionOperatorType.RouletteWheel};
        public static CrossoverOperatorType[] CrossoverOperator = {CrossoverOperatorType.Arithmetic, CrossoverOperatorType.Heuristic, CrossoverOperatorType.OnePoint, CrossoverOperatorType.TwoPoint};
        public static MutationOperatorType[] MutationOperator = {MutationOperatorType.FloatingPointBoundary, MutationOperatorType.Gaussian, MutationOperatorType.Uniform, MutationOperatorType.BitFlip, MutationOperatorType.BinaryBoundary, MutationOperatorType.Simple};
        public static int[] TournamentSize = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20}; // tsize > N/2
        public static double[] SelectivePressure = {1.0, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 2.0};

        public int ChromosomeRepresentationIndex { get; set; }
        public int PopulationSizeIndex { get; set; }
        public int CrossoverRateIndex { get; set; }
        public int MutationRateIndex { get; set; }
        public int ElitismRateIndex { get; set; }
        public int GenerationGapIndex { get; set; }
        public int GAVariantIndex { get; set; }
        public int SelectionOperatorIndex { get {return 0;} set {} }
        public int CrossoverOperatorIndex { get; set; }
        public int MutationOperatorIndex { get; set; }
        public int TournamentSizeIndex { get; set; }
        public int SelectivePressureIndex { get; set; }

        public static GAParameters CreateRandom()
        {
            Random r = RandomNumberGeneratorProvider.Instance;

            GAParameters g = new GAParameters();

            g.ChromosomeRepresentationIndex = r.Next(0, ChromosomeRepresentation.Length);
            g.PopulationSizeIndex = r.Next(0, PopulationSize.Length);
            g.CrossoverRateIndex = r.Next(0, CrossoverRate.Length);
            g.MutationRateIndex = r.Next(0, MutationRate.Length);
            g.ElitismRateIndex = r.Next(0, ElitismRate.Length);
            g.GenerationGapIndex = r.Next(0, GenerationGap.Length);
            g.GAVariantIndex = r.Next(0, GAVariant.Length);
            g.SelectionOperatorIndex = r.Next(0, SelectionOperator.Length);
            g.CrossoverOperatorIndex = r.Next(0, CrossoverOperator.Length);
            g.MutationOperatorIndex = r.Next(0, MutationOperator.Length);
            g.TournamentSizeIndex = r.Next(0, TournamentSize.Length);
            g.SelectivePressureIndex = r.Next(0, SelectivePressure.Length);

            while (!g.IsValid())
            {
                g.ChromosomeRepresentationIndex = r.Next(0, ChromosomeRepresentation.Length);
                g.PopulationSizeIndex = r.Next(0, PopulationSize.Length);
                g.CrossoverRateIndex = r.Next(0, CrossoverRate.Length);
                g.MutationRateIndex = r.Next(0, MutationRate.Length);
                g.ElitismRateIndex = r.Next(0, ElitismRate.Length);
                g.GenerationGapIndex = r.Next(0, GenerationGap.Length);
                g.GAVariantIndex = r.Next(0, GAVariant.Length);
                g.SelectionOperatorIndex = r.Next(0, SelectionOperator.Length);
                g.CrossoverOperatorIndex = r.Next(0, CrossoverOperator.Length);
                g.MutationOperatorIndex = r.Next(0, MutationOperator.Length);
                g.TournamentSizeIndex = r.Next(0, TournamentSize.Length);
                g.SelectivePressureIndex = r.Next(0, SelectivePressure.Length);
            }

            return g;
        }

        public static GAParameters Mutate(GAParameters parameters)
        {
            var g = parameters.Copy();
            var r = RandomNumberGeneratorProvider.Instance;
            var idx = r.Next(0, 13);

            if (idx == 0) g.ChromosomeRepresentationIndex = r.Next(0, ChromosomeRepresentation.Length);
            else if (idx == 1) g.PopulationSizeIndex = r.Next(0, PopulationSize.Length);
            else if (idx == 2) g.CrossoverRateIndex = r.Next(0, CrossoverRate.Length);
            else if (idx == 3) g.MutationRateIndex = r.Next(0, MutationRate.Length);
            else if (idx == 4) g.ElitismRateIndex = r.Next(0, ElitismRate.Length);
            else if (idx == 5) g.GenerationGapIndex = r.Next(0, GenerationGap.Length);
            else if (idx == 6) g.GAVariantIndex = r.Next(0, GAVariant.Length);
            else if (idx == 7) g.SelectionOperatorIndex = r.Next(0, SelectionOperator.Length);
            else if (idx == 8) g.CrossoverOperatorIndex = r.Next(0, CrossoverOperator.Length);
            else if (idx == 9) g.MutationOperatorIndex = r.Next(0, MutationOperator.Length);
            else if (idx == 10) g.TournamentSizeIndex = r.Next(0, TournamentSize.Length);
            else if (idx == 11) g.SelectivePressureIndex = r.Next(0, SelectivePressure.Length);

            while (!g.IsValid())
            {
                if (idx == 0) g.ChromosomeRepresentationIndex = r.Next(0, ChromosomeRepresentation.Length);
                else if (idx == 1) g.PopulationSizeIndex = r.Next(0, PopulationSize.Length);
                else if (idx == 2) g.CrossoverRateIndex = r.Next(0, CrossoverRate.Length);
                else if (idx == 3) g.MutationRateIndex = r.Next(0, MutationRate.Length);
                else if (idx == 4) g.ElitismRateIndex = r.Next(0, ElitismRate.Length);
                else if (idx == 5) g.GenerationGapIndex = r.Next(0, GenerationGap.Length);
                else if (idx == 6) g.GAVariantIndex = r.Next(0, GAVariant.Length);
                else if (idx == 7) g.SelectionOperatorIndex = r.Next(0, SelectionOperator.Length);
                else if (idx == 8) g.CrossoverOperatorIndex = r.Next(0, CrossoverOperator.Length);
                else if (idx == 9) g.MutationOperatorIndex = r.Next(0, MutationOperator.Length);
                else if (idx == 10) g.TournamentSizeIndex = r.Next(0, TournamentSize.Length);
                else if (idx == 11) g.SelectivePressureIndex = r.Next(0, SelectivePressure.Length);
            }

            return g;
        }

        public static Tuple<GAParameters, GAParameters> Cross(GAParameters g1, GAParameters g2)
        {
            var r = RandomNumberGeneratorProvider.Instance;
            var prob = r.NextDouble();
            if (prob < 0.05) 
                if (r.NextDouble() < 0.5) g1.PopulationSizeIndex = r.Next(0, PopulationSize.Length);
                else g2.PopulationSizeIndex = r.Next(0, PopulationSize.Length);
            else if (prob < 0.1) 
                if (r.NextDouble() < 0.5) g1.CrossoverRateIndex = r.Next(0, CrossoverRate.Length);
                else g2.CrossoverRateIndex = r.Next(0, CrossoverRate.Length);
            else if (prob < 0.15) 
                if (r.NextDouble() < 0.5) g1.MutationRateIndex = r.Next(0, MutationRate.Length);
                else g2.MutationRateIndex = r.Next(0, MutationRate.Length);
            else if (prob < 0.20) 
                if (r.NextDouble() < 0.5) g1.ChromosomeRepresentationIndex = r.Next(0, ChromosomeRepresentation.Length);
                else g2.ChromosomeRepresentationIndex = r.Next(0, ChromosomeRepresentation.Length);
            else if (prob < 0.25) 
                if (r.NextDouble() < 0.5) g1.GAVariantIndex = r.Next(0, GAVariant.Length);
                else g2.GAVariantIndex = r.Next(0, GAVariant.Length);
            else if (prob < 0.30) 
                if (r.NextDouble() < 0.5) g1.CrossoverOperatorIndex = r.Next(0, CrossoverOperator.Length);
                else g2.CrossoverOperatorIndex = r.Next(0, CrossoverOperator.Length);
            else if (prob < 0.35) 
                if (r.NextDouble() < 0.5) g1.SelectionOperatorIndex = r.Next(0, SelectionOperator.Length);
                else g2.SelectionOperatorIndex = r.Next(0, SelectionOperator.Length);
            else if (prob < 0.40) 
                if (r.NextDouble() < 0.5) g1.MutationOperatorIndex = r.Next(0, MutationOperator.Length);
                else g2.MutationOperatorIndex = r.Next(0, MutationOperator.Length);
            else if (prob < 0.45) 
                if (r.NextDouble() < 0.5) g1.TournamentSizeIndex = r.Next(0, TournamentSize.Length);
                else g2.TournamentSizeIndex = r.Next(0, TournamentSize.Length);
            else if (prob < 0.50) 
                if (r.NextDouble() < 0.5) g1.ElitismRateIndex = r.Next(0, ElitismRate.Length);
                else g2.ElitismRateIndex = r.Next(0, ElitismRate.Length);
            else if (prob < 0.50) 
                if (r.NextDouble() < 0.5) g1.GenerationGapIndex = r.Next(0, GenerationGap.Length);
                else g2.GenerationGapIndex = r.Next(0, GenerationGap.Length);
            else if (prob < 0.50)
                if (r.NextDouble() < 0.5) g1.SelectivePressureIndex = r.Next(0, SelectivePressure.Length);
                else g2.SelectivePressureIndex = r.Next(0, SelectivePressure.Length);

            while (!g1.IsValid() && !g2.IsValid())
            {
                if (prob < 0.05)
                    if (r.NextDouble() < 0.5) g1.PopulationSizeIndex = r.Next(0, PopulationSize.Length);
                    else g2.PopulationSizeIndex = r.Next(0, PopulationSize.Length);
                else if (prob < 0.1)
                    if (r.NextDouble() < 0.5) g1.CrossoverRateIndex = r.Next(0, CrossoverRate.Length);
                    else g2.CrossoverRateIndex = r.Next(0, CrossoverRate.Length);
                else if (prob < 0.15)
                    if (r.NextDouble() < 0.5) g1.MutationRateIndex = r.Next(0, MutationRate.Length);
                    else g2.MutationRateIndex = r.Next(0, MutationRate.Length);
                else if (prob < 0.20)
                    if (r.NextDouble() < 0.5) g1.ChromosomeRepresentationIndex = r.Next(0, ChromosomeRepresentation.Length);
                    else g2.ChromosomeRepresentationIndex = r.Next(0, ChromosomeRepresentation.Length);
                else if (prob < 0.25)
                    if (r.NextDouble() < 0.5) g1.GAVariantIndex = r.Next(0, GAVariant.Length);
                    else g2.GAVariantIndex = r.Next(0, GAVariant.Length);
                else if (prob < 0.30)
                    if (r.NextDouble() < 0.5) g1.CrossoverOperatorIndex = r.Next(0, CrossoverOperator.Length);
                    else g2.CrossoverOperatorIndex = r.Next(0, CrossoverOperator.Length);
                else if (prob < 0.35)
                    if (r.NextDouble() < 0.5) g1.SelectionOperatorIndex = r.Next(0, SelectionOperator.Length);
                    else g2.SelectionOperatorIndex = r.Next(0, SelectionOperator.Length);
                else if (prob < 0.40)
                    if (r.NextDouble() < 0.5) g1.MutationOperatorIndex = r.Next(0, MutationOperator.Length);
                    else g2.MutationOperatorIndex = r.Next(0, MutationOperator.Length);
                else if (prob < 0.45)
                    if (r.NextDouble() < 0.5) g1.TournamentSizeIndex = r.Next(0, TournamentSize.Length);
                    else g2.TournamentSizeIndex = r.Next(0, TournamentSize.Length);
                else if (prob < 0.50)
                    if (r.NextDouble() < 0.5) g1.ElitismRateIndex = r.Next(0, ElitismRate.Length);
                    else g2.ElitismRateIndex = r.Next(0, ElitismRate.Length);
                else if (prob < 0.50)
                    if (r.NextDouble() < 0.5) g1.GenerationGapIndex = r.Next(0, GenerationGap.Length);
                    else g2.GenerationGapIndex = r.Next(0, GenerationGap.Length);
                else if (prob < 0.50)
                    if (r.NextDouble() < 0.5) g1.SelectivePressureIndex = r.Next(0, SelectivePressure.Length);
                    else g2.SelectivePressureIndex = r.Next(0, SelectivePressure.Length);
            }

            return new Tuple<GAParameters, GAParameters>(g1, g2);
        }

        public GAParameters Copy()
        {
            return new GAParameters()
            {
                ChromosomeRepresentationIndex = this.ChromosomeRepresentationIndex,
                PopulationSizeIndex = this.PopulationSizeIndex,
                CrossoverRateIndex = this.CrossoverRateIndex,
                MutationRateIndex = this.MutationRateIndex,
                ElitismRateIndex = this.ElitismRateIndex,
                GenerationGapIndex = this.GenerationGapIndex,
                GAVariantIndex = this.GAVariantIndex,
                SelectionOperatorIndex = this.SelectionOperatorIndex,
                CrossoverOperatorIndex = this.CrossoverOperatorIndex,
                MutationOperatorIndex = this.MutationOperatorIndex,
                TournamentSizeIndex = this.TournamentSizeIndex,
                SelectivePressureIndex = this.SelectivePressureIndex
            };
        }

        public bool IsValid()
        {
            // Check if these parameters can be used together
            if (TournamentSize[TournamentSizeIndex] > PopulationSize[PopulationSizeIndex]) return false;
            if (ChromosomeRepresentation[ChromosomeRepresentationIndex] == ChromosomeRepresentationType.Binary)
            {
                if (CrossoverOperatorIndex < 2 || MutationOperatorIndex < 3) return false;
            }
            else
            {
                if (CrossoverOperatorIndex > 1 || MutationOperatorIndex > 2) return false;
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine();

            sb.AppendLine("Variant: " + GAVariant[GAVariantIndex]);
            sb.AppendLine("Representation: " + ChromosomeRepresentation[ChromosomeRepresentationIndex]);
            sb.AppendLine("Population size: " + PopulationSize[PopulationSizeIndex]);
            sb.AppendLine("Selection: " + SelectionOperator[SelectionOperatorIndex]);

            if (SelectionOperator[SelectionOperatorIndex] == SelectionOperatorType.Tournament)
            {
                sb.AppendLine("Tournament size: " + TournamentSize[TournamentSizeIndex]);
            }
            else
            {
                sb.AppendLine("Selective pressure: " + SelectivePressure[SelectivePressureIndex]);
            }

            sb.AppendLine("Crossover: " + CrossoverOperator[CrossoverOperatorIndex]);
            sb.AppendLine("Crossover rate: " + CrossoverRate[CrossoverRateIndex]);
            sb.AppendLine("Mutation: " + MutationOperator[MutationOperatorIndex]);
            sb.AppendLine("MutationRate: " + MutationRate[MutationRateIndex]);

            if (GAVariant[GAVariantIndex] == GeneticAlgorithmVariant.Generational)
            {
                sb.AppendLine("Elitism rate: " + ElitismRate[ElitismRateIndex]);
            }
            else
            {
                sb.AppendLine("Generation gap: " + GenerationGap[GenerationGapIndex]);
            }

            return sb.ToString();
        }
    }
}