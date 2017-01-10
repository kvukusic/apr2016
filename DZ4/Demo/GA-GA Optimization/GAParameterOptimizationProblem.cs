using System;
using System.Collections;
using System.Collections.Generic;
using APR.DZ2.Functions;
using System.Linq;

namespace APR.DZ4.Demo
{
    public class GAParameterOptimizationProblem : IProblem<GAParametersChromosome>
    {
        private class GAParametersFitnessComparer : IComparer<GAParametersChromosome>
        {
            public int Compare(GAParametersChromosome x, GAParametersChromosome y)
            {
                if (x.Fitness == y.Fitness) return 0;
                if (x.Fitness < y.Fitness) return 1;
                return -1;
            }
        }

        private int _evaluations;
        private Function _function;
        private int _functionDimension;

        public GAParameterOptimizationProblem(
            Function function, int functionDimension)
        {
            _evaluations = 0;
            _function = function;
            _functionDimension = functionDimension;
        }

        public int Dimension
        {
            get { return 1; }
        }

        public IComparer<GAParametersChromosome> FitnessComparer
        {
            get { return new GAParametersFitnessComparer(); }
        }

        public int FitnessFunctionEvaluations
        {
            get { return _evaluations; }
        }

        public GAParametersChromosome CreateChromosome()
        {
            return new GAParametersChromosome(this, GAParameters.CreateRandom());
        }

        public void Evaluate(GAParametersChromosome chromosome)
        {
            // if (chromosome.IsEvaluated) return;

            _evaluations++;

            GAParameters parameters = chromosome.GetValue(0);

            _function.Clear();

            int iters = 10;
            int childEvaluations = 10000;
            int maxGenerations = 100;
            double[] childGAFitnesses = new double[iters];

            var chromosomeRepresentation = GAParameters.ChromosomeRepresentation[parameters.ChromosomeRepresentationIndex];

            for (int i = 0; i < iters; i++)
            {
                if (chromosomeRepresentation == GAParameters.ChromosomeRepresentationType.Binary)
                {
                    IProblem<BinaryChromosome> problem = new FunctionMinimizationBinaryProblem(_function, _functionDimension, 4, -50, 150);
                    int populationSize = GAParameters.PopulationSize[parameters.PopulationSizeIndex];
                    double crossoverRate = GAParameters.CrossoverRate[parameters.CrossoverRateIndex];
                    ICrossoverOperator<BinaryChromosome> crossoverOperator;
                    ISelectionOperator<BinaryChromosome> selectionOperator;
                    int tournamentSize = GAParameters.TournamentSize[parameters.TournamentSizeIndex];
                    double selectivePressure = GAParameters.SelectivePressure[parameters.SelectivePressureIndex];
                    IMutationOperator<BinaryChromosome> mutationOperator;
                    double mutationRate = GAParameters.MutationRate[parameters.MutationRateIndex];
                    GeneticAlgorithmVariant variant = GAParameters.GAVariant[parameters.GAVariantIndex];
                    double elitismRate = GAParameters.ElitismRate[parameters.ElitismRateIndex];
                    double generationGap = GAParameters.GenerationGap[parameters.GenerationGapIndex];

                    var crossOperatorType = GAParameters.CrossoverOperator[parameters.CrossoverOperatorIndex];
                    if (crossOperatorType == GAParameters.CrossoverOperatorType.OnePoint)
                    {
                        crossoverOperator = new OnePointCrossoverOperator(crossoverRate);
                    }
                    else
                    {
                        crossoverOperator = new TwoPointCrossoverOperator(crossoverRate);
                    }

                    var mutationOperatorType = GAParameters.MutationOperator[parameters.MutationOperatorIndex];
                    if (mutationOperatorType == GAParameters.MutationOperatorType.BinaryBoundary)
                    {
                        mutationOperator = new BinaryBoundaryMutationOperator(mutationRate);
                    }
                    else if (mutationOperatorType == GAParameters.MutationOperatorType.BitFlip)
                    {
                        mutationOperator = new BitFlipMutationOperator(mutationRate);
                    }
                    else
                    {
                        mutationOperator = new SimpleMutationOperator(mutationRate);
                    }

                    if (GAParameters.SelectionOperator[parameters.SelectionOperatorIndex] == GAParameters.SelectionOperatorType.Tournament)
                    {
                        selectionOperator = new TournamentSelectionOperator<BinaryChromosome>(tournamentSize, problem);
                    }
                    else
                    {   
                        selectionOperator = new RouletteWheelSelectionOperator<BinaryChromosome>(selectivePressure, problem);
                    }

                    IGeneticAlgorithm<BinaryChromosome> ga;

                    if (variant == GeneticAlgorithmVariant.Generational)
                    {
                        ga = new GenerationalGeneticAlgorithm<BinaryChromosome>(
                            problem,
                            populationSize,
                            selectionOperator,
                            crossoverOperator,
                            mutationOperator,
                            new CompositeTerminationCondition(
                                new MaxEvaluationsTerminationCondition(childEvaluations),
                                new MaxGenerationsTerminationCondition(maxGenerations)
                            ),
                            elitismRate
                        );

                    }
                    else
                    {
                        ga = new SteadyStateGeneticAlgorithm<BinaryChromosome>(
                            problem,
                            populationSize,
                            selectionOperator,
                            crossoverOperator,
                            mutationOperator,
                            new CompositeTerminationCondition(
                                new MaxEvaluationsTerminationCondition(childEvaluations),
                                new MaxGenerationsTerminationCondition(maxGenerations)
                            ),
                            generationGap,
                            new WorstFitnessReplacementOperator<BinaryChromosome>(problem)
                        );
                    }

                    // new GeneticAlgorithmRunner<BinaryChromosome>(ga).Run();

                    ga.Run();

                    childGAFitnesses[i] = ga.BestIndividual.Fitness;
                    
                }
                else
                {
                    IProblem<FloatingPointChromosome> problem = new FunctionMinimizationFloatingPointProblem(_function, _functionDimension, -50, 150);
                    int populationSize = GAParameters.PopulationSize[parameters.PopulationSizeIndex];
                    double crossoverRate = GAParameters.CrossoverRate[parameters.CrossoverRateIndex];
                    ICrossoverOperator<FloatingPointChromosome> crossoverOperator;
                    ISelectionOperator<FloatingPointChromosome> selectionOperator;
                    int tournamentSize = GAParameters.TournamentSize[parameters.TournamentSizeIndex];
                    double selectivePressure = GAParameters.SelectivePressure[parameters.SelectivePressureIndex];
                    IMutationOperator<FloatingPointChromosome> mutationOperator;
                    double mutationRate = GAParameters.MutationRate[parameters.MutationRateIndex];
                    GeneticAlgorithmVariant variant = GAParameters.GAVariant[parameters.GAVariantIndex];
                    double elitismRate = GAParameters.ElitismRate[parameters.ElitismRateIndex];
                    double generationGap = GAParameters.GenerationGap[parameters.GenerationGapIndex];

                    var crossOperatorType = GAParameters.CrossoverOperator[parameters.CrossoverOperatorIndex];
                    if (crossOperatorType == GAParameters.CrossoverOperatorType.Arithmetic)
                    {
                        crossoverOperator = new ArithmeticCrossoverOperator(crossoverRate);
                    }
                    else
                    {
                        crossoverOperator = new HeuristicCrossoverOperator(crossoverRate);
                    }

                    var mutationOperatorType = GAParameters.MutationOperator[parameters.MutationOperatorIndex];
                    if (mutationOperatorType == GAParameters.MutationOperatorType.FloatingPointBoundary)
                    {
                        mutationOperator = new FloatingPointBoundaryMutationOperator(mutationRate);
                    }
                    else if (mutationOperatorType == GAParameters.MutationOperatorType.Uniform)
                    {
                        mutationOperator = new UniformMutationOperator(mutationRate);
                    }
                    else
                    {
                        mutationOperator = new GaussianMutationOperator(mutationRate);
                    }

                    if (GAParameters.SelectionOperator[parameters.SelectionOperatorIndex] == GAParameters.SelectionOperatorType.Tournament)
                    {
                        selectionOperator = new TournamentSelectionOperator<FloatingPointChromosome>(tournamentSize, problem);
                    }
                    else
                    {   
                        selectionOperator = new RouletteWheelSelectionOperator<FloatingPointChromosome>(selectivePressure, problem);
                    }

                    IGeneticAlgorithm<FloatingPointChromosome> ga;

                    if (variant == GeneticAlgorithmVariant.Generational)
                    {
                        ga = new GenerationalGeneticAlgorithm<FloatingPointChromosome>(
                            problem,
                            populationSize,
                            selectionOperator,
                            crossoverOperator,
                            mutationOperator,
                            new CompositeTerminationCondition(
                                new MaxEvaluationsTerminationCondition(childEvaluations),
                                new MaxGenerationsTerminationCondition(maxGenerations)
                            ),
                            elitismRate
                        );

                    }
                    else
                    {
                        ga = new SteadyStateGeneticAlgorithm<FloatingPointChromosome>(
                            problem,
                            populationSize,
                            selectionOperator,
                            crossoverOperator,
                            mutationOperator,
                            new CompositeTerminationCondition(
                                new MaxEvaluationsTerminationCondition(childEvaluations),
                                new MaxGenerationsTerminationCondition(maxGenerations)
                            ),
                            generationGap,
                            new WorstFitnessReplacementOperator<FloatingPointChromosome>(problem)
                        );
                    }

                    // new GeneticAlgorithmRunner<FloatingPointChromosome>(ga).Run();

                    ga.Run();

                    childGAFitnesses[i] = ga.BestIndividual.Fitness;
                }
            }

            chromosome.Fitness = childGAFitnesses.Average();
        }

        public bool isValid(GAParametersChromosome chromosome)
        {
            return chromosome.GetValue(0).IsValid();
        }
    }
}