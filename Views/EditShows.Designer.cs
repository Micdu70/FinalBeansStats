namespace FinalBeansStats {
    partial class EditShows {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditShows));
            this.picEditShowsIcon = new System.Windows.Forms.PictureBox();
            this.lblEditShowsQuestion = new MetroFramework.Controls.MetroLabel();
            this.lblEditShowslabel = new MetroFramework.Controls.MetroLabel();
            this.cboShows = new MetroFramework.Controls.MetroComboBox();
            this.cboProfiles = new MetroFramework.Controls.MetroComboBox();
            this.lblEditShowsBackColor = new System.Windows.Forms.Label();
            this.chkUseLinkedProfiles = new MetroFramework.Controls.MetroCheckBox();
            this.btnEditShowsSave = new MetroFramework.Controls.MetroButton();
            this.btnEditShowsCancel = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.picEditShowsIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // picEditShowsIcon
            // 
            this.picEditShowsIcon.Image = global::FinalBeansStats.Properties.Resources.finalbeans_icon;
            this.picEditShowsIcon.Location = new System.Drawing.Point(38, 78);
            this.picEditShowsIcon.Name = "picEditShowsIcon";
            this.picEditShowsIcon.Size = new System.Drawing.Size(50, 43);
            this.picEditShowsIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picEditShowsIcon.TabIndex = 0;
            this.picEditShowsIcon.TabStop = false;
            // 
            // lblEditShowsQuestion
            // 
            this.lblEditShowsQuestion.AutoSize = true;
            this.lblEditShowsQuestion.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblEditShowsQuestion.Location = new System.Drawing.Point(100, 80);
            this.lblEditShowsQuestion.Name = "lblEditShowsQuestion";
            this.lblEditShowsQuestion.Size = new System.Drawing.Size(74, 19);
            this.lblEditShowsQuestion.TabIndex = 0;
            this.lblEditShowsQuestion.Text = "Description";
            // 
            // lblEditShowslabel
            // 
            this.lblEditShowslabel.AutoSize = true;
            this.lblEditShowslabel.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.lblEditShowslabel.Location = new System.Drawing.Point(86, 137);
            this.lblEditShowslabel.Name = "lblEditShowslabel";
            this.lblEditShowslabel.Size = new System.Drawing.Size(69, 19);
            this.lblEditShowslabel.TabIndex = 0;
            this.lblEditShowslabel.Text = "Profile:";
            // 
            // cboShows
            // 
            this.cboShows.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboShows.FormattingEnabled = true;
            this.cboShows.ItemHeight = 23;
            this.cboShows.Location = new System.Drawing.Point(185, 132);
            this.cboShows.Name = "cboShows";
            this.cboShows.Size = new System.Drawing.Size(198, 29);
            this.cboShows.TabIndex = 0;
            this.cboShows.UseSelectable = true;
            this.cboShows.SelectedIndexChanged += new System.EventHandler(this.cboShows_Changed);
            // 
            // cboProfiles
            // 
            this.cboProfiles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboProfiles.FormattingEnabled = true;
            this.cboProfiles.ItemHeight = 23;
            this.cboProfiles.Location = new System.Drawing.Point(185, 132);
            this.cboProfiles.Name = "cboProfiles";
            this.cboProfiles.Size = new System.Drawing.Size(198, 29);
            this.cboProfiles.TabIndex = 0;
            this.cboProfiles.UseSelectable = true;
            this.cboProfiles.SelectedIndexChanged += new System.EventHandler(this.cboProfiles_Changed);
            // 
            // lblEditShowsBackColor
            // 
            this.lblEditShowsBackColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(0)))), ((int)(((byte)(182)))), ((int)(((byte)(254)))));
            this.lblEditShowsBackColor.Location = new System.Drawing.Point(0, 200);
            this.lblEditShowsBackColor.Name = "lblEditShowsBackColor";
            this.lblEditShowsBackColor.Size = new System.Drawing.Size(445, 64);
            this.lblEditShowsBackColor.TabIndex = 1;
            // 
            // chkUseLinkedProfiles
            // 
            this.chkUseLinkedProfiles.AutoSize = true;
            this.chkUseLinkedProfiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(0)))), ((int)(((byte)(182)))), ((int)(((byte)(254)))));
            this.chkUseLinkedProfiles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkUseLinkedProfiles.ForeColor = System.Drawing.Color.Teal;
            this.chkUseLinkedProfiles.Location = new System.Drawing.Point(18, 224);
            this.chkUseLinkedProfiles.Name = "chkUseLinkedProfiles";
            this.chkUseLinkedProfiles.Size = new System.Drawing.Size(119, 15);
            this.chkUseLinkedProfiles.TabIndex = 1;
            this.chkUseLinkedProfiles.Text = "Use linked profiles";
            this.chkUseLinkedProfiles.UseCustomBackColor = true;
            this.chkUseLinkedProfiles.UseCustomForeColor = true;
            this.chkUseLinkedProfiles.UseSelectable = true;
            this.chkUseLinkedProfiles.CheckedChanged += new System.EventHandler(this.chkUseLinkedProfiles_CheckedChanged);
            // 
            // btnEditShowsSave
            // 
            this.btnEditShowsSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditShowsSave.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnEditShowsSave.Location = new System.Drawing.Point(238, 219);
            this.btnEditShowsSave.Name = "btnEditShowsSave";
            this.btnEditShowsSave.Size = new System.Drawing.Size(87, 26);
            this.btnEditShowsSave.TabIndex = 2;
            this.btnEditShowsSave.Text = "Save";
            this.btnEditShowsSave.UseSelectable = true;
            this.btnEditShowsSave.Click += new System.EventHandler(this.btnEditShowsSave_Click);
            // 
            // btnEditShowsCancel
            // 
            this.btnEditShowsCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditShowsCancel.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.btnEditShowsCancel.Location = new System.Drawing.Point(337, 219);
            this.btnEditShowsCancel.Name = "btnEditShowsCancel";
            this.btnEditShowsCancel.Size = new System.Drawing.Size(87, 26);
            this.btnEditShowsCancel.TabIndex = 3;
            this.btnEditShowsCancel.Text = "Cancel";
            this.btnEditShowsCancel.UseSelectable = true;
            this.btnEditShowsCancel.Click += new System.EventHandler(this.btnEditShowsCancel_Click);
            // 
            // EditShows
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(445, 263);
            this.Controls.Add(this.picEditShowsIcon);
            this.Controls.Add(this.lblEditShowsQuestion);
            this.Controls.Add(this.lblEditShowslabel);
            this.Controls.Add(this.cboShows);
            this.Controls.Add(this.cboProfiles);
            this.Controls.Add(this.btnEditShowsSave);
            this.Controls.Add(this.btnEditShowsCancel);
            this.Controls.Add(this.chkUseLinkedProfiles);
            this.Controls.Add(this.lblEditShowsBackColor);
            this.Icon = global::FinalBeansStats.Properties.Resources.fbstats;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditShows";
            this.Padding = new System.Windows.Forms.Padding(23, 60, 23, 20);
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
            this.ShowInTaskbar = true;
            this.Style = MetroFramework.MetroColorStyle.Teal;
            this.Text = "Edit Shows";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.EditShows_Load);
            this.Shown += new System.EventHandler(this.EditShows_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picEditShowsIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.PictureBox picEditShowsIcon;
        private MetroFramework.Controls.MetroLabel lblEditShowsQuestion;
        private MetroFramework.Controls.MetroLabel lblEditShowslabel;
        private MetroFramework.Controls.MetroComboBox cboShows;
        private MetroFramework.Controls.MetroComboBox cboProfiles;
        private System.Windows.Forms.Label lblEditShowsBackColor;
        private MetroFramework.Controls.MetroCheckBox chkUseLinkedProfiles;
        private MetroFramework.Controls.MetroButton btnEditShowsSave;
        private MetroFramework.Controls.MetroButton btnEditShowsCancel;
    }
}