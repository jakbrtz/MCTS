using System;
using System.Collections.Generic;

namespace MCTS
{
    /// <summary>
    /// A node that contains the state of a game, where the game is turn based perfect information game with no random elements
    /// The nodes form a tree that is explored in the PickNextMove() method
    /// Each move is represented by an index
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// The node the lead to this node
        /// </summary>
        public Node Parent { get; private set; }

        /// <summary>
        /// Every node that this could lead to
        /// </summary>
        public Node[] Children { get; private set; }

        static private readonly Random rand = new Random();

        /// <summary>
        /// Get a random child from this node
        /// </summary>
        private Node RandomChild()
        {
            List<int> possibleMoves = new List<int>();
            int numberOfMoves = NumberOfMoves();
            for (int possibleMove = 0; possibleMove < numberOfMoves; possibleMove++)
                if (MoveIsLegal(possibleMove))
                    possibleMoves.Add(possibleMove);
            int next = possibleMoves[rand.Next(possibleMoves.Count)];
            if (Children == null)
                return GetNextState(next);
            return Children[next];
        }

        // Keeps track of how many times the algorithm uses this node
        private int visits = 1;
        private int score;

        /// <summary>
        /// How likely this node is to lead to a success from the perspective of the Parent's active player
        /// </summary>
        public double Weight()
        {
            return (double)score / visits;
        }

        public override string ToString()
        {
            return $"{score} / {visits}";
        }

        /// <summary>
        /// The index of the current player
        /// </summary>
        public int ActivePlayer;
        /// <summary>
        /// Create the next that would result from a certain move being made. (Do not place it in the tree)
        /// </summary>
        public abstract Node GetNextState(int move);
        /// <summary>
        /// Check if the move is allowed to be made from this node
        /// </summary>
        public abstract bool MoveIsLegal(int move);
        /// <summary>
        /// Each move from this node is given a 0-based index. This should return 1+ the largest index
        /// This should result in a constant value. It doesn't matter if not all of the indexes refer to legal moves
        /// </summary>
        public abstract int NumberOfMoves();
        /// <summary>
        /// The index of the player that has won the game in this position, or -1 if there is no winner
        /// </summary>
        public abstract int Winner();
        /// <summary>
        /// Checks if the game still in progress. The reason a game would not be in progress is because someone has won, or no legal move can be made
        /// </summary>
        public virtual bool GameInProgress()
        {
            if (Winner() != -1) return false;
            int numberOfMoves = NumberOfMoves();
            for (int move = 0; move < numberOfMoves; move++)
                if (MoveIsLegal(move))
                    return true;
            return false;
        }

        /// <summary>
        /// There exists a strategy that this player could use that would guarantee a win. If -1, then a strategy hasn't been found.
        /// </summary>
        public int PlayerThatCanForceWin { get; private set; } = -1;

        /// <summary>
        /// Pick a move for the current player to made. This algorithm uses the Monte Carlos Tree Search Algorithm (MCTS), using the Upper Confidence Bound (UCB) 
        /// https://en.wikipedia.org/wiki/Monte_Carlo_tree_search
        /// </summary>
        /// <param name="Simulations">The number of times to search the tree</param>
        /// <param name="TuningParameter">A weight which determines whether this algorithm favours planning ahead (low value) or experimenting (high values)</param>
        /// <returns>An index which represents a recommended move</returns>
        public int PickNextMove(int Simulations = 10000, double TuningParameter = 1.4142)
        {
            if (PlayerThatCanForceWin == ActivePlayer) 
                Simulations = 0;

            for (int i = 0; i < Simulations; i++)
            {
                //Selection
                Node current = this;
                while (current.Children != null && current.PlayerThatCanForceWin != ActivePlayer)
                {
                    double bestUCB = double.MinValue;
                    Node nextNode = null;
                    Node child;
                    for (int move = 0; move < current.Children.Length; move++)
                    {
                        child = current.Children[move];
                        if (child == null) continue;
                        double childUCB = child.Weight() + TuningParameter * Math.Sqrt(Math.Log(current.visits) / child.visits);
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
                if (current.GameInProgress() && current.PlayerThatCanForceWin != ActivePlayer)
                {
                    //Expansion
                    current.Children = new Node[current.NumberOfMoves()];
                    for (int possibleMove = 0; possibleMove < current.Children.Length; possibleMove++)
                    {
                        if (current.MoveIsLegal(possibleMove))
                        {
                            current.Children[possibleMove] = current.GetNextState(possibleMove);
                            current.Children[possibleMove].Parent = current;
                        }
                    }
                    current = current.RandomChild();
                    //Simulation
                    simulated = current;
                    while (simulated.GameInProgress())
                        simulated = simulated.RandomChild();
                }
                int winner = simulated.Winner();
                //Backpropogation
                bool checkingForForcedWin = simulated == current;
                if (checkingForForcedWin)
                {
                    if (winner == -1)
                        winner = current.PlayerThatCanForceWin;
                    else
                        current.PlayerThatCanForceWin = winner;
                }
                while (current != this)
                {
                    current.visits++;
                    if (current.Parent.ActivePlayer == winner)
                    {
                        current.score++;
                        if (checkingForForcedWin)
                        {
                            current.Parent.PlayerThatCanForceWin = winner;
                        }
                    }
                    else if (winner != -1)
                    {
                        current.score--;
                        if (checkingForForcedWin)
                        {
                            bool AllSiblingsForceWin()
                            {
                                for (int s = 0; s < current.Parent.Children.Length; s++)
                                    if (current.Parent.Children[s] != null && current.Parent.Children[s].PlayerThatCanForceWin != winner)
                                        return false;
                                return true;
                            }

                            if (AllSiblingsForceWin())
                                current.Parent.PlayerThatCanForceWin = winner;
                            else
                                checkingForForcedWin = false;
                        }
                    }
                    current = current.Parent;
                }
                current.visits++;
            }

            //Pick best score (ignoring UCB)
            double bestWeight = double.MinValue;
            int selectedMove = -1;

            for (int possibleMove = 0; possibleMove < Children.Length; possibleMove++)
            {
                Node child = Children[possibleMove];
                if (child != null)
                {
                    if (child.PlayerThatCanForceWin == ActivePlayer)
                    {
                        return possibleMove;
                    }
                    if (child.Weight() > bestWeight) 
                    {
                        bestWeight = child.Weight();
                        selectedMove = possibleMove;
                    }
                }
            }
            if (selectedMove == -1) throw new IndexOutOfRangeException("No move was selected");
            return selectedMove;
        }

        /// <summary>
        /// Attempt to get the existing child node, or create one if it doesn't exist
        /// </summary>
        /// <param name="move">The index that represents a move</param>
        /// <returns>The next node</returns>
        public Node DoMove(int move)
        {
            if (Children != null && Children[move] != null)
            {
                // If the next move has already been considered, move to that node
                return Children[move];
            }
            else
            {
                // If the next move hasn't been considered, create a new node to represent the new game state
                Node nextNode = GetNextState(move);
                nextNode.Parent = this;
                Children = new Node[NumberOfMoves()];
                Children[move] = nextNode;
                return nextNode;
            }
        }
    }

    public abstract class ConsoleGamePlay
    {
        /// <summary>
        /// Starts a game from a given Node
        /// </summary>
        public void Play(Node gameNode)
        {
            Draw(gameNode);

            while (gameNode.GameInProgress())
            {
                int player = gameNode.ActivePlayer;
                int nextMove = -1;

                if (IsHumanTurn(gameNode))
                {
                    while (!gameNode.MoveIsLegal(nextMove) || nextMove < 0 || nextMove >= gameNode.NumberOfMoves())
                    {
                        nextMove = GetHumanMove(gameNode);
                    }
                }
                else
                {
                    nextMove = GetComputerMove(gameNode);
                }

                gameNode = gameNode.DoMove(nextMove);
                Draw(gameNode, player, nextMove);
            }

            ReportWinner(gameNode);
        }

        /// <summary>
        /// Draws node and extra information about the most recent move
        /// </summary>
        void Draw(Node node, int recentPlayer = -1, int recentMove = -1)
        {
            Console.Clear();
            Console.WriteLine(BoardToString(node));
            Console.WriteLine();
            Console.WriteLine();
            if (recentMove != -1)
            {
                Console.WriteLine(GetPlayerName(recentPlayer) + " played " + MoveToString(recentMove));
            }
        }

        /// <summary>
        /// Gets a description of a player given their index
        /// </summary>
        public abstract string GetPlayerName(int player);

        /// <summary>
        /// Gets a string description of the board
        /// </summary>
        public abstract string BoardToString(Node node);

        /// <summary>
        /// Gets a description of a move from its index
        /// </summary>
        public abstract string MoveToString(int move);

        /// <summary>
        /// Asks the user to enter data that represents a move, and convert it to an index
        /// </summary>
        public abstract int GetHumanMove(Node node);

        /// <summary>
        /// Checks whether the human should go next or the computer
        /// </summary>
        public virtual bool IsHumanTurn(Node node)
        {
            return node.ActivePlayer == 0;
        }

        /// <summary>
        /// Get the computer to pick a move
        /// </summary>
        public virtual int GetComputerMove(Node node)
        {
            Console.WriteLine("Thinking...");
            return node.PickNextMove();
        }

        /// <summary>
        /// Display a message stating the game is over
        /// </summary>
        public virtual void ReportWinner(Node node)
        {
            Console.WriteLine(GetPlayerName(node.Winner()) + " wins");
        }
    }
}
