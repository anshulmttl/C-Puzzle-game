using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACW
{
    static class RandomLetter
    {
        static Random _random = new Random();

        public static char GetLetter()
        {
            int num = _random.Next(0, 26);
            char let = (char)('a' + num);
            return let;
        }
    }

    struct WordToFind
    {
        public int index;
        public string wordToFind;
        public int colBegin;
        public int rowBegin;
        public int colEnd;
        public int rowEnd;

        public WordToFind(int i, string word, int col_b, int row_b, int col_e, int row_e)
        {
            index = i;
            wordToFind = word;
            colBegin = col_b;
            rowBegin = row_b;
            colEnd = col_e;
            rowEnd = row_e;
        }
    }

    struct FoundCharacters
    {
        public int col;
        public int row;

        public FoundCharacters(int c, int r)
        {
            col = c;
            row = r;
        }
    }
    class ACWPuzzle
    {
        ACWFile acwFile;

        char[,] array;

        int columns;

        int rows; 

        List<WordToFind> WordToFindList = new List<WordToFind>();

        List<string> FoundWords = new List<string>();

        List<FoundCharacters> FoundCharacters = new List<FoundCharacters>();

        List<FoundCharacters> InvalidSelection = new List<FoundCharacters>();

        // Default constructor ACWPuzzle
        public ACWPuzzle()
        {
        }

        // Constructor
        public ACWPuzzle(string fileName)
        {
            acwFile = new ACWFile(fileName);
        }

        public bool InitializeACW()
        {
            bool bValidation = true;
            bValidation = acwFile.ReadPuzzleFile(); //Read puzzle file
            if (!bValidation)
                return bValidation; // File is invalid.

            bValidation = acwFile.ValidateFile();
            if (!bValidation)
                return bValidation;

            List<int> rowColumns = acwFile.GetRowsColumns();
            array = new char[rowColumns[0], rowColumns[1]]; // Create array with m rows and n columns
            return bValidation;
        }

        public void Processing()
        {
            List<int> rowColumns = acwFile.GetRowsColumns();
            columns = rowColumns[0];
            rows = rowColumns[1];
            // Fill 2D array with random letters
            for (int i = 0; i < rowColumns[0]; ++i)
            {
                for(int j = 0; j < rowColumns[1]; ++j)
                {
                    array[i, j] = RandomLetter.GetLetter();
                }
            }

            int index = 0;
            // Fill 2D array with words to find
            List<WordFormat> words = acwFile.GetWordsToFind();
            foreach(WordFormat w in words)
            {
                string wordToFind = w.wordToFind;
                string direction = (w.direction).ToLower();
                int col = w.column;
                int row = w.row;
                int col_end = col;
                int row_end = row;
                if(direction == "right")
                {
                    for(int i = 0; i < wordToFind.Length; ++i)
                    {
                        array[i + col, row] = wordToFind[i];
                        col_end = i + col;
                        row_end = row;
                    }
                }
                else if(direction == "left")
                {
                    for(int i = 0; i < wordToFind.Length; ++i)
                    {
                        array[col - i, row] = wordToFind[i];
                        col_end = col - i;
                        row_end = row;
                    }
                }
                else if(direction == "down")
                {
                    for(int i = 0; i < wordToFind.Length; ++i)
                    {
                        array[col, row + i] = wordToFind[i];
                        col_end = col;
                        row_end = row + i;
                    }
                }
                else if(direction == "up")
                {
                    for(int i = 0; i < wordToFind.Length; ++i)
                    {
                        array[col, row - i] = wordToFind[i];
                        col_end = col;
                        row_end = row - i;
                    }
                }
                else if(direction == "leftup")
                {
                    for(int i = 0; i < wordToFind.Length; ++i)
                    {
                        array[col - i, row - i] = wordToFind[i];
                        col_end = col - i;
                        row_end = row - i;
                    }
                }
                else if(direction == "rightup")
                {
                    for (int i = 0; i < wordToFind.Length; ++i)
                    {
                        array[col + i, row - i] = wordToFind[i];
                        col_end = col + i;
                        row_end = row - i;
                    }
                }
                else if (direction == "leftdown")
                {
                    for(int i = 0; i < wordToFind.Length; ++i)
                    {
                        array[col - i, row + i] = wordToFind[i];
                        col_end = col - i;
                        row_end = row + i;
                    }
                }
                else if(direction == "rightdown")
                {
                    for(int i = 0; i < wordToFind.Length; ++i)
                    {
                        array[col + i, row + i] = wordToFind[i];
                        col_end = col + i;
                        row_end = row + i;
                    }
                }

                WordToFindList.Add(new WordToFind(index, wordToFind, col, row, col_end, row_end));
                ++index;
            }
        }

        public void DisplayUserSelection(bool customMessages = false, bool bFound = false, string foundWord = "")
        {
            Console.Clear();
            if(true == bFound)
            {
                Console.WriteLine("Congratulations, you found {0}", foundWord);
            }
            Console.Write("\t");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            for(int i = 0; i < columns; ++i)
            {
                Console.Write(i + "\t");
            }
            Console.ResetColor();
            Console.WriteLine("");
            for(int i = 0; i < rows; ++i)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(i + "\t");
                Console.ResetColor();
                for(int j = 0; j < columns; ++j)
                {
                    // Check if character is found character
                    foreach(FoundCharacters c in FoundCharacters)
                    {
                        if(c.col == j && c.row == i)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                        }
                    }

                    // Check if character is in InvalidCharacters
                    foreach(FoundCharacters c in InvalidSelection)
                    {
                        if(c.col == j && c.row == i)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                        }
                    }
                    Console.Write(array[j,i] + "\t");
                    Console.ResetColor();
                }
                Console.WriteLine("");
            }

            Console.WriteLine("");
            Console.WriteLine("Words To Find");
            foreach(WordToFind word in WordToFindList)
            {
                if(FoundWords.Contains(word.wordToFind))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                }
                Console.WriteLine(word.wordToFind);
                Console.ResetColor();
            }
        }

        public void GetUserInputAndProcess()
        {
            int user_colb, user_rowb, user_cole, user_rowe;
            string userColumnBegin,userRowBegin,userColumnEnd,userRowEnd;
            Console.WriteLine("");
            Console.WriteLine("Please enter start column");
            Console.WriteLine("Enter a number from 0 to {0} inclusive",columns - 1);
            userColumnBegin = Console.ReadLine();
            user_colb = Convert.ToInt32(userColumnBegin);

            Console.WriteLine("Please enter start row");
            Console.WriteLine("Enter a number from 0 to {0} inclusive",rows - 1);
            userRowBegin = Console.ReadLine();
            user_rowb = Convert.ToInt32(userRowBegin);

            Console.WriteLine("Please enter end column");
            Console.WriteLine("Enter a number from 0 to {0} inclusive", columns - 1);
            userColumnEnd = Console.ReadLine();
            user_cole = Convert.ToInt32(userColumnEnd);

            Console.WriteLine("Please enter end row");
            Console.WriteLine("Enter a number from 0 to {0} inclusive",rows - 1);
            userRowEnd = Console.ReadLine();
            user_rowe = Convert.ToInt32(userRowEnd);

            // Do not save invalid selections for long duration
            InvalidSelection.Clear();

            bool bFound = false;
            string foundWord = string.Empty;
            for (int i = 0; i < WordToFindList.Count; ++i)
            {
                WordToFind w = WordToFindList[i];
                if (w.colBegin == user_colb && w.rowBegin == user_rowb && w.colEnd == user_cole && w.rowEnd == user_rowe)
                {
                    bFound = true;
                    foundWord = w.wordToFind;
                    FoundWords.Add(foundWord);

                    // Populate the found characters
                    if(w.rowBegin == w.rowEnd && w.colBegin < w.colEnd)
                    {
                        // Right
                        for(int c= w.colBegin; c <= w.colEnd; ++c)
                        {
                            FoundCharacters.Add(new FoundCharacters(c, w.rowBegin));
                        }
                    }
                    else if(w.rowBegin == w.rowEnd && w.colBegin > w.colEnd)
                    {
                        // Left
                        for(int c = w.colBegin; c >= w.colEnd; --c)
                        {
                            FoundCharacters.Add(new FoundCharacters(c, w.rowBegin));
                        }
                    }
                    else if(w.colBegin == w.colEnd && w.rowBegin < w.rowEnd)
                    {
                        // down
                        for(int c = w.rowBegin; c <= w.rowEnd; ++c)
                        {
                            FoundCharacters.Add(new FoundCharacters(w.colBegin, c));
                        }
                    }
                    else if(w.colBegin == w.colEnd && w.rowBegin > w.rowEnd)
                    {
                        // up
                        for(int c = w.rowBegin; c >= w.rowEnd; --c)
                        {
                            FoundCharacters.Add(new FoundCharacters(w.colBegin,c));
                        }
                    }
                    else if(w.colBegin > w.colEnd && w.rowBegin > w.rowEnd)
                    {
                        // left up
                        for(int c = 0; w.colBegin - c >= w.colEnd; ++c)
                        {
                            FoundCharacters.Add(new FoundCharacters(w.colBegin - c, w.rowBegin - c));
                        }
                    }
                    else if(w.colBegin > w.colEnd && w.rowBegin < w.rowEnd)
                    {
                        // leftdown
                        for(int c = 0; w.colBegin - c >= w.colEnd; ++c)
                        {
                            FoundCharacters.Add(new FoundCharacters(w.colBegin - c, w.rowBegin + c));
                        }
                    }
                    else if(w.colBegin < w.colEnd && w.rowBegin > w.rowEnd)
                    {
                        //rightup
                        for(int c = 0; w.colBegin + c <= w.colEnd; ++c)
                        {
                            FoundCharacters.Add(new FoundCharacters(w.colBegin + c, w.rowBegin - c));
                        }
                    }
                    else if(w.colBegin < w.colEnd && w.rowBegin < w.rowEnd)
                    {
                        // rightdown
                        for(int c = 0; w.colBegin + c <= w.colEnd; ++c)
                        {
                            FoundCharacters.Add(new FoundCharacters(w.colBegin + c, w.rowBegin + c));
                        }
                    }
                    break;
                }
            }
            if(!bFound)
            {
                    int iterations_col = 0, iterations_row = 0;
                    // Word not found list 'Red'. Store invalid values in variable.
                    // Fill coordinates to display in red in InvalidSelection
                    if (user_colb < user_cole && user_rowb < user_rowe)
                    {
                        for (int c = user_colb; c <= user_cole; ++c)
                        {
                            iterations_col += 1;
                            InvalidSelection.Add(new FoundCharacters(c, user_rowb));
                        }
                        for (int c = user_rowb; c <= user_rowe; ++c)
                        {
                            iterations_row += 1;
                            InvalidSelection.Add(new FoundCharacters(user_cole, c));
                        }

                        if (iterations_col == iterations_row)
                        {
                            // Direct path exist. Create direct path
                            InvalidSelection.Clear();
                            for (int c = 0; c < iterations_col; ++c)
                            {
                                InvalidSelection.Add(new FoundCharacters(user_colb + c, user_rowb + c));
                            }
                        }
                    }
                    else if (user_colb < user_cole && user_rowb > user_rowe)
                    {
                        for (int c = user_colb; c <= user_cole; ++c)
                        {
                            iterations_col += 1;
                            InvalidSelection.Add(new FoundCharacters(c, user_rowb));
                        }
                        for (int c = user_rowb; c >= user_rowe; --c)
                        {
                            iterations_row += 1;
                            InvalidSelection.Add(new FoundCharacters(user_cole, c));
                        }
                        if (iterations_col == iterations_row)
                        {
                            // Direct path exist. Create direct path
                            InvalidSelection.Clear();
                            for (int c = 0; c < iterations_col; ++c)
                            {
                                InvalidSelection.Add(new FoundCharacters(user_colb + c, user_rowb - c));
                            }
                        }
                    }
                    else if (user_colb > user_cole && user_rowb < user_rowe)
                    {
                        for (int c = user_colb; c >= user_cole; --c)
                        {
                            iterations_col += 1;
                            InvalidSelection.Add(new FoundCharacters(c, user_rowb));
                        }
                        for (int c = user_rowb; c <= user_rowe; ++c)
                        {
                            iterations_row += 1;
                            InvalidSelection.Add(new FoundCharacters(user_cole, c));
                        }
                        if (iterations_col == iterations_row)
                        {
                            // Direct path exist. Create direct path
                            InvalidSelection.Clear();
                            for (int c = 0; c < iterations_col; ++c)
                            {
                                InvalidSelection.Add(new FoundCharacters(user_colb - c, user_rowb + c));
                            }
                        }
                    }
                    else if (user_colb > user_cole && user_rowb > user_rowe)
                    {
                        for (int c = user_colb; c >= user_cole; --c)
                        {
                            iterations_col += 1;
                            InvalidSelection.Add(new FoundCharacters(c, user_rowb));
                        }
                        for (int c = user_rowb; c >= user_rowe; --c)
                        {
                            iterations_row += 1;
                            InvalidSelection.Add(new FoundCharacters(user_cole, c));
                        }
                        if (iterations_col == iterations_row)
                        {
                            // Direct path exist. Create direct path
                            InvalidSelection.Clear();
                            for (int c = 0; c < iterations_col; ++c)
                            {
                                InvalidSelection.Add(new FoundCharacters(user_colb - c, user_rowb - c));
                            }
                        }
                    }
            }

            DisplayUserSelection(true, bFound, foundWord);

            List<WordFormat> words = acwFile.GetWordsToFind();
            if (!(FoundWords.Count == words.Count))
            {
                GetUserInputAndProcess();
            }
            else
            {
                // Congratulate the user
                Console.WriteLine("Congratulations, you found all the words");
            }
        }
    }
}
