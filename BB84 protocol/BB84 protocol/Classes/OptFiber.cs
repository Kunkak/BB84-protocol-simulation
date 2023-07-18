using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB84_protocol.Classes
{
    class OptFiber
    {
        double error;

        public OptFiber(double Error)
        {
            error = Error / 100;
        }

        // Method to simulate the optical fiber's behavior
        // The optical fiber can sometimes alter the base of the data.
        public List<Data> RunThrough(List<Data> receivedData)
        {
            Random rand = new();
            foreach (var d in receivedData)
            {
                // If there is an error, the base changes and the state is random.
                if (rand.NextDouble() < error)
                {
                    d.dataBase = !d.dataBase;
                    d.dataState = rand.Next(2) == 1;
                }
            }
            return receivedData;
        }
    }
}
