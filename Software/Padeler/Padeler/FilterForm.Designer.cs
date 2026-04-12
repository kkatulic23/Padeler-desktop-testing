namespace Padeler
{
    partial class FilterForm
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
            this.lblFilter = new System.Windows.Forms.Label();
            this.trkRadius = new System.Windows.Forms.TrackBar();
            this.lblRadiusText = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.lblRadius = new System.Windows.Forms.Label();
            this.cboGender = new System.Windows.Forms.ComboBox();
            this.cboLevel = new System.Windows.Forms.ComboBox();
            this.cboPosition = new System.Windows.Forms.ComboBox();
            this.cboFrequency = new System.Windows.Forms.ComboBox();
            this.lblGender = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblPosition = new System.Windows.Forms.Label();
            this.lblFrequency = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trkRadius)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold);
            this.lblFilter.Location = new System.Drawing.Point(1, 0);
            this.lblFilter.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Padding = new System.Windows.Forms.Padding(8);
            this.lblFilter.Size = new System.Drawing.Size(82, 46);
            this.lblFilter.TabIndex = 1;
            this.lblFilter.Text = "Filter";
            // 
            // trkRadius
            // 
            this.trkRadius.Location = new System.Drawing.Point(142, 74);
            this.trkRadius.Maximum = 50;
            this.trkRadius.Minimum = 1;
            this.trkRadius.Name = "trkRadius";
            this.trkRadius.Size = new System.Drawing.Size(323, 45);
            this.trkRadius.TabIndex = 2;
            this.trkRadius.Value = 50;
            this.trkRadius.ValueChanged += new System.EventHandler(this.trkRadius_ValueChanged);
            // 
            // lblRadiusText
            // 
            this.lblRadiusText.AutoSize = true;
            this.lblRadiusText.Location = new System.Drawing.Point(93, 83);
            this.lblRadiusText.Name = "lblRadiusText";
            this.lblRadiusText.Size = new System.Drawing.Size(43, 13);
            this.lblRadiusText.TabIndex = 3;
            this.lblRadiusText.Text = "Radius:";
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(259, 253);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "Apply filters";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // lblRadius
            // 
            this.lblRadius.AutoSize = true;
            this.lblRadius.Location = new System.Drawing.Point(472, 83);
            this.lblRadius.Name = "lblRadius";
            this.lblRadius.Size = new System.Drawing.Size(36, 13);
            this.lblRadius.TabIndex = 5;
            this.lblRadius.Text = "50 km";
            // 
            // cboGender
            // 
            this.cboGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGender.FormattingEnabled = true;
            this.cboGender.Location = new System.Drawing.Point(235, 125);
            this.cboGender.Name = "cboGender";
            this.cboGender.Size = new System.Drawing.Size(121, 21);
            this.cboGender.TabIndex = 6;
            // 
            // cboLevel
            // 
            this.cboLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLevel.FormattingEnabled = true;
            this.cboLevel.Location = new System.Drawing.Point(235, 152);
            this.cboLevel.Name = "cboLevel";
            this.cboLevel.Size = new System.Drawing.Size(121, 21);
            this.cboLevel.TabIndex = 7;
            // 
            // cboPosition
            // 
            this.cboPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPosition.FormattingEnabled = true;
            this.cboPosition.Location = new System.Drawing.Point(235, 179);
            this.cboPosition.Name = "cboPosition";
            this.cboPosition.Size = new System.Drawing.Size(121, 21);
            this.cboPosition.TabIndex = 8;
            // 
            // cboFrequency
            // 
            this.cboFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFrequency.FormattingEnabled = true;
            this.cboFrequency.Location = new System.Drawing.Point(235, 207);
            this.cboFrequency.Name = "cboFrequency";
            this.cboFrequency.Size = new System.Drawing.Size(121, 21);
            this.cboFrequency.TabIndex = 9;
            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Location = new System.Drawing.Point(184, 133);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new System.Drawing.Size(45, 13);
            this.lblGender.TabIndex = 10;
            this.lblGender.Text = "Gender:";
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(193, 160);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(36, 13);
            this.lblLevel.TabIndex = 11;
            this.lblLevel.Text = "Level:";
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.Location = new System.Drawing.Point(182, 187);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(47, 13);
            this.lblPosition.TabIndex = 12;
            this.lblPosition.Text = "Position:";
            // 
            // lblFrequency
            // 
            this.lblFrequency.AutoSize = true;
            this.lblFrequency.Location = new System.Drawing.Point(169, 215);
            this.lblFrequency.Name = "lblFrequency";
            this.lblFrequency.Size = new System.Drawing.Size(60, 13);
            this.lblFrequency.TabIndex = 13;
            this.lblFrequency.Text = "Frequency:";
            // 
            // FilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 366);
            this.Controls.Add(this.lblFrequency);
            this.Controls.Add(this.lblPosition);
            this.Controls.Add(this.lblLevel);
            this.Controls.Add(this.lblGender);
            this.Controls.Add(this.cboFrequency);
            this.Controls.Add(this.cboPosition);
            this.Controls.Add(this.cboLevel);
            this.Controls.Add(this.cboGender);
            this.Controls.Add(this.lblRadius);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.lblRadiusText);
            this.Controls.Add(this.trkRadius);
            this.Controls.Add(this.lblFilter);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FilterForm";
            this.Text = "FilterForm";
            this.Load += new System.EventHandler(this.FilterForm_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.trkRadius)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.TrackBar trkRadius;
        private System.Windows.Forms.Label lblRadiusText;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label lblRadius;
        private System.Windows.Forms.ComboBox cboGender;
        private System.Windows.Forms.ComboBox cboLevel;
        private System.Windows.Forms.ComboBox cboPosition;
        private System.Windows.Forms.ComboBox cboFrequency;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.Label lblFrequency;
    }
}