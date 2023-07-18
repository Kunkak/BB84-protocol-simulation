using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB84_protocol.Classes
{
    class Data
    {
        public bool dataBase;
        public bool dataState;

        // Constructor to initialize the data with both base and state
        public Data(bool Base, bool State)
        {
            dataBase = Base;
            dataState = State;
        }

        // Constructor to initialize the data with only the state
        // This constructor is used for shortening the data
        public Data(bool State)
        {
            dataState = State;
        }

        // Method that calculates QBER (Quantum Bit Error Rate)
        public static double QBER(List<Data> A, List<Data> B)
        {
            int corrBases = 0;
            int corrResults = 0;

            for (int i = 0; i < A.Count; i++)
            {
                // Checks if data was read by both A and B
                if (A[i] != null && B[i] != null)
                {
                    // If bases match, increment the counter for matching bases
                    if (A[i].dataBase == B[i].dataBase)
                    {
                        corrBases++;
                        // If states and bases match, increment the counter for matching results
                        if (A[i].dataState == B[i].dataState)
                            corrResults++;
                    }
                }
            }
            // Returns 1 minus the ratio of matching bases to matching everything (%)
            return 100 * ((double)1 - (double)corrResults / corrBases);
        }

        // Method that shortens data by XORing 2 neighboring states between Sender A and Receiver B
        public static void Shorten(Sender A, Receiver B)
        {
            List<bool> a = new();
            List<bool> b = new();
            int dlugosc = A.data.Count;

            for (int i = 0; i < dlugosc; i++)
            {
                if (A.data[i].dataBase == B.data[i].dataBase)
                {
                    a.Add(A.data[i].dataState);
                    b.Add(B.data[i].dataState);
                }
            }
            dlugosc = a.Count;
            for (int i = 0; i < dlugosc - 1; i += 2)
            {
                A.shortenData.Add(new Data(a[i] != a[i + 1]));
                B.shortenData.Add(new Data(b[i] != b[i + 1]));
            }
        }

        // Method that shortens data by XORing 2 neighboring states between Sender A, Receiver B, and Hacker C
        public static void Shorten(Sender A, Receiver B, Hacker C)
        {
            List<bool> a = new();
            List<bool> b = new();
            List<Data> c = new();

            for (int i = 0; i < A.data.Count - 1; i++)
            {
                if (A.data[i].dataBase == B.data[i].dataBase)
                {
                    a.Add(A.data[i].dataState);
                    b.Add(B.data[i].dataState);

                    if (C.data[i] != null && A.data[i].dataBase == C.data[i].dataBase)
                        c.Add(new Data(C.data[i].dataState));
                    else
                        c.Add(null);
                }
            }
            for (int i = 0; i < a.Count - 1; i += 2)
            {
                A.shortenData.Add(new Data(a[i] != a[i + 1]));
                B.shortenData.Add(new Data(b[i] != b[i + 1]));
                if (c[i] != null && c[i + 1] != null)
                    C.shortenData.Add(new Data(c[i].dataState != c[i + 1].dataState));
                else
                    C.shortenData.Add(null);
            }
        }

        // Method that shortens data by XORing 2 random states
        // (performance-intensive method, may slow down the program significantly)
        public static void ShortenRandomly(Sender A, Receiver B, Hacker C)
        {
            List<bool> a = new();
            List<bool> b = new();
            List<Data> c = new();
            Random rand = new();

            // Add data to the lists where bases match between Sender and Receiver
            for (int i = 0; i < A.data.Count - 1; i++)
            {
                if (A.data[i].dataBase == B.data[i].dataBase)
                {
                    a.Add(A.data[i].dataState);
                    b.Add(B.data[i].dataState);

                    // If the hacker's base doesn't match, there is no point in storing his result
                    if (C.data[i] != null && A.data[i].dataBase == C.data[i].dataBase)
                        c.Add(new Data(C.data[i].dataState));
                    else
                        c.Add(null);
                }
            }
            while (a.Count > 1)
            {
                // Take a random element from the lists
                int j = rand.Next(a.Count);
                bool first_a = a[j], first_b = b[j];
                Data first_c = c[j];

                // Delete the taken element from the lists
                a.RemoveAt(j);
                b.RemoveAt(j);
                c.RemoveAt(j);
                j = rand.Next(a.Count);

                // XOR the random element with the taken element
                A.shortenData.Add(new Data(first_a != a[j]));
                B.shortenData.Add(new Data(first_b != b[j]));
                if (first_c != null && c[j] != null)
                    C.shortenData.Add(new Data(first_c.dataState != c[j].dataState));
                else
                    C.shortenData.Add(null);

                // Delete the second element from the list
                a.RemoveAt(j);
                b.RemoveAt(j);
                c.RemoveAt(j);
            }
        }
    }
}
