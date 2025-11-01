namespace FinalBeansStats {
    partial class ChangePlayerName {
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
            this.txtPlayerName = new MetroFramework.Controls.MetroTextBox();
            this.lblBackColor = new System.Windows.Forms.Label();
            this.btnSave = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
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
            this.txtPlayerName.Location = new System.Drawing.Point(23, 75);
            this.txtPlayerName.MaxLength = 20;
            this.txtPlayerName.Name = "txtPlayerName";
            this.txtPlayerName.PasswordChar = '\0';
            this.txtPlayerName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPlayerName.SelectedText = "";
            this.txtPlayerName.SelectionLength = 0;
            this.txtPlayerName.SelectionStart = 0;
            this.txtPlayerName.ShortcutsEnabled = true;
            this.txtPlayerName.Size = new System.Drawing.Size(285, 25);
            this.txtPlayerName.TabIndex = 1;
            this.txtPlayerName.UseSelectable = true;
            this.txtPlayerName.WaterMark = "Your FinalBeans\' Nickname";
            this.txtPlayerName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtPlayerName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPlayerName.TextChanged += new System.EventHandler(this.txtPlayerName_TextChanged);
            this.txtPlayerName.Validating += new System.ComponentModel.CancelEventHandler(this.txtPlayerName_Validating);
            // 
            // lblBackColor
            // 
            this.lblBackColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(0)))), ((int)(((byte)(182)))), ((int)(((byte)(254)))));
            this.lblBackColor.Location = new System.Drawing.Point(0, 130);
            this.lblBackColor.Name = "lblBackColor";
            this.lblBackColor.Size = new System.Drawing.Size(330, 55);
            this.lblBackColor.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(233, 143);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Confirm";
            this.btnSave.UseSelectable = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ChangePlayerName
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(331, 182);
            this.Controls.Add(this.txtPlayerName);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblBackColor);
            this.Icon = global::FinalBeansStats.Properties.Resources.fbstats;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangePlayerName";
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
            this.ShowInTaskbar = false;
            this.Style = MetroFramework.MetroColorStyle.Teal;
            this.Text = "Change your nickname";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ChangePlayerName_Load);
            this.Shown += new System.EventHandler(this.ChangePlayerName_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChangePlayerName_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private MetroFramework.Controls.MetroTextBox txtPlayerName;
        private System.Windows.Forms.Label lblBackColor;
        private MetroFramework.Controls.MetroButton btnSave;
    }
}