using System;
using System.Collections.Generic;

namespace MCTS
{
    class Program
    {
        // CHANGE THESE VARIABLES:
        public static readonly bool autoRestart = false;
        public static readonly bool Xhuman = true;
        public static readonly bool Ohuman = false;
        public static readonly int simulations = 10000;
        static readonly double tuningParameter = Math.Sqrt(2);

        // Keeping track of results
        static int Xwins;
        static int Owins;
        static int Ties;

        static void Main(string[] args)
        {
            // Create loop so multiple games can be played
            while (true)
            {
                Node gameNode = new Node();
                int nextMove = Move.None;
                Player winner = Player.none;

                Draw(gameNode, nextMove);

                // While the game is in play
                while (!gameNode.state.GameOver())
                {
                    if ((Xhuman && gameNode.state.turn == Player.X) || (Ohuman && gameNode.state.turn == Player.O))
                    {
                        // Let the human pick a move
                        Console.WriteLine("Waiting for human...");
                        // Confirm that the human is playing a legal move
                        nextMove = Move.None;
                        while (!gameNode.state.MoveIsLegal(nextMove))
                        {
                            // FIXME: convert the player's input into a move
                            int x = Console.ReadKey().KeyChar - '1';
                            int y = Console.ReadKey().KeyChar - '1';
                            nextMove = Move.ToInt(x, y);
                        }
                    }
                    else
                    {
                        // Let the computer pick a move
                        nextMove = PickNextMove(gameNode);
                    }

                    if (gameNode.children != null && gameNode.children[nextMove] != null)
                    {
                        // If the next move has already been considered, move to that node
                        gameNode = gameNode.children[nextMove];
                    }
                    else
                    {
                        // If the next move hasn't been considered, create a new node to represent the new game state
                        Node nextState = new Node { state = new State(gameNode.state, nextMove), parent = gameNode };
                        gameNode.children = new Node[Move.Range];
                        gameNode.children[nextMove] = nextState;
                        gameNode = nextState;
                    }

                    Draw(gameNode, nextMove);
                    winner = gameNode.state.Winner();
                }

                // Keep track of how many times each player wins
                switch (winner)
                {
                    case Player.X:
                        Xwins++;
                        break;
                    case Player.O:
                        Owins++;
                        break;
                    default:
                        Ties++;
                        break;
                }

                Draw(gameNode, nextMove);
                Console.WriteLine();
                Console.WriteLine(winner + " wins");

                // Restart the game
                if (!autoRestart)
                {
                    Console.WriteLine("Press any key to restart");
                    Console.ReadKey();
                }
            }
        }

        // Display the state of the game and other details
        static void Draw(Node node, int recentMove)
        {
            Console.Clear();
            Console.Write(node.state.ToString());
            Console.WriteLine();
            Console.WriteLine();
            if (node.parent != null)
            {
                Console.WriteLine(node.parent.state.turn.ToString() + " played " + recentMove.Description());
                Console.WriteLine();
            }
            Console.WriteLine("X wins: " + Xwins);
            Console.WriteLine("O wins: " + Owins);
            Console.WriteLine("Ties:   " + Ties);
        }

        /* The computer picks its next move using the Monte Carlos Tree Search Algorithm (MCTS), using the Upper Confidence Bound (UCB)
         * https://en.wikipedia.org/wiki/Monte_Carlo_tree_search
         * */
        static int PickNextMove(Node root)
        {
            for (int i = 0; i < simulations; i++)
            {
                //Selection
                Node current = root;
                while (current.children != null)
                {
                    double bestUCB = double.MinValue;
                    Node nextNode = null;
                    foreach (Node child in current.children)
                    {
                        if (child == null) continue;
                        double childUCB = child.Weight() + tuningParameter * Math.Sqrt(Math.Log(current.visits) / child.visits);
                        if (childUCB > bestUCB)
                        {
                            bestUCB = childUCB;
                            nextNode = child;
                        }
                    }
                    current = nextNode ?? throw new Exception("No node selected");
                }
                //Check the game isn't over
                Node simulated = current;
                if (!current.state.GameOver())
                {
                    //Expansion
                    current.children = new Node[Move.Range];
                    for (int possibleMove = 0; possibleMove < Move.Range; possibleMove++)
                        if (current.state.MoveIsLegal(possibleMove))
                            current.children[possibleMove] = new Node { state = new State(current.state, possibleMove), parent = current };
                    current = current.RandomChild();
                    //Simulation
                    simulated = current;
                    while (!simulated.state.GameOver())
                        simulated = simulated.RandomChild();
                }
                Player winner = simulated.state.Winner();
                //Backpropogation
                while (current != root)
                {
                    current.visits++;
                    if (current.state.turn == winner.Next())
                        current.score++;
                    else if (current.state.turn == winner)
                        current.score--;
                    current = current.parent;
                }
                current.visits++;
            }

            //Pick best score (ignoring UCB)
            double bestWeight = double.MinValue;
            int selectedMove = Move.None;

            for (int possibleMove = 0; possibleMove < Move.Range; possibleMove++)
            {
                Node child = root.children[possibleMove];
                if (child != null && child.Weight() > bestWeight)
                {
                    bestWeight = child.Weight();
                    selectedMove = possibleMove;
                }
            }
            if (selectedMove == Move.None) throw new IndexOutOfRangeException("No move was selected");
            return selectedMove;
        }
    }

    // The Player enumerable represents which piece is in which cell, and who's turn it is
    enum Player { none, X, O }

    static class PieceExtensions
    {
        public static Player Next(this Player current)
        {
            switch (current)
            {
                case Player.X:
                    return Player.O;
                case Player.O:
                    return Player.X;
                default:
                    return Player.none;
            }
        }

        public static bool IsVacant(this Player current)
        {
            return current == Player.none;
        }
    }

    /* The Move class allows moves to be represented by numbers
     * The MCTS needs to iterate over all moves. 
     * This is done by iterating from 0 to Move.range, then converting those integers to moves
     * */
    static class Move
    {
        // `Range` is the number of possible moves
        // FIXME: adjust this number to represent the range
        public static readonly int Range = 9 * 9;
        // `None` represents no move
        public static readonly int None = -1;

        // FIXME: write a function that describes a move that has been played
        public static string Description(this int move)
        {
            string output = "";
            if (move.Y() / 3 == 0) output += "Top";
            if (move.Y() / 3 == 2) output += "Bottom";
            if (move.X() / 3 == 0) output += "Left";
            if (move.X() / 3 == 2) output += "Right";
            if (move.X() / 3 == 1 && move.Y() / 3 == 1) output += "Center";
            output += " ";
            if (move.Y() % 3 == 0) output += "Top";
            if (move.Y() % 3 == 2) output += "Bottom";
            if (move.X() % 3 == 0) output += "Left";
            if (move.X() % 3 == 2) output += "Right";
            if (move.X() % 3 == 1 && move.Y() % 3 == 1) output += "Center";
            return output;
        }

        /* This function is only used to convert a player's move into a number
         * It is only used once in Program.Main, but I believe that it belongs here
         * FIXME: convert player input into a move. Or don't. This is just a helper function.
         */
        public static int ToInt(int x, int y)
        {
            return 9 * x + y;
        }

        // FIXME: helper functions that convert integers to moves

        public static int X(this int move)
        {
            return move / 9;
        }

        public static int Y(this int move)
        {
            return move % 9;
        }

    }

    /* The State class holds all the details about the state of each game
     * It also has methods such as detecting which moves are legal, and when the game is over
     */
    class State
    {
        // Keep track who's turn it is
        public Player turn = Player.X;

        // FIXME: write variables that keep track of the state of the game
        public Player[,] board = new Player[9, 9];
        MiniCoord miniBoard;

        // FIXME (optional): write helper structures for the variables that keep track of the state of the game
        struct MiniCoord
        {
            private int value;
            public int X { get { return value / 3; } }
            public int Y { get { return value % 3; } }
            public bool HasValue { get { return value != -1; } }
            public void Set(int x, int y) { value = 3 * x + y; }
            public void Reset() { value = -1; }
        }

        // Parameterless contructor
        public State()
        {
            // FIXME (optional): make sure the default value of class variables are correct
            miniBoard.Reset();
        }

        // Constructor that continues the game from an existing position
        public State(State previous, int move)
        {
            // Copy the previous state
            // FIXME: make sure all the class fields are here
            board = (Player[,])previous.board.Clone();
            turn = previous.turn;
            miniBoard = previous.miniBoard;

            // Take turn
            // FIXME: write the code that modifies the board based on the move a player took
            int x = move.X();
            int y = move.Y();

            board[x, y] = turn;
            turn = turn.Next();
            miniBoard.Set(x % 3, y % 3);
            if (MiniWinner(miniBoard) != Player.none || MiniFull(miniBoard))
                miniBoard.Reset();
        }

        // Checks that a move can be legally done from this state
        public bool MoveIsLegal(int move)
        {
            if (move == Move.None) return false;
            // FIXME: write a function that detects whether a move is legal
            int x = move.X();
            int y = move.Y();
            return
                x >= 0 && x < 9 && y >= 0 && y < 9 &&
                board[x, y].IsVacant() && (
                (miniBoard.HasValue && miniBoard.X == x / 3 && miniBoard.Y == y / 3) ||
                (!miniBoard.HasValue && !MiniFull(x / 3, y / 3) && MiniWinner(x / 3, y / 3) == Player.none)
                );
        }

        // Checks whether the game has been won, and who the winner is
        public Player Winner()
        {
            // FIXME: write a function that detects who has won

            if (MiniWinner(0, 0) != Player.none && MiniWinner(0, 0) == MiniWinner(1, 0) && MiniWinner(1, 0) == MiniWinner(2, 0))
                return MiniWinner(0, 0);

            if (MiniWinner(0, 1) != Player.none && MiniWinner(0, 1) == MiniWinner(1, 1) && MiniWinner(1, 1) == MiniWinner(2, 1))
                return MiniWinner(0, 1);

            if (MiniWinner(0, 2) != Player.none && MiniWinner(0, 2) == MiniWinner(1, 2) && MiniWinner(1, 2) == MiniWinner(2, 2))
                return MiniWinner(0, 2);

            if (MiniWinner(0, 0) != Player.none && MiniWinner(0, 0) == MiniWinner(0, 1) && MiniWinner(0, 1) == MiniWinner(0, 2))
                return MiniWinner(0, 0);

            if (MiniWinner(1, 0) != Player.none && MiniWinner(1, 0) == MiniWinner(1, 1) && MiniWinner(1, 1) == MiniWinner(1, 2))
                return MiniWinner(1, 0);

            if (MiniWinner(2, 0) != Player.none && MiniWinner(2, 0) == MiniWinner(2, 1) && MiniWinner(2, 1) == MiniWinner(2, 2))
                return MiniWinner(2, 0);

            if (MiniWinner(0, 0) != Player.none && MiniWinner(0, 0) == MiniWinner(1, 1) && MiniWinner(1, 1) == MiniWinner(2, 2))
                return MiniWinner(0, 0);

            if (MiniWinner(0, 2) != Player.none && MiniWinner(0, 2) == MiniWinner(1, 1) && MiniWinner(1, 1) == MiniWinner(2, 0))
                return MiniWinner(0, 2);

            return Player.none;
        }

        // Checks whether the game is finished
        public bool GameOver()
        {
            if (Winner() != Player.none) return true;

            // FIXME: write a function that checks there are no more legal moves
            // This might be possible by checking if every cell in the board is full

            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                    if (MiniWinner(x, y) == Player.none && !MiniFull(x, y))
                        return false;
            return true;
        }

        // Displays the board
        public override string ToString()
        {
            // FIXME: write a function that neatly presents the game

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

                    switch (board[x, y])
                    {
                        case Player.X:
                            output += "X";
                            break;
                        case Player.O:
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

        // FIXME (optional): include any helper functions
        Player MiniWinner(MiniCoord coord)
        {
            return MiniWinner(coord.X, coord.Y);
        }
        Player MiniWinner(int x, int y)
        {
            if (board[3 * x + 0, 3 * y + 0] != Player.none && board[3 * x + 0, 3 * y + 0] == board[3 * x + 1, 3 * y + 0] && board[3 * x + 1, 3 * y + 0] == board[3 * x + 2, 3 * y + 0])
                return board[3 * x + 0, 3 * y + 0];

            if (board[3 * x + 0, 3 * y + 1] != Player.none && board[3 * x + 0, 3 * y + 1] == board[3 * x + 1, 3 * y + 1] && board[3 * x + 1, 3 * y + 1] == board[3 * x + 2, 3 * y + 1])
                return board[3 * x + 0, 3 * y + 1];

            if (board[3 * x + 0, 3 * y + 2] != Player.none && board[3 * x + 0, 3 * y + 2] == board[3 * x + 1, 3 * y + 2] && board[3 * x + 1, 3 * y + 2] == board[3 * x + 2, 3 * y + 2])
                return board[3 * x + 0, 3 * y + 2];

            if (board[3 * x + 0, 3 * y + 0] != Player.none && board[3 * x + 0, 3 * y + 0] == board[3 * x + 0, 3 * y + 1] && board[3 * x + 0, 3 * y + 1] == board[3 * x + 0, 3 * y + 2])
                return board[3 * x + 0, 3 * y + 0];

            if (board[3 * x + 1, 3 * y + 0] != Player.none && board[3 * x + 1, 3 * y + 0] == board[3 * x + 1, 3 * y + 1] && board[3 * x + 1, 3 * y + 1] == board[3 * x + 1, 3 * y + 2])
                return board[3 * x + 1, 3 * y + 0];

            if (board[3 * x + 2, 3 * y + 0] != Player.none && board[3 * x + 2, 3 * y + 0] == board[3 * x + 2, 3 * y + 1] && board[3 * x + 2, 3 * y + 1] == board[3 * x + 2, 3 * y + 2])
                return board[3 * x + 2, 3 * y + 0];

            if (board[3 * x + 0, 3 * y + 0] != Player.none && board[3 * x + 0, 3 * y + 0] == board[3 * x + 1, 3 * y + 1] && board[3 * x + 1, 3 * y + 1] == board[3 * x + 2, 3 * y + 2])
                return board[3 * x + 0, 3 * y + 0];

            if (board[3 * x + 0, 3 * y + 2] != Player.none && board[3 * x + 0, 3 * y + 2] == board[3 * x + 1, 3 * y + 1] && board[3 * x + 1, 3 * y + 1] == board[3 * x + 2, 3 * y + 0])
                return board[3 * x + 0, 3 * y + 2];

            return Player.none;
        }

        public bool MiniFull(int x, int y)
        {
            for (int i = 0; i < 3u; i++)
                for (int j = 0; j < 3; j++)
                    if (board[3 * x + i, 3 * y + j].IsVacant())
                        return false;
            return true;
        }

        bool MiniFull(MiniCoord coord)
        {
            return MiniFull(coord.X, coord.Y);
        }
    }

    // Class for creating the Tree of ways the game can be played
    class Node
    {
        // Keeps track of its position in the tree
        public Node parent;
        public Node[] children;
        public State state = new State();

        // Keeps track of how the algorithm uses this node
        public int visits = 1;
        public int score;

        static readonly Random rand = new Random();
        public Node RandomChild()
        {
            List<int> possibleMoves = new List<int>();
            for (int possibleMove = 0; possibleMove < Move.Range; possibleMove++)
                if (state.MoveIsLegal(possibleMove))
                    possibleMoves.Add(possibleMove);
            int next = possibleMoves[rand.Next(possibleMoves.Count)];
            if (children == null)
                return new Node { state = new State(state, next) };
            return children[next];
        }

        public double Weight()
        {
            return (double)score / visits;
        }

        public override string ToString()
        {
            return score.ToString() + " / " + visits.ToString();
        }
    }
}
