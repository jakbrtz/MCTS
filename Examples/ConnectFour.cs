using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCTS;

namespace Examples
{
    class ConnectFourNode : Node
    {
        public static readonly int width = 7;
        public static readonly int height = 6;
        int[,] board = new int[width, height];

        public int GetPiece(int x, int y) => board[x, y];

        public ConnectFourNode()
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    board[x, y] = -1;
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

    class ConnectFour : ConsoleGamePlay
    {
        public static void Play()
        {
            while (true)
            {
                new ConnectFour().Play(new ConnectFourNode());
                Console.ReadKey();
            }
        }

        bool hasUsedTaunt = false;

        public override string BoardToString(Node node)
        {
            string output = "";
            for (int y = 0; y < ConnectFourNode.height; y++)
            {
                output += "|";
                for (int x = 0; x < ConnectFourNode.width; x++)
                {
                    switch ((node as ConnectFourNode).GetPiece(x, y))
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

        public override int GetHumanMove(Node node)
        {
            if (!hasUsedTaunt && node.PlayerThatCanForceWin == 1)
            {
                Console.WriteLine("I'm going to win and there's nothing you can do to stop me");
                hasUsedTaunt = true;
            }
            else
            {
                Console.WriteLine("Please pick a move...");
            }
            return Console.ReadKey().KeyChar - '1';
        }

        public override string GetPlayerName(int player)
        {
            switch (player)
            {
                case 0: return "X";
                case 1: return "O";
                default: return "No one";
            }
        }

        public override string MoveToString(int move)
        {
            return (move + 1).ToString();
        }
    }
}
