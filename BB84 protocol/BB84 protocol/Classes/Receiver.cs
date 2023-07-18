using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB84_protocol.Classes
{
    class Receiver
    {
        public List<Data> data = new();
        public List<Data> shortenData = new();
        double error;

        public Receiver(double Error)
        {
            error = Error / 100;
        }

        // Method to measure received data from Sender (Alice) and simulate Receiver (Bob) actions
        // Every time the Receiver (Bob) randomly chooses the base,
        // If the base is the same as the received data and no error occurs, the measured data is saved correctly.
        // If the base is not the same, the measured state is random.
        // If there is an error, the base is saved as it has been chosen,
        // but the state is measured as if it were the opposite.
        public void Measure(List<Data> receivedData)
        {
            bool baseReceiver;
            Random rand = new();

            for (int i = 0; i < receivedData.Count; i++)
            {
                // Randomly choose a base:
                baseReceiver = rand.Next(2) == 0;

                // When there is no error
                if (rand.NextDouble() >= error)
                {
                    if (receivedData[i].dataBase == baseReceiver)
                        data.Add(new Data(baseReceiver, receivedData[i].dataState));
                    else
                        data.Add(new Data(baseReceiver, rand.Next(2) == 1));
                }
                // When there is an error
                else
                {
                    if (receivedData[i].dataBase != baseReceiver)
                        data.Add(new Data(baseReceiver, receivedData[i].dataState));
                    else
                        data.Add(new Data(baseReceiver, rand.Next(2) == 1));
                }
            }
        }

        // Same method as above, but with a probability that data will be measured
        public void Measure(List<Data> receivedData, double probability)
        {
            bool baseReceiver;
            probability /= 100;
            Random rand = new();

            for (int i = 0; i < receivedData.Count; i++)
            {
                // Checks the probability that data has been measured
                if (rand.NextDouble() <= probability)
                {
                    // Randomly choose a base
                    baseReceiver = rand.Next(2) == 0;

                    // When there is no error
                    if (rand.NextDouble() >= error)
                    {
                        if (receivedData[i].dataBase == baseReceiver)
                            data.Add(new Data(baseReceiver, receivedData[i].dataState));
                        else
                            data.Add(new Data(baseReceiver, rand.Next(2) == 1));
                    }
                    // When there is an error
                    else
                    {
                        if (receivedData[i].dataBase != baseReceiver)
                            data.Add(new Data(baseReceiver, receivedData[i].dataState));
                        else
                            data.Add(new Data(baseReceiver, rand.Next(2) == 1));
                    }
                }
                else
                    data.Add(null); // Data was not measured (null represents this case)
            }
        }
    }
}
