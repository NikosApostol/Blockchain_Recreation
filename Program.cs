using System;
using System.Text;

namespace MyConsoleApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1. Create a new BlockChain");
            Console.WriteLine("2. Add to an existing BlockChain");
            Int32.TryParse(Console.ReadLine(),out int choise);

            switch (choise)
            {
                case 1:
                    {
                        Console.WriteLine("Please specify the name of the folder");
                        string folderName = Console.ReadLine();
                        CreateBlockchain(folderName,choise);
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("Please specify the name of the folder");
                        string folderName = Console.ReadLine();
                        CreateBlockchain(folderName, choise);
                        break;
                    }
                default:
                    break;
            }
        }


        public static void CreateBlockchain(string blockChainFolder, int choise)
        {
            var filePaths = new List<string>();
            (string fileName, string blockName, string blockNumber) = ("", "", "");

            string direcrory = $@"C:\C#_Projects\{blockChainFolder}";            
            if (choise == 1)
            {
                Directory.CreateDirectory(direcrory);
                fileName = direcrory + @"\GenesisBlock.txt";
            }
            else
            {
                filePaths = Directory.GetFiles(direcrory, "*.txt").ToList();
                string lastFilePath = filePaths.Last();
                String pattern = @"\\";
                String[] elements = System.Text.RegularExpressions.Regex.Split(lastFilePath, pattern);
                blockName = elements.Last();
                if (blockName == "GenesisBlock.txt")                
                    fileName = "Block_1.txt";                
                else
                {
                    String pattern2 = "_";
                    String[] elements2 = System.Text.RegularExpressions.Regex.Split(lastFilePath, pattern2);
                    blockNumber = elements.Last();
                    Int32.TryParse(blockNumber, out int number);
                    number++;
                    fileName = "Block_" + number +".txt";
                }
            }

            //if (!Directory.Exists(direcrory))            
            //    Directory.CreateDirectory(direcrory);
            //else            
            //    filePaths = Directory.GetFiles(direcrory, "*.txt").ToList();

            //string lastFilePath = filePaths.Last();
            //String pattern = @"\\";
            //String[] elements = System.Text.RegularExpressions.Regex.Split(lastFilePath, pattern);

            //blockName = elements.Last();
            //if (blockName == "GenesisBlock") 
            //{

            //}

               

            // Create a new file     
            using (FileStream fs = File.Create(fileName))
            {
                Byte[] hash = new UTF8Encoding(true).GetBytes(GetHashCode(choise));
                fs.Write(hash, 0, hash.Length);

                byte[] hashOfPreviousBlock = new UTF8Encoding(true).GetBytes("Previous Hash Code ");
                fs.Write(hashOfPreviousBlock, 0, hashOfPreviousBlock.Length);

                byte[] data = new UTF8Encoding(true).GetBytes("Alice gave Bill 5$");
                fs.Write(data, 0, data.Length);
            }
        }
            
        public static string GetHashCode(int choise)
        {
            string hashCode = "";
            if (choise != 1)
                for (int i = 0; i < 4; i++)
                {
                    Random random = new Random();
                    int num = random.Next(0, 10);
                    hashCode = hashCode + num.ToString();
                }
            else
                hashCode = "0000 ";

            return hashCode;
        }
    }
}