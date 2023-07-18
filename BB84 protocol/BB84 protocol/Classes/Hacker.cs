using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB84_protocol.Classes
{
    class Hacker
    {
        // List to store the measured data and shortened data (data after the shorten process)
        public List<Data> data = new();
        public List<Data> shortenData = new();

        // Error rate for the hacker's measurements
        double error;

        public Hacker(double Error)
        {
            error = Error / 100;
        }

        // Main method to choose the strategy and simulate data stealing
        public List<Data> Steal(List<Data> receivedData, double probability, int strategy)
        {
            switch (strategy)
            {
                case 1:
                    return ReadAndSubmit(receivedData, probability);
                case 2:
                    return ReadRandomBase(receivedData, probability);
                case 3:
                    return ReadRandom(receivedData, probability);
                case 4:
                    return ReadSendSame(receivedData, probability);
                case 0:
                    // If the strategy is set to 0, the hacker won't steal anything and returns the received data as is
                    for (int i = 0; i < receivedData.Count; i++)
                        data.Add(null);
                    return receivedData;
                default:
                    // Invalid strategy, won't steal anything and return the received data as is
                    Console.WriteLine("There is no such strategy (doesn't read)");
                    for (int i = 0; i < receivedData.Count; i++)
                        data.Add(null);
                    return receivedData;
            }
        }

        // Hacker sends what he reads (he doesn't know if the base was correct)
        public List<Data> ReadAndSubmit(List<Data> receivedData, double probability)
        {
            Random rand = new();
            List<Data> sentData = new();

            // First, the hacker simply reads data just like the receiver, but you can choose how much data he will read (probability)
            Receiver hackerRead = new(error * 100);
            hackerRead.Measure(receivedData, probability);
            data = hackerRead.data;

            for (int i = 0; i < receivedData.Count; i++)
            {
                // If data was not measured, nothing happens
                if (data[i] == null)
                    sentData.Add(receivedData[i]);
                // If there is an error, the base changes, and the state is random
                else if (rand.NextDouble() < error)
                    sentData.Add(new Data(!data[i].dataBase, rand.Next(2) == 1));
                // If there is no error, the hacker sends what he measured
                else
                    sentData.Add(new Data(data[i].dataBase, data[i].dataState));
            }
            return sentData;
        }

        // Hacker sends entirely random data
        public List<Data> ReadRandom(List<Data> receivedData, double probability)
        {
            List<Data> sentData = new();

            // First, the hacker simply reads data just like the receiver, but you can choose how much data he will read (probability)
            Receiver hackerRead = new(error * 100);
            hackerRead.Measure(receivedData, probability);
            data = hackerRead.data;

            for (int i = 0; i < receivedData.Count; i++)
            {
                // If data was not measured, nothing happens
                if (data[i] == null)
                    sentData.Add(receivedData[i]);
                // If data was measured, he generates random data and sends it
                else
                    sentData.Add(Generator.GenerateRandom());
            }
            return sentData;
        }

        // Hacker sends data with a random base, but he uses the same state as the measured
        public List<Data> ReadRandomBase(List<Data> receivedData, double probability)
        {
            Random rand = new();
            List<Data> sentData = new();

            Receiver hackerRead = new(error * 100);
            hackerRead.Measure(receivedData, probability);
            data = hackerRead.data;

            for (int i = 0; i < receivedData.Count; i++)
            {
                // If data was not measured, nothing happens
                if (data[i] == null)
                    sentData.Add(receivedData[i]);
                // If there is no error, the base is random, and the state is the same as measured
                else if (rand.NextDouble() >= error)
                    sentData.Add(new Data(rand.Next(2) == 1, data[i].dataState));
                // If there is an error, the base is random, and the state is random
                else
                    sentData.Add(new Data(rand.Next(2) == 1, rand.Next(2) == 1));
            }
            return sentData;
        }

        // Hacker sends the same data (1, 1) for all measurements (really bad strategy)
        public List<Data> ReadSendSame(List<Data> receivedData, double probability)
        {
            Random rand = new();
            List<Data> sentData = new();

            Receiver hackerRead = new(error * 100);
            hackerRead.Measure(receivedData, probability);
            data = hackerRead.data;

            for (int i = 0; i < receivedData.Count; i++)
            {
                // If data was not measured, nothing happens
                if (data[i] == null)
                    sentData.Add(receivedData[i]);
                // If there is no error, the hacker sends (true, true)
                else if (rand.NextDouble() >= error)
                    sentData.Add(new Data(true, true));
                // If there is an error, the base is false, and the state is random
                else
                    sentData.Add(new Data(false, rand.Next(2) == 1));
            }
            return sentData;
        }

        // Methods that count how much data was stolen
        public double HowMuchStolen()
        {
            int counter = 0;
            foreach (var b in data)
            {
                if (b != null)
                    counter++;
            }
            return (double)counter / data.Count * 100;
        }

        public double HowMuchStolenShorten()
        {
            int counter = 0;
            foreach (var b in shortenData)
            {
                if (b != null)
                    counter++;
            }
            return (double)counter / shortenData.Count * 100;
        }

        // Helper method to provide a user-friendly string representation of the hacker's chosen strategy
        public string Strategy(int strategy)
        {
            switch (strategy)
            {
                case 1:
                    return "BELIEVE IN YOUR result";
                case 2:
                    return "Random base, state stays the same";
                case 3:
                    return "Random data";
                case 4:
                    return "Always send the same data (1, 1)";
                case 0:
                    return "Doesn't steal (it's rude)";
                default:
                    return "Nothing (wrong input)";
            }
        }
    }
}
