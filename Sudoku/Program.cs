using System;
using System.Linq;
using System.Collections.Generic;

namespace sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            // In the properties of the project, make sure that you always run the projects with 81 ints as an argument (may be that git already did this, but not sure)
            // Properties -> Run -> Configurations -> Default -> Fill in 81 ints in the arguments section
            // Can copy paste these:
            // 0 0 3 0 2 0 6 0 0 9 0 0 3 0 5 0 0 1 0 0 1 8 0 6 4 0 0 0 0 8 1 0 2 9 0 0 7 0 0 0 0 0 0 0 8 0 0 6 7 0 8 2 0 0 0 0 2 6 0 9 5 0 0 8 0 0 2 0 3 0 0 9 0 0 5 0 1 0 3 0 0
            // You can also run the application from the command line and just fill in the 81 ints from there
            Sudoku sudoku = new Sudoku(convertToInt(args));
        }

        static int[] convertToInt(string[] args)
        {
            int[] toInts = new int[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                toInts[i] = int.Parse(args[i]);
            }
            return toInts;
        }
    }
}
