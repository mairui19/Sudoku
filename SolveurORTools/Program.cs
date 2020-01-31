using Noyau;
using System;
using System.IO;

namespace SolveurORTools
{
    class Program
    {
        static void Main(string[] args)
        {
            var lignes = File.ReadAllLines(@"..\..\..\Sudoku_Easy50.txt");
            var sudokus = Sudoku.ParseMulti(lignes);
            var solveur = new Solveur_tools();
            Console.WriteLine(sudokus[0]);
            var solution1 = solveur.ResoudreSudoku(sudokus[0]);
            Console.WriteLine(solution1);
            Console.Read();

        }

    }

   

}




  

