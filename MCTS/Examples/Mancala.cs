using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MCTS;

namespace MCTSexample
{
    class MancalaNode : Node
    {
        public byte[,] board = { { 0, 4, 4, 4, 4, 4, 4 }, { 0, 4, 4, 4, 4, 4, 4 } };

        public override Node GetNextState(int move)
        {
            byte[,] nextBoard = (byte[,])board.Clone();
            int player = ActivePlayer;

            move++;
            int side = player;
            byte beans = nextBoard[side, move];
            nextBoard[side, move] = 0;
            while (beans > 0)
            {
                move--;
                if (move == -1)
                {
                    move = 6;
                    side = 1 - side;
                }
                if (side == player || move > 0)
                {
                    beans--;
                    nextBoard[side, move]++;
                }
            }

            if (move > 0)
            {
                if (side == player && nextBoard[side, move] == 1 && nextBoard[1 - side, 7 - move] > 0)
                {
                    nextBoard[player, 0] += nextBoard[player, move];
                    nextBoard[player, move] = 0;
                    nextBoard[player, 0] += nextBoard[1 - player, 7 - move];
                    nextBoard[1 - player, 7 - move] = 0;
                }
                player = (player + 1)%2;
            }

            bool noreallyended = false;
            for (int i = 0; i < 2; i++)
            {
                bool ended = true;
                for (int j = 1; j <= 6; j++)
                    if (nextBoard[i, j] > 0)
                        ended = false;
                if (ended)
                    noreallyended = true;
            }
            if (noreallyended)
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 1; j <= 6; j++)
                    {
                        beans = nextBoard[i, j];
                        nextBoard[i, j] = 0;
                        nextBoard[i, 0] += beans;
                    }
                }
            }
            return new MancalaNode() { board = nextBoard, ActivePlayer = player };
        }

        public override bool MoveIsLegal(int move)
        {
            return move >= 0 && move < NumberOfMoves() && board[ActivePlayer, move + 1] > 0;
        }

        public override int NumberOfMoves()
        {
            return 6;
        }

        public override int Winner()
        {
            if (board[0, 0] + board[1, 0] < 48) return -1;
            int diff = board[0, 0] - board[1, 0];
            if (diff > 0) return 0;
            if (diff < 0) return 1;
            return -1;
        }

        public string BoardAsString()
        {
            string output = "";

            output += "   ";
            for (int i = 0; i < 6; i++)
                output += "| " + board[1, i+1].ToString("D2") + " ";
            output += "|   \n";
            output += board[1, 0].ToString("D2") + " +----+----+----+----+----+----+ " + board[0, 0].ToString("D2") + "\n";
            output += "   ";
            for (int i = 0; i < 6; i++)
                output += "| " + board[0, 6 - i].ToString("D2") + " ";
            output += "|   ";

            return output;
        }
    }

    class Mancala
    {
        public static void Play()
        {
            while (true)
            {
                List<int> moveLog = new List<int>();
                int previousPlayer = -1;

                Node gameNode = new MancalaNode();
                Draw(gameNode, moveLog);

                while (gameNode.GameInProgress())
                {
                    if (previousPlayer != gameNode.ActivePlayer) 
                        moveLog.Clear();
                    previousPlayer = gameNode.ActivePlayer;

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
                    moveLog.Add(nextMove + 1);
                    Draw(gameNode, moveLog);
                }

                Console.WriteLine(GetPlayerName(gameNode.Winner()) + " wins");
                Console.ReadKey();
            }

            void Draw(Node node, List<int> recentMoves)
            {
                Console.Clear();
                Console.Write((node as MancalaNode).BoardAsString());
                Console.WriteLine();
                Console.WriteLine();
                if (node.Parent != null)
                {
                    Console.WriteLine(GetPlayerName(node.Parent.ActivePlayer) + " played " + string.Join(", ", recentMoves));
                }

            }

            string GetPlayerName(int player)
            {
                switch (player)
                {
                    case 0: return "Player 1";
                    case 1: return "Player 2";
                    default: return "No one";
                }
            }
        }
    }
}
