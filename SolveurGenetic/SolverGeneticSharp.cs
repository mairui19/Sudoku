using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Randomizations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using Noyau;

namespace SolveurGenetic
{
    class SolverGeneticSharp : ISudokuSolveur
    {
        public Sudoku ResoudreSudoku(Sudoku s)
        {
            var populationSize = 1000;

            var sudokuChromosome = new SudokuPermutationsChromosome(s);


            var fitness = new SudokuFitness(s);

            var selection = new EliteSelection();

            var crossover = new UniformCrossover();

            var mutation = new UniformMutation();



            var population = new Population(populationSize, populationSize, sudokuChromosome);

            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation)

            {

                Termination = new OrTermination(new ITermination[]

                {

                    new FitnessThresholdTermination(0),

                    new GenerationNumberTermination(80)

                })

            };

            ga.Start();



            var bestIndividual = (ISudokuChromosome)(ga.Population.BestChromosome);

            var solutions = bestIndividual.GetSudokus();

            return solutions[0];
        }
    }



    /// <summary>

    /// Each type of chromosome for solving a sudoku is simply required to output a list of candidate sudokus

    /// </summary>

    public interface ISudokuChromosome

    {

        IList<Sudoku> GetSudokus();

    }



    /// <summary>

    /// Evaluates a sudoku chromosome for completion by counting duplicates in rows, columns, boxes, and differences from the target mask

    /// </summary>

    public class SudokuFitness : IFitness

    {

        /// <summary>

        /// The target Sudoku Mask to solve.

        /// </summary>

        private readonly Sudoku _targetSudoku;



        public SudokuFitness(Sudoku targetSudoku)

        {

            _targetSudoku = targetSudoku;

        }



        /// <summary>

        /// Evaluates a chromosome according to the IFitness interface. Simply reroutes to a typed version.

        /// </summary>

        /// <param name="chromosome"></param>

        /// <returns></returns>

        public double Evaluate(IChromosome chromosome)

        {

            return Evaluate((ISudokuChromosome)chromosome);

        }



        /// <summary>

        /// Evaluates a ISudokuChromosome by summing over the fitnesses of its corresponding Sudoku boards.

        /// </summary>

        /// <param name="chromosome">a Chromosome that can build Sudokus</param>

        /// <returns>the chromosome's fitness</returns>

        public double Evaluate(ISudokuChromosome chromosome)

        {

            List<double> scores = new List<double>();



            var sudokus = chromosome.GetSudokus();

            foreach (var sudoku in sudokus)

            {

                scores.Add(Evaluate(sudoku));

            }



            return scores.Sum();

        }



        /// <summary>

        /// Evaluates a single Sudoku board by counting the duplicates in rows, boxes

        /// and the digits differing from the target mask.

        /// </summary>

        /// <param name="testSudoku">the board to evaluate</param>

        /// <returns>the number of mistakes the Sudoku contains.</returns>

        public double Evaluate(Sudoku testSudoku)

        {

            // We use a large lambda expression to count duplicates in rows, columns and boxes

            var cells = testSudoku.Cells.Select((c, i) => new { index = i, cell = c });

            var toTest = cells.GroupBy(x => x.index / 9).Select(g => g.Select(c => c.cell)) // rows

              .Concat(cells.GroupBy(x => x.index % 9).Select(g => g.Select(c => c.cell))) //columns

              .Concat(cells.GroupBy(x => x.index / 27 * 27 + x.index % 9 / 3 * 3).Select(g => g.Select(c => c.cell))); //boxes

            var toReturn = -toTest.Sum(test => test.GroupBy(x => x).Select(g => g.Count() - 1).Sum()); // Summing over duplicates

            toReturn -= cells.Count(x => _targetSudoku.Cells[x.index] > 0 && _targetSudoku.Cells[x.index] != x.cell); // Mask

            return toReturn;

        }







    }


    /// <summary>

    /// This simple chromosome simply represents each cell by a gene with value between 1 and 9, accounting for the target mask if given

    /// </summary>

 
    public class SudokuPermutationsChromosome : ChromosomeBase, ISudokuChromosome

    {

        /// <summary>

        /// The target Sudoku mask to solve

        /// </summary>

        protected readonly Sudoku TargetSudoku;



        /// <summary>

        /// The list of row permutations accounting for the mask

        /// </summary>

        protected readonly IList<IList<IList<int>>> TargetRowsPermutations;



        /// <summary>

        /// This constructor assumes no mask

        /// </summary>

        public SudokuPermutationsChromosome() : this(null)

        {

        }



        /// <summary>

        /// Constructor with a mask sudoku to solve, assuming a length of 9 genes

        /// </summary>

        /// <param name="targetSudoku">the target sudoku to solve</param>

        public SudokuPermutationsChromosome(Sudoku targetSudoku) : this(targetSudoku, 9)

        {



        }



        /// <summary>

        /// Constructor with a mask and a number of genes

        /// </summary>

        /// <param name="targetSudoku">the target sudoku to solve</param>

        /// <param name="length">the number of genes</param>

        public SudokuPermutationsChromosome(Sudoku targetSudoku, int length) : base(length)

        {

            TargetSudoku = targetSudoku;

            TargetRowsPermutations = GetRowsPermutations(TargetSudoku);

            for (int i = 0; i < Length; i++)

            {

                ReplaceGene(i, GenerateGene(i));

            }

        }



        /// <summary>

        /// generates a chromosome gene from its index containing a random row permutation

        /// amongst those respecting the target mask. 

        /// </summary>

        /// <param name="geneIndex">the index for the gene</param>

        /// <returns>a gene generated for the index</returns>

        public override Gene GenerateGene(int geneIndex)

        {



            var rnd = RandomizationProvider.Current;

            //we randomize amongst the permutations that account for the target mask.

            var permIdx = rnd.GetInt(0, TargetRowsPermutations[geneIndex].Count);

            return new Gene(permIdx);

        }



        public override IChromosome CreateNew()

        {

            var toReturn = new SudokuPermutationsChromosome(TargetSudoku);

            return toReturn;

        }





        /// <summary>

        /// builds a single Sudoku from the given row permutation genes

        /// </summary>

        /// <returns>a list with the single Sudoku built from the genes</returns>

        public virtual IList<Sudoku> GetSudokus()

        {

            var listInt = new List<int>(81);

            for (int i = 0; i < 9; i++)

            {

                int permIDx = GetPermutationIndex(i);

                // we use a modulo operator in case the gene was swapped:

                // It may contain a number higher than the number of available permutations. 

                var perm = TargetRowsPermutations[i][permIDx % TargetRowsPermutations[i].Count].ToList();

                listInt.AddRange(perm);

            }

            var sudoku = new Sudoku() { Cells = listInt };

            return new List<Sudoku>(new[] { sudoku });

        }



        /// <summary>

        /// Gets the permutation to apply from the index of the row concerned

        /// </summary>

        /// <param name="rowIndex">the index of the row to permute</param>

        /// <returns>the index of the permutation to apply</returns>

        protected virtual int GetPermutationIndex(int rowIndex)

        {

            return (int)GetGene(rowIndex).Value;

        }





        /// <summary>

        /// This method computes for each row the list of digit permutations that respect the target mask, that is the list of valid rows discarding columns and boxes

        /// </summary>

        /// <param name="Sudoku">the target sudoku to account for</param>

        /// <returns>the list of permutations available</returns>

        public IList<IList<IList<int>>> GetRowsPermutations(Sudoku Sudoku)

        {

            if (Sudoku == null)

            {

                return UnfilteredPermutations;

            }



            // we store permutations to compute them once only for each target Sudoku

            if (!_rowsPermutations.TryGetValue(Sudoku, out var toReturn))

            {

                // Since this is a static member we use a lock to prevent parallelism.

                // This should be computed once only.

                lock (_rowsPermutations)

                {

                    if (!_rowsPermutations.TryGetValue(Sudoku, out toReturn))

                    {

                        toReturn = GetRowsPermutationsUncached(Sudoku);

                        _rowsPermutations[Sudoku] = toReturn;

                    }

                }

            }

            return toReturn;

        }



        private IList<IList<IList<int>>> GetRowsPermutationsUncached(Sudoku Sudoku)

        {

            var toReturn = new List<IList<IList<int>>>(9);

            for (int i = 0; i < 9; i++)

            {

                var tempList = new List<IList<int>>();

                foreach (var perm in AllPermutations)

                {

                    if (!Range9.Any(j => Sudoku.Cells[i*9+ j] > 0

                                         && (perm[j] != Sudoku.Cells[i * 9 + j])))

                    {

                        tempList.Add(perm);

                    }

                }

                toReturn.Add(tempList);

            }



            return toReturn;

        }







        /// <summary>

        /// Produces 9 copies of the complete list of permutations

        /// </summary>

        public static IList<IList<IList<int>>> UnfilteredPermutations

        {

            get

            {

                if (!_unfilteredPermutations.Any())

                {

                    lock (_unfilteredPermutations)

                    {

                        if (!_unfilteredPermutations.Any())

                        {

                            _unfilteredPermutations = Range9.Select(i => AllPermutations).ToList();

                        }

                    }

                }

                return _unfilteredPermutations;

            }

        }



        /// <summary>

        /// Builds the complete list permutations for {1,2,3,4,5,6,7,8,9}

        /// </summary>

        public static IList<IList<int>> AllPermutations

        {

            get

            {

                if (!_allPermutations.Any())

                {

                    lock (_allPermutations)

                    {

                        if (!_allPermutations.Any())

                        {

                            _allPermutations = GetPermutations(Enumerable.Range(1, 9), 9);

                        }

                    }

                }

                return _allPermutations;

            }

        }



        /// <summary>

        /// The list of compatible permutations for a given Sudoku is stored in a static member for fast retrieval

        /// </summary>

        private static readonly IDictionary<Sudoku, IList<IList<IList<int>>>> _rowsPermutations = new Dictionary<Sudoku, IList<IList<IList<int>>>>();



        /// <summary>

        /// The list of row indexes is used many times and thus stored for quicker access.

        /// </summary>

        private static readonly List<int> Range9 = Enumerable.Range(0, 9).ToList();



        /// <summary>

        /// The complete list of unfiltered permutations is stored for quicker access

        /// </summary>

        private static IList<IList<int>> _allPermutations = (IList<IList<int>>)new List<IList<int>>();

        private static IList<IList<IList<int>>> _unfilteredPermutations = (IList<IList<IList<int>>>)new List<IList<IList<int>>>();



        /// <summary>

        /// Computes all possible permutation for a given set

        /// </summary>

        /// <typeparam name="T">the type of elements the set contains</typeparam>

        /// <param name="list">the list of elements to use in permutations</param>

        /// <param name="length">the size of the resulting list with permuted elements</param>

        /// <returns>a list of all permutations for given size as lists of elements.</returns>

        static IList<IList<T>> GetPermutations<T>(IEnumerable<T> list, int length)

        {

            if (length == 1) return list.Select(t => (IList<T>)(new T[] { t }.ToList())).ToList();



            var enumeratedList = list.ToList();

            return (IList<IList<T>>)GetPermutations(enumeratedList, length - 1)

              .SelectMany(t => enumeratedList.Where(e => !t.Contains(e)),

                (t1, t2) => (IList<T>)t1.Concat(new T[] { t2 }).ToList()).ToList();

        }

    }

}
