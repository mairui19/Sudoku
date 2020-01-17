using System;
using System.Collections.Generic;
using System.Text;

namespace Noyau
{
    public interface ISudokuSolveur
    {
        Sudoku ResoudreSudoku(Sudoku s);
    }
}
