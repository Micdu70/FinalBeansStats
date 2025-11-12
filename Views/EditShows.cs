using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Controls;

namespace FinalBeansStats {
    public partial class EditShows : MetroFramework.Forms.MetroForm {
        public Stats StatsForm { get; set; }
        public List<RoundInfo> AllStats { get; set; }
        public List<Profiles> Profiles { get; set; }
        public Dictionary<string, string> Shows = new Dictionary<string, string>();
        public string SelectedShowName = null;
        public string SelectedShowNameId = null;
        public int SelectedProfileId = 0;
        public string FunctionFlag = string.Empty;
        public int SelectedCount = 0;
        public bool UseLinkedProfiles;
        public EditShows() {
            this.InitializeComponent();
            this.Opacity = 0;
        }

        private void EditShows_Load(object sender, EventArgs e) {
            this.SetTheme(Stats.CurrentTheme);
            this.ChangeLanguage();
            this.cboShows.Items.Clear();
            this.cboProfiles.Items.Clear();

            if (this.FunctionFlag == "rename") {
                this.cboProfiles.Visible = false;

                List<string> addedShowList = new List<string>();
                foreach (string showId in this.StatsForm.PublicShowIdList) {
                    if (!string.Equals(showId, "fb_ltm")) {
                        Shows.Add(Multilingual.GetShowName(showId), showId);
                    }
                    addedShowList.Add(showId);
                }
                foreach (string showId in this.StatsForm.PublicShowIdList2) {
                    Shows.Add(Multilingual.GetShowName(showId), showId);
                    addedShowList.Add(showId);
                }
                foreach (var stat in this.AllStats.FindAll(r => !addedShowList.Contains(r.ShowNameId))) {
                    if (addedShowList.Contains(stat.ShowNameId)) continue;

                    Shows.Add(stat.ShowName ?? stat.ShowNameId, stat.ShowNameId);
                    addedShowList.Add(stat.ShowNameId);
                }
                for (int i = Shows.Count - 1; i >= 0; i--) {
                    this.cboShows.Items.Insert(0, Shows.ElementAt(i).Key);
                }
                this.chkUseLinkedProfiles.Checked = this.StatsForm.CurrentSettings.AutoChangeProfile;
            } else {
                this.cboShows.Visible = false;

                this.Profiles = this.Profiles.OrderBy(p => p.ProfileOrder).ToList();
                if (this.Profiles.Count == 1) {
                    this.cboProfiles.Items.Insert(0, this.Profiles[0].ProfileName);
                    this.cboProfiles.SelectedIndex = 0;
                    this.chkUseLinkedProfiles.Visible = false;
                } else {
                    for (int i = this.Profiles.Count - 1; i >= 0; i--) {
                        if (this.FunctionFlag == "move" && this.Profiles[i].ProfileId == StatsForm.GetCurrentProfileId()) continue;

                        this.cboProfiles.Items.Insert(0, this.Profiles[i].ProfileName);
                    }
                    this.cboProfiles.SelectedIndex = 0;

                    switch (this.FunctionFlag) {
                        case "add" when this.StatsForm.CurrentSettings.AutoChangeProfile:
                            this.chkUseLinkedProfiles.Visible = false;
                            this.chkUseLinkedProfiles.Checked = true;
                            break;
                        case "move":
                            this.chkUseLinkedProfiles.Visible = false;
                            break;
                    }
                }
            }
        }

        private void EditShows_Shown(object sender, EventArgs e) {
            this.Opacity = 1;
        }

        private void SetTheme(MetroThemeStyle theme) {
            this.SuspendLayout();
            foreach (Control c1 in Controls) {
                if (c1 is MetroLabel mlb1) {
                    mlb1.Theme = theme;
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
                } else if (c1 is MetroLink ml1) {
                    ml1.Theme = theme;
                } else if (c1 is MetroTabControl mtc1) {
                    mtc1.Theme = theme;
                    foreach (Control c2 in mtc1.Controls) {
                        if (c2 is MetroTabPage mtp2) {
                            mtp2.Theme = theme;
                            foreach (Control c3 in mtp2.Controls) {
                                if (c3 is MetroLabel mlb3) {
                                    mlb3.Theme = theme;
                                } else if (c3 is MetroTextBox mtb3) {
                                    mtb3.Theme = theme;
                                } else if (c3 is MetroButton mb3) {
                                    mb3.Theme = theme;
                                } else if (c3 is MetroCheckBox mcb3) {
                                    mcb3.Theme = theme;
                                } else if (c3 is MetroRadioButton mrb3) {
                                    mrb3.Theme = theme;
                                } else if (c3 is MetroComboBox mcbo3) {
                                    mcbo3.Theme = theme;
                                }
                            }
                        }
                    }
                } else if (c1 is GroupBox gb1) {
                    gb1.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                    foreach (Control c2 in gb1.Controls) {
                        if (c2 is MetroLabel mlb2) {
                            mlb2.Theme = theme;
                        } else if (c2 is MetroTextBox mtb2) {
                            mtb2.Theme = theme;
                        } else if (c2 is MetroButton mb2) {
                            mb2.Theme = theme;
                        } else if (c2 is MetroCheckBox mcb2) {
                            mcb2.Theme = theme;
                        } else if (c2 is MetroRadioButton mrb2) {
                            mrb2.Theme = theme;
                        } else if (c2 is MetroComboBox mcbo2) {
                            mcbo2.Theme = theme;
                        } else if (c2 is GroupBox gb2) {
                            gb2.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                            foreach (Control c3 in gb2.Controls) {
                                if (c3 is MetroRadioButton mrb3) {
                                    mrb3.Theme = theme;
                                }
                            }
                        }
                    }
                }
            }
            this.Theme = theme;
            this.ResumeLayout();
        }

        private void cboShows_Changed(object sender, EventArgs e) {
            this.SelectedShowName = Shows.ElementAt(this.cboShows.SelectedIndex).Key;
            this.SelectedShowNameId = Shows.ElementAt(this.cboShows.SelectedIndex).Value;
        }

        private void cboProfiles_Changed(object sender, EventArgs e) {
            this.SelectedProfileId = this.Profiles.Find(p => p.ProfileName == (string)this.cboProfiles.SelectedItem).ProfileId;
        }

        private void chkUseLinkedProfiles_CheckedChanged(object sender, EventArgs e) {
            this.UseLinkedProfiles = ((CheckBox)sender).Checked;
            this.lblEditShowsQuestion.Text = this.FunctionFlag == "rename"
                                             ? $"{Multilingual.GetWord("show_rename_select_description_prefix")}{Environment.NewLine}{Multilingual.GetWord("show_rename_select_description_suffix")}: {this.SelectedCount:N0}{Multilingual.GetWord("numeric_suffix")}"
                                             : $"{Multilingual.GetWord("profile_add_select_question_prefix")}{Environment.NewLine}{(this.UseLinkedProfiles ? Multilingual.GetWord("profile_add_select_question_suffix_linked_profiles") : Multilingual.GetWord("profile_add_select_question_suffix"))}";
            if (this.UseLinkedProfiles) {
                this.lblEditShowslabel.Visible = this.FunctionFlag == "rename";
                this.cboProfiles.Visible = false;
                if (this.FunctionFlag != "rename") {
                    this.cboProfiles.SelectedIndex = 0;
                }
            } else {
                this.lblEditShowslabel.Visible = true;
                if (this.FunctionFlag != "rename") {
                    this.cboProfiles.SelectedIndex = 0;
                }
                this.cboProfiles.Visible = this.FunctionFlag != "rename";
            }
        }

        private void btnEditShowsSave_Click(object sender, EventArgs e) {
            this.Visible = false;
            switch (this.FunctionFlag) {
                case "add":
                    if (this.UseLinkedProfiles) {
                        if (Messenger.MessageBox($"{Multilingual.GetWord("message_save_data_linked_profiles")}{Environment.NewLine}{Multilingual.GetWord("message_save_data_linked_profiles_info_prefix")} ({this.cboProfiles.SelectedItem}) {Multilingual.GetWord("message_save_data_linked_profiles_info_suffix")}",
                                                 $"{Multilingual.GetWord("message_save_data_caption")}",
                                MsgIcon.Question, MessageBoxButtons.YesNo, Stats.CurrentTheme == MetroThemeStyle.Dark, MessageBoxDefaultButton.Button1, this) == DialogResult.Yes) {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    } else {
                        if (Messenger.MessageBox($"{Multilingual.GetWord("message_save_profile_prefix")} ({this.cboProfiles.SelectedItem}) {Multilingual.GetWord("message_save_profile_suffix")}",
                                                 $"{Multilingual.GetWord("message_save_profile_caption")}",
                                MsgIcon.Question, MessageBoxButtons.YesNo, Stats.CurrentTheme == MetroThemeStyle.Dark, MessageBoxDefaultButton.Button1, this) == DialogResult.Yes) {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                    break;
                case "rename":
                    if (this.cboShows.SelectedIndex < 0) {
                        this.Close();
                        break;
                    }
                    if (this.UseLinkedProfiles) {
                        if (Messenger.MessageBox($"{Multilingual.GetWord("show_rename_and_move_select_question_prefix")} ({this.SelectedCount:N0}) {Multilingual.GetWord("show_rename_and_move_select_question_suffix")}",
                                                 $"{Multilingual.GetWord("show_rename_and_move_select_title")}",
                                MsgIcon.Question, MessageBoxButtons.YesNo, Stats.CurrentTheme == MetroThemeStyle.Dark, MessageBoxDefaultButton.Button1, this) == DialogResult.Yes) {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    } else {
                        if (Messenger.MessageBox($"{Multilingual.GetWord("show_rename_select_question_prefix")} ({this.SelectedCount:N0}) {Multilingual.GetWord("show_rename_select_question_suffix")}",
                                                 $"{Multilingual.GetWord("show_rename_select_title")}",
                                MsgIcon.Question, MessageBoxButtons.YesNo, Stats.CurrentTheme == MetroThemeStyle.Dark, MessageBoxDefaultButton.Button1, this) == DialogResult.Yes) {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                    break;
                case "move":
                    if (Messenger.MessageBox($"{Multilingual.GetWord("profile_move_select_question_prefix")} ({this.SelectedCount:N0}) {Multilingual.GetWord("profile_move_select_question_infix")} ({this.cboProfiles.SelectedItem}) {Multilingual.GetWord("profile_move_select_question_suffix")}",
                                             $"{Multilingual.GetWord("profile_move_select_title")}",
                            MsgIcon.Question, MessageBoxButtons.YesNo, Stats.CurrentTheme == MetroThemeStyle.Dark, MessageBoxDefaultButton.Button1, this) == DialogResult.Yes) {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    break;
            }
        }

        private void btnEditShowsCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (keyData == Keys.Enter && this.ActiveControl is MetroCheckBox) {
                this.chkUseLinkedProfiles.Checked = !this.chkUseLinkedProfiles.Checked;
            }
            if (keyData == Keys.Tab) { SendKeys.Send("%"); }
            if (keyData == Keys.Escape) {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ChangeLanguage() {
            this.Font = Overlay.GetMainFont(9);
            switch (this.FunctionFlag) {
                case "add":
                    this.Text = Multilingual.GetWord("profile_add_select_title");
                    this.lblEditShowsQuestion.Text = $"{Multilingual.GetWord("profile_add_select_question_prefix")}{Environment.NewLine}{Multilingual.GetWord("profile_add_select_question_suffix")}";
                    this.chkUseLinkedProfiles.Text = Multilingual.GetWord("profile_add_select_use_linked_profiles");
                    break;
                case "rename":
                    this.Text = Multilingual.GetWord("show_rename_select_title");
                    this.lblEditShowsQuestion.Text = $"{Multilingual.GetWord("show_rename_select_description_prefix")}{Environment.NewLine}{Multilingual.GetWord("show_rename_select_description_suffix")}: {this.SelectedCount:N0}{Multilingual.GetWord("numeric_suffix")}";
                    this.chkUseLinkedProfiles.Text = Multilingual.GetWord("show_rename_select_use_linked_profiles");
                    break;
                case "move":
                    this.Text = Multilingual.GetWord("profile_move_select_title");
                    this.lblEditShowsQuestion.Text = $"{Multilingual.GetWord("profile_move_select_description_prefix")}{Environment.NewLine}{Multilingual.GetWord("profile_move_select_description_suffix")}: {this.SelectedCount:N0}{Multilingual.GetWord("numeric_suffix")}";
                    break;
            }
            this.lblEditShowslabel.Text = this.FunctionFlag != "rename" ? Multilingual.GetWord("profile_list_selection") : Multilingual.GetWord("show_list_selection");
            if (Stats.CurrentLanguage == Language.English) {
                this.Width = 445;
                this.cboShows.Left = 180;
                this.cboProfiles.Left = 145;
                //this.cboProfiles.Size = new Size(198, 29);
                this.lblEditShowsBackColor.Width = 445;
            } else if (Stats.CurrentLanguage == Language.French) {
                this.Width = 525;
                this.cboShows.Left = 220;
                this.cboProfiles.Left = 140;
                //this.cboProfiles.Size = new Size(198, 29);
                this.lblEditShowsBackColor.Width = 525;
            } else if (Stats.CurrentLanguage == Language.Korean) {
                this.Width = 445;
                this.cboShows.Left = 180;
                this.cboProfiles.Left = 145;
                //this.cboProfiles.Size = new Size(198, 29);
                this.lblEditShowsBackColor.Width = 445;
            } else if (Stats.CurrentLanguage == Language.Japanese) {
                this.Width = 540;
                this.cboShows.Left = 180;
                this.cboProfiles.Left = 230;
                //this.cboProfiles.Size = new Size(198, 29);
                this.lblEditShowsBackColor.Width = 540;
            } else if (Stats.CurrentLanguage == Language.SimplifiedChinese || Stats.CurrentLanguage == Language.TraditionalChinese) {
                this.Width = 445;
                this.cboShows.Left = 180;
                this.cboProfiles.Left = 145;
                //this.cboProfiles.Size = new Size(198, 29);
                this.lblEditShowsBackColor.Width = 445;
            }
            this.btnEditShowsCancel.Text = Multilingual.GetWord("undo_change_button");
            this.btnEditShowsCancel.Width = TextRenderer.MeasureText(this.btnEditShowsCancel.Text, this.btnEditShowsCancel.Font).Width + 45;
            this.btnEditShowsCancel.Left = this.Width - this.btnEditShowsCancel.Width - 20;
            this.btnEditShowsSave.Text = Multilingual.GetWord("apply_change_button");
            this.btnEditShowsSave.Width = TextRenderer.MeasureText(this.btnEditShowsSave.Text, this.btnEditShowsSave.Font).Width + 45;
            this.btnEditShowsSave.Left = this.btnEditShowsCancel.Left - this.btnEditShowsSave.Width - 15;
        }
    }
}