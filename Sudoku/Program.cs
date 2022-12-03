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
            Console.WriteLine(evaluation(sudoku));
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
            

        public static int evaluation(Sudoku current)
        {
            int score = 0;

            //iterate over rows
            for(int i = 0; i < 9; i++)
            {
                //select relevant blokken for current row
                int pos = i / 3;
                SudokuBlok blok1 = current.blokken[pos, 0];
                SudokuBlok blok2 = current.blokken[pos, 1];
                SudokuBlok blok3 = current.blokken[pos, 2];

                //intialize array representing current row
                int[] row = new int[9];

                //fill in current values
                pos = i % 3;
                for(int j = 0; j < 3; j++)
                {
                    row[j] = blok1.blok[pos, j];
                    row[j + 3] = blok2.blok[pos, j];
                    row[j + 6] = blok3.blok[pos, j];
                }

                //use binary array to keep track of numbers present in row
                int[] numbers = new int[9] {1,1,1,1,1,1,1,1,1};
                for(int j = 0; j < 9; j++)
                {
                    int number = row[j];
                    if (numbers[number - 1] == 1)
                        numbers[number - 1] = 0;
                }

                //add number of absent numbers to score
                score += numbers.Sum();
            }

            //iterate over columns
            for (int i = 0; i < 9; i++)
            {
                //select relevant blokken for current column
                int pos = i / 3;
                SudokuBlok blok1 = current.blokken[0, pos];
                SudokuBlok blok2 = current.blokken[1, pos];
                SudokuBlok blok3 = current.blokken[2, pos];

                //intialize array representing current column
                int[] col = new int[9];

                //fill in current values
                pos = i % 3;
                for (int j = 0; j < 3; j++)
                {
                    col[j] = blok1.blok[j, pos];
                    col[j + 3] = blok2.blok[j, pos];
                    col[j + 6] = blok3.blok[j, pos];
                }

                //use binary array to keep track of numbers present in row
                int[] numbers = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                for (int j = 0; j < 9; j++)
                {
                    int number = col[j];
                    if (numbers[number - 1] == 1)
                        numbers[number - 1] = 0;
                }

                //add number of absent numbers to score
                score += numbers.Sum();
            }
            return score;
        }
    }
}
