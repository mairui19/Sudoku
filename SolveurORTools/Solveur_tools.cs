using Noyau;
using System;
using System.Collections.Generic;
using System.Text;
using Google.OrTools;
using Google.OrTools.Sat;

namespace SolveurORTools
{
    public class Solveur_tools : ISudokuSolveur
    {
        public Sudoku ResoudreSudoku(Sudoku s)
        {
            CpModel model = new CpModel();

            IntVar[][] tab_s = new IntVar[9][];
            for (int i = 0; i < tab_s.Length; i++)
            {
                tab_s[i] = new IntVar[9];
            }

            for (int i = 0; i < tab_s.Length; i++)
            {
                for (int j = 0; j < tab_s[i].Length; j++)
                {
                    tab_s[i][j] = model.NewIntVar(1, 9, "grid" + "(" + i + "," + j + ")");
                }
            }
            // The different contraints
            // All differents on rows 
            for (int i = 0; i < tab_s.Length; i++)
            {
                model.AddAllDifferent(tab_s[i]);
            }
            // All differents on columns 
            IntVar[] tpm = new IntVar[9];
            for (int j = 0; j < tab_s[0].Length; j++)
            {
                for (int i = 0; i < tab_s.Length; i++)
                {
                    tpm[i] = tab_s[i][j];
                }
                model.AddAllDifferent(tpm);
                Array.Clear(tpm, 0, tpm.Length);
            }

            // All differents on cells 
            List<IntVar> ls = new List<IntVar>();
            for (int i = 0; i < 7; i += 3)
            {
                for (int j = 0; j < 7; j += 3)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            ls.Add(tab_s[i + k][j + l]);
                        }

                    }
                    model.AddAllDifferent(ls);
                    ls.Clear();
                }
            }

            // Initial Value
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (s.GetCell(i, j) != 0)
                    {
                        model.Add(tab_s[i][j] == s.GetCell(i, j));
                    }
                }
            }

            // Creation of the Solver 
            CpSolver solver = new CpSolver();
            CpSolverStatus status = solver.Solve(model);
            List<int> lsol = new List<int>();

            if (status == CpSolverStatus.Feasible)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {

                        lsol.Add((int)(solver.Value(tab_s[i][j])));
                    }


                }
            }

            Sudoku resolution = new Sudoku(lsol);

            return resolution;

        }
    }
}
