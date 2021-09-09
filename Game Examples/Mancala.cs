using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using MCTS;

namespace Examples
{
    public partial class Mancala : Form
    {
        public Mancala()
        {
            InitializeComponent();
        }

        int computerPlayer = 1;

        private void Mancala_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Click \"new game\" to start");
        }

        private void BTNrestartH_Click(object sender, EventArgs e)
        {
            computerPlayer = 1;
            backgroundWorker1.RunWorkerAsync();
        }

        private void BTNrestartC_Click(object sender, EventArgs e)
        {
            computerPlayer = 0;
            backgroundWorker1.RunWorkerAsync();
        }

        private readonly ManualResetEvent waitForHumanMove = new ManualResetEvent(false);
        private int moveFromButton = -1;

        void Draw()
        {
            Invoke((MethodInvoker)delegate
            {
                foreach (Button button in new Button[] { BTN10, BTN11, BTN12, BTN13, BTN14, BTN15, BTN16, BTN20, BTN21, BTN22, BTN23, BTN24, BTN25, BTN26 })
                    button.Text = gameNode.board[CBXflip.Checked == (computerPlayer == 1) == (button.Name[3] == '1') ? 1 : 0, button.Name[4] - '0'].ToString();
            });
        }

        void Log(string s1, string s2, string s3)
        {
            Invoke((MethodInvoker)delegate
            {
                dataGridView1.Rows.Add(s1, s2, s3);
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
            });
        }

        private void BTN_Click(object sender, EventArgs e)
        {
            if (CBXflip.Checked != ((sender as Button).Name[3] == '1'))
            {
                moveFromButton = (sender as Button).Name[4] - '0' - 1;
                waitForHumanMove.Set();
            }
        }

        MancalaNode gameNode;

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            gameNode = new MancalaNode();
            Draw();

            while (gameNode.GameInProgress())
            {
                int nextMove = -1;

                if (gameNode.ActivePlayer == computerPlayer)
                {
                    nextMove = gameNode.PickNextMove((int)NUDsimulations.Value);
                }
                else
                {
                    while (!gameNode.MoveIsLegal(nextMove))
                    {
                        waitForHumanMove.WaitOne();
                        nextMove = moveFromButton;
                        moveFromButton = -1;
                    }
                }

                int previousPlayer = gameNode.ActivePlayer;
                gameNode = gameNode.DoMove(nextMove) as MancalaNode;
                Draw();
                Log(previousPlayer == computerPlayer ? "Computer" : "Human",
                    (nextMove + 1).ToString() + (gameNode.ActivePlayer == previousPlayer ? ", Go Again" : ""),
                    (gameNode.Weight() * (previousPlayer == computerPlayer ? 50 : -50) + 50).ToString("0.00") + "%"); // weight usually displays a number from -1 to 1
            }

            int scoreC = gameNode.board[computerPlayer, 0];
            int scoreH = gameNode.board[1 - computerPlayer, 0];
            Log("", "", "");
            if (scoreC > scoreH)
                Log("Computer", "Wins", scoreC.ToString() + " - " + scoreH.ToString());
            else if (scoreH > scoreC)
                Log("Human", "Wins", scoreH.ToString() + " - " + scoreC.ToString());
            else
                Log("Tie", "", scoreH.ToString() + " - " + scoreC.ToString());
            Log("", "", "");
        }

        private void CBXflip_CheckedChanged(object sender, EventArgs e)
        {
            Draw();
        }
    }

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
                player = (player + 1) % 2;
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
    }
}
