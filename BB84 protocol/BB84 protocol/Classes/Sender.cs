using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB84_protocol.Classes
{
    class Sender
    {
        public List<Data> data = new();
        public List<Data> shortenData = new();
        double error;

        public Sender(double Error)
        {
            error = Error / 100;
        }

        // Method that simulates Sender (Alice) behavior
        // First, the Sender generates data. Then, if there is an error:
        // Changes the base, and the state is randomly generated.
        public List<Data> Send(List<Data> receivedData)
        {
            Random rand = new();
            data = receivedData;
            List<Data> sentData = new();

            // Here, the program checks if there is an error for every single data
            foreach (Data d in receivedData)
            {
                if (rand.NextDouble() < error)
                    sentData.Add(new Data(!d.dataBase, rand.Next(2) == 1));
                else
                    sentData.Add(new Data(d.dataBase, d.dataState));
            }
            return sentData;
        }
    }
}
