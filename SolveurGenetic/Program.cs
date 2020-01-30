using Noyau;
using System;
using System.IO;

namespace SolveurGenetic
{
    class Program
    {
        static void Main(string[] args)
        {
            var lignes = File.ReadAllLines(@"..\..\..\Sudoku_Easy50.txt");
            var sudokus = Sudoku.ParseMulti(lignes);
            var solveur = new SolverGeneticSharp();
            var sudokuAResoudre = sudokus[0];
            Console.WriteLine(sudokuAResoudre);
            var solution1 = solveur.ResoudreSudoku(sudokuAResoudre);
            Console.WriteLine(solution1);
            Console.Read();
        }
    }
}
