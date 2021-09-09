using MCTS;
using System;
using System.Runtime.InteropServices;

namespace Examples
{
    class OrderAndChaosNode : Node
    {
        OrderAndChaosPiece[,] board = new OrderAndChaosPiece[6, 6];

        public OrderAndChaosPiece GetPiece(int x, int y) => board[x, y];

        public static int GetMoveAsInt(int x, int y, OrderAndChaosPiece piece)
        {
            return y * 12 + x * 2 + ((int)piece - 1);
        }

        public static OrderAndChaosPiece PieceComponent(int move)
        {
            return (OrderAndChaosPiece)(move % 2 + 1);
        }

        public static int Xcomponent(int move)
        {
            return move / 2 % 6;
        }

        public static int Ycomponent(int move)
        {
            return move / 12;
        }

        public override Node GetNextState(int move)
        {
            OrderAndChaosPiece[,] nextBoard = (OrderAndChaosPiece[,])board.Clone();
            nextBoard[Xcomponent(move), Ycomponent(move)] = PieceComponent(move);
            return new OrderAndChaosNode { ActivePlayer = (ActivePlayer + 1) % 2, board = nextBoard };
        }

        public override bool MoveIsLegal(int move)
        {
            return move >= 0 && move < NumberOfMoves() && board[Xcomponent(move), Ycomponent(move)] == OrderAndChaosPiece.None;
        }

        public override int NumberOfMoves()
        {
            return 72;
        }

        public override int Winner()
        {
            if (OrderHasWon()) return 0;
            if (!GameInProgress()) return 1;
            return -1;
        }

        bool OrderHasWon()
        {
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    if (board[x, y] == OrderAndChaosPiece.None)
                        continue;

                    bool CheckForConsecutive(int startX, int startY, int dx, int dy, int length)
                    {
                        int endX = startX + (length - 1) * dx;
                        if (endX < 0 || endX >= 6) return false;
                        int endY = startY + (length - 1) * dy;
                        if (endY < 0 || endY >= 6) return false;
                        for (int i = 0; i < length; i++)
                            if (board[startX, startY] != board[startX + i * dx, startY + i * dy])
                                return false;
                        return true;
                    }

                    if (CheckForConsecutive(x, y, 1, 0, 5) ||
                        CheckForConsecutive(x, y, 0, 1, 5) ||
                        CheckForConsecutive(x, y, 1, 1, 5) ||
                        CheckForConsecutive(x, y, 1, -1, 5))
                        return true;
                }
            }
            return false;
        }

        public override bool GameInProgress()
        {
            if (OrderHasWon())
                return false;
            for (int x = 0; x < 6; x++)
                for (int y = 0; y < 6; y++)
                    if (board[x, y] == OrderAndChaosPiece.None)
                        return true;
            return false;
        }
    }

    class OrderAndChaos : ConsoleGamePlay
    {
        public static void Play()
        {
            while (true)
            {
                new OrderAndChaos().Play(new OrderAndChaosNode());
                Console.ReadKey();
            }
        }

        public override string BoardToString(Node node)
        {
            OrderAndChaosNode n = node as OrderAndChaosNode;
            string output = "";
            for (int y = 0; y < 6; y++)
            {
                output += "|";
                for (int x = 0; x < 6; x++)
                {
                    switch (n.GetPiece(x, y))
                    {
                        case OrderAndChaosPiece.X:
                            output += "X|";
                            break;
                        case OrderAndChaosPiece.O:
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
            Console.WriteLine("Please pick a move (x,y,piece)");
            int x = Console.ReadKey().KeyChar - '1';
            int y = Console.ReadKey().KeyChar - '1';
            OrderAndChaosPiece piece = char.ToUpper(Console.ReadKey().KeyChar) == 'X' ? OrderAndChaosPiece.X : OrderAndChaosPiece.O;
            return OrderAndChaosNode.GetMoveAsInt(x, y, piece);
        }

        public override string GetPlayerName(int player)
        {
            switch (player)
            {
                case 0: return "Order";
                case 1: return "Choas";
                default: return "No one";
            }
        }

        public override string MoveToString(int move)
        {
            return OrderAndChaosNode.PieceComponent(move) + " at " + (OrderAndChaosNode.Xcomponent(move) + 1) + ", " + (OrderAndChaosNode.Ycomponent(move) + 1);
        }
    }

    enum OrderAndChaosPiece { None, X, O }
}
