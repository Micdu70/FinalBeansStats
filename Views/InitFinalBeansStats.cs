using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Controls;

namespace FinalBeansStats {
    public partial class InitFinalBeansStats : MetroFramework.Forms.MetroForm {
        public Stats StatsForm { get; set; }
        public string playerName;
        private readonly Language defaultLanguage;
        public Language selectedLanguage;
        public bool autoGenerateProfiles;

        public InitFinalBeansStats(string sysLang) {
            this.defaultLanguage = string.Equals(sysLang, "fr", StringComparison.OrdinalIgnoreCase) ? Language.French :
                                   string.Equals(sysLang, "ko", StringComparison.OrdinalIgnoreCase) ? Language.Korean :
                                   string.Equals(sysLang, "ja", StringComparison.OrdinalIgnoreCase) ? Language.Japanese :
                                   string.Equals(sysLang, "zh-chs", StringComparison.OrdinalIgnoreCase) ? Language.SimplifiedChinese :
                                   string.Equals(sysLang, "zh-cht", StringComparison.OrdinalIgnoreCase) ? Language.TraditionalChinese : Language.English;
            this.InitializeComponent();
            this.Opacity = 0;
        }

        private void InitFinalBeansStats_Load(object sender, EventArgs e) {
            this.cboLanguage.SelectedIndex = (int)this.defaultLanguage;
            this.SetTheme(Stats.CurrentTheme);
            this.ChangeLanguage(this.defaultLanguage);
        }

        private void InitFinalBeansStats_Shown(object sender, EventArgs e) {
            this.Opacity = 1;
        }

        private void txtPlayerName_TextChanged(object sender, EventArgs e) {
            this.btnSave.Enabled = !string.IsNullOrWhiteSpace(this.txtPlayerName.Text);
        }

        private void txtPlayerName_Validating(object sender, CancelEventArgs e) {
            this.txtPlayerName.Text = string.Concat(this.txtPlayerName.Text.Where(c => !char.IsWhiteSpace(c)));
        }

        private void cboLanguage_SelectedIndexChanged(object sender, EventArgs e) {
            this.selectedLanguage = (Language)((ComboBox)sender).SelectedIndex;
            this.ChangeLanguage(this.selectedLanguage);
        }

        private void chkAutoGenerateProfile_CheckedChanged(object sender, EventArgs e) {
            this.autoGenerateProfiles = ((CheckBox)sender).Checked;
        }

        private void InitFinalBeansStats_FormClosing(object sender, FormClosingEventArgs e) {
            // Dispose the database and exit the program (if force-closed or "Esc" key pressed)
            this.StatsForm.StatsDB.Dispose();
            Environment.Exit(0);
        }

        private void btnSave_Click(object sender, EventArgs e) {
            this.playerName = this.txtPlayerName.Text;
            this.DialogResult = DialogResult.OK;
            this.FormClosing -= new FormClosingEventHandler(this.InitFinalBeansStats_FormClosing);
            this.Close();
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (keyData == Keys.Enter && this.ActiveControl is MetroCheckBox) {
                this.chkAutoGenerateProfile.Checked = !this.chkAutoGenerateProfile.Checked;
            }
            if (keyData == Keys.Tab) { SendKeys.Send("%"); }
            if (keyData == Keys.Escape) {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ChangeLanguage(Language lang) {
            this.Font = Overlay.GetMainFont(9, FontStyle.Regular, lang);
            this.Text = $"     {Multilingual.GetWord("settings_select_language_title", lang)}";
            this.txtPlayerName.WaterMark = Multilingual.GetWord("settings_change_player_name_watermark", lang);
            this.chkAutoGenerateProfile.Text = Multilingual.GetWord("settings_auto_generate_profiles", lang);
            this.btnSave.Text = Multilingual.GetWord("settings_select_language_button", lang);
            this.Refresh();
        }
    }
}