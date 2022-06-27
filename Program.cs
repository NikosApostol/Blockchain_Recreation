using System;
using System.Linq;
using System.Text;

namespace MyConsoleApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string answer;
            var dataTransaction = CreateDictionary();
            Console.WriteLine("Hello there! Please choose one option from the following menu.");
            do
            {
                Console.WriteLine("1. Create a new BlockChain");
                Console.WriteLine("2. Add to an existing BlockChain");
                Console.WriteLine("3. Delete a certain block from an existing BlockChain");
                int.TryParse(Console.ReadLine(), out int choise);

                switch (choise)
                {
                    case 1:
                        {
                            string folderName;
                            do
                            {
                                Console.WriteLine("Please specify the name of the folder you want the blockchain to be placed in.");
                                folderName = Console.ReadLine().Trim();
                            } while (folderName == "");
                            if (CreateBlockchain(folderName, choise, dataTransaction))
                                Console.WriteLine("New blockchan created successfully!");
                            break;
                        }
                    case 2:
                        {
                            string folderName;
                            do
                            {
                                Console.WriteLine("Please specify the name of the folder where the blockchain exists.");
                                folderName = Console.ReadLine().Trim();
                            } while (folderName == "");
                            if (CreateBlockchain(folderName, choise, dataTransaction))
                                Console.WriteLine("A new block was added successfully.");
                            break;
                        }
                    case 3:
                        {
                            string folderName;
                            do
                            {
                                Console.WriteLine("Please specify the name of the folder from where you want a certain block to be deleted.");
                                folderName = Console.ReadLine().Trim();
                            } while (folderName == "");
                            if (CreateBlockchain(folderName, choise, dataTransaction))
                                Console.WriteLine("Block deleted successfully!");
                            break;
                        }
                }

                do
                {
                    Console.WriteLine("Do you want to continue? Press 'Y' if yes or 'N' if no.");
                    answer = Console.ReadLine().ToUpper().Trim();
                } while (!(answer == "Y" || answer == "N"));

            } while (answer == "Y");
        }

        public static bool CreateBlockchain(string blockChainFolder, int choise, Dictionary<int, string> transactionData)
        {
            bool operationCompleted = true;
            var filePaths = new List<string>();
            (string fullFilePath, string blockName, string blockNumber) = ("", "", "");
            string direcrory = $@"C:\{blockChainFolder}";

            if (choise == 1)
            {
                if (!Directory.Exists(direcrory))
                {
                    Directory.CreateDirectory(direcrory);
                    fullFilePath = direcrory + @"\1_Block.txt";
                    CreateBlockFile(fullFilePath, "", true, transactionData);
                }
                else
                {
                    Console.WriteLine("This folder already exists");
                    operationCompleted = false;
                }
            }
            else if (choise == 2)
            {
                if (Directory.Exists(direcrory))
                {
                    filePaths = Directory.GetFiles(direcrory, "*.txt").ToList();
                    if (!CheckBlockchainValidity(direcrory, filePaths))
                        return false;
                    string lastFilePath = filePaths.Last();
                    var number = FindBlockNumber(direcrory, lastFilePath);
                    number++;
                    fullFilePath = direcrory + @"\" + number + "_Block.txt";
                    CreateBlockFile(fullFilePath, lastFilePath, false, transactionData);
                }
                else
                {
                    Console.WriteLine("This folder does not exist");
                    operationCompleted = false;
                }
            }
            else
            {
                if (Directory.Exists(direcrory))
                {
                    filePaths = Directory.GetFiles(direcrory, "*.txt").ToList();
                    if (!CheckBlockchainValidity(direcrory, filePaths))
                        return false;
                    Console.WriteLine("This blockchain contains: " + filePaths.Count + " blocks. Which one do you want to delete?");
                    int.TryParse(Console.ReadLine(), out int blockOfChoise);
                    var kot = filePaths.Where(x => x.Contains(blockOfChoise+"_Block")).First();
                    File.Delete(kot);
                }
                else
                {
                    Console.WriteLine("This folder does not exist");
                    operationCompleted = false;
                }
            }
            if(operationCompleted)
                ProofOfWork();
            return operationCompleted;
        }

        public static void CreateBlockFile(string filePath, string lastFilePath, bool isGenesisBlockBool, Dictionary<int, string> transactionData)
        {
            byte[] hashOfPreviousBlock;
            byte[] hash;
            byte[] data;
            using (FileStream fs = File.Create(filePath))
            { 
                hash = new UTF8Encoding(true).GetBytes(CreateHashCodeOfBlock());
                fs.Write(hash, 0, hash.Length);

                hashOfPreviousBlock = isGenesisBlockBool ? new UTF8Encoding(true).GetBytes("0000 ") : new UTF8Encoding(true).GetBytes(GetPreviousHashCode(lastFilePath));
                fs.Write(hashOfPreviousBlock, 0, hashOfPreviousBlock.Length);

                Random random = new Random();
                int id = random.Next(1, 5);
                data = new UTF8Encoding(true).GetBytes(GetTransactionData(id, transactionData));
                fs.Write(data, 0, data.Length);
            }
        }

        public static bool CheckBlockchainValidity(string direcrory, List<string> filePaths)
        {
            var counterOfBlocks = 0;
            foreach (var block in filePaths)
            {
                var number1 = FindBlockNumber(direcrory, block);
                counterOfBlocks++;
                if (number1 != counterOfBlocks)
                {
                    Console.WriteLine("This blockchain is corrupted.");
                    return false;
                }
            }
            return true;
        }

        public static int FindBlockNumber(string direcrory, string filePath)
        {
            string pattern = @"\\";
            string[] elements = System.Text.RegularExpressions.Regex.Split(filePath, pattern);
            var blockName = elements.Last();
            string pattern2 = "_";
            string[] elements2 = System.Text.RegularExpressions.Regex.Split(blockName, pattern2);
            var blockNumber = elements2.First();
            int.TryParse(blockNumber, out int number);
            return number;
        }

        public static string GetTransactionData(int id, Dictionary<int, string> transactionData)
        {
            return transactionData.Where(x => x.Key == id).First().Value;
        }

        public static string GetPreviousHashCode(string filePath)
        {
            string text = File.ReadAllText(filePath);
            string pattern = " ";
            string[] elements = System.Text.RegularExpressions.Regex.Split(text, pattern);
            string hashCode = elements.First();

            return hashCode + " ";
        }

        public static string CreateHashCodeOfBlock()
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

        public static void ProofOfWork()
        {
            DateTime now = DateTime.Now;
            while (DateTime.Now.Subtract(now).Seconds < 5)
            {
                // wait for 5 seconds
            }
        }

        public static Dictionary<int, string> CreateDictionary()
        {
            IDictionary<int, string> transactionData = new Dictionary<int, string>();
            transactionData.Add(1, "Alice gave Bill 5$");
            transactionData.Add(2, "Bill gave Nick 15$");
            transactionData.Add(3, "Nick gave Tzenh 7$");
            transactionData.Add(4, "Tzenh gave Alice 29$");
            transactionData.Add(5, "Jim gave Nick 759$");

            return (Dictionary<int, string>)transactionData;
        }
    }
}