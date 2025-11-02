using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiteDB;
using MetroFramework;
using MetroFramework.Components;
using MetroFramework.Controls;
using Trinet.Core.IO.Ntfs;

namespace FinalBeansStats {
    public partial class Stats : MetroFramework.Forms.MetroForm {
        [STAThread]
        private static void Main() {
            try {
                bool isAppUpdated = false;
#if AllowUpdate
                if (File.Exists($"{CURRENTDIR}{Path.GetFileName(Assembly.GetEntryAssembly().Location)}.bak")) {
                    isAppUpdated = true;
                }
#endif
                if (isAppUpdated || !IsAlreadyRunning()) {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    SplashForm splashWindow = new SplashForm();
                    Screen primaryScreen = Screen.PrimaryScreen;
                    Rectangle workingArea = primaryScreen.WorkingArea;
                    int x = workingArea.Left + (workingArea.Width - splashWindow.Width) / 2;
                    int y = workingArea.Top + (workingArea.Height - splashWindow.Height) / 2;
                    splashWindow.Location = new Point(x, y);
                    splashWindow.Show();
                    splashWindow.Refresh();
                    splashWindow.TopMost = true;
                    splashWindow.TopMost = false;
                    Application.DoEvents();

                    Stats statsForm = new Stats();
                    splashWindow.Close();
                    Application.Run(statsForm);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString(), @"Run Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetEventWaitHandle() {
            EventWaitHandle eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "FinalBeansStatsEventWaitHandle", out bool createdNew);
            if (!createdNew) {
                Application.Exit();
            } else {
                Task.Run(() => {
                    while (eventWaitHandle.WaitOne()) {
                        this.Invoke((Action)(() => {
                            this.Visible = true;
                            this.TopMost = true;
                            this.TopMost = false;
                        }));
                    }
                });
            }
        }

        private static bool IsAlreadyRunning() {
            try {
                string currentProcessName = Process.GetCurrentProcess().ProcessName;
                foreach (var process in Process.GetProcessesByName(currentProcessName)) {
                    if (process.Id != Process.GetCurrentProcess().Id) {
                        EventWaitHandle eventWaitHandle = EventWaitHandle.OpenExisting("FinalBeansStatsEventWaitHandle");
                        eventWaitHandle.Set();
                        // Utils.ShowWindow(process.MainWindowHandle, 9);
                        // Utils.SetForegroundWindow(process.MainWindowHandle);
                        return true;
                    }
                }
                return false;
            } catch (Exception ex) {
                MessageBox.Show(ex.ToString(), @"Process Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }

        public static bool IsExitingProgram;
#if AllowUpdate
        public static bool IsExitingForUpdate;
        public static bool IsUpdatingOnAppLaunch;
#endif

        public static readonly string CURRENTDIR = AppDomain.CurrentDomain.BaseDirectory;

        internal static readonly string LOGPATH = Path.Combine($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}Low", "matti", "FinalBeans");
        private static readonly string LOGFILENAME = "Player.log";

        public static readonly int CURRENTSEASON = 2;
        public static readonly (string Name, DateTime StartDate)[] Seasons = new (string Name, DateTime StartDate)[] {
            ("FS1-1.0.X", new DateTime(2025, 8, 14, 0, 0, 0, DateTimeKind.Utc)),     // Season 1
            ("FS2-2.0.0", new DateTime(2025, 10, 23, 0, 0, 0, DateTimeKind.Utc)),    // Season 2 (Frightful Final-Ween) { Broken Physics }
            ("FS2-2.0.1", new DateTime(2025, 10, 26, 0, 0, 0, DateTimeKind.Utc)),    // Season 2 (Frightful Final-Ween) { Fixed Physics }
        };
        private static DateTime SeasonStart, WeekStart, DayStart;
        private static DateTime SessionStart = DateTime.UtcNow;
        public static Language CurrentLanguage;
        public static MetroThemeStyle CurrentTheme = MetroThemeStyle.Light;
        public static bool InstalledEmojiFont;

        public static bool IsSpectating = false;
        public static bool InShow = false;
        public static bool EndedShow = false;

        public static bool IsDisplayOverlayTime = true;
        public static bool IsDisplayOverlayPing = true;
        public static bool IsOverlayRoundInfoNeedRefresh;

        public static bool IsGameRunning = false;
        public static bool IsClientHasBeenClosed = false;

        public static bool IsConnectedToServer = false;
        public static DateTime ConnectedToServerDate = DateTime.MinValue;
        public static string LastServerIp = string.Empty;
        public static string LastCountryAlpha2Code = string.Empty;
        public static string LastCountryRegion = string.Empty;
        public static string LastCountryCity = string.Empty;
        public static long LastServerPing = 0;
        public static bool IsBadServerPing = false;

        public static readonly List<string> SucceededPlayerNames = new List<string>();
        public static readonly List<int> SucceededPlayerIds = new List<int>();
        public static readonly List<int> EliminatedPlayerIds = new List<int>();
        public static readonly Dictionary<int, string> ReadyPlayerIds = new Dictionary<int, string>();

        public static string SavedSessionId { get; set; }
        public static int SavedRoundCount { get; set; }
        public static int NumPlayersSucceeded { get; set; }
        public static int NumPlayersPsSucceeded { get; set; }
        public static int NumPlayersXbSucceeded { get; set; }
        public static int NumPlayersSwSucceeded { get; set; }
        public static int NumPlayersMbSucceeded { get; set; }
        public static int NumPlayersPcSucceeded { get; set; }
        public static int NumPlayersEliminated { get; set; }
        public static int NumPlayersPsEliminated { get; set; }
        public static int NumPlayersXbEliminated { get; set; }
        public static int NumPlayersSwEliminated { get; set; }
        public static int NumPlayersMbEliminated { get; set; }
        public static int NumPlayersPcEliminated { get; set; }
        public static bool IsLastRoundRunning { get; set; }
        public static bool IsLastPlayedRoundStillPlaying { get; set; }
        public static DateTime LastGameStart { get; set; } = DateTime.MinValue;
        public static DateTime LastRoundLoad { get; set; } = DateTime.MinValue;
        public static DateTime? LastPlayedRoundStart { get; set; }
        public static DateTime? LastPlayedRoundEnd { get; set; }
        public static DateTime LastPlayersCount { get; set; } = DateTime.MinValue;

        public static bool IsQueuing { get; set; }
        public static int QueuedPlayers { get; set; }

        public static bool UseWebProxy;
        public static string ProxyAddress = string.Empty;
        public static string ProxyPort = string.Empty;
        public static bool EnableProxyAuthentication;
        public static string ProxyUsername = string.Empty;
        public static string ProxyPassword = string.Empty;
        public static bool SucceededTestProxy;

        public static int IpGeolocationService;
        public static string IPinfoToken;
        public static readonly string IPinfoTokenFileName = "IPinfo.io.txt";

        readonly DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        readonly DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        public List<RoundInfo> AllStats = new List<RoundInfo>();
        public List<RoundInfo> CurrentRound = new List<RoundInfo>();
        public Dictionary<string, LevelStats> StatLookup;
        public List<LevelStats> StatDetails;
        private readonly LogFileWatcher logFile = new LogFileWatcher();
        private int Shows, Rounds, CustomShows, CustomRounds;
        private TimeSpan Duration;
        private int Wins, Finals, Kudos;
        private int GoldMedals, SilverMedals, BronzeMedals, PinkMedals, EliminatedMedals;
        private int CustomGoldMedals, CustomSilverMedals, CustomBronzeMedals, CustomPinkMedals, CustomEliminatedMedals;
        private int nextShowID;
        private bool loadingExisting;
        private bool updateFilterType, updateFilterRange;
        private DateTime customfilterRangeStart = DateTime.MinValue;
        private DateTime customfilterRangeEnd = DateTime.MaxValue;
        private int selectedCustomTemplateSeason = -1;
        private bool updateSelectedProfile, useLinkedProfiles;
        public LiteDatabase StatsDB;
        public ILiteCollection<RoundInfo> RoundDetails;
        public ILiteCollection<UserSettings> UserSettings;
        public ILiteCollection<Profiles> Profiles;
        public ILiteCollection<ServerConnectionLog> ServerConnectionLog;
        public ILiteCollection<PersonalBestLog> PersonalBestLog;
        public ILiteCollection<LevelTimeLimit> LevelTimeLimit;
        public List<Profiles> AllProfiles = new List<Profiles>();
        public UserSettings CurrentSettings;
        public List<ServerConnectionLog> ServerConnectionLogCache = new List<ServerConnectionLog>();
        public List<PersonalBestLog> PersonalBestLogCache = new List<PersonalBestLog>();
        public List<LevelTimeLimit> LevelTimeLimitCache = new List<LevelTimeLimit>();
        public readonly Overlay overlay;
        private DateTime lastAddedShow = DateTime.MinValue;
        private readonly DateTime startupTime = DateTime.UtcNow;
        private readonly int randSecond = new Random().Next(0, 30);
        private int askedPreviousShows;
        private readonly TextInfo textInfo;
        private int currentProfile, currentLanguage;
        private Color infoStripForeColor;
        public List<ToolStripMenuItem> ProfileMenuItems = new List<ToolStripMenuItem>();
        public List<ToolStripMenuItem> ProfileTrayItems = new List<ToolStripMenuItem>();

        private readonly Image numberOne = Utils.ImageOpacity(Properties.Resources.number_1, 0.5F);
        private readonly Image numberTwo = Utils.ImageOpacity(Properties.Resources.number_2, 0.5F);
        private readonly Image numberThree = Utils.ImageOpacity(Properties.Resources.number_3, 0.5F);
        private readonly Image numberFour = Utils.ImageOpacity(Properties.Resources.number_4, 0.5F);
        private readonly Image numberFive = Utils.ImageOpacity(Properties.Resources.number_5, 0.5F);
        private readonly Image numberSix = Utils.ImageOpacity(Properties.Resources.number_6, 0.5F);
        private readonly Image numberSeven = Utils.ImageOpacity(Properties.Resources.number_7, 0.5F);
        private readonly Image numberEight = Utils.ImageOpacity(Properties.Resources.number_8, 0.5F);
        private readonly Image numberNine = Utils.ImageOpacity(Properties.Resources.number_9, 0.5F);

        private bool maximizedForm;
        private bool isFocused;
        private bool shiftKeyToggle, ctrlKeyToggle;
        private MetroToolTip mtt = new MetroToolTip();
        private MetroToolTip cmtt = new MetroToolTip();
        private MetroToolTip omtt = new MetroToolTip();
        private DWM_WINDOW_CORNER_PREFERENCE windowConerPreference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUNDSMALL;
        private string mainWndTitle;
        private bool isStartingUp = true;
        private bool isAvailableNewVersion;
        private string availableNewVersion;
        private int profileWithLinkedCustomShow = -1;
        private Toast toast;
        public Point screenCenter;

        public event Action OnUpdatedLevelRows;
        private readonly System.Windows.Forms.Timer scrollTimer = new System.Windows.Forms.Timer { Interval = 100 };
        private bool isScrollingStopped = true;

        private readonly object dbTaskLock = new object();
        public List<Task> dbTasks = new List<Task>();

        private readonly int currentDbVersion = 0;

        public readonly string[] PublicShowIdList = {
            "fb_main_show",
            "fb_ltm"
        };

        public readonly string[] PublicShowIdList2 = {
            "fb_frightful_final_ween"
            // "fb_skilled_speeders"
        };

        public void RunDatabaseTask(Task task, bool runAsync) {
            lock (this.dbTaskLock) {
                if (IsExitingProgram) return;
                this.dbTasks.Add(task);
                task.Start();
                if (!runAsync) task.Wait();
            }
            task.ContinueWith(t => {
                lock (this.dbTaskLock) {
                    if (IsExitingProgram) return;
                    this.dbTasks.Remove(t);
                }
            });
        }

        private void DatabaseMigration() {
            Task migrationTask = new Task(() => {
                if (File.Exists($"{CURRENTDIR}data_new.db")) {
                    File.SetAttributes($"{CURRENTDIR}data_new.db", FileAttributes.Normal);
                    File.Delete($"{CURRENTDIR}data_new.db");
                }

                if (File.Exists($"{CURRENTDIR}data.db")) {
                    int sourceDbUserVersion = 0;
                    using (var sourceDb = new LiteDatabase($@"{CURRENTDIR}data.db")) {
                        sourceDbUserVersion = sourceDb.UserVersion;
                        if (sourceDbUserVersion >= 0) return;

                        using (var targetDb = new LiteDatabase($@"Filename={CURRENTDIR}data_new.db;Upgrade=true")) {
                            string[] tableNames = { "Profiles", "RoundDetails", "UserSettings", "PersonalBestLog", "LevelTimeLimit" };
                            foreach (var tableName in tableNames) {
                                if (!sourceDb.CollectionExists(tableName)) continue;
                                var sourceData = sourceDb.GetCollection(tableName).FindAll();
                                var targetCollection = targetDb.GetCollection(tableName);
                                targetCollection.InsertBulk(sourceData);
                            }
                            targetDb.UserVersion = 0;
                        }
                    }
                    if (!File.Exists($"{CURRENTDIR}data_bak_v{sourceDbUserVersion}.db")) {
                        File.Move($"{CURRENTDIR}data.db", $"{CURRENTDIR}data_bak_v{sourceDbUserVersion}.db");
                    } else {
                        File.SetAttributes($"{CURRENTDIR}data.db", FileAttributes.Normal);
                        File.Delete($"{CURRENTDIR}data.db");
                    }
                    File.Move($"{CURRENTDIR}data_new.db", $"{CURRENTDIR}data.db");
                }
            });
            this.RunDatabaseTask(migrationTask, false);
        }

        private void DatabaseBackup(bool initJob) {
            lock (this.StatsDB) {
                Task backupTask = new Task(() => {
                    // Create a new backup file for today if it doesn't exist
                    string todayBackupDbDate = DateTime.Today.ToString("yyyyMMdd");
                    if (File.Exists($"{CURRENTDIR}data_bak_({todayBackupDbDate}).db")) return;

                    this.StatsDB.Checkpoint();
                    File.Copy($"{CURRENTDIR}data.db", $"{CURRENTDIR}data_bak_({todayBackupDbDate}).db");

                    // If there are more than 3 backup files, delete the oldest one
                    string[] backupDbFiles = Directory.GetFiles(CURRENTDIR, "data_bak_(*).db");
                    if (backupDbFiles.Length > 3) {
                        DateTime oldestBackupDbDate = DateTime.MaxValue;
                        foreach (var backupDbFile in backupDbFiles) {
                            if (DateTime.TryParseExact(backupDbFile.Substring(backupDbFile.Length - 12, 8), "yyyyMMdd",
                                                       CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime backupDbDate)) {
                                if (backupDbDate < oldestBackupDbDate) {
                                    oldestBackupDbDate = backupDbDate;
                                }
                            }
                        }
                        if (oldestBackupDbDate != DateTime.MaxValue) {
                            File.SetAttributes($"{CURRENTDIR}data_bak_({oldestBackupDbDate:yyyyMMdd}).db", FileAttributes.Normal);
                            File.Delete($"{CURRENTDIR}data_bak_({oldestBackupDbDate:yyyyMMdd}).db");
                        }
                    }
                });
                this.RunDatabaseTask(backupTask, false);
                this.DatabaseBackupJob(initJob);
            }
        }

        private void DatabaseBackupJob(bool initJob) {
            double interval;
            if (initJob) {
                initJob = false;
                DateTime now = DateTime.Now;
                DateTime targetTime = new DateTime(now.Year, now.Month, now.Day, 0, 1, 0);
                if (now >= targetTime) {
                    targetTime = targetTime.AddDays(1);
                }
                interval = (targetTime - now).TotalMilliseconds + 1000;
            } else {
                interval = 24 * 60 * 60 * 1000;
            }
            new TimerAbsolute((s, e) => this.DatabaseBackup(initJob)).Start(interval);
        }

        private void UpdateDatabaseOnlineJob(bool initJob, bool retry = false) {
            double interval;
            if (retry) {
                interval = 5 * 1000;
            } else if (initJob) {
                initJob = false;
                DateTime currentUtc = DateTime.UtcNow;
                DateTime targetTime = new DateTime(currentUtc.Year, currentUtc.Month, currentUtc.Day, 12, 1, this.randSecond);
                if (currentUtc >= targetTime) {
                    targetTime = targetTime.AddDays(1);
                }
                interval = (targetTime - currentUtc).TotalMilliseconds + 1000;
            } else {
                interval = 24 * 60 * 60 * 1000;
            }
            new TimerAbsolute((s, e) => this.UpdateDatabaseOnline(initJob)).Start(interval);
        }

        private void UpdateLevelTimeLimit() {
            lock (this.LevelTimeLimitCache) {
                using (ApiWebClient web = new ApiWebClient()) {
                    try {
                        string json = web.DownloadString(Utils.FINALBEANSSTATS_LEVEL_TIME_LIMIT_DB_URL);
                        LevelTimeLimitInfo levelTimeLimit = System.Text.Json.JsonSerializer.Deserialize<LevelTimeLimitInfo>(json);
                        if (levelTimeLimit.version > this.CurrentSettings.LevelTimeLimitVersion) {
                            List<LevelTimeLimit> newList = new List<LevelTimeLimit>();
                            foreach (var roundpool in levelTimeLimit.data.roundpools) {
                                foreach (var level in roundpool.levels) {
                                    newList.Add(new LevelTimeLimit { LevelId = level.id, Duration = level.duration });
                                }
                            }
                            this.LevelTimeLimitCache = newList;

                            lock (this.StatsDB) {
                                Task updateLevelTimeLimitTask = new Task(() => {
                                    this.StatsDB.BeginTrans();
                                    this.LevelTimeLimit.DeleteAll();
                                    this.LevelTimeLimit.InsertBulk(this.LevelTimeLimitCache);
                                    this.StatsDB.Commit();
                                });
                                this.RunDatabaseTask(updateLevelTimeLimitTask, false);
                            }
                            this.CurrentSettings.LevelTimeLimitVersion = levelTimeLimit.version;
                            this.SaveUserSettings();
                        }
                    } catch {
                        // ignored
                    }
                }
            }
        }

        public struct LatestDbVersionsInfo {
            public bool success { get; set; }
            public List<DbInfo> db_list { get; set; }
            public struct DbInfo {
                public string name { get; set; }
                public int version { get; set; }
            }
        }

        public struct LevelTimeLimitInfo {
            public int version { get; set; }
            public ShowData data { get; set; }
            public struct ShowData {
                public List<Roundpool> roundpools { get; set; }
                public struct Roundpool {
                    public string id { get; set; }
                    public List<Level> levels { get; set; }
                    public struct Level {
                        public string id { get; set; }
                        public int duration { get; set; }
                    }
                }
            }
        }

        private void CheckDatabaseUpdate() {
            using (ApiWebClient web = new ApiWebClient()) {
                try {
                    string json = web.DownloadString(Utils.FINALBEANSSTATS_LATEST_DB_VERSIONS_URL);
                    LatestDbVersionsInfo latestDbVersionsInfo = System.Text.Json.JsonSerializer.Deserialize<LatestDbVersionsInfo>(json);
                    if (latestDbVersionsInfo.success) {
                        foreach (var dbInfo in latestDbVersionsInfo.db_list) {
                            if (string.Equals(dbInfo.name, "LevelTimeLimit") && dbInfo.version > this.CurrentSettings.LevelTimeLimitVersion) {
                                this.UpdateLevelTimeLimit();
                            }
                        }
                    }
                } catch {
                    // ignored
                }
            }
        }

        private void UpdateDatabaseOnline(bool initJob) {
            if (!Utils.IsInternetConnected()) {
                this.UpdateDatabaseOnlineJob(true, true);
                return;
            }
            this.CheckDatabaseUpdate();
            this.UpdateDatabaseOnlineJob(initJob);
        }

        private string TranslateChangelog(string s) {
            string[] lines = s.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            string rtnStr = string.Empty;
            for (int i = 0; i < lines.Length; i++) {
                if (i > 0) rtnStr += Environment.NewLine;
                rtnStr += CurrentLanguage == Language.English || string.IsNullOrEmpty(Multilingual.GetWord(lines[i].Replace("  - ", "message_changelog_").Replace(" ", "_")))
                          ? lines[i]
                          : $"  - {Multilingual.GetWord(lines[i].Replace("  - ", "message_changelog_").Replace(" ", "_"))}";
            }
            for (int i = 0; i < 5 - lines.Length; i++) {
                rtnStr += Environment.NewLine;
            }
            return rtnStr;
        }

        private void InitLogData() {
            this.ClearPersonalBestLog(15);
            // this.ClearServerConnectionLog(5);
            // this.ServerConnectionLogCache = this.ServerConnectionLog.FindAll().ToList();
            this.PersonalBestLogCache = this.PersonalBestLog.FindAll().ToList();
            this.LevelTimeLimitCache = this.LevelTimeLimit.FindAll().ToList();
        }

        private void SetWindowCorner() {
            Utils.DwmSetWindowAttribute(this.menu.Handle, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref windowConerPreference, sizeof(uint));
            Utils.DwmSetWindowAttribute(this.menuFilters.DropDown.Handle, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref windowConerPreference, sizeof(uint));
            Utils.DwmSetWindowAttribute(this.menuStatsFilter.DropDown.Handle, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref windowConerPreference, sizeof(uint));
            Utils.DwmSetWindowAttribute(this.menuPartyFilter.DropDown.Handle, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref windowConerPreference, sizeof(uint));
            Utils.DwmSetWindowAttribute(this.menuProfile.DropDown.Handle, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref windowConerPreference, sizeof(uint));
            Utils.DwmSetWindowAttribute(this.menuUsefulThings.DropDown.Handle, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref windowConerPreference, sizeof(uint));
            Utils.DwmSetWindowAttribute(this.trayCMenu.Handle, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref windowConerPreference, sizeof(uint));
            Utils.DwmSetWindowAttribute(this.trayFilters.DropDown.Handle, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref windowConerPreference, sizeof(uint));
            Utils.DwmSetWindowAttribute(this.trayStatsFilter.DropDown.Handle, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref windowConerPreference, sizeof(uint));
            Utils.DwmSetWindowAttribute(this.trayPartyFilter.DropDown.Handle, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref windowConerPreference, sizeof(uint));
            Utils.DwmSetWindowAttribute(this.trayProfile.DropDown.Handle, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref windowConerPreference, sizeof(uint));
            Utils.DwmSetWindowAttribute(this.trayUsefulThings.DropDown.Handle, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref windowConerPreference, sizeof(uint));
        }

        private Stats() {
            this.DatabaseMigration();

            this.mainWndTitle = $"     {Multilingual.GetWord("main_finalbeans_stats")} v{Assembly.GetExecutingAssembly().GetName().Version.ToString(2)}";
            this.StatsDB = new LiteDatabase($@"{CURRENTDIR}data.db");
            this.StatsDB.Pragma("UTC_DATE", true);
            this.UserSettings = this.StatsDB.GetCollection<UserSettings>("UserSettings");

            Task initUserSettingsTask = new Task(() => {
                try {
                    this.CurrentSettings = this.UserSettings.FindAll().First();
                    CurrentLanguage = (Language)this.CurrentSettings.Multilingual;
                    CurrentTheme = this.CurrentSettings.Theme == 0 ? MetroThemeStyle.Light : MetroThemeStyle.Dark;
                    if (this.CurrentSettings.Version > 0) {
                        // If "Version" > 0, it means it's an incompatible database (FallGuysStats database)
                        // Dispose the database and exit the program.
                        this.StatsDB.Dispose();
                        MessageBox.Show($"{Multilingual.GetWord("message_incompatible_database")}", $"{Multilingual.GetWord("message_incompatible_database_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(0);
                    }
                } catch {
                    this.CurrentSettings = this.GetDefaultSettings();
                    CurrentTheme = MetroThemeStyle.Dark;
                    this.StatsDB.BeginTrans();
                    this.UserSettings.DeleteAll();
                    this.UserSettings.Insert(this.CurrentSettings);
                    this.StatsDB.Commit();
                }
            });
            this.RunDatabaseTask(initUserSettingsTask, false);

#if AllowUpdate
            this.RemoveBackupFiles();
#endif

            this.InitializeComponent();

            this.SetEventWaitHandle();

#if !AllowUpdate
            this.menu.Items.Remove(this.menuUpdate);
            this.trayCMenu.Items.Remove(this.trayUpdate);
#endif

            this.ShowInTaskbar = false;
            this.Opacity = 0;
            this.trayCMenu.Opacity = 0;
            this.textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;

            IpGeolocationService = this.CurrentSettings.IpGeolocationService;
            if (File.Exists($"{CURRENTDIR}{IPinfoTokenFileName}")) {
                try {
                    StreamReader sr = new StreamReader($"{CURRENTDIR}{IPinfoTokenFileName}");
                    IPinfoToken = sr.ReadLine();
                    sr.Close();
                } catch {
                    IPinfoToken = string.Empty;
                }
            } else {
                IPinfoToken = string.Empty;
            }

            this.RoundDetails = this.StatsDB.GetCollection<RoundInfo>("RoundDetails");
            this.Profiles = this.StatsDB.GetCollection<Profiles>("Profiles");
            // this.ServerConnectionLog = this.StatsDB.GetCollection<ServerConnectionLog>("ServerConnectionLog");
            this.PersonalBestLog = this.StatsDB.GetCollection<PersonalBestLog>("PersonalBestLog");
            this.LevelTimeLimit = this.StatsDB.GetCollection<LevelTimeLimit>("LevelTimeLimit");

            Task ensureCollectionsIndexTask = new Task(() => {
                this.StatsDB.BeginTrans();

                this.RoundDetails.EnsureIndex(r => r.Name);
                this.RoundDetails.EnsureIndex(r => r.ShowID);
                this.RoundDetails.EnsureIndex(r => r.Round);
                this.RoundDetails.EnsureIndex(r => r.Start);
                this.RoundDetails.EnsureIndex(r => r.InParty);

                this.Profiles.EnsureIndex(p => p.ProfileId);

                // this.ServerConnectionLog.EnsureIndex(f => f.SessionId);
                this.PersonalBestLog.EnsureIndex(f => f.PbDate);

                this.LevelTimeLimit.EnsureIndex(f => f.LevelId);

                this.StatsDB.Commit();
            });
            this.RunDatabaseTask(ensureCollectionsIndexTask, false);

            if (this.Profiles.Count() == 0) {
                this.EnableInfoStrip(false);
                this.EnableMainMenu(false);
                string sysLang = CultureInfo.CurrentUICulture.Name.StartsWith("zh") ?
                                 CultureInfo.CurrentUICulture.Name :
                                 CultureInfo.CurrentUICulture.Name.Substring(0, 2);
                using (InitFinalBeansStats initFinalBeansStats = new InitFinalBeansStats(sysLang)) {
                    initFinalBeansStats.BackImage = Properties.Resources.finalbeans_icon;
                    initFinalBeansStats.BackMaxSize = 32;
                    initFinalBeansStats.BackImagePadding = new Padding(20, 19, 0, 0);
                    initFinalBeansStats.StatsForm = this;
                    if (initFinalBeansStats.ShowDialog(this) == DialogResult.OK) {
                        Task initProfilesTask = new Task(() => {
                            CurrentLanguage = initFinalBeansStats.selectedLanguage;
                            Overlay.SetDefaultFont(18, CurrentLanguage);
                            this.CurrentSettings.PlayerName = initFinalBeansStats.playerName;
                            this.CurrentSettings.Multilingual = (int)initFinalBeansStats.selectedLanguage;
                            this.StatsDB.BeginTrans();
                            if (initFinalBeansStats.autoGenerateProfiles) {
                                int profileOrder = this.PublicShowIdList.Length + this.PublicShowIdList2.Length;
                                for (int i = this.PublicShowIdList2.Length; i >= 1; i--) {
                                    string showId = this.PublicShowIdList2[i - 1];
                                    this.Profiles.Insert(new Profiles { ProfileId = profileOrder - 1, ProfileName = Multilingual.GetShowName(showId), ProfileOrder = profileOrder, LinkedShowId = showId, DoNotCombineShows = true });
                                    profileOrder--;
                                }
                                for (int i = this.PublicShowIdList.Length; i >= 1; i--) {
                                    string showId = this.PublicShowIdList[i - 1];
                                    this.Profiles.Insert(new Profiles { ProfileId = profileOrder - 1, ProfileName = Multilingual.GetShowName(showId), ProfileOrder = profileOrder, LinkedShowId = showId, DoNotCombineShows = false });
                                    profileOrder--;
                                }
                            } else {
                                int profileOrder = this.PublicShowIdList.Length;
                                for (int i = this.PublicShowIdList.Length; i >= 1; i--) {
                                    string showId = this.PublicShowIdList[i - 1];
                                    this.Profiles.Insert(new Profiles { ProfileId = profileOrder - 1, ProfileName = Multilingual.GetShowName(showId), ProfileOrder = profileOrder, LinkedShowId = showId, DoNotCombineShows = false });
                                    profileOrder--;
                                }
                                // this.Profiles.Insert(new Profiles { ProfileId = 5, ProfileName = Multilingual.GetWord("main_profile_custom"), ProfileOrder = 6, LinkedShowId = "private_lobbies", DoNotCombineShows = false });
                                // this.Profiles.Insert(new Profiles { ProfileId = 4, ProfileName = Multilingual.GetWord("main_profile_squad"), ProfileOrder = 5, LinkedShowId = "squads_4player", DoNotCombineShows = false });
                                // this.Profiles.Insert(new Profiles { ProfileId = 3, ProfileName = Multilingual.GetWord("main_profile_trio"), ProfileOrder = 4, LinkedShowId = "squads_3player_template", DoNotCombineShows = false });
                                // this.Profiles.Insert(new Profiles { ProfileId = 2, ProfileName = Multilingual.GetWord("main_profile_duo"), ProfileOrder = 3, LinkedShowId = "squads_2player_template", DoNotCombineShows = false });
                                //
                                // this.Profiles.Insert(new Profiles { ProfileId = 1, ProfileName = Multilingual.GetShowName("fb_ltm"), ProfileOrder = 2, LinkedShowId = "fb_ltm", DoNotCombineShows = false });
                                // this.Profiles.Insert(new Profiles { ProfileId = 0, ProfileName = Multilingual.GetShowName("fb_main_show"), ProfileOrder = 1, LinkedShowId = "fb_main_show", DoNotCombineShows = false });
                            }
                            this.UserSettings.Update(this.CurrentSettings);
                            this.StatsDB.Commit();
                            this.StatsDB.UserVersion = 0;
                        });
                        this.RunDatabaseTask(initProfilesTask, false);
                    }
                }
                this.EnableInfoStrip(true);
                this.EnableMainMenu(true);
            }

            this.InitLogData();

            // this.GenerateLevelStats();

            this.StatLookup = LevelStats.ALL.ToDictionary(entry => entry.Key, entry => entry.Value);

            this.StatDetails = LevelStats.ALL.Select(entry => entry.Value).ToList();

            this.DatabaseBackup(true);

            Task updateDbVersionTask = new Task(() => this.UpdateDatabaseVersion());
            this.RunDatabaseTask(updateDbVersionTask, true);
            updateDbVersionTask.Wait();

            Task.Run(() => this.UpdateDatabaseOnline(true));

            this.BackImage = Properties.Resources.main_48_icon;
            this.BackMaxSize = 36;
            this.BackImagePadding = new Padding(18, 18, 0, 0);
            this.SetMinimumSize();
            this.ChangeLanguage();
            this.InitMainDataGridView();
            this.UpdateGridRoundName();

            this.overlay = new Overlay { Text = @"FinalBeans Stats Overlay", StatsForm = this, Icon = this.Icon, ShowIcon = true, BackgroundResourceName = this.CurrentSettings.OverlayBackgroundResourceName, TabResourceName = this.CurrentSettings.OverlayTabResourceName };

            Screen screen = Utils.GetCurrentScreen(this.overlay.Location);
            Point screenLocation = screen != null ? screen.Bounds.Location : Screen.PrimaryScreen.Bounds.Location;
            Size screenSize = screen != null ? screen.Bounds.Size : Screen.PrimaryScreen.Bounds.Size;
            this.screenCenter = new Point(screenLocation.X + (screenSize.Width / 2), screenLocation.Y + (screenSize.Height / 2));

            this.logFile.OnParsedLogLines += this.LogFile_OnParsedLogLines;
            this.logFile.OnNewLogFileDate += this.LogFile_OnNewLogFileDate;
            this.logFile.OnServerConnectionNotification += this.LogFile_OnServerConnectionNotification;
            this.logFile.OnPersonalBestNotification += this.LogFile_OnPersonalBestNotification;
            this.logFile.OnError += this.LogFile_OnError;
            this.logFile.OnParsedLogLinesCurrent += this.LogFile_OnParsedLogLinesCurrent;
            this.logFile.StatsForm = this;

            string fixedPosition = this.CurrentSettings.OverlayFixedPosition;
            this.overlay.SetFixedPosition(
                string.Equals(fixedPosition, "ne"),
                string.Equals(fixedPosition, "nw"),
                string.Equals(fixedPosition, "se"),
                string.Equals(fixedPosition, "sw"),
                string.Equals(fixedPosition, "free")
            );
            if (this.overlay.IsFixed()) this.overlay.Cursor = Cursors.Default;
            this.overlay.Opacity = this.CurrentSettings.OverlayBackgroundOpacity / 100D;
            this.overlay.Show();
            this.overlay.Hide();
            this.overlay.StartTimer();

            this.UpdateGameExeLocation();
        }

        protected override void WndProc(ref Message m) {
            if (m.Msg == 0x0011) {
                this.Stats_ExitProgram(this, null);
            } else {
                base.WndProc(ref m);
            }
        }

        public void cmtt_levelDetails_Draw(object sender, DrawToolTipEventArgs e) {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Draw the standard background.
            //e.DrawBackground();
            // Draw the custom background.
            e.Graphics.FillRectangle(CurrentTheme == MetroThemeStyle.Light ? Brushes.Black : Brushes.WhiteSmoke, e.Bounds);

            // Draw the standard border.
            e.DrawBorder();
            // Draw the custom border to appear 3-dimensional.
            //e.Graphics.DrawLines(SystemPens.ControlLightLight, new[] {
            //    new Point (0, e.Bounds.Height - 1), 
            //    new Point (0, 0), 
            //    new Point (e.Bounds.Width - 1, 0)
            //});
            //e.Graphics.DrawLines(SystemPens.ControlDarkDark, new[] {
            //    new Point (0, e.Bounds.Height - 1), 
            //    new Point (e.Bounds.Width - 1, e.Bounds.Height - 1), 
            //    new Point (e.Bounds.Width - 1, 0)
            //});

            // Draw the standard text with customized formatting options.
            //e.DrawText(TextFormatFlags.TextBoxControl | TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak | TextFormatFlags.LeftAndRightPadding);
            // Draw the custom text.
            // The using block will dispose the StringFormat automatically.
            //using (StringFormat sf = new StringFormat()) {
            //    sf.Alignment = StringAlignment.Near;
            //    sf.LineAlignment = StringAlignment.Near;
            //    sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
            //    sf.FormatFlags = StringFormatFlags.NoWrap;
            //    e.Graphics.DrawString(e.ToolTipText, Overlay.GetMainFont(12), SystemBrushes.ActiveCaptionText, e.Bounds, sf);
            //    //using (Font f = new Font("Tahoma", 9)) {
            //    //    e.Graphics.DrawString(e.ToolTipText, f, SystemBrushes.ActiveCaptionText, e.Bounds, sf);
            //    //}
            //}
            e.Graphics.DrawString(e.ToolTipText, InstalledEmojiFont ? new Font("Segoe UI Emoji", 8.6f) : e.Font, CurrentTheme == MetroThemeStyle.Light ? Brushes.DarkGray : Brushes.Black, new PointF(e.Bounds.X + 8, e.Bounds.Y - 8));

            MetroToolTip t = (MetroToolTip)sender;
            PropertyInfo h = t.GetType().GetProperty("Handle", BindingFlags.NonPublic | BindingFlags.Instance);
            IntPtr handle = (IntPtr)h.GetValue(t);
            Control c = e.AssociatedControl;
            if (c.Parent != null) {
                Point location = c.Parent.PointToScreen(new Point(c.Right - e.Bounds.Width, c.Bottom));
                Utils.MoveWindow(handle, location.X, location.Y, e.Bounds.Width, e.Bounds.Height, false);
            }
        }

        public void cmtt_levelDetails_Draw2(object sender, DrawToolTipEventArgs e) {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            e.Graphics.FillRectangle(CurrentTheme == MetroThemeStyle.Light ? Brushes.Black : Brushes.WhiteSmoke, e.Bounds);

            e.DrawBorder();
            e.Graphics.DrawString(e.ToolTipText, e.Font, CurrentTheme == MetroThemeStyle.Light ? Brushes.DarkGray : Brushes.Black, new PointF(e.Bounds.X + 8, e.Bounds.Y - 8));

            MetroToolTip t = (MetroToolTip)sender;
            PropertyInfo h = t.GetType().GetProperty("Handle", BindingFlags.NonPublic | BindingFlags.Instance);
            IntPtr handle = (IntPtr)h.GetValue(t);
            Control c = e.AssociatedControl;
            if (c.Parent != null) {
                Point location = c.Parent.PointToScreen(new Point(c.Right - e.Bounds.Width, c.Bottom));
                Utils.MoveWindow(handle, location.X, location.Y, e.Bounds.Width, e.Bounds.Height, false);
            }
        }

        private void cmtt_overlay_Draw(object sender, DrawToolTipEventArgs e) {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Draw the custom background.
            e.Graphics.FillRectangle(CurrentTheme == MetroThemeStyle.Light ? Brushes.Black : Brushes.WhiteSmoke, e.Bounds);

            // Draw the standard border.
            e.DrawBorder();

            e.Graphics.DrawString(e.ToolTipText, e.Font, CurrentTheme == MetroThemeStyle.Light ? Brushes.DarkGray : Brushes.Black, new PointF(e.Bounds.X + 2, e.Bounds.Y + 2));

            MetroToolTip t = (MetroToolTip)sender;
            PropertyInfo h = t.GetType().GetProperty("Handle", BindingFlags.NonPublic | BindingFlags.Instance);
            IntPtr handle = (IntPtr)h.GetValue(t);
            Control c = e.AssociatedControl;
            if (c.Parent != null) {
                Point location = c.Parent.PointToScreen(new Point(c.Right - e.Bounds.Width, c.Bottom));
                Utils.MoveWindow(handle, location.X, location.Y, e.Bounds.Width, e.Bounds.Height, false);
            }
        }

        private void cmtt_center_Draw(object sender, DrawToolTipEventArgs e) {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Draw the custom background.
            e.Graphics.FillRectangle(CurrentTheme == MetroThemeStyle.Light ? Brushes.Black : Brushes.WhiteSmoke, e.Bounds);

            // Draw the standard border.
            e.DrawBorder();

            // Draw the custom text.
            // The using block will dispose the StringFormat automatically.
            using (StringFormat sf = new StringFormat()) {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                sf.HotkeyPrefix = HotkeyPrefix.None;
                sf.FormatFlags = StringFormatFlags.NoWrap;
                e.Graphics.DrawString(e.ToolTipText, e.Font, CurrentTheme == MetroThemeStyle.Light ? Brushes.DarkGray : Brushes.Black, e.Bounds, sf);
            }

            MetroToolTip t = (MetroToolTip)sender;
            PropertyInfo h = t.GetType().GetProperty("Handle", BindingFlags.NonPublic | BindingFlags.Instance);
            IntPtr handle = (IntPtr)h.GetValue(t);
            Control c = e.AssociatedControl;
            if (c.Parent != null) {
                Point location = c.Parent.PointToScreen(new Point(c.Right - e.Bounds.Width, c.Bottom));
                Utils.MoveWindow(handle, location.X, location.Y, e.Bounds.Width, e.Bounds.Height, false);
            }
        }

        public class CustomToolStripSystemRenderer : ToolStripSystemRenderer {
            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) {
                //base.OnRenderToolStripBorder(e);
            }
        }

        public class CustomLightArrowRenderer : ToolStripProfessionalRenderer {
            public CustomLightArrowRenderer() : base(new CustomLightColorTable()) { }
            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e) {
                //var tsMenuItem = e.Item as ToolStripMenuItem;
                //if (tsMenuItem != null) e.ArrowColor = CurrentTheme == MetroThemeStyle.Dark ? Color.DarkGray : Color.FromArgb(17, 17, 17);
                //Point point = new Point(e.ArrowRectangle.Left + e.ArrowRectangle.Width / 2, e.ArrowRectangle.Top + e.ArrowRectangle.Height / 2);
                //Point[] points = new Point[3]
                //{
                //    new Point(point.X - 2, point.Y - 4),
                //    new Point(point.X - 2, point.Y + 4),
                //    new Point(point.X + 2, point.Y)
                //};
                //e.Graphics.FillPolygon(Brushes.DarkGray, points);
                e.ArrowColor = Color.FromArgb(17, 17, 17);
                base.OnRenderArrow(e);
            }
        }

        public class CustomDarkArrowRenderer : ToolStripProfessionalRenderer {
            public CustomDarkArrowRenderer() : base(new CustomDarkColorTable()) { }
            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e) {
                //var tsMenuItem = e.Item as ToolStripMenuItem;
                //if (tsMenuItem != null) e.ArrowColor = CurrentTheme == MetroThemeStyle.Dark ? Color.DarkGray : Color.FromArgb(17, 17, 17);
                //Point point = new Point(e.ArrowRectangle.Left + e.ArrowRectangle.Width / 2, e.ArrowRectangle.Top + e.ArrowRectangle.Height / 2);
                //Point[] points = new Point[3]
                //{
                //    new Point(point.X - 2, point.Y - 4),
                //    new Point(point.X - 2, point.Y + 4),
                //    new Point(point.X + 2, point.Y)
                //};
                //e.Graphics.FillPolygon(Brushes.DarkGray, points);
                e.ArrowColor = Color.DarkGray;
                base.OnRenderArrow(e);
            }
        }

        private class CustomLightColorTable : ProfessionalColorTable {
            public CustomLightColorTable() { UseSystemColors = false; }
            //public override Color ToolStripBorder {
            //    get { return Color.Red; }
            //}
            public override Color MenuBorder {
                get { return Color.White; }
            }
            public override Color ToolStripDropDownBackground {
                get { return Color.White; }
            }
            public override Color MenuItemBorder {
                get { return Color.DarkSeaGreen; }
            }
            public override Color MenuItemSelected {
                get { return Color.LightGreen; }
            }
            //public override Color MenuItemSelectedGradientBegin {
            //    get { return Color.LawnGreen; }
            //}
            //public override Color MenuItemSelectedGradientEnd {
            //    get { return Color.MediumSeaGreen; }
            //}
            //public override Color MenuStripGradientBegin {
            //    get { return Color.AliceBlue; }
            //}
            //public override Color MenuStripGradientEnd {
            //    get { return Color.DodgerBlue; }
            //}
        }

        private class CustomDarkColorTable : ProfessionalColorTable {
            public CustomDarkColorTable() { UseSystemColors = false; }
            //public override Color ToolStripBorder {
            //    get { return Color.Red; }
            //}
            public override Color MenuBorder {
                get { return Color.FromArgb(17, 17, 17); }
            }
            public override Color ToolStripDropDownBackground {
                get { return Color.FromArgb(17, 17, 17); }
            }
            public override Color MenuItemBorder {
                get { return Color.DarkSeaGreen; }
            }
            public override Color MenuItemSelected {
                get { return Color.LightGreen; }
            }
            //public override Color MenuItemSelectedGradientBegin {
            //    get { return Color.LawnGreen; }
            //}
            //public override Color MenuItemSelectedGradientEnd {
            //    get { return Color.MediumSeaGreen; }
            //}
            //public override Color MenuStripGradientBegin {
            //    get { return Color.AliceBlue; }
            //}
            //public override Color MenuStripGradientEnd {
            //    get { return Color.DodgerBlue; }
            //}
        }

        private TaskbarPosition GetTaskbarPosition() {
            TaskbarPosition taskbarPosition = TaskbarPosition.Bottom;
            Rectangle screenBounds = Screen.GetBounds(Cursor.Position);
            Rectangle workingArea = Screen.GetWorkingArea(Cursor.Position);
            if (workingArea.Width == screenBounds.Width) {
                if (workingArea.Top > 0) { taskbarPosition = TaskbarPosition.Top; }
            } else {
                if (workingArea.Left > screenBounds.Left) {
                    taskbarPosition = TaskbarPosition.Left;
                } else if (workingArea.Right < screenBounds.Right) {
                    taskbarPosition = TaskbarPosition.Right;
                }
            }
            return taskbarPosition;
        }

        public void PreventOverlayMouseClicks() {
            this.BeginInvoke((MethodInvoker)delegate {
                if (this.overlay.IsMouseOver() && ActiveForm != this) { this.SetCursorPositionCenter(); }
            });
        }

        private void SetCursorPositionCenter() {
            if (this.overlay.Location.X <= this.screenCenter.X && this.overlay.Location.Y <= this.screenCenter.Y) {
                Cursor.Position = new Point(this.screenCenter.X * 2, this.screenCenter.Y * 2); // NW
            } else if (this.overlay.Location.X <= this.screenCenter.X && this.overlay.Location.Y > this.screenCenter.Y) {
                Cursor.Position = new Point(this.screenCenter.X * 2, 0); // SW
            } else if (this.overlay.Location.X > this.screenCenter.X && this.overlay.Location.Y <= this.screenCenter.Y) {
                Cursor.Position = new Point(0, this.screenCenter.Y * 2); // NE
            } else if (this.overlay.Location.X > this.screenCenter.X && this.overlay.Location.Y > this.screenCenter.Y) {
                Cursor.Position = new Point(0, 0); // SE
            }
        }

        private void SetTheme(MetroThemeStyle theme) {
            this.SuspendLayout();
            this.mtt.Theme = theme;
            this.omtt.Theme = theme;
            this.menu.Renderer = theme == MetroThemeStyle.Light ? new CustomLightArrowRenderer() : new CustomDarkArrowRenderer() as ToolStripRenderer;
            this.trayCMenu.Renderer = theme == MetroThemeStyle.Light ? new CustomLightArrowRenderer() : new CustomDarkArrowRenderer() as ToolStripRenderer;
            foreach (Control c1 in Controls) {
                if (c1 is MenuStrip ms1) {
                    foreach (var item in ms1.Items) {
                        if (item is ToolStripMenuItem tsmi1) {
                            if (Equals(tsmi1, this.menuSettings)) {
                                tsmi1.Image = theme == MetroThemeStyle.Light ? Properties.Resources.setting_icon : Properties.Resources.setting_gray_icon;
                                tsmi1.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                            } else if (Equals(tsmi1, this.menuFilters)) {
                                tsmi1.Image = theme == MetroThemeStyle.Light ? Properties.Resources.filter_icon : Properties.Resources.filter_gray_icon;
                                tsmi1.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                            } else if (Equals(tsmi1, this.menuProfile)) {
                                tsmi1.Image = theme == MetroThemeStyle.Light ? Properties.Resources.profile_icon : Properties.Resources.profile_gray_icon;
                                tsmi1.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                            } else if (Equals(tsmi1, this.menuUpdate)) {
                                tsmi1.Image = theme == MetroThemeStyle.Light ? (this.isAvailableNewVersion ? Properties.Resources.github_update_icon : Properties.Resources.github_icon)
                                                                             : (this.isAvailableNewVersion ? Properties.Resources.github_update_gray_icon : Properties.Resources.github_gray_icon);
                                tsmi1.ForeColor = this.isAvailableNewVersion ? (theme == MetroThemeStyle.Light ? Color.FromArgb(0, 174, 219) : Color.GreenYellow) : (theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray);
                            } else if (Equals(tsmi1, this.menuHelp)) {
                                tsmi1.Image = theme == MetroThemeStyle.Light ? Properties.Resources.github_icon : Properties.Resources.github_gray_icon;
                                tsmi1.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                            } else if (Equals(tsmi1, this.menuOverlay)) {
                                tsmi1.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                            } else if (Equals(tsmi1, this.menuLaunchFinalBeans)) {
                                tsmi1.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                            } else if (Equals(tsmi1, this.menuUsefulThings)) {
                                tsmi1.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                            }

                            tsmi1.MouseEnter += this.menu_MouseEnter;
                            tsmi1.MouseLeave += this.menu_MouseLeave;
                            foreach (var item1 in tsmi1.DropDownItems) {
                                if (item1 is ToolStripMenuItem subTsmi1) {
                                    if (Equals(subTsmi1, this.menuEditProfiles)) { subTsmi1.Image = theme == MetroThemeStyle.Light ? Properties.Resources.setting_icon : Properties.Resources.setting_gray_icon; }
                                    subTsmi1.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                                    subTsmi1.BackColor = theme == MetroThemeStyle.Light ? Color.White : Color.FromArgb(17, 17, 17);
                                    subTsmi1.MouseEnter += this.menu_MouseEnter;
                                    subTsmi1.MouseLeave += this.menu_MouseLeave;
                                    foreach (var item2 in subTsmi1.DropDownItems) {
                                        if (item2 is ToolStripMenuItem subTsmi2) {
                                            if (Equals(subTsmi2, this.menuCustomRangeStats)) { subTsmi2.Image = theme == MetroThemeStyle.Light ? Properties.Resources.calendar_icon : Properties.Resources.calendar_gray_icon; }
                                            subTsmi2.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                                            subTsmi2.BackColor = theme == MetroThemeStyle.Light ? Color.White : Color.FromArgb(17, 17, 17);
                                            subTsmi2.MouseEnter += this.menu_MouseEnter;
                                            subTsmi2.MouseLeave += this.menu_MouseLeave;
                                        } else if (item2 is ToolStripSeparator subTss2) {
                                            subTss2.Paint += this.CustomToolStripSeparatorCustom_Paint;
                                            subTss2.BackColor = theme == MetroThemeStyle.Light ? Color.White : Color.FromArgb(17, 17, 17);
                                            subTss2.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                                        }
                                    }
                                } else if (item1 is ToolStripSeparator subTss1) {
                                    subTss1.Paint += this.CustomToolStripSeparatorCustom_Paint;
                                    subTss1.BackColor = theme == MetroThemeStyle.Light ? Color.White : Color.FromArgb(17, 17, 17);
                                    subTss1.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                                }
                            }
                        }
                    }
                } else if (c1 is ToolStrip ts1) {
                    ts1.BackColor = Color.Transparent;
                    foreach (var tsi1 in ts1.Items) {
                        if (tsi1 is ToolStripLabel tsl1) {
                            if (Equals(tsl1, this.lblCurrentProfile) || Equals(tsl1, this.lblPlayerName)) {
                                tsl1.Font = Overlay.GetMainFont(14f);
                                tsl1.ForeColor = theme == MetroThemeStyle.Light ? Color.Red : Color.FromArgb(0, 192, 192);
                            } else if (Equals(tsl1, this.lblTotalTime)) {
                                tsl1.Font = Overlay.GetMainFont(14f);
                                tsl1.Image = theme == MetroThemeStyle.Light ? Properties.Resources.clock_icon : Properties.Resources.clock_gray_icon;
                                tsl1.ForeColor = theme == MetroThemeStyle.Light ? Color.Blue : Color.Orange;
                            } else if (Equals(tsl1, this.lblTotalShows) || Equals(tsl1, this.lblTotalWins)) {
                                tsl1.ForeColor = theme == MetroThemeStyle.Light ? Color.Blue : Color.Orange;
                            } else if (Equals(tsl1, this.lblTotalRounds)) {
                                tsl1.Image = theme == MetroThemeStyle.Light ? Properties.Resources.round_icon : Properties.Resources.round_gray_icon;
                                tsl1.ForeColor = theme == MetroThemeStyle.Light ? Color.Blue : Color.Orange;
                            } else if (Equals(tsl1, this.lblTotalFinals)) {
                                tsl1.Image = theme == MetroThemeStyle.Light ? Properties.Resources.final_icon : Properties.Resources.final_gray_icon;
                                tsl1.ForeColor = theme == MetroThemeStyle.Light ? Color.Blue : Color.Orange;
                            } else if (Equals(tsl1, this.lblGoldMedal) || Equals(tsl1, this.lblSilverMedal) ||
                                       Equals(tsl1, this.lblBronzeMedal) || Equals(tsl1, this.lblPinkMedal) ||
                                       Equals(tsl1, this.lblEliminatedMedal) || Equals(tsl1, this.lblKudos)) {
                                tsl1.Font = Overlay.GetMainFont(14f);
                                tsl1.ForeColor = theme == MetroThemeStyle.Light ? Color.DarkSlateGray : Color.DarkGray;
                            }
                        } else if (tsi1 is ToolStripSeparator tss1) {
                            tss1.ForeColor = theme == MetroThemeStyle.Light ? Color.DarkSlateGray : Color.DarkGray; break;
                        }
                    }
                } else if (c1 is MetroToggle mt1) {
                    mt1.Theme = theme;
                } else if (c1 is MetroLink ml1) {
                    ml1.Theme = theme;
                } else if (c1 is Label lbl1) {
                    lbl1.Font = Overlay.GetMainFont(13f);
                    if (Equals(lbl1, this.lblIgnoreLevelTypeWhenSorting)) {
                        lbl1.ForeColor = this.mtgIgnoreLevelTypeWhenSorting.Checked ? (theme == MetroThemeStyle.Light ? Color.FromArgb(0, 174, 219) : Color.GreenYellow) : (theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray);
                    }
                }
            }

            this.gridDetails.Theme = theme;
            this.gridDetails.SetContextMenuTheme();
            this.gridDetails.BackgroundColor = theme == MetroThemeStyle.Light ? Color.White : Color.FromArgb(17, 17, 17);
            this.dataGridViewCellStyle1.BackColor = theme == MetroThemeStyle.Light ? Color.LightGray : Color.FromArgb(2, 2, 2);
            this.dataGridViewCellStyle1.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
            this.dataGridViewCellStyle1.SelectionBackColor = theme == MetroThemeStyle.Light ? Color.Cyan : Color.DarkSlateBlue;
            //this.dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            this.dataGridViewCellStyle2.BackColor = theme == MetroThemeStyle.Light ? Color.White : Color.FromArgb(49, 51, 56);
            this.dataGridViewCellStyle2.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.WhiteSmoke;
            this.dataGridViewCellStyle2.SelectionBackColor = theme == MetroThemeStyle.Light ? Color.DeepSkyBlue : Color.PaleGreen;
            this.dataGridViewCellStyle2.SelectionForeColor = Color.Black;

            foreach (var item in this.trayCMenu.Items) {
                if (item is ToolStripMenuItem tsmi) {
                    tsmi.BackColor = theme == MetroThemeStyle.Light ? Color.White : Color.FromArgb(17, 17, 17);
                    tsmi.MouseEnter += this.trayMenu_MouseEnter;
                    tsmi.MouseLeave += this.trayMenu_MouseLeave;
                    if (Equals(tsmi, this.traySettings)) {
                        tsmi.Image = theme == MetroThemeStyle.Light ? Properties.Resources.setting_icon : Properties.Resources.setting_gray_icon;
                        tsmi.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                    } else if (Equals(tsmi, this.trayFilters)) {
                        tsmi.Image = theme == MetroThemeStyle.Light ? Properties.Resources.filter_icon : Properties.Resources.filter_gray_icon;
                        tsmi.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                    } else if (Equals(tsmi, this.trayProfile)) {
                        tsmi.Image = theme == MetroThemeStyle.Light ? Properties.Resources.profile_icon : Properties.Resources.profile_gray_icon;
                        tsmi.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                    } else if (Equals(tsmi, this.trayUpdate)) {
                        tsmi.Image = theme == MetroThemeStyle.Light ? (this.isAvailableNewVersion ? Properties.Resources.github_update_icon : Properties.Resources.github_icon)
                                                                    : (this.isAvailableNewVersion ? Properties.Resources.github_update_gray_icon : Properties.Resources.github_gray_icon);
                        tsmi.ForeColor = this.isAvailableNewVersion ? (theme == MetroThemeStyle.Light ? Color.FromArgb(0, 174, 219) : Color.GreenYellow) : (theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray);
                    } else if (Equals(tsmi, this.trayHelp)) {
                        tsmi.Image = theme == MetroThemeStyle.Light ? Properties.Resources.github_icon : Properties.Resources.github_gray_icon;
                        tsmi.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                    } else if (Equals(tsmi, this.trayExitProgram)) {
                        tsmi.Image = theme == MetroThemeStyle.Light ? Properties.Resources.shutdown_icon : Properties.Resources.shutdown_gray_icon;
                        tsmi.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                    } else if (Equals(tsmi, this.trayOverlay)) {
                        tsmi.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                    } else if (Equals(tsmi, this.trayLaunchFinalBeans)) {
                        tsmi.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                    } else if (Equals(tsmi, this.trayUsefulThings)) {
                        tsmi.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                    }

                    foreach (var subItem1 in tsmi.DropDownItems) {
                        if (subItem1 is ToolStripMenuItem stsmi1) {
                            if (Equals(stsmi1, this.trayEditProfiles)) { stsmi1.Image = theme == MetroThemeStyle.Light ? Properties.Resources.setting_icon : Properties.Resources.setting_gray_icon; }
                            stsmi1.BackColor = theme == MetroThemeStyle.Light ? Color.White : Color.FromArgb(17, 17, 17);
                            stsmi1.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                            stsmi1.MouseEnter += this.trayMenu_MouseEnter;
                            stsmi1.MouseLeave += this.trayMenu_MouseLeave;
                            foreach (var subItem2 in stsmi1.DropDownItems) {
                                if (subItem2 is ToolStripMenuItem stsmi2) {
                                    if (Equals(stsmi2, this.trayCustomRangeStats)) { stsmi2.Image = theme == MetroThemeStyle.Light ? Properties.Resources.calendar_icon : Properties.Resources.calendar_gray_icon; }
                                    stsmi2.BackColor = theme == MetroThemeStyle.Light ? Color.White : Color.FromArgb(17, 17, 17);
                                    stsmi2.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                                    stsmi2.MouseEnter += this.trayMenu_MouseEnter;
                                    stsmi2.MouseLeave += this.trayMenu_MouseLeave;
                                } else if (subItem2 is ToolStripSeparator stss2) {
                                    stss2.Paint += this.CustomToolStripSeparatorCustom_Paint;
                                    stss2.BackColor = theme == MetroThemeStyle.Light ? Color.White : Color.FromArgb(17, 17, 17);
                                    stss2.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                                }
                            }
                        } else if (subItem1 is ToolStripSeparator stss1) {
                            stss1.Paint += this.CustomToolStripSeparatorCustom_Paint;
                            stss1.BackColor = theme == MetroThemeStyle.Light ? Color.White : Color.FromArgb(17, 17, 17);
                            stss1.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                        }
                    }
                } else if (item is ToolStripSeparator tss) {
                    tss.Paint += this.CustomToolStripSeparatorCustom_Paint;
                    tss.BackColor = theme == MetroThemeStyle.Light ? Color.White : Color.FromArgb(17, 17, 17);
                    tss.ForeColor = theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                }
            }
            this.Theme = theme;
            this.ResumeLayout();
            this.Invalidate(true);
        }

        private void CustomToolStripSeparatorCustom_Paint(Object sender, PaintEventArgs e) {
            ToolStripSeparator separator = (ToolStripSeparator)sender;
            e.Graphics.FillRectangle(new SolidBrush(this.Theme == MetroThemeStyle.Light ? Color.White : Color.FromArgb(17, 17, 17)), 0, 0, separator.Width, separator.Height); // CUSTOM_COLOR_BACKGROUND
            e.Graphics.DrawLine(new Pen(this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray), 30, separator.Height / 2, separator.Width - 4, separator.Height / 2); // CUSTOM_COLOR_FOREGROUND
        }

        private void trayMenu_MouseEnter(object sender, EventArgs e) {
            switch (sender) {
                case ToolStripMenuItem tsi: {
                        tsi.ForeColor = Color.Black;
                        if (Equals(tsi, this.traySettings)) {
                            tsi.Image = Properties.Resources.setting_icon;
                        } else if (Equals(tsi, this.trayFilters)) {
                            tsi.Image = Properties.Resources.filter_icon;
                        } else if (Equals(tsi, this.trayCustomRangeStats)) {
                            tsi.Image = Properties.Resources.calendar_icon;
                        } else if (Equals(tsi, this.trayProfile)) {
                            tsi.Image = Properties.Resources.profile_icon;
                        } else if (Equals(tsi, this.trayUpdate)) {
                            tsi.Image = this.isAvailableNewVersion ? Properties.Resources.github_update_icon : Properties.Resources.github_icon;
                        } else if (Equals(tsi, this.trayHelp)) {
                            tsi.Image = Properties.Resources.github_icon;
                        } else if (Equals(tsi, this.trayEditProfiles)) {
                            tsi.Image = Properties.Resources.setting_icon;
                        } else if (Equals(tsi, this.trayExitProgram)) {
                            tsi.Image = Properties.Resources.shutdown_icon;
                        }
                        break;
                    }
            }
        }

        private void trayMenu_MouseLeave(object sender, EventArgs e) {
            this.Cursor = Cursors.Default;
            switch (sender) {
                case ToolStripMenuItem tsi: {
                        if (Equals(tsi, this.traySettings)) {
                            tsi.Image = this.Theme == MetroThemeStyle.Light ? Properties.Resources.setting_icon : Properties.Resources.setting_gray_icon;
                            tsi.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                        } else if (Equals(tsi, this.trayFilters)) {
                            tsi.Image = this.Theme == MetroThemeStyle.Light ? Properties.Resources.filter_icon : Properties.Resources.filter_gray_icon;
                            tsi.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                        } else if (Equals(tsi, this.trayCustomRangeStats)) {
                            tsi.Image = this.Theme == MetroThemeStyle.Light ? Properties.Resources.calendar_icon : Properties.Resources.calendar_gray_icon;
                            tsi.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                        } else if (Equals(tsi, this.trayProfile)) {
                            tsi.Image = this.Theme == MetroThemeStyle.Light ? Properties.Resources.profile_icon : Properties.Resources.profile_gray_icon;
                            tsi.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                        } else if (Equals(tsi, this.trayUpdate)) {
                            tsi.Image = this.Theme == MetroThemeStyle.Light ? (this.isAvailableNewVersion ? Properties.Resources.github_update_icon : Properties.Resources.github_icon)
                                                                            : (this.isAvailableNewVersion ? Properties.Resources.github_update_gray_icon : Properties.Resources.github_gray_icon);
                            tsi.ForeColor = this.isAvailableNewVersion ? (this.Theme == MetroThemeStyle.Light ? Color.FromArgb(0, 174, 219) : Color.GreenYellow) : (this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray);
                        } else if (Equals(tsi, this.trayHelp)) {
                            tsi.Image = this.Theme == MetroThemeStyle.Light ? Properties.Resources.github_icon : Properties.Resources.github_gray_icon;
                            tsi.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                        } else if (Equals(tsi, this.trayEditProfiles)) {
                            tsi.Image = this.Theme == MetroThemeStyle.Light ? Properties.Resources.setting_icon : Properties.Resources.setting_gray_icon;
                            tsi.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                        } else if (Equals(tsi, this.trayExitProgram)) {
                            tsi.Image = this.Theme == MetroThemeStyle.Light ? Properties.Resources.shutdown_icon : Properties.Resources.shutdown_gray_icon;
                            tsi.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                        } else {
                            tsi.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                        }
                        break;
                    }
            }
        }

        private void menu_MouseEnter(object sender, EventArgs e) {
            switch (sender) {
                case ToolStripMenuItem tsmi: {
                        tsmi.ForeColor = Color.Black;
                        if (Equals(tsmi, this.menuSettings)) {
                            tsmi.Image = Properties.Resources.setting_icon;
                        } else if (Equals(tsmi, this.menuFilters)) {
                            tsmi.Image = Properties.Resources.filter_icon;
                        } else if (Equals(tsmi, this.menuCustomRangeStats)) {
                            tsmi.Image = Properties.Resources.calendar_icon;
                        } else if (Equals(tsmi, this.menuProfile)) {
                            tsmi.Image = Properties.Resources.profile_icon;
                        } else if (Equals(tsmi, this.menuUpdate)) {
                            tsmi.Image = this.isAvailableNewVersion ? Properties.Resources.github_update_icon : Properties.Resources.github_icon;
                        } else if (Equals(tsmi, this.menuHelp)) {
                            tsmi.Image = Properties.Resources.github_icon;
                        } else if (Equals(tsmi, this.menuEditProfiles)) {
                            tsmi.Image = Properties.Resources.setting_icon;
                        }
                        break;
                    }
            }
        }

        private void menu_MouseLeave(object sender, EventArgs e) {
            this.Cursor = Cursors.Default;
            switch (sender) {
                case ToolStripMenuItem tsmi: {
                        if (Equals(tsmi, this.menuSettings)) {
                            tsmi.Image = this.Theme == MetroThemeStyle.Light ? Properties.Resources.setting_icon : Properties.Resources.setting_gray_icon;
                            tsmi.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                        } else if (Equals(tsmi, this.menuFilters)) {
                            tsmi.Image = this.Theme == MetroThemeStyle.Light ? Properties.Resources.filter_icon : Properties.Resources.filter_gray_icon;
                            tsmi.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                        } else if (Equals(tsmi, this.menuCustomRangeStats)) {
                            tsmi.Image = this.Theme == MetroThemeStyle.Light ? Properties.Resources.calendar_icon : Properties.Resources.calendar_gray_icon;
                            tsmi.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                        } else if (Equals(tsmi, this.menuProfile)) {
                            tsmi.Image = this.Theme == MetroThemeStyle.Light ? Properties.Resources.profile_icon : Properties.Resources.profile_gray_icon;
                            tsmi.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                        } else if (Equals(tsmi, this.menuUpdate)) {
                            tsmi.Image = this.Theme == MetroThemeStyle.Light ? (this.isAvailableNewVersion ? Properties.Resources.github_update_icon : Properties.Resources.github_icon)
                                                                             : (this.isAvailableNewVersion ? Properties.Resources.github_update_gray_icon : Properties.Resources.github_gray_icon);
                            tsmi.ForeColor = this.isAvailableNewVersion ? (this.Theme == MetroThemeStyle.Light ? Color.FromArgb(0, 174, 219) : Color.GreenYellow) : (this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray);
                        } else if (Equals(tsmi, this.menuHelp)) {
                            tsmi.Image = this.Theme == MetroThemeStyle.Light ? Properties.Resources.github_icon : Properties.Resources.github_gray_icon;
                            tsmi.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                        } else if (Equals(tsmi, this.menuEditProfiles)) {
                            tsmi.Image = this.Theme == MetroThemeStyle.Light ? Properties.Resources.setting_icon : Properties.Resources.setting_gray_icon;
                            tsmi.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                        } else {
                            tsmi.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                        }
                        break;
                    }
            }
        }

        private void infoStrip_MouseEnter(object sender, EventArgs e) {
            switch (sender) {
                case ToolStripLabel lblInfo: {
                        this.Cursor = Cursors.Hand;
                        this.infoStripForeColor = Equals(lblInfo, this.lblCurrentProfile) || Equals(lblInfo, this.lblPlayerName)
                            ? this.Theme == MetroThemeStyle.Light ? Color.Red : Color.FromArgb(0, 192, 192)
                            : this.Theme == MetroThemeStyle.Light ? Color.Blue : Color.Orange;

                        lblInfo.ForeColor = Equals(lblInfo, this.lblCurrentProfile) || Equals(lblInfo, this.lblPlayerName)
                            ? this.Theme == MetroThemeStyle.Light ? Color.FromArgb(245, 154, 168) : Color.FromArgb(231, 251, 255)
                            : this.Theme == MetroThemeStyle.Light ? Color.FromArgb(147, 174, 248) : Color.FromArgb(255, 250, 244);

                        Point cursorPosition = this.PointToClient(Cursor.Position);
                        Point position = new Point(cursorPosition.X + 16, cursorPosition.Y + 16);
                        this.AllocCustomTooltip(this.cmtt_center_Draw);
                        if (Equals(lblInfo, this.lblCurrentProfileIcon)) {
                            this.ShowCustomTooltip(Multilingual.GetWord($"{(this.CurrentSettings.AutoChangeProfile ? "profile_icon_enable_tooltip" : "profile_icon_disable_tooltip")}"), this, position);
                        } else if (Equals(lblInfo, this.lblCurrentProfile)) {
                            this.ShowCustomTooltip(Multilingual.GetWord("profile_change_tooltip"), this, position);
                        } else if (Equals(lblInfo, this.lblTotalShows)) {
                            this.ShowCustomTooltip(Multilingual.GetWord("shows_detail_tooltip"), this, position);
                        } else if (Equals(lblInfo, this.lblTotalRounds)) {
                            this.ShowCustomTooltip(Multilingual.GetWord("rounds_detail_tooltip"), this, position);
                        } else if (Equals(lblInfo, this.lblTotalFinals)) {
                            this.ShowCustomTooltip(Multilingual.GetWord("finals_detail_tooltip"), this, position);
                        } else if (Equals(lblInfo, this.lblTotalWins)) {
                            this.ShowCustomTooltip(Multilingual.GetWord("wins_detail_tooltip"), this, position);
                        } else if (Equals(lblInfo, this.lblTotalTime)) {
                            this.ShowCustomTooltip(Multilingual.GetWord("stats_detail_tooltip"), this, position);
                        } else if (Equals(lblInfo, this.lblPlayerName)) {
                            this.ShowCustomTooltip(Multilingual.GetWord("player_name_change_tooltip"), this, position);
                        }

                        break;
                    }
            }
        }

        private void infoStrip_MouseLeave(object sender, EventArgs e) {
            this.Cursor = Cursors.Default;
            this.HideCustomTooltip(this);
            if (sender is ToolStripLabel lblInfo) {
                lblInfo.ForeColor = this.infoStripForeColor;
            }
        }

        public void ReloadProfileMenuItems() {
            this.ProfileMenuItems.Clear();
            this.menuProfile.DropDownItems.Clear();
            this.menuProfile.DropDownItems.Add(this.menuEditProfiles);
            this.menuProfile.DropDownItems.Add(this.menuSeparator2);
            this.menuSeparator2.Paint += this.CustomToolStripSeparatorCustom_Paint;
            this.menuSeparator2.BackColor = this.Theme == MetroThemeStyle.Light ? Color.White : Color.FromArgb(17, 17, 17);
            this.menuSeparator2.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;

            this.ProfileTrayItems.Clear();
            this.trayProfile.DropDownItems.Clear();
            this.trayProfile.DropDownItems.Add(this.trayEditProfiles);
            this.trayProfile.DropDownItems.Add(this.traySubSeparator2);
            this.traySubSeparator2.Paint += this.CustomToolStripSeparatorCustom_Paint;
            this.traySubSeparator2.BackColor = this.Theme == MetroThemeStyle.Light ? Color.White : Color.FromArgb(17, 17, 17);
            this.traySubSeparator2.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;

            this.AllProfiles.Clear();
            this.AllProfiles = this.Profiles.FindAll().ToList();
            this.profileWithLinkedCustomShow = this.AllProfiles.Find(p => string.Equals(p.LinkedShowId, "private_lobbies"))?.ProfileId ?? -1;
            int profileNumber = 0;
            for (int i = this.AllProfiles.Count - 1; i >= 0; i--) {
                Profiles profile = this.AllProfiles[i];
                ToolStripMenuItem menuItem = new ToolStripMenuItem {
                    Checked = this.CurrentSettings.SelectedProfile == profile.ProfileId,
                    CheckOnClick = true,
                    CheckState = this.CurrentSettings.SelectedProfile == profile.ProfileId ? CheckState.Checked : CheckState.Unchecked,
                    Name = $@"menuProfile{profile.ProfileId}"
                };
                ToolStripMenuItem trayItem = new ToolStripMenuItem {
                    Checked = this.CurrentSettings.SelectedProfile == profile.ProfileId,
                    CheckOnClick = true,
                    CheckState = this.CurrentSettings.SelectedProfile == profile.ProfileId ? CheckState.Checked : CheckState.Unchecked,
                    Name = $@"menuProfile{profile.ProfileId}"
                };

                switch (profileNumber++) {
                    case 0: menuItem.Image = this.numberOne; trayItem.Image = this.numberOne; break;
                    case 1: menuItem.Image = this.numberTwo; trayItem.Image = this.numberTwo; break;
                    case 2: menuItem.Image = this.numberThree; trayItem.Image = this.numberThree; break;
                    case 3: menuItem.Image = this.numberFour; trayItem.Image = this.numberFour; break;
                    case 4: menuItem.Image = this.numberFive; trayItem.Image = this.numberFive; break;
                    case 5: menuItem.Image = this.numberSix; trayItem.Image = this.numberSix; break;
                    case 6: menuItem.Image = this.numberSeven; trayItem.Image = this.numberSeven; break;
                    case 7: menuItem.Image = this.numberEight; trayItem.Image = this.numberEight; break;
                    case 8: menuItem.Image = this.numberNine; trayItem.Image = this.numberNine; break;
                }
                menuItem.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                menuItem.BackColor = this.Theme == MetroThemeStyle.Light ? Color.White : Color.FromArgb(17, 17, 17);
                menuItem.Size = new Size(180, 22);
                menuItem.Text = profile.ProfileName.Replace("&", "&&");
                menuItem.Click += this.menuStats_Click;
                menuItem.Paint += this.menuProfile_Paint;
                menuItem.MouseMove += this.setCursor_MouseMove;
                // menuItem.MouseLeave += this.setCursor_MouseLeave;
                menuItem.MouseEnter += this.menu_MouseEnter;
                menuItem.MouseLeave += this.menu_MouseLeave;
                this.menuProfile.DropDownItems.Add(menuItem);
                this.ProfileMenuItems.Add(menuItem);

                trayItem.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                trayItem.BackColor = this.Theme == MetroThemeStyle.Light ? Color.White : Color.FromArgb(17, 17, 17);
                trayItem.Size = new Size(180, 22);
                trayItem.Text = profile.ProfileName.Replace("&", "&&");
                trayItem.Click += this.menuStats_Click;
                trayItem.Paint += this.menuProfile_Paint;
                trayItem.MouseEnter += this.trayMenu_MouseEnter;
                trayItem.MouseLeave += this.trayMenu_MouseLeave;
                this.trayProfile.DropDownItems.Add(trayItem);
                this.ProfileTrayItems.Add(trayItem);

                //((ToolStripDropDownMenu)menuProfile.DropDown).ShowCheckMargin = true;
                //((ToolStripDropDownMenu)menuProfile.DropDown).ShowImageMargin = true;

                if (this.CurrentSettings.SelectedProfile == profile.ProfileId) {
                    if (this.AllProfiles.Count != 0) this.SetCurrentProfileIcon(!string.IsNullOrEmpty(profile.LinkedShowId));
                    this.menuStats_Click(menuItem, EventArgs.Empty);
                }
            }
        }

        private void menuProfile_Paint(object sender, PaintEventArgs e) {
            if (this.AllProfiles.FindIndex(p => string.Equals(p.ProfileId.ToString(), ((ToolStripMenuItem)sender).Name.Substring(11)) && !string.IsNullOrEmpty(p.LinkedShowId)) != -1) {
                e.Graphics.DrawImage(this.CurrentSettings.AutoChangeProfile ? Properties.Resources.link_on_icon :
                                     this.Theme == MetroThemeStyle.Light ? Properties.Resources.link_icon : Properties.Resources.link_gray_icon, 21, 5, 11, 11);
            }
        }

#if AllowUpdate
        private void RemoveBackupFiles() {
            foreach (string file in Directory.EnumerateFiles(CURRENTDIR, "*.bak")) {
                try {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                } catch {
                    // ignored
                }
            }
        }
#endif

        private void UpdateDatabaseVersion() {
            for (int version = this.CurrentSettings.Version_FB; version < currentDbVersion; version++) {
                switch (version) {
                    default: break;
                }
            }
            if (this.CurrentSettings.Version_FB < currentDbVersion) {
                this.CurrentSettings.Version_FB = currentDbVersion;
                this.SaveUserSettings();
            }
        }

        private UserSettings GetDefaultSettings() {
            return new UserSettings {
                ID = 1,
                LogPath = string.Empty,
                Theme = 1,
                Visible = true,
                PlayerName = string.Empty,
                FilterType = 1,
                CustomFilterRangeStart = DateTime.MinValue,
                CustomFilterRangeEnd = DateTime.MaxValue,
                SelectedCustomTemplateSeason = -1,
                SelectedProfile = 0,
                OverlayLocationX = null,
                OverlayLocationY = null,
                OverlayFixedPosition = string.Empty,
                OverlayFixedPositionX = null,
                OverlayFixedPositionY = null,
                OverlayFixedWidth = 786,
                OverlayFixedHeight = 134,
                OverlayBackground = 8,
                OverlayBackgroundResourceName = "background_frightful_final_ween",
                OverlayTabResourceName = "tab_unselected_frightful_final_ween",
                OverlayBackgroundOpacity = 100,
                IsOverlayBackgroundCustomized = false,
                OverlayColor = 0,
                LockButtonLocation = 1,
                FlippedDisplay = false,
                FixedFlippedDisplay = false,
                SwitchBetweenLongest = false,
                SwitchBetweenQualify = false,
                SwitchBetweenPlayers = false,
                SwitchBetweenStreaks = true,
                OnlyShowLongest = false,
                OnlyShowGold = false,
                OnlyShowPing = false,
                OnlyShowFinalStreak = false,
                CycleTimeSeconds = 5,
                OverlayVisible = false,
                OverlayNotOnTop = false,
                PlayerByConsoleType = false,
                ColorByRoundType = true,
                AutoChangeProfile = true,
                ShadeTheFlagImage = false,
                DisplayCurrentTime = true,
                DisplayGamePlayedInfo = true,
                CountPlayersDuringTheLevel = true,
                PreviousWins = 0,
                WinsFilter = 1,
                FastestFilter = 1,
                QualifyFilter = 1,
                HideWinsInfo = false,
                HideRoundInfo = false,
                HideTimeInfo = false,
                ShowOverlayTabs = true,
                ShowPercentages = false,
                AutoUpdate = true,
                SystemTrayIcon = false,
                PreventOverlayMouseClicks = false,
                NotifyServerConnected = true,
                NotifyPersonalBest = true,
                MuteNotificationSounds = false,
                NotificationSounds = 0,
                NotificationWindowPosition = 3,
                NotificationWindowAnimation = 0,
                MaximizedWindowState = false,
                FormLocationX = null,
                FormLocationY = null,
                FormWidth = null,
                FormHeight = null,
                OverlayWidth = 786,
                OverlayHeight = 134,
                HideOverlayPercentages = false,
                IgnoreLevelTypeWhenSorting = false,
                RecordEscapeDuringAGame = true,
                GameExeLocation = string.Empty,
                AutoLaunchGameOnStartup = true,
                OverlayFontSerialized = string.Empty,
                OverlayFontColorSerialized = string.Empty,
                WinPerDayGraphStyle = 2,
                ShowChangelog = true,
                IpGeolocationService = 0,
                LevelTimeLimitVersion = 0,
                Version = 0,
                Version_FB = currentDbVersion
            };
        }

        private void UpdateGridRoundName() {
            foreach (KeyValuePair<string, string> item in Multilingual.GetLevelsDictionary().Where(r => r.Key.StartsWith("round_"))) {
                if (this.StatLookup.TryGetValue(item.Key, out LevelStats level)) {
                    level.Name = item.Value;
                }
            }
            this.SortGridDetails(true);
            this.gridDetails.Invalidate();
        }

        public void UpdateDates() {
            if (DateTime.Now.Date.ToUniversalTime() == DayStart) return;

            DateTime currentUtc = DateTime.UtcNow;
            for (int i = Seasons.Count() - 1; i >= 0; i--) {
                if (currentUtc > Seasons[i].StartDate) {
                    SeasonStart = Seasons[i].StartDate;
                    break;
                }
            }
            WeekStart = DateTime.Now.Date.AddDays(-7).ToUniversalTime();
            DayStart = DateTime.Now.Date.ToUniversalTime();

            this.ResetStats();
        }

        public void SaveUserSettings() {
            lock (this.StatsDB) {
                Task saveUserSettingsTask = new Task(() => {
                    this.StatsDB.BeginTrans();
                    this.UserSettings.Update(this.CurrentSettings);
                    this.StatsDB.Commit();
                });
                this.RunDatabaseTask(saveUserSettingsTask, false);
            }
        }

        public void ResetStats() {
            for (int i = 0; i < this.StatDetails.Count; i++) {
                LevelStats calculator = this.StatDetails[i];
                calculator.Clear();
            }

            this.ClearTotals();

            List<RoundInfo> rounds = new List<RoundInfo>();
            int profile = this.GetCurrentProfileId();

            lock (this.AllStats) {
                this.AllStats.Clear();
                this.nextShowID = 0;
                this.lastAddedShow = DateTime.MinValue;
                if (this.RoundDetails.Count() > 0) {
                    this.AllStats.AddRange(this.RoundDetails.FindAll());
                    this.AllStats.Sort();

                    if (this.AllStats.Count > 0) {
                        this.nextShowID = this.AllStats[this.AllStats.Count - 1].ShowID;

                        int lastAddedShowId = -1;
                        for (int i = this.AllStats.Count - 1; i >= 0; i--) {
                            RoundInfo info = this.AllStats[i];
                            info.ToLocalTime();
                            if (info.Profile != profile) continue;

                            if (info.ShowID == lastAddedShowId || (IsInStatsFilter(info) && IsInPartyFilter(info))) {
                                lastAddedShowId = info.ShowID;
                                rounds.Add(info);
                            }

                            if (info.Start > lastAddedShow && info.Round == 1) {
                                this.lastAddedShow = info.Start;
                            }
                        }
                    }
                }
            }

            lock (this.CurrentRound) {
                this.CurrentRound.Clear();
                for (int i = this.AllStats.Count - 1; i >= 0; i--) {
                    RoundInfo info = AllStats[i];
                    if (info.Profile != profile) continue;

                    this.CurrentRound.Insert(0, info);
                    if (info.Round == 1) {
                        break;
                    }
                }
            }

            rounds.Sort();
            this.loadingExisting = true;
            this.LogFile_OnParsedLogLines(rounds);
            this.loadingExisting = false;
        }

        private void mlReportBug_MouseEnter(object sender, EventArgs e) {
            Rectangle rectangle = ((MetroLink)sender).Bounds;
            Point position = new Point(rectangle.Left - 42, rectangle.Top - 32);
            this.AllocTooltip();
            this.ShowTooltip($"{Multilingual.GetWord("report_bug_icon_tooltip")}", this, position);
        }

        private void mlReportBug_MouseLeave(object sender, EventArgs e) {
            this.HideTooltip(this);
        }

        private void menuUsefulThings_Click(object sender, EventArgs e) {
            try {
                if (sender.Equals(this.menuFinalBeansOfficial) || sender.Equals(this.trayFinalBeansOfficial)) {
                    Process.Start(@"https://finalbeans.com/");
                } else if (sender.Equals(this.menuFinalBeansDiscord) || sender.Equals(this.trayFinalBeansDiscord)) {
                    Process.Start(@"https://discord.com/invite/yUwkctsw9F");
                } else if (sender.Equals(this.menuFinalBeansTwitterX) || sender.Equals(this.trayFinalBeansTwitterX)) {
                    Process.Start(@"https://x.com/finalbeansgame");
                } else if (sender.Equals(this.menuFinalBeansTrello) || sender.Equals(this.trayFinalBeansTrello)) {
                    Process.Start(@"https://trello.com/b/S3gVJ6RO");
                } else if (sender.Equals(this.menuRollOffClub) || sender.Equals(this.trayRollOffClub)) {
                    if (CurrentLanguage == Language.Korean) {
                        Process.Start(@"https://rolloff.club/ko/");
                    } else if (CurrentLanguage == Language.Japanese) {
                        Process.Start(@"https://rolloff.club/ja/");
                    } else if (CurrentLanguage == Language.SimplifiedChinese) {
                        Process.Start(@"https://rolloff.club/zh/");
                    } else {
                        Process.Start(@"https://rolloff.club/");
                    }
                } else if (sender.Equals(this.menuLostTempleAnalyzer) || sender.Equals(this.trayLostTempleAnalyzer)) {
                    Process.Start(@"https://alexjlockwood.github.io/lost-temple-analyzer/");
                }
            } catch (Exception ex) {
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuUsefulThings_MouseEnter(object sender, EventArgs e) {
            Rectangle rectangle = this.menuUsefulThings.Bounds;
            Point position = new Point(rectangle.Left, rectangle.Bottom + 148);
            this.AllocCustomTooltip(this.cmtt_center_Draw);
            if (sender.Equals(this.menuFinalBeans)) {
                this.ShowCustomTooltip(Multilingual.GetWord("main_finalbeans_official_tooltip"), this, position);
            } else if (sender.Equals(this.menuRollOffClub)) {
                this.ShowCustomTooltip(Multilingual.GetWord("main_roll_off_club_tooltip"), this, position);
            } else if (sender.Equals(this.menuLostTempleAnalyzer)) {
                this.ShowCustomTooltip(Multilingual.GetWord("main_lost_temple_analyzer_tooltip"), this, position);
            }
        }

        private void menuUsefulThings_MouseLeave(object sender, EventArgs e) {
            this.HideCustomTooltip(this);
            this.Cursor = Cursors.Default;
        }

        private void menuUpdate_MouseEnter(object sender, EventArgs e) {
            Rectangle rectangle = ((ToolStripMenuItem)sender).Bounds;
            Point position = new Point(rectangle.Left, rectangle.Bottom + 68);
            this.AllocTooltip();
            this.ShowTooltip(sender.Equals(this.menuUpdate) && this.isAvailableNewVersion ? $"{Multilingual.GetWord("main_you_can_update_new_version_prefix_tooltip")}v{this.availableNewVersion}{Multilingual.GetWord("main_you_can_update_new_version_suffix_tooltip")}" :
                $"{Multilingual.GetWord("main_update_prefix_tooltip")}{Environment.NewLine}{Multilingual.GetWord("main_update_suffix_tooltip")}",
                this, position);
        }

        private void menuUpdate_MouseLeave(object sender, EventArgs e) {
            this.HideTooltip(this);
            this.Cursor = Cursors.Default;
        }

        private void menuOverlay_MouseEnter(object sender, EventArgs e) {
            this.Cursor = Cursors.Hand;
            Rectangle rectangle = this.menuOverlay.Bounds;
            Point position = new Point(rectangle.Left, rectangle.Bottom + 68);
            this.AllocCustomTooltip(this.cmtt_overlay_Draw);
            this.ShowCustomTooltip($"{Multilingual.GetWord(this.overlay.Visible ? "main_overlay_hide_tooltip" : "main_overlay_show_tooltip")}{Environment.NewLine}{Multilingual.GetWord("main_overlay_shortcut_tooltip")}", this, position);
        }

        private void menuOverlay_MouseLeave(object sender, EventArgs e) {
            this.HideCustomTooltip(this);
            this.Cursor = Cursors.Default;
        }

        private void setCursor_MouseMove(object sender, MouseEventArgs e) {
            this.Cursor = Cursors.Hand;
        }

        private void setCursor_MouseLeave(object sender, EventArgs e) {
            this.Cursor = Cursors.Default;
        }

        private void trayCMenu_Closing(object sender, ToolStripDropDownClosingEventArgs e) {
            this.trayCMenu.Opacity = 0;
        }

        private void trayCMenu_Opening(object sender, CancelEventArgs e) {
            this.trayCMenu.Opacity = 100;
        }

        private void trayIcon_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                int menuPositionX = 0, menuPositionY = 0;
                switch (this.GetTaskbarPosition()) {
                    case TaskbarPosition.Bottom:
                        if (MousePosition.Y >= Screen.GetWorkingArea(MousePosition).Height) {
                            menuPositionX = MousePosition.X;
                            menuPositionY = Screen.GetWorkingArea(MousePosition).Height - this.trayCMenu.Height;
                        } else {
                            menuPositionX = MousePosition.X + 5;
                            menuPositionY = this.trayCMenu.Location.Y - 5;
                        }
                        break;
                    case TaskbarPosition.Left:
                        if (MousePosition.X <= (Screen.GetBounds(MousePosition).Width - Screen.GetWorkingArea(MousePosition).Width)) {
                            menuPositionX = Screen.GetBounds(MousePosition).Width - Screen.GetWorkingArea(MousePosition).Width;
                            menuPositionY = this.trayCMenu.Location.Y;
                        } else {
                            menuPositionX = MousePosition.X + 5;
                            menuPositionY = this.trayCMenu.Location.Y - 5;
                        }
                        break;
                    case TaskbarPosition.Right:
                        if (MousePosition.X >= Screen.GetWorkingArea(MousePosition).Width) {
                            menuPositionX = Screen.GetWorkingArea(MousePosition).Width - this.trayCMenu.Width;
                            menuPositionY = MousePosition.Y - this.trayCMenu.Height;
                        } else {
                            menuPositionX = MousePosition.X - this.trayCMenu.Width - 5;
                            menuPositionY = this.trayCMenu.Location.Y - 5;
                        }
                        break;
                    case TaskbarPosition.Top:
                        if (MousePosition.Y <= (Screen.GetBounds(MousePosition).Height - Screen.GetWorkingArea(MousePosition).Height)) {
                            menuPositionX = MousePosition.X - this.trayCMenu.Width;
                            menuPositionY = Screen.GetBounds(MousePosition).Height - Screen.GetWorkingArea(MousePosition).Height;
                        } else {
                            menuPositionX = MousePosition.X - this.trayCMenu.Width - 5;
                            menuPositionY = this.trayCMenu.Location.Y + 5;
                        }
                        break;
                }
                this.trayCMenu.Location = new Point(menuPositionX, menuPositionY);
            }
        }

        private void trayIcon_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                if (this.Visible && this.WindowState == FormWindowState.Minimized) {
                    this.isFocused = true;
                    this.WindowState = this.maximizedForm ? FormWindowState.Maximized : FormWindowState.Normal;
                    this.TopMost = true;
                    this.TopMost = false;
                    Utils.SetForegroundWindow(Utils.FindWindow(null, this.mainWndTitle));
                } else if (this.Visible && this.WindowState != FormWindowState.Minimized) {
                    if (this.isFocused) {
                        this.isFocused = false;
                        this.Hide();
                    } else {
                        this.isFocused = true;
                        this.TopMost = true;
                        this.TopMost = false;
                        Utils.SetForegroundWindow(Utils.FindWindow(null, this.mainWndTitle));
                    }
                } else {
                    this.isFocused = true;
                    this.Show();
                }
            }
        }

        private void trayIcon_MouseMove(object sender, MouseEventArgs e) {
            this.isFocused = ActiveForm == this;
        }

        private void Stats_VisibleChanged(object sender, EventArgs e) {
            if (this.Visible) {
                this.TopMost = true;
                this.TopMost = false;
                Utils.SetForegroundWindow(Utils.FindWindow(null, this.mainWndTitle));
                this.SetMainDataGridViewOrder();
            }
        }

        private void Stats_Resize(object sender, EventArgs e) {
            this.isFocused = true;
            if (this.WindowState == FormWindowState.Maximized) {
                this.maximizedForm = true;
            } else if (this.WindowState == FormWindowState.Normal) {
                this.maximizedForm = false;
            }
        }

        public void SaveWindowState() {
            this.CurrentSettings.Visible = this.Visible;

            if (this.WindowState != FormWindowState.Normal) {
                this.CurrentSettings.FormLocationX = this.RestoreBounds.Location.X;
                this.CurrentSettings.FormLocationY = this.RestoreBounds.Location.Y;
                this.CurrentSettings.FormWidth = this.RestoreBounds.Size.Width;
                this.CurrentSettings.FormHeight = this.RestoreBounds.Size.Height;
                this.CurrentSettings.MaximizedWindowState = this.maximizedForm;
            } else {
                this.CurrentSettings.FormLocationX = this.Location.X;
                this.CurrentSettings.FormLocationY = this.Location.Y;
                this.CurrentSettings.FormWidth = this.Size.Width;
                this.CurrentSettings.FormHeight = this.Size.Height;
                this.CurrentSettings.MaximizedWindowState = false;
            }
        }

        public void Stats_ExitProgram(object sender, EventArgs e) {
            try {
#if AllowUpdate
                if (IsExitingForUpdate) {
                    this.trayIcon.Visible = false;
                    this.CurrentSettings.ShowChangelog = true;
                }

                if (!IsUpdatingOnAppLaunch && !this.IsDisposed && !this.Disposing) {
                    this.SaveWindowState();
                }
#else
                if (!this.IsDisposed && !this.Disposing) {
                    this.SaveWindowState();
                }
#endif
                this.SaveUserSettings();

                IsExitingProgram = true;
                this.Hide();
                this.overlay?.Dispose();

                if (this.logFile.logFileWatcher != null) {
                    Task.Run(() => this.logFile.Stop()).Wait();
                }

                lock (this.dbTaskLock) {
                    Task.WaitAll(this.dbTasks.ToArray());
                    this.StatsDB?.Dispose();
                }
#if AllowUpdate
                if (IsExitingForUpdate) return;
#endif
            } catch (Exception ex) {
                IsExitingProgram = true;
                lock (this.dbTaskLock) {
                    Task.WaitAll(this.dbTasks.ToArray());
                    this.StatsDB?.Dispose();
                }
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
#if AllowUpdate
                if (IsExitingForUpdate) return;
#endif
            }
            this.Close();
        }

        private void Stats_FormClosing(object sender, FormClosingEventArgs e) {
            if (IsExitingProgram) return;

            if (this.CurrentSettings.SystemTrayIcon) {
                this.Hide();
                e.Cancel = true;
            } else {
                try {
                    if (!this.IsDisposed && !this.Disposing) {
                        this.SaveWindowState();
                    }
                    this.SaveUserSettings();
                    IsExitingProgram = true;
                    this.Hide();
                    this.overlay?.Dispose();
                    if (this.logFile.logFileWatcher != null) {
                        Task.Run(() => this.logFile.Stop()).Wait();
                    }
                    lock (this.dbTaskLock) {
                        Task.WaitAll(this.dbTasks.ToArray());
                        this.StatsDB?.Dispose();
                    }
                } catch (Exception ex) {
                    IsExitingProgram = true;
                    lock (this.dbTaskLock) {
                        Task.WaitAll(this.dbTasks.ToArray());
                        this.StatsDB?.Dispose();
                    }
                    MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Stats_Load(object sender, EventArgs e) {
            try {
                InstalledEmojiFont = Utils.IsFontInstalled("Segoe UI Emoji");

                this.SetTheme(CurrentTheme);
                this.infoStrip.Renderer = new CustomToolStripSystemRenderer();
                this.infoStrip2.Renderer = new CustomToolStripSystemRenderer();
                this.infoStrip3.Renderer = new CustomToolStripSystemRenderer();

                this.UpdateDates();
            } catch (Exception ex) {
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Stats_Shown(object sender, EventArgs e) {
            try {
#if AllowUpdate
                if (Utils.IsInternetConnected()) {
                    if (this.CurrentSettings.AutoUpdate) {
                        this.EnableInfoStrip(false);
                        this.EnableMainMenu(false);
                        if (this.CheckForUpdate(true)) {
                            return;
                        }
                        this.EnableInfoStrip(true);
                        this.EnableMainMenu(true);
                    } else {
                        this.CheckForNewVersion();
                        this.CheckForNewVersionJob();
                    }
                    if (this.CurrentSettings.ShowChangelog) {
                        using (var webClient = new WebClient()) {
                            try {
                                Version version = Assembly.GetEntryAssembly().GetName().Version;
                                // string changeLogContent = File.ReadAllText(Path.Combine(CURRENTDIR, "CHANGELOG.md"));
                                string changeLogContent = webClient.DownloadString(Utils.FINALBEANSSTATS_RELEASES_CHANGELOG_URL);
                                string[] changeLogLines = changeLogContent.Split(new[] { '\n' });
                                List<string> changeLog = new List<string>();
                                foreach (string lineInfo in changeLogLines) {
                                    if (lineInfo.StartsWith($"## v{version.ToString(2)}")) {
                                        for (int i = Array.IndexOf(changeLogLines, lineInfo) + 1; i < changeLogLines.Length; i++) {
                                            if (changeLogLines[i].StartsWith("#")) {
                                                break;
                                            }
                                            changeLog.Add(changeLogLines[i]);
                                        }
                                        break;
                                    }
                                }
                                if (changeLog.Count == 0) {
                                    throw new Exception("No changelog found for your current version.");
                                }

                                MetroMessageBox.Show(this, $"{Environment.NewLine}" +
                                                           $"{string.Join("", changeLog)}" +
                                                           $"{Environment.NewLine}{Environment.NewLine}" +
                                                           $"{Multilingual.GetWord("main_update_prefix_tooltip").Trim()}" +
                                                           $"{Environment.NewLine}" +
                                                           $"{Multilingual.GetWord("main_update_suffix_tooltip").Trim()}",
                                                           $"{Multilingual.GetWord("message_changelog_caption")} - {Multilingual.GetWord("main_finalbeans_stats")} v{Assembly.GetExecutingAssembly().GetName().Version.ToString(2)}",
                                                     MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                this.CurrentSettings.ShowChangelog = false;
                                this.SaveUserSettings();
                            } catch {
                                // ignored
                            }
                        }
                    }
                } else {
                    this.CheckForNewVersionJob(true);
                }
#endif
                this.ReloadProfileMenuItems();

                string logPath = !string.IsNullOrEmpty(this.CurrentSettings.LogPath) && Directory.Exists(this.CurrentSettings.LogPath) ? this.CurrentSettings.LogPath : LOGPATH;
                this.logFile.Start(logPath, LOGFILENAME);

                this.SetWindowCorner();
                this.SetMainDataGridViewOrder();
                this.scrollTimer.Tick += this.scrollTimer_Tick;

                if (this.CurrentSettings.FormWidth.HasValue) {
                    this.Size = new Size(this.CurrentSettings.FormWidth.Value, this.CurrentSettings.FormHeight.Value);
                }
                if (this.CurrentSettings.FormLocationX.HasValue && Utils.IsOnScreen(this.CurrentSettings.FormLocationX.Value, this.CurrentSettings.FormLocationY.Value, this.Width, this.Height)) {
                    this.Location = new Point(this.CurrentSettings.FormLocationX.Value, this.CurrentSettings.FormLocationY.Value);
                }

                this.WindowState = this.CurrentSettings.MaximizedWindowState ? FormWindowState.Maximized : FormWindowState.Normal;

                if (this.CurrentSettings.AutoLaunchGameOnStartup) {
                    this.EnableInfoStrip(false);
                    this.EnableMainMenu(false);
                    this.LaunchGame(true);
                    this.EnableInfoStrip(true);
                    this.EnableMainMenu(true);
                }

                if (this.CurrentSettings.SystemTrayIcon || this.WindowState != FormWindowState.Minimized) {
                    this.WindowState = this.CurrentSettings.MaximizedWindowState ? FormWindowState.Maximized : FormWindowState.Normal;
                }

                this.ShowInTaskbar = true;
                this.SetSystemTrayIcon(this.CurrentSettings.SystemTrayIcon);

                this.Hide();
                if (this.CurrentSettings.Visible) {
                    this.Show();
                }
                this.Opacity = 1;

                this.overlay.UpdateDisplay(true);
                if (this.CurrentSettings.OverlayVisible) {
                    this.ToggleOverlay(this.overlay, false);
                }

                this.selectedCustomTemplateSeason = this.CurrentSettings.SelectedCustomTemplateSeason;
                this.customfilterRangeStart = this.CurrentSettings.CustomFilterRangeStart;
                this.customfilterRangeEnd = this.CurrentSettings.CustomFilterRangeEnd;
                this.menuAllStats.Checked = false;
                this.trayAllStats.Checked = false;
                switch (this.CurrentSettings.FilterType) {
                    case 0:
                        this.menuCustomRangeStats.Checked = true;
                        this.trayCustomRangeStats.Checked = true;
                        this.menuStats_Click(this.menuCustomRangeStats, EventArgs.Empty);
                        break;
                    case 1:
                        this.menuAllStats.Checked = true;
                        this.trayAllStats.Checked = true;
                        this.menuStats_Click(this.menuAllStats, EventArgs.Empty);
                        break;
                    case 2:
                        this.menuSeasonStats.Checked = true;
                        this.traySeasonStats.Checked = true;
                        this.menuStats_Click(this.menuSeasonStats, EventArgs.Empty);
                        break;
                    case 3:
                        this.menuWeekStats.Checked = true;
                        this.trayWeekStats.Checked = true;
                        this.menuStats_Click(this.menuWeekStats, EventArgs.Empty);
                        break;
                    case 4:
                        this.menuDayStats.Checked = true;
                        this.trayDayStats.Checked = true;
                        this.menuStats_Click(this.menuDayStats, EventArgs.Empty);
                        break;
                    case 5:
                        this.menuSessionStats.Checked = true;
                        this.traySessionStats.Checked = true;
                        this.menuStats_Click(this.menuSessionStats, EventArgs.Empty);
                        break;
                }
                this.isStartingUp = false;
            } catch (Exception ex) {
                this.EnableInfoStrip(true);
                this.EnableMainMenu(true);
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LogFile_OnError(string error) {
            if (!this.Disposing && !this.IsDisposed) {
                try {
                    if (this.InvokeRequired) {
                        this.Invoke((Action<string>)LogFile_OnError, error);
                    } else {
                        MetroMessageBox.Show(this, error, $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } catch {
                    // ignored
                }
            }
        }

        private void LogFile_OnPersonalBestNotification(string showNameId, string roundId, double currentPb, double currentRecord) {
            string timeDiffContent = string.Empty;
            if (currentPb > 0) {
                TimeSpan timeDiff = TimeSpan.FromMilliseconds(currentPb - currentRecord);
                timeDiffContent = timeDiff.Minutes > 0 ? $" ⏱️{Multilingual.GetWord("message_new_personal_best_timediff_by_minute_prefix")}{timeDiff.Minutes}{Multilingual.GetWord("message_new_personal_best_timediff_by_minute_infix")} {timeDiff.Seconds}.{timeDiff.Milliseconds}{Multilingual.GetWord("message_new_personal_best_timediff_by_minute_suffix")}"
                                  : $" ⏱️{timeDiff.Seconds}.{timeDiff.Milliseconds}{Multilingual.GetWord("message_new_personal_best_timediff_by_second")}";
            }
            string levelName = this.StatLookup.TryGetValue(roundId, out LevelStats l1) ? l1.Name : roundId.Substring(0, roundId.Length - 3);
            string showName = $"{(string.Equals(Multilingual.GetShowName(showNameId), levelName) ? $"({levelName})" : $"({Multilingual.GetShowName(showNameId)} • {levelName})")}";
            string description = $"{Multilingual.GetWord("message_new_personal_best_prefix")}{showName}{Multilingual.GetWord("message_new_personal_best_suffix")}{timeDiffContent}";
            ToastPosition toastPosition = Enum.TryParse(this.CurrentSettings.NotificationWindowPosition.ToString(), out ToastPosition position) ? position : ToastPosition.BottomRight;
            ToastTheme toastTheme = this.Theme == MetroThemeStyle.Light ? ToastTheme.Light : ToastTheme.Dark;
            ToastAnimation toastAnimation = this.CurrentSettings.NotificationWindowAnimation == 0 ? ToastAnimation.FADE : ToastAnimation.SLIDE;
            ToastSound toastSound = Enum.TryParse(this.CurrentSettings.NotificationSounds.ToString(), out ToastSound sound) ? sound : ToastSound.Generic01;
            this.ShowToastNotification(this, null, Multilingual.GetWord("message_new_personal_best_caption"), description, Overlay.GetMainFont(16, FontStyle.Bold, CurrentLanguage),
                null, ToastDuration.MEDIUM, toastPosition, toastAnimation, toastTheme, toastSound, this.CurrentSettings.MuteNotificationSounds, true);
        }

        private void LogFile_OnServerConnectionNotification() {
            string countryFullName;
            if (!string.IsNullOrEmpty(LastCountryAlpha2Code)) {
                countryFullName = Multilingual.GetCountryName(LastCountryAlpha2Code);
                if (!string.IsNullOrEmpty(LastCountryRegion)) {
                    countryFullName += $" ({LastCountryRegion}";
                    if (!string.IsNullOrEmpty(LastCountryCity)) {

                        if (!string.Equals(LastCountryCity, LastCountryRegion)) {
                            countryFullName += $", {LastCountryCity})";
                        } else {
                            countryFullName += ")";
                        }
                    } else {
                        countryFullName += ")";
                    }
                } else {
                    if (!string.IsNullOrEmpty(LastCountryCity)) {
                        countryFullName += $" ({LastCountryCity})";
                    }
                }
            } else {
                countryFullName = "UNKNOWN";
            }
            string description = $"{Multilingual.GetWord("message_connected_to_server_prefix")}{countryFullName}{Multilingual.GetWord("message_connected_to_server_suffix")}";
            Image flagImage = (Image)Properties.Resources.ResourceManager.GetObject($"country_{(string.IsNullOrEmpty(LastCountryAlpha2Code) ? "unknown" : LastCountryAlpha2Code)}{(this.CurrentSettings.ShadeTheFlagImage ? "_shiny" : "")}_icon");
            ToastPosition toastPosition = Enum.TryParse(this.CurrentSettings.NotificationWindowPosition.ToString(), out ToastPosition position) ? position : ToastPosition.BottomRight;
            ToastTheme toastTheme = this.Theme == MetroThemeStyle.Light ? ToastTheme.Light : ToastTheme.Dark;
            ToastAnimation toastAnimation = this.CurrentSettings.NotificationWindowAnimation == 0 ? ToastAnimation.FADE : ToastAnimation.SLIDE;
            ToastSound toastSound = Enum.TryParse(this.CurrentSettings.NotificationSounds.ToString(), out ToastSound sound) ? sound : ToastSound.Generic01;
            this.ShowToastNotification(this, null, Multilingual.GetWord("message_connected_to_server_caption"), description, Overlay.GetMainFont(16, FontStyle.Bold, CurrentLanguage),
                flagImage, ToastDuration.MEDIUM, toastPosition, toastAnimation, toastTheme, toastSound, this.CurrentSettings.MuteNotificationSounds, true);
        }

        private void LogFile_OnNewLogFileDate(DateTime newDate) {
            if (SessionStart != newDate) {
                SessionStart = newDate;
                if (this.menuSessionStats.Checked) {
                    this.menuStats_Click(this.menuSessionStats, EventArgs.Empty);
                }
            }
        }

        private void LogFile_OnParsedLogLinesCurrent(List<RoundInfo> round) {
            lock (this.CurrentRound) {
                if (this.CurrentRound == null || this.CurrentRound.Count != round.Count) {
                    this.CurrentRound = round;
                } else {
                    for (int i = 0; i < this.CurrentRound.Count; i++) {
                        RoundInfo info = this.CurrentRound[i];
                        if (!info.Equals(round[i])) {
                            this.CurrentRound = round;
                            break;
                        }
                    }
                }
            }
        }

        private void LogFile_OnParsedLogLines(List<RoundInfo> round) {
            try {
                if (this.InvokeRequired) {
                    this.Invoke((Action<List<RoundInfo>>)this.LogFile_OnParsedLogLines, round);
                    return;
                }

                lock (this.StatsDB) {
                    if (!this.loadingExisting) { this.StatsDB.BeginTrans(); }

                    int profile = this.currentProfile;
                    foreach (var stat in round) {
                        if (!this.loadingExisting) {
                            RoundInfo info = null;
                            for (int i = this.AllStats.Count - 1; i >= 0; i--) {
                                RoundInfo temp = this.AllStats[i];
                                if (temp.Start == stat.Start && temp.Name == stat.Name) {
                                    info = temp;
                                    break;
                                }
                            }

                            if (info == null && stat.Start > this.lastAddedShow) {
                                if (stat.ShowEnd < this.startupTime && this.askedPreviousShows == 0) {
                                    IsDisplayOverlayTime = false;
                                    this.EnableInfoStrip(false);
                                    this.EnableMainMenu(false);
                                    using (EditShows editShows = new EditShows()) {
                                        editShows.FunctionFlag = "add";
                                        editShows.Profiles = this.AllProfiles;
                                        editShows.StatsForm = this;
                                        if (editShows.ShowDialog(this) == DialogResult.OK) {
                                            this.askedPreviousShows = 1;
                                            if (editShows.UseLinkedProfiles) {
                                                this.useLinkedProfiles = true;
                                            } else {
                                                profile = editShows.SelectedProfileId;
                                                this.CurrentSettings.SelectedProfile = profile;
                                                this.SetProfileMenu(profile);
                                            }
                                        } else {
                                            this.askedPreviousShows = 2;
                                        }
                                    }
                                    this.EnableInfoStrip(true);
                                    this.EnableMainMenu(true);
                                    IsDisplayOverlayTime = true;
                                }

                                if (stat.ShowEnd < this.startupTime && this.askedPreviousShows == 2) {
                                    continue;
                                }

                                if (stat.ShowEnd < this.startupTime && this.useLinkedProfiles) {
                                    profile = this.GetLinkedProfileId(stat.ShowNameId, stat.PrivateLobby);
                                    this.CurrentSettings.SelectedProfile = profile;
                                    this.SetProfileMenu(profile);
                                }

                                if (stat.Round == 1) {
                                    this.nextShowID++;
                                    this.lastAddedShow = stat.Start;
                                }
                                stat.ShowID = this.nextShowID;
                                stat.Profile = profile;

                                this.RoundDetails.Insert(stat);
                                this.AllStats.Add(stat);
                            } else {
                                if (this.CurrentSettings.AutoChangeProfile) {
                                    profile = this.GetLinkedProfileId(stat.ShowNameId, stat.PrivateLobby);
                                    this.CurrentSettings.SelectedProfile = profile;
                                    this.SetProfileMenu(profile);
                                }
                                continue;
                            }
                        }

                        if (stat.PrivateLobby) {
                            if (stat.Round == 1) {
                                this.CustomShows++;
                            }
                            this.CustomRounds++;
                        } else {
                            if (stat.Round == 1) {
                                this.Shows++;
                            }
                            this.Rounds++;
                        }
                        this.Duration += stat.End - stat.Start;

                        if (stat.PrivateLobby) {
                            if (stat.Qualified) {
                                switch (stat.Tier) {
                                    case 0:
                                        this.CustomPinkMedals++;
                                        break;
                                    case 1:
                                        this.CustomGoldMedals++;
                                        break;
                                    case 2:
                                        this.CustomSilverMedals++;
                                        break;
                                    case 3:
                                        this.CustomBronzeMedals++;
                                        break;
                                }
                            } else {
                                this.CustomEliminatedMedals++;
                            }
                        } else {
                            if (stat.Qualified) {
                                switch (stat.Tier) {
                                    case 0:
                                        this.PinkMedals++;
                                        break;
                                    case 1:
                                        this.GoldMedals++;
                                        break;
                                    case 2:
                                        this.SilverMedals++;
                                        break;
                                    case 3:
                                        this.BronzeMedals++;
                                        break;
                                }
                            } else {
                                this.EliminatedMedals++;
                            }
                        }

                        this.Kudos += stat.Kudos;

                        // add new type of round to the rounds lookup
                        if (!this.StatLookup.ContainsKey(stat.Name)) {
                            string roundName = stat.Name.StartsWith("round_") ? stat.Name.Substring(6).Replace('_', ' ')
                                                                              : stat.Name.Replace('_', ' ');

                            LevelStats newLevel = new LevelStats(stat.Name, this.textInfo.ToTitleCase(roundName), LevelType.Unknown, BestRecordType.Fastest, false, CURRENTSEASON, Properties.Resources.round_unknown_icon, Properties.Resources.round_unknown_big_icon);
                            this.StatLookup.Add(stat.Name, newLevel);
                            this.StatDetails.Add(newLevel);
                            this.gridDetails.DataSource = null;
                            // this.gridDetails.DataSource = this.StatDetails;
                            this.gridDetails.DataSource = this.StatDetails;
                        }

                        stat.ToLocalTime();

                        if (!stat.PrivateLobby) {
                            if (stat.IsFinal || stat.Crown) {
                                this.Finals++;
                                if (stat.Qualified) {
                                    this.Wins++;
                                }
                            }
                        }

                        if (this.StatLookup.TryGetValue(stat.Name, out LevelStats levelStats)) {
                            levelStats.Increase(stat, this.profileWithLinkedCustomShow == stat.Profile);
                            levelStats.Add(stat);
                        }
                    }

                    if (!this.loadingExisting) { this.StatsDB.Commit(); }
                    this.OnUpdatedLevelRows?.Invoke();
                }

                lock (this.CurrentRound) {
                    this.CurrentRound.Clear();
                    for (int i = round.Count - 1; i >= 0; i--) {
                        RoundInfo info = round[i];
                        this.CurrentRound.Insert(0, info);
                        if (info.Round == 1) {
                            break;
                        }
                    }
                }

                if (!this.Disposing && !this.IsDisposed) {
                    try {
                        this.UpdateTotals();
                    } catch {
                        // ignored
                    }
                }

                IsOverlayRoundInfoNeedRefresh = true;
            } catch (Exception ex) {
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // public bool ExistsPersonalBestLog(DateTime pbDate) {
        //     return this.PersonalBestLog.Exists(Query.EQ("_id", pbDate));
        // }

        public bool ExistsPersonalBestLog(DateTime pbDate) {
            return this.PersonalBestLogCache.Exists(l => l.PbDate == pbDate);
        }

        // public PersonalBestLog SelectPersonalBestLog(string sessionId, string showId, string roundId) {
        //     BsonExpression condition = Query.And(
        //         Query.EQ("SessionId", sessionId),
        //         Query.EQ("ShowId", showId),
        //         Query.EQ("RoundId", roundId)
        //     );
        //     return this.PersonalBestLog.FindOne(condition);
        // }

        public void InsertPersonalBestLog(DateTime finish, string showId, string roundId, double record, bool isPb) {
            lock (this.StatsDB) {
                PersonalBestLog log = new PersonalBestLog {
                    PbDate = finish, ShowId = showId, RoundId = roundId, Record = record, IsPb = isPb
                };
                this.PersonalBestLogCache.Add(log);
                Task insertPersonalBestTask = new Task(() => {
                    this.StatsDB.BeginTrans();
                    this.PersonalBestLog.Insert(log);
                    this.StatsDB.Commit();
                });
                this.RunDatabaseTask(insertPersonalBestTask, false);
            }
        }

        // private void UpsertPersonalBestLog(string sessionId, string showId, string roundId, double record, DateTime finish, bool isPb) {
        //     lock (this.StatsDB) {
        //         Task upsertPersonalBestTask = new Task(() => {
        //             this.StatsDB.BeginTrans();
        //             this.PersonalBestLog.Upsert(new PersonalBestLog {
        //                 PbDate = finish, ShowId = showId, RoundId = roundId, Record = record, IsPb = isPb
        //             });
        //             this.StatsDB.Commit();
        //         });
        //         this.RunDatabaseTask(upsertPersonalBestTask, false);
        //     }
        // }

        // private void ClearUnsavedPersonalBestLog() {
        //     lock (this.PersonalBestLogCache) {
        //         bool isCacheUpdated = false;
        //         List<DateTime> allPbDateLogs = this.PersonalBestLogCache.FindAll(l => l.IsPb).Select(l => l.PbDate).ToList();
        //         foreach (DateTime pbDate in allPbDateLogs) {
        //             if (!this.RoundDetails.Exists(r => r.Finish.HasValue && r.Finish.Value == pbDate)) {
        //                 PersonalBestLog pbLog = this.PersonalBestLogCache.Find(l => l.PbDate == pbDate);
        //                 this.PersonalBestLogCache.Remove(pbLog);
        //                 isCacheUpdated = true;
        //             }
        //         }
        //         if (isCacheUpdated) {
        //             Task clearUnsavedPersonalBestTask = new Task(() => {
        //                 this.StatsDB.BeginTrans();
        //                 this.PersonalBestLog.DeleteAll();
        //                 this.PersonalBestLog.InsertBulk(this.PersonalBestLogCache);
        //                 this.StatsDB.Commit();
        //             });
        //             this.RunDatabaseTask(clearUnsavedPersonalBestTask, false);
        //         }
        //     }
        // }

        private void ClearPersonalBestLog(int days) {
            lock (this.StatsDB) {
                DateTime daysCond = DateTime.Now.AddDays(days * -1);
                BsonExpression condition = Query.LT("_id", daysCond);
                Task clearPersonalBestTask = new Task(() => {
                    this.StatsDB.BeginTrans();
                    this.PersonalBestLog.DeleteMany(condition);
                    this.StatsDB.Commit();
                });
                this.RunDatabaseTask(clearPersonalBestTask, false);
            }
        }

        private void ClearServerConnectionLog(int days) {
            lock (this.StatsDB) {
                DateTime daysCond = DateTime.Now.AddDays(days * -1);
                BsonExpression condition = Query.LT("ConnectionDate", daysCond);
                Task clearServerConnectionTask = new Task(() => {
                    this.StatsDB.BeginTrans();
                    this.ServerConnectionLog.DeleteMany(condition);
                    this.StatsDB.Commit();
                });
                this.RunDatabaseTask(clearServerConnectionTask, false);
            }
        }

        public bool ExistsServerConnectionLog(string sessionId) {
            if (string.IsNullOrEmpty(sessionId)) return false;
            // BsonExpression condition = Query.And(
            //     Query.EQ("_id", sessionId),
            //     Query.EQ("ShowId", showId)
            // );
            // return this.ServerConnectionLog.Exists(condition);
            return this.ServerConnectionLogCache.Exists(l => string.Equals(l.SessionId, sessionId));
        }

        public ServerConnectionLog SelectServerConnectionLog(string sessionId) {
            // BsonExpression condition = Query.And(
            //     Query.EQ("_id", sessionId),
            //     Query.EQ("ShowId", showId)
            // );
            // return this.ServerConnectionLog.FindOne(condition);
            return this.ServerConnectionLogCache.Find(l => string.Equals(l.SessionId, sessionId));
        }

        public void InsertServerConnectionLog(string sessionId, string showId, string serverIp, DateTime connectionDate, bool isNotify, bool isPlaying) {
            lock (this.StatsDB) {
                ServerConnectionLog log = new ServerConnectionLog {
                    SessionId = sessionId, ShowId = showId, ServerIp = serverIp, ConnectionDate = connectionDate,
                    IsNotify = isNotify, IsPlaying = isPlaying
                };
                this.ServerConnectionLogCache.Add(log);
                Task insertServerConnectionTask = new Task(() => {
                    this.StatsDB.BeginTrans();
                    this.ServerConnectionLog.Insert(log);
                    this.StatsDB.Commit();
                });
                this.RunDatabaseTask(insertServerConnectionTask, false);
            }
        }

        public void UpdateServerConnectionLog(string sessionId, bool isPlaying) {
            lock (this.ServerConnectionLogCache) {
                ServerConnectionLog log = this.SelectServerConnectionLog(sessionId);
                if (log != null && !Equals(log.IsPlaying, isPlaying)) {
                    this.ServerConnectionLogCache.Remove(log);
                    log.IsPlaying = isPlaying;
                    this.ServerConnectionLogCache.Add(log);
                    lock (this.StatsDB) {
                        Task updateServerConnectionTask = new Task(() => {
                            this.StatsDB.BeginTrans();
                            this.ServerConnectionLog.Update(log);
                            this.StatsDB.Commit();
                        });
                        this.RunDatabaseTask(updateServerConnectionTask, false);
                    }
                }
            }
        }

        // public void UpsertServerConnectionLog(string sessionId, string showNameId, string serverIp, DateTime connectionDate, bool isNotify, bool isPlaying) {
        //     lock (this.StatsDB) {
        //         Task upsertServerConnectionTask = new Task(() => {
        //             this.StatsDB.BeginTrans();
        //             this.ServerConnectionLog.Upsert(new ServerConnectionLog {
        //                 SessionId = sessionId, ShowId = showNameId, ServerIp = serverIp, ConnectionDate = connectionDate,
        //                 IsNotify = isNotify, IsPlaying = isPlaying
        //             });
        //             this.StatsDB.Commit();
        //         });
        //         this.RunDatabaseTask(upsertServerConnectionTask, false);
        //     }
        // }

        private bool IsInStatsFilter(RoundInfo info) {
            return (this.menuCustomRangeStats.Checked && info.Start >= this.customfilterRangeStart && info.Start <= this.customfilterRangeEnd)
                   || this.menuAllStats.Checked
                   || (this.menuSeasonStats.Checked && info.Start > SeasonStart)
                   || (this.menuWeekStats.Checked && info.Start > WeekStart)
                   || (this.menuDayStats.Checked && info.Start > DayStart)
                   || (this.menuSessionStats.Checked && info.Start > SessionStart);
        }

        private bool IsInPartyFilter(RoundInfo info) {
            return this.menuAllPartyStats.Checked
                   || (this.menuSoloStats.Checked && !info.InParty)
                   || (this.menuPartyStats.Checked && info.InParty);
        }

        public string GetCurrentFilterName() {
            if (this.menuCustomRangeStats.Checked && this.selectedCustomTemplateSeason > -1) {
                return Seasons[this.selectedCustomTemplateSeason].Name;
            } else {
                return this.menuCustomRangeStats.Checked ? Multilingual.GetWord("main_custom_range") :
                       this.menuAllStats.Checked ? Multilingual.GetWord("main_all") :
                       this.menuSeasonStats.Checked ? Multilingual.GetWord("main_season") :
                       this.menuWeekStats.Checked ? Multilingual.GetWord("main_week") :
                       this.menuDayStats.Checked ? Multilingual.GetWord("main_day") : Multilingual.GetWord("main_session");
            }
        }

        public string GetCurrentProfileName() {
            if (this.AllProfiles.Count == 0) return String.Empty;
            return this.AllProfiles.Find(p => p.ProfileId == this.GetCurrentProfileId()).ProfileName;
        }

        public int GetCurrentProfileId() {
            return this.currentProfile;
        }

        private int GetProfileIdByName(string profileName) {
            if (this.AllProfiles.Count == 0 || string.IsNullOrEmpty(profileName)) return 0;
            return this.AllProfiles.Find(p => string.Equals(p.ProfileName, profileName)).ProfileId;
        }

        private string GetCurrentProfileLinkedShowId() {
            if (this.AllProfiles.Count == 0) return String.Empty;
            string currentProfileLinkedShowId = this.AllProfiles.Find(p => p.ProfileId == this.GetCurrentProfileId()).LinkedShowId;
            return currentProfileLinkedShowId ?? string.Empty;
        }

        public string GetAlternateShowId(string showId) {
            switch (showId) {
                case "event_day_at_the_races_ltm":
                    return "event_only_races_any_final_template";
                case "event_le_anchovy_private_lobbies":
                    return "event_le_anchovy_template";
                case "event_only_jump_club_custom_lobby":
                    return "event_only_jump_club_template";
                case "event_only_roll_out_custom_lobby":
                    return "event_only_roll_out";
                case "knockout_mode_pl":
                    return "knockout_mode";
                case "live_event_timeattack_shuffle_pl":
                    return "live_event_timeattack_shuffle";
                case "pl_duos_show":
                    return "classic_duos_show";
                case "pl_solo_main_show":
                    return "classic_solo_main_show";
                case "pl_squads_show":
                    return "classic_squads_show";
                default:
                    return showId;
            }
        }

        public string GetMainGroupShowId(string showId) {
            switch (showId) {
                case "fb_frightful_final_ween":
                case "fb_skilled_speeders":
                    return "fb_ltm";
                case "ranked_show_knockout":
                case "xtreme_solos_template_ranked":
                    return "ranked_solo_show";
                case "event_only_fall_ball_trios_ranked":
                    return "ranked_trios_show";
                case "greatestsquads_ranked":
                    return "ranked_squads_show";
                // case "anniversary_fp12_ltm":
                case "classic_solo_main_show":
                case "ftue_uk_show":
                case "knockout_mode":
                case "no_elimination_explore":
                case "event_only_races_any_final_template":
                case "turbo_2_show":
                case "turbo_show":
                    return "main_show";
                case "classic_duos_show":
                case "knockout_duos":
                case "teams_show_ltm":
                    return "squads_2player_template";
                case "sports_show":
                    return "squads_3player_template";
                case "classic_squads_show":
                case "event_day_at_races_squads_template":
                case "event_only_ss2_squads_template":
                case "knockout_squads":
                case "squadcelebration":
                    return "squads_4player";
                case "fp16_ski_fall_high_scorers":
                    return "event_only_skeefall_timetrial_s6_1";
                case "invisibeans_pistachio_template":
                case "invisibeans_template":
                    return "invisibeans_mode";
                case "live_event_timeattack_dizzyheights":
                case "live_event_timeattack_lilyleapers":
                case "live_event_timeattack_partyprom":
                case "live_event_timeattack_shuffle":
                case "live_event_timeattack_trackattack":
                case "live_event_timeattack_treetoptumble":
                case "live_event_timeattack_tundrarun":
                    return "timeattack_mode";
                case "xtreme_explore":
                    return "event_xtreme_fall_guys_template";
                default:
                    return showId;
            }
        }

        private int GetLinkedProfileId(string realShowId, bool isPrivateLobbies) {
            if (this.AllProfiles.Count == 0 || string.IsNullOrEmpty(realShowId)) return 0;
            realShowId = this.GetAlternateShowId(realShowId);
            string showId = this.GetMainGroupShowId(realShowId);
            foreach (Profiles profiles in this.AllProfiles.OrderBy(p => p.DoNotCombineShows ? 0 : 1)) {
                if (profiles.DoNotCombineShows) {
                    if (!isPrivateLobbies && !string.IsNullOrEmpty(profiles.LinkedShowId) && realShowId.IndexOf(profiles.LinkedShowId, StringComparison.OrdinalIgnoreCase) != -1) {
                        return profiles.ProfileId;
                    }
                } else {
                    if (isPrivateLobbies) {
                        if (!string.IsNullOrEmpty(profiles.LinkedShowId) && string.Equals(profiles.LinkedShowId, "private_lobbies")) {
                            return profiles.ProfileId;
                        }
                    } else {
                        if (!string.IsNullOrEmpty(profiles.LinkedShowId) && showId.IndexOf(profiles.LinkedShowId, StringComparison.OrdinalIgnoreCase) != -1) {
                            return profiles.ProfileId;
                        }
                    }
                }
            }
            if (isPrivateLobbies) {
                // return corresponding linked profile when possible if no linked "private_lobbies" profile was found
                return (from profiles in this.AllProfiles.OrderBy(p => p.DoNotCombineShows ? 0 : 1) where !string.IsNullOrEmpty(profiles.LinkedShowId) && showId.IndexOf(profiles.LinkedShowId, StringComparison.OrdinalIgnoreCase) != -1 select profiles.ProfileId).FirstOrDefault();
            }
            // return ProfileId 0 if no linked profile was found/matched
            return 0;
        }

        public void SetLinkedProfileMenu(string realShowId, bool isPrivateLobbies) {
            if (this.AllProfiles.Count == 0 || string.IsNullOrEmpty(realShowId)) return;

            realShowId = this.GetAlternateShowId(realShowId);

            string currentProfileLinkedShowId = this.GetCurrentProfileLinkedShowId();
            bool isCurrentProfileIsDNCS = this.AllProfiles.Find(p => p.ProfileId == this.GetCurrentProfileId()).DoNotCombineShows;
            if (isCurrentProfileIsDNCS && string.Equals(currentProfileLinkedShowId, realShowId)) return;

            string showId = this.GetMainGroupShowId(realShowId);
            int linkedDNCSProfileId = this.AllProfiles.Find(p => p.DoNotCombineShows && string.Equals(p.LinkedShowId, realShowId))?.ProfileId ?? -1;
            if (linkedDNCSProfileId == -1 && string.Equals(currentProfileLinkedShowId, showId)) return;

            this.BeginInvoke((MethodInvoker)delegate {
                int profileId = -1;
                bool isLinkedProfileFound = false;
                foreach (Profiles profiles in this.AllProfiles.FindAll(p => p.DoNotCombineShows)) {
                    if (!isPrivateLobbies && !string.IsNullOrEmpty(profiles.LinkedShowId) && realShowId.IndexOf(profiles.LinkedShowId, StringComparison.OrdinalIgnoreCase) != -1) {
                        profileId = profiles.ProfileId;
                        isLinkedProfileFound = true;
                        break;
                    }
                }
                for (int i = 0; i < this.AllProfiles.Count; i++) {
                    if (isLinkedProfileFound) {
                        if (this.AllProfiles[i].ProfileId == profileId) {
                            ToolStripMenuItem item = this.ProfileMenuItems[this.AllProfiles.Count - 1 - i];
                            if (!item.Checked) { this.menuStats_Click(item, EventArgs.Empty); }
                            return;
                        } else {
                            continue;
                        }
                    }
                    if (this.AllProfiles[i].DoNotCombineShows) continue;
                    if (isPrivateLobbies) {
                        if (!string.IsNullOrEmpty(this.AllProfiles[i].LinkedShowId) && string.Equals(this.AllProfiles[i].LinkedShowId, "private_lobbies")) {
                            ToolStripMenuItem item = this.ProfileMenuItems[this.AllProfiles.Count - 1 - i];
                            if (!item.Checked) { this.menuStats_Click(item, EventArgs.Empty); }
                            return;
                        }
                    } else {
                        if (!string.IsNullOrEmpty(this.AllProfiles[i].LinkedShowId) && showId.IndexOf(this.AllProfiles[i].LinkedShowId, StringComparison.OrdinalIgnoreCase) != -1) {
                            ToolStripMenuItem item = this.ProfileMenuItems[this.AllProfiles.Count - 1 - i];
                            if (!item.Checked) { this.menuStats_Click(item, EventArgs.Empty); }
                            return;
                        }
                    }
                }
                if (isPrivateLobbies) { // select corresponding linked profile when possible if no linked "private_lobbies" profile was found
                    foreach (Profiles profiles in this.AllProfiles.FindAll(p => p.DoNotCombineShows)) {
                        if (!string.IsNullOrEmpty(profiles.LinkedShowId) && realShowId.IndexOf(profiles.LinkedShowId, StringComparison.OrdinalIgnoreCase) != -1) {
                            profileId = profiles.ProfileId;
                            isLinkedProfileFound = true;
                            break;
                        }
                    }
                    for (int j = 0; j < this.AllProfiles.Count; j++) {
                        if (isLinkedProfileFound) {
                            if (this.AllProfiles[j].ProfileId == profileId) {
                                ToolStripMenuItem item = this.ProfileMenuItems[this.AllProfiles.Count - 1 - j];
                                if (!item.Checked) { this.menuStats_Click(item, EventArgs.Empty); }
                                return;
                            } else {
                                continue;
                            }
                        } else {
                            if (this.AllProfiles[j].DoNotCombineShows || string.IsNullOrEmpty(this.AllProfiles[j].LinkedShowId) || showId.IndexOf(this.AllProfiles[j].LinkedShowId, StringComparison.OrdinalIgnoreCase) == -1) continue;

                            ToolStripMenuItem item = this.ProfileMenuItems[this.AllProfiles.Count - 1 - j];
                            if (!item.Checked) { this.menuStats_Click(item, EventArgs.Empty); }
                            return;
                        }
                    }
                }
                // select ProfileId 0 if no linked profile was found/matched
                for (int k = 0; k < this.AllProfiles.Count; k++) {
                    if (this.AllProfiles[k].ProfileId != 0) continue;

                    ToolStripMenuItem item = this.ProfileMenuItems[this.AllProfiles.Count - 1 - k];
                    if (!item.Checked) { this.menuStats_Click(item, EventArgs.Empty); }
                    return;
                }
            });
        }

        private void SetProfileMenu(int profile) {
            if (profile == -1 || this.AllProfiles.Count == 0) return;

            this.Invoke((MethodInvoker)delegate {
                ToolStripMenuItem tsmi = this.menuProfile.DropDownItems[$"menuProfile{profile}"] as ToolStripMenuItem;
                if (tsmi.Checked) return;

                this.menuStats_Click(tsmi, EventArgs.Empty);
            });
        }

        private void SetCurrentProfileIcon(bool linked) {
            this.BeginInvoke((MethodInvoker)delegate {
                if (this.CurrentSettings.AutoChangeProfile) {
                    this.lblCurrentProfileIcon.Image = linked ? Properties.Resources.profile2_linked_icon : Properties.Resources.profile2_unlinked_icon;
                    this.overlay.SetCurrentProfileForeColor(linked ? Color.GreenYellow
                                                            : string.IsNullOrEmpty(this.CurrentSettings.OverlayFontColorSerialized) ? Color.White
                                                              : (Color)new ColorConverter().ConvertFromString(this.CurrentSettings.OverlayFontColorSerialized));
                } else {
                    this.lblCurrentProfileIcon.Image = Properties.Resources.profile2_icon;
                    this.overlay.SetCurrentProfileForeColor(string.IsNullOrEmpty(this.CurrentSettings.OverlayFontColorSerialized) ? Color.White
                                                            : (Color)new ColorConverter().ConvertFromString(this.CurrentSettings.OverlayFontColorSerialized));
                }
            });
        }

        public StatSummary GetLevelInfo(string levelId, LevelType type, BestRecordType record) {
            StatSummary summary = new StatSummary {
                CurrentStreak = 0,
                CurrentFinalStreak = 0,
                BestStreak = 0,
                BestFinalStreak = 0,
                AllWins = 0,
                TotalWins = 0,
                TotalShows = 0,
                TotalFinals = 0,
                TotalPlays = 0,
                TotalQualify = 0,
                TotalGolds = 0,
                FastestFinish = null,
                FastestFinishOverall = null,
                LongestFinish = null,
                LongestFinishOverall = null,
                HighScore = null,
                LowScore = null
            };

            int lastShow = -1;
            if (!this.StatLookup.TryGetValue(levelId, out LevelStats currentLevel)) {
                string roundName = levelId.StartsWith("round_") ? levelId.Substring(6).Replace('_', ' ')
                                                                : levelId.Replace('_', ' ');

                currentLevel = new LevelStats(levelId, this.textInfo.ToTitleCase(roundName), LevelType.Unknown, BestRecordType.Fastest, false, CURRENTSEASON, Properties.Resources.round_unknown_icon, Properties.Resources.round_unknown_big_icon);
            }

            List<RoundInfo> roundInfo = this.AllStats.FindAll(r => r.Profile == this.GetCurrentProfileId());

            for (int i = 0; i < roundInfo.Count; i++) {
                RoundInfo info = roundInfo[i];
                TimeSpan finishTime = info.Finish.GetValueOrDefault(info.Start) - info.Start;
                bool hasFinishTime = finishTime.TotalSeconds > 1.1;
                bool hasLevelDetails = this.StatLookup.ContainsKey(info.Name);
                bool isCurrentLevel = false;
                if (string.Equals(info.Name, currentLevel.Id)) {
                    isCurrentLevel = true;
                }

                int startRoundShowId = info.ShowID;
                RoundInfo endRound = info;
                for (int j = i + 1; j < roundInfo.Count; j++) {
                    if (roundInfo[j].ShowID != startRoundShowId) {
                        break;
                    }
                    endRound = roundInfo[j];
                }

                bool isNotPrivateLobby = !endRound.PrivateLobby;

                bool isInWinsFilter = isNotPrivateLobby
                                      && (this.CurrentSettings.WinsFilter == 0
                                          || (this.CurrentSettings.WinsFilter == 1 && this.IsInStatsFilter(endRound) && this.IsInPartyFilter(info))
                                          || (this.CurrentSettings.WinsFilter == 2 && endRound.Start > SeasonStart)
                                          || (this.CurrentSettings.WinsFilter == 3 && endRound.Start > WeekStart)
                                          || (this.CurrentSettings.WinsFilter == 4 && endRound.Start > DayStart)
                                          || (this.CurrentSettings.WinsFilter == 5 && endRound.Start > SessionStart));
                bool isInQualifyFilter = isNotPrivateLobby
                                         && (this.CurrentSettings.QualifyFilter == 0
                                             || (this.CurrentSettings.QualifyFilter == 1 && this.IsInStatsFilter(endRound) && this.IsInPartyFilter(info))
                                             || (this.CurrentSettings.QualifyFilter == 2 && endRound.Start > SeasonStart)
                                             || (this.CurrentSettings.QualifyFilter == 3 && endRound.Start > WeekStart)
                                             || (this.CurrentSettings.QualifyFilter == 4 && endRound.Start > DayStart)
                                             || (this.CurrentSettings.QualifyFilter == 5 && endRound.Start > SessionStart));
                bool isInFastestFilter = this.CurrentSettings.FastestFilter == 0
                                         || (this.CurrentSettings.FastestFilter == 1 && this.IsInStatsFilter(endRound) && this.IsInPartyFilter(info))
                                         || (this.CurrentSettings.FastestFilter == 2 && endRound.Start > SeasonStart)
                                         || (this.CurrentSettings.FastestFilter == 3 && endRound.Start > WeekStart)
                                         || (this.CurrentSettings.FastestFilter == 4 && endRound.Start > DayStart)
                                         || (this.CurrentSettings.FastestFilter == 5 && endRound.Start > SessionStart);

                if (info.ShowID != lastShow) {
                    lastShow = info.ShowID;
                    if (isInWinsFilter) {
                        summary.TotalShows++;
                    }
                }

                if (isCurrentLevel) {
                    if (isInQualifyFilter) {
                        summary.TotalPlays++;
                    }

                    if (hasFinishTime && (!summary.FastestFinishOverall.HasValue || summary.FastestFinishOverall.Value > finishTime)) {
                        summary.FastestFinishOverall = finishTime;
                    }

                    if (hasFinishTime && (!summary.LongestFinishOverall.HasValue || summary.LongestFinishOverall.Value < finishTime)) {
                        summary.LongestFinishOverall = finishTime;
                    }

                    if (isInFastestFilter) {
                        if (hasFinishTime && (!summary.FastestFinish.HasValue || summary.FastestFinish.Value > finishTime)) {
                            summary.FastestFinish = finishTime;
                        }

                        if (hasFinishTime && (!summary.LongestFinish.HasValue || summary.LongestFinish.Value < finishTime)) {
                            summary.LongestFinish = finishTime;
                        }

                        if ((!hasLevelDetails || record == BestRecordType.HighScore) && info.Score.HasValue && (!summary.HighScore.HasValue || info.Score.Value > summary.HighScore.Value)) {
                            summary.HighScore = info.Score;
                        }

                        if ((!hasLevelDetails || record == BestRecordType.HighScore) && info.Score.HasValue && (!summary.LowScore.HasValue || info.Score.Value < summary.LowScore.Value)) {
                            summary.LowScore = info.Score;
                        }
                    }
                }

                bool isFinalRound = (info.IsFinal || info.Crown) && !endRound.PrivateLobby;

                if (ReferenceEquals(info, endRound) && isFinalRound) {
                    summary.CurrentFinalStreak++;
                    if (summary.BestFinalStreak < summary.CurrentFinalStreak) {
                        summary.BestFinalStreak = summary.CurrentFinalStreak;
                    }
                }

                isNotPrivateLobby = !info.PrivateLobby;

                if (info.Qualified) {
                    if (hasLevelDetails && (info.IsFinal || info.Crown)) {
                        if (isNotPrivateLobby) {
                            summary.AllWins++;
                        }

                        if (isInWinsFilter) {
                            summary.TotalWins++;
                            summary.TotalFinals++;
                        }

                        if (isNotPrivateLobby) {
                            summary.CurrentStreak++;
                            if (summary.CurrentStreak > summary.BestStreak) {
                                summary.BestStreak = summary.CurrentStreak;
                            }
                        }
                    }

                    if (isCurrentLevel) {
                        if (isInQualifyFilter) {
                            if (info.Tier == (int)QualifyTier.Gold) {
                                summary.TotalGolds++;
                            }
                            summary.TotalQualify++;
                        }
                    }
                } else if (isNotPrivateLobby) {
                    if (!info.IsFinal && !info.Crown) {
                        summary.CurrentFinalStreak = 0;
                    }
                    summary.CurrentStreak = 0;
                    if (isInWinsFilter && hasLevelDetails && (info.IsFinal || info.Crown)) {
                        summary.TotalFinals++;
                    }
                }
            }

            return summary;
        }

        private void ClearTotals() {
            this.Wins = 0;
            this.Shows = 0;
            this.Rounds = 0;
            this.CustomRounds = 0;
            this.Duration = TimeSpan.Zero;
            this.CustomShows = 0;
            this.Finals = 0;
            this.GoldMedals = 0;
            this.SilverMedals = 0;
            this.BronzeMedals = 0;
            this.PinkMedals = 0;
            this.EliminatedMedals = 0;
            this.CustomGoldMedals = 0;
            this.CustomSilverMedals = 0;
            this.CustomBronzeMedals = 0;
            this.CustomPinkMedals = 0;
            this.CustomEliminatedMedals = 0;
            this.Kudos = 0;
        }

        private void UpdateTotals() {
            try {
                this.lblCurrentProfile.Text = $"{this.GetCurrentProfileName().Replace("&", "&&")}";
                //this.lblCurrentProfile.ToolTipText = $"{Multilingual.GetWord("profile_change_tooltiptext")}";
                this.lblTotalShows.Text = $"{this.Shows:N0}{Multilingual.GetWord("main_inning")}";
                if (this.CustomShows > 0) this.lblTotalShows.Text += $" ({Multilingual.GetWord("main_custom_shows")} : {this.CustomShows:N0}{Multilingual.GetWord("main_inning")})";
                //this.lblTotalShows.ToolTipText = $"{Multilingual.GetWord("shows_detail_tooltiptext")}";
                this.lblTotalRounds.Text = $"{this.Rounds:N0}{Multilingual.GetWord("main_round")}";
                if (this.CustomRounds > 0) this.lblTotalRounds.Text += $" ({Multilingual.GetWord("main_custom_shows")} : {this.CustomRounds:N0}{Multilingual.GetWord("main_round")})";
                //this.lblTotalRounds.ToolTipText = $"{Multilingual.GetWord("rounds_detail_tooltiptext")}";
                this.lblTotalTime.Text = $"{(int)this.Duration.TotalHours}{Multilingual.GetWord("main_hour")}{this.Duration:mm}{Multilingual.GetWord("main_min")}{this.Duration:ss}{Multilingual.GetWord("main_sec")}";
                //this.lblTotalTime.ToolTipText = $"{Multilingual.GetWord("stats_detail_tooltiptext")}";
                float winChance = (float)this.Wins * 100 / Math.Max(1, this.Shows);
                this.lblTotalWins.Text = $"{this.Wins:N0}{Multilingual.GetWord("main_win")} ({Math.Truncate(winChance * 10) / 10} %)";
                //this.lblTotalWins.ToolTipText = $"{Multilingual.GetWord("wins_detail_tooltiptext")}";
                float finalChance = (float)this.Finals * 100 / Math.Max(1, this.Shows);
                this.lblTotalFinals.Text = $"{this.Finals:N0}{Multilingual.GetWord("main_inning")} ({Math.Truncate(finalChance * 10) / 10} %)";
                //this.lblTotalFinals.ToolTipText = $"{Multilingual.GetWord("finals_detail_tooltiptext")}";
                this.lblGoldMedal.Text = $"{this.GoldMedals:N0}";
                if (this.CustomGoldMedals > 0) this.lblGoldMedal.Text += $" ({this.CustomGoldMedals:N0})";
                this.lblSilverMedal.Text = $"{this.SilverMedals:N0}";
                if (this.CustomSilverMedals > 0) this.lblSilverMedal.Text += $" ({this.CustomSilverMedals:N0})";
                this.lblBronzeMedal.Text = $"{this.BronzeMedals:N0}";
                if (this.CustomBronzeMedals > 0) this.lblBronzeMedal.Text += $" ({this.CustomBronzeMedals:N0})";
                this.lblPinkMedal.Text = $"{this.PinkMedals:N0}";
                if (this.CustomPinkMedals > 0) this.lblPinkMedal.Text += $" ({this.CustomPinkMedals:N0})";
                this.lblEliminatedMedal.Text = $"{this.EliminatedMedals:N0}";
                if (this.CustomEliminatedMedals > 0) this.lblEliminatedMedal.Text += $" ({this.CustomEliminatedMedals:N0})";
                this.lblGoldMedal.Visible = this.GoldMedals != 0 || this.CustomGoldMedals != 0;
                this.lblSilverMedal.Visible = this.SilverMedals != 0 || this.CustomSilverMedals != 0;
                this.lblBronzeMedal.Visible = this.BronzeMedals != 0 || this.CustomBronzeMedals != 0;
                this.lblPinkMedal.Visible = this.PinkMedals != 0 || this.CustomPinkMedals != 0;
                this.lblEliminatedMedal.Visible = this.EliminatedMedals != 0 || this.CustomEliminatedMedals != 0;
                this.lblKudos.Text = $"{this.Kudos:N0}";
                this.lblKudos.Visible = this.Kudos != 0;
                this.gridDetails.Invalidate();
            } catch (Exception ex) {
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ShowToastNotification(IWin32Window window, Image thumbNail, string caption, string description, Font font, Image appOwnerIcon, ToastDuration duration, ToastPosition position, ToastAnimation animation, ToastTheme theme, ToastSound toastSound, bool muting, bool isAsync) {
            this.BeginInvoke((MethodInvoker)delegate {
                this.toast = Toast.Build(window, caption, description, font, thumbNail, appOwnerIcon,
                    duration, position, animation, ToastCloseStyle.ButtonAndClickEntire, theme, toastSound, muting);
                if (isAsync) {
                    this.toast.ShowAsync();
                } else {
                    this.toast.Show();
                }
            });
        }

        public void ShowNotification(string title, string text, ToolTipIcon toolTipIcon, int timeout) {
            if (this.trayIcon.Visible) {
                this.trayIcon.BalloonTipTitle = title;
                this.trayIcon.BalloonTipText = text;
                this.trayIcon.BalloonTipIcon = toolTipIcon;
                this.trayIcon.ShowBalloonTip(timeout);
            }
            // else {
            //     MetroMessageBox.Show(this, text, title, MessageBoxButtons.OK, toolTipIcon == ToolTipIcon.None ? MessageBoxIcon.None :
            //                                                                                  toolTipIcon == ToolTipIcon.Error ? MessageBoxIcon.Error :
            //                                                                                  toolTipIcon == ToolTipIcon.Info ? MessageBoxIcon.Information :
            //                                                                                  toolTipIcon == ToolTipIcon.Warning ? MessageBoxIcon.Warning : MessageBoxIcon.None);
            // }
        }

        public void AllocOverlayTooltip() {
            this.omtt = new MetroToolTip {
                Theme = this.Theme
            };
        }

        public void ShowOverlayTooltip(string message, IWin32Window window, Point position, int duration = -1) {
            if (duration == -1) {
                this.omtt.Show(message, window, position);
            } else {
                this.omtt.Show(message, window, position, duration);
            }
        }

        public void HideOverlayTooltip(IWin32Window window) {
            this.omtt.Hide(window);
        }

        public void AllocCustomTooltip(DrawToolTipEventHandler drawFunc) {
            this.cmtt = new MetroToolTip {
                OwnerDraw = true
            };
            this.cmtt.Draw += drawFunc;
        }

        public void ShowCustomTooltip(string message, IWin32Window window, Point position, int duration = -1) {
            if (duration == -1) {
                this.cmtt.Show(message, window, position);
            } else {
                this.cmtt.Show(message, window, position, duration);
            }
        }

        public void HideCustomTooltip(IWin32Window window) {
            this.cmtt.Hide(window);
        }

        public void AllocTooltip() {
            this.mtt = new MetroToolTip {
                Theme = this.Theme
            };
        }

        public void ShowTooltip(string message, IWin32Window window, Point position, int duration = -1) {
            if (duration == -1) {
                this.mtt.Show(message, window, position);
            } else {
                this.mtt.Show(message, window, position, duration);
            }
        }

        public void HideTooltip(IWin32Window window) {
            this.mtt.Hide(window);
        }

        private void Toggle_MouseEnter(object sender, EventArgs e) {
            if (sender.Equals(this.mtgIgnoreLevelTypeWhenSorting) || sender.Equals(this.lblIgnoreLevelTypeWhenSorting)) {
                if (!this.mtgIgnoreLevelTypeWhenSorting.Checked) this.lblIgnoreLevelTypeWhenSorting.ForeColor = Color.DimGray;
            }
        }

        private void Toggle_MouseLeave(object sender, EventArgs e) {
            if (sender.Equals(this.mtgIgnoreLevelTypeWhenSorting) || sender.Equals(this.lblIgnoreLevelTypeWhenSorting)) {
                if (!this.mtgIgnoreLevelTypeWhenSorting.Checked) this.lblIgnoreLevelTypeWhenSorting.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
            }
        }

        private void lblIgnoreLevelTypeWhenSorting_Click(object sender, EventArgs e) {
            this.mtgIgnoreLevelTypeWhenSorting.Checked = !this.mtgIgnoreLevelTypeWhenSorting.Checked;
        }

        private void mtgIgnoreLevelTypeWhenSorting_CheckedChanged(object sender, EventArgs e) {
            bool mtgChecked = ((MetroToggle)sender).Checked;
            this.lblIgnoreLevelTypeWhenSorting.ForeColor = mtgChecked ? (this.Theme == MetroThemeStyle.Light ? Color.FromArgb(0, 174, 219) : Color.GreenYellow) : Color.DimGray;
            this.CurrentSettings.IgnoreLevelTypeWhenSorting = mtgChecked;
            this.SortGridDetails(true);
            this.SaveUserSettings();
        }

        private void scrollTimer_Tick(object sender, EventArgs e) {
            this.scrollTimer.Stop();
            this.isScrollingStopped = true;
        }

        private void gridDetails_Scroll(object sender, ScrollEventArgs e) {
            this.isScrollingStopped = false;
            this.scrollTimer.Stop();
            this.scrollTimer.Start();
        }

        private void gridDetails_DataSourceChanged(object sender, EventArgs e) {
            try {
                if (((Grid)sender).Columns.Count == 0) return;

                int pos = 0;
                ((Grid)sender).Columns["RoundBigIcon"].Visible = false;
                ((Grid)sender).Columns["AveKudos"].Visible = false;
                ((Grid)sender).Columns["AveDuration"].Visible = false;
                ((Grid)sender).Columns["Id"].Visible = false;
                ((Grid)sender).Setup("RoundIcon", pos++, this.GetDataGridViewColumnWidth("RoundIcon", ""), "", DataGridViewContentAlignment.MiddleCenter);
                ((Grid)sender).Columns["RoundIcon"].Resizable = DataGridViewTriState.False;
                ((Grid)sender).Setup("Name", pos++, this.GetDataGridViewColumnWidth("Name", Multilingual.GetWord("main_round_name")), Multilingual.GetWord("main_round_name"), DataGridViewContentAlignment.MiddleLeft);
                ((Grid)sender).Columns["Name"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                ((Grid)sender).Setup("Played", pos++, this.GetDataGridViewColumnWidth("Played", Multilingual.GetWord("main_played")), Multilingual.GetWord("main_played"), DataGridViewContentAlignment.MiddleRight);
                ((Grid)sender).Setup("Qualified", pos++, this.GetDataGridViewColumnWidth("Qualified", Multilingual.GetWord("main_qualified")), Multilingual.GetWord("main_qualified"), DataGridViewContentAlignment.MiddleRight);
                ((Grid)sender).Setup("Gold", pos++, this.GetDataGridViewColumnWidth("Gold", Multilingual.GetWord("main_gold")), Multilingual.GetWord("main_gold"), DataGridViewContentAlignment.MiddleRight);
                ((Grid)sender).Setup("Silver", pos++, this.GetDataGridViewColumnWidth("Silver", Multilingual.GetWord("main_silver")), Multilingual.GetWord("main_silver"), DataGridViewContentAlignment.MiddleRight);
                ((Grid)sender).Setup("Bronze", pos++, this.GetDataGridViewColumnWidth("Bronze", Multilingual.GetWord("main_bronze")), Multilingual.GetWord("main_bronze"), DataGridViewContentAlignment.MiddleRight);
                ((Grid)sender).Setup("Kudos", pos++, this.GetDataGridViewColumnWidth("Kudos", Multilingual.GetWord("main_kudos")), Multilingual.GetWord("main_kudos"), DataGridViewContentAlignment.MiddleRight);
                ((Grid)sender).Setup("Fastest", pos++, this.GetDataGridViewColumnWidth("Fastest", Multilingual.GetWord("main_fastest")), Multilingual.GetWord("main_fastest"), DataGridViewContentAlignment.MiddleRight);
                ((Grid)sender).Setup("Longest", pos++, this.GetDataGridViewColumnWidth("Longest", Multilingual.GetWord("main_longest")), Multilingual.GetWord("main_longest"), DataGridViewContentAlignment.MiddleRight);
                ((Grid)sender).Setup("AveFinish", pos, this.GetDataGridViewColumnWidth("AveFinish", Multilingual.GetWord("main_ave_finish")), Multilingual.GetWord("main_ave_finish"), DataGridViewContentAlignment.MiddleRight);
            } catch (Exception ex) {
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetDataGridViewColumnWidth(string columnName, string columnText) {
            int sizeOfText;
            switch (columnName) {
                case "RoundIcon":
                    sizeOfText = 13;
                    break;
                case "Name":
                    return 0;
                case "Played":
                    sizeOfText = TextRenderer.MeasureText(columnText, this.dataGridViewCellStyle1.Font).Width;
                    break;
                case "Qualified":
                    sizeOfText = TextRenderer.MeasureText(columnText, this.dataGridViewCellStyle1.Font).Width;
                    sizeOfText += CurrentLanguage == Language.English || CurrentLanguage == Language.French ? 0 : 5;
                    break;
                case "Gold":
                    sizeOfText = TextRenderer.MeasureText(columnText, this.dataGridViewCellStyle1.Font).Width;
                    sizeOfText += CurrentLanguage == Language.French ? 12 : CurrentLanguage == Language.SimplifiedChinese || CurrentLanguage == Language.TraditionalChinese ? 5 : 0;
                    break;
                case "Silver":
                    sizeOfText = TextRenderer.MeasureText(columnText, this.dataGridViewCellStyle1.Font).Width;
                    sizeOfText += CurrentLanguage == Language.SimplifiedChinese || CurrentLanguage == Language.TraditionalChinese ? 5 : 0;
                    break;
                case "Bronze":
                    sizeOfText = TextRenderer.MeasureText(columnText, this.dataGridViewCellStyle1.Font).Width;
                    sizeOfText += CurrentLanguage == Language.SimplifiedChinese || CurrentLanguage == Language.TraditionalChinese ? 5 : 0;
                    break;
                case "Kudos":
                    sizeOfText = TextRenderer.MeasureText(columnText, this.dataGridViewCellStyle1.Font).Width;
                    break;
                case "Fastest":
                    sizeOfText = TextRenderer.MeasureText(columnText, this.dataGridViewCellStyle1.Font).Width;
                    sizeOfText += 20;
                    break;
                case "Longest":
                    sizeOfText = TextRenderer.MeasureText(columnText, this.dataGridViewCellStyle1.Font).Width;
                    sizeOfText += 20;
                    break;
                case "AveFinish":
                    sizeOfText = TextRenderer.MeasureText(columnText, this.dataGridViewCellStyle1.Font).Width;
                    sizeOfText += 20;
                    break;
                default:
                    return 0;
            }

            return sizeOfText + 24;
        }

        private void InitMainDataGridView() {
            this.dataGridViewCellStyle1.Font = Overlay.GetMainFont(12);
            this.dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //this.dataGridViewCellStyle1.BackColor = Color.LightGray;
            //this.dataGridViewCellStyle1.ForeColor = Color.Black;
            //this.dataGridViewCellStyle1.SelectionBackColor = Color.Cyan;
            //this.dataGridViewCellStyle1.SelectionForeColor = Color.Black;
            this.dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            this.gridDetails.ColumnHeadersDefaultCellStyle = this.dataGridViewCellStyle1;
            this.gridDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridDetails.ColumnHeadersHeight = 20;

            this.dataGridViewCellStyle2.Font = Overlay.GetMainFont(14);
            this.dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //this.dataGridViewCellStyle2.BackColor = Color.White;
            //this.dataGridViewCellStyle2.ForeColor = Color.Black;
            //this.dataGridViewCellStyle2.SelectionBackColor = Color.DeepSkyBlue;
            //this.dataGridViewCellStyle2.SelectionForeColor = Color.Black;
            this.dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            this.gridDetails.DefaultCellStyle = this.dataGridViewCellStyle2;
            this.gridDetails.RowTemplate.Height = 25;

            // this.gridDetails.DataSource = this.StatDetails;
            this.gridDetails.DataSource = this.StatDetails;
        }

        private void SetMainDataGridViewOrder() {
            int pos = 0;
            this.gridDetails.Columns["RoundIcon"].DisplayIndex = pos++;
            this.gridDetails.Columns["Name"].DisplayIndex = pos++;
            this.gridDetails.Columns["Played"].DisplayIndex = pos++;
            this.gridDetails.Columns["Qualified"].DisplayIndex = pos++;
            this.gridDetails.Columns["Gold"].DisplayIndex = pos++;
            this.gridDetails.Columns["Silver"].DisplayIndex = pos++;
            this.gridDetails.Columns["Bronze"].DisplayIndex = pos++;
            this.gridDetails.Columns["Kudos"].DisplayIndex = pos++;
            this.gridDetails.Columns["Fastest"].DisplayIndex = pos++;
            this.gridDetails.Columns["Longest"].DisplayIndex = pos++;
            this.gridDetails.Columns["AveFinish"].DisplayIndex = pos;
        }

        private void gridDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) {
            try {
                if (e.RowIndex < 0) return;
                if (!((Grid)sender).Rows[e.RowIndex].Visible) return;

                LevelStats levelStats = ((Grid)sender).Rows[e.RowIndex].DataBoundItem as LevelStats;
                float fBrightness = 0.85f;
                Color cellColor;
                switch (((Grid)sender).Columns[e.ColumnIndex].Name) {
                    case "RoundIcon":
                        if (levelStats.IsFinal) {
                            cellColor = Color.FromArgb(255, 240, 200);
                            e.CellStyle.BackColor = this.Theme == MetroThemeStyle.Light ? cellColor : Utils.GetColorBrightnessAdjustment(cellColor, fBrightness);
                            break;
                        }
                        switch (levelStats.Type) {
                            case LevelType.Race:
                                cellColor = Color.FromArgb(210, 255, 220);
                                e.CellStyle.BackColor = this.Theme == MetroThemeStyle.Light ? cellColor : Utils.GetColorBrightnessAdjustment(cellColor, fBrightness);
                                break;
                            case LevelType.Survival:
                                cellColor = Color.FromArgb(250, 205, 255);
                                e.CellStyle.BackColor = this.Theme == MetroThemeStyle.Light ? cellColor : Utils.GetColorBrightnessAdjustment(cellColor, fBrightness);
                                break;
                            case LevelType.Hunt:
                                cellColor = Color.FromArgb(200, 220, 255);
                                e.CellStyle.BackColor = this.Theme == MetroThemeStyle.Light ? cellColor : Utils.GetColorBrightnessAdjustment(cellColor, fBrightness);
                                break;
                            case LevelType.Logic:
                                cellColor = Color.FromArgb(230, 250, 255);
                                e.CellStyle.BackColor = this.Theme == MetroThemeStyle.Light ? cellColor : Utils.GetColorBrightnessAdjustment(cellColor, fBrightness);
                                break;
                            case LevelType.Team:
                                cellColor = Color.FromArgb(255, 220, 205);
                                e.CellStyle.BackColor = this.Theme == MetroThemeStyle.Light ? cellColor : Utils.GetColorBrightnessAdjustment(cellColor, fBrightness);
                                break;
                            case LevelType.Invisibeans:
                                cellColor = Color.FromArgb(255, 255, 255);
                                e.CellStyle.BackColor = this.Theme == MetroThemeStyle.Light ? cellColor : Utils.GetColorBrightnessAdjustment(cellColor, fBrightness);
                                break;
                            case LevelType.Unknown:
                                e.CellStyle.BackColor = this.Theme == MetroThemeStyle.Light ? Color.LightGray : Color.DarkGray;
                                break;
                        }
                        break;
                    case "Name":
                        e.CellStyle.ForeColor = Color.Black;
                        if (levelStats.IsFinal) {
                            cellColor = Color.FromArgb(255, 240, 200);
                            e.CellStyle.BackColor = this.Theme == MetroThemeStyle.Light ? cellColor : Utils.GetColorBrightnessAdjustment(cellColor, fBrightness);
                            break;
                        }
                        switch (levelStats.Type) {
                            case LevelType.Race:
                                cellColor = Color.FromArgb(210, 255, 220);
                                e.CellStyle.BackColor = this.Theme == MetroThemeStyle.Light ? cellColor : Utils.GetColorBrightnessAdjustment(cellColor, fBrightness);
                                break;
                            case LevelType.Survival:
                                cellColor = Color.FromArgb(250, 205, 255);
                                e.CellStyle.BackColor = this.Theme == MetroThemeStyle.Light ? cellColor : Utils.GetColorBrightnessAdjustment(cellColor, fBrightness);
                                break;
                            case LevelType.Hunt:
                                cellColor = Color.FromArgb(200, 220, 255);
                                e.CellStyle.BackColor = this.Theme == MetroThemeStyle.Light ? cellColor : Utils.GetColorBrightnessAdjustment(cellColor, fBrightness);
                                break;
                            case LevelType.Logic:
                                cellColor = Color.FromArgb(230, 250, 255);
                                e.CellStyle.BackColor = this.Theme == MetroThemeStyle.Light ? cellColor : Utils.GetColorBrightnessAdjustment(cellColor, fBrightness);
                                break;
                            case LevelType.Team:
                                cellColor = Color.FromArgb(255, 220, 205);
                                e.CellStyle.BackColor = this.Theme == MetroThemeStyle.Light ? cellColor : Utils.GetColorBrightnessAdjustment(cellColor, fBrightness);
                                break;
                            case LevelType.Invisibeans:
                                cellColor = Color.FromArgb(255, 255, 255);
                                e.CellStyle.BackColor = this.Theme == MetroThemeStyle.Light ? cellColor : Utils.GetColorBrightnessAdjustment(cellColor, fBrightness);
                                break;
                            case LevelType.Unknown:
                                e.CellStyle.BackColor = this.Theme == MetroThemeStyle.Light ? Color.LightGray : Color.DarkGray;
                                break;
                        }
                        break;
                    case "Played":
                        fBrightness -= 0.2f;
                        cellColor = Color.FromArgb(0, 126, 222);
                        e.CellStyle.ForeColor = this.Theme == MetroThemeStyle.Light ? Utils.GetColorBrightnessAdjustment(cellColor, fBrightness) : cellColor;
                        e.Value = levelStats.Played == 0 ? "-" : $"{e.Value:N0}";
                        break;
                    case "Qualified":
                        fBrightness -= 0.2f;
                        cellColor = Color.FromArgb(255, 20, 147);
                        e.CellStyle.ForeColor = this.Theme == MetroThemeStyle.Light ? Utils.GetColorBrightnessAdjustment(cellColor, fBrightness) : cellColor;
                        if (levelStats.Qualified == 0) {
                            e.Value = "-";
                            ((Grid)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = "";
                        } else {
                            float qualifyChance = levelStats.Qualified * 100f / Math.Max(1, levelStats.Played);
                            if (this.CurrentSettings.ShowPercentages) {
                                e.Value = $"{Math.Truncate(qualifyChance * 10) / 10}%";
                                ((Grid)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = $"{levelStats.Qualified:N0}";
                            } else {
                                e.Value = $"{levelStats.Qualified:N0}";
                                ((Grid)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = $"{Math.Truncate(qualifyChance * 10) / 10}%";
                            }
                        }
                        break;
                    case "Gold":
                        fBrightness -= 0.2f;
                        cellColor = Color.FromArgb(255, 215, 0);
                        e.CellStyle.ForeColor = this.Theme == MetroThemeStyle.Light ? Utils.GetColorBrightnessAdjustment(cellColor, fBrightness) : cellColor;
                        if (levelStats.Gold == 0) {
                            e.Value = "-";
                            ((Grid)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = "";
                        } else {
                            float goldChance = levelStats.Gold * 100f / Math.Max(1, levelStats.Played);
                            if (this.CurrentSettings.ShowPercentages) {
                                e.Value = $"{Math.Truncate(goldChance * 10) / 10}%";
                                ((Grid)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = $"{levelStats.Gold:N0}";
                            } else {
                                e.Value = $"{levelStats.Gold:N0}";
                                ((Grid)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = $"{Math.Truncate(goldChance * 10) / 10}%";
                            }
                        }
                        break;
                    case "Silver":
                        fBrightness -= 0.3f;
                        cellColor = Color.FromArgb(192, 192, 192);
                        e.CellStyle.ForeColor = this.Theme == MetroThemeStyle.Light ? Utils.GetColorBrightnessAdjustment(cellColor, fBrightness) : cellColor;
                        if (levelStats.Silver == 0) {
                            e.Value = "-";
                            ((Grid)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = "";
                        } else {
                            float silverChance = levelStats.Silver * 100f / Math.Max(1, levelStats.Played);
                            if (this.CurrentSettings.ShowPercentages) {
                                e.Value = $"{Math.Truncate(silverChance * 10) / 10}%";
                                ((Grid)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = $"{levelStats.Silver:N0}";
                            } else {
                                e.Value = $"{levelStats.Silver:N0}";
                                ((Grid)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = $"{Math.Truncate(silverChance * 10) / 10}%";
                            }
                        }
                        break;
                    case "Bronze":
                        fBrightness -= 0.2f;
                        cellColor = Color.FromArgb(205, 127, 50);
                        e.CellStyle.ForeColor = this.Theme == MetroThemeStyle.Light ? Utils.GetColorBrightnessAdjustment(cellColor, fBrightness) : cellColor;
                        if (levelStats.Bronze == 0) {
                            e.Value = "-";
                            ((Grid)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = "";
                        } else {
                            float bronzeChance = levelStats.Bronze * 100f / Math.Max(1, levelStats.Played);
                            if (this.CurrentSettings.ShowPercentages) {
                                e.Value = $"{Math.Truncate(bronzeChance * 10) / 10}%";
                                ((Grid)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = $"{levelStats.Bronze:N0}";
                            } else {
                                e.Value = $"{levelStats.Bronze:N0}";
                                ((Grid)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = $"{Math.Truncate(bronzeChance * 10) / 10}%";
                            }
                        }
                        break;
                    case "Kudos":
                        fBrightness -= 0.2f;
                        cellColor = Color.FromArgb(218, 112, 214);
                        e.CellStyle.ForeColor = this.Theme == MetroThemeStyle.Light ? Utils.GetColorBrightnessAdjustment(cellColor, fBrightness) : cellColor;
                        e.Value = levelStats.Kudos == 0 ? "-" : $"{e.Value:N0}";
                        break;
                    case "AveFinish":
                        fBrightness -= 0.2f;
                        cellColor = Color.FromArgb(0, 192, 192);
                        e.CellStyle.ForeColor = this.Theme == MetroThemeStyle.Light ? Utils.GetColorBrightnessAdjustment(cellColor, fBrightness) : cellColor;
                        e.Value = levelStats.AveFinish == TimeSpan.Zero ? "-" : levelStats.AveFinish.ToString("m\\:ss\\.fff");
                        break;
                    case "Fastest":
                        fBrightness -= 0.2f;
                        cellColor = Color.FromArgb(0, 192, 192);
                        e.CellStyle.ForeColor = this.Theme == MetroThemeStyle.Light ? Utils.GetColorBrightnessAdjustment(cellColor, fBrightness) : cellColor;
                        e.Value = levelStats.Fastest == TimeSpan.Zero ? "-" : levelStats.Fastest.ToString("m\\:ss\\.fff");
                        break;
                    case "Longest":
                        fBrightness -= 0.2f;
                        cellColor = Color.FromArgb(0, 192, 192);
                        e.CellStyle.ForeColor = this.Theme == MetroThemeStyle.Light ? Utils.GetColorBrightnessAdjustment(cellColor, fBrightness) : cellColor;
                        e.Value = levelStats.Longest == TimeSpan.Zero ? "-" : levelStats.Longest.ToString("m\\:ss\\.fff");
                        break;
                }
            } catch (Exception ex) {
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridDetails_CellMouseLeave(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                ((Grid)sender).SuspendLayout();
                ((Grid)sender).Cursor = Cursors.Default;
                this.HideCustomTooltip(this);
                ((Grid)sender).ResumeLayout();
            }
        }

        private void gridDetails_CellMouseEnter(object sender, DataGridViewCellEventArgs e) {
            if (!this.isScrollingStopped) return;

            try {
                ((Grid)sender).SuspendLayout();
                if (e.RowIndex >= 0 && (((Grid)sender).Columns[e.ColumnIndex].Name == "Name" || ((Grid)sender).Columns[e.ColumnIndex].Name == "RoundIcon")) {
                    ((Grid)sender).ShowCellToolTips = false;
                    ((Grid)sender).Cursor = Cursors.Hand;
                    Point cursorPosition = this.PointToClient(Cursor.Position);
                    Point position = new Point(cursorPosition.X + 16, cursorPosition.Y + 16);
                    this.AllocCustomTooltip(this.cmtt_center_Draw);
                    this.ShowCustomTooltip($"{Multilingual.GetWord("level_detail_tooltiptext_prefix")}{((Grid)sender).Rows[e.RowIndex].Cells["Name"].Value}{Multilingual.GetWord("level_detail_tooltiptext_suffix")}", this, position);
                } else if (e.RowIndex >= 0) {
                    ((Grid)sender).ShowCellToolTips = true;
                    ((Grid)sender).Cursor = e.RowIndex >= 0 && !(((Grid)sender).Columns[e.ColumnIndex].Name == "Name" || ((Grid)sender).Columns[e.ColumnIndex].Name == "RoundIcon")
                                              ? this.Theme == MetroThemeStyle.Light
                                                ? new Cursor(Properties.Resources.transform_icon.GetHicon())
                                                : new Cursor(Properties.Resources.transform_gray_icon.GetHicon())
                                              : Cursors.Default;
                }
                ((Grid)sender).ResumeLayout();
            } catch (Exception ex) {
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridDetails_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e) {
            this.mtgIgnoreLevelTypeWhenSorting.Checked = this.CurrentSettings.IgnoreLevelTypeWhenSorting;
        }

        private void SortGridDetails(bool isInitialize, int columnIndex = 0) {
            if (this.StatDetails == null) return;

            string columnName = this.gridDetails.Columns[columnIndex].Name;
            SortOrder sortOrder = isInitialize ? SortOrder.None : this.gridDetails.GetSortOrder(columnName);

            this.StatDetails.Sort((one, two) => {
                LevelType oneType = one.IsFinal ? LevelType.Final : one.Type;
                LevelType twoType = two.IsFinal ? LevelType.Final : two.Type;

                int typeCompare = this.CurrentSettings.IgnoreLevelTypeWhenSorting && sortOrder != SortOrder.None ? 0 : ((int)oneType).CompareTo((int)twoType);

                if (sortOrder == SortOrder.Descending) {
                    (one, two) = (two, one);
                }

                int nameCompare = $"{one.Name}".CompareTo($"{two.Name}");
                bool percents = this.CurrentSettings.ShowPercentages;
                if (typeCompare == 0 && sortOrder != SortOrder.None) {
                    switch (columnName) {
                        case "Played": typeCompare = one.Played.CompareTo(two.Played); break;
                        case "Qualified": typeCompare = ((double)one.Qualified / (one.Played > 0 && percents ? one.Played : 1)).CompareTo((double)two.Qualified / (two.Played > 0 && percents ? two.Played : 1)); break;
                        case "Gold": typeCompare = ((double)one.Gold / (one.Played > 0 && percents ? one.Played : 1)).CompareTo((double)two.Gold / (two.Played > 0 && percents ? two.Played : 1)); break;
                        case "Silver": typeCompare = ((double)one.Silver / (one.Played > 0 && percents ? one.Played : 1)).CompareTo((double)two.Silver / (two.Played > 0 && percents ? two.Played : 1)); break;
                        case "Bronze": typeCompare = ((double)one.Bronze / (one.Played > 0 && percents ? one.Played : 1)).CompareTo((double)two.Bronze / (two.Played > 0 && percents ? two.Played : 1)); break;
                        case "Kudos": typeCompare = one.Kudos.CompareTo(two.Kudos); break;
                        case "Fastest": typeCompare = one.Fastest.CompareTo(two.Fastest); break;
                        case "Longest": typeCompare = one.Longest.CompareTo(two.Longest); break;
                        case "AveFinish": typeCompare = one.AveFinish.CompareTo(two.AveFinish); break;
                        case "AveKudos": typeCompare = one.AveKudos.CompareTo(two.AveKudos); break;
                        default: typeCompare = nameCompare; break;
                    }
                }

                if (typeCompare == 0) {
                    typeCompare = nameCompare;
                }

                return typeCompare;
            });

            this.gridDetails.DataSource = null;
            // this.gridDetails.DataSource = this.StatDetails;
            this.gridDetails.DataSource = this.StatDetails;
            this.gridDetails.Columns[columnName].HeaderCell.SortGlyphDirection = sortOrder;
        }

        private void gridDetails_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            this.SortGridDetails(false, e.ColumnIndex);
        }

        private void gridDetails_SelectionChanged(object sender, EventArgs e) {
            if (((Grid)sender).SelectedCells.Count > 0) {
                ((Grid)sender).ClearSelection();
            }
        }

        private void gridDetails_CellClick(object sender, DataGridViewCellEventArgs e) {
            try {
                if (e.RowIndex < 0) return;
                if (((Grid)sender).Columns[e.ColumnIndex].Name == "Name" || ((Grid)sender).Columns[e.ColumnIndex].Name == "RoundIcon") {
                    LevelStats levelStats = ((Grid)sender).Rows[e.RowIndex].DataBoundItem as LevelStats;
                    using (LevelDetails levelDetails = new LevelDetails {
                        StatsForm = this,
                        LevelId = levelStats.Id,
                        LevelName = levelStats.Name,
                        RoundIcon = levelStats.RoundBigIcon,
                        RoundDetails = levelStats.Stats
                    }) {
                        this.EnableInfoStrip(false);
                        this.EnableMainMenu(false);
                        this.OnUpdatedLevelRows += levelDetails.LevelDetails_OnUpdatedLevelRows;
                        levelDetails.ShowDialog(this);
                        this.OnUpdatedLevelRows -= levelDetails.LevelDetails_OnUpdatedLevelRows;
                        this.EnableInfoStrip(true);
                        this.EnableMainMenu(true);
                    }
                } else {
                    this.CurrentSettings.ShowPercentages = !this.CurrentSettings.ShowPercentages;
                    this.SaveUserSettings();
                    this.gridDetails.Invalidate();
                }
            } catch (Exception ex) {
                this.EnableInfoStrip(true);
                this.EnableMainMenu(true);
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public List<RoundInfo> GetShowsForDisplay() {
            return this.AllStats
                    .Where(r => r.Profile == this.GetCurrentProfileId()
                                && this.IsInStatsFilter(r)
                                && this.IsInPartyFilter(r))
                    .GroupBy(r => r.ShowID)
                    .Select(g => new {
                        ShowID = g.Key,
                        SortedRounds = g.OrderBy(r => r.Round).ToList()
                    })
                    .Select(g => new RoundInfo {
                        ShowID = g.ShowID,
                        // Name = g.SortedRounds.LastOrDefault().IsFinal || g.SortedRounds.LastOrDefault().Crown ? "Final" : string.Empty,
                        Name = string.Join(";", g.SortedRounds.Select(r => r.Name)),
                        ShowNameId = string.Join(";", g.SortedRounds.Select(r => r.ShowNameId)),
                        IsFinal = g.SortedRounds.LastOrDefault().IsFinal,
                        End = g.SortedRounds.Max(r => r.End),
                        Start = g.SortedRounds.Min(r => r.Start),
                        StartLocal = g.SortedRounds.Min(r => r.StartLocal),
                        Kudos = g.SortedRounds.Sum(r => r.Kudos),
                        Qualified = g.SortedRounds.LastOrDefault().Qualified,
                        Round = g.SortedRounds.Max(r => r.Round),
                        Tier = g.SortedRounds.LastOrDefault().Qualified ? 1 : 0,
                        PrivateLobby = g.SortedRounds.LastOrDefault().PrivateLobby
                    }).ToList();
        }

        private void DisplayShows() {
            using (LevelDetails levelDetails = new LevelDetails()) {
                levelDetails.StatsForm = this;
                levelDetails.LevelName = "Shows";
                levelDetails.RoundDetails = this.GetShowsForDisplay();
                this.OnUpdatedLevelRows += levelDetails.LevelDetails_OnUpdatedLevelRows;
                levelDetails.ShowDialog(this);
                this.OnUpdatedLevelRows -= levelDetails.LevelDetails_OnUpdatedLevelRows;
            }
        }

        public List<RoundInfo> GetRoundsForDisplay() {
            return this.AllStats
                .Where(r => r.Profile == this.GetCurrentProfileId() &&
                            this.IsInStatsFilter(r) &&
                            this.IsInPartyFilter(r))
                .OrderBy(r => r.ShowID)
                .ThenBy(r => r.Round)
                .ToList();
        }

        private void DisplayRounds() {
            using (LevelDetails levelDetails = new LevelDetails()) {
                levelDetails.StatsForm = this;
                levelDetails.LevelName = "Rounds";
                levelDetails.RoundDetails = this.GetRoundsForDisplay();
                this.OnUpdatedLevelRows += levelDetails.LevelDetails_OnUpdatedLevelRows;
                levelDetails.ShowDialog(this);
                this.OnUpdatedLevelRows -= levelDetails.LevelDetails_OnUpdatedLevelRows;
            }
        }

        public List<RoundInfo> GetFinalsForDisplay() {
            return this.AllStats
                .Where(r => r.Profile == this.GetCurrentProfileId() &&
                            this.IsInStatsFilter(r) &&
                            this.IsInPartyFilter(r))
                .GroupBy(r => r.ShowID)
                .Where(g => g.Any(r => (r.Round == g.Max(x => x.Round)) && (r.IsFinal || r.Crown)))
                .SelectMany(g => g)
                .ToList();
        }

        private void DisplayFinals() {
            using (LevelDetails levelDetails = new LevelDetails()) {
                levelDetails.StatsForm = this;
                levelDetails.LevelName = "Finals";
                levelDetails.RoundDetails = this.GetFinalsForDisplay();
                this.OnUpdatedLevelRows += levelDetails.LevelDetails_OnUpdatedLevelRows;
                levelDetails.ShowDialog(this);
                this.OnUpdatedLevelRows -= levelDetails.LevelDetails_OnUpdatedLevelRows;
            }
        }

        private void DisplayWinsGraph() {
            using (WinStatsDisplay display = new WinStatsDisplay {
                StatsForm = this,
                Text = $@"     {Multilingual.GetWord("level_detail_wins_per_day")} - {this.GetCurrentProfileName().Replace("&", "&&")} ({this.GetCurrentFilterName()})",
                BackImage = Properties.Resources.crown_icon,
                BackMaxSize = 32,
                BackImagePadding = new Padding(20, 20, 0, 0)
            }) {
                List<RoundInfo> rounds = this.AllStats
                    .Where(r => r.Profile == this.GetCurrentProfileId() &&
                                this.IsInStatsFilter(r) &&
                                this.IsInPartyFilter(r))
                    .OrderBy(r => r.End).ToList();

                var dates = new ArrayList();
                var shows = new ArrayList();
                var finals = new ArrayList();
                var wins = new ArrayList();
                var winsInfo = new Dictionary<double, SortedList<string, int>>();
                if (rounds.Count > 0) {
                    DateTime start = rounds[0].StartLocal;
                    int currentShows = 0;
                    int currentFinals = 0;
                    int currentWins = 0;
                    bool isIncrementedShows = false;
                    bool isIncrementedFinals = false;
                    bool isIncrementedWins = false;
                    bool isOverDate = false;
                    foreach (RoundInfo info in rounds.Where(info => !info.PrivateLobby)) {
                        if (info.Round == 1) {
                            currentShows += isOverDate ? 2 : 1;
                            isIncrementedShows = true;
                        }

                        if (info.Crown || info.IsFinal) {
                            isOverDate = start.Date < info.StartLocal.Date;
                            currentFinals++;
                            isIncrementedFinals = true;

                            if (info.Qualified) {
                                currentWins++;
                                isIncrementedWins = true;

                                string levelName = this.StatLookup.TryGetValue(info.Name, out LevelStats l1) ? l1.Name : info.Name.Substring(0, info.Name.Length - 3);
                                if (winsInfo.TryGetValue(isOverDate ? info.StartLocal.Date.ToOADate() : start.Date.ToOADate(), out SortedList<string, int> wi)) {
                                    if (wi.ContainsKey($"{levelName};crown")) {
                                        wi[$"{levelName};crown"] += 1;
                                    } else {
                                        wi[$"{levelName};crown"] = 1;
                                    }
                                } else {
                                    winsInfo.Add(isOverDate ? info.StartLocal.Date.ToOADate() : start.Date.ToOADate(), new SortedList<string, int> { { $"{levelName};crown", 1 } });
                                }

                                if (isOverDate) {
                                    currentShows--;
                                    isIncrementedShows = false;
                                }
                            } else {
                                string levelName = this.StatLookup.TryGetValue(info.Name, out LevelStats l1) ? l1.Name : info.Name.Substring(0, info.Name.Length - 3);
                                if (winsInfo.TryGetValue(isOverDate ? info.StartLocal.Date.ToOADate() : start.Date.ToOADate(), out SortedList<string, int> wi)) {
                                    if (wi.ContainsKey($"{levelName};eliminated")) {
                                        wi[$"{levelName};eliminated"] += 1;
                                    } else {
                                        wi[$"{levelName};eliminated"] = 1;
                                    }
                                } else {
                                    winsInfo.Add(isOverDate ? info.StartLocal.Date.ToOADate() : start.Date.ToOADate(), new SortedList<string, int> { { $"{levelName};eliminated", 1 } });
                                }

                                if (isOverDate) {
                                    currentShows--;
                                    isIncrementedShows = false;
                                }
                            }
                        }

                        if (info.StartLocal.Date > start.Date && (isIncrementedShows || isIncrementedFinals)) {
                            dates.Add(start.Date.ToOADate());
                            shows.Add(Convert.ToDouble(isIncrementedShows ? --currentShows : currentShows));
                            finals.Add(Convert.ToDouble(isIncrementedFinals ? --currentFinals : currentFinals));
                            wins.Add(Convert.ToDouble(isIncrementedWins ? --currentWins : currentWins));

                            int daysWithoutStats = (int)(info.StartLocal.Date - start.Date).TotalDays - 1;
                            while (daysWithoutStats > 0) {
                                daysWithoutStats--;
                                start = start.Date.AddDays(1);
                                dates.Add(start.ToOADate());
                                shows.Add(0d);
                                finals.Add(0d);
                                wins.Add(0d);
                            }

                            currentShows = isIncrementedShows ? 1 : 0;
                            currentFinals = isIncrementedFinals ? 1 : 0;
                            currentWins = isIncrementedWins ? 1 : 0;
                            start = info.StartLocal;
                        }

                        isIncrementedShows = false;
                        isIncrementedFinals = false;
                        isIncrementedWins = false;
                    }

                    if (isOverDate) currentShows += 1;

                    dates.Add(start.Date.ToOADate());
                    shows.Add(Convert.ToDouble(currentShows));
                    finals.Add(Convert.ToDouble(currentFinals));
                    wins.Add(Convert.ToDouble(currentWins));

                    display.manualSpacing = Math.Ceiling(dates.Count / 28d);
                } else {
                    dates.Add(DateTime.Now.Date.ToOADate());
                    shows.Add(0d);
                    finals.Add(0d);
                    wins.Add(0d);

                    display.manualSpacing = 1.0;
                }
                display.dates = (double[])dates.ToArray(typeof(double));
                display.shows = (double[])shows.ToArray(typeof(double));
                display.finals = (double[])finals.ToArray(typeof(double));
                display.wins = (double[])wins.ToArray(typeof(double));
                display.winsInfo = winsInfo;

                display.graphStyle = this.CurrentSettings.WinPerDayGraphStyle;
                display.ShowDialog(this);
                if (display.graphStyle != this.CurrentSettings.WinPerDayGraphStyle) {
                    this.CurrentSettings.WinPerDayGraphStyle = display.graphStyle;
                    this.SaveUserSettings();
                }
            }
        }

        private void DisplayLevelGraph() {
            using (LevelStatsDisplay levelStatsDisplay = new LevelStatsDisplay()) {
                levelStatsDisplay.StatsForm = this;
                levelStatsDisplay.Text = $@"     {Multilingual.GetWord("level_detail_stats_by_round")} - {this.GetCurrentProfileName().Replace("&", "&&")} ({this.GetCurrentFilterName()})";
                levelStatsDisplay.BackImage = this.Theme == MetroThemeStyle.Light ? Properties.Resources.round_icon : Properties.Resources.round_gray_icon;
                levelStatsDisplay.BackMaxSize = 32;
                levelStatsDisplay.BackImagePadding = new Padding(20, 20, 0, 0);
                List<RoundInfo> rounds = this.AllStats.Where(r => r.Profile == this.GetCurrentProfileId()
                                                                  && this.IsInStatsFilter(r)
                                                                  && this.IsInPartyFilter(r))
                    .OrderBy(r => r.Name)
                    .ThenBy(r => r.Name).ToList();

                if (rounds.Count == 0) return;

                var levelMedalInfo = new Dictionary<string, double[]>();
                var levelTotalPlayTime = new Dictionary<string, TimeSpan>();
                var levelTimeInfo = new Dictionary<string, string[]>();
                var levelScoreInfo = new Dictionary<string, string[]>();
                var levelList = new Dictionary<string, string>();

                double p = 0, gm = 0, sm = 0, bm = 0, pm = 0, em = 0;
                int hs = -1, ls = int.MaxValue;
                TimeSpan pt = TimeSpan.Zero, ft = TimeSpan.MaxValue, lt = TimeSpan.Zero;
                for (int i = 0; i < rounds.Count; i++) {
                    bool isCurrentRoundInfoAvailable = this.StatLookup.TryGetValue(rounds[i].Name, out LevelStats l1);
                    if (i > 0) {
                        bool isCurrentRoundIsCreative = !isCurrentRoundInfoAvailable;
                        bool isPreviousRoundInfoAvailable = this.StatLookup.TryGetValue(rounds[i - 1].Name, out LevelStats l2);
                        bool isPreviousRoundIsCreative = !isPreviousRoundInfoAvailable;
                        if ((isCurrentRoundIsCreative && isPreviousRoundIsCreative && isCurrentRoundInfoAvailable && isPreviousRoundInfoAvailable)
                             || (!isCurrentRoundIsCreative && isPreviousRoundIsCreative)
                             || (!isCurrentRoundIsCreative && !isPreviousRoundIsCreative && !string.Equals(rounds[i].Name, rounds[i - 1].Name))
                             || (isCurrentRoundInfoAvailable && !isPreviousRoundInfoAvailable)
                             || (!isCurrentRoundInfoAvailable && isPreviousRoundInfoAvailable)
                             || (!isCurrentRoundInfoAvailable && !isPreviousRoundInfoAvailable && !string.Equals(rounds[i].Name, rounds[i - 1].Name))) {
                            string levelId = rounds[i - 1].Name;
                            string levelName = isPreviousRoundInfoAvailable ? l2.Name : rounds[i - 1].Name.Substring(0, rounds[i - 1].Name.Length - 3);
                            levelTotalPlayTime.Add(levelId, pt);
                            levelMedalInfo.Add(levelId, new[] { p, gm, sm, bm, pm, em });
                            levelTimeInfo.Add(levelId, new[] { ft < TimeSpan.MaxValue ? $"{ft:m\\:ss\\.fff}" : @"-", lt > TimeSpan.Zero ? $"{lt:m\\:ss\\.fff}" : @"-" });
                            levelScoreInfo.Add(levelId, new[] { hs >= 0 ? $"{hs}" : @"-", ls < int.MaxValue ? $"{ls}" : @"-" });
                            levelList.Add(rounds[i - 1].Name, levelName.Replace("&", "&&"));
                            pt = TimeSpan.Zero; ft = TimeSpan.MaxValue; lt = TimeSpan.Zero;
                            hs = -1; ls = int.MaxValue;
                            p = 0; gm = 0; sm = 0; bm = 0; pm = 0; em = 0;
                        }
                    }
                    TimeSpan rft = rounds[i].Finish.GetValueOrDefault(rounds[i].Start) - rounds[i].Start;
                    if (rounds[i].Finish.HasValue && rft.TotalSeconds > 1.1) {
                        ft = rft < ft ? rft : ft;
                        lt = rft > lt ? rft : lt;
                    }
                    if (rounds[i].Score.HasValue) {
                        hs = (int)(rounds[i].Score > hs ? rounds[i].Score : hs);
                        ls = (int)(rounds[i].Score < ls ? rounds[i].Score : ls);
                    }

                    pt += rounds[i].End - rounds[i].Start;
                    ++p;
                    if (rounds[i].Qualified) {
                        switch (rounds[i].Tier) {
                            case 0: ++pm; break;
                            case 1: ++gm; break;
                            case 2: ++sm; break;
                            case 3: ++bm; break;
                        }
                    } else {
                        ++em;
                    }

                    if (i == rounds.Count - 1) {
                        string levelId = rounds[i].Name;
                        string levelName = isCurrentRoundInfoAvailable ? l1.Name : rounds[i].Name.Substring(0, rounds[i].Name.Length - 3);
                        levelTotalPlayTime.Add(levelId, pt);
                        levelMedalInfo.Add(levelId, new[] { p, gm, sm, bm, pm, em });
                        levelTimeInfo.Add(levelId, new[] { ft < TimeSpan.MaxValue ? $"{ft:m\\:ss\\.fff}" : @"-", lt > TimeSpan.Zero ? $"{lt:m\\:ss\\.fff}" : @"-" });
                        levelScoreInfo.Add(levelId, new[] { hs >= 0 ? $"{hs}" : @"-", ls < int.MaxValue ? $"{ls}" : @"-" });
                        levelList.Add(rounds[i].Name, levelName.Replace("&", "&&"));
                    }
                }

                levelStatsDisplay.levelList = from pair in levelList orderby pair.Value.Trim() ascending select pair;
                levelStatsDisplay.levelTotalPlayTime = levelTotalPlayTime;
                levelStatsDisplay.levelTimeInfo = levelTimeInfo;
                levelStatsDisplay.levelScoreInfo = levelScoreInfo;
                levelStatsDisplay.levelMedalInfo = levelMedalInfo;

                levelStatsDisplay.ShowDialog(this);
            }
        }

        private void LaunchHelpInBrowser() {
            try {
                if (CurrentLanguage == Language.French) {
                    Process.Start("https://github.com/Micdu70/FinalBeansStats/tree/main/docs/fr#table-des-mati%C3%A8res");
                } else {
                    Process.Start("https://github.com/Micdu70/FinalBeansStats#table-of-contents");
                }
            } catch (Exception ex) {
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LaunchGame(bool isSilent) {
            try {
                if (!string.IsNullOrEmpty(this.CurrentSettings.GameExeLocation) && File.Exists(this.CurrentSettings.GameExeLocation)) {
                    Process[] processes = Process.GetProcesses();
                    string finalBeansProcessName = Path.GetFileNameWithoutExtension(this.CurrentSettings.GameExeLocation);
                    if (processes.Select(t => t.ProcessName).Any(name => string.Equals(name, finalBeansProcessName, StringComparison.OrdinalIgnoreCase))) {
                        if (!isSilent) {
                            MetroMessageBox.Show(this, Multilingual.GetWord("message_finalbeans_already_running"),
                                Multilingual.GetWord("message_already_running_caption"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return;
                    }

                    if (MetroMessageBox.Show(this, $"{Multilingual.GetWord("message_execution_question")}", $"{Multilingual.GetWord("message_execution_caption")}",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
                        this.UnlockGameExeFile();
                        Process.Start(this.CurrentSettings.GameExeLocation);
                        this.WindowState = FormWindowState.Minimized;
                    }
                } else if (!isSilent) {
                    MetroMessageBox.Show(this, Multilingual.GetWord("message_register_exe"),
                        Multilingual.GetWord("message_register_exe_caption"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } catch (Exception ex) {
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UnlockGameExeFile() {
            try {
                FileInfo fileInfo = new FileInfo(this.CurrentSettings.GameExeLocation);
                fileInfo.DeleteAlternateDataStream("Zone.Identifier");
            } catch {
                // ignored
            }
        }

        public void UpdateGameExeLocation() {
            string FinalBeansExeLocation = this.FindFinalBeansExeLocation();

            this.CurrentSettings.GameExeLocation = FinalBeansExeLocation;
            this.SaveUserSettings();
        }

        public string FindFinalBeansExeLocation() {
            try {
                string gamePath;
                // Try to get the game path from the game log file (at the second line)
                string filePath = !string.IsNullOrEmpty(this.CurrentSettings.LogPath) && Directory.Exists(this.CurrentSettings.LogPath) ? this.CurrentSettings.LogPath : LOGPATH;
                filePath = Path.Combine(filePath, LOGFILENAME);
                if (File.Exists(filePath)) {
                    using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                        using (var sr = new StreamReader(fs)) {
                            int lineNum = 0;
                            string line;
                            while ((line = sr.ReadLine()) != null) {
                                lineNum++;
                                if (lineNum == 1) continue; // Skip first line

                                if (line.StartsWith("[Subsystems] Discovering subsystems at path ", StringComparison.OrdinalIgnoreCase)) {
                                    int index = line.IndexOf(" at path ", StringComparison.OrdinalIgnoreCase) + 9;
                                    int index2 = line.LastIndexOf("FinalBeans_Data", StringComparison.OrdinalIgnoreCase);
                                    gamePath = Path.Combine(line.Substring(index, index2 - index), "FinalBeans.exe");
                                    if (File.Exists(gamePath)) { return gamePath; }
                                }
                                break; // Only read the second line
                            }
                        }
                    }
                }
                // Else try to get the game path from the launcher (settings file)
                filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "finalbeanslauncher", "settings.json");
                if (File.Exists(filePath)) {
                    using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                        using (var sr = new StreamReader(fs)) {
                            string line;
                            while ((line = sr.ReadLine()) != null) {
                                if (line.IndexOf("\"installPath\":", StringComparison.OrdinalIgnoreCase) != -1) {
                                    MatchCollection mc = Regex.Matches(line, @"""([^""]*)""");
                                    if (mc.Count == 1) break; // "installPath" value is null

                                    gamePath = Path.Combine(mc[1].ToString().Replace("\"", ""), "FinalBeans.exe");
                                    if (File.Exists(gamePath)) { return gamePath; }
                                    break;
                                }
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return string.Empty;
        }

        private void EnableMainMenu(bool enable) {
            this.menuSettings.Enabled = enable;
            this.menuFilters.Enabled = enable;
            this.menuProfile.Enabled = enable;
            this.menuUpdate.Enabled = enable;
            this.menuLaunchFinalBeans.Enabled = enable;
            this.lblPlayerName.Enabled = enable;
            if (enable) {
                this.menuSettings.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                this.menuSettings.Image = this.Theme == MetroThemeStyle.Light ? Properties.Resources.setting_icon : Properties.Resources.setting_gray_icon;
            }
            if (this.trayIcon.Visible) {
                this.traySettings.Enabled = enable;
                this.trayFilters.Enabled = enable;
                this.trayProfile.Enabled = enable;
                this.trayUpdate.Enabled = enable;
                this.trayLaunchFinalBeans.Enabled = enable;
                this.trayExitProgram.Enabled = enable;
                if (enable) {
                    this.traySettings.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Black : Color.DarkGray;
                    this.traySettings.Image = this.Theme == MetroThemeStyle.Light ? Properties.Resources.setting_icon : Properties.Resources.setting_gray_icon;
                }
            }
        }

        private void EnableInfoStrip(bool enable) {
            this.infoStrip.Enabled = enable;
            this.infoStrip2.Enabled = enable;
            this.infoStrip3.Enabled = enable;
            this.lblTotalTime.Enabled = enable;
            if (enable) this.lblTotalTime.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.Blue : Color.Orange;
            foreach (var tsi in this.infoStrip.Items) {
                if (tsi is ToolStripLabel tsl) {
                    tsl.Enabled = enable;
                    if (enable) {
                        this.Cursor = Cursors.Default;
                        tsl.ForeColor = tsl.Equals(this.lblCurrentProfile) || tsl.Equals(this.lblPlayerName)
                            ? this.Theme == MetroThemeStyle.Light ? Color.Red : Color.FromArgb(0, 192, 192)
                            : this.Theme == MetroThemeStyle.Light ? Color.Blue : Color.Orange;
                    }
                }
            }
        }

        private void Stats_KeyUp(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.ControlKey:
                    this.ctrlKeyToggle = false;
                    break;
                case Keys.ShiftKey:
                    this.shiftKeyToggle = false;
                    break;
            }
        }

        private void Stats_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.ControlKey:
                    this.ctrlKeyToggle = true;
                    break;
                case Keys.ShiftKey:
                    this.shiftKeyToggle = true;
                    break;
            }

            switch (e.Control) {
                case true when e.KeyCode == Keys.M:
                    this.CurrentSettings.OverlayNotOnTop = !this.CurrentSettings.OverlayNotOnTop;
                    this.SetOverlayTopMost(!this.CurrentSettings.OverlayNotOnTop);
                    this.SaveUserSettings();
                    break;
                case true when e.KeyCode == Keys.T:
                    int colorOption = 0;
                    if (this.overlay.BackColor.ToArgb() == Color.FromArgb(224, 224, 224).ToArgb()) {
                        colorOption = 1;
                    } else if (this.overlay.BackColor.ToArgb() == Color.White.ToArgb()) {
                        colorOption = 2;
                    } else if (this.overlay.BackColor.ToArgb() == Color.Black.ToArgb()) {
                        colorOption = 3;
                    } else if (this.overlay.BackColor.ToArgb() == Color.Magenta.ToArgb()) {
                        colorOption = 4;
                    } else if (this.overlay.BackColor.ToArgb() == Color.Red.ToArgb()) {
                        colorOption = 5;
                    } else if (this.overlay.BackColor.ToArgb() == Color.Green.ToArgb()) {
                        colorOption = 6;
                    } else if (this.overlay.BackColor.ToArgb() == Color.Blue.ToArgb()) {
                        colorOption = 0;
                    }
                    this.overlay.SetBackgroundColor(colorOption);
                    this.CurrentSettings.OverlayColor = colorOption;
                    this.SaveUserSettings();
                    break;
                case true when e.KeyCode == Keys.F:
                    if (!this.overlay.IsFixed()) {
                        this.overlay.FlipDisplay(!this.overlay.flippedImage);
                        this.CurrentSettings.FlippedDisplay = this.overlay.flippedImage;
                        this.SaveUserSettings();
                        this.overlay.UpdateDisplay();
                    }
                    break;
                case true when e.KeyCode == Keys.R:
                    this.CurrentSettings.ColorByRoundType = !this.CurrentSettings.ColorByRoundType;
                    this.SaveUserSettings();
                    this.overlay.UpdateDisplay();
                    break;
                case true when e.KeyCode == Keys.C:
                    this.CurrentSettings.PlayerByConsoleType = !this.CurrentSettings.PlayerByConsoleType;
                    this.SaveUserSettings();
                    this.overlay.UpdateDisplay();
                    break;
                case false when e.KeyCode == Keys.ControlKey:
                    this.ctrlKeyToggle = true;
                    break;
                case false when e.KeyCode == Keys.ShiftKey:
                    this.shiftKeyToggle = true;
                    break;
                case true when e.Shift && e.KeyCode == Keys.Z:
                    this.SetAutoChangeProfile(!this.CurrentSettings.AutoChangeProfile);
                    break;
                case true when e.Shift && e.KeyCode == Keys.X:
                    this.overlay.ResetOverlaySize();
                    break;
                case true when e.Shift && e.KeyCode == Keys.C:
                    this.overlay.ResetOverlayLocation();
                    break;
                case true when e.Shift && e.KeyCode == Keys.Up:
                    this.SetOverlayBackgroundOpacity(this.CurrentSettings.OverlayBackgroundOpacity + 5);
                    break;
                case true when e.Shift && e.KeyCode == Keys.Down:
                    this.SetOverlayBackgroundOpacity(this.CurrentSettings.OverlayBackgroundOpacity - 5);
                    break;
            }
            e.SuppressKeyPress = true;
        }

        private void lblCurrentProfileIcon_Click(object sender, EventArgs e) {
            this.SetAutoChangeProfile(!this.CurrentSettings.AutoChangeProfile);
            this.HideCustomTooltip(this);
        }

        private void lblCurrentProfile_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                for (int i = 0; i < this.ProfileMenuItems.Count; i++) {
                    if (!(this.ProfileMenuItems[i] is ToolStripMenuItem menuItem)) continue;
                    if (this.shiftKeyToggle) {
                        if (menuItem.Checked && i - 1 >= 0) {
                            this.menuStats_Click(this.ProfileMenuItems[i - 1], EventArgs.Empty);
                            break;
                        }
                        if (menuItem.Checked && i - 1 < 0) {
                            this.menuStats_Click(this.ProfileMenuItems[this.ProfileMenuItems.Count - 1], EventArgs.Empty);
                            break;
                        }
                    } else {
                        if (menuItem.Checked && i + 1 < this.ProfileMenuItems.Count) {
                            this.menuStats_Click(this.ProfileMenuItems[i + 1], EventArgs.Empty);
                            break;
                        }
                        if (menuItem.Checked && i + 1 >= this.ProfileMenuItems.Count) {
                            this.menuStats_Click(this.ProfileMenuItems[0], EventArgs.Empty);
                            break;
                        }
                    }
                }
            } else if (e.Button == MouseButtons.Right) {
                for (int i = 0; i < this.ProfileMenuItems.Count; i++) {
                    if (!(this.ProfileMenuItems[i] is ToolStripMenuItem menuItem)) continue;
                    if (menuItem.Checked && i - 1 >= 0) {
                        this.menuStats_Click(this.ProfileMenuItems[i - 1], EventArgs.Empty);
                        break;
                    }
                    if (menuItem.Checked && i - 1 < 0) {
                        this.menuStats_Click(this.ProfileMenuItems[this.ProfileMenuItems.Count - 1], EventArgs.Empty);
                        break;
                    }
                }
            }
        }

        private void lblTotalTime_Click(object sender, EventArgs e) {
            try {
                this.EnableInfoStrip(false);
                this.EnableMainMenu(false);
                this.DisplayLevelGraph();
                this.EnableInfoStrip(true);
                this.EnableMainMenu(true);
            } catch (Exception ex) {
                this.EnableInfoStrip(true);
                this.EnableMainMenu(true);
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblTotalFinals_Click(object sender, EventArgs e) {
            try {
                this.EnableInfoStrip(false);
                this.EnableMainMenu(false);
                this.DisplayFinals();
                this.EnableInfoStrip(true);
                this.EnableMainMenu(true);
            } catch (Exception ex) {
                this.EnableInfoStrip(true);
                this.EnableMainMenu(true);
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblTotalShows_Click(object sender, EventArgs e) {
            try {
                this.EnableInfoStrip(false);
                this.EnableMainMenu(false);
                this.DisplayShows();
                this.EnableInfoStrip(true);
                this.EnableMainMenu(true);
            } catch (Exception ex) {
                this.EnableInfoStrip(true);
                this.EnableMainMenu(true);
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblTotalRounds_Click(object sender, EventArgs e) {
            try {
                this.EnableInfoStrip(false);
                this.EnableMainMenu(false);
                this.DisplayRounds();
                this.EnableInfoStrip(true);
                this.EnableMainMenu(true);
            } catch (Exception ex) {
                this.EnableInfoStrip(true);
                this.EnableMainMenu(true);
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblTotalWins_Click(object sender, EventArgs e) {
            try {
                this.EnableInfoStrip(false);
                this.EnableMainMenu(false);
                this.DisplayWinsGraph();
                this.EnableInfoStrip(true);
                this.EnableMainMenu(true);
            } catch (Exception ex) {
                this.EnableInfoStrip(true);
                this.EnableMainMenu(true);
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void lblPlayerName_Click(object sender, EventArgs e) {
            try {
                using (ChangePlayerName changePlayerName = new ChangePlayerName()) {
                    changePlayerName.CurrentSettings = this.CurrentSettings;
                    changePlayerName.BackImage = Properties.Resources.finalbeans_icon;
                    changePlayerName.BackMaxSize = 32;
                    changePlayerName.BackImagePadding = new Padding(20, 19, 0, 0);
                    string lastPlayerName = this.CurrentSettings.PlayerName ?? string.Empty;
                    this.EnableInfoStrip(false);
                    this.EnableMainMenu(false);
                    if (changePlayerName.ShowDialog(this) == DialogResult.OK) {
                        this.CurrentSettings = changePlayerName.CurrentSettings;
                        if (!string.Equals(this.CurrentSettings.PlayerName, lastPlayerName)) {
                            this.SaveUserSettings();
                            this.UpdatePlayerNameInfo();
                            await this.logFile.Stop();
                            this.askedPreviousShows = 0;
                            string logPath = !string.IsNullOrEmpty(this.CurrentSettings.LogPath) && Directory.Exists(this.CurrentSettings.LogPath) ? this.CurrentSettings.LogPath : LOGPATH;
                            this.logFile.Start(logPath, LOGFILENAME);
                        }
                    }
                    this.EnableInfoStrip(true);
                    this.EnableMainMenu(true);
                }
            } catch (Exception ex) {
                this.EnableInfoStrip(true);
                this.EnableMainMenu(true);
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mlReportBug_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/Micdu70/FinalBeansStats/issues/new");
        }

        public void menuStats_Click(object sender, EventArgs e) {
            try {
                ToolStripMenuItem button = sender as ToolStripMenuItem;
                if (Equals(button, this.menuCustomRangeStats) || Equals(button, this.trayCustomRangeStats)) {
                    if (this.isStartingUp) {
                        this.updateFilterRange = true;
                    } else {
                        this.EnableInfoStrip(false);
                        this.EnableMainMenu(false);
                        using (FilterCustomRange filterCustomRange = new FilterCustomRange()) {
                            filterCustomRange.StatsForm = this;
                            filterCustomRange.startDate = this.customfilterRangeStart;
                            filterCustomRange.endDate = this.customfilterRangeEnd;
                            filterCustomRange.selectedCustomTemplateSeason = this.selectedCustomTemplateSeason;
                            if (filterCustomRange.ShowDialog(this) == DialogResult.OK) {
                                this.menuCustomRangeStats.Checked = true;
                                this.menuAllStats.Checked = false;
                                this.menuSeasonStats.Checked = false;
                                this.menuWeekStats.Checked = false;
                                this.menuDayStats.Checked = false;
                                this.menuSessionStats.Checked = false;
                                this.trayCustomRangeStats.Checked = true;
                                this.trayAllStats.Checked = false;
                                this.traySeasonStats.Checked = false;
                                this.trayWeekStats.Checked = false;
                                this.trayDayStats.Checked = false;
                                this.traySessionStats.Checked = false;
                                this.selectedCustomTemplateSeason = filterCustomRange.selectedCustomTemplateSeason;
                                this.customfilterRangeStart = filterCustomRange.startDate;
                                this.customfilterRangeEnd = filterCustomRange.endDate;
                                this.updateFilterRange = true;
                            } else {
                                this.EnableInfoStrip(true);
                                this.EnableMainMenu(true);
                                return;
                            }
                        }
                        this.EnableInfoStrip(true);
                        this.EnableMainMenu(true);
                    }
                } else if (Equals(button, this.menuAllStats) || Equals(button, this.menuSeasonStats) || Equals(button, this.menuWeekStats) || Equals(button, this.menuDayStats) || Equals(button, this.menuSessionStats)) {
                    if (!this.menuAllStats.Checked && !this.menuSeasonStats.Checked && !this.menuWeekStats.Checked && !this.menuDayStats.Checked && !this.menuSessionStats.Checked) {
                        button.Checked = true;
                        switch (button.Name) {
                            case "menuCustomRangeStats":
                                this.trayCustomRangeStats.Checked = true; break;
                            case "menuAllStats":
                                this.trayAllStats.Checked = true; break;
                            case "menuSeasonStats":
                                this.traySeasonStats.Checked = true; break;
                            case "menuWeekStats":
                                this.trayWeekStats.Checked = true; break;
                            case "menuDayStats":
                                this.trayDayStats.Checked = true; break;
                            case "menuSessionStats":
                                this.traySessionStats.Checked = true; break;
                        }
                        return;
                    }
                    this.updateFilterType = true;
                    this.updateFilterRange = false;

                    foreach (var item in this.menuStatsFilter.DropDownItems) {
                        if (item is ToolStripMenuItem menuItem) {
                            if (menuItem != null && menuItem.Checked && menuItem != button) {
                                menuItem.Checked = false;
                                switch (menuItem.Name) {
                                    case "menuCustomRangeStats":
                                        this.trayCustomRangeStats.Checked = false; break;
                                    case "menuAllStats":
                                        this.trayAllStats.Checked = false; break;
                                    case "menuSeasonStats":
                                        this.traySeasonStats.Checked = false; break;
                                    case "menuWeekStats":
                                        this.trayWeekStats.Checked = false; break;
                                    case "menuDayStats":
                                        this.trayDayStats.Checked = false; break;
                                    case "menuSessionStats":
                                        this.traySessionStats.Checked = false; break;
                                }
                            }

                            if (menuItem.Checked) {
                                switch (menuItem.Name) {
                                    case "menuCustomRangeStats":
                                        this.trayCustomRangeStats.Checked = true; break;
                                    case "menuAllStats":
                                        this.trayAllStats.Checked = true; break;
                                    case "menuSeasonStats":
                                        this.traySeasonStats.Checked = true; break;
                                    case "menuWeekStats":
                                        this.trayWeekStats.Checked = true; break;
                                    case "menuDayStats":
                                        this.trayDayStats.Checked = true; break;
                                    case "menuSessionStats":
                                        this.traySessionStats.Checked = true; break;
                                }
                            }
                        }
                    }
                } else if (Equals(button, this.menuAllPartyStats) || Equals(button, this.menuSoloStats) || Equals(button, this.menuPartyStats)) {
                    if (!this.menuAllPartyStats.Checked && !this.menuSoloStats.Checked && !this.menuPartyStats.Checked) {
                        button.Checked = true;
                        switch (button.Name) {
                            case "menuAllPartyStats":
                                this.trayAllPartyStats.Checked = true; break;
                            case "menuSoloStats":
                                this.traySoloStats.Checked = true; break;
                            case "menuPartyStats":
                                this.trayPartyStats.Checked = true; break;
                        }
                        return;
                    }

                    foreach (var item in this.menuPartyFilter.DropDownItems) {
                        if (item is ToolStripMenuItem menuItem) {
                            if (menuItem != null && menuItem.Checked && menuItem != button) {
                                menuItem.Checked = false;
                                switch (menuItem.Name) {
                                    case "menuAllPartyStats":
                                        this.trayAllPartyStats.Checked = false; break;
                                    case "menuSoloStats":
                                        this.traySoloStats.Checked = false; break;
                                    case "menuPartyStats":
                                        this.trayPartyStats.Checked = false; break;
                                }
                            }

                            if (menuItem.Checked) {
                                switch (menuItem.Name) {
                                    case "menuAllPartyStats":
                                        this.trayAllPartyStats.Checked = true; break;
                                    case "menuSoloStats":
                                        this.traySoloStats.Checked = true; break;
                                    case "menuPartyStats":
                                        this.trayPartyStats.Checked = true; break;
                                }
                            }
                        }
                    }
                } else if (this.ProfileMenuItems.Contains(button)) {
                    if (this.AllProfiles.Count != 0) {
                        for (int i = this.ProfileMenuItems.Count - 1; i >= 0; i--) {
                            if (this.ProfileMenuItems[i].Name == button.Name) {
                                this.SetCurrentProfileIcon(this.AllProfiles.FindIndex(p => string.Equals(p.ProfileName, this.ProfileMenuItems[i].Text.Replace("&&", "&")) && !string.IsNullOrEmpty(p.LinkedShowId)) != -1);
                            }
                            this.ProfileMenuItems[i].Checked = this.ProfileMenuItems[i].Name == button.Name;
                            this.ProfileTrayItems[i].Checked = this.ProfileTrayItems[i].Name == button.Name;
                        }
                    }
                    this.currentProfile = this.GetProfileIdByName(button.Text.Replace("&&", "&"));
                    this.updateSelectedProfile = true;
                } else if (Equals(button, this.trayAllStats) || Equals(button, this.traySeasonStats) || Equals(button, this.trayWeekStats) || Equals(button, this.trayDayStats) || Equals(button, this.traySessionStats)) {
                    if (!this.trayAllStats.Checked && !this.traySeasonStats.Checked && !this.trayWeekStats.Checked && !this.trayDayStats.Checked && !this.traySessionStats.Checked) {
                        button.Checked = true;
                        switch (button.Name) {
                            case "trayCustomRangeStats":
                                this.menuCustomRangeStats.Checked = true; break;
                            case "trayAllStats":
                                this.menuAllStats.Checked = true; break;
                            case "traySeasonStats":
                                this.menuSeasonStats.Checked = true; break;
                            case "trayWeekStats":
                                this.menuWeekStats.Checked = true; break;
                            case "trayDayStats":
                                this.menuDayStats.Checked = true; break;
                            case "traySessionStats":
                                this.menuSessionStats.Checked = true; break;
                        }
                        return;
                    }
                    this.updateFilterType = true;
                    this.updateFilterRange = false;

                    foreach (var item in this.trayStatsFilter.DropDownItems) {
                        if (item is ToolStripMenuItem menuItem) {
                            if (menuItem != null && menuItem.Checked && menuItem != button) {
                                menuItem.Checked = false;
                                switch (menuItem.Name) {
                                    case "trayCustomRangeStats":
                                        this.menuCustomRangeStats.Checked = false; break;
                                    case "trayAllStats":
                                        this.menuAllStats.Checked = false; break;
                                    case "traySeasonStats":
                                        this.menuSeasonStats.Checked = false; break;
                                    case "trayWeekStats":
                                        this.menuWeekStats.Checked = false; break;
                                    case "trayDayStats":
                                        this.menuDayStats.Checked = false; break;
                                    case "traySessionStats":
                                        this.menuSessionStats.Checked = false; break;
                                }
                            }

                            if (menuItem.Checked) {
                                switch (menuItem.Name) {
                                    case "trayCustomRangeStats":
                                        this.menuCustomRangeStats.Checked = true; break;
                                    case "trayAllStats":
                                        this.menuAllStats.Checked = true; break;
                                    case "traySeasonStats":
                                        this.menuSeasonStats.Checked = true; break;
                                    case "trayWeekStats":
                                        this.menuWeekStats.Checked = true; break;
                                    case "trayDayStats":
                                        this.menuDayStats.Checked = true; break;
                                    case "traySessionStats":
                                        this.menuSessionStats.Checked = true; break;
                                }
                            }
                        }
                    }
                } else if (Equals(button, this.trayAllPartyStats) || Equals(button, this.traySoloStats) || Equals(button, this.trayPartyStats)) {
                    if (!this.trayAllPartyStats.Checked && !this.traySoloStats.Checked && !this.trayPartyStats.Checked) {
                        button.Checked = true;
                        switch (button.Name) {
                            case "trayAllPartyStats":
                                this.menuAllPartyStats.Checked = true; break;
                            case "traySoloStats":
                                this.menuSoloStats.Checked = true; break;
                            case "trayPartyStats":
                                this.menuPartyStats.Checked = true; break;
                        }
                        return;
                    }

                    foreach (var item in this.trayPartyFilter.DropDownItems) {
                        if (item is ToolStripMenuItem menuItem) {
                            if (menuItem != null && menuItem.Checked && menuItem != button) {
                                menuItem.Checked = false;
                                switch (menuItem.Name) {
                                    case "trayAllPartyStats":
                                        this.menuAllPartyStats.Checked = false; break;
                                    case "traySoloStats":
                                        this.menuSoloStats.Checked = false; break;
                                    case "trayPartyStats":
                                        this.menuPartyStats.Checked = false; break;
                                }
                            }

                            if (menuItem.Checked) {
                                switch (menuItem.Name) {
                                    case "trayAllPartyStats":
                                        this.menuAllPartyStats.Checked = true; break;
                                    case "traySoloStats":
                                        this.menuSoloStats.Checked = true; break;
                                    case "trayPartyStats":
                                        this.menuPartyStats.Checked = true; break;
                                }
                            }
                        }
                    }
                } else if (this.ProfileTrayItems.Contains(button)) {
                    if (this.AllProfiles.Count != 0) {
                        for (int i = this.ProfileTrayItems.Count - 1; i >= 0; i--) {
                            if (this.ProfileTrayItems[i].Name == button.Name) {
                                this.SetCurrentProfileIcon(this.AllProfiles.FindIndex(p => string.Equals(p.ProfileName, this.ProfileTrayItems[i].Text.Replace("&&", "&")) && !string.IsNullOrEmpty(p.LinkedShowId)) != -1);
                            }
                            this.ProfileTrayItems[i].Checked = this.ProfileTrayItems[i].Name == button.Name;
                            this.ProfileMenuItems[i].Checked = this.ProfileMenuItems[i].Name == button.Name;
                        }
                    }
                    this.currentProfile = this.GetProfileIdByName(button.Text.Replace("&&", "&"));
                    this.updateSelectedProfile = true;
                }

                foreach (LevelStats calculator in this.StatDetails) {
                    calculator.Clear();
                }

                this.ClearTotals();

                int profile = this.currentProfile;

                List<RoundInfo> rounds;
                if (this.menuCustomRangeStats.Checked) {
                    rounds = this.AllStats.Where(roundInfo => roundInfo.Start >= this.customfilterRangeStart &&
                                                              roundInfo.Start <= this.customfilterRangeEnd &&
                                                              roundInfo.Profile == profile && this.IsInPartyFilter(roundInfo)).ToList();
                } else {
                    DateTime compareDate = this.menuAllStats.Checked ? DateTime.MinValue :
                                           this.menuSeasonStats.Checked ? SeasonStart :
                                           this.menuWeekStats.Checked ? WeekStart :
                                           this.menuDayStats.Checked ? DayStart : SessionStart;
                    rounds = this.AllStats.Where(roundInfo => roundInfo.Start > compareDate && roundInfo.Profile == profile && this.IsInPartyFilter(roundInfo)).ToList();
                }

                rounds.Sort();

                if (!this.isStartingUp && this.updateFilterType) {
                    this.updateFilterType = false;
                    this.CurrentSettings.FilterType = this.menuSeasonStats.Checked ? 2 :
                                                      this.menuWeekStats.Checked ? 3 :
                                                      this.menuDayStats.Checked ? 4 :
                                                      this.menuSessionStats.Checked ? 5 : 1;
                    this.CurrentSettings.SelectedCustomTemplateSeason = -1;
                    this.CurrentSettings.CustomFilterRangeStart = DateTime.MinValue;
                    this.CurrentSettings.CustomFilterRangeEnd = DateTime.MaxValue;
                    this.SaveUserSettings();
                } else if (!this.isStartingUp && this.updateFilterRange) {
                    this.updateFilterRange = false;
                    this.CurrentSettings.FilterType = 0;
                    this.CurrentSettings.SelectedCustomTemplateSeason = this.selectedCustomTemplateSeason;
                    this.CurrentSettings.CustomFilterRangeStart = this.customfilterRangeStart;
                    this.CurrentSettings.CustomFilterRangeEnd = this.customfilterRangeEnd;
                    this.SaveUserSettings();
                } else if (!this.isStartingUp && this.updateSelectedProfile) {
                    this.updateSelectedProfile = false;
                    this.CurrentSettings.SelectedProfile = profile;
                    this.SaveUserSettings();
                }

                this.overlay.UpdateDisplay();

                this.loadingExisting = true;
                this.LogFile_OnParsedLogLines(rounds);
                this.loadingExisting = false;
            } catch (Exception ex) {
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuUpdate_Click(object sender, EventArgs e) {
            try {
                if (Utils.IsInternetConnected()) {
                    this.EnableInfoStrip(false);
                    this.EnableMainMenu(false);
                    if (this.CheckForUpdate(false)) {
                        return;
                    }
                    this.EnableInfoStrip(true);
                    this.EnableMainMenu(true);
                } else {
                    MetroMessageBox.Show(this, $"{Multilingual.GetWord("message_check_internet_connection")}", $"{Multilingual.GetWord("message_check_internet_connection_caption")}",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } catch (Exception ex) {
                this.EnableInfoStrip(true);
                this.EnableMainMenu(true);
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_update_error_caption")}",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

#if AllowUpdate
        public void ChangeStateForAvailableNewVersion(string newVersion) {
            this.isAvailableNewVersion = true;
            this.availableNewVersion = newVersion;
            this.menuUpdate.Image = CurrentTheme == MetroThemeStyle.Light ? Properties.Resources.github_update_icon : Properties.Resources.github_update_gray_icon;
            this.trayUpdate.Image = CurrentTheme == MetroThemeStyle.Light ? Properties.Resources.github_update_icon : Properties.Resources.github_update_gray_icon;
            this.menuUpdate.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.FromArgb(0, 174, 219) : Color.GreenYellow;
            this.trayUpdate.ForeColor = this.Theme == MetroThemeStyle.Light ? Color.FromArgb(0, 174, 219) : Color.GreenYellow;
        }

        private bool CheckForNewVersion() {
            using (ZipWebClient web = new ZipWebClient()) {
                try {
                    string assemblyInfo = web.DownloadString(@"https://raw.githubusercontent.com/Micdu70/FinalBeansStats/main/Properties/AssemblyInfo.cs");
                    int index = assemblyInfo.IndexOf("AssemblyVersion(");
                    if (index > 0) {
                        int indexEnd = assemblyInfo.IndexOf("\")", index);
                        Version currentVersion = Assembly.GetEntryAssembly().GetName().Version;
                        Version newVersion = new Version(assemblyInfo.Substring(index + 17, indexEnd - index - 17));
                        if (newVersion > currentVersion) {
                            this.ChangeStateForAvailableNewVersion(newVersion.ToString(2));
                            return true;
                        }
                    }
                } catch {
                    return false;
                }
            }
            return false;
        }

        private void CheckForNewVersionJob(bool retry = false) {
            double interval;
            if (retry) {
                interval = 5 * 1000;
            } else {
                interval = 24 * 60 * 60 * 1000;
            }
            TimerAbsolute checkForNewVersionTimer = new TimerAbsolute((s, e) => {
                Task.Run(() => {
                    if (!Utils.IsInternetConnected()) {
                        this.CheckForNewVersionJob(true);
                        return;
                    }

                    this.CheckForNewVersion();
                    this.CheckForNewVersionJob();
                });
            });
            checkForNewVersionTimer.Start(interval);
        }
#endif

        private bool CheckForUpdate(bool isSilent) {
#if AllowUpdate
            using (ZipWebClient web = new ZipWebClient()) {
                try {
                    string assemblyInfo = web.DownloadString(@"https://raw.githubusercontent.com/Micdu70/FinalBeansStats/main/Properties/AssemblyInfo.cs");
                    int index = assemblyInfo.IndexOf("AssemblyVersion(");
                    if (index > 0) {
                        int indexEnd = assemblyInfo.IndexOf("\")", index);
                        Version currentVersion = Assembly.GetEntryAssembly().GetName().Version;
                        Version newVersion = new Version(assemblyInfo.Substring(index + 17, indexEnd - index - 17));
                        if (newVersion > currentVersion) {
                            this.ChangeStateForAvailableNewVersion(newVersion.ToString(2));
                            if (MetroMessageBox.Show(this,
                                                     $"{Multilingual.GetWord("message_update_question_prefix")} [ v{newVersion.ToString(2)} ] {Multilingual.GetWord("message_update_question_suffix")}",
                                                     $"{Multilingual.GetWord("message_update_question_caption")}",
                                                     MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
                                Task.Run(() => {
                                    IsExitingForUpdate = true;
                                    IsUpdatingOnAppLaunch = isSilent;
                                    this.Stats_ExitProgram(this, null);
                                    this.UpdateProgram(web);
                                });
                                return true;
                            }
                        } else if (!isSilent) {
                            MetroMessageBox.Show(this,
                                $"{Multilingual.GetWord("message_update_latest_version")}" +
                                $"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}" +
                                $"{Multilingual.GetWord("main_update_prefix_tooltip").Trim()}{Environment.NewLine}{Multilingual.GetWord("main_update_suffix_tooltip").Trim()}",
                                $"{Multilingual.GetWord("message_update_question_caption")}",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    } else if (!isSilent) {
                        MetroMessageBox.Show(this, $"{Multilingual.GetWord("message_update_not_determine_version")}",
                            $"{Multilingual.GetWord("message_update_error_caption")}",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } catch {
                    return false;
                }
            }
#else
            this.LaunchHelpInBrowser();
#endif
            return false;
        }

#if AllowUpdate
        public void UpdateProgram(ZipWebClient web) {
            using (DownloadProgress updater = new DownloadProgress()) {
                updater.CurrentExeName = Path.GetFileName(Assembly.GetEntryAssembly().Location);
                updater.ZipWebClient = web;
                updater.DownloadUrl = Utils.FINALBEANSSTATS_RELEASES_LATEST_DOWNLOAD_URL;
                updater.ZipFileName = $"{CURRENTDIR}FinalBeansStats.zip";
                updater.ShowDialog(this);
            }
        }
#endif

        private void SetSystemTrayIcon(bool enable) {
            this.trayIcon.Visible = enable;
            if (!enable && !this.Visible) {
                this.Visible = true;
                this.CurrentSettings.Visible = true;
            }
        }

        public void SetOverlayTopMost(bool topMost) {
            this.overlay.TopMost = topMost;
            if (this.overlay.Visible) {
                this.overlay.Hide();
                this.overlay.ShowInTaskbar = !topMost;
                this.overlay.Show();
            } else {
                this.overlay.ShowInTaskbar = !topMost;
            }
            this.overlay.UpdateDisplay();
        }

        public void SetAutoChangeProfile(bool autoChangeProfile) {
            this.CurrentSettings.AutoChangeProfile = autoChangeProfile;
            if (this.AllProfiles.Count != 0) {
                this.SetCurrentProfileIcon(this.AllProfiles.FindIndex(p => p.ProfileId == this.GetCurrentProfileId() && !string.IsNullOrEmpty(p.LinkedShowId)) != -1);
            }
            this.SaveUserSettings();
        }

        public void SetOverlayBackgroundOpacity(int opacity) {
            if (opacity > 100) { opacity = 100; }
            if (opacity < 0) { opacity = 0; }
            this.CurrentSettings.OverlayBackgroundOpacity = opacity;
            this.overlay.Opacity = opacity / 100d;
            this.SaveUserSettings();
            this.overlay.UpdateDisplay();
        }

        private void SetMinimumSize() {
            this.MinimumSize = new Size(CurrentLanguage == Language.English || CurrentLanguage == Language.French ? 905 :
                                        CurrentLanguage == Language.Korean ? 740 :
                                        CurrentLanguage == Language.Japanese ? 835 : 715
                                        , 350);
        }

        private async void menuSettings_Click(object sender, EventArgs e) {
            try {
                using (Settings settings = new Settings()) {
                    //settings.Icon = this.Icon;
                    settings.CurrentSettings = this.CurrentSettings;
                    settings.BackMaxSize = 32;
                    settings.BackImagePadding = new Padding(20, 19, 0, 0);
                    settings.StatsForm = this;
                    settings.Overlay = this.overlay;
                    string lastLogPath = this.CurrentSettings.LogPath ?? string.Empty;
                    this.EnableInfoStrip(false);
                    this.EnableMainMenu(false);
                    if (settings.ShowDialog(this) == DialogResult.OK) {
                        this.CurrentSettings = settings.CurrentSettings;

                        IpGeolocationService = this.CurrentSettings.IpGeolocationService;

                        this.SetSystemTrayIcon(this.CurrentSettings.SystemTrayIcon);
                        this.SetTheme(CurrentTheme);
                        this.SaveUserSettings();
                        if (this.currentLanguage != (int)CurrentLanguage) {
                            this.SetMinimumSize();
                            this.ChangeLanguage();
                            this.UpdateTotals();
                            this.gridDetails.ChangeContextMenuLanguage();
                            this.UpdateGridRoundName();
                            this.overlay.ChangeLanguage();
                        }
                        this.SortGridDetails(true);
                        this.SetOverlayTopMost(!this.CurrentSettings.OverlayNotOnTop);
                        this.SetOverlayBackgroundOpacity(this.CurrentSettings.OverlayBackgroundOpacity);
                        this.overlay.SetBackgroundResourcesName(this.CurrentSettings.OverlayBackgroundResourceName, this.CurrentSettings.OverlayTabResourceName);
                        if (this.AllProfiles.Count != 0) {
                            this.SetCurrentProfileIcon(this.AllProfiles.FindIndex(p => p.ProfileId == this.GetCurrentProfileId() && !string.IsNullOrEmpty(p.LinkedShowId)) != -1);
                        }
                        this.Invalidate();

                        IsDisplayOverlayPing = this.CurrentSettings.OverlayVisible && !this.CurrentSettings.HideRoundInfo && (this.CurrentSettings.SwitchBetweenPlayers || this.CurrentSettings.OnlyShowPing);
                        IsOverlayRoundInfoNeedRefresh = true;

                        if (!string.Equals(this.CurrentSettings.LogPath, lastLogPath, StringComparison.OrdinalIgnoreCase)) {
                            await this.logFile.Stop();
                            string logPath = !string.IsNullOrEmpty(this.CurrentSettings.LogPath) && Directory.Exists(this.CurrentSettings.LogPath) ? this.CurrentSettings.LogPath : LOGPATH;
                            this.logFile.Start(logPath, LOGFILENAME);
                        }

                        this.overlay.UpdateDisplay(true);
                    } else {
                        this.overlay.Opacity = this.CurrentSettings.OverlayBackgroundOpacity / 100D;
                    }
                    this.EnableInfoStrip(true);
                    this.EnableMainMenu(true);
                }
            } catch (Exception ex) {
                this.EnableInfoStrip(true);
                this.EnableMainMenu(true);
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuOverlay_Click(object sender, EventArgs e) {
            this.ToggleOverlay(this.overlay);
        }

        public void ToggleOverlay(Overlay overlay, bool saveSetting = true) {
            if (overlay.Visible) {
                IsDisplayOverlayPing = false;
                overlay.Hide();
                this.menuOverlay.Image = Properties.Resources.stat_gray_icon;
                this.menuOverlay.Text = $"{Multilingual.GetWord("main_show_overlay")}";
                this.trayOverlay.Image = Properties.Resources.stat_gray_icon;
                this.trayOverlay.Text = $"{Multilingual.GetWord("main_show_overlay")}";
            } else {
                this.overlay.saveDisplayChange = false;
                if (overlay.IsFixed()) {
                    if (this.CurrentSettings.OverlayFixedPositionX.HasValue &&
                        Utils.IsOnScreen(this.CurrentSettings.OverlayFixedPositionX.Value, this.CurrentSettings.OverlayFixedPositionY.Value, overlay.Width, overlay.Height)) {
                        overlay.FlipDisplay(this.CurrentSettings.FixedFlippedDisplay);
                        overlay.Location = new Point(this.CurrentSettings.OverlayFixedPositionX.Value, this.CurrentSettings.OverlayFixedPositionY.Value);
                    } else {
                        overlay.Location = this.Location;
                    }
                } else {
                    overlay.Location = this.CurrentSettings.OverlayLocationX.HasValue && Utils.IsOnScreen(this.CurrentSettings.OverlayLocationX.Value, this.CurrentSettings.OverlayLocationY.Value, overlay.Width, overlay.Height)
                                       ? new Point(this.CurrentSettings.OverlayLocationX.Value, this.CurrentSettings.OverlayLocationY.Value)
                                       : this.Location;
                }
                this.overlay.saveDisplayChange = true;
                this.overlay.UpdateDisplay();
                IsDisplayOverlayPing = !this.CurrentSettings.HideRoundInfo && (this.CurrentSettings.SwitchBetweenPlayers || this.CurrentSettings.OnlyShowPing);
                overlay.TopMost = !this.CurrentSettings.OverlayNotOnTop;
                overlay.Show();
                overlay.ShowInTaskbar = this.CurrentSettings.OverlayNotOnTop;
                this.menuOverlay.Image = Properties.Resources.stat_icon;
                this.menuOverlay.Text = $"{Multilingual.GetWord("main_hide_overlay")}";
                this.trayOverlay.Image = Properties.Resources.stat_icon;
                this.trayOverlay.Text = $"{Multilingual.GetWord("main_hide_overlay")}";
            }
            if (saveSetting) {
                this.CurrentSettings.OverlayVisible = overlay.Visible;
                this.SaveUserSettings();
            }
        }

        private void menuHelp_Click(object sender, EventArgs e) {
            this.LaunchHelpInBrowser();
        }

        private void menuEditProfiles_Click(object sender, EventArgs e) {
            try {
                using (EditProfiles editProfiles = new EditProfiles()) {
                    editProfiles.StatsForm = this;
                    editProfiles.Profiles = this.AllProfiles;
                    editProfiles.AllStats = this.AllStats;
                    this.EnableInfoStrip(false);
                    this.EnableMainMenu(false);
                    editProfiles.ShowDialog(this);
                    if (editProfiles.IsUpdate || editProfiles.IsDelete) {
                        lock (this.StatsDB) {
                            Task editProfilesTask = new Task(() => {
                                this.StatsDB.BeginTrans();
                                this.AllProfiles = editProfiles.Profiles;
                                this.Profiles.DeleteAll();
                                this.Profiles.InsertBulk(this.AllProfiles);
                                this.AllStats = editProfiles.AllStats;
                                if (editProfiles.IsUpdate) this.RoundDetails.Update(this.AllStats);
                                if (editProfiles.IsDelete) {
                                    foreach (int p in editProfiles.DeleteList) {
                                        this.RoundDetails.DeleteMany(r => r.Profile == p);
                                    }
                                }
                                this.StatsDB.Commit();
                            });
                            this.RunDatabaseTask(editProfilesTask, false);
                        }
                        this.ReloadProfileMenuItems();
                        IsOverlayRoundInfoNeedRefresh = true;
                    }
                    this.EnableInfoStrip(true);
                    this.EnableMainMenu(true);
                }
            } catch (Exception ex) {
                this.EnableInfoStrip(true);
                this.EnableMainMenu(true);
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuLaunchFinalBeans_Click(object sender, EventArgs e) {
            try {
                this.EnableInfoStrip(false);
                this.EnableMainMenu(false);
                this.LaunchGame(false);
                this.EnableInfoStrip(true);
                this.EnableMainMenu(true);
            } catch (Exception ex) {
                this.EnableInfoStrip(true);
                this.EnableMainMenu(true);
                MetroMessageBox.Show(this, ex.ToString(), $"{Multilingual.GetWord("message_program_error_caption")}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UpdatePlayerNameInfo() {
            this.lblPlayerName.Text = !string.IsNullOrWhiteSpace(this.CurrentSettings.PlayerName) ? this.CurrentSettings.PlayerName : Multilingual.GetWord("main_set_player_name");
        }

        private void ChangeLanguage() {
            this.SuspendLayout();
            this.currentLanguage = (int)CurrentLanguage;
            this.mainWndTitle = $@"     {Multilingual.GetWord("main_finalbeans_stats")} v{Assembly.GetExecutingAssembly().GetName().Version.ToString(2)}";
            this.trayIcon.Text = this.mainWndTitle.Trim();
            this.Text = this.mainWndTitle;
            this.menu.Font = Overlay.GetMainFont(12);
            this.menuLaunchFinalBeans.Font = Overlay.GetMainFont(12);
            this.infoStrip.Font = Overlay.GetMainFont(13);
            this.infoStrip2.Font = Overlay.GetMainFont(13);
            this.infoStrip3.Font = Overlay.GetMainFont(13);
            this.dataGridViewCellStyle1.Font = Overlay.GetMainFont(12);
            this.dataGridViewCellStyle2.Font = Overlay.GetMainFont(14);

            this.lblIgnoreLevelTypeWhenSorting.Text = Multilingual.GetWord("settings_ignore_level_type_when_sorting");
            this.UpdatePlayerNameInfo();

            this.traySettings.Text = Multilingual.GetWord("main_settings");
            this.trayFilters.Text = Multilingual.GetWord("main_filters");
            this.trayStatsFilter.Text = Multilingual.GetWord("main_stats");
            this.trayCustomRangeStats.Text = Multilingual.GetWord("main_custom_range");
            this.trayAllStats.Text = Multilingual.GetWord("main_all");
            this.traySeasonStats.Text = Multilingual.GetWord("main_season");
            this.trayWeekStats.Text = Multilingual.GetWord("main_week");
            this.trayDayStats.Text = Multilingual.GetWord("main_day");
            this.traySessionStats.Text = Multilingual.GetWord("main_session");
            this.trayPartyFilter.Text = Multilingual.GetWord("main_party_type");
            this.trayAllPartyStats.Text = Multilingual.GetWord("main_all");
            this.traySoloStats.Text = Multilingual.GetWord("main_solo");
            this.trayPartyStats.Text = Multilingual.GetWord("main_party");
            this.trayProfile.Text = Multilingual.GetWord("main_profile");
            this.trayEditProfiles.Text = Multilingual.GetWord("main_profile_setting");
            this.trayOverlay.Text = Multilingual.GetWord(this.CurrentSettings.OverlayVisible ? "main_hide_overlay" : "main_show_overlay");
            this.trayUsefulThings.Text = Multilingual.GetWord("main_useful_things");
            this.trayRollOffClub.Text = Multilingual.GetWord("main_roll_off_club");
            this.trayLostTempleAnalyzer.Text = Multilingual.GetWord("main_lost_temple_analyzer");
            this.trayUpdate.Text = Multilingual.GetWord("main_update");
            this.trayHelp.Text = Multilingual.GetWord("main_help");
            this.trayLaunchFinalBeans.Text = Multilingual.GetWord("main_launch_finalbeans");
            this.trayExitProgram.Text = Multilingual.GetWord("main_exit_program");

            this.menuSettings.Text = Multilingual.GetWord("main_settings");
            this.menuFilters.Text = Multilingual.GetWord("main_filters");
            this.menuStatsFilter.Text = Multilingual.GetWord("main_stats");
            this.menuCustomRangeStats.Text = Multilingual.GetWord("main_custom_range");
            this.menuAllStats.Text = Multilingual.GetWord("main_all");
            this.menuSeasonStats.Text = Multilingual.GetWord("main_season");
            this.menuWeekStats.Text = Multilingual.GetWord("main_week");
            this.menuDayStats.Text = Multilingual.GetWord("main_day");
            this.menuSessionStats.Text = Multilingual.GetWord("main_session");
            this.menuPartyFilter.Text = Multilingual.GetWord("main_party_type");
            this.menuAllPartyStats.Text = Multilingual.GetWord("main_all");
            this.menuSoloStats.Text = Multilingual.GetWord("main_solo");
            this.menuPartyStats.Text = Multilingual.GetWord("main_party");
            this.menuProfile.Text = Multilingual.GetWord("main_profile");
            this.menuEditProfiles.Text = Multilingual.GetWord("main_profile_setting");
            this.menuOverlay.Text = Multilingual.GetWord(this.CurrentSettings.OverlayVisible ? "main_hide_overlay" : "main_show_overlay");
            this.menuUpdate.Text = Multilingual.GetWord("main_update");
            this.menuHelp.Text = Multilingual.GetWord("main_help");
            this.menuLaunchFinalBeans.Text = Multilingual.GetWord("main_launch_finalbeans");
            this.menuUsefulThings.Text = Multilingual.GetWord("main_useful_things");
            this.menuRollOffClub.Text = Multilingual.GetWord("main_roll_off_club");
            this.menuLostTempleAnalyzer.Text = Multilingual.GetWord("main_lost_temple_analyzer");
            // this.SetLeaderboardTitle();
            this.ResumeLayout();
        }
    }
}
