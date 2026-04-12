namespace Padeler
{
    partial class PushNotificationForm
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
            this.lblMessage = new System.Windows.Forms.Label();
            this.lnlblOk = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblMessage.ForeColor = System.Drawing.Color.White;
            this.lblMessage.Location = new System.Drawing.Point(10, 15);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(220, 50);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "MATCH!";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMessage.Click += new System.EventHandler(this.lblMessage_Click);
            // 
            // lnlblOk
            // 
            this.lnlblOk.ActiveLinkColor = System.Drawing.Color.White;
            this.lnlblOk.AutoSize = true;
            this.lnlblOk.BackColor = System.Drawing.Color.Transparent;
            this.lnlblOk.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lnlblOk.LinkColor = System.Drawing.Color.WhiteSmoke;
            this.lnlblOk.Location = new System.Drawing.Point(260, 55);
            this.lnlblOk.Name = "lnlblOk";
            this.lnlblOk.Size = new System.Drawing.Size(29, 20);
            this.lnlblOk.TabIndex = 1;
            this.lnlblOk.TabStop = true;
            this.lnlblOk.Text = "OK";
            this.lnlblOk.VisitedLinkColor = System.Drawing.Color.WhiteSmoke;
            this.lnlblOk.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnlblOk_LinkClicked);
            // 
            // PushNotificationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(43)))), ((int)(((byte)(43)))));
            this.ClientSize = new System.Drawing.Size(320, 90);
            this.Controls.Add(this.lnlblOk);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PushNotificationForm";
            this.Opacity = 0.8D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "PushNotificationForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.PushNotificationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.LinkLabel lnlblOk;
    }
}