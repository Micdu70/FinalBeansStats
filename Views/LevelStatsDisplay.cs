using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Controls;
using ScottPlot;
using ScottPlot.Plottables;
using Color = System.Drawing.Color;
using Label = System.Windows.Forms.Label;

namespace FinalBeansStats {
    public partial class LevelStatsDisplay : MetroFramework.Forms.MetroForm {
        public Stats StatsForm { get; set; }
        public Dictionary<string, double[]> levelMedalInfo;
        public Dictionary<string, TimeSpan> levelTotalPlayTime;
        public Dictionary<string, string[]> levelTimeInfo;
        public Dictionary<string, string[]> levelScoreInfo;
        public IOrderedEnumerable<KeyValuePair<string, string>> levelList;
        private RadialGaugePlot radialGauges;
        private readonly string[] labelList = {
            Multilingual.GetWord("main_played"), Multilingual.GetWord("level_detail_gold"),
            Multilingual.GetWord("level_detail_silver"), Multilingual.GetWord("level_detail_bronze"),
            Multilingual.GetWord("level_detail_pink"), Multilingual.GetWord("level_detail_eliminated")
        };
        private bool isInitComplete;
        private string goldMedalCount, silverMedalCount, bronzeMedalCount, pinkMedalCount, eliminatedMedalCount;
        private string goldMedalPercent, silverMedalPercent, bronzeMedalPercent, pinkMedalPercent, eliminatedMedalPercent;
        private bool switching;
        public LevelStatsDisplay() {
            this.InitializeComponent();
            this.Opacity = 0;
        }

        private void RoundStatsDisplay_Load(object sender, EventArgs e) {
            this.SetTheme(Stats.CurrentTheme);
            this.ChangeLanguage();

            this.cboRoundList.DataSource = new BindingSource(this.levelList, null);
            this.cboRoundList.DisplayMember = "Value";
            this.cboRoundList.ValueMember = "Key";

            this.formsPlot.Plot.Legend.SetBestFontOnEachRender = true;
            this.formsPlot.Plot.ShowLegend(Alignment.UpperRight);
            this.formsPlot.UserInputProcessor.IsEnabled = false;
            this.SetGraph(this.cboRoundList.SelectedItem);
            this.isInitComplete = true;
        }

        private void RoundStatsDisplay_Shown(object sender, EventArgs e) {
            this.Opacity = 1;
        }

        private void SetTheme(MetroThemeStyle theme) {
            this.SuspendLayout();
            ScottPlot.Color figureColor = ScottPlot.Color.FromColor(theme == MetroThemeStyle.Dark ? Color.FromArgb(17, 17, 17) : Color.White);
            ScottPlot.Color axesColor = ScottPlot.Color.FromColor(theme == MetroThemeStyle.Dark ? Color.WhiteSmoke : Color.DimGray);
            this.formsPlot.Plot.SetStyle(new PlotStyle {
                FigureBackgroundColor = figureColor,
                DataBackgroundColor = figureColor,
                AxisColor = axesColor,
                GridMajorLineColor = ScottPlot.Color.FromColor(theme == MetroThemeStyle.Dark ? Color.FromArgb(64, 64, 64) : Color.LightGray),
                LegendBackgroundColor = figureColor,
                LegendFontColor = axesColor,
                LegendOutlineColor = axesColor
            });
            if (theme == MetroThemeStyle.Dark) {
                foreach (Control c1 in Controls) {
                    if (c1 is MetroComboBox mcbo1) {
                        mcbo1.Theme = theme;
                    } else if (c1 is MetroLabel mlb1) {
                        mlb1.Theme = theme;
                    } else if (c1 is Label lb1) {
                        if (lb1.Equals(this.lblRoundType) || lb1.Equals(this.lblRoundTime) || lb1.Equals(this.lblBestRecord) || lb1.Equals(this.lblWorstRecord)) continue;
                        lb1.ForeColor = Color.DarkGray;
                    }
                }
            }
            this.Theme = theme;
            this.ResumeLayout();
        }

        private void SetGraph(object selectedItem) {
            KeyValuePair<string, string> selectedRoundPair = (KeyValuePair<string, string>)selectedItem;
            string levelId = selectedRoundPair.Key;

            if (this.StatsForm.StatLookup.TryGetValue(levelId, out LevelStats level)) {
                this.picRoundIcon.Size = level.RoundBigIcon.Size;
                this.picRoundIcon.Image = level.RoundBigIcon;
                this.formsPlot.Plot.Title(level.Name);
                this.lblRoundType.Text = level.Type.LevelTitle(level.IsFinal);
                this.lblRoundType.borderColor = level.Type.LevelDefaultColor(level.IsFinal);
                this.lblRoundType.backColor = level.Type.LevelDefaultColor(level.IsFinal);
                this.lblRoundType.Width = TextRenderer.MeasureText(this.lblRoundType.Text, this.lblRoundType.Font).Width + 12;
                this.lblBestRecord.Left = this.lblRoundType.Right + 12;
                this.lblWorstRecord.Left = this.lblRoundType.Right + 12;

                switch (level.BestRecordType) {
                    case BestRecordType.Fastest:
                        this.lblBestRecord.Text = $"{Multilingual.GetWord("overlay_fastest")} : {this.levelTimeInfo[levelId][0]}";
                        this.lblWorstRecord.Text = $"{Multilingual.GetWord("overlay_longest")} : {(!string.Equals(this.levelTimeInfo[levelId][0], this.levelTimeInfo[levelId][1]) ? this.levelTimeInfo[levelId][1] : @"-")}";
                        break;
                    case BestRecordType.Longest:
                        this.lblBestRecord.Text = $"{Multilingual.GetWord("overlay_longest")} : {this.levelTimeInfo[levelId][1]}";
                        this.lblWorstRecord.Text = $"{Multilingual.GetWord("overlay_fastest")} : {(!string.Equals(this.levelTimeInfo[levelId][1], this.levelTimeInfo[levelId][0]) ? this.levelTimeInfo[levelId][0] : @"-")}";
                        break;
                    case BestRecordType.HighScore:
                        this.lblBestRecord.Text = $"{Multilingual.GetWord("overlay_high_score")} : {this.levelScoreInfo[levelId][0]}";
                        this.lblWorstRecord.Text = $"{Multilingual.GetWord("overlay_low_score")} : {(!string.Equals(this.levelScoreInfo[levelId][0], this.levelScoreInfo[levelId][1]) ? this.levelScoreInfo[levelId][1] : @"-")}";
                        break;
                    default:
                        this.lblBestRecord.Text = $"{Multilingual.GetWord("overlay_fastest")} : {this.levelTimeInfo[levelId][0]}";
                        this.lblWorstRecord.Text = $"{Multilingual.GetWord("overlay_longest")} : {(!string.Equals(this.levelTimeInfo[levelId][0], this.levelTimeInfo[levelId][1]) ? this.levelTimeInfo[levelId][1] : @"-")}";
                        break;
                }
            }

            TimeSpan playTime = this.levelTotalPlayTime[levelId];
            this.lblRoundTime.Text = $@"{Multilingual.GetWord("level_round_played_prefix")} {(int)playTime.TotalHours}{Multilingual.GetWord("main_hour")}{playTime:mm}{Multilingual.GetWord("main_min")}{playTime:ss}{Multilingual.GetWord("main_sec")} {Multilingual.GetWord("level_round_played_suffix")}";
            double[] values = this.levelMedalInfo[levelId];

            this.goldMedalPercent = $@"{Math.Round((values[1] / values[0]) * 100, 2)}%";
            this.silverMedalPercent = $@"{Math.Round((values[2] / values[0]) * 100, 2)}%";
            this.bronzeMedalPercent = $@"{Math.Round((values[3] / values[0]) * 100, 2)}%";
            this.pinkMedalPercent = $@"{Math.Round((values[4] / values[0]) * 100, 2)}%";
            this.eliminatedMedalPercent = $@"{Math.Round((values[5] / values[0]) * 100, 2)}%";

            this.goldMedalCount = $@"{values[1]:N0}";
            this.silverMedalCount = $@"{values[2]:N0}";
            this.bronzeMedalCount = $@"{values[3]:N0}";
            this.pinkMedalCount = $@"{values[4]:N0}";
            this.eliminatedMedalCount = $@"{values[5]:N0}";

            if (this.switching) {
                this.lblCountGoldMedal.Text = this.goldMedalPercent;
                this.lblCountSilverMedal.Text = this.silverMedalPercent;
                this.lblCountBronzeMedal.Text = this.bronzeMedalPercent;
                this.lblCountPinkMedal.Text = this.pinkMedalPercent;
                this.lblCountEliminatedMedal.Text = this.eliminatedMedalPercent;
            } else {
                this.lblCountGoldMedal.Text = this.goldMedalCount;
                this.lblCountSilverMedal.Text = this.silverMedalCount;
                this.lblCountBronzeMedal.Text = this.bronzeMedalCount;
                this.lblCountPinkMedal.Text = this.pinkMedalCount;
                this.lblCountEliminatedMedal.Text = this.eliminatedMedalCount;
            }



            this.radialGauges = this.formsPlot.Plot.Add.RadialGaugePlot(values);
            this.radialGauges.Colors = new[] {
                ScottPlot.Color.FromColor(Color.FromArgb(31, 119, 180)), ScottPlot.Color.FromColor(Color.FromArgb(255, 215, 0)),
                ScottPlot.Color.FromColor(Color.FromArgb(192, 192, 192)), ScottPlot.Color.FromColor(Color.FromArgb(205, 127, 50)),
                ScottPlot.Color.FromColor(Color.FromArgb(255, 20, 147)), ScottPlot.Color.FromColor(Color.FromArgb(128, 0, 128))
            };
            this.radialGauges.OrderInsideOut = false;
            this.radialGauges.SpaceFraction = .1;
            this.radialGauges.StartingAngle = 270;
            this.radialGauges.EndCap = SkiaSharp.SKStrokeCap.Round;
            this.radialGauges.ShowLevels = false;
            this.radialGauges.Labels = this.labelList;

            for (int i = 0; i < values.Length; i++) {
                int gaugePosition = values.Length - i;
                double labelY = (double)gaugePosition / values.Length - 0.01;
                Text valueLabel = this.formsPlot.Plot.Add.Text(values[i] == 0 ? string.Empty : values[i].ToString(), 0, labelY);
                valueLabel.Alignment = Alignment.MiddleCenter;
                valueLabel.LabelFontSize = 20;
                valueLabel.LabelFontColor = ScottPlot.Color.FromColor(this.Theme == MetroThemeStyle.Dark ? Color.WhiteSmoke : Color.Black);
                valueLabel.LabelBold = true;
            }

            this.formsPlot.Plot.HideAxesAndGrid();
            this.formsPlot.Refresh();

            var pb = this.formsPlot.Controls.OfType<PictureBox>().FirstOrDefault();
            if (pb != null) {
                pb.Enabled = false;
            }
        }

        private void Medal_MouseEnter(object sender, EventArgs e) {
            this.Cursor = this.Theme == MetroThemeStyle.Dark
                          ? new System.Windows.Forms.Cursor(Properties.Resources.transform_gray_icon.GetHicon())
                          : new System.Windows.Forms.Cursor(Properties.Resources.transform_icon.GetHicon());
        }

        private void Medal_MouseLeave(object sender, EventArgs e) {
            this.Cursor = Cursors.Default;
        }

        private void Medal_MouseClick(object sender, EventArgs e) {
            this.switching = !this.switching;
            if (this.switching) {
                this.lblCountGoldMedal.Text = this.goldMedalPercent;
                this.lblCountSilverMedal.Text = this.silverMedalPercent;
                this.lblCountBronzeMedal.Text = this.bronzeMedalPercent;
                this.lblCountPinkMedal.Text = this.pinkMedalPercent;
                this.lblCountEliminatedMedal.Text = this.eliminatedMedalPercent;
            } else {
                this.lblCountGoldMedal.Text = this.goldMedalCount;
                this.lblCountSilverMedal.Text = this.silverMedalCount;
                this.lblCountBronzeMedal.Text = this.bronzeMedalCount;
                this.lblCountPinkMedal.Text = this.pinkMedalCount;
                this.lblCountEliminatedMedal.Text = this.eliminatedMedalCount;
            }
        }

        private void cboRoundList_SelectedIndexChanged(object sender, EventArgs e) {
            if (this.isInitComplete) {
                this.formsPlot.Plot.Clear();
                this.SetGraph(((MetroComboBox)sender).SelectedItem);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (keyData == Keys.Tab) { SendKeys.Send("%"); }
            if (keyData == Keys.Escape) {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ChangeLanguage() {
            this.lblRoundTime.Font = Overlay.GetDefaultFont(18, Stats.CurrentLanguage);
            this.lblBestRecord.Font = Overlay.GetDefaultFont(18, Stats.CurrentLanguage);
            this.lblWorstRecord.Font = Overlay.GetDefaultFont(18, Stats.CurrentLanguage);
            this.lblRoundType.Font = Overlay.GetDefaultFont(18, Stats.CurrentLanguage);
            this.lblCountGoldMedal.Font = Overlay.GetDefaultFont(45, 0);
            this.lblCountSilverMedal.Font = Overlay.GetDefaultFont(45, 0);
            this.lblCountBronzeMedal.Font = Overlay.GetDefaultFont(45, 0);
            this.lblCountPinkMedal.Font = Overlay.GetDefaultFont(45, 0);
            this.lblCountEliminatedMedal.Font = Overlay.GetDefaultFont(45, 0);
        }
    }
}