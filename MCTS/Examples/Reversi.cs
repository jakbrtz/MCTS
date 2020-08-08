using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MCTS;

namespace MCTSexample
{
    public partial class Reversi : Form
    {
        ReversiNode gamenode;

        public Reversi()
        {
            InitializeComponent();
        }

        void NewGame()
        {
            gamenode = new ReversiNode();
            Draw("New game");
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!gamenode.GameInProgress())
            {
                NewGame();
                return;
            }
            if ((gamenode.ActivePlayer == 0 && !checkBox0.Checked) || (gamenode.ActivePlayer == 1 && !checkBox1.Checked))
            {
                int x = ReversiNode.size * e.Location.X / panel1.Width;
                int y = ReversiNode.size * e.Location.Y / panel1.Height;
                y = ReversiNode.size - 1 - y;
                int move = ReversiNode.size * x + y;
                if (!gamenode.MoveIsLegal(move)) return;
                gamenode = gamenode.DoMove(move) as ReversiNode;
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (((gamenode.ActivePlayer == 0 && checkBox0.Checked) || (gamenode.ActivePlayer == 1 && checkBox1.Checked)) && gamenode.GameInProgress())
            {
                Draw("Thinking...");
                int move = gamenode.PickNextMove((int)numericUpDown1.Value);
                gamenode = gamenode.DoMove(move) as ReversiNode;
                Thread.Sleep(10);
            }
            string msg = "Your turn";
            if (!gamenode.GameInProgress())
            {
                switch (gamenode.Winner())
                {
                    case 0:
                        msg = "White wins";
                        break;
                    case 1:
                        msg = "Black wins";
                        break;
                    default:
                        msg = "It's a tie";
                        break;
                }
            }
            Draw(msg);
        }

        void Draw(string msg)
        {
            Bitmap bmp = new Bitmap(panel1.Width, panel1.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Green);
                for (int x = 0; x < ReversiNode.size; x++)
                {
                    for (int y = 0; y < ReversiNode.size; y++)
                    {
                        Rectangle rect = new Rectangle(x * bmp.Width / ReversiNode.size, (ReversiNode.size - 1 - y) * bmp.Height / ReversiNode.size, bmp.Width / ReversiNode.size, bmp.Height / ReversiNode.size);
                        g.DrawRectangle(Pens.Black, rect);
                        if (gamenode.GetPiece(x, y)==0)
                            g.FillEllipse(Brushes.White, rect);
                        if (gamenode.GetPiece(x, y) == 1)
                            g.FillEllipse(Brushes.Black, rect);
                    }
                }
            }
            Invoke((MethodInvoker)delegate
            {
                panel1.BackgroundImage = bmp;
                Response.Text = msg;
            });
        }

        private void Reversi_Load(object sender, EventArgs e)
        {
            NewGame();
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy) backgroundWorker1.RunWorkerAsync();
        }

        private void Reversi_Resize(object sender, EventArgs e)
        {
            Draw(Response.Text);
        }
    }

    public class ReversiNode : Node
    {
        public const int size = 8;
        readonly int[,] board;

        public int GetPiece(int x, int y)
        {
            return board[x, y];
        }

        public ReversiNode(int[,] board = null, int turn = 0)
        {
            if (board == null)
            {
                board = new int[size, size];
                for (int x = 0; x < size; x++)
                    for (int y = 0; y < size; y++)
                        board[x, y] = -1;
                board[size / 2, size / 2] = 0;
                board[size / 2 - 1, size / 2] = 1;
                board[size / 2, size / 2 - 1] = 1;
                board[size / 2 - 1, size / 2 - 1] = 0;
            }
            this.board = board;
            ActivePlayer = turn;
        }

        public override Node GetNextState(int move)
        {
            int x = move / size;
            int y = move % size;
            int[,] nextBoard = (int[,])board.Clone();
            PerformFlipsIfLegal(x, y, 1, 0, nextBoard);
            PerformFlipsIfLegal(x, y, 1, 1, nextBoard);
            PerformFlipsIfLegal(x, y, 0, 1, nextBoard);
            PerformFlipsIfLegal(x, y, -1, 1, nextBoard);
            PerformFlipsIfLegal(x, y, -1, 0, nextBoard);
            PerformFlipsIfLegal(x, y, -1, -1, nextBoard);
            PerformFlipsIfLegal(x, y, 0, -1, nextBoard);
            PerformFlipsIfLegal(x, y, 1, -1, nextBoard);
            ReversiNode nextState = new ReversiNode(nextBoard, 1 - ActivePlayer);
            if (nextState.HasLegalMove()) return nextState;
            ReversiNode alternateNextState = new ReversiNode(nextBoard, ActivePlayer);
            if (alternateNextState.HasLegalMove()) return alternateNextState;
            return nextState;

        }

        bool CheckForFlips(int x, int y, int dx, int dy, int[,] board)
        {
            x += dx;
            y += dy;
            if (x < 0 || x >= size || y < 0 || y >= size || board[x, y] != 1 - ActivePlayer)
                return false;
            x += dx;
            y += dy;
            while (x >= 0 && x < size && y >= 0 && y < size)
            {
                if (board[x, y] == ActivePlayer)
                    return true;
                if (board[x, y] == -1)
                    return false;
                x += dx;
                y += dy;
            }
            return false;
        }

        void PerformFlipsIfLegal(int x, int y, int dx, int dy, int[,] board)
        {
            if (CheckForFlips(x, y, dx, dy, board))
            {
                do
                {
                    board[x, y] = ActivePlayer;
                    x += dx;
                    y += dy;
                } while (board[x, y] != ActivePlayer);
            }
        }

        bool HasLegalMove()
        {
            for (int move = 0; move < size * size; move++)
                if (MoveIsLegal(move))
                    return true;
            return false;
        }

        public override bool MoveIsLegal(int move)
        {
            int x = move / size;
            int y = move % size;
            if (board[x, y] != -1) return false;
            return 
                CheckForFlips(x, y, 1, 0, board) ||
                CheckForFlips(x, y, 1, 1, board) ||
                CheckForFlips(x, y, 0, 1, board) ||
                CheckForFlips(x, y, -1, 1, board) ||
                CheckForFlips(x, y, -1, 0, board) ||
                CheckForFlips(x, y, -1, -1, board) ||
                CheckForFlips(x, y, 0, -1, board) ||
                CheckForFlips(x, y, 1, -1, board);
        }

        public override int NumberOfMoves()
        {
            return size * size;
        }

        public override int Winner()
        {
            if (GameInProgress()) return -1;
            int[] pieceCount = new int[2];
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    if (board[x, y] != -1)
                        pieceCount[board[x, y]]++;
            if (pieceCount[0] > pieceCount[1]) return 0;
            if (pieceCount[0] < pieceCount[1]) return 1;
            return -1;
        }

        public override bool GameInProgress()
        {
            for (int move = 0; move < size * size; move++)
                if (MoveIsLegal(move))
                    return true;
            return false;
        }
    }
}
