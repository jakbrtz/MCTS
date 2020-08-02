using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                    bool CheckForConsecutive(int startX, int startY, int dx, int dy, int length)
                    {
                        int endX = startX + (length - 1) * dx;
                        if (endX < 0 || endX >= width) return false;
                        int endY = startY + (length - 1) * dy;
                        if (endY < 0 || endY >= height) return false;
                        for (int i = 0; i < length; i++)
                            if (board[startX, startY] != board[startX + i * dx, startY + i * dy])
                                return false;
                        return true;
                    }

                    if (CheckForConsecutive(x, y, 1, 0, 4) ||
                        CheckForConsecutive(x, y, 0, 1, 4) ||
                        CheckForConsecutive(x, y, 1, 1, 4) ||
                        CheckForConsecutive(x, y, 1, -1, 4))
                        return board[x,y];
                }
            }

            return -1;
        }
    }


    class ConnectFour
    {
        public static void Play()
        {
            while (true)
            {
                bool hasUsedTaunt = false;
                Node gameNode = new ConnectFourNode();
                Draw(gameNode);

                while (gameNode.GameInProgress())
                {
                    int nextMove = -1;

                    if (gameNode.ActivePlayer == 0)
                    {
                        while (!gameNode.MoveIsLegal(nextMove))
                        {
                            if (!hasUsedTaunt && gameNode.PlayerThatCanForceWin == 1)
                            {
                                Console.WriteLine("I'm going to win and there's nothing you can do to stop me");
                                hasUsedTaunt = true;
                            }
                            else
                            {
                                Console.WriteLine("Please pick a move...");
                            }
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
