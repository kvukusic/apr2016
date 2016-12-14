using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using APR.DZ4.Extensions;

namespace APR.DZ4
{
    public class GeneticAlgorithm
    {
        /// <summary>
        /// Supported crossover operators for <see cref="EncodingType.BINARY"/> chromosome
        /// encoding.
        /// </summary>
        private readonly CrossoverOperator[] _binaryCrossoverOperators =
        {
            CrossoverOperator.OnePoint,
            CrossoverOperator.MultiPoint,
            CrossoverOperator.Uniform
        };

        /// <summary>
        /// Supported crossover operators for <see cref="EncodingType.FL_POINT"/> chromosome
        /// encoding.
        /// </summary>
        private readonly CrossoverOperator[] _floatingPointCrossoverOperators =
        {
            CrossoverOperator.Simple,
            CrossoverOperator.Arithmetic,
            CrossoverOperator.Heuristic,
        };

        /// <summary>
        /// A function wrapper that defines additional explicit constraints for
        /// each variable.
        /// </summary>
        private readonly GAFunction _function;

        /// <summary>
        /// The number of bits the chromosome will contain in each dimension. The values are ignored if
        /// the <see cref="Encoding"/> is other than <see cref="EncodingType.BINARY"/>. The length of this array
        /// is the same as the dimension of the fitness function.
        /// </summary>
        private int[] _chromosomeSizes;

        /// <summary>
        /// Holds the value of the current best fitness.
        /// </summary>
        private double _bestFitness;

        /// <summary>
        /// Contains the index in the population of the best individual.
        /// </summary>
        private int _bestFitnessIndex = -1;

        /// <summary>
        /// Holds the average fitness of the population.
        /// </summary>
        private double _avgFitness;

        /// <summary>
        /// Holds the sum of all fitness values in the current population.
        /// </summary>
        private double _sumFitness;

        ///// <summary>
        ///// Holds the best variable values of the current best fitness. Length
        ///// of this array is the same as the dimension of the fitness function.
        ///// </summary>
        //private double[] _bestValues;

        /// <summary>
        /// Holds the current best fitness chromosome in the population.
        /// </summary>
        private Chromosome _bestChromosome;

        /// <summary>
        /// The current population in this GA.
        /// </summary>
        private Chromosome[] _population;

        /// <summary>
        /// The current mating pool.
        /// </summary>
        private Chromosome[] _matingPool;

        /// <summary>
        /// Random number generator.
        /// </summary>
        private readonly Random _random;

        #region GA Parameters
        /// <summary>
        /// The number of population members (chromosomes).
        /// </summary>
        public int PopulationSize { get; set; }

        /// <summary>
        /// Defines the tournament size in when the <see cref="Selection"/> parameter
        /// is <see cref="SelectionType.TOURNAMENT"/> (i.e. how many individuals will compete with each other).
        /// Valid values are from <c>2</c> to half of the population size (<see cref="PopulationSize"/>/2).
        /// </summary>
        public int TournamentSize { get; set; }

        /// <summary>
        /// Defines the mutation rate (probability of changing a single bit in a chromosome).
        /// </summary>
        /// <remarks>
        /// Valid values are between <c>1</c> and <c>100</c>.
        /// </remarks>
        public int MutationRate { get; set; }

        /// <summary>
        /// <para>
        /// Determines the probability that an offspring will be created by the crossover of
        /// two parents as opposed to a replica of one parent if the <see cref="Selection"/>
        /// parameter is of generational type (<see cref="SelectionType.R_WHEEL"/>).
        /// </para>
        /// 
        /// <para>
        /// If the <see cref="Selection"/> parameter is <see cref="SelectionType.ELIMINATION"/> then this value defines
        /// the portion of the population that is replaced each generation (a generation gap). A generation gap of
        /// 0 means none of the population is replaced, conversely a generation gap of 100 means that the entire
        /// population is replaced each generation.
        /// </para>
        /// 
        /// <para>
        /// For any other <see cref="Selection"/> type, this value is ignored.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Valid values are between <c>0</c> and <c>100</c>.
        /// </remarks>
        public int CrossoverRate { get; set; }

        /// <summary>
        /// Determines whether the best member in a population always survives a generation.
        /// </summary>
        public bool IsElitismEnabled { get; set; }

        /// <summary>
        /// A stopping criteria of the GA. The trial will stop if the current generation number is
        /// greater or equal to this value.
        /// </summary>
        public int? StopGen { get; set; }

        /// <summary>
        /// A stopping criteria of the GA. The trial will stop if no progress is made in the specified
        /// number of generations (the best member fitness value has not changed).
        /// </summary>
        /// <remarks>
        /// A single generation is defined as a creation of <see cref="PopulationSize"/> new individuals.
        /// </remarks>
        public int? StopNew { get; set; }

        /// <summary>
        /// A stopping criteria of the GA. The trial will stop if the number of fitness function evaluations
        /// (the number of created chromosomes) exceeds this value.
        /// </summary>
        public int? StopEval { get; set; }

        /// <summary>
        /// A stopping criteria of the GA. The trial will stop if best members fitness value is less then
        /// specified (when searching for a minimum value).
        /// </summary>
        public double? StopValue { get; set; }

        /// <summary>
        /// Defines the minimum precision in problem domain (number of digits after decimal point).
        /// Has effect only when the <see cref="Encoding"/> parameter is <see cref="EncodingType.BINARY"/>.
        /// </summary>
        public int Precision { get; set; }

        /// <summary>
        /// Defines the used chromosome encoding.
        /// <para>
        /// If <see cref="EncodingType.BINARY"/> is used, GA encodes real numbers as binary strings and
        /// operates on them as binaries. Real value is decoded and used for evaluation only.
        /// </para>
        /// <para>
        /// If <see cref="EncodingType.FL_POINT"/> is used, GA uses the system <see cref="double"/> values
        /// as chromosomes.
        /// </para>
        /// </summary>
        public ChromosomeEncoding Encoding { get; set; }

        /// <summary>
        /// Defines the selection method to be used in GA.
        /// <see cref="SelectionType.R_WHEEL"/> is of generational (new population is created from copies of existing ones) 
        /// policy and <see cref="SelectionType.ELIMINATION"/> and <see cref="SelectionType.TOURNAMENT"/> implement a steady-state
        /// (some chromosomes are eliminated, and others replace them) GA.
        /// </summary>
        public SelectionOperator SelectionOperator { get; set; }

        /// <summary>
        /// Defines the used chromosome crossover operator.
        /// </summary>
        public CrossoverOperator CrossoverOperator { get; set; }

        /// <summary>
        /// Defines the used chromosome mutation operator.
        /// </summary>
        public MutationOperator MutationOperator { get; set; }

        /// <summary>
        /// Assures that all parameters are valid. If they are not, an exception
        /// will be thrown with the descibing reason.
        /// </summary>
        private void CheckParameters()
        {
            if (PopulationSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(PopulationSize), PopulationSize,
                    "Needs to be positive.");
            }

            if (SelectionOperator == SelectionOperator.Tournament && (TournamentSize < 2 || TournamentSize > PopulationSize / 2))
            {
                throw new ArgumentOutOfRangeException(nameof(TournamentSize), TournamentSize,
                    "Must be between 2 and N/2.");
            }

            if (MutationRate < 0 || MutationRate > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(MutationRate), MutationRate,
                    "Must be between 0 and 100.");
            }

            if (CrossoverRate < 0 || CrossoverRate > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(CrossoverRate), CrossoverRate,
                    "Must be between 0 and 100.");
            }

            if (StopEval == null && StopGen == null && StopNew == null && StopValue == null)
            {
                throw new ArgumentException("One or more stopping criteria needs to be specified.");
            }

            if (StopEval <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(StopEval), StopEval,
                    "Needs to be positive.");
            }

            if (StopGen <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(StopGen), StopEval,
                    "Needs to be positive.");
            }

            if (StopNew <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(StopNew), StopEval,
                    "Needs to be positive.");
            }

            if (Encoding == ChromosomeEncoding.Binary && Precision <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Precision), Precision,
                    "Needs to be positive.");
            }

            if (Encoding == ChromosomeEncoding.Binary)
            {
                if (!_binaryCrossoverOperators.Contains(CrossoverOperator))
                {
                    throw new ArgumentException(
                        "Defined crossover operator cannot be applied with the specified encoding.",
                        nameof(CrossoverOperator));
                }
            }
            else if (Encoding == ChromosomeEncoding.FloatingPoint)
            {
                if (!_floatingPointCrossoverOperators.Contains(CrossoverOperator))
                {
                    throw new ArgumentException(
                        "Defined crossover operator cannot be applied with the specified encoding.",
                        nameof(CrossoverOperator));
                }
            }
        }

        #endregion

        public GeneticAlgorithm(GAFunction function)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            _function = function;

            // Initialize with default values
            PopulationSize = 200;
            TournamentSize = 3;
            MutationRate = 5;
            CrossoverRate = 70;
            IsElitismEnabled = true;
            StopEval = 5000000;
            Precision = 1;
            Encoding = ChromosomeEncoding.Binary;
            SelectionOperator = SelectionOperator.Tournament;
            CrossoverOperator = CrossoverOperator.OnePoint;
            MutationOperator = MutationOperator.Uniform;

            // Since we are minimizing functions, the initial best fitness is set to positive infinity
            _bestFitness = double.PositiveInfinity;
            //_bestValues = new double[_function.Dimension];

            // Create random number generator
            _random = new Random();
        }

        /// <summary>
        /// Initializes this GA with the default parameters and a random population.
        /// </summary>
        public void Initialize()
        {
            CheckParameters();
            DetermineChromosomeLengths();
            CreateRandomPopulation();
        }

        /// <summary>
        /// Executes the GA with the specified parameters on the current population.
        /// </summary>
        public void Run()
        {
            // TODO trenutno je podržana samo 3-turnirska selekcija iz skripte, trebalo bi napraviti
            // TODO neki framework i staviti na GitHub, isto tako nisu podržani svi operatori koji
            // su definirani

            // Evaluate the initial population
            EvaluatePopulation();

            var end = false;
            var endReason = string.Empty;
            int gen = 0;
            int fitnessUnchangedGen = 0;
            var bestFitness = _bestFitness;

            const int OUTPUT_PRECISION = 5;

            while (!end)
            {
                //// Select
                //Select();

                //// Crossover
                //Crossover();

                //// Mutate

                for (int j = 0; j < PopulationSize; j++)
                {
                    // Select
                    var excludedIndexes = IsElitismEnabled ? new int[] {_bestFitnessIndex} : null;
                    var indexes = _random.NextInts(0, _population.Length, TournamentSize, excludedIndexes).OrderBy(i => _population[i].Fitness).ToArray();
                    var highestFittnesIndex = indexes.Last();

                    // Crossover
                    var child = Crossover(_population[indexes[0]], _population[indexes[1]]);
                    _population[highestFittnesIndex] = child;

                    // Mutate
                    if (_random.Next(0, 100) + 1 < MutationRate)
                    {
                        Mutate(child);
                    }

                    // Evaluate chromosome
                    EvaluateChromosome(child);
                }

                // Reevaluate population
                EvaluatePopulation();

                gen++;

                //var cursorLeft = Console.CursorLeft;
                //var cursorTop = Console.CursorTop;
                //Console.SetCursorPosition(0, 0);
                Console.Write("\rGeneration: " + gen + " Evaluations: " + _function.Evaluations);
                //Console.SetCursorPosition(cursorLeft, cursorTop);

                if (_bestFitness < bestFitness)
                {
                    bestFitness = _bestFitness;
                    fitnessUnchangedGen = 0;

                    Console.WriteLine(
                        "\rBest fitness changed at {0} evaluations in {1}. generation.\nBest individual is [{2}] with fitness [{3}]\n",
                        _function.Evaluations, gen, _bestChromosome, _bestChromosome.Fitness.ToString("F" + OUTPUT_PRECISION));
                }
                else
                {
                    fitnessUnchangedGen++;
                }

                if (StopNew.HasValue && fitnessUnchangedGen >= StopNew)
                {
                    end = true;
                    endReason = "Termination condition reached: No improvement in " + StopNew + " generations";
                }

                if (StopGen.HasValue && gen >= StopGen)
                {
                    end = true;
                    endReason = "Termination condition reached: Number of generations >= " + StopGen;
                }

                if (_bestChromosome != null && StopValue.HasValue && _bestChromosome.Fitness <= StopValue)
                {
                    end = true;
                    endReason = "Termination condition reached: Fitness value <= " + StopValue;
                }

                if (StopEval.HasValue && _function.Evaluations >= StopEval)
                {
                    end = true;
                    endReason = "Termination condition reached: Number of evaluations >= " + StopEval;
                }
            }

            Console.WriteLine("\n\n" + endReason + "\n");
            Console.WriteLine("Evolved generations: " + gen);
            Console.WriteLine(
                        "Best individual: [{0}]\nFitness value: [{1}]",
                        _bestChromosome, _bestChromosome.Fitness);
            Console.WriteLine();
        }

        /// <summary>
        /// Calculate the chromosome lengths.
        /// </summary>
        private void DetermineChromosomeLengths()
        {
            if (Encoding == ChromosomeEncoding.Binary)
            {
                _chromosomeSizes = new int[_function.Dimension];
                for (int i = 0; i < _chromosomeSizes.Length; i++)
                {
                    var min = _function[i].Min;
                    var max = _function[i].Max;

                    // Calculate needed number of bits
                    var n =
                        (int)Math.Ceiling(Math.Log10(Math.Floor(1 + (max - min) * Math.Pow(10, Precision))) / Math.Log10(2));

                    _chromosomeSizes[i] = n;
                }
            }
        }

        /// <summary>
        /// Initializes a new population of this GA and populates
        /// it with random chromosomes.
        /// </summary>
        private void CreateRandomPopulation()
        {
            _population = new Chromosome[PopulationSize];
            for (int i = 0; i < PopulationSize; i++)
            {
                _population[i] = CreateRandomChromosome();
            }
        }

        /// <summary>
        /// Creates a random chromosome with correct encoding.
        /// </summary>
        /// <returns></returns>
        private Chromosome CreateRandomChromosome()
        {
            switch (Encoding)
            {
                case ChromosomeEncoding.Binary:
                {
                    var chr = new BinaryChromosome(_function.Dimension, _chromosomeSizes);
                    for (int i = 0; i < _function.Dimension; i++)
                    {
                        var randomInt = _random.Next(0, (int) Math.Pow(2, chr.Size(i)));
                        var binaryString = Convert.ToString(randomInt, 2).PadLeft(chr.Size(i), '0');
                        for (int j = 0; j < chr.Size(i); j++)
                        {
                            chr.SetGenome(i, j, binaryString[j]);
                        }
                    }

                    return chr;
                }
                case ChromosomeEncoding.FloatingPoint:
                {
                    var chr = new FloatingPointChromosome(_function.Dimension);
                    for (int i = 0; i < _function.Dimension; i++)
                    {
                        chr.SetValue(i, _random.NextDouble(_function[i].Min, _function[i].Max));
                    }

                    return chr;
                }
                default:
                {
                    return null;
                }
            }
        }

        private void EvaluatePopulation()
        {
            var avgFitness = 0.0;
            var sumFitness = 0.0;
            for (int i = 0; i < _population.Length; i++)
            {
                var chr = _population[i];
                sumFitness += EvaluateChromosome(chr);
                if (chr < _bestChromosome)
                {
                    _bestFitness = chr.Fitness;
                    _bestFitnessIndex = i;
                    _bestChromosome = chr.Copy();
                }
            }

            avgFitness = sumFitness/_population.Length;
        }

        private double EvaluateChromosome(Chromosome chr)
        {
            switch (Encoding)
            {
                case ChromosomeEncoding.Binary:
                {
                    var binChr = (BinaryChromosome) chr;
                    var binaryValues = binChr.GetGenomes().Select(c => Convert.ToInt32(new string(c), 2)).ToArray();
                    var floatValues =
                        binaryValues.Select(
                            (i, index) =>
                                _function[index].Min +
                                i*(_function[index].Max - _function[index].Min)/(Math.Pow(2, binChr.Size(index)) - 1))
                            .ToArray();

                    var fitness = _function.Fitness(floatValues);
                    binChr.Fitness = fitness;
                    return fitness;
                }
                case ChromosomeEncoding.FloatingPoint:
                {
                    var flChr = (FloatingPointChromosome) chr;
                    var values = flChr.GetValues();
                    var fitness = _function.Fitness(values);
                    flChr.Fitness = fitness;
                    return fitness;
                }
                default:
                {
                    throw new Exception();
                }
            }
        }

        private void Select()
        {
            
        }

        private Chromosome Crossover(Chromosome chr1, Chromosome chr2)
        {
            if (chr1 is BinaryChromosome && chr2 is BinaryChromosome)
            {
                var b1 = (BinaryChromosome)chr1;
                var b2 = (BinaryChromosome)chr2;

                var r1 = new BinaryChromosome(_function.Dimension, _chromosomeSizes);

                switch (CrossoverOperator)
                {
                    case CrossoverOperator.OnePoint:
                    {
                        for (int i = 0; i < _function.Dimension; i++)
                        {
                            var crossPoint = _random.Next(0, _chromosomeSizes[i]);
                            for (int j = 0; j < crossPoint; j++)
                            {
                                r1.SetGenome(i, j, b1.GetGenome(i, j));
                            }
                            for (int j = crossPoint; j < b2.Size(i); j++)
                            {
                                r1.SetGenome(i, j, b2.GetGenome(i, j));
                            }
                        }

                        break;
                    }
                    case CrossoverOperator.MultiPoint:
                    {
                        throw new NotImplementedException();
                    }
                    case CrossoverOperator.Uniform:
                    {
                        var randomChr = (BinaryChromosome) CreateRandomChromosome();

                        for (int i = 0; i < _function.Dimension; i++)
                        {
                            for (int j = 0; j < _chromosomeSizes[i]; j++)
                            {
                                bool a = b1.GetGenome(i, j) == '1';
                                bool b = b2.GetGenome(i, j) == '1';
                                bool r = randomChr.GetGenome(i, j) == '1';

                                r1.SetGenome(i, j, ((a & b) | (r & (a ^ b))) ? '1' : '0');
                            }
                        }

                        break;
                    }
                }

                return r1;
            }
            else if (chr1 is FloatingPointChromosome && chr2 is FloatingPointChromosome)
            {
                var f1 = (FloatingPointChromosome) chr1;
                var f2 = (FloatingPointChromosome) chr2;

                var chr = new FloatingPointChromosome(_function.Dimension);

                switch (CrossoverOperator)
                {
                    case CrossoverOperator.Simple:
                    {
                        throw new NotImplementedException();

                        break;
                    }
                    case CrossoverOperator.Arithmetic:
                    {
                        //var a = _random.NextDouble();
                        for (int i = 0; i < _function.Dimension; i++)
                        {
                            var a = _random.NextDouble();
                            chr.SetValue(i, a*f1.GetValue(i) + (1 - a)*f2.GetValue(i));
                        }

                        break;
                    }
                    case CrossoverOperator.Heuristic:
                    {
                        if (f1.Fitness < f2.Fitness)
                        {
                            var temp = f1;
                            f1 = f2;
                            f2 = temp;
                        }

                        //var a = _random.NextDouble();
                        for (int i = 0; i < _function.Dimension; i++)
                        {
                            var a = _random.NextDouble();
                            var value = a*(f2.GetValue(i) - f1.GetValue(i)) + f2.GetValue(i);
                            while (value < _function.ConstraintAt(i).Min || value > _function.ConstraintAt(i).Max)
                            {
                                a = _random.NextDouble();
                                value = a*(f2.GetValue(i) - f1.GetValue(i)) + f2.GetValue(i);
                            }

                            chr.SetValue(i, value);
                        }

                        break;
                    }
                }

                EvaluateChromosome(chr);

                return chr;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        private void Crossover()
        {
            switch (CrossoverOperator)
            {
                case CrossoverOperator.Simple:
                {
                    break;
                }
                case CrossoverOperator.Arithmetic:
                {
                    break;
                }
                case CrossoverOperator.Heuristic:
                {
                    break;
                }
                case CrossoverOperator.OnePoint:
                {
                    break;
                }
                case CrossoverOperator.MultiPoint:
                {
                    break;
                }
                case CrossoverOperator.Uniform:
                {
                    break;
                }
            }
        }

        private void Mutate(Chromosome chr)
        {
            switch (MutationOperator)
            {
                case MutationOperator.Uniform:
                {
                    if (chr is BinaryChromosome)
                    {
                        var binChr = (BinaryChromosome) chr;
                        for (int i = 0; i < _function.Dimension; i++)
                        {
                            if (_random.NextBool())
                            {
                                    var mutationIndex = _random.Next(0, _chromosomeSizes[i]);
                                binChr.SetGenome(i, mutationIndex,
                                    binChr.GetGenome(i, mutationIndex) == '0' ? '1' : '0');
                            }
                        }
                    }
                    else if (chr is FloatingPointChromosome)
                    {
                        var f = (FloatingPointChromosome) chr;
                        for (int i = 0; i < _function.Dimension; i++)
                        {
                            f.SetValue(i, _random.NextDouble(_function[i].Min, _function[i].Max));
                        }
                    }
                    else
                    {
                        throw new ArgumentException();
                    }

                    break;
                }
                case MutationOperator.NonUniform:
                {
                    throw new NotImplementedException();
                    break;
                }
                case MutationOperator.Boundary:
                {
                    throw new NotImplementedException();
                    break;
                }
            }
        }
    }
}