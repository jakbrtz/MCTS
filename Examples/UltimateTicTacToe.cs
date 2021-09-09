using System;
using MCTS;

namespace Examples
{
    class UltimateTicTacToeNode : Node
    {
        public int[,] board = new int[9, 9];
        MiniCoord miniBoard;

        public int GetPiece(int x, int y) => board[x, y];

        struct MiniCoord
        {
            private int value;
            public int X => value / 3;
            public int Y => value % 3;
            public bool HasValue => value != -1;
            public void Set(int x, int y) { value = 3 * x + y; }
            public void Reset() { value = -1; }
        }

        int MiniWinner(MiniCoord coord)
        {
            return MiniWinner(coord.X, coord.Y);
        }

        int MiniWinner(int x, int y)
        {
            if (board[3 * x + 0, 3 * y + 0] != -1 && board[3 * x + 0, 3 * y + 0] == board[3 * x + 1, 3 * y + 0] && board[3 * x + 1, 3 * y + 0] == board[3 * x + 2, 3 * y + 0])
                return board[3 * x + 0, 3 * y + 0];

            if (board[3 * x + 0, 3 * y + 1] != -1 && board[3 * x + 0, 3 * y + 1] == board[3 * x + 1, 3 * y + 1] && board[3 * x + 1, 3 * y + 1] == board[3 * x + 2, 3 * y + 1])
                return board[3 * x + 0, 3 * y + 1];

            if (board[3 * x + 0, 3 * y + 2] != -1 && board[3 * x + 0, 3 * y + 2] == board[3 * x + 1, 3 * y + 2] && board[3 * x + 1, 3 * y + 2] == board[3 * x + 2, 3 * y + 2])
                return board[3 * x + 0, 3 * y + 2];

            if (board[3 * x + 0, 3 * y + 0] != -1 && board[3 * x + 0, 3 * y + 0] == board[3 * x + 0, 3 * y + 1] && board[3 * x + 0, 3 * y + 1] == board[3 * x + 0, 3 * y + 2])
                return board[3 * x + 0, 3 * y + 0];

            if (board[3 * x + 1, 3 * y + 0] != -1 && board[3 * x + 1, 3 * y + 0] == board[3 * x + 1, 3 * y + 1] && board[3 * x + 1, 3 * y + 1] == board[3 * x + 1, 3 * y + 2])
                return board[3 * x + 1, 3 * y + 0];

            if (board[3 * x + 2, 3 * y + 0] != -1 && board[3 * x + 2, 3 * y + 0] == board[3 * x + 2, 3 * y + 1] && board[3 * x + 2, 3 * y + 1] == board[3 * x + 2, 3 * y + 2])
                return board[3 * x + 2, 3 * y + 0];

            if (board[3 * x + 0, 3 * y + 0] != -1 && board[3 * x + 0, 3 * y + 0] == board[3 * x + 1, 3 * y + 1] && board[3 * x + 1, 3 * y + 1] == board[3 * x + 2, 3 * y + 2])
                return board[3 * x + 0, 3 * y + 0];

            if (board[3 * x + 0, 3 * y + 2] != -1 && board[3 * x + 0, 3 * y + 2] == board[3 * x + 1, 3 * y + 1] && board[3 * x + 1, 3 * y + 1] == board[3 * x + 2, 3 * y + 0])
                return board[3 * x + 0, 3 * y + 2];

            return -1;
        }

        public bool MiniFull(int x, int y)
        {
            for (int i = 0; i<3u; i++)
                for (int j = 0; j < 3; j++)
                    if (board[3 * x + i, 3 * y + j] == -1)
                        return false;
            return true;
        }

        bool MiniFull(MiniCoord coord)
        {
            return MiniFull(coord.X, coord.Y);
        }

        public UltimateTicTacToeNode()
        {
            for (int x = 0; x < 9; x++)
                for (int y = 0; y < 9; y++)
                    board[x, y] = -1;
            miniBoard.Reset();
        }

        public override Node GetNextState(int move)
        {
            int[,] nextBoard = (int[,])board.Clone();
            MiniCoord nextCoord = miniBoard;

            int x = move / 9;
            int y = move % 9;

            nextBoard[x, y] = ActivePlayer;
            nextCoord.Set(x % 3, y % 3);
            if (MiniWinner(nextCoord) != -1 || MiniFull(nextCoord))
                nextCoord.Reset();

            return new UltimateTicTacToeNode() { ActivePlayer = (ActivePlayer + 1) % 2, board = nextBoard, miniBoard = nextCoord };
        }

        public override bool MoveIsLegal(int move)
        {
            int x = move / 9;
            int y = move % 9;
            return
                x >= 0 && x < 9 && y >= 0 && y < 9 &&
                board[x, y]==-1 && (
                (miniBoard.HasValue && miniBoard.X == x / 3 && miniBoard.Y == y / 3) ||
                (!miniBoard.HasValue && !MiniFull(x / 3, y / 3) && MiniWinner(x / 3, y / 3) == -1)
                );
        }

        public override int NumberOfMoves()
        {
            return 9 * 9;
        }

        public override int Winner()
        {
            if (MiniWinner(0, 0) != -1 && MiniWinner(0, 0) == MiniWinner(1, 0) && MiniWinner(1, 0) == MiniWinner(2, 0))
                return MiniWinner(0, 0);

            if (MiniWinner(0, 1) != -1 && MiniWinner(0, 1) == MiniWinner(1, 1) && MiniWinner(1, 1) == MiniWinner(2, 1))
                return MiniWinner(0, 1);

            if (MiniWinner(0, 2) != -1 && MiniWinner(0, 2) == MiniWinner(1, 2) && MiniWinner(1, 2) == MiniWinner(2, 2))
                return MiniWinner(0, 2);

            if (MiniWinner(0, 0) != -1 && MiniWinner(0, 0) == MiniWinner(0, 1) && MiniWinner(0, 1) == MiniWinner(0, 2))
                return MiniWinner(0, 0);

            if (MiniWinner(1, 0) != -1 && MiniWinner(1, 0) == MiniWinner(1, 1) && MiniWinner(1, 1) == MiniWinner(1, 2))
                return MiniWinner(1, 0);

            if (MiniWinner(2, 0) != -1 && MiniWinner(2, 0) == MiniWinner(2, 1) && MiniWinner(2, 1) == MiniWinner(2, 2))
                return MiniWinner(2, 0);

            if (MiniWinner(0, 0) != -1 && MiniWinner(0, 0) == MiniWinner(1, 1) && MiniWinner(1, 1) == MiniWinner(2, 2))
                return MiniWinner(0, 0);

            if (MiniWinner(0, 2) != -1 && MiniWinner(0, 2) == MiniWinner(1, 1) && MiniWinner(1, 1) == MiniWinner(2, 0))
                return MiniWinner(0, 2);

            return -1;
        }
    }

    class UltimateTicTacToe : ConsoleGamePlay
    {
        public static void Play()
        {
            while (true)
            {
                new UltimateTicTacToe().Play(new UltimateTicTacToeNode());
                Console.ReadKey();
            }
        }

        public override string BoardToString(Node node)
        {
            string output = "";
            for (int y = 0; y < 9; y++)
            {
                if (y % 3 == 0)
                    output += "\n                   \n";
                else
                    output += "\n -+-+- -+-+- -+-+- \n";

                for (int x = 0; x < 9; x++)
                {
                    if (x % 3 == 0)
                        output += " ";
                    else
                        output += "|";

                    switch ((node as UltimateTicTacToeNode).GetPiece(x, y))
                    {
                        case 0:
                            output += "X";
                            break;
                        case 1:
                            output += "O";
                            break;
                        default:
                            output += " ";
                            break;
                    }
                }
                output += " ";
            }
            return output;
        }

        public override int GetHumanMove(Node node)
        {
            Console.WriteLine("Please pick a move...");
            int x = Console.ReadKey().KeyChar - '1';
            int y = Console.ReadKey().KeyChar - '1';
            return 9 * x + y;
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
            string output = "";
            if (move % 9 / 3 == 0) output += "Top";
            if (move % 9 / 3 == 2) output += "Bottom";
            if (move / 9 / 3 == 0) output += "Left";
            if (move / 9 / 3 == 2) output += "Right";
            if (move / 9 / 3 == 1 && move % 9 / 3 == 1) output += "Center";
            output += " ";
            if (move % 9 % 3 == 0) output += "Top";
            if (move % 9 % 3 == 2) output += "Bottom";
            if (move / 9 % 3 == 0) output += "Left";
            if (move / 9 % 3 == 2) output += "Right";
            if (move / 9 % 3 == 1 && move % 9 % 3 == 1) output += "Center";
            return output;
        }
    }
}
