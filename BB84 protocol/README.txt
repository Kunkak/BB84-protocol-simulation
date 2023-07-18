The BB84 Protocol Simulator is a C# program that simulates the BB84 quantum key distribution protocol.
The BB84 protocol is a quantum cryptographic protocol used to establish a secure communication channel between two parties (Alice and Bob)
over an insecure communication channel, even in the presence of an eavesdropper (Eve).

Features
Simulation of quantum state generation and random bit generation for secure communication.
Error simulation during quantum state transmission.
Implementation of a hacker (Eve) with various strategies to intercept data.
Calculation of Quantum Bit Error Rate (QBER) to assess the security of the communication.
Data shortening by XORing adjacent states to improve efficiency and reduce data loss.
Option to use prepared random bits from a file for states generation.

Requirements
.NET Framework (minimum version: 4.7.2)
C# compiler (such as Visual Studio or .NET CLI)


Usage
-Clone the repository or download the source code to your local machine.
-Open the solution file in Visual Studio or any C# development environment.
-Build the project to generate the executable file.
-Run the executable file to start the BB84 Protocol Simulator.

Upon running the simulator, you will be prompted to choose the strategy for the hacker (Eve),
as well as other parameters such as data size and error rates for different components of the simulation (Alice, Bob, Eve, and the optical fiber).

The simulator will display the results of the simulation, 
including the QBER before and after data shortening, and the percentage of stolen data by the hacker.

License
xd