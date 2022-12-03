﻿using System;
using System.Collections;
using sudoku;

namespace sudoku
{
	public class Sudoku
	{
        int blockRows = 3;
        int blockColumns = 3;
        int numsPerBlock = 9;

        public SudokuBlock[,] field;
        public Sudoku(int[] list)
        {
            field = new SudokuBlock[blockRows, blockColumns];
            // We get 81 ints, these are placed into blocks of 9 ints
            // n keeps track of the current block
            int n = 0;
            for (int i = 0; i < blockRows; i++)
            {
                for (int j = 0; j < blockColumns; j++)
                {
                    // We generate a new temporary array with the next 9 ints in the given array
                    int[] temp = new int[9];
                    for (int x = 0; x < numsPerBlock; x++)
                    {
                        temp[x] = list[x + (numsPerBlock * n)];
                    }
                    n++;
                    field[i, j] = new SudokuBlock(temp);
                }
            }

            printSudoku();
        }

        public void printSudoku()
        {
            string stringBuild = "+---------------------------------------+\n";
            stringBuild += "|+-----------++-----------++-----------+|\n";
            // For every row of 3 blocks
            for (int x = 0; x < 3; x++)
            {
                // For every row in a block
                for (int j = 0; j < 3; j++)
                {
                    // For every block in the row of 3 blocks
                    for (int i = 0; i < 3; i++)
                    {
                        // Add the xth, ith block, in the jth row in that block, to the string
                        stringBuild += field[x, i].printBlock(j);
                        if (i == 2) stringBuild += "||\n";
                    }
                    stringBuild += "|+-----------++-----------++-----------+|\n";
                }
                if (x != 2) stringBuild += "|+-----------++-----------++-----------+|\n";
                else stringBuild += "+---------------------------------------+";
            }
            
            Console.WriteLine(stringBuild);
        }
    }

    public class SudokuBlock
    {
        int rows = 3;
        int columns = 3;
        public int[,] block;
        // 1 is locked, 0 is unlocked in the mask array. Used to check if a number can be swapped
        int[,] mask;
        Random rnd;

        public SudokuBlock(int[] ints)
        {
            block = new int[rows, columns];
            mask = new int[rows, columns];
            rnd = new Random();
            // This list keeps track of all the numbers that are already placed in the block
            List<int> alreadyGenerated = new List<int>();

            // We get an array of 9 ints, these are placed in the 2dimensional array blok
            // n keeps track of where in the given int list we are
            int n = 0;
            for (int i = 0; i < 3; i ++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Setting the mask to either 0 or 1, depending on if the int given is 0
                    if (ints[n] == 0) mask[i, j] = 0;
                    else
                    {
                        // Adding the number to the numbers list if it is locked
                        alreadyGenerated.Add(ints[n]);
                        mask[i, j] = 1;
                    }
                    block[i, j] = ints[n];
                    n++;
                }
            }

            // For every 0, we need to generate a random number, that does not exist yet in this block
            generateRandomNewNumbers(alreadyGenerated);
        }

        private void generateRandomNewNumbers(List<int> alreadyGenerated)
        {
            // Generate a new random number if the mask is set to 0 at that position
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (mask[i, j] == 0)
                    {
                        block[i, j] = getNewRnd(alreadyGenerated);
                    }
                }
            }
        }

        private int getNewRnd(List<int> alreadyGenerated)
        {
            // Generate a new random number
            int rand = rnd.Next(1, 10);
            // Go through the list to check if that number already exists within this block
            bool isAlreadyHere = false;
            for (int i = 0; i < alreadyGenerated.Count; i++)
            {
                if (alreadyGenerated[i] == rand) isAlreadyHere = true;
            }
            // If so, we return this function recursively. If not, we return the generated random number
            if (isAlreadyHere) return getNewRnd(alreadyGenerated);
            else
            {
                alreadyGenerated.Add(rand);
                return rand;
            }
        }

        internal string printBlock(int row)
        {
            string toPrint = "|| ";
            for (int i = 0; i < columns; i++)
            {
                toPrint += block[row, i] + " ";
                if (i != columns - 1)
                {
                    toPrint += "| ";
                }
            }
            return toPrint;
        }
    }
}
