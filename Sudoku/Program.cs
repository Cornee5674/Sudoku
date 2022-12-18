using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creating the Sudoku
            string text = File.ReadAllText("Arguments.txt");
            string[] textArgs = text.Split(" ");
            Sudoku sudoku;
            if (args.Length > 0)
            {
                sudoku = new Sudoku(convertToInt(args));
            }else
            {
                sudoku = new Sudoku(convertToInt(textArgs));
            }
            Console.WriteLine("Evaluation of unsolved sudoku: " + sudoku.evaluate());
            Console.WriteLine("\n");

            // Starting stopwatch for time measurement
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Solve the sudoku
            Sudoku solvedSudoku = SolveSudoku(sudoku);

            // Printint elapsed time
            stopWatch.Stop();
            Console.WriteLine("Milliseconds elapsed: " + stopWatch.ElapsedMilliseconds);
            Console.ReadKey();
        }

        static Sudoku SolveSudoku(Sudoku sudoku)
        {
            //MAIN LOOP:
            Sudoku toSolve = sudoku;
            int curValue = toSolve.evaluate();
            int lowestFound = curValue;

            int totalSteps = 0;
            int totalRandomWalks = 0;

            int lowestUntilNow = curValue;
            int lowestAllTime = curValue;
            int amountOfTimesNotLower = 0;
            int whenToApplyRandomWalk = 100;

            int S = 2;
            Random rnd = new Random();
            while (curValue > 0 )
            {
                //select random block
                int blockIdxX = rnd.Next(3);
                int blockIdxY = rnd.Next(3);

                //generate successor and store new evaluation value
                toSolve = sudoku.generateBestSuccessor(blockIdxX, blockIdxY, curValue);
                curValue = toSolve.evaluate();

                if (curValue < lowestAllTime) lowestAllTime = curValue;

                //check if in local optimum and / or plateau
                if (curValue < lowestUntilNow)
                {
                    lowestUntilNow = curValue;
                    amountOfTimesNotLower = 0;
                }
                else
                {
                    // If our current value stayed the s ame, we increment amountOfTimesNotLower
                    // If this value hits a certain threshold, meaning we stagnated,
                    // We do the randomwalk an S amount of times.
                    amountOfTimesNotLower++;
                    if (amountOfTimesNotLower == whenToApplyRandomWalk)
                    {
                        toSolve = randomWalk(toSolve, S);
                        totalRandomWalks++;
                        amountOfTimesNotLower = 0;                    
                        curValue = toSolve.evaluate();
                        lowestUntilNow = curValue;
                    }
                }

                totalSteps++;
            }
            Console.WriteLine("Solved sudoku: ");
            toSolve.printSudoku();
            Console.WriteLine("Random walks: " + totalRandomWalks);
            Console.WriteLine("Total steps: " + totalSteps);
            Console.WriteLine("Evaluation of solved sudoku: " + curValue);
            return toSolve;
        }

        static Sudoku randomWalk(Sudoku sudoku, int howManyTimes)
        {
            Random rnd = new Random();
            Sudoku s = sudoku;
            int timesDone = 0;
            // We choose a random next state an howManyTimes amount. We dont use the function getbestsuccessor since we want to choose randomly,
            // And not based on evaluation function
            while (timesDone != howManyTimes)
            {
                int blockX = rnd.Next(3);
                int blockY = rnd.Next(3);
                List<SudokuBlock> tmpList = s.generateChildren(blockX, blockY);
                int indexToChoose = rnd.Next(tmpList.Count);
                s = s.copyWithUpdatedBlock(blockX, blockY, tmpList[indexToChoose]);
                timesDone++;
            }
            return s;
        }

        static int[] convertToInt(string[] args)
        {
            // Helper function to convert a list of strings to a list of ints
            int[] toInts = new int[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                toInts[i] = int.Parse(args[i]);
            }
            return toInts;
        }     
    }
}
