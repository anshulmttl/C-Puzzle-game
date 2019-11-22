using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACW
{
    class Program
    {
        static void Main(string[] args)
        {
            string userSelection = string.Empty;
            Console.WriteLine("Would you like to load wordsearch from file? (y / n)");
            userSelection = Console.ReadLine();
            int fileToLoad;
            if(userSelection == "y" || userSelection == "Y")
            {
                string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(),"*.wrd");
                Console.WriteLine("Select a file to load");
                Console.WriteLine("Total files {0}",files.Length);
                for (int i = 0; i < files.Length; ++i)
                {
                    Console.WriteLine("{0}. {1}",i + 1, files[i]);
                }

                string fileSelection = string.Empty;
                fileSelection = Console.ReadLine();
                fileToLoad = Convert.ToInt32(fileSelection) - 1; // File to load for puzzle

                bool bValidateResult = true;
                ACWPuzzle acwPuzzle = new ACWPuzzle(files[fileToLoad]);
                bValidateResult = acwPuzzle.InitializeACW();
                if (bValidateResult)
                {
                    acwPuzzle.Processing();
                    acwPuzzle.DisplayUserSelection();
                    acwPuzzle.GetUserInputAndProcess();
                }
                else
                {
                    Console.WriteLine("Selected file is in incorrect format");
                }
            }
        }
    }
}
