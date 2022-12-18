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
            // We create a new empty sudokublock for every row and column
            field = new SudokuBlock[blockRows, blockColumns];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    field[i, j] = new SudokuBlock();
                }
            }
            // We initialise the values in the sudokublocks using the read intlist
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int row = 0; row < 3; row++)
                    {
                        field[i, j].block[row, 0] = list[((row * 9) + (j * 3)) + (i * 27)];
                        field[i, j].block[row, 1] = list[(((row * 9) + 1) + (j * 3)) + (i * 27)];
                        field[i, j].block[row, 2] = list[(((row * 9) + 2) + (j * 3)) + (i * 27)];
                    }
                }
            }
            Console.WriteLine("Empty sudoku: ");
            printSudoku();
            // After specifying the fixed numbers, we generate the random ones.
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    field[i, j].generateRandomNewNumbers();
                }
            }
        }

        public Sudoku(SudokuBlock[,] fields)
        {
            field = fields;
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

        public List<SudokuBlock> generateChildren(int blockIndexX, int blockIndexY)
        {
            return field[blockIndexX, blockIndexY].GenerateSuccessors();
        }

        public Sudoku generateBestSuccessor(int blockIndexX, int blockIndexY, int lowestFound)
        {
            // Using the list of generated children
            List<SudokuBlock> allChildren = generateChildren(blockIndexX, blockIndexY);
            int currentBestScore = int.MaxValue;
            int indexChildren = 0;
            Sudoku tempSudoku;

            // We loop over this list, and check if the sudoku with this new childBlock has a better evaluation
            for (int i = 0; i < allChildren.Count; i++)
            {
                tempSudoku = copyWithUpdatedBlock(blockIndexX, blockIndexY, allChildren[i]);
                int score = tempSudoku.evaluate();
                // If so, we store the index of this block in the list
                if (score < currentBestScore)
                {
                    currentBestScore = score;
                    indexChildren = i;
                }
            }
            // We return the sudoku with the lowest evaluation score
            // Note that the the unchanged sudoku is in the children list, so no check is required to see if it should remain unchanged.
            Sudoku newS = copyWithUpdatedBlock(blockIndexX, blockIndexY, allChildren[indexChildren]);
            return newS;
        }

        public Sudoku copyWithUpdatedBlock(int blockIndexX, int blockIndexY, SudokuBlock newBlock)
        {
            // Update the sudoku with the provided block
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
    }

    public class SudokuBlock
    {
        int rows = 3;
        int columns = 3;
        public int[,] block;
        // 1 is locked, 0 is unlocked in the mask array. Used to check if a number can be swapped
        int[,] mask;
        Random rnd;

        public SudokuBlock()
        {
            block = new int[rows, columns];
            mask = new int[rows, columns];
            rnd = new Random();

            // Creating of all the initial values in this block
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    block[i, j] = 0;
                    mask[i, j] = 0;
                }
            }
        }

        public SudokuBlock(int[,] field, int[,] mask) //Used to generate new blocks from old ones
        {
            this.block = field;
            this.mask = mask;
        }

        public void generateRandomNewNumbers()
        {
            // We make a list of values already present in this block and set the mask to 1 if a values is found
            List<int> alreadyPresent = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (block[i, j] != 0)
                    {
                        alreadyPresent.Add(block[i, j]);
                        mask[i, j] = 1;
                    }
                }
            }

            // Generate a new random number if the mask is set to 0 at that position
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (mask[i, j] == 0)
                    {
                        block[i, j] = getNewRnd(alreadyPresent);
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
            // Debug printer function
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

        public void printBlock()
        {
            // Debug printer function
            string wholeString = "";
            for (int i = 0; i < 3; i++)
            {
                wholeString += printBlock(i) + "\n";
            }
            Console.WriteLine(wholeString);
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
            res.Add(this);
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

