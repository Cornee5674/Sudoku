using System;
using System.Collections;
using System.Reflection.Metadata;
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

        public (Sudoku,int) generateBestSuccessor(int blockIndexX, int blockIndexY)
        {
            List<SudokuBlock> allChildren = field[blockIndexX,blockIndexY].GenerateSuccessors();
            int currentBestScore = this.evaluate(); ;
            Sudoku temp;
            Sudoku currentBestSudoku = this;
            List<int> allValues= new List<int>();
            for (int i = 0; i < allChildren.Count; i++)
            {
                temp = this.copyWithUpdatedBlock(blockIndexX, blockIndexY, allChildren[i]);
                int score = temp.evaluate();
                allValues.Add(score);
                if (score <= currentBestScore)
                {
                    currentBestScore = score;
                    currentBestSudoku = temp;
                }
                if (currentBestScore == 0) break; //If the sudoku is solved there is no need to continue searching
            }
            currentBestSudoku.printSudoku();
            Console.WriteLine(currentBestScore);
            return (currentBestSudoku, currentBestScore);
        }

        public Sudoku copyWithUpdatedBlock(int blockIndexX, int blockIndexY, SudokuBlock newBlock) 
        {
            Sudoku res = this;
            res.field[blockIndexX, blockIndexY] = newBlock;
            return res;
        }

        public int evaluate()
        {
            int score = 0;

            //iterate over rows
            for (int i = 0; i < 9; i++)
            {
                //select relevant blokken for current row
                int pos = i / 3;
                SudokuBlock blok1 = field[pos, 0];
                SudokuBlock blok2 = field[pos, 1];
                SudokuBlock blok3 = field[pos, 2];

                //intialize array representing current row
                int[] row = new int[9];

                //fill in current values
                pos = i % 3;
                for (int j = 0; j < 3; j++)
                {
                    row[j] = blok1.block[pos, j];
                    row[j + 3] = blok2.block[pos, j];
                    row[j + 6] = blok3.block[pos, j];
                }

                //use binary array to keep track of numbers present in row
                int[] numbers = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                for (int j = 0; j < 9; j++)
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
                SudokuBlock blok1 = field[0, pos];
                SudokuBlock blok2 = field[1, pos];
                SudokuBlock blok3 = field[2, pos];

                //intialize array representing current column
                int[] col = new int[9];

                //fill in current values
                pos = i % 3;
                for (int j = 0; j < 3; j++)
                {
                    col[j] = blok1.block[j, pos];
                    col[j + 3] = blok2.block[j, pos];
                    col[j + 6] = blok3.block[j, pos];
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

        public Sudoku randomWalk()
        {
            return this;
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
        public SudokuBlock(int[,] field, int[,] mask) //Used to generate new blocks from old ones
        {
            this.block = field;
            this.mask = mask;
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

        public List<SudokuBlock> GenerateSuccessors()
        {
            //Temp mask is used to keep track of the numbers in a block that have already been checked with all of the other numbers
            //This will prevent checking duplicate swaps
            //The mask works exactly the same as the other mask, where 0 is not checked and 1 is already checked
            int[,] tempMask = generateBlockWith0(); 
            List<SudokuBlock> res = new();
            for (int x = 0; x < 3; x++) for (int y = 0; y < 3; y++)
                {
                    //If this value is locked, continue
                    if (mask[x, y] == 1) 
                        continue;

                    for (int i = 0; i < 3; i++) for (int j = 0; j < 3; j++)
                        {
                            //If this value has already been switched with all values, no need to swap again
                            if (tempMask[i, j] == 1) continue;
                            //If target value is locked or it is the same value we are swapping with
                            if (mask[i, j] == 1 || (x == i && y == j)) continue;
                            //Once we are sure this swap is valid, we can do the swap and add it to all the possible successor
                            int[,] old = block.Clone() as int[,];
                            int[,] newBlock = Swap(old, x, y, i, j);
                            res.Add(new SudokuBlock(newBlock,mask));
                        }
                    //After having checked this value with every other number in the block, we can add it to the tempMask
                    tempMask[x,y] = 1;
                }
            return res;
        }

        //Generates a 3*3 array filled with only 0
        private int[,] generateBlockWith0()
        {
            int[,] res = new int[3, 3];
            for (int x = 0; x < 3; x++) for (int y = 0; y < 3; y++)
            {
                    res[x, y] = 0;
            }
            return res;
        }
        
        //Swaps 2 values and returns the new block
        private int[,] Swap(int[,] oldBlock, int x1, int y1, int x2, int y2)
        {
            int v1 = oldBlock[x1, y1];
            int v2 = oldBlock[x2, y2];
            int[,] res = oldBlock.Clone() as int[,];
            res[x2, y2] = v1;
            res[x1, y1] = v2;
            return res;
        }
    }
}

