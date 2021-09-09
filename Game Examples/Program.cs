using System;
using System.Windows.Forms;

namespace Examples
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
            Console.WriteLine("5. Reversi");

            switch (Console.ReadKey().KeyChar)
            {
                case '1':
                    Console.WriteLine("A simple example of a game that can be made using this code.");
                    Console.WriteLine("Make a move by pressing keys 1-7");
                    Console.ReadKey();
                    ConnectFour.Play();
                    break;
                case '2':
                    Console.WriteLine("A simple example of a how to apply this code to a form");
                    Console.WriteLine("Make a move by clicking the buttons on the screen");
                    Console.ReadKey();
                    Application.EnableVisualStyles();
                    Application.Run(new Mancala());
                    break;
                case '3':
                    Console.WriteLine("A more complicated game where a move has multiple components");
                    Console.WriteLine("First press a digit to represent the X component, then the Y component, then an X or O");
                    Console.ReadKey();
                    OrderAndChaos.Play();
                    break;
                case '4':
                    Console.WriteLine("A more complicated game where a move has multiple components, and there are heavier restrictions on where you can move");
                    Console.WriteLine("First press a digit to represent the X component, then the Y component");
                    Console.ReadKey();
                    UltimateTicTacToe.Play();
                    break;
                case '5':
                    Console.WriteLine("A more complicated game where the game has a UI");
                    Console.WriteLine("Click where you want to place a piece");
                    Console.ReadKey();
                    Application.EnableVisualStyles();
                    Application.Run(new Reversi());
                    break;
            }
        }
    }
}
