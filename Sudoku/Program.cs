using System;
using System.Linq;
using System.Collections.Generic;

namespace sudoku
{
    class Program
    {
        static void Main()
        {

        }
    }
    public class SudokuBlok
    {
        int[,] blok = new int[3, 3];
        int[,] mask = new int[3, 3];
    }
    public class Sudoku
    {
        SudokuBlok[,] field = new SudokuBlok[3, 3];
    }
}
