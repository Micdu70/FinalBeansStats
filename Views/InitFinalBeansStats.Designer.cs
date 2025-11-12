using System.Windows.Forms;

namespace FinalBeansStats {
    partial class InitFinalBeansStats {
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
            this.picLanguageSelection = new System.Windows.Forms.PictureBox();
            this.cboLanguage = new MetroFramework.Controls.MetroComboBox();
            this.chkAutoGenerateProfile = new MetroFramework.Controls.MetroCheckBox();
            this.lblBackColor = new System.Windows.Forms.Label();
            this.btnSave = new MetroFramework.Controls.MetroButton();
            this.txtPlayerName = new MetroFramework.Controls.MetroTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picLanguageSelection)).BeginInit();
            this.SuspendLayout();
            // 
            // picLanguageSelection
            // 
            this.picLanguageSelection.Image = global::FinalBeansStats.Properties.Resources.language_icon;
            this.picLanguageSelection.Location = new System.Drawing.Point(73, 111);
            this.picLanguageSelection.Name = "picLanguageSelection";
            this.picLanguageSelection.Size = new System.Drawing.Size(32, 32);
            this.picLanguageSelection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLanguageSelection.TabIndex = 1;
            this.picLanguageSelection.TabStop = false;
            // 
            // cboLanguage
            // 
            this.cboLanguage.FormattingEnabled = true;
            this.cboLanguage.ItemHeight = 23;
            this.cboLanguage.Items.AddRange(new object[] {
            "🇺🇸 English",
            "🇫🇷 Français",
            "🇰🇷 한국어",
            "🇯🇵 日本語",
            "🇨🇳 简体中文",
            "🇨🇳 繁體中文"});
            this.cboLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboLanguage.Location = new System.Drawing.Point(111, 112);
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.Size = new System.Drawing.Size(120, 29);
            this.cboLanguage.TabIndex = 2;
            this.cboLanguage.UseSelectable = true;
            this.cboLanguage.SelectedIndexChanged += new System.EventHandler(this.cboLanguage_SelectedIndexChanged);
            // 
            // chkAutoGenerateProfile
            // 
            this.chkAutoGenerateProfile.AutoSize = true;
            this.chkAutoGenerateProfile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(0)))), ((int)(((byte)(182)))), ((int)(((byte)(254)))));
            this.chkAutoGenerateProfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkAutoGenerateProfile.ForeColor = System.Drawing.Color.Teal;
            this.chkAutoGenerateProfile.Location = new System.Drawing.Point(8, 186);
            this.chkAutoGenerateProfile.Name = "chkAutoGenerateProfile";
            this.chkAutoGenerateProfile.Size = new System.Drawing.Size(142, 15);
            this.chkAutoGenerateProfile.TabIndex = 4;
            this.chkAutoGenerateProfile.Text = "Auto-generate profiles";
            this.chkAutoGenerateProfile.UseCustomBackColor = true;
            this.chkAutoGenerateProfile.UseCustomForeColor = true;
            this.chkAutoGenerateProfile.UseSelectable = true;
            this.chkAutoGenerateProfile.CheckedChanged += new System.EventHandler(this.chkAutoGenerateProfile_CheckedChanged);
            // 
            // lblBackColor
            // 
            this.lblBackColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(0)))), ((int)(((byte)(182)))), ((int)(((byte)(254)))));
            this.lblBackColor.Location = new System.Drawing.Point(-5, 164);
            this.lblBackColor.Name = "lblBackColor";
            this.lblBackColor.Size = new System.Drawing.Size(341, 63);
            this.lblBackColor.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(238, 181);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Confirm";
            this.btnSave.UseSelectable = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtPlayerName
            // 
            this.txtPlayerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtPlayerName.CustomButton.Image = null;
            this.txtPlayerName.CustomButton.Location = new System.Drawing.Point(261, 1);
            this.txtPlayerName.CustomButton.Name = "";
            this.txtPlayerName.CustomButton.Size = new System.Drawing.Size(23, 23);
            this.txtPlayerName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtPlayerName.CustomButton.TabIndex = 0;
            this.txtPlayerName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtPlayerName.CustomButton.UseSelectable = true;
            this.txtPlayerName.CustomButton.Visible = false;
            this.txtPlayerName.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.txtPlayerName.Lines = new string[0];
            this.txtPlayerName.Location = new System.Drawing.Point(23, 63);
            this.txtPlayerName.MaxLength = 20;
            this.txtPlayerName.Name = "txtPlayerName";
            this.txtPlayerName.PasswordChar = '\0';
            this.txtPlayerName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPlayerName.SelectedText = "";
            this.txtPlayerName.SelectionLength = 0;
            this.txtPlayerName.SelectionStart = 0;
            this.txtPlayerName.ShortcutsEnabled = true;
            this.txtPlayerName.Size = new System.Drawing.Size(285, 25);
            this.txtPlayerName.TabIndex = 0;
            this.txtPlayerName.UseSelectable = true;
            this.txtPlayerName.WaterMark = "Your FinalBeans\' Nickname";
            this.txtPlayerName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtPlayerName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPlayerName.TextChanged += new System.EventHandler(this.txtPlayerName_TextChanged);
            this.txtPlayerName.Validating += new System.ComponentModel.CancelEventHandler(this.txtPlayerName_Validating);
            // 
            // InitFinalBeansStats
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(331, 221);
            this.ControlBox = false;
            this.Controls.Add(this.txtPlayerName);
            this.Controls.Add(this.picLanguageSelection);
            this.Controls.Add(this.cboLanguage);
            this.Controls.Add(this.chkAutoGenerateProfile);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblBackColor);
            this.Icon = global::FinalBeansStats.Properties.Resources.fbstats;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InitFinalBeansStats";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
            this.ShowInTaskbar = false;
            this.Style = MetroFramework.MetroColorStyle.Teal;
            this.Text = "Nickname && Language";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InitFinalBeansStats_FormClosing);
            this.Load += new System.EventHandler(this.InitFinalBeansStats_Load);
            this.Shown += new System.EventHandler(this.InitFinalBeansStats_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picLanguageSelection)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox picLanguageSelection;
        private MetroFramework.Controls.MetroComboBox cboLanguage;
        private MetroFramework.Controls.MetroCheckBox chkAutoGenerateProfile;
        private System.Windows.Forms.Label lblBackColor;
        private MetroFramework.Controls.MetroButton btnSave;
        private MetroFramework.Controls.MetroTextBox txtPlayerName;
    }
}