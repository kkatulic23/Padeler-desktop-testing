namespace Padeler
{
    partial class GradeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GradeForm));
            this.nudGrade = new System.Windows.Forms.NumericUpDown();
            this.rtbComment = new System.Windows.Forms.RichTextBox();
            this.btnSend = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrade)).BeginInit();
            this.SuspendLayout();
            // 
            // nudGrade
            // 
            this.nudGrade.Location = new System.Drawing.Point(12, 12);
            this.nudGrade.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudGrade.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudGrade.Name = "nudGrade";
            this.nudGrade.ReadOnly = true;
            this.nudGrade.Size = new System.Drawing.Size(320, 22);
            this.nudGrade.TabIndex = 0;
            this.nudGrade.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // rtbComment
            // 
            this.rtbComment.Location = new System.Drawing.Point(12, 40);
            this.rtbComment.Name = "rtbComment";
            this.rtbComment.Size = new System.Drawing.Size(320, 92);
            this.rtbComment.TabIndex = 1;
            this.rtbComment.Text = "";
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(252, 138);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(80, 33);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // GradeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(344, 183);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.rtbComment);
            this.Controls.Add(this.nudGrade);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GradeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Grade user";
            ((System.ComponentModel.ISupportInitialize)(this.nudGrade)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudGrade;
        private System.Windows.Forms.RichTextBox rtbComment;
        private System.Windows.Forms.Button btnSend;
    }
}