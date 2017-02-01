namespace A_star_pathfinder
{
    partial class DrawingForm
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
            this.newMazeButton = new System.Windows.Forms.Button();
            this.startStopButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // newMazeButton
            // 
            this.newMazeButton.Location = new System.Drawing.Point(742, 12);
            this.newMazeButton.Name = "newMazeButton";
            this.newMazeButton.Size = new System.Drawing.Size(75, 23);
            this.newMazeButton.TabIndex = 0;
            this.newMazeButton.Text = "New maze";
            this.newMazeButton.UseVisualStyleBackColor = true;
            this.newMazeButton.Click += new System.EventHandler(this.newMazeButton_Click);
            // 
            // startStopButton
            // 
            this.startStopButton.Location = new System.Drawing.Point(742, 41);
            this.startStopButton.Name = "startStopButton";
            this.startStopButton.Size = new System.Drawing.Size(75, 23);
            this.startStopButton.TabIndex = 1;
            this.startStopButton.Text = "Start";
            this.startStopButton.UseVisualStyleBackColor = true;
            // 
            // DrawingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 616);
            this.Controls.Add(this.startStopButton);
            this.Controls.Add(this.newMazeButton);
            this.Name = "DrawingForm";
            this.Text = "A* Pathfinding Algorithm";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawingForm_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button newMazeButton;
        private System.Windows.Forms.Button startStopButton;
    }
}

