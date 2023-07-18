using System;
using System.Collections.Generic;
using BB84_protocol.Classes;
using System.IO;
using System.Diagnostics;

namespace BB84_protocol
{
    class Program
    {
        static void Main()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            // Path to the file containing prepared random bits for states generation.
            // If you want to use the C# Random class instead, leave this as an empty string.
            string randomDataPath = "";

            // Choose the strategy that the hacker will use:
            // 1 - Believe what you read
            // 2 - Random base and preserved state
            // 3 - Random data
            // 4 - Always the same data
            // 0 - Don't steal anything (it's rude)
            int strategy = 0;

            // Input data
            int totalDataSize = 10000000;
            double aliceError = 0.1; // Error rate for Alice
            double bobError = 0.1; // Error rate for Bob
            double eveError = 0.1; // Error rate for Eve
            double fiberError = 0.1; // Error rate for optical fiber
            double eveStealingProbability = 100; // Probability that data will be stolen (%)

            // Object creation
            List<Data> data = Generator.GenerateFromFile(totalDataSize, randomDataPath);
            Sender alice = new Sender(aliceError);
            Receiver bob = new Receiver(bobError);
            OptFiber fiber = new OptFiber(fiberError / 2);
            Hacker eve = new Hacker(eveError);

            // Looking for errors
            if (data == null)
                return;

            // Data transmission
            data = alice.Send(data);
            data = fiber.RunThrough(data);
            data = eve.Steal(data, eveStealingProbability, strategy);
            data = fiber.RunThrough(data);
            bob.Measure(data);
            Data.Shorten(alice, bob, eve);

            // Write results to console/file
            StreamWriter sw = File.AppendText("Output.txt");
            // sw.WriteLine(         // Comment out this line if you want to print to the console
            Console.WriteLine(      // Comment out this line if you want to print to the file
                $"Data size: {totalDataSize}\n" +
                $"Alice's error: {aliceError}%\nBob's error: {bobError}%\n" +
                $"Eve's error: {eveError}%\nOptical fiber error: {fiberError}%\n\n" +

                $"Eve's strategy: {eve.Strategy(strategy)}\n\n" +

                $"Data stolen: {eve.HowMuchStolen()}%\n" +
                $"QBER Eve (before shorten): {Data.QBER(alice.data, eve.data)}%\n" +
                $"QBER Bob (before shorten): {Data.QBER(alice.data, bob.data)}%\n\n" +

                $"After data shorten, Eve has {eve.HowMuchStolenShorten()}% of the data\n" +
                $"QBER Eve (after shorten): {Data.QBER(alice.shortenData, eve.shortenData)}%\n" +
                $"QBER Bob (after shorten): {Data.QBER(alice.shortenData, bob.shortenData)}%\n");

            sw.Close();
            stopwatch.Stop();
            Console.Write($"Simulation lasted: {(double)stopwatch.ElapsedMilliseconds / 1000}s\n");
        }
    }
}
