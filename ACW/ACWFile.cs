using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACW
{
    struct WordFormat
    {
        public string wordToFind;
        public int column;
        public int row;
        public string direction;

        public WordFormat(string word, int c, int r, string dir)
        {
            wordToFind = word;
            column = c;
            row = r;
            direction = dir;
        }
    }

    class ACWFile
    {
        string fileName = string.Empty;

        // storage for wordtofind
        List<WordFormat> words = new List<WordFormat>();

        // Storage for first line of puzzle
        List<int> firstLine = new List<int>();

        // Default constructor
        public ACWFile() { }

        // Constructor
        public ACWFile(string fName)
        {
            fileName = fName;
        }

        public bool ReadPuzzleFile()
        {
            if (string.IsNullOrEmpty(fileName))
                return false;

            string[] lines = System.IO.File.ReadAllLines(fileName);
            bool readingFirstLine = true;
            foreach(string line in lines)
            {
                if(readingFirstLine)
                {
                    // read first line
                    readingFirstLine = false;

                    string[] strList = line.Split(',');

                    if (!(strList.Length == 3))
                        return false;  // Validation for 3 fields
                    
                    foreach(string str in strList)
                    {
                        firstLine.Add(Convert.ToInt32(str));
                    }
                }
                else
                {
                    string[] strList = line.Split(',');
                    WordFormat word;

                    if (!(strList.Length == 4))
                        return false; // Validation for 4 fields

                    words.Add(new WordFormat(strList[0], Convert.ToInt32(strList[1]), Convert.ToInt32(strList[2]), strList[3]));
                }
            }
            return true;
        }

        public bool ValidateFile()
        {
            //Perform post validation of file
            bool bValidationSuccessful = true;

            // Test if first line contains any negative number
            foreach(int val in firstLine)
            {
                if(val < 0)
                {
                    bValidationSuccessful = false;
                    return bValidationSuccessful;
                }
            }

            return bValidationSuccessful;
        }

        public List<int> GetRowsColumns()
        {
            List<int> rowColumn = new List<int>();
            rowColumn.Add(firstLine[0]);
            rowColumn.Add(firstLine[1]);
            return rowColumn;
        }

        public List<int> GetFirstLine()
        {
            return firstLine;
        }

        public List<WordFormat> GetWordsToFind()
        {
            return words; // to return the 
        }
    }
}
