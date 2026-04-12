namespace Padeler
{
    partial class HomeForm
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
            this.components = new System.ComponentModel.Container();
            this.tlpHome = new System.Windows.Forms.TableLayoutPanel();
            this.pnlCardContainer = new System.Windows.Forms.Panel();
            this.pnlBadges = new System.Windows.Forms.Panel();
            this.pnlPlayerCard = new System.Windows.Forms.Panel();
            this.pnlFrontCard = new System.Windows.Forms.Panel();
            this.pbInfoFront = new System.Windows.Forms.PictureBox();
            this.pbProfilePicture = new System.Windows.Forms.PictureBox();
            this.lblDistance = new System.Windows.Forms.Label();
            this.lblPlayer = new System.Windows.Forms.Label();
            this.lblYearsOld = new System.Windows.Forms.Label();
            this.pnlBackCard = new System.Windows.Forms.Panel();
            this.pbInfoBack = new System.Windows.Forms.PictureBox();
            this.lblPos = new System.Windows.Forms.Label();
            this.lblPosition = new System.Windows.Forms.Label();
            this.lblFreq = new System.Windows.Forms.Label();
            this.lblFrequency = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblLevelOfPlay = new System.Windows.Forms.Label();
            this.lblAbout = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.pbLike = new System.Windows.Forms.PictureBox();
            this.pbDisslike = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.flipTimer = new System.Windows.Forms.Timer(this.components);
            this.lblRating = new System.Windows.Forms.Label();
            this.tlpHome.SuspendLayout();
            this.pnlCardContainer.SuspendLayout();
            this.pnlPlayerCard.SuspendLayout();
            this.pnlFrontCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfoFront)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProfilePicture)).BeginInit();
            this.pnlBackCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfoBack)).BeginInit();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLike)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDisslike)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpHome
            // 
            this.tlpHome.ColumnCount = 1;
            this.tlpHome.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHome.Controls.Add(this.pnlCardContainer, 0, 1);
            this.tlpHome.Controls.Add(this.pnlButtons, 0, 2);
            this.tlpHome.Controls.Add(this.lblTitle, 0, 0);
            this.tlpHome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpHome.Location = new System.Drawing.Point(0, 0);
            this.tlpHome.Name = "tlpHome";
            this.tlpHome.RowCount = 3;
            this.tlpHome.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpHome.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHome.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpHome.Size = new System.Drawing.Size(800, 669);
            this.tlpHome.TabIndex = 0;
            this.tlpHome.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // pnlCardContainer
            // 
            this.pnlCardContainer.Controls.Add(this.pnlBadges);
            this.pnlCardContainer.Controls.Add(this.pnlPlayerCard);
            this.pnlCardContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCardContainer.Location = new System.Drawing.Point(3, 60);
            this.pnlCardContainer.Name = "pnlCardContainer";
            this.pnlCardContainer.Size = new System.Drawing.Size(794, 550);
            this.pnlCardContainer.TabIndex = 2;
            // 
            // pnlBadges
            // 
            this.pnlBadges.AutoScroll = true;
            this.pnlBadges.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlBadges.Location = new System.Drawing.Point(634, 0);
            this.pnlBadges.Name = "pnlBadges";
            this.pnlBadges.Size = new System.Drawing.Size(160, 550);
            this.pnlBadges.TabIndex = 1;
            // 
            // pnlPlayerCard
            // 
            this.pnlPlayerCard.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlPlayerCard.BackColor = System.Drawing.Color.White;
            this.pnlPlayerCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPlayerCard.Controls.Add(this.pnlFrontCard);
            this.pnlPlayerCard.Controls.Add(this.pnlBackCard);
            this.pnlPlayerCard.Location = new System.Drawing.Point(200, 40);
            this.pnlPlayerCard.Name = "pnlPlayerCard";
            this.pnlPlayerCard.Size = new System.Drawing.Size(331, 466);
            this.pnlPlayerCard.TabIndex = 0;
            // 
            // pnlFrontCard
            // 
            this.pnlFrontCard.Controls.Add(this.lblRating);
            this.pnlFrontCard.Controls.Add(this.pbInfoFront);
            this.pnlFrontCard.Controls.Add(this.pbProfilePicture);
            this.pnlFrontCard.Controls.Add(this.lblDistance);
            this.pnlFrontCard.Controls.Add(this.lblPlayer);
            this.pnlFrontCard.Controls.Add(this.lblYearsOld);
            this.pnlFrontCard.Location = new System.Drawing.Point(22, 17);
            this.pnlFrontCard.Name = "pnlFrontCard";
            this.pnlFrontCard.Size = new System.Drawing.Size(287, 430);
            this.pnlFrontCard.TabIndex = 1;
            // 
            // pbInfoFront
            // 
            this.pbInfoFront.Image = global::Padeler.Properties.Resources.information;
            this.pbInfoFront.Location = new System.Drawing.Point(255, 399);
            this.pbInfoFront.Name = "pbInfoFront";
            this.pbInfoFront.Size = new System.Drawing.Size(23, 28);
            this.pbInfoFront.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbInfoFront.TabIndex = 4;
            this.pbInfoFront.TabStop = false;
            this.pbInfoFront.Click += new System.EventHandler(this.pbInfoFront_Click);
            // 
            // pbProfilePicture
            // 
            this.pbProfilePicture.Image = global::Padeler.Properties.Resources.user__1_;
            this.pbProfilePicture.Location = new System.Drawing.Point(7, 3);
            this.pbProfilePicture.Name = "pbProfilePicture";
            this.pbProfilePicture.Size = new System.Drawing.Size(271, 335);
            this.pbProfilePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbProfilePicture.TabIndex = 0;
            this.pbProfilePicture.TabStop = false;
            // 
            // lblDistance
            // 
            this.lblDistance.AutoSize = true;
            this.lblDistance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblDistance.Location = new System.Drawing.Point(-4, 406);
            this.lblDistance.Name = "lblDistance";
            this.lblDistance.Size = new System.Drawing.Size(60, 23);
            this.lblDistance.TabIndex = 3;
            this.lblDistance.Text = "0,0 km";
            this.lblDistance.Click += new System.EventHandler(this.lblDistance_Click);
            // 
            // lblPlayer
            // 
            this.lblPlayer.AutoSize = true;
            this.lblPlayer.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblPlayer.Location = new System.Drawing.Point(-6, 351);
            this.lblPlayer.Name = "lblPlayer";
            this.lblPlayer.Size = new System.Drawing.Size(129, 32);
            this.lblPlayer.TabIndex = 1;
            this.lblPlayer.Text = "Pero Perić";
            // 
            // lblYearsOld
            // 
            this.lblYearsOld.AutoSize = true;
            this.lblYearsOld.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblYearsOld.Location = new System.Drawing.Point(-4, 383);
            this.lblYearsOld.Name = "lblYearsOld";
            this.lblYearsOld.Size = new System.Drawing.Size(92, 23);
            this.lblYearsOld.TabIndex = 2;
            this.lblYearsOld.Text = "0 years old";
            // 
            // pnlBackCard
            // 
            this.pnlBackCard.Controls.Add(this.pbInfoBack);
            this.pnlBackCard.Controls.Add(this.lblPos);
            this.pnlBackCard.Controls.Add(this.lblPosition);
            this.pnlBackCard.Controls.Add(this.lblFreq);
            this.pnlBackCard.Controls.Add(this.lblFrequency);
            this.pnlBackCard.Controls.Add(this.lblLevel);
            this.pnlBackCard.Controls.Add(this.lblLevelOfPlay);
            this.pnlBackCard.Controls.Add(this.lblAbout);
            this.pnlBackCard.Location = new System.Drawing.Point(22, 17);
            this.pnlBackCard.Name = "pnlBackCard";
            this.pnlBackCard.Size = new System.Drawing.Size(287, 430);
            this.pnlBackCard.TabIndex = 1;
            // 
            // pbInfoBack
            // 
            this.pbInfoBack.Image = global::Padeler.Properties.Resources.information;
            this.pbInfoBack.Location = new System.Drawing.Point(255, 401);
            this.pbInfoBack.Name = "pbInfoBack";
            this.pbInfoBack.Size = new System.Drawing.Size(23, 28);
            this.pbInfoBack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbInfoBack.TabIndex = 5;
            this.pbInfoBack.TabStop = false;
            this.pbInfoBack.Click += new System.EventHandler(this.pbInfoBack_Click);
            // 
            // lblPos
            // 
            this.lblPos.AutoSize = true;
            this.lblPos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblPos.Location = new System.Drawing.Point(30, 324);
            this.lblPos.Name = "lblPos";
            this.lblPos.Size = new System.Drawing.Size(44, 20);
            this.lblPos.TabIndex = 6;
            this.lblPos.Text = "Right";
            this.lblPos.Click += new System.EventHandler(this.lblPos_Click);
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblPosition.Location = new System.Drawing.Point(29, 290);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(89, 25);
            this.lblPosition.TabIndex = 5;
            this.lblPosition.Text = "Position:";
            // 
            // lblFreq
            // 
            this.lblFreq.AutoSize = true;
            this.lblFreq.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblFreq.Location = new System.Drawing.Point(30, 232);
            this.lblFreq.Name = "lblFreq";
            this.lblFreq.Size = new System.Drawing.Size(43, 20);
            this.lblFreq.TabIndex = 4;
            this.lblFreq.Text = "Daily";
            // 
            // lblFrequency
            // 
            this.lblFrequency.AutoSize = true;
            this.lblFrequency.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblFrequency.Location = new System.Drawing.Point(29, 197);
            this.lblFrequency.Name = "lblFrequency";
            this.lblFrequency.Size = new System.Drawing.Size(110, 25);
            this.lblFrequency.TabIndex = 3;
            this.lblFrequency.Text = "Frequency:";
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Font = new System.Drawing.Font("Segoe UI", 9.2F);
            this.lblLevel.Location = new System.Drawing.Point(30, 147);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(98, 21);
            this.lblLevel.TabIndex = 2;
            this.lblLevel.Text = "Intermediate";
            // 
            // lblLevelOfPlay
            // 
            this.lblLevelOfPlay.AutoSize = true;
            this.lblLevelOfPlay.Font = new System.Drawing.Font("Segoe UI", 11.2F, System.Drawing.FontStyle.Bold);
            this.lblLevelOfPlay.Location = new System.Drawing.Point(29, 115);
            this.lblLevelOfPlay.Name = "lblLevelOfPlay";
            this.lblLevelOfPlay.Size = new System.Drawing.Size(128, 25);
            this.lblLevelOfPlay.TabIndex = 1;
            this.lblLevelOfPlay.Text = "Level of play:";
            // 
            // lblAbout
            // 
            this.lblAbout.AutoSize = true;
            this.lblAbout.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblAbout.Location = new System.Drawing.Point(62, 54);
            this.lblAbout.Name = "lblAbout";
            this.lblAbout.Size = new System.Drawing.Size(163, 32);
            this.lblAbout.TabIndex = 0;
            this.lblAbout.Text = "About player";
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.pbLike);
            this.pnlButtons.Controls.Add(this.pbDisslike);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlButtons.Location = new System.Drawing.Point(3, 616);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(794, 50);
            this.pnlButtons.TabIndex = 3;
            // 
            // pbLike
            // 
            this.pbLike.Image = global::Padeler.Properties.Resources.green_love;
            this.pbLike.Location = new System.Drawing.Point(463, 4);
            this.pbLike.Name = "pbLike";
            this.pbLike.Size = new System.Drawing.Size(50, 46);
            this.pbLike.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLike.TabIndex = 5;
            this.pbLike.TabStop = false;
            this.pbLike.Click += new System.EventHandler(this.pbLike_Click);
            // 
            // pbDisslike
            // 
            this.pbDisslike.Image = global::Padeler.Properties.Resources.close;
            this.pbDisslike.Location = new System.Drawing.Point(273, 3);
            this.pbDisslike.Name = "pbDisslike";
            this.pbDisslike.Size = new System.Drawing.Size(50, 46);
            this.pbDisslike.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDisslike.TabIndex = 4;
            this.pbDisslike.TabStop = false;
            this.pbDisslike.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblTitle.Location = new System.Drawing.Point(3, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(10);
            this.lblTitle.Size = new System.Drawing.Size(115, 57);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Home";
            // 
            // flipTimer
            // 
            this.flipTimer.Interval = 15;
            this.flipTimer.Tick += new System.EventHandler(this.flipTimer_Tick);
            // 
            // lblRating
            // 
            this.lblRating.AutoSize = true;
            this.lblRating.Location = new System.Drawing.Point(232, 362);
            this.lblRating.Name = "lblRating";
            this.lblRating.Size = new System.Drawing.Size(46, 16);
            this.lblRating.TabIndex = 5;
            this.lblRating.Text = "Rating";
            // 
            // HomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 669);
            this.Controls.Add(this.tlpHome);
            this.Name = "HomeForm";
            this.Text = "HomeForm";
            this.Load += new System.EventHandler(this.HomeForm_Load);
            this.tlpHome.ResumeLayout(false);
            this.tlpHome.PerformLayout();
            this.pnlCardContainer.ResumeLayout(false);
            this.pnlPlayerCard.ResumeLayout(false);
            this.pnlFrontCard.ResumeLayout(false);
            this.pnlFrontCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfoFront)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbProfilePicture)).EndInit();
            this.pnlBackCard.ResumeLayout(false);
            this.pnlBackCard.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfoBack)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbLike)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDisslike)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpHome;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlCardContainer;
        private System.Windows.Forms.Panel pnlPlayerCard;
        private System.Windows.Forms.PictureBox pbProfilePicture;
        private System.Windows.Forms.Label lblPlayer;
        private System.Windows.Forms.Label lblDistance;
        private System.Windows.Forms.Label lblYearsOld;
        private System.Windows.Forms.PictureBox pbInfoFront;
        private System.Windows.Forms.Panel pnlFrontCard;
        private System.Windows.Forms.Panel pnlBackCard;
        private System.Windows.Forms.Label lblPos;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.Label lblFreq;
        private System.Windows.Forms.Label lblFrequency;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.Label lblLevelOfPlay;
        private System.Windows.Forms.Label lblAbout;
        private System.Windows.Forms.PictureBox pbInfoBack;
        private System.Windows.Forms.Timer flipTimer;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.PictureBox pbLike;
        private System.Windows.Forms.PictureBox pbDisslike;
        private System.Windows.Forms.Panel pnlBadges;
        private System.Windows.Forms.Label lblRating;
    }
}
