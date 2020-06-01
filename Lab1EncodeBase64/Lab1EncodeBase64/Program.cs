using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lab1EncodeBase64
{
    class Program
    {
        public static void CharactProbability(Dictionary<char, double> chrcts, int totalCount)
        {
            //The number of keys in the dictionary
            int countKeysDict = chrcts.Keys.Count;
            char[] keysDict = new char[countKeysDict];
            chrcts.Keys.CopyTo(keysDict, 0);

            for (int iter = 0; iter < countKeysDict; iter++)
            {
                chrcts[keysDict[iter]] /= totalCount;
            }
        }

        public static double AvrgEntropy(Dictionary<char, double> chrcts)
        {
            int countChrct = chrcts.Keys.Count;
            char[] keysDict = new char[countChrct];
            chrcts.Keys.CopyTo(keysDict, 0);
            double probability = 0, entropy = 0;

            for (int iter = 0; iter < countChrct; iter++)
            {
                probability = chrcts[keysDict[iter]];
                entropy -= probability * Math.Log(probability, 2);
            }
            return entropy;
        }

        public static double InfoQuantity(double entrp, int chrctCount)
        {
            return entrp * chrctCount;
        }

        public static void Print(double infoQuant, long fileSize)
        {
            Console.WriteLine("Size of file = {0} bytes", fileSize);
            Console.WriteLine("Info Quantity = {0} bytes", infoQuant / 8);
            Console.WriteLine("Info Quantity = {0} bits\n", infoQuant);
        }

        public static void Print(double infoQuant, long fileSize, string base64Text)
        {
            Console.WriteLine("Size of file = {0} bytes",   fileSize);
            Console.WriteLine("Info Quantity = {0} bytes",  infoQuant / 8);
            Console.WriteLine("Info Quantity = {0} bits\n",   infoQuant);
            Console.WriteLine(base64Text);
            Console.WriteLine();
        }

        public static long ReadFile(string pathFile, Dictionary<char, double> chrcts, out int totalCountChrcts)
        {
            FileInfo fileSize = new FileInfo(pathFile);
            int iterator;
            //Repetition of a character in the text
            double chrctRecur;
            totalCountChrcts = 0;

            string allText = File.ReadAllText(pathFile);
            iterator = 0;
            while (iterator < allText.Length)
            {
                chrctRecur = 1;
                if (!chrcts.ContainsKey(allText[iterator]))
                {
                    chrcts.Add(allText[iterator], chrctRecur);
                }
                else
                    if (chrcts.ContainsKey(allText[iterator]))
                {
                    chrcts[allText[iterator]]++;
                }
                iterator++;
                totalCountChrcts++;
            }
            return fileSize.Length;
        }

        public static void WriteFile(string pathFile, string text)
        {
            using (StreamWriter sw = new StreamWriter(pathFile, false, System.Text.Encoding.UTF8))
            {
                sw.WriteLine(text);
            }
        }

        public static byte[] GetTextInBytes(string pathFile)
        {
            Encoding u8 = Encoding.UTF8;

            string allText = File.ReadAllText(pathFile);
            char[] chars = allText.ToCharArray(0, allText.Length);
            byte[] bytes = u8.GetBytes(chars);

            return bytes;
        }

        public static string CheckEncoding(string pathFile)
        {
            string allText = File.ReadAllText(pathFile);
            string base64Text = Convert.ToBase64String(Encoding.UTF8.GetBytes(allText));

            return base64Text;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            string path = @"C:\Users\kaena\Desktop\CS\CS_labs\Laboratory work 1\TextFiles\";
            string pathToWrite = @"C:\Users\kaena\Desktop\CS\CS_labs\Laboratory work 1\EncryptTextFiles\";
            string pathToCmprssed = @"C:\Users\kaena\Desktop\CS\CS_labs\Laboratory work 1\EncryptTextFiles\";
            string textFile, encryptFile, compressedFile;
            char[] encodeText;
            int totalCountCharacters;
            long fileSize;
            double entropy, infoQuant;
            Dictionary<char, double> characters = new Dictionary<char, double>();
            Base64Encrypt base64;

            try
            {
                Console.Write("Enter the name of text file: ");
                textFile = Console.ReadLine();
                Console.WriteLine();

                Console.Write("Enter the name of crypt file: ");
                encryptFile = Console.ReadLine();
                Console.WriteLine();

                Console.Write("Enter the name of compressed Base64 file: ");
                compressedFile = Console.ReadLine();
                Console.WriteLine();

                path += textFile + ".txt";
                pathToWrite += encryptFile + ".txt";

                fileSize = ReadFile(path, characters, out totalCountCharacters);
                CharactProbability(characters, totalCountCharacters);
                entropy = AvrgEntropy(characters);
                infoQuant = InfoQuantity(entropy, totalCountCharacters);
                Print(infoQuant, fileSize);


                fileSize = ReadFile(pathToWrite, characters, out totalCountCharacters);
                CharactProbability(characters, totalCountCharacters);
                entropy = AvrgEntropy(characters);
                infoQuant = InfoQuantity(entropy, totalCountCharacters);

                base64 = new Base64Encrypt(GetTextInBytes(path));
                encodeText = base64.GetEncoded();

                string encryptRow = new string(encodeText);
                WriteFile(pathToWrite, encryptRow);
                Print(infoQuant, fileSize, CheckEncoding(path));
                
                pathToCmprssed += compressedFile + ".txt.bz2";
                fileSize = ReadFile(pathToCmprssed, characters, out totalCountCharacters);
                CharactProbability(characters, totalCountCharacters);
                entropy = AvrgEntropy(characters);
                infoQuant = InfoQuantity(entropy, totalCountCharacters);
                Print(infoQuant, fileSize);
            }
            catch (FileNotFoundException fnfexc)
            {
                Console.WriteLine(fnfexc.Message);
            }
            catch (IOException ioexc)
            {
                Console.WriteLine(ioexc.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }
    }
}