using System.Windows.Forms;

namespace FinalBeansStats {
    partial class Stats {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Stats));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mtgIgnoreLevelTypeWhenSorting = new MetroFramework.Controls.MetroToggle();
            this.lblIgnoreLevelTypeWhenSorting = new System.Windows.Forms.Label();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.menuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFilters = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStatsFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCustomRangeStats = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuAllStats = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeasonStats = new System.Windows.Forms.ToolStripMenuItem();
            this.menuWeekStats = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDayStats = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSessionStats = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPartyFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAllPartyStats = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSoloStats = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPartyStats = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditProfiles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuOverlay = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLaunchFinalBeans = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUsefulThings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFinalBeans = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFinalBeansOfficial = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFinalBeansDiscord = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFinalBeansTwitterX = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFinalBeansTrello = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuRollOffClub = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.menuLostTempleAnalyzer = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFinalBeansDBMain = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFinalBeansDBShows = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFinalBeansDBDiscovery = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFinalBeansDBShop = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFinalBeansDBNewsfeeds = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFinalBeansDBStrings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFinalBeansDBCosmetics = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFinalBeansDBCrownRanks = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFinalBeansDBLiveEvents = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFinalBeansDBDailyShop = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator18 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFinalBeansDBCreative = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSeparator19 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.infoStrip = new System.Windows.Forms.ToolStrip();
            this.lblCurrentProfileIcon = new System.Windows.Forms.ToolStripLabel();
            this.lblCurrentProfile = new System.Windows.Forms.ToolStripLabel();
            this.lblTotalShows = new System.Windows.Forms.ToolStripLabel();
            this.lblTotalRounds = new System.Windows.Forms.ToolStripLabel();
            this.lblTotalFinals = new System.Windows.Forms.ToolStripLabel();
            this.lblTotalWins = new System.Windows.Forms.ToolStripLabel();
            this.lblPlayerName = new System.Windows.Forms.ToolStripLabel();
            this.infoStrip2 = new System.Windows.Forms.ToolStrip();
            this.lblTotalTime = new System.Windows.Forms.ToolStripLabel();
            this.lblGoldMedal = new System.Windows.Forms.ToolStripLabel();
            this.lblSilverMedal = new System.Windows.Forms.ToolStripLabel();
            this.lblBronzeMedal = new System.Windows.Forms.ToolStripLabel();
            this.lblPinkMedal = new System.Windows.Forms.ToolStripLabel();
            this.lblEliminatedMedal = new System.Windows.Forms.ToolStripLabel();
            this.lblKudos = new System.Windows.Forms.ToolStripLabel();
            this.infoStrip3 = new System.Windows.Forms.ToolStrip();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayCMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.trayOverlay = new System.Windows.Forms.ToolStripMenuItem();
            this.traySeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.traySettings = new System.Windows.Forms.ToolStripMenuItem();
            this.traySeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.trayFilters = new System.Windows.Forms.ToolStripMenuItem();
            this.trayStatsFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.trayCustomRangeStats = new System.Windows.Forms.ToolStripMenuItem();
            this.traySubSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.trayAllStats = new System.Windows.Forms.ToolStripMenuItem();
            this.traySeasonStats = new System.Windows.Forms.ToolStripMenuItem();
            this.trayWeekStats = new System.Windows.Forms.ToolStripMenuItem();
            this.trayDayStats = new System.Windows.Forms.ToolStripMenuItem();
            this.traySessionStats = new System.Windows.Forms.ToolStripMenuItem();
            this.trayPartyFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.trayAllPartyStats = new System.Windows.Forms.ToolStripMenuItem();
            this.traySoloStats = new System.Windows.Forms.ToolStripMenuItem();
            this.trayPartyStats = new System.Windows.Forms.ToolStripMenuItem();
            this.trayProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.trayEditProfiles = new System.Windows.Forms.ToolStripMenuItem();
            this.traySubSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.traySeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.trayUsefulThings = new System.Windows.Forms.ToolStripMenuItem();
            this.trayFinalBeans = new System.Windows.Forms.ToolStripMenuItem();
            this.trayFinalBeansOfficial = new System.Windows.Forms.ToolStripMenuItem();
            this.traySubSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.trayFinalBeansDiscord = new System.Windows.Forms.ToolStripMenuItem();
            this.traySubSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.trayFinalBeansTwitterX = new System.Windows.Forms.ToolStripMenuItem();
            this.traySubSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.trayFinalBeansTrello = new System.Windows.Forms.ToolStripMenuItem();
            this.traySubSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.trayRollOffClub = new System.Windows.Forms.ToolStripMenuItem();
            this.traySubSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.trayLostTempleAnalyzer = new System.Windows.Forms.ToolStripMenuItem();
            this.trayUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.trayHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.trayLaunchFinalBeans = new System.Windows.Forms.ToolStripMenuItem();
            this.traySeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.trayExitProgram = new System.Windows.Forms.ToolStripMenuItem();
            this.gridDetails = new FinalBeansStats.Grid();
            this.mlReportBug = new MetroFramework.Controls.MetroLink();
            this.menu.SuspendLayout();
            this.infoStrip.SuspendLayout();
            this.infoStrip2.SuspendLayout();
            this.infoStrip3.SuspendLayout();
            this.trayCMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // mtgIgnoreLevelTypeWhenSorting
            // 
            this.mtgIgnoreLevelTypeWhenSorting.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mtgIgnoreLevelTypeWhenSorting.DisplayStatus = false;
            this.mtgIgnoreLevelTypeWhenSorting.Location = new System.Drawing.Point(17, 180);
            this.mtgIgnoreLevelTypeWhenSorting.Name = "mtgIgnoreLevelTypeWhenSorting";
            this.mtgIgnoreLevelTypeWhenSorting.Size = new System.Drawing.Size(21, 15);
            this.mtgIgnoreLevelTypeWhenSorting.TabIndex = 3;
            this.mtgIgnoreLevelTypeWhenSorting.Text = "Off";
            this.mtgIgnoreLevelTypeWhenSorting.UseSelectable = true;
            this.mtgIgnoreLevelTypeWhenSorting.CheckedChanged += new System.EventHandler(this.mtgIgnoreLevelTypeWhenSorting_CheckedChanged);
            this.mtgIgnoreLevelTypeWhenSorting.MouseEnter += new System.EventHandler(this.Toggle_MouseEnter);
            this.mtgIgnoreLevelTypeWhenSorting.MouseLeave += new System.EventHandler(this.Toggle_MouseLeave);
            // 
            // lblIgnoreLevelTypeWhenSorting
            // 
            this.lblIgnoreLevelTypeWhenSorting.AutoSize = true;
            this.lblIgnoreLevelTypeWhenSorting.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblIgnoreLevelTypeWhenSorting.Location = new System.Drawing.Point(38, 180);
            this.lblIgnoreLevelTypeWhenSorting.Name = "lblIgnoreLevelTypeWhenSorting";
            this.lblIgnoreLevelTypeWhenSorting.Size = new System.Drawing.Size(0, 13);
            this.lblIgnoreLevelTypeWhenSorting.TabIndex = 6;
            this.lblIgnoreLevelTypeWhenSorting.Click += new System.EventHandler(this.lblIgnoreLevelTypeWhenSorting_Click);
            this.lblIgnoreLevelTypeWhenSorting.MouseEnter += new System.EventHandler(this.Toggle_MouseEnter);
            this.lblIgnoreLevelTypeWhenSorting.MouseLeave += new System.EventHandler(this.Toggle_MouseLeave);
            // 
            // menu
            // 
            this.menu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.menu.AutoSize = false;
            this.menu.BackColor = System.Drawing.Color.Transparent;
            this.menu.Dock = System.Windows.Forms.DockStyle.None;
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSettings,
            this.menuFilters,
            this.menuProfile,
            this.menuOverlay,
            this.menuUpdate,
            this.menuHelp,
            this.menuLaunchFinalBeans,
            this.menuUsefulThings});
            this.menu.Location = new System.Drawing.Point(0, 65);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(940, 27);
            this.menu.TabIndex = 12;
            this.menu.Text = "menuStrip1";
            // 
            // menuSettings
            // 
            this.menuSettings.Image = global::FinalBeansStats.Properties.Resources.setting_icon;
            this.menuSettings.Name = "menuSettings";
            this.menuSettings.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuSettings.Size = new System.Drawing.Size(77, 23);
            this.menuSettings.Text = "Settings";
            this.menuSettings.Click += new System.EventHandler(this.menuSettings_Click);
            this.menuSettings.MouseLeave += new System.EventHandler(this.setCursor_MouseLeave);
            this.menuSettings.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // menuFilters
            // 
            this.menuFilters.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStatsFilter,
            this.menuPartyFilter});
            this.menuFilters.Image = global::FinalBeansStats.Properties.Resources.filter_icon;
            this.menuFilters.Name = "menuFilters";
            this.menuFilters.Size = new System.Drawing.Size(66, 23);
            this.menuFilters.Text = "Filters";
            // 
            // menuStatsFilter
            // 
            this.menuStatsFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCustomRangeStats,
            this.menuSeparator1,
            this.menuAllStats,
            this.menuSeasonStats,
            this.menuWeekStats,
            this.menuDayStats,
            this.menuSessionStats});
            this.menuStatsFilter.Image = global::FinalBeansStats.Properties.Resources.stat_icon;
            this.menuStatsFilter.Name = "menuStatsFilter";
            this.menuStatsFilter.Size = new System.Drawing.Size(101, 22);
            this.menuStatsFilter.Text = "Stats";
            // 
            // menuCustomRangeStats
            // 
            this.menuCustomRangeStats.Image = global::FinalBeansStats.Properties.Resources.calendar_icon;
            this.menuCustomRangeStats.Name = "menuCustomRangeStats";
            this.menuCustomRangeStats.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Q)));
            this.menuCustomRangeStats.Size = new System.Drawing.Size(227, 22);
            this.menuCustomRangeStats.Text = "Custom Range";
            this.menuCustomRangeStats.Click += new System.EventHandler(this.menuStats_Click);
            this.menuCustomRangeStats.MouseLeave += new System.EventHandler(this.setCursor_MouseLeave);
            this.menuCustomRangeStats.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // menuSeparator1
            // 
            this.menuSeparator1.Name = "menuSeparator1";
            this.menuSeparator1.Size = new System.Drawing.Size(224, 6);
            // 
            // menuAllStats
            // 
            this.menuAllStats.Checked = true;
            this.menuAllStats.CheckOnClick = true;
            this.menuAllStats.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuAllStats.Name = "menuAllStats";
            this.menuAllStats.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
            this.menuAllStats.Size = new System.Drawing.Size(227, 22);
            this.menuAllStats.Text = "All";
            this.menuAllStats.Click += new System.EventHandler(this.menuStats_Click);
            this.menuAllStats.MouseLeave += new System.EventHandler(this.setCursor_MouseLeave);
            this.menuAllStats.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // menuSeasonStats
            // 
            this.menuSeasonStats.CheckOnClick = true;
            this.menuSeasonStats.Name = "menuSeasonStats";
            this.menuSeasonStats.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.menuSeasonStats.Size = new System.Drawing.Size(227, 22);
            this.menuSeasonStats.Text = "Season";
            this.menuSeasonStats.Click += new System.EventHandler(this.menuStats_Click);
            this.menuSeasonStats.MouseLeave += new System.EventHandler(this.setCursor_MouseLeave);
            this.menuSeasonStats.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // menuWeekStats
            // 
            this.menuWeekStats.CheckOnClick = true;
            this.menuWeekStats.Name = "menuWeekStats";
            this.menuWeekStats.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.W)));
            this.menuWeekStats.Size = new System.Drawing.Size(227, 22);
            this.menuWeekStats.Text = "Week";
            this.menuWeekStats.Click += new System.EventHandler(this.menuStats_Click);
            this.menuWeekStats.MouseLeave += new System.EventHandler(this.setCursor_MouseLeave);
            this.menuWeekStats.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // menuDayStats
            // 
            this.menuDayStats.CheckOnClick = true;
            this.menuDayStats.Name = "menuDayStats";
            this.menuDayStats.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
            this.menuDayStats.Size = new System.Drawing.Size(227, 22);
            this.menuDayStats.Text = "Day";
            this.menuDayStats.Click += new System.EventHandler(this.menuStats_Click);
            this.menuDayStats.MouseLeave += new System.EventHandler(this.setCursor_MouseLeave);
            this.menuDayStats.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // menuSessionStats
            // 
            this.menuSessionStats.CheckOnClick = true;
            this.menuSessionStats.Name = "menuSessionStats";
            this.menuSessionStats.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.G)));
            this.menuSessionStats.Size = new System.Drawing.Size(227, 22);
            this.menuSessionStats.Text = "Session";
            this.menuSessionStats.Click += new System.EventHandler(this.menuStats_Click);
            this.menuSessionStats.MouseLeave += new System.EventHandler(this.setCursor_MouseLeave);
            this.menuSessionStats.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // menuPartyFilter
            // 
            this.menuPartyFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAllPartyStats,
            this.menuSoloStats,
            this.menuPartyStats});
            this.menuPartyFilter.Image = global::FinalBeansStats.Properties.Resources.player_icon;
            this.menuPartyFilter.Name = "menuPartyFilter";
            this.menuPartyFilter.Size = new System.Drawing.Size(101, 22);
            this.menuPartyFilter.Text = "Party";
            // 
            // menuAllPartyStats
            // 
            this.menuAllPartyStats.Checked = true;
            this.menuAllPartyStats.CheckOnClick = true;
            this.menuAllPartyStats.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuAllPartyStats.Name = "menuAllPartyStats";
            this.menuAllPartyStats.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F)));
            this.menuAllPartyStats.Size = new System.Drawing.Size(174, 22);
            this.menuAllPartyStats.Text = "All";
            this.menuAllPartyStats.Click += new System.EventHandler(this.menuStats_Click);
            this.menuAllPartyStats.MouseLeave += new System.EventHandler(this.setCursor_MouseLeave);
            this.menuAllPartyStats.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // menuSoloStats
            // 
            this.menuSoloStats.CheckOnClick = true;
            this.menuSoloStats.Name = "menuSoloStats";
            this.menuSoloStats.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.menuSoloStats.Size = new System.Drawing.Size(174, 22);
            this.menuSoloStats.Text = "Solo";
            this.menuSoloStats.Click += new System.EventHandler(this.menuStats_Click);
            this.menuSoloStats.MouseLeave += new System.EventHandler(this.setCursor_MouseLeave);
            this.menuSoloStats.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // menuPartyStats
            // 
            this.menuPartyStats.CheckOnClick = true;
            this.menuPartyStats.Name = "menuPartyStats";
            this.menuPartyStats.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
            this.menuPartyStats.Size = new System.Drawing.Size(174, 22);
            this.menuPartyStats.Text = "Party";
            this.menuPartyStats.Click += new System.EventHandler(this.menuStats_Click);
            this.menuPartyStats.MouseLeave += new System.EventHandler(this.setCursor_MouseLeave);
            this.menuPartyStats.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // menuProfile
            // 
            this.menuProfile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEditProfiles,
            this.menuSeparator2});
            this.menuProfile.Image = global::FinalBeansStats.Properties.Resources.profile_icon;
            this.menuProfile.Name = "menuProfile";
            this.menuProfile.Size = new System.Drawing.Size(69, 23);
            this.menuProfile.Text = "Profile";
            // 
            // menuEditProfiles
            // 
            this.menuEditProfiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.menuEditProfiles.Image = global::FinalBeansStats.Properties.Resources.setting_icon;
            this.menuEditProfiles.Name = "menuEditProfiles";
            this.menuEditProfiles.Size = new System.Drawing.Size(153, 22);
            this.menuEditProfiles.Text = "Profile Settings";
            this.menuEditProfiles.Click += new System.EventHandler(this.menuEditProfiles_Click);
            this.menuEditProfiles.MouseLeave += new System.EventHandler(this.setCursor_MouseLeave);
            this.menuEditProfiles.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // menuSeparator2
            // 
            this.menuSeparator2.Name = "menuSeparator2";
            this.menuSeparator2.Size = new System.Drawing.Size(150, 6);
            // 
            // menuOverlay
            // 
            this.menuOverlay.Image = global::FinalBeansStats.Properties.Resources.stat_gray_icon;
            this.menuOverlay.Name = "menuOverlay";
            this.menuOverlay.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuOverlay.Size = new System.Drawing.Size(107, 23);
            this.menuOverlay.Text = "Show Overlay";
            this.menuOverlay.Click += new System.EventHandler(this.menuOverlay_Click);
            this.menuOverlay.MouseEnter += new System.EventHandler(this.menuOverlay_MouseEnter);
            this.menuOverlay.MouseLeave += new System.EventHandler(this.menuOverlay_MouseLeave);
            this.menuOverlay.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // menuUpdate
            // 
            this.menuUpdate.Image = global::FinalBeansStats.Properties.Resources.github_icon;
            this.menuUpdate.Name = "menuUpdate";
            this.menuUpdate.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.menuUpdate.Size = new System.Drawing.Size(73, 23);
            this.menuUpdate.Text = "Update";
            this.menuUpdate.Click += new System.EventHandler(this.menuUpdate_Click);
            this.menuUpdate.MouseEnter += new System.EventHandler(this.menuUpdate_MouseEnter);
            this.menuUpdate.MouseLeave += new System.EventHandler(this.menuUpdate_MouseLeave);
            this.menuUpdate.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // menuHelp
            // 
            this.menuHelp.Image = global::FinalBeansStats.Properties.Resources.github_icon;
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.menuHelp.Size = new System.Drawing.Size(60, 23);
            this.menuHelp.Text = "Help";
            this.menuHelp.Click += new System.EventHandler(this.menuHelp_Click);
            this.menuHelp.MouseEnter += new System.EventHandler(this.menuUpdate_MouseEnter);
            this.menuHelp.MouseLeave += new System.EventHandler(this.menuUpdate_MouseLeave);
            this.menuHelp.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // menuLaunchFinalBeans
            // 
            this.menuLaunchFinalBeans.Image = global::FinalBeansStats.Properties.Resources.finalbeans_icon;
            this.menuLaunchFinalBeans.Name = "menuLaunchFinalBeans";
            this.menuLaunchFinalBeans.Size = new System.Drawing.Size(133, 23);
            this.menuLaunchFinalBeans.Text = "Launch FinalBeans";
            this.menuLaunchFinalBeans.Click += new System.EventHandler(this.menuLaunchFinalBeans_Click);
            this.menuLaunchFinalBeans.MouseLeave += new System.EventHandler(this.setCursor_MouseLeave);
            this.menuLaunchFinalBeans.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // menuUsefulThings
            // 
            this.menuUsefulThings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFinalBeans,
            this.toolStripSeparator1,
            this.menuRollOffClub,
            this.menuSeparator7,
            this.menuLostTempleAnalyzer});
            this.menuUsefulThings.Image = global::FinalBeansStats.Properties.Resources.main_icon;
            this.menuUsefulThings.Name = "menuUsefulThings";
            this.menuUsefulThings.Size = new System.Drawing.Size(107, 23);
            this.menuUsefulThings.Text = "Useful things!";
            // 
            // menuFinalBeans
            // 
            this.menuFinalBeans.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFinalBeansOfficial,
            this.toolStripSeparator2,
            this.menuFinalBeansDiscord,
            this.toolStripSeparator3,
            this.menuFinalBeansTwitterX,
            this.toolStripSeparator4,
            this.menuFinalBeansTrello});
            this.menuFinalBeans.Image = global::FinalBeansStats.Properties.Resources.finalbeans_icon;
            this.menuFinalBeans.Name = "menuFinalBeans";
            this.menuFinalBeans.Size = new System.Drawing.Size(186, 22);
            this.menuFinalBeans.Text = "FinalBeans";
            // 
            // menuFinalBeansOfficial
            // 
            this.menuFinalBeansOfficial.Image = global::FinalBeansStats.Properties.Resources.fb_website_icon;
            this.menuFinalBeansOfficial.Name = "menuFinalBeansOfficial";
            this.menuFinalBeansOfficial.Size = new System.Drawing.Size(183, 22);
            this.menuFinalBeansOfficial.Text = "Website";
            this.menuFinalBeansOfficial.Click += new System.EventHandler(this.menuUsefulThings_Click);
            this.menuFinalBeansOfficial.MouseEnter += new System.EventHandler(this.menuUsefulThings_MouseEnter);
            this.menuFinalBeansOfficial.MouseLeave += new System.EventHandler(this.menuUsefulThings_MouseLeave);
            this.menuFinalBeansOfficial.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(180, 6);
            // 
            // menuFinalBeansDiscord
            // 
            this.menuFinalBeansDiscord.Image = global::FinalBeansStats.Properties.Resources.discord_logo;
            this.menuFinalBeansDiscord.Name = "menuFinalBeansDiscord";
            this.menuFinalBeansDiscord.Size = new System.Drawing.Size(183, 22);
            this.menuFinalBeansDiscord.Text = "Discord";
            this.menuFinalBeansDiscord.Click += new System.EventHandler(this.menuUsefulThings_Click);
            this.menuFinalBeansDiscord.MouseEnter += new System.EventHandler(this.menuUsefulThings_MouseEnter);
            this.menuFinalBeansDiscord.MouseLeave += new System.EventHandler(this.menuUsefulThings_MouseLeave);
            this.menuFinalBeansDiscord.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(180, 6);
            // 
            // menuFinalBeansTwitterX
            // 
            this.menuFinalBeansTwitterX.Image = global::FinalBeansStats.Properties.Resources.x_icon;
            this.menuFinalBeansTwitterX.Name = "menuFinalBeansTwitterX";
            this.menuFinalBeansTwitterX.Size = new System.Drawing.Size(183, 22);
            this.menuFinalBeansTwitterX.Text = "Twitter/X";
            this.menuFinalBeansTwitterX.Click += new System.EventHandler(this.menuUsefulThings_Click);
            this.menuFinalBeansTwitterX.MouseEnter += new System.EventHandler(this.menuUsefulThings_MouseEnter);
            this.menuFinalBeansTwitterX.MouseLeave += new System.EventHandler(this.menuUsefulThings_MouseLeave);
            this.menuFinalBeansTwitterX.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(180, 6);
            // 
            // menuFinalBeansTrello
            // 
            this.menuFinalBeansTrello.Image = global::FinalBeansStats.Properties.Resources.trello_logo;
            this.menuFinalBeansTrello.Name = "menuFinalBeansTrello";
            this.menuFinalBeansTrello.Size = new System.Drawing.Size(183, 22);
            this.menuFinalBeansTrello.Text = "Trello (Bug Tracking)";
            this.menuFinalBeansTrello.Click += new System.EventHandler(this.menuUsefulThings_Click);
            this.menuFinalBeansTrello.MouseEnter += new System.EventHandler(this.menuUsefulThings_MouseEnter);
            this.menuFinalBeansTrello.MouseLeave += new System.EventHandler(this.menuUsefulThings_MouseLeave);
            this.menuFinalBeansTrello.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(183, 6);
            // 
            // menuRollOffClub
            // 
            this.menuRollOffClub.Image = global::FinalBeansStats.Properties.Resources.roll_off_club_icon;
            this.menuRollOffClub.Name = "menuRollOffClub";
            this.menuRollOffClub.Size = new System.Drawing.Size(186, 22);
            this.menuRollOffClub.Text = "Roll Off Club";
            this.menuRollOffClub.Click += new System.EventHandler(this.menuUsefulThings_Click);
            this.menuRollOffClub.MouseEnter += new System.EventHandler(this.menuUsefulThings_MouseEnter);
            this.menuRollOffClub.MouseLeave += new System.EventHandler(this.menuUsefulThings_MouseLeave);
            this.menuRollOffClub.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // menuSeparator7
            // 
            this.menuSeparator7.Name = "menuSeparator7";
            this.menuSeparator7.Size = new System.Drawing.Size(183, 6);
            // 
            // menuLostTempleAnalyzer
            // 
            this.menuLostTempleAnalyzer.Image = global::FinalBeansStats.Properties.Resources.lost_temple_analyzer_icon;
            this.menuLostTempleAnalyzer.Name = "menuLostTempleAnalyzer";
            this.menuLostTempleAnalyzer.Size = new System.Drawing.Size(186, 22);
            this.menuLostTempleAnalyzer.Text = "Lost Temple Analyzer";
            this.menuLostTempleAnalyzer.Click += new System.EventHandler(this.menuUsefulThings_Click);
            this.menuLostTempleAnalyzer.MouseEnter += new System.EventHandler(this.menuUsefulThings_MouseEnter);
            this.menuLostTempleAnalyzer.MouseLeave += new System.EventHandler(this.menuUsefulThings_MouseLeave);
            this.menuLostTempleAnalyzer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // menuFinalBeansDBMain
            // 
            this.menuFinalBeansDBMain.Name = "menuFinalBeansDBMain";
            this.menuFinalBeansDBMain.Size = new System.Drawing.Size(32, 19);
            // 
            // menuSeparator9
            // 
            this.menuSeparator9.Name = "menuSeparator9";
            this.menuSeparator9.Size = new System.Drawing.Size(6, 6);
            // 
            // menuFinalBeansDBShows
            // 
            this.menuFinalBeansDBShows.Name = "menuFinalBeansDBShows";
            this.menuFinalBeansDBShows.Size = new System.Drawing.Size(32, 19);
            // 
            // menuSeparator10
            // 
            this.menuSeparator10.Name = "menuSeparator10";
            this.menuSeparator10.Size = new System.Drawing.Size(6, 6);
            // 
            // menuFinalBeansDBDiscovery
            // 
            this.menuFinalBeansDBDiscovery.Name = "menuFinalBeansDBDiscovery";
            this.menuFinalBeansDBDiscovery.Size = new System.Drawing.Size(32, 19);
            // 
            // menuSeparator11
            // 
            this.menuSeparator11.Name = "menuSeparator11";
            this.menuSeparator11.Size = new System.Drawing.Size(6, 6);
            // 
            // menuFinalBeansDBShop
            // 
            this.menuFinalBeansDBShop.Name = "menuFinalBeansDBShop";
            this.menuFinalBeansDBShop.Size = new System.Drawing.Size(32, 19);
            // 
            // menuSeparator12
            // 
            this.menuSeparator12.Name = "menuSeparator12";
            this.menuSeparator12.Size = new System.Drawing.Size(6, 6);
            // 
            // menuFinalBeansDBNewsfeeds
            // 
            this.menuFinalBeansDBNewsfeeds.Name = "menuFinalBeansDBNewsfeeds";
            this.menuFinalBeansDBNewsfeeds.Size = new System.Drawing.Size(32, 19);
            // 
            // menuSeparator13
            // 
            this.menuSeparator13.Name = "menuSeparator13";
            this.menuSeparator13.Size = new System.Drawing.Size(6, 6);
            // 
            // menuFinalBeansDBStrings
            // 
            this.menuFinalBeansDBStrings.Name = "menuFinalBeansDBStrings";
            this.menuFinalBeansDBStrings.Size = new System.Drawing.Size(32, 19);
            // 
            // menuSeparator14
            // 
            this.menuSeparator14.Name = "menuSeparator14";
            this.menuSeparator14.Size = new System.Drawing.Size(159, 6);
            // 
            // menuFinalBeansDBCosmetics
            // 
            this.menuFinalBeansDBCosmetics.Name = "menuFinalBeansDBCosmetics";
            this.menuFinalBeansDBCosmetics.Size = new System.Drawing.Size(175, 22);
            this.menuFinalBeansDBCosmetics.Text = "Cosmetics";
            this.menuFinalBeansDBCosmetics.Click += new System.EventHandler(this.menuUsefulThings_Click);
            this.menuFinalBeansDBCosmetics.MouseMove += new System.Windows.Forms.MouseEventHandler(this.setCursor_MouseMove);
            // 
            // menuSeparator15
            // 
            this.menuSeparator15.Name = "menuSeparator15";
            this.menuSeparator15.Size = new System.Drawing.Size(6, 6);
            // 
            // menuFinalBeansDBCrownRanks
            // 
            this.menuFinalBeansDBCrownRanks.Name = "menuFinalBeansDBCrownRanks";
            this.menuFinalBeansDBCrownRanks.Size = new System.Drawing.Size(32, 19);
            // 
            // menuSeparator16
            // 
            this.menuSeparator16.Name = "menuSeparator16";
            this.menuSeparator16.Size = new System.Drawing.Size(6, 6);
            // 
            // menuFinalBeansDBLiveEvents
            // 
            this.menuFinalBeansDBLiveEvents.Name = "menuFinalBeansDBLiveEvents";
            this.menuFinalBeansDBLiveEvents.Size = new System.Drawing.Size(32, 19);
            // 
            // menuSeparator17
            // 
            this.menuSeparator17.Name = "menuSeparator17";
            this.menuSeparator17.Size = new System.Drawing.Size(6, 6);
            // 
            // menuFinalBeansDBDailyShop
            // 
            this.menuFinalBeansDBDailyShop.Name = "menuFinalBeansDBDailyShop";
            this.menuFinalBeansDBDailyShop.Size = new System.Drawing.Size(32, 19);
            // 
            // menuSeparator18
            // 
            this.menuSeparator18.Name = "menuSeparator18";
            this.menuSeparator18.Size = new System.Drawing.Size(6, 6);
            // 
            // menuFinalBeansDBCreative
            // 
            this.menuFinalBeansDBCreative.Name = "menuFinalBeansDBCreative";
            this.menuFinalBeansDBCreative.Size = new System.Drawing.Size(32, 19);
            // 
            // menuSeparator19
            // 
            this.menuSeparator19.Name = "menuSeparator19";
            this.menuSeparator19.Size = new System.Drawing.Size(6, 6);
            // 
            // menuSeparator3
            // 
            this.menuSeparator3.Name = "menuSeparator3";
            this.menuSeparator3.Size = new System.Drawing.Size(6, 6);
            // 
            // menuSeparator8
            // 
            this.menuSeparator8.Name = "menuSeparator8";
            this.menuSeparator8.Size = new System.Drawing.Size(6, 6);
            // 
            // infoStrip
            // 
            this.infoStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoStrip.AutoSize = false;
            this.infoStrip.BackColor = System.Drawing.Color.Transparent;
            this.infoStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.infoStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.infoStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.infoStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblCurrentProfileIcon,
            this.lblCurrentProfile,
            this.lblTotalShows,
            this.lblTotalRounds,
            this.lblTotalFinals,
            this.lblTotalWins});
            this.infoStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.infoStrip.Location = new System.Drawing.Point(0, 93);
            this.infoStrip.Name = "infoStrip";
            this.infoStrip.Padding = new System.Windows.Forms.Padding(20, 6, 20, 1);
            this.infoStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.infoStrip.Size = new System.Drawing.Size(1050, 27);
            this.infoStrip.TabIndex = 13;
            // 
            // lblCurrentProfileIcon
            // 
            this.lblCurrentProfileIcon.ForeColor = System.Drawing.Color.Crimson;
            this.lblCurrentProfileIcon.Image = global::FinalBeansStats.Properties.Resources.profile2_icon;
            this.lblCurrentProfileIcon.Margin = new System.Windows.Forms.Padding(4, 3, 1, 3);
            this.lblCurrentProfileIcon.Name = "lblCurrentProfileIcon";
            this.lblCurrentProfileIcon.Size = new System.Drawing.Size(16, 16);
            this.lblCurrentProfileIcon.Click += new System.EventHandler(this.lblCurrentProfileIcon_Click);
            this.lblCurrentProfileIcon.MouseEnter += new System.EventHandler(this.infoStrip_MouseEnter);
            this.lblCurrentProfileIcon.MouseLeave += new System.EventHandler(this.infoStrip_MouseLeave);
            // 
            // lblCurrentProfile
            // 
            this.lblCurrentProfile.Margin = new System.Windows.Forms.Padding(1, 1, 17, 2);
            this.lblCurrentProfile.Name = "lblCurrentProfile";
            this.lblCurrentProfile.Size = new System.Drawing.Size(30, 15);
            this.lblCurrentProfile.Text = "Solo";
            this.lblCurrentProfile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblCurrentProfile_MouseDown);
            this.lblCurrentProfile.MouseEnter += new System.EventHandler(this.infoStrip_MouseEnter);
            this.lblCurrentProfile.MouseLeave += new System.EventHandler(this.infoStrip_MouseLeave);
            // 
            // lblTotalShows
            // 
            this.lblTotalShows.ForeColor = System.Drawing.Color.Blue;
            this.lblTotalShows.Image = global::FinalBeansStats.Properties.Resources.show_icon;
            this.lblTotalShows.Margin = new System.Windows.Forms.Padding(10, 1, 5, 2);
            this.lblTotalShows.Name = "lblTotalShows";
            this.lblTotalShows.Size = new System.Drawing.Size(29, 16);
            this.lblTotalShows.Text = "0";
            this.lblTotalShows.Click += new System.EventHandler(this.lblTotalShows_Click);
            this.lblTotalShows.MouseEnter += new System.EventHandler(this.infoStrip_MouseEnter);
            this.lblTotalShows.MouseLeave += new System.EventHandler(this.infoStrip_MouseLeave);
            // 
            // lblTotalRounds
            // 
            this.lblTotalRounds.ForeColor = System.Drawing.Color.Blue;
            this.lblTotalRounds.Image = global::FinalBeansStats.Properties.Resources.round_icon;
            this.lblTotalRounds.Margin = new System.Windows.Forms.Padding(10, 1, 5, 2);
            this.lblTotalRounds.Name = "lblTotalRounds";
            this.lblTotalRounds.Size = new System.Drawing.Size(29, 16);
            this.lblTotalRounds.Text = "0";
            this.lblTotalRounds.Click += new System.EventHandler(this.lblTotalRounds_Click);
            this.lblTotalRounds.MouseEnter += new System.EventHandler(this.infoStrip_MouseEnter);
            this.lblTotalRounds.MouseLeave += new System.EventHandler(this.infoStrip_MouseLeave);
            // 
            // lblTotalFinals
            // 
            this.lblTotalFinals.ForeColor = System.Drawing.Color.Blue;
            this.lblTotalFinals.Image = global::FinalBeansStats.Properties.Resources.final_icon;
            this.lblTotalFinals.Margin = new System.Windows.Forms.Padding(10, 1, 5, 2);
            this.lblTotalFinals.Name = "lblTotalFinals";
            this.lblTotalFinals.Size = new System.Drawing.Size(65, 16);
            this.lblTotalFinals.Text = "0 (0.0%)";
            this.lblTotalFinals.Click += new System.EventHandler(this.lblTotalFinals_Click);
            this.lblTotalFinals.MouseEnter += new System.EventHandler(this.infoStrip_MouseEnter);
            this.lblTotalFinals.MouseLeave += new System.EventHandler(this.infoStrip_MouseLeave);
            // 
            // lblTotalWins
            // 
            this.lblTotalWins.ForeColor = System.Drawing.Color.Blue;
            this.lblTotalWins.Image = global::FinalBeansStats.Properties.Resources.crown_icon;
            this.lblTotalWins.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.lblTotalWins.Name = "lblTotalWins";
            this.lblTotalWins.Size = new System.Drawing.Size(65, 16);
            this.lblTotalWins.Text = "0 (0.0%)";
            this.lblTotalWins.Click += new System.EventHandler(this.lblTotalWins_Click);
            this.lblTotalWins.MouseEnter += new System.EventHandler(this.infoStrip_MouseEnter);
            this.lblTotalWins.MouseLeave += new System.EventHandler(this.infoStrip_MouseLeave);
            // 
            // lblPlayerName
            // 
            this.lblPlayerName.ForeColor = System.Drawing.Color.Blue;
            this.lblPlayerName.Image = global::FinalBeansStats.Properties.Resources.pc_icon;
            this.lblPlayerName.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.lblPlayerName.Name = "lblPlayerName";
            this.lblPlayerName.Size = new System.Drawing.Size(87, 16);
            this.lblPlayerName.Text = "PlayerName";
            this.lblPlayerName.Click += new System.EventHandler(this.lblPlayerName_Click);
            this.lblPlayerName.MouseEnter += new System.EventHandler(this.infoStrip_MouseEnter);
            this.lblPlayerName.MouseLeave += new System.EventHandler(this.infoStrip_MouseLeave);
            // 
            // infoStrip2
            // 
            this.infoStrip2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoStrip2.AutoSize = false;
            this.infoStrip2.BackColor = System.Drawing.Color.Transparent;
            this.infoStrip2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.infoStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.infoStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.infoStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblTotalTime,
            this.lblGoldMedal,
            this.lblSilverMedal,
            this.lblBronzeMedal,
            this.lblPinkMedal,
            this.lblEliminatedMedal,
            this.lblKudos});
            this.infoStrip2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.infoStrip2.Location = new System.Drawing.Point(0, 120);
            this.infoStrip2.Name = "infoStrip2";
            this.infoStrip2.Padding = new System.Windows.Forms.Padding(14, 6, 14, 1);
            this.infoStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.infoStrip2.Size = new System.Drawing.Size(1050, 27);
            this.infoStrip2.TabIndex = 14;
            // 
            // lblTotalTime
            // 
            this.lblTotalTime.ForeColor = System.Drawing.Color.Blue;
            this.lblTotalTime.Image = global::FinalBeansStats.Properties.Resources.clock_icon;
            this.lblTotalTime.Margin = new System.Windows.Forms.Padding(10, 1, 20, 2);
            this.lblTotalTime.Name = "lblTotalTime";
            this.lblTotalTime.Size = new System.Drawing.Size(59, 16);
            this.lblTotalTime.Text = "0:00:00";
            this.lblTotalTime.Click += new System.EventHandler(this.lblTotalTime_Click);
            this.lblTotalTime.MouseEnter += new System.EventHandler(this.infoStrip_MouseEnter);
            this.lblTotalTime.MouseLeave += new System.EventHandler(this.infoStrip_MouseLeave);
            // 
            // lblGoldMedal
            // 
            this.lblGoldMedal.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblGoldMedal.Image = global::FinalBeansStats.Properties.Resources.medal_gold;
            this.lblGoldMedal.Margin = new System.Windows.Forms.Padding(10, 1, 5, 2);
            this.lblGoldMedal.Name = "lblGoldMedal";
            this.lblGoldMedal.Size = new System.Drawing.Size(29, 16);
            this.lblGoldMedal.Text = "0";
            // 
            // lblSilverMedal
            // 
            this.lblSilverMedal.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblSilverMedal.Image = global::FinalBeansStats.Properties.Resources.medal_silver;
            this.lblSilverMedal.Margin = new System.Windows.Forms.Padding(10, 1, 5, 2);
            this.lblSilverMedal.Name = "lblSilverMedal";
            this.lblSilverMedal.Size = new System.Drawing.Size(29, 16);
            this.lblSilverMedal.Text = "0";
            // 
            // lblBronzeMedal
            // 
            this.lblBronzeMedal.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblBronzeMedal.Image = global::FinalBeansStats.Properties.Resources.medal_bronze;
            this.lblBronzeMedal.Margin = new System.Windows.Forms.Padding(10, 1, 5, 2);
            this.lblBronzeMedal.Name = "lblBronzeMedal";
            this.lblBronzeMedal.Size = new System.Drawing.Size(29, 16);
            this.lblBronzeMedal.Text = "0";
            // 
            // lblPinkMedal
            // 
            this.lblPinkMedal.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblPinkMedal.Image = global::FinalBeansStats.Properties.Resources.medal_pink;
            this.lblPinkMedal.Margin = new System.Windows.Forms.Padding(10, 1, 5, 2);
            this.lblPinkMedal.Name = "lblPinkMedal";
            this.lblPinkMedal.Size = new System.Drawing.Size(29, 16);
            this.lblPinkMedal.Text = "0";
            // 
            // lblEliminatedMedal
            // 
            this.lblEliminatedMedal.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblEliminatedMedal.Image = global::FinalBeansStats.Properties.Resources.medal_eliminated;
            this.lblEliminatedMedal.Margin = new System.Windows.Forms.Padding(10, 1, 5, 2);
            this.lblEliminatedMedal.Name = "lblEliminatedMedal";
            this.lblEliminatedMedal.Size = new System.Drawing.Size(29, 16);
            this.lblEliminatedMedal.Text = "0";
            // 
            // lblKudos
            // 
            this.lblKudos.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblKudos.Image = global::FinalBeansStats.Properties.Resources.kudos_icon;
            this.lblKudos.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.lblKudos.Name = "lblKudos";
            this.lblKudos.Size = new System.Drawing.Size(29, 16);
            this.lblKudos.Text = "0";
            // 
            // infoStrip3
            // 
            this.infoStrip3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoStrip3.AutoSize = false;
            this.infoStrip3.BackColor = System.Drawing.Color.Transparent;
            this.infoStrip3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.infoStrip3.Dock = System.Windows.Forms.DockStyle.None;
            this.infoStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.infoStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblPlayerName});
            this.infoStrip3.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.infoStrip3.Location = new System.Drawing.Point(0, 147);
            this.infoStrip3.Name = "infoStrip3";
            this.infoStrip3.Padding = new System.Windows.Forms.Padding(14, 6, 14, 1);
            this.infoStrip3.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.infoStrip3.Size = new System.Drawing.Size(1050, 28);
            this.infoStrip3.TabIndex = 15;
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayCMenu;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "trayIcon";
            this.trayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseClick);
            this.trayIcon.MouseMove += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseMove);
            this.trayIcon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseUp);
            // 
            // trayCMenu
            // 
            this.trayCMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trayOverlay,
            this.traySeparator1,
            this.traySettings,
            this.traySeparator2,
            this.trayFilters,
            this.trayProfile,
            this.traySeparator3,
            this.trayUsefulThings,
            this.trayUpdate,
            this.trayHelp,
            this.trayLaunchFinalBeans,
            this.traySeparator4,
            this.trayExitProgram});
            this.trayCMenu.Name = "trayCMenu";
            this.trayCMenu.Size = new System.Drawing.Size(173, 226);
            this.trayCMenu.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.trayCMenu_Closing);
            this.trayCMenu.Opening += new System.ComponentModel.CancelEventHandler(this.trayCMenu_Opening);
            // 
            // trayOverlay
            // 
            this.trayOverlay.Image = global::FinalBeansStats.Properties.Resources.stat_gray_icon;
            this.trayOverlay.Name = "trayOverlay";
            this.trayOverlay.Size = new System.Drawing.Size(172, 22);
            this.trayOverlay.Text = "Show Overlay";
            this.trayOverlay.Click += new System.EventHandler(this.menuOverlay_Click);
            // 
            // traySeparator1
            // 
            this.traySeparator1.Name = "traySeparator1";
            this.traySeparator1.Size = new System.Drawing.Size(169, 6);
            // 
            // traySettings
            // 
            this.traySettings.Image = global::FinalBeansStats.Properties.Resources.setting_icon;
            this.traySettings.Name = "traySettings";
            this.traySettings.Size = new System.Drawing.Size(172, 22);
            this.traySettings.Text = "Settings";
            this.traySettings.Click += new System.EventHandler(this.menuSettings_Click);
            // 
            // traySeparator2
            // 
            this.traySeparator2.Name = "traySeparator2";
            this.traySeparator2.Size = new System.Drawing.Size(169, 6);
            // 
            // trayFilters
            // 
            this.trayFilters.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trayStatsFilter,
            this.trayPartyFilter});
            this.trayFilters.Image = global::FinalBeansStats.Properties.Resources.filter_icon;
            this.trayFilters.Name = "trayFilters";
            this.trayFilters.Size = new System.Drawing.Size(172, 22);
            this.trayFilters.Text = "Filters";
            // 
            // trayStatsFilter
            // 
            this.trayStatsFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trayCustomRangeStats,
            this.traySubSeparator1,
            this.trayAllStats,
            this.traySeasonStats,
            this.trayWeekStats,
            this.trayDayStats,
            this.traySessionStats});
            this.trayStatsFilter.Image = global::FinalBeansStats.Properties.Resources.stat_icon;
            this.trayStatsFilter.Name = "trayStatsFilter";
            this.trayStatsFilter.Size = new System.Drawing.Size(101, 22);
            this.trayStatsFilter.Text = "Stats";
            // 
            // trayCustomRangeStats
            // 
            this.trayCustomRangeStats.CheckOnClick = true;
            this.trayCustomRangeStats.Image = global::FinalBeansStats.Properties.Resources.calendar_icon;
            this.trayCustomRangeStats.Name = "trayCustomRangeStats";
            this.trayCustomRangeStats.Size = new System.Drawing.Size(152, 22);
            this.trayCustomRangeStats.Text = "Custom Range";
            this.trayCustomRangeStats.Click += new System.EventHandler(this.menuStats_Click);
            // 
            // traySubSeparator1
            // 
            this.traySubSeparator1.Name = "traySubSeparator1";
            this.traySubSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // trayAllStats
            // 
            this.trayAllStats.Checked = true;
            this.trayAllStats.CheckOnClick = true;
            this.trayAllStats.CheckState = System.Windows.Forms.CheckState.Checked;
            this.trayAllStats.Name = "trayAllStats";
            this.trayAllStats.Size = new System.Drawing.Size(152, 22);
            this.trayAllStats.Text = "All";
            this.trayAllStats.Click += new System.EventHandler(this.menuStats_Click);
            // 
            // traySeasonStats
            // 
            this.traySeasonStats.CheckOnClick = true;
            this.traySeasonStats.Name = "traySeasonStats";
            this.traySeasonStats.Size = new System.Drawing.Size(152, 22);
            this.traySeasonStats.Text = "Season";
            this.traySeasonStats.Click += new System.EventHandler(this.menuStats_Click);
            // 
            // trayWeekStats
            // 
            this.trayWeekStats.CheckOnClick = true;
            this.trayWeekStats.Name = "trayWeekStats";
            this.trayWeekStats.Size = new System.Drawing.Size(152, 22);
            this.trayWeekStats.Text = "Week";
            this.trayWeekStats.Click += new System.EventHandler(this.menuStats_Click);
            // 
            // trayDayStats
            // 
            this.trayDayStats.CheckOnClick = true;
            this.trayDayStats.Name = "trayDayStats";
            this.trayDayStats.Size = new System.Drawing.Size(152, 22);
            this.trayDayStats.Text = "Day";
            this.trayDayStats.Click += new System.EventHandler(this.menuStats_Click);
            // 
            // traySessionStats
            // 
            this.traySessionStats.CheckOnClick = true;
            this.traySessionStats.Name = "traySessionStats";
            this.traySessionStats.Size = new System.Drawing.Size(152, 22);
            this.traySessionStats.Text = "Session";
            this.traySessionStats.Click += new System.EventHandler(this.menuStats_Click);
            // 
            // trayPartyFilter
            // 
            this.trayPartyFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trayAllPartyStats,
            this.traySoloStats,
            this.trayPartyStats});
            this.trayPartyFilter.Image = global::FinalBeansStats.Properties.Resources.player_icon;
            this.trayPartyFilter.Name = "trayPartyFilter";
            this.trayPartyFilter.Size = new System.Drawing.Size(101, 22);
            this.trayPartyFilter.Text = "Party";
            // 
            // trayAllPartyStats
            // 
            this.trayAllPartyStats.Checked = true;
            this.trayAllPartyStats.CheckOnClick = true;
            this.trayAllPartyStats.CheckState = System.Windows.Forms.CheckState.Checked;
            this.trayAllPartyStats.Name = "trayAllPartyStats";
            this.trayAllPartyStats.Size = new System.Drawing.Size(101, 22);
            this.trayAllPartyStats.Text = "All";
            this.trayAllPartyStats.Click += new System.EventHandler(this.menuStats_Click);
            // 
            // traySoloStats
            // 
            this.traySoloStats.CheckOnClick = true;
            this.traySoloStats.Name = "traySoloStats";
            this.traySoloStats.Size = new System.Drawing.Size(101, 22);
            this.traySoloStats.Text = "Solo";
            this.traySoloStats.Click += new System.EventHandler(this.menuStats_Click);
            // 
            // trayPartyStats
            // 
            this.trayPartyStats.CheckOnClick = true;
            this.trayPartyStats.Name = "trayPartyStats";
            this.trayPartyStats.Size = new System.Drawing.Size(101, 22);
            this.trayPartyStats.Text = "Party";
            this.trayPartyStats.Click += new System.EventHandler(this.menuStats_Click);
            // 
            // trayProfile
            // 
            this.trayProfile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trayEditProfiles,
            this.traySubSeparator2});
            this.trayProfile.Image = global::FinalBeansStats.Properties.Resources.profile_icon;
            this.trayProfile.Name = "trayProfile";
            this.trayProfile.Size = new System.Drawing.Size(172, 22);
            this.trayProfile.Text = "Profile";
            // 
            // trayEditProfiles
            // 
            this.trayEditProfiles.Image = global::FinalBeansStats.Properties.Resources.setting_icon;
            this.trayEditProfiles.Name = "trayEditProfiles";
            this.trayEditProfiles.Size = new System.Drawing.Size(153, 22);
            this.trayEditProfiles.Text = "Profile Settings";
            this.trayEditProfiles.Click += new System.EventHandler(this.menuEditProfiles_Click);
            // 
            // traySubSeparator2
            // 
            this.traySubSeparator2.Name = "traySubSeparator2";
            this.traySubSeparator2.Size = new System.Drawing.Size(150, 6);
            // 
            // traySeparator3
            // 
            this.traySeparator3.Name = "traySeparator3";
            this.traySeparator3.Size = new System.Drawing.Size(169, 6);
            // 
            // trayUsefulThings
            // 
            this.trayUsefulThings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trayFinalBeans,
            this.traySubSeparator6,
            this.trayRollOffClub,
            this.traySubSeparator7,
            this.trayLostTempleAnalyzer});
            this.trayUsefulThings.Image = global::FinalBeansStats.Properties.Resources.main_icon;
            this.trayUsefulThings.Name = "trayUsefulThings";
            this.trayUsefulThings.Size = new System.Drawing.Size(172, 22);
            this.trayUsefulThings.Text = "Useful things!";
            // 
            // trayFinalBeans
            // 
            this.trayFinalBeans.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trayFinalBeansOfficial,
            this.traySubSeparator3,
            this.trayFinalBeansDiscord,
            this.traySubSeparator4,
            this.trayFinalBeansTwitterX,
            this.traySubSeparator5,
            this.trayFinalBeansTrello});
            this.trayFinalBeans.Image = global::FinalBeansStats.Properties.Resources.finalbeans_icon;
            this.trayFinalBeans.Name = "trayFinalBeans";
            this.trayFinalBeans.Size = new System.Drawing.Size(186, 22);
            this.trayFinalBeans.Text = "FinalBeans";
            // 
            // trayFinalBeansOfficial
            // 
            this.trayFinalBeansOfficial.Image = global::FinalBeansStats.Properties.Resources.fb_website_icon;
            this.trayFinalBeansOfficial.Name = "trayFinalBeansOfficial";
            this.trayFinalBeansOfficial.Size = new System.Drawing.Size(183, 22);
            this.trayFinalBeansOfficial.Text = "Website";
            this.trayFinalBeansOfficial.Click += new System.EventHandler(this.menuUsefulThings_Click);
            // 
            // traySubSeparator3
            // 
            this.traySubSeparator3.Name = "traySubSeparator3";
            this.traySubSeparator3.Size = new System.Drawing.Size(180, 6);
            // 
            // trayFinalBeansDiscord
            // 
            this.trayFinalBeansDiscord.Image = global::FinalBeansStats.Properties.Resources.discord_logo;
            this.trayFinalBeansDiscord.Name = "trayFinalBeansDiscord";
            this.trayFinalBeansDiscord.Size = new System.Drawing.Size(183, 22);
            this.trayFinalBeansDiscord.Text = "Discord";
            this.trayFinalBeansDiscord.Click += new System.EventHandler(this.menuUsefulThings_Click);
            // 
            // traySubSeparator4
            // 
            this.traySubSeparator4.Name = "traySubSeparator4";
            this.traySubSeparator4.Size = new System.Drawing.Size(180, 6);
            // 
            // trayFinalBeansTwitterX
            // 
            this.trayFinalBeansTwitterX.Image = global::FinalBeansStats.Properties.Resources.x_icon;
            this.trayFinalBeansTwitterX.Name = "trayFinalBeansTwitterX";
            this.trayFinalBeansTwitterX.Size = new System.Drawing.Size(183, 22);
            this.trayFinalBeansTwitterX.Text = "Twitter/X";
            this.trayFinalBeansTwitterX.Click += new System.EventHandler(this.menuUsefulThings_Click);
            // 
            // traySubSeparator5
            // 
            this.traySubSeparator5.Name = "traySubSeparator5";
            this.traySubSeparator5.Size = new System.Drawing.Size(180, 6);
            // 
            // trayFinalBeansTrello
            // 
            this.trayFinalBeansTrello.Image = global::FinalBeansStats.Properties.Resources.trello_logo;
            this.trayFinalBeansTrello.Name = "trayFinalBeansTrello";
            this.trayFinalBeansTrello.Size = new System.Drawing.Size(183, 22);
            this.trayFinalBeansTrello.Text = "Trello (Bug Tracking)";
            this.trayFinalBeansTrello.Click += new System.EventHandler(this.menuUsefulThings_Click);
            // 
            // traySubSeparator6
            // 
            this.traySubSeparator6.Name = "traySubSeparator6";
            this.traySubSeparator6.Size = new System.Drawing.Size(183, 6);
            // 
            // trayRollOffClub
            // 
            this.trayRollOffClub.Image = global::FinalBeansStats.Properties.Resources.roll_off_club_icon;
            this.trayRollOffClub.Name = "trayRollOffClub";
            this.trayRollOffClub.Size = new System.Drawing.Size(186, 22);
            this.trayRollOffClub.Text = "Roll Off Club";
            this.trayRollOffClub.Click += new System.EventHandler(this.menuUsefulThings_Click);
            // 
            // traySubSeparator7
            // 
            this.traySubSeparator7.Name = "traySubSeparator7";
            this.traySubSeparator7.Size = new System.Drawing.Size(183, 6);
            // 
            // trayLostTempleAnalyzer
            // 
            this.trayLostTempleAnalyzer.Image = global::FinalBeansStats.Properties.Resources.lost_temple_analyzer_icon;
            this.trayLostTempleAnalyzer.Name = "trayLostTempleAnalyzer";
            this.trayLostTempleAnalyzer.Size = new System.Drawing.Size(186, 22);
            this.trayLostTempleAnalyzer.Text = "Lost Temple Analyzer";
            this.trayLostTempleAnalyzer.Click += new System.EventHandler(this.menuUsefulThings_Click);
            // 
            // trayUpdate
            // 
            this.trayUpdate.Image = global::FinalBeansStats.Properties.Resources.github_icon;
            this.trayUpdate.Name = "trayUpdate";
            this.trayUpdate.Size = new System.Drawing.Size(172, 22);
            this.trayUpdate.Text = "Update";
            this.trayUpdate.Click += new System.EventHandler(this.menuUpdate_Click);
            // 
            // trayHelp
            // 
            this.trayHelp.Image = global::FinalBeansStats.Properties.Resources.github_icon;
            this.trayHelp.Name = "trayHelp";
            this.trayHelp.Size = new System.Drawing.Size(172, 22);
            this.trayHelp.Text = "Help";
            this.trayHelp.Click += new System.EventHandler(this.menuHelp_Click);
            // 
            // trayLaunchFinalBeans
            // 
            this.trayLaunchFinalBeans.Image = global::FinalBeansStats.Properties.Resources.finalbeans_icon;
            this.trayLaunchFinalBeans.Name = "trayLaunchFinalBeans";
            this.trayLaunchFinalBeans.Size = new System.Drawing.Size(172, 22);
            this.trayLaunchFinalBeans.Text = "Launch FinalBeans";
            this.trayLaunchFinalBeans.Click += new System.EventHandler(this.menuLaunchFinalBeans_Click);
            // 
            // traySeparator4
            // 
            this.traySeparator4.Name = "traySeparator4";
            this.traySeparator4.Size = new System.Drawing.Size(169, 6);
            // 
            // trayExitProgram
            // 
            this.trayExitProgram.Image = global::FinalBeansStats.Properties.Resources.shutdown_icon;
            this.trayExitProgram.Name = "trayExitProgram";
            this.trayExitProgram.Size = new System.Drawing.Size(172, 22);
            this.trayExitProgram.Text = "Exit";
            this.trayExitProgram.Click += new System.EventHandler(this.Stats_ExitProgram);
            // 
            // gridDetails
            // 
            this.gridDetails.AllowUserToDeleteRows = false;
            this.gridDetails.AllowUserToOrderColumns = false;
            this.gridDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridDetails.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gridDetails.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Cyan;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridDetails.DefaultCellStyle = dataGridViewCellStyle2;
            this.gridDetails.EnableHeadersVisualStyles = false;
            this.gridDetails.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gridDetails.GridColor = System.Drawing.Color.Gray;
            this.gridDetails.Location = new System.Drawing.Point(15, 200);
            this.gridDetails.Margin = new System.Windows.Forms.Padding(0);
            this.gridDetails.Name = "gridDetails";
            this.gridDetails.ReadOnly = true;
            this.gridDetails.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDetails.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridDetails.RowHeadersVisible = false;
            this.gridDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetails.Size = new System.Drawing.Size(912, 448);
            this.gridDetails.TabIndex = 11;
            this.gridDetails.TabStop = false;
            this.gridDetails.DataSourceChanged += new System.EventHandler(this.gridDetails_DataSourceChanged);
            this.gridDetails.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridDetails_CellClick);
            this.gridDetails.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridDetails_CellFormatting);
            this.gridDetails.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridDetails_CellMouseEnter);
            this.gridDetails.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridDetails_CellMouseLeave);
            this.gridDetails.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridDetails_ColumnHeaderMouseClick);
            this.gridDetails.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.gridDetails_DataBindingComplete);
            this.gridDetails.Scroll += new System.Windows.Forms.ScrollEventHandler(this.gridDetails_Scroll);
            this.gridDetails.SelectionChanged += new System.EventHandler(this.gridDetails_SelectionChanged);
            // 
            // mlReportBug
            // 
            this.mlReportBug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mlReportBug.AutoSize = true;
            this.mlReportBug.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mlReportBug.FontSize = MetroFramework.MetroLinkSize.Medium;
            this.mlReportBug.Image = global::FinalBeansStats.Properties.Resources.report_icon;
            this.mlReportBug.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.mlReportBug.Location = new System.Drawing.Point(909, 68);
            this.mlReportBug.Margin = new System.Windows.Forms.Padding(0, 0, 60, 0);
            this.mlReportBug.Name = "mlReportBug";
            this.mlReportBug.Size = new System.Drawing.Size(18, 23);
            this.mlReportBug.TabIndex = 7;
            this.mlReportBug.UseSelectable = true;
            this.mlReportBug.UseStyleColors = true;
            this.mlReportBug.Click += new System.EventHandler(this.mlReportBug_Click);
            this.mlReportBug.MouseEnter += new System.EventHandler(this.mlReportBug_MouseEnter);
            this.mlReportBug.MouseLeave += new System.EventHandler(this.mlReportBug_MouseLeave);
            // 
            // Stats
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(942, 650);
            this.Controls.Add(this.mlReportBug);
            this.Controls.Add(this.menu);
            this.Controls.Add(this.infoStrip);
            this.Controls.Add(this.infoStrip2);
            this.Controls.Add(this.infoStrip3);
            this.Controls.Add(this.mtgIgnoreLevelTypeWhenSorting);
            this.Controls.Add(this.lblIgnoreLevelTypeWhenSorting);
            this.Controls.Add(this.gridDetails);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = global::FinalBeansStats.Properties.Resources.fbstats;
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(15, 15);
            this.MinimumSize = new System.Drawing.Size(720, 350);
            this.Name = "Stats";
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.AeroShadow;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Style = MetroFramework.MetroColorStyle.Teal;
            this.Text = "FinalBeans Stats";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Stats_FormClosing);
            this.Load += new System.EventHandler(this.Stats_Load);
            this.Shown += new System.EventHandler(this.Stats_Shown);
            this.VisibleChanged += new System.EventHandler(this.Stats_VisibleChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Stats_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Stats_KeyUp);
            this.Resize += new System.EventHandler(this.Stats_Resize);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.infoStrip.ResumeLayout(false);
            this.infoStrip.PerformLayout();
            this.infoStrip2.ResumeLayout(false);
            this.infoStrip2.PerformLayout();
            this.infoStrip3.ResumeLayout(false);
            this.infoStrip3.PerformLayout();
            this.trayCMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetails)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.NotifyIcon trayIcon;
        private MetroFramework.Controls.MetroToggle mtgIgnoreLevelTypeWhenSorting;
        private System.Windows.Forms.Label lblIgnoreLevelTypeWhenSorting;
        private FinalBeansStats.Grid gridDetails;
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem menuSettings;
        private System.Windows.Forms.ToolStripMenuItem menuFilters;
        private System.Windows.Forms.ToolStripMenuItem menuStatsFilter;
        private System.Windows.Forms.ToolStripMenuItem menuAllStats;
        private System.Windows.Forms.ToolStripMenuItem menuSeasonStats;
        private System.Windows.Forms.ToolStripMenuItem menuWeekStats;
        private System.Windows.Forms.ToolStripMenuItem menuDayStats;
        private System.Windows.Forms.ToolStripMenuItem menuSessionStats;
        private System.Windows.Forms.ToolStripMenuItem menuPartyFilter;
        private System.Windows.Forms.ToolStripMenuItem menuAllPartyStats;
        private System.Windows.Forms.ToolStripMenuItem menuSoloStats;
        private System.Windows.Forms.ToolStripMenuItem menuPartyStats;
        private System.Windows.Forms.ToolStripMenuItem menuOverlay;
        private System.Windows.Forms.ToolStripMenuItem menuUpdate;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuProfile;
        private System.Windows.Forms.ToolStripMenuItem menuEditProfiles;
        private System.Windows.Forms.ToolStripSeparator menuSeparator2;
        private System.Windows.Forms.ToolStrip infoStrip;
        private System.Windows.Forms.ToolStrip infoStrip2;
        private System.Windows.Forms.ToolStrip infoStrip3;
        private System.Windows.Forms.ToolStripLabel lblCurrentProfileIcon;
        private System.Windows.Forms.ToolStripLabel lblCurrentProfile;
        private System.Windows.Forms.ToolStripLabel lblPlayerName;
        private System.Windows.Forms.ToolStripLabel lblTotalTime;
        private System.Windows.Forms.ToolStripLabel lblTotalShows;
        private System.Windows.Forms.ToolStripLabel lblTotalRounds;
        private System.Windows.Forms.ToolStripLabel lblTotalWins;
        private System.Windows.Forms.ToolStripLabel lblTotalFinals;
        private System.Windows.Forms.ToolStripLabel lblGoldMedal;
        private System.Windows.Forms.ToolStripLabel lblSilverMedal;
        private System.Windows.Forms.ToolStripLabel lblBronzeMedal;
        private System.Windows.Forms.ToolStripLabel lblPinkMedal;
        private System.Windows.Forms.ToolStripLabel lblEliminatedMedal;
        private System.Windows.Forms.ToolStripLabel lblKudos;
        private System.Windows.Forms.ToolStripMenuItem menuLaunchFinalBeans;
        private System.Windows.Forms.ToolStripMenuItem menuUsefulThings;
        private System.Windows.Forms.ToolStripSeparator menuSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuRollOffClub;
        private System.Windows.Forms.ToolStripMenuItem menuLostTempleAnalyzer;
        private System.Windows.Forms.ToolStripSeparator menuSeparator7;
        private System.Windows.Forms.ToolStripMenuItem menuFinalBeansDBMain;
        private System.Windows.Forms.ToolStripSeparator menuSeparator9;
        private System.Windows.Forms.ToolStripMenuItem menuFinalBeansDBShows;
        private System.Windows.Forms.ToolStripSeparator menuSeparator10;
        private System.Windows.Forms.ToolStripMenuItem menuFinalBeansDBDiscovery;
        private System.Windows.Forms.ToolStripSeparator menuSeparator11;
        private System.Windows.Forms.ToolStripMenuItem menuFinalBeansDBShop;
        private System.Windows.Forms.ToolStripSeparator menuSeparator12;
        private System.Windows.Forms.ToolStripMenuItem menuFinalBeansDBNewsfeeds;
        private System.Windows.Forms.ToolStripSeparator menuSeparator13;
        private System.Windows.Forms.ToolStripMenuItem menuFinalBeansDBStrings;
        private System.Windows.Forms.ToolStripSeparator menuSeparator14;
        private System.Windows.Forms.ToolStripMenuItem menuFinalBeansDBCosmetics;
        private System.Windows.Forms.ToolStripSeparator menuSeparator15;
        private System.Windows.Forms.ToolStripMenuItem menuFinalBeansDBCrownRanks;
        private System.Windows.Forms.ToolStripSeparator menuSeparator16;
        private System.Windows.Forms.ToolStripMenuItem menuFinalBeansDBLiveEvents;
        private System.Windows.Forms.ToolStripSeparator menuSeparator17;
        private System.Windows.Forms.ToolStripMenuItem menuFinalBeansDBDailyShop;
        private System.Windows.Forms.ToolStripSeparator menuSeparator18;
        private System.Windows.Forms.ToolStripMenuItem menuFinalBeansDBCreative;
        private System.Windows.Forms.ToolStripSeparator menuSeparator19;
        private System.Windows.Forms.ToolStripSeparator menuSeparator8;
        
        private System.Windows.Forms.ContextMenuStrip trayCMenu;
        private System.Windows.Forms.ToolStripMenuItem trayAllPartyStats;
        private System.Windows.Forms.ToolStripMenuItem traySoloStats;
        private System.Windows.Forms.ToolStripMenuItem trayPartyStats;
        private System.Windows.Forms.ToolStripMenuItem trayDayStats;
        private System.Windows.Forms.ToolStripMenuItem traySessionStats;
        private System.Windows.Forms.ToolStripMenuItem trayCustomRangeStats;
        private System.Windows.Forms.ToolStripSeparator traySubSeparator1;
        private System.Windows.Forms.ToolStripMenuItem trayAllStats;
        private System.Windows.Forms.ToolStripMenuItem traySeasonStats;
        private System.Windows.Forms.ToolStripMenuItem trayWeekStats;
        private System.Windows.Forms.ToolStripMenuItem trayPartyFilter;
        private System.Windows.Forms.ToolStripMenuItem trayStatsFilter;
        private System.Windows.Forms.ToolStripSeparator traySeparator3;
        private System.Windows.Forms.ToolStripMenuItem trayUsefulThings;
        private System.Windows.Forms.ToolStripMenuItem trayFinalBeans;
        private System.Windows.Forms.ToolStripMenuItem trayFinalBeansDiscord;
        private System.Windows.Forms.ToolStripSeparator traySubSeparator3;
        private System.Windows.Forms.ToolStripMenuItem trayFinalBeansOfficial;
        private System.Windows.Forms.ToolStripMenuItem trayFinalBeansTwitterX;
        private System.Windows.Forms.ToolStripSeparator traySubSeparator4;
        private System.Windows.Forms.ToolStripMenuItem trayFinalBeansTrello;
        private System.Windows.Forms.ToolStripSeparator traySubSeparator5;
        private System.Windows.Forms.ToolStripMenuItem trayRollOffClub;
        private System.Windows.Forms.ToolStripSeparator traySubSeparator6;
        private System.Windows.Forms.ToolStripMenuItem trayLostTempleAnalyzer;
        private System.Windows.Forms.ToolStripSeparator traySubSeparator7;
        private System.Windows.Forms.ToolStripMenuItem trayUpdate;
        private System.Windows.Forms.ToolStripMenuItem trayLaunchFinalBeans;
        private System.Windows.Forms.ToolStripMenuItem trayHelp;
        private System.Windows.Forms.ToolStripSeparator traySeparator4;
        private System.Windows.Forms.ToolStripMenuItem trayExitProgram;
        private System.Windows.Forms.ToolStripMenuItem traySettings;
        private System.Windows.Forms.ToolStripSeparator traySeparator2;
        private System.Windows.Forms.ToolStripMenuItem trayFilters;
        private System.Windows.Forms.ToolStripSeparator traySeparator1;
        private System.Windows.Forms.ToolStripMenuItem trayProfile;
        private System.Windows.Forms.ToolStripSeparator traySubSeparator2;
        private System.Windows.Forms.ToolStripMenuItem trayEditProfiles;
        private System.Windows.Forms.ToolStripMenuItem trayOverlay;
        private System.Windows.Forms.ToolStripMenuItem menuCustomRangeStats;
        private System.Windows.Forms.ToolStripSeparator menuSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuFinalBeans;
        private System.Windows.Forms.ToolStripMenuItem menuFinalBeansOfficial;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuFinalBeansDiscord;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuFinalBeansTwitterX;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuFinalBeansTrello;
        private MetroFramework.Controls.MetroLink mlReportBug;
    }
}

