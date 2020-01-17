using System;
using System.IO;
using Noyau;

namespace SolveurLiensDansants
{
    class Program
    {
        static void Main(string[] args)
        {
            var lignes = File.ReadAllLines(@"..\..\..\Sudoku_Easy50.txt");
            var sudokus = Sudoku.ParseMulti(lignes);
            var solveur = new SolveurLD();
            var solution1 = solveur.ResoudreSudoku(sudokus[0]);
            Console.WriteLine(solution1);
            Console.Read();
        }
    }
}
