using System;
using MCTS;

namespace MCTSexample
{
    class ConnectFourNode : Node
    {
        public static readonly int width = 7;
        public static readonly int height = 6;
        int[,] board = new int[width, height];

        public ConnectFourNode()
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    board[x, y] = -1;
        }

        public string BoardAsString()
        {
            string output = "";
            for (int y = 0; y < height; y++)
            {
                output += "|";
                for (int x = 0; x < width; x++)
                {
                    switch (board[x, y])
                    {
                        case 0:
                            output += "X|";
                            break;
                        case 1:
                            output += "O|";
                            break;
                        default:
                            output += " |";
                            break;
                    }
                }
                output += '\n';
            }
            return output;
        }

        public override Node GetNextState(int move)
        {
            int[,] boardClone = (int[,])board.Clone();

            int y = height - 1;
            while (y >= 0 && boardClone[move, y] != -1) y--;
            boardClone[move, y] = ActivePlayer;

            return new ConnectFourNode { ActivePlayer = (ActivePlayer + 1) % 2, board = boardClone };
        }

        public override bool MoveIsLegal(int move)
        {
            return move >= 0 && move < width && board[move, 0] == -1;
        }

        public override int NumberOfMoves()
        {
            return width;
        }

        public override int Winner()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (board[x, y] == -1)
                        continue;

                    //Horizontals
                    if (x + 4 <= width && board[x, y] == board[x + 1, y] && board[x, y] == board[x + 2, y] && board[x, y] == board[x + 3, y])
                        return board[x, y];

                    //Verticals
                    if (y + 4 <= height && board[x, y] == board[x, y + 1] && board[x, y] == board[x, y + 2] && board[x, y] == board[x, y + 3])
                        return board[x, y];

                    //Diagonal \
                    if (x + 4 <= width && y + 4 <= height && board[x, y] == board[x + 1, y + 1] && board[x, y] == board[x + 2, y + 2] && board[x, y] == board[x + 3, y + 3])
                        return board[x, y];

                    //Diagonal /
                    if (x + 4 <= width && y >= 3 && board[x, y] == board[x + 1, y - 1] && board[x, y] == board[x + 2, y - 2] && board[x, y] == board[x + 3, y - 3])
                        return board[x, y];
                }
            }

            return -1;
        }
    }

    class Program
    {
        static void Main()
        {
            while (true)
            {
                Node gameNode = new ConnectFourNode();
                Draw(gameNode);

                while (gameNode.GameInProgress())
                {
                    int nextMove = -1;

                    if (gameNode.ActivePlayer == 0)
                    {
                        while (!gameNode.MoveIsLegal(nextMove))
                        {
                            Console.WriteLine("Please pick a move...");
                            nextMove = Console.ReadKey().KeyChar - '1';
                        }
                    }
                    else
                    {
                        Console.WriteLine("Thinking...");
                        nextMove = gameNode.PickNextMove();
                    }

                    gameNode = gameNode.DoMove(nextMove);
                    Draw(gameNode, nextMove);
                }

                Console.WriteLine(GetPlayerName(gameNode.Winner()) + " wins");
                Console.ReadKey();
            }

            void Draw(Node node, int recentMove = -1)
            {
                Console.Clear();
                Console.Write((node as ConnectFourNode).BoardAsString());
                Console.WriteLine();
                Console.WriteLine();
                if (node.Parent != null)
                {
                    Console.WriteLine(GetPlayerName(node.Parent.ActivePlayer) + " played " + (recentMove + 1));
                }
            }

            string GetPlayerName(int player)
            {
                switch (player)
                {
                    case 0: return "X";
                    case 1: return "O";
                    default: return "No one";
                }
            }
        }
    }
}