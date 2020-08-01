using MCTS;
using System;
using System.Runtime.InteropServices;

namespace MCTSexample
{
    class OrderAndChaosNode : Node
    {
        Piece[,] board = new Piece[6, 6];

        public static int GetMoveAsInt(int x, int y, Piece piece)
        {
            return y * 12 + x * 2 + ((int)piece - 1);
        }

        public static Piece PieceComponent(int move)
        {
            return (Piece)(move % 2 + 1);
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
            Piece[,] nextBoard = (Piece[,])board.Clone();
            nextBoard[Xcomponent(move), Ycomponent(move)] = PieceComponent(move);
            return new OrderAndChaosNode { ActivePlayer = (ActivePlayer + 1) % 2, board = nextBoard };
        }

        public override bool MoveIsLegal(int move)
        {
            return move >= 0 && move < NumberOfMoves() && board[Xcomponent(move), Ycomponent(move)] == Piece.None;
        }

        public override int NumberOfMoves()
        {
            return 72;
        }

        public override int Winner()
        {
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    if (board[x, y] == Piece.None)
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
                        return 0;
                }
            }

            if (!GameInProgress()) return 1;

            return -1;
        }

        public override bool GameInProgress()
        {
            for (int x = 0; x < 6; x++)
                for (int y = 0; y < 6; y++)
                    if (board[x, y] == Piece.None)
                        return true;
            return false;
        }

        public string BoardAsString()
        {
            string output = "";
            for (int y = 0; y < 6; y++)
            {
                output += "|";
                for (int x = 0; x < 6; x++)
                {
                    switch (board[x, y])
                    {
                        case Piece.X:
                            output += "X|";
                            break;
                        case Piece.O:
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
    }

    enum Piece { None, X, O }

    class OrderAndChaos
    {
        public static void Play()
        {
            while (true)
            {
                Node gameNode = new OrderAndChaosNode();
                Draw(gameNode);

                while (gameNode.GameInProgress())
                {
                    int nextMove = -1;

                    if (gameNode.ActivePlayer == 0)
                    {
                        while (!gameNode.MoveIsLegal(nextMove))
                        {
                            Console.WriteLine("Please pick a move (x,y,piece)");
                            int x = Console.ReadKey().KeyChar - '1';
                            int y = Console.ReadKey().KeyChar - '1';
                            Piece piece = char.ToUpper(Console.ReadKey().KeyChar) == 'X' ? Piece.X : Piece.O;
                            nextMove = OrderAndChaosNode.GetMoveAsInt(x, y, piece);
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
                Console.Write((node as OrderAndChaosNode).BoardAsString());
                Console.WriteLine();
                Console.WriteLine();
                if (node.Parent != null)
                {
                    Console.WriteLine(GetPlayerName(node.Parent.ActivePlayer) + " played " + OrderAndChaosNode.PieceComponent(recentMove) +
                        " at " + (OrderAndChaosNode.Xcomponent(recentMove) + 1) + ", " + (OrderAndChaosNode.Ycomponent(recentMove) + 1));
                }
            }

            string GetPlayerName(int player)
            {
                switch (player)
                {
                    case 0: return "Order";
                    case 1: return "Choas";
                    default: return "No one";
                }
            }
        }
    }
}
