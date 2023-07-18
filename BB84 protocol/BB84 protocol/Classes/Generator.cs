using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BB84_protocol.Classes
{
    class Generator
    {
        // Method that returns a randomly generated list of data of the given length
        public static List<Data> GenerateRandom(int size)
        {
            List<Data> generatedData = new();
            Random rand = new();
            for (int i = 0; i < size; i++)
            {
                generatedData.Add(new Data(rand.Next(2) == 1, rand.Next(2) == 1));
            }
            return generatedData;
        }

        // Method that generates one Data element (base and state)
        public static Data GenerateRandom()
        {
            Random rand = new();
            return new Data(rand.Next(2) == 1, rand.Next(2) == 1);
        }

        // Method that returns a list of data of the given length
        // Data is read from the .dat file which contains 0s and 1s only
        public static List<Data> GenerateFromFile(int size, string randomDataPath)
        {
            if (string.IsNullOrWhiteSpace(randomDataPath))
                return GenerateRandom(size);

            List<Data> data = new(); // List of output data
            List<bool> bits = new(); // List of bits read from the file

            try
            {
                // Check if the file exists and is accessible
                if (!File.Exists(randomDataPath))
                    Console.WriteLine("Error: The specified file does not exist.");

                BinaryReader br = new(File.Open(randomDataPath, FileMode.Open));
                int numberOfBytes = size / 4 + 1; // Number of bytes that have to be read from the file
                for (int i = 0; i < numberOfBytes; i++)
                {
                    // Reads 1 byte and converts it to bits
                    string bitsStr = Convert.ToString(br.ReadByte(), 2);
                    // Correction which adds 0s at the beginning (e.g., 110101 -> 00110101)
                    while (bitsStr.Length < 8)
                        bitsStr = "0" + bitsStr;

                    // Adds read bits to the list
                    foreach (var b in bitsStr)
                        bits.Add(b == '1');
                }
                br.Close();

                // Generates data using the prepared list of bits
                for (int i = 0; i < 2 * size; i += 2)
                    data.Add(new Data(bits[i], bits[i + 1]));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading random data from the file: " + ex.Message);
                return null;
            }

            return data;
        }
    }
}
