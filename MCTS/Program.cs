using System;
using System.Threading;

namespace MCTSexample
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Pick a game to play:");
            Console.WriteLine();
            Console.WriteLine("1. Connect Four");
            Console.WriteLine("2. Mancala");
            Console.WriteLine("3. Order and Chaos");
            Console.WriteLine("4. Ultimate Tic Tac Toe");

            switch (Console.ReadKey().KeyChar)
            {
                case '1':
                    Console.WriteLine("A simple example of a game that can be made using this code.");
                    Console.WriteLine("There are 7 moves you can do, and this can be done by pressing keys 1-7");
                    Thread.Sleep(5000);
                    ConnectFour.Play();
                    break;
                case '2':
                    Console.WriteLine("Another simple example of a game that can be made using this code.");
                    Console.WriteLine("There are 6 moves you can do, and this can be done by pressing keys 1-7");
                    Thread.Sleep(5000);
                    Mancala.Play();
                    break;
                case '3':
                    Console.WriteLine("A more complicated game where a move has multiple components");
                    Console.WriteLine("First press a digit to represent the X component, then the Y component, then an X or O");
                    Thread.Sleep(5000);
                    OrderAndChaos.Play();
                    break;
                case '4':
                    Console.WriteLine("A more complicated game where a move has multiple components, and there are heavier restrictions on where you can move");
                    Console.WriteLine("First press a digit to represent the X component, then the Y component");
                    UltimateTicTacToe.Play();
                    break;
            }
        }
    }
}