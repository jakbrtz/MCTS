namespace MCTSexample
{
    partial class Mancala
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BTNrestartH = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.BTNrestartC = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Turn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GameMove = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Confidence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CBXflip = new System.Windows.Forms.CheckBox();
            this.NUDsimulations = new System.Windows.Forms.NumericUpDown();
            this.BTN10 = new System.Windows.Forms.Button();
            this.BTN11 = new System.Windows.Forms.Button();
            this.BTN12 = new System.Windows.Forms.Button();
            this.BTN13 = new System.Windows.Forms.Button();
            this.BTN14 = new System.Windows.Forms.Button();
            this.BTN15 = new System.Windows.Forms.Button();
            this.BTN16 = new System.Windows.Forms.Button();
            this.BTN20 = new System.Windows.Forms.Button();
            this.BTN21 = new System.Windows.Forms.Button();
            this.BTN22 = new System.Windows.Forms.Button();
            this.BTN23 = new System.Windows.Forms.Button();
            this.BTN24 = new System.Windows.Forms.Button();
            this.BTN25 = new System.Windows.Forms.Button();
            this.BTN26 = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUDsimulations)).BeginInit();
            this.SuspendLayout();
            // 
            // BTNrestartH
            // 
            this.BTNrestartH.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BTNrestartH.Location = new System.Drawing.Point(180, 239);
            this.BTNrestartH.Name = "BTNrestartH";
            this.BTNrestartH.Size = new System.Drawing.Size(162, 23);
            this.BTNrestartH.TabIndex = 59;
            this.BTNrestartH.Text = "New Game (human first)";
            this.BTNrestartH.UseVisualStyleBackColor = true;
            this.BTNrestartH.Click += new System.EventHandler(this.BTNrestartH_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 244);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 56;
            this.label1.Text = "Difficulty";
            // 
            // BTNrestartC
            // 
            this.BTNrestartC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BTNrestartC.Location = new System.Drawing.Point(180, 265);
            this.BTNrestartC.Name = "BTNrestartC";
            this.BTNrestartC.Size = new System.Drawing.Size(162, 23);
            this.BTNrestartC.TabIndex = 60;
            this.BTNrestartC.Text = "New Game (computer first)";
            this.BTNrestartC.UseVisualStyleBackColor = true;
            this.BTNrestartC.Click += new System.EventHandler(this.BTNrestartC_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Turn,
            this.GameMove,
            this.Confidence});
            this.dataGridView1.Location = new System.Drawing.Point(12, 97);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.Size = new System.Drawing.Size(330, 136);
            this.dataGridView1.TabIndex = 55;
            // 
            // Turn
            // 
            this.Turn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Turn.HeaderText = "Player";
            this.Turn.Name = "Turn";
            this.Turn.Width = 61;
            // 
            // GameMove
            // 
            this.GameMove.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.GameMove.HeaderText = "Move";
            this.GameMove.Name = "GameMove";
            // 
            // Confidence
            // 
            this.Confidence.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Confidence.HeaderText = "Confidence";
            this.Confidence.Name = "Confidence";
            this.Confidence.Width = 86;
            // 
            // CBXflip
            // 
            this.CBXflip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CBXflip.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CBXflip.Location = new System.Drawing.Point(12, 267);
            this.CBXflip.Name = "CBXflip";
            this.CBXflip.Size = new System.Drawing.Size(162, 20);
            this.CBXflip.TabIndex = 54;
            this.CBXflip.Text = "Flip Board:";
            this.CBXflip.UseVisualStyleBackColor = true;
            this.CBXflip.CheckedChanged += new System.EventHandler(this.CBXflip_CheckedChanged);
            // 
            // NUDsimulations
            // 
            this.NUDsimulations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NUDsimulations.Increment = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.NUDsimulations.Location = new System.Drawing.Point(84, 242);
            this.NUDsimulations.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.NUDsimulations.Name = "NUDsimulations";
            this.NUDsimulations.Size = new System.Drawing.Size(90, 20);
            this.NUDsimulations.TabIndex = 53;
            this.NUDsimulations.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // BTN10
            // 
            this.BTN10.Location = new System.Drawing.Point(306, 12);
            this.BTN10.Name = "BTN10";
            this.BTN10.Size = new System.Drawing.Size(36, 78);
            this.BTN10.TabIndex = 39;
            this.BTN10.UseVisualStyleBackColor = true;
            // 
            // BTN11
            // 
            this.BTN11.Location = new System.Drawing.Point(264, 54);
            this.BTN11.Name = "BTN11";
            this.BTN11.Size = new System.Drawing.Size(36, 36);
            this.BTN11.TabIndex = 40;
            this.BTN11.UseVisualStyleBackColor = true;
            this.BTN11.Click += new System.EventHandler(this.BTN_Click);
            // 
            // BTN12
            // 
            this.BTN12.Location = new System.Drawing.Point(222, 54);
            this.BTN12.Name = "BTN12";
            this.BTN12.Size = new System.Drawing.Size(36, 36);
            this.BTN12.TabIndex = 41;
            this.BTN12.UseVisualStyleBackColor = true;
            this.BTN12.Click += new System.EventHandler(this.BTN_Click);
            // 
            // BTN13
            // 
            this.BTN13.Location = new System.Drawing.Point(180, 54);
            this.BTN13.Name = "BTN13";
            this.BTN13.Size = new System.Drawing.Size(36, 36);
            this.BTN13.TabIndex = 42;
            this.BTN13.UseVisualStyleBackColor = true;
            this.BTN13.Click += new System.EventHandler(this.BTN_Click);
            // 
            // BTN14
            // 
            this.BTN14.Location = new System.Drawing.Point(138, 54);
            this.BTN14.Name = "BTN14";
            this.BTN14.Size = new System.Drawing.Size(36, 36);
            this.BTN14.TabIndex = 43;
            this.BTN14.UseVisualStyleBackColor = true;
            this.BTN14.Click += new System.EventHandler(this.BTN_Click);
            // 
            // BTN15
            // 
            this.BTN15.Location = new System.Drawing.Point(96, 54);
            this.BTN15.Name = "BTN15";
            this.BTN15.Size = new System.Drawing.Size(36, 36);
            this.BTN15.TabIndex = 44;
            this.BTN15.UseVisualStyleBackColor = true;
            this.BTN15.Click += new System.EventHandler(this.BTN_Click);
            // 
            // BTN16
            // 
            this.BTN16.Location = new System.Drawing.Point(54, 54);
            this.BTN16.Name = "BTN16";
            this.BTN16.Size = new System.Drawing.Size(36, 36);
            this.BTN16.TabIndex = 45;
            this.BTN16.UseVisualStyleBackColor = true;
            this.BTN16.Click += new System.EventHandler(this.BTN_Click);
            // 
            // BTN20
            // 
            this.BTN20.Location = new System.Drawing.Point(12, 12);
            this.BTN20.Name = "BTN20";
            this.BTN20.Size = new System.Drawing.Size(36, 78);
            this.BTN20.TabIndex = 46;
            this.BTN20.UseVisualStyleBackColor = true;
            // 
            // BTN21
            // 
            this.BTN21.Location = new System.Drawing.Point(54, 12);
            this.BTN21.Name = "BTN21";
            this.BTN21.Size = new System.Drawing.Size(36, 36);
            this.BTN21.TabIndex = 47;
            this.BTN21.UseVisualStyleBackColor = true;
            this.BTN21.Click += new System.EventHandler(this.BTN_Click);
            // 
            // BTN22
            // 
            this.BTN22.Location = new System.Drawing.Point(96, 12);
            this.BTN22.Name = "BTN22";
            this.BTN22.Size = new System.Drawing.Size(36, 36);
            this.BTN22.TabIndex = 48;
            this.BTN22.UseVisualStyleBackColor = true;
            this.BTN22.Click += new System.EventHandler(this.BTN_Click);
            // 
            // BTN23
            // 
            this.BTN23.Location = new System.Drawing.Point(138, 12);
            this.BTN23.Name = "BTN23";
            this.BTN23.Size = new System.Drawing.Size(36, 36);
            this.BTN23.TabIndex = 49;
            this.BTN23.UseVisualStyleBackColor = true;
            this.BTN23.Click += new System.EventHandler(this.BTN_Click);
            // 
            // BTN24
            // 
            this.BTN24.Location = new System.Drawing.Point(180, 12);
            this.BTN24.Name = "BTN24";
            this.BTN24.Size = new System.Drawing.Size(36, 36);
            this.BTN24.TabIndex = 50;
            this.BTN24.UseVisualStyleBackColor = true;
            this.BTN24.Click += new System.EventHandler(this.BTN_Click);
            // 
            // BTN25
            // 
            this.BTN25.Location = new System.Drawing.Point(222, 12);
            this.BTN25.Name = "BTN25";
            this.BTN25.Size = new System.Drawing.Size(36, 36);
            this.BTN25.TabIndex = 51;
            this.BTN25.UseVisualStyleBackColor = true;
            this.BTN25.Click += new System.EventHandler(this.BTN_Click);
            // 
            // BTN26
            // 
            this.BTN26.Location = new System.Drawing.Point(264, 12);
            this.BTN26.Name = "BTN26";
            this.BTN26.Size = new System.Drawing.Size(36, 36);
            this.BTN26.TabIndex = 52;
            this.BTN26.UseVisualStyleBackColor = true;
            this.BTN26.Click += new System.EventHandler(this.BTN_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            // 
            // Mancala
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 299);
            this.Controls.Add(this.BTNrestartH);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BTNrestartC);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.CBXflip);
            this.Controls.Add(this.NUDsimulations);
            this.Controls.Add(this.BTN10);
            this.Controls.Add(this.BTN11);
            this.Controls.Add(this.BTN12);
            this.Controls.Add(this.BTN13);
            this.Controls.Add(this.BTN14);
            this.Controls.Add(this.BTN15);
            this.Controls.Add(this.BTN16);
            this.Controls.Add(this.BTN20);
            this.Controls.Add(this.BTN21);
            this.Controls.Add(this.BTN22);
            this.Controls.Add(this.BTN23);
            this.Controls.Add(this.BTN24);
            this.Controls.Add(this.BTN25);
            this.Controls.Add(this.BTN26);
            this.Name = "Mancala";
            this.Text = "Mancala";
            this.Load += new System.EventHandler(this.Mancala_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUDsimulations)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BTNrestartH;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BTNrestartC;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox CBXflip;
        private System.Windows.Forms.NumericUpDown NUDsimulations;
        private System.Windows.Forms.Button BTN10;
        private System.Windows.Forms.Button BTN11;
        private System.Windows.Forms.Button BTN12;
        private System.Windows.Forms.Button BTN13;
        private System.Windows.Forms.Button BTN14;
        private System.Windows.Forms.Button BTN15;
        private System.Windows.Forms.Button BTN16;
        private System.Windows.Forms.Button BTN20;
        private System.Windows.Forms.Button BTN21;
        private System.Windows.Forms.Button BTN22;
        private System.Windows.Forms.Button BTN23;
        private System.Windows.Forms.Button BTN24;
        private System.Windows.Forms.Button BTN25;
        private System.Windows.Forms.Button BTN26;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Turn;
        private System.Windows.Forms.DataGridViewTextBoxColumn GameMove;
        private System.Windows.Forms.DataGridViewTextBoxColumn Confidence;
    }
}