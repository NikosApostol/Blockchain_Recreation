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
            (string fileName, string blockName, string blockNumber ) = ("", "", "");

            string direcrory = $@"C:\{blockChainFolder}";      
            
            if (choise == 1)
            {
                if (!Directory.Exists(direcrory))
                {
                    Directory.CreateDirectory(direcrory);
                    fileName = direcrory + @"\1_Block.txt";
                    CreateBlockFile(fileName, "", true);
                }
                else                
                    Console.WriteLine("This folder already exists");                                    
            }
            else
            {
                if (Directory.Exists(direcrory))
                {
                    filePaths = Directory.GetFiles(direcrory, "*.txt").ToList();
                    string lastFilePath = filePaths.Last();
                    String pattern = @"\\";
                    String[] elements = System.Text.RegularExpressions.Regex.Split(lastFilePath, pattern);
                    blockName = elements.Last();
                    if (blockName == "1_Block.txt")
                    {
                        fileName = direcrory + @"\2_Block.txt";
                        CreateBlockFile(fileName, lastFilePath, false);
                    }
                    else
                    {
                        String pattern2 = "_";
                        String[] elements2 = System.Text.RegularExpressions.Regex.Split(lastFilePath, pattern2);
                        blockNumber = elements2.First();
                        String pattern3 = @"\\";
                        String[] elements3 = System.Text.RegularExpressions.Regex.Split(blockNumber, pattern3);
                        blockNumber = elements3.Last();
                        Int32.TryParse(blockNumber, out int number);
                        number++;
                        fileName = direcrory + @"\" + number + "_Block.txt";
                        CreateBlockFile(fileName, lastFilePath, false);
                    }
                }   
                else
                    Console.WriteLine("This folder does not exist");                
            }               
        }

        public static void CreateBlockFile(string fileName, string lastFilePath, bool isGenesisBlockBool)
        {
            byte[] hashOfPreviousBlock;
            byte[] hash;
            byte[] data;
            using (FileStream fs = File.Create(fileName))
            {
                hash = new UTF8Encoding(true).GetBytes(GetHashCode());
                fs.Write(hash, 0, hash.Length);

                hashOfPreviousBlock = isGenesisBlockBool ? new UTF8Encoding(true).GetBytes("0000 ") : new UTF8Encoding(true).GetBytes(GetPreviousHashCode(lastFilePath));
                fs.Write(hashOfPreviousBlock, 0, hashOfPreviousBlock.Length);

                data = new UTF8Encoding(true).GetBytes("Alice gave Bill 5$");
                fs.Write(data, 0, data.Length);
            }
        }

        public static string GetPreviousHashCode(string filePath)
        {
            string hashCode = "";

            string text = File.ReadAllText(filePath);
            string pattern = " ";
            string[] elements = System.Text.RegularExpressions.Regex.Split(text, pattern);
            hashCode = elements.First();

            return hashCode + " ";
        }

        public static string GetHashCode()
        {
            string hashCode = "";
            for (int i = 0; i < 4; i++)
            {
                Random random = new Random();
                int num = random.Next(0, 10);
                hashCode = hashCode + num.ToString();
            }

            return hashCode + " ";
        }

    }
}