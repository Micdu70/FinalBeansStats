using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Controls;

namespace FinalBeansStats {
    public partial class ChangePlayerName : MetroFramework.Forms.MetroForm {
        public UserSettings CurrentSettings { get; set; }
        public ChangePlayerName() {
            InitializeComponent();
            this.Opacity = 0;
        }

        private void ChangePlayerName_Load(object sender, EventArgs e) {
            this.txtPlayerName.Text = !string.IsNullOrWhiteSpace(this.CurrentSettings.PlayerName) && this.CurrentSettings.PlayerName.Length > 20
                                      ? this.CurrentSettings.PlayerName.Remove(20)
                                      : this.CurrentSettings.PlayerName ?? string.Empty;
            this.btnSave.Enabled = !string.IsNullOrWhiteSpace(this.txtPlayerName.Text);
            this.SetTheme(Stats.CurrentTheme);
            this.ChangeLanguage();
        }

        private void ChangePlayerName_Shown(object sender, EventArgs e) {
            this.Opacity = 1;
        }

        private void txtPlayerName_TextChanged(object sender, EventArgs e) {
            this.btnSave.Enabled = !string.IsNullOrWhiteSpace(this.txtPlayerName.Text);
        }

        private void txtPlayerName_Validating(object sender, CancelEventArgs e) {
            this.txtPlayerName.Text = string.Concat(this.txtPlayerName.Text.Where(c => !char.IsWhiteSpace(c)));
        }

        private void btnSave_Click(object sender, EventArgs e) {
            this.CurrentSettings.PlayerName = this.txtPlayerName.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ChangePlayerName_KeyDown(object sender, KeyEventArgs e) {
            MetroCheckBox mcb1 = this.ActiveControl as MetroCheckBox;
            switch (e.KeyCode) {
                case Keys.Escape:
                    this.DialogResult = DialogResult.Cancel;
                    Close();
                    break;
                case Keys.Enter when this.btnSave.Enabled:
                    this.btnSave.PerformClick();
                    break;
            }
        }

        private void SetTheme(MetroThemeStyle theme) {
            this.SuspendLayout();
            this.Theme = theme;
            foreach (Control c1 in Controls) {
                if (c1 is MetroLabel ml1) {
                    ml1.Theme = theme;
                } else if (c1 is MetroTextBox mtb1) {
                    mtb1.Theme = theme;
                } else if (c1 is MetroButton mb1) {
                    mb1.Theme = theme;
                } else if (c1 is MetroCheckBox mcb1) {
                    mcb1.Theme = theme;
                } else if (c1 is MetroRadioButton mrb1) {
                    mrb1.Theme = theme;
                } else if (c1 is MetroComboBox mcbo1) {
                    mcbo1.Theme = theme;
                } else if (c1 is MetroDateTime mdt1) {
                    mdt1.Theme = theme;
                } else if (c1 is GroupBox gb1) {
                    gb1.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                    foreach (Control c2 in gb1.Controls) {
                        if (c2 is ListBox lb1) {
                            if (theme == MetroThemeStyle.Dark) {
                                lb1.BackColor = Color.FromArgb(21, 21, 21);
                                lb1.ForeColor = Color.WhiteSmoke;
                            }
                        }
                    }
                }
            }
            this.ResumeLayout();
        }

        private void ChangeLanguage() {
            this.Text = $"     {Multilingual.GetWord("settings_change_player_name_title")}";
            this.txtPlayerName.WaterMark = Multilingual.GetWord("settings_change_player_name_watermark");
            this.btnSave.Text = Multilingual.GetWord("settings_save");
        }
    }
}