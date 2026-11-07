using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinalBeansStats {
    public class LogLine {
        public TimeSpan Time { get; } = TimeSpan.Zero;
        public DateTime Date { get; set; } = DateTime.MinValue;
        public string Line { get; set; }
        public bool IsValid { get; set; }
        public long Offset { get; set; }

        public LogLine(string line, long offset) {
            this.Offset = offset;
            this.Line = line;
            bool isValidSemiColon = line.IndexOf(":") == 2 && line.IndexOf(":", 3) == 5 && line.IndexOf(":", 6) == 12;
            bool isValidDot = line.IndexOf(".") == 2 && line.IndexOf(".", 3) == 5 && line.IndexOf(":", 6) == 12;
            this.IsValid = isValidSemiColon || isValidDot;
            if (this.IsValid) {
                this.Time = TimeSpan.ParseExact(line.Substring(0, 12), isValidSemiColon ? "hh\\:mm\\:ss\\.fff" : "hh\\.mm\\.ss\\.fff", null);
            }
        }

        public override string ToString() {
            return $"{this.Time}: {this.Line} ({this.Offset})";
        }
    }
    public class LogRound {
        public bool CountingPlayers;
        public bool GetCurrentPlayerID;
        public int CurrentPlayerID;
        public bool FindingPosition;

        public RoundInfo Info;
        public RoundInfo PlayersInfo;
    }

    public class ThreadLocalData {
        public long lastQueuingInfoLinePos;

        public bool isPrivateLobby;
        public bool currentlyInParty;
        public string currentShowNameId;
        public string currentSessionId;
        public string currentRoundId;
        public DateTime currentRoundStart;

        public bool toggleCountryInfoApi;
    }

    public class LogFileWatcher {
        private const int UpdateDelay = 500;

        private string playerName;
        private string logPath;
        private string filePath;
        private string prevFilePath;
        private bool isWatcherRunning;
        private bool stop;

        internal Task logFileWatcher;

        public Stats StatsForm { get; set; }

        private readonly ThreadLocal<ThreadLocalData> threadLocalVariable = new ThreadLocal<ThreadLocalData>(() => new ThreadLocalData());
        public event Action<List<RoundInfo>> OnParsedLogLines;
        public event Action<List<RoundInfo>> OnParsedLogLinesCurrent;
        public event Action<DateTime> OnNewLogFileDate;
        public event Action OnServerConnectionNotification;
        public event Action<string, string, double, double> OnPersonalBestNotification;
        public event Action<string> OnError;

        private readonly ServerPingWatcher serverPingWatcher = new ServerPingWatcher();
        private readonly GameStateWatcher gameStateWatcher = new GameStateWatcher();

        public void Start(string logDirectory, string fileName) {
            if (this.isWatcherRunning) return;

            this.playerName = !string.IsNullOrWhiteSpace(this.StatsForm.CurrentSettings.PlayerName) ? this.StatsForm.CurrentSettings.PlayerName : string.Empty;
            this.logPath = logDirectory;
            this.filePath = Path.Combine(this.logPath, fileName);
            this.prevFilePath = Path.Combine(this.logPath, $"{Path.GetFileNameWithoutExtension(fileName)}-prev.log");
            this.stop = false;
            this.logFileWatcher = new Task(this.ReadLogFile);
            this.logFileWatcher.Start();
        }

        public async Task Stop() {
            this.stop = true;
            while (this.isWatcherRunning || this.logFileWatcher == null || this.logFileWatcher.Status == TaskStatus.Created) {
                await Task.Delay(50);
            }

            await Task.Run(() => this.logFileWatcher?.Wait());

            // await this.gameStateWatcher.Stop();
            // await this.serverPingWatcher.Stop();
        }

        private void ReadLogFile() {
            this.isWatcherRunning = true;
            List<RoundInfo> allStats = new List<RoundInfo>();
            List<LogLine> tempLines = new List<LogLine>();
            DateTime lastDate = DateTime.MinValue;
            bool prevFileCompleted = false;
            string currentFilePath = this.prevFilePath;
            long offset = 0;
            while (!this.stop) {
                try {
                    if (File.Exists(currentFilePath)) {
                        using (FileStream fs = new FileStream(currentFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                            tempLines.Clear();

                            if (fs.Length > offset) {
                                fs.Seek(offset, SeekOrigin.Begin);

                                LineReader sr = new LineReader(fs);
                                string line;
                                DateTime currentDate = lastDate;
                                while ((line = sr.ReadLine()) != null) {
                                    LogLine logLine = new LogLine(line, sr.Position);

                                    if (logLine.IsValid) {
                                        int index;
                                        if ((index = line.IndexOf("[GlobalGameStateClient].PreStart called at ")) != -1) {
                                            this.ResetMainLocalVariables();
                                            currentDate = DateTime.SpecifyKind(DateTime.Parse(line.Substring(index + 43, 19)), DateTimeKind.Utc);
                                            this.OnNewLogFileDate?.Invoke(currentDate);
                                        }

                                        if (currentDate != DateTime.MinValue) {
                                            if (currentDate.TimeOfDay.TotalSeconds - logLine.Time.TotalSeconds > 60000) {
                                                currentDate = currentDate.AddDays(1);
                                            }
                                            currentDate = currentDate.AddSeconds(logLine.Time.TotalSeconds - currentDate.TimeOfDay.TotalSeconds);
                                            logLine.Date = currentDate;
                                        }
                                        //
                                        // Not present in FinalBeans logs
                                        //
                                        /*
                                        if (line.IndexOf(" == [CompletedEpisodeDto] ==") != -1) {
                                            StringBuilder sb = new StringBuilder(line);
                                            sb.AppendLine();
                                            while ((line = sr.ReadLine()) != null) {
                                                LogLine temp = new LogLine(line, fs.Position);
                                                if (temp.IsValid) {
                                                    logLine.Line = sb.ToString();
                                                    logLine.Offset = sr.Position;
                                                    tempLines.Add(logLine);
                                                    tempLines.Add(temp);
                                                    break;
                                                } else if (!string.IsNullOrEmpty(line)) {
                                                    sb.AppendLine(line);
                                                }
                                            }
                                        } else if (line.IndexOf("[FNMMSClientRemoteService] Status message received: {") != -1
                                                   && logLine.Offset > this.threadLocalVariable.Value.lastQueuingInfoLinePos) {
                                            this.threadLocalVariable.Value.lastQueuingInfoLinePos = logLine.Offset;
                                            while ((line = sr.ReadLine()) != null) {
                                                if (line.IndexOf("\"queuedPlayers\": ") != -1) {
                                                    string content = Regex.Replace(line.Substring(21), "[\",]", "");
                                                    if (!string.Equals(content, "null")
                                                        && int.TryParse(content, out int queuedPlayers)) {
                                                        Stats.QueuedPlayers = queuedPlayers;
                                                        Stats.IsQueuing = !this.threadLocalVariable.Value.isPrivateLobby;
                                                    }
                                                    break;
                                                }
                                            }
                                        } else if (line.IndexOf("[FNMMSRemoteServiceBase] Disposed") != -1
                                                   && logLine.Offset > this.threadLocalVariable.Value.lastQueuingInfoLinePos) {
                                            this.threadLocalVariable.Value.lastQueuingInfoLinePos = logLine.Offset;
                                            Stats.IsQueuing = false;
                                            Stats.QueuedPlayers = 0;
                                        }
                                        */
                                        tempLines.Add(logLine);
                                    }
                                }
                            } else if (offset > fs.Length) {
                                offset = this.threadLocalVariable.Value.lastQueuingInfoLinePos = 0;
                            }
                        }
                    }

                    if (tempLines.Count > 0) {
                        List<RoundInfo> round = new List<RoundInfo>();
                        LogRound logRound = new LogRound();
                        List<LogLine> currentLines = new List<LogLine>();

                        for (int i = 0; i < tempLines.Count; i++) {
                            LogLine line = tempLines[i];
                            currentLines.Add(line);
                            if (this.ParseLine(line, round, logRound)) {
                                lastDate = line.Date;
                                offset = line.Offset;
                                allStats.AddRange(round);
                                if (allStats.Count > 0) {
                                    this.OnParsedLogLines?.Invoke(allStats);
                                    allStats.Clear();
                                }
                                currentLines.Clear();
                            } else if (line.Line.IndexOf("[LobbyManager] Show selected index: ", StringComparison.OrdinalIgnoreCase) != -1
                                       || line.Line.IndexOf("[StateMainMenu] No server address specified, attempting to matchmake", StringComparison.OrdinalIgnoreCase) != -1
                                       || line.Line.IndexOf("[GlobalGameStateClient] SwitchToDisconnectingState called with reason IngameMenuLeaveMatch", StringComparison.OrdinalIgnoreCase) != -1
                                       || line.Line.IndexOf("[StateDisconnectingFromServer] Shutting down game and resetting scene to reconnect", StringComparison.OrdinalIgnoreCase) != -1
                                       || line.Line.IndexOf("[GameStateMachine] Replacing FGClient.StateReloadingToMainMenu with FGClient.StateMainMenu", StringComparison.OrdinalIgnoreCase) != -1
                                       || line.Line.IndexOf("[StateMainMenu] Loading scene MainMenu", StringComparison.OrdinalIgnoreCase) != -1
                                       || line.Line.IndexOf("[GlobalGameStateClient] OnDestroy called", StringComparison.OrdinalIgnoreCase) != -1) {
                                if (line.Line.IndexOf("[LobbyManager] Show selected index: ", StringComparison.OrdinalIgnoreCase) != -1) {
                                    Stats.SelectedShowIndex = int.Parse(line.Line.Substring(line.Line.IndexOf("index: ", StringComparison.OrdinalIgnoreCase) + 7));
                                }
                                lastDate = line.Date;
                                offset = i > 0 ? tempLines[i - 1].Offset : offset;
                            } else if (this.StatsForm.CurrentSettings.AutoChangeProfile && line.Line.IndexOf("[RoundManager] OnGameStarted", StringComparison.OrdinalIgnoreCase) != -1) {
                                if (Stats.InShow && !Stats.EndedShow) {
                                    this.StatsForm.SetLinkedProfileMenu(this.threadLocalVariable.Value.currentShowNameId, this.threadLocalVariable.Value.isPrivateLobby);
                                }
                            } else if (this.StatsForm.CurrentSettings.PreventOverlayMouseClicks && line.Line.IndexOf("[GameSession] Changing state from Countdown to Playing", StringComparison.OrdinalIgnoreCase) != -1) {
                                if (Stats.InShow && !Stats.EndedShow) {
                                    this.StatsForm.PreventOverlayMouseClicks();
                                }
                            }
                        }

                        this.OnParsedLogLinesCurrent?.Invoke(round);
                    }

                    if (!prevFileCompleted) {
                        prevFileCompleted = true;
                        offset = this.threadLocalVariable.Value.lastQueuingInfoLinePos = 0;
                        currentFilePath = this.filePath;
                    }
                } catch (Exception ex) {
                    this.OnError?.Invoke(ex.ToString());
                }
                Thread.Sleep(UpdateDelay);
            }
            this.isWatcherRunning = false;
        }

        private void AddLineAfterClientShutdown() {
            try {
                bool isValidSemiColon, isValidDot, isValid;
                string lastTime = DateTime.UtcNow.ToString("hh:mm:ss.fff");
                using (var fs = new FileStream(this.filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)) {
                    using (var sr = new StreamReader(fs)) {
                        string line;
                        while ((line = sr.ReadLine()) != null) {
                            isValidSemiColon = line.IndexOf(":") == 2 && line.IndexOf(":", 3) == 5 && line.IndexOf(":", 6) == 12;
                            isValidDot = line.IndexOf(".") == 2 && line.IndexOf(".", 3) == 5 && line.IndexOf(":", 6) == 12;
                            isValid = isValidSemiColon || isValidDot;
                            if (isValid) {
                                lastTime = line.Substring(0, 12);
                            }
                        }
                    }
                    using (var sw = new StreamWriter(fs)) {
                        sw.WriteLine();
                        sw.WriteLine($"{lastTime}: [GlobalGameStateClient] OnDestroy called");
                        sw.WriteLine();
                    }
                }
            } catch {
                // ignored
            }
        }

        //
        // Old method to get the show name from the lastGames directory, no longer needed since FS4
        //
        //private string GetShowNameFromLastGamesDir(DateTime gameStartedDate, bool returnAsId = false) {
        //    try {
        //        if (gameStartedDate != Stats.LastGameDate) {
        //            Stats.LastGameDate = gameStartedDate;
        //            string date = gameStartedDate.ToLocalTime().ToString("yyyy-MM-dd_HH-mm-ss");
        //            string[] files = Directory.GetFiles(Path.Combine(this.logPath, "lastGames"), $"*{date}.txt");
        //            if (files.Length == 0) {
        //                date = gameStartedDate.AddSeconds(-1).ToLocalTime().ToString("yyyy-MM-dd_HH-mm-ss");
        //                files = Directory.GetFiles(Path.Combine(this.logPath, "lastGames"), $"*{date}.txt");
        //            }
        //            string fileName = Path.GetFileNameWithoutExtension(files[0]);
        //            int index = fileName.IndexOf($" - {date}");
        //            Stats.LastShowName = fileName.Substring(0, index);
        //            Stats.LastShowNameId = $"fb_{Regex.Replace(Stats.LastShowName.ToLower(), "[^0-9a-z_]", "_").Trim(new Char[] { '_' })}";
        //        }
        //        if (returnAsId) {
        //            return Stats.LastShowNameId;
        //        }
        //        return Stats.LastShowName;
        //    } catch {
        //        Stats.LastShowName = null;
        //        Stats.LastShowNameId = null;
        //        return null;
        //    }
        //}

        private string VerifiedRoundId(string roundId) {
            if (!string.Equals(roundId, Stats.LastRoundId)) {
                Stats.LastRoundId = roundId;
                string[] knownRoundIds = this.StatsForm.StatLookup.Keys.ToArray();
                Array.Sort(knownRoundIds);
                Array.Reverse(knownRoundIds);
                foreach (string knownRoundId in knownRoundIds) {
                    if (roundId.StartsWith(knownRoundId, StringComparison.OrdinalIgnoreCase)) {
                        Stats.LastRoundName = knownRoundId;
                        return Stats.LastRoundName;
                    }
                }
                Stats.LastRoundName = roundId;
            }
            return Stats.LastRoundName;
        }

        private bool IsRealFinalRound(string roundId, string showId) {
            if (string.Equals(showId, "show_skilled_speeders")) {
                return true;
            }

            return ((roundId.IndexOf("round_1v1_volleyfall", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_basketfall", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_fall_ball", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_jinxed", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_pixelperfect", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_redlight_greenlight", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_robotrampage", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_territory_control", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_zombean", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("_squads_squadcelebration", StringComparison.OrdinalIgnoreCase) != -1)
                        && roundId.EndsWith("_final", StringComparison.OrdinalIgnoreCase))

                     || roundId.EndsWith("_2teamsfinal", StringComparison.OrdinalIgnoreCase)

                     || roundId.EndsWith("_timeattack_final", StringComparison.OrdinalIgnoreCase)

                     || roundId.EndsWith("_xtreme_party_final", StringComparison.OrdinalIgnoreCase);
            /*
            || (string.Equals(showId, "event_animals_template")
                && roundNum == 4)

            || (string.Equals(showId, "event_yeetus_template")
                && roundNum == 3)

            || (string.Equals(showId, "event_only_finals_v3_template")
                && roundId.EndsWith("_final", StringComparison.OrdinalIgnoreCase))

            || (string.Equals(showId, "event_only_hoverboard_template")
                && roundNum == 3)

            || (string.Equals(showId, "event_snowday_stumble")
                && roundNum == 4)

            || (string.Equals(showId, "fp16_ski_fall_high_scorers")
                && roundNum == 1)

            || (string.Equals(showId, "ftue_uk_show")
                && string.Equals(roundId, "round_snowballsurvival_noelim_ftue_s2"))

            || (string.Equals(showId, "no_elimination_show")
                && roundNum == 3)

            || (string.Equals(showId, "sports_show")
                && roundId.EndsWith("_final", StringComparison.OrdinalIgnoreCase))

            || (string.Equals(showId, "showcase_fp13")
                && (string.Equals(roundId, "scrapyard_derrameburbujeante")
                    || roundId.EndsWith("_final", StringComparison.OrdinalIgnoreCase)))

            || (string.Equals(showId, "showcase_fp16")
                && (roundId.EndsWith("_final", StringComparison.OrdinalIgnoreCase)
                    || roundId.EndsWith("_goopropegrandslam", StringComparison.OrdinalIgnoreCase)))

            || (string.Equals(showId, "showcase_fp17")
                && (string.Equals(roundId, "round_fp17_gardenpardon")
                    || string.Equals(roundId, "round_fp17_castlesiege")))

            || (string.Equals(showId, "showcase_fp18")
                && (string.Equals(roundId, "showcase_bulletfallwoods")
                    || string.Equals(roundId, "showcase_treeclimberswoods")))

            || (string.Equals(showId, "showcase_fp19")
                && (roundNum == 3 || string.Equals(roundId, "fp19_mellowcakes")))

            || (string.Equals(showId, "showcase_fp20")
                && (roundNum == 3 || string.Equals(roundId, "showcase_boats")))

            || (string.Equals(showId, "wle_mrs_bouncy_bean_time")
                && roundNum == 3)

            || (string.Equals(showId, "wle_nature_ltm")
                && (roundNum == 3 || string.Equals(roundId, "logroll_nature_ltm")))

            || (showId.StartsWith("greatestsquads_")
                && (roundNum == 3 || string.Equals(roundId, "gs_slimecycle")));
            */

            /*
            // "Knockout" Shows
            || (showId.StartsWith("knockout_")
                && (string.Equals(roundId, "knockout_rotateandeliminate")
                    || string.Equals(roundId, "knockout_gooprope_rodeo")
                    || string.Equals(roundId, "knockout_slimeballshowdown")
                    || string.Equals(roundId, "knockout_blunderblocks")
                    || string.Equals(roundId, "knockout_pier_pressure")
                    || string.Equals(roundId, "round_fp17_knockout_castlesiege")
                    || string.Equals(roundId, "round_fp17_knockout_gardenpardon")
                    || (!string.Equals(roundId, "knockout_fp10_final_8")
                        && roundId.StartsWith("knockout_", StringComparison.OrdinalIgnoreCase)
                        && (roundId.EndsWith("_opener_4", StringComparison.OrdinalIgnoreCase)
                            || roundId.IndexOf("_final", StringComparison.OrdinalIgnoreCase) != -1))));
            */
        }

        private bool IsModeException(string roundId, string showId) {
            return roundId.IndexOf("round_1v1_button_basher_event_only", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_lava_event_only_slime_climb", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_slimeclimb_2_event_only", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_tip_toe_event_only", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_kraken_attack_only_finals", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_blastball_only_finals", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_floor_fall_only_finals", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_hexsnake_only_finals", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_jump_showdown_only_finals", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_hexaring_only_finals", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_tunnel_final_only_finals", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_thin_ice_only_finals", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_drumtop_event_only", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_floor_fall_event_only", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_floor_fall_event_only_low_grav", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_floor_fall_event_walnut", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_hexaring_event_only", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_hexaring_event_walnut", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_hexsnake_event_walnut", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_kraken_attack_event_only_survival", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_thin_ice_event_only", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_fall_ball_cup_only_trios", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_blastball_arenasurvival_blast_ball_trials", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_sports_suddendeath_fall_ball", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_robotrampage_arena_2_ss2_show1", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_floor_fall_squads_survival", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_thin_ice_squads_survival", StringComparison.OrdinalIgnoreCase) != -1

                   || roundId.IndexOf("round_crown_maze_trials", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_fall_mountain_trials", StringComparison.OrdinalIgnoreCase) != -1
                   || roundId.IndexOf("round_tunnel_final_trials", StringComparison.OrdinalIgnoreCase) != -1;

            /*
            || string.Equals(showId, "event_blast_ball_banger_template")
            */

            /*
            || showId.StartsWith("knockout_")
            || showId.StartsWith("ranked_"); // "Ranked Knockout" Show
            */
        }

        private bool IsModeFinalException(string roundId) {
            return ((roundId.IndexOf("round_1v1_button_basher_event_only", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_lava_event_only_slime_climb", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_slimeclimb_2_event_only", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_tip_toe_event_only", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_kraken_attack_only_finals", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_blastball_only_finals", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_floor_fall_only_finals", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_hexsnake_only_finals", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_jump_showdown_only_finals", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_hexaring_only_finals", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_tunnel_final_only_finals", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_thin_ice_only_finals", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_drumtop_event_only", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_floor_fall_event_only", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_floor_fall_event_only_low_grav", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_floor_fall_event_walnut", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_hexaring_event_only", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_hexaring_event_walnut", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_hexsnake_event_walnut", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_kraken_attack_event_only_survival", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_thin_ice_event_only", StringComparison.OrdinalIgnoreCase) != -1
                     || roundId.IndexOf("round_fall_ball_cup_only_trios", StringComparison.OrdinalIgnoreCase) != -1)
                        && roundId.EndsWith("_final", StringComparison.OrdinalIgnoreCase))

                     || (roundId.IndexOf("round_blastball_arenasurvival_blast_ball_trials", StringComparison.OrdinalIgnoreCase) != -1
                         && roundId.EndsWith("_fn", StringComparison.OrdinalIgnoreCase))

                     || (roundId.IndexOf("round_sports_suddendeath_fall_ball", StringComparison.OrdinalIgnoreCase) != -1
                         && roundId.EndsWith("_02", StringComparison.OrdinalIgnoreCase))

                     || ((roundId.IndexOf("round_robotrampage_arena_2_ss2_show1", StringComparison.OrdinalIgnoreCase) != -1
                          || roundId.IndexOf("round_fall_mountain_trials", StringComparison.OrdinalIgnoreCase) != -1)
                             && roundId.EndsWith("_03", StringComparison.OrdinalIgnoreCase))

                     || (roundId.IndexOf("round_crown_maze_trials", StringComparison.OrdinalIgnoreCase) != -1
                         && roundId.EndsWith("_3", StringComparison.OrdinalIgnoreCase));

            /*
            || string.Equals(roundId, "round_blastball_arenasurvival_blast_ball_banger")
            */

            /*
            // "Knockout" Shows
            string.Equals(roundId, "round_fp17_knockout_castlesiege")
            string.Equals(roundId, "round_fp17_knockout_gardenpardon")
            (!string.Equals(roundId, "knockout_fp10_final_8")
             && roundId.StartsWith("knockout_", StringComparison.OrdinalIgnoreCase)
             && (roundId.EndsWith("_opener_4", StringComparison.OrdinalIgnoreCase)
                 || roundId.IndexOf("_final", StringComparison.OrdinalIgnoreCase) != -1))

            // "Ranked Knockout" Show
            || (roundId.StartsWith("ranked_", StringComparison.OrdinalIgnoreCase)
                && roundId.EndsWith("_final", StringComparison.OrdinalIgnoreCase))
            */
        }

        private bool IsTeamException(string roundId) {
            return (roundId.IndexOf("round_1v1_volleyfall", StringComparison.OrdinalIgnoreCase) != -1
                    || roundId.IndexOf("round_hoops_revenge", StringComparison.OrdinalIgnoreCase) != -1)
                       && (roundId.IndexOf("_duos", StringComparison.OrdinalIgnoreCase) != -1
                           || roundId.IndexOf("_squads", StringComparison.OrdinalIgnoreCase) != -1);
        }

        private void SetCountryCodeByIp(string ip) {
            if (this.threadLocalVariable.Value.toggleCountryInfoApi || !Utils.IsProcessRunning("FinalBeans")) return;

            this.threadLocalVariable.Value.toggleCountryInfoApi = true;
            Stats.LastCountryAlpha2Code = string.Empty;
            Stats.LastCountryRegion = string.Empty;
            Stats.LastCountryCity = string.Empty;
            try {
                string ci = Utils.GetCountryInfo(ip);
                if (!string.IsNullOrEmpty(ci)) {
                    string[] countryInfo = ci.Split(';');
                    Stats.LastCountryAlpha2Code = countryInfo[0].ToLower();
                    Stats.LastCountryRegion = !string.Equals(countryInfo[1].ToLower(), "unknown") ? countryInfo[1] : string.Empty;
                    Stats.LastCountryCity = !string.Equals(countryInfo[2].ToLower(), "unknown") ? countryInfo[2] : string.Empty;
                } else {
                    string countryCode = Utils.GetCountryCode(ip);
                    Stats.LastCountryAlpha2Code = !string.IsNullOrEmpty(countryCode) ? countryCode.ToLower() : string.Empty;
                    Stats.LastCountryRegion = string.Empty;
                    Stats.LastCountryCity = string.Empty;
                }
            } catch {
                this.threadLocalVariable.Value.toggleCountryInfoApi = false;
                Stats.LastCountryAlpha2Code = string.Empty;
                Stats.LastCountryRegion = string.Empty;
                Stats.LastCountryCity = string.Empty;
            }
        }

        private void ResetVariablesUsedForOverlay() {
            Stats.IsQueuing = false;
            Stats.QueuedPlayers = 0;
            Stats.IsShowStarting = false;
            Stats.IsConnectedToServer = false;
            Stats.LastServerPing = 0;
            Stats.IsBadServerPing = false;
            Stats.LastCountryAlpha2Code = string.Empty;
            Stats.LastCountryRegion = string.Empty;
            Stats.LastCountryCity = string.Empty;
            this.threadLocalVariable.Value.toggleCountryInfoApi = false;
            Stats.LastPlayedRoundEnd = null;
            Stats.SavedRoundCount = 0;
            Stats.PrecountdownDate = DateTime.MinValue;
        }

        private void ResetMainLocalVariables() {
            this.threadLocalVariable.Value.isPrivateLobby = false;
            this.threadLocalVariable.Value.currentlyInParty = false;
            this.threadLocalVariable.Value.currentShowNameId = string.Empty;
            this.threadLocalVariable.Value.currentSessionId = string.Empty;
            this.threadLocalVariable.Value.currentRoundId = string.Empty;
            this.threadLocalVariable.Value.currentRoundStart = DateTime.MinValue;
        }

        private void UpdateServerConnectionLog(string session, string show) {
            lock (this.StatsForm.ServerConnectionLogCache) {
                if (!this.StatsForm.ExistsServerConnectionLog(session)) {
                    this.StatsForm.InsertServerConnectionLog(session, show, Stats.LastServerIp, Stats.ConnectedToServerDate, true, true);
                    this.serverPingWatcher.Start();
                    this.SetCountryCodeByIp(Stats.LastServerIp);
                    if (!Stats.IsClientHasBeenClosed && this.StatsForm.CurrentSettings.NotifyServerConnected && !string.IsNullOrEmpty(Stats.LastCountryAlpha2Code)) {
                        this.OnServerConnectionNotification?.Invoke();
                    }
                } else {
                    ServerConnectionLog serverConnectionLog = this.StatsForm.SelectServerConnectionLog(session);
                    if (!serverConnectionLog.IsNotify) {
                        if (!Stats.IsClientHasBeenClosed && this.StatsForm.CurrentSettings.NotifyServerConnected && !string.IsNullOrEmpty(Stats.LastCountryAlpha2Code)) {
                            this.OnServerConnectionNotification?.Invoke();
                        }
                    }

                    if (serverConnectionLog.IsPlaying) {
                        this.serverPingWatcher.Start();
                        this.SetCountryCodeByIp(Stats.LastServerIp);
                    }
                }
            }
        }

        private void UpdatePersonalBestLog(RoundInfo info) {
            lock (this.StatsForm.PersonalBestLogCache) {
                if (info.PrivateLobby || !info.Finish.HasValue) return;

                string levelId = info.Name;
                if (!LevelStats.ALL.TryGetValue(levelId, out LevelStats currentLevel) || (currentLevel.BestRecordType != BestRecordType.Fastest)) return;

                if (!this.StatsForm.ExistsPersonalBestLog(info.Finish.Value)) {
                    List<RoundInfo> roundInfoList = new List<RoundInfo>();
                    roundInfoList = this.StatsForm.AllStats.FindAll(r => !r.PrivateLobby &&
                                                                         string.Equals(r.ShowNameId, info.ShowNameId) &&
                                                                         string.Equals(r.Name, levelId) &&
                                                                         r.Finish.HasValue);

                    double currentPb = roundInfoList.Count > 0 ? roundInfoList.Min(r => (r.Finish.Value - r.Start).TotalMilliseconds) : 0;
                    double currentRecord = (info.Finish.Value - info.Start).TotalMilliseconds;
                    bool isNewPb = currentPb == 0 || currentRecord < currentPb;

                    this.StatsForm.InsertPersonalBestLog(info.Finish.Value, info.ShowNameId, levelId, currentRecord, isNewPb);
                    if (this.StatsForm.CurrentSettings.NotifyPersonalBest && isNewPb) {
                        this.OnPersonalBestNotification?.Invoke(info.ShowNameId, levelId, currentPb, currentRecord);
                    }
                }
            }
        }

        private bool ParseLine(LogLine line, List<RoundInfo> round, LogRound logRound) {
            int index;
            if (line.Line.IndexOf("[GlobalGameStateClient] SwitchToDisconnectingState called with reason IngameMenuLeaveMatch", StringComparison.OrdinalIgnoreCase) != -1
                || line.Line.IndexOf("[StateDisconnectingFromServer] Shutting down game and resetting scene to reconnect", StringComparison.OrdinalIgnoreCase) != -1
                || line.Line.IndexOf("[GameStateMachine] Replacing FGClient.StateReloadingToMainMenu with FGClient.StateMainMenu", StringComparison.OrdinalIgnoreCase) != -1
                || line.Line.IndexOf("[StateMainMenu] Loading scene MainMenu", StringComparison.OrdinalIgnoreCase) != -1
                || line.Line.IndexOf("[GlobalGameStateClient] OnDestroy called", StringComparison.OrdinalIgnoreCase) != -1
                || Stats.IsClientHasBeenClosed) {
                this.ResetVariablesUsedForOverlay();

                if (Stats.IsClientHasBeenClosed) {
                    Stats.IsClientHasBeenClosed = false;
                    this.AddLineAfterClientShutdown();
                    return false;
                }

                if (Stats.InShow && Stats.LastPlayedRoundStart.HasValue && !Stats.LastPlayedRoundEnd.HasValue) {
                    Stats.LastPlayedRoundEnd = line.Date;
                }
                Stats.IsLastPlayedRoundStillPlaying = false;

                logRound.CountingPlayers = false;
                logRound.GetCurrentPlayerID = false;
                logRound.FindingPosition = false;

                if (logRound.Info != null) {
                    if (logRound.Info.End == DateTime.MinValue) {
                        logRound.Info.End = line.Date;
                    }
                    logRound.Info.Playing = false;
                    //
                    // No " == [CompletedEpisodeDto] ==" (show summary) info in FinalBeans logs
                    //
                    // "RecordEscapeDuringAGame" setting MUST always be 'true' to save game stats
                    //
                    if (!string.IsNullOrEmpty(this.playerName) && this.StatsForm.CurrentSettings.RecordEscapeDuringAGame && !Stats.EndedShow) {
                        DateTime showStart = DateTime.MinValue;
                        DateTime showEnd = logRound.Info.End;
                        for (int i = 0; i < round.Count; i++) {
                            if (i == 0) {
                                showStart = round[i].Start;
                            }
                            round[i].ShowStart = showStart;
                            round[i].ShowEnd = showEnd;
                            round[i].Playing = false;
                            // round[i].Round = i + 1;
                            bool isNotParticipated = false;
                            if (i == (round.Count - 1)) {
                                for (int j = i; j >= 0; j--) {
                                    if (string.IsNullOrEmpty(round[j].Name)) {
                                        round.RemoveAt(j);
                                        continue;
                                    }
                                    //
                                    // Elimination info (of the local player only) is not always present in FinalBeans logs (mostly in "Survival" rounds)
                                    // so check if "Participating" flag was set to true or not
                                    //
                                    // If "Participating" flag is 'false' at round start => [don't save this round info; save previous rounds only]
                                    //
                                    if (!round[j].Participating) {
                                        isNotParticipated = true;
                                        if (j < i && round.ElementAtOrDefault(j + 1) != null) {
                                            round.RemoveAt(j + 1);
                                        }
                                        round.RemoveAt(j);
                                        continue;
                                    }
                                    //
                                    // Qualification/Win info is not always present in FinalBeans logs (mostly in "Survival" rounds)
                                    // So no "Finish" time set when it happens (it's an issue!)
                                    // So if the player participate to the last round and no elimination info line is present
                                    // Manually set "Finish" time / set "Qualified" flag to 'true' for all played rounds
                                    //
                                    // IMPORTANT: If the player is eliminated and no elimination info line is present
                                    // "Finish" time will wrongly be set
                                    // User can still delete "Finish" time of this round later to fix it...
                                    //
                                    for (int k = j; k >= 0; k--) {
                                        if (k == j && isNotParticipated) {
                                            // Means the player was eliminated in this round, so don't set "Finish" time / "Qualified" flag
                                            continue;
                                        }
                                        if (!round[k].Finish.HasValue) {
                                            round[k].Finish = round[k].End;
                                        }
                                        round[k].Qualified = true;
                                    }
                                }
                                if (round.Count > 0) {
                                    var lastRoundInfo = round[round.Count - 1];
                                    // If the player leaves the match from the in-game menu or quit the game, unset "Finish" time / "Qualified" flag for the last round
                                    if (line.Line.IndexOf("[GlobalGameStateClient] SwitchToDisconnectingState called with reason IngameMenuLeaveMatch", StringComparison.OrdinalIgnoreCase) != -1
                                        || line.Line.IndexOf("[GlobalGameStateClient] OnDestroy called", StringComparison.OrdinalIgnoreCase) != -1) {
                                        lastRoundInfo.Finish = null;
                                        lastRoundInfo.Qualified = false;
                                    } else if (lastRoundInfo.IsFinal && lastRoundInfo.Qualified) {
                                        lastRoundInfo.Position = 1;
                                        lastRoundInfo.Tier = 1;
                                        lastRoundInfo.Crown = true;
                                    }
                                    lastRoundInfo.LastRound = true;
                                }
                                //
                                // No "SessionId" info in FinalBeans logs
                                //
                                // this.StatsForm.UpdateServerConnectionLog(this.threadLocalVariable.Value.currentSessionId, false);
                                logRound.Info = null;
                                Stats.InShow = false;
                                Stats.EndedShow = true;
                                return true;
                            }
                        }
                    }
                }
                logRound.Info = null;
                Stats.InShow = false;
                Stats.EndedShow = true;
            } else if (line.Line.IndexOf("[StateMainMenu] No server address specified, attempting to matchmake", StringComparison.OrdinalIgnoreCase) != -1) {
                //
                // No "ServerIp" and no "SessionId" info in FinalBeans logs
                //

                // this.threadLocalVariable.Value.isPrivateLobby = line.Line.IndexOf("StatePrivateLobby", StringComparison.OrdinalIgnoreCase) != -1
                //                                                 || line.Line.IndexOf("private lobby", StringComparison.OrdinalIgnoreCase) != -1;

                // this.threadLocalVariable.Value.currentlyInParty = !this.threadLocalVariable.Value.isPrivateLobby
                //                                                   && (line.Line.IndexOf("solo", StringComparison.OrdinalIgnoreCase) == -1);

                Stats.IsShowStarting = true;
                Stats.EndedShow = false;

                logRound.CountingPlayers = false;
                logRound.GetCurrentPlayerID = false;
                logRound.FindingPosition = false;
                logRound.PlayersInfo = null;

                round.Clear();
            } else if ((index = line.Line.IndexOf("ROUNDMANAGER - SELECTED ROUND ", StringComparison.OrdinalIgnoreCase)) != -1) {
                if (line.Date > Stats.LastRoundLoad) {
                    Stats.LastRoundLoad = line.Date;
                    Stats.InShow = true;
                    Stats.SucceededPlayerNames.Clear();
                    Stats.SucceededPlayerIds.Clear();
                    Stats.EliminatedPlayerIds.Clear();
                    Stats.ReadyPlayerIds.Clear();
                    Stats.NumPlayersSucceeded = 0;
                    Stats.NumPlayersPsSucceeded = 0;
                    Stats.NumPlayersXbSucceeded = 0;
                    Stats.NumPlayersSwSucceeded = 0;
                    Stats.NumPlayersPcSucceeded = 0;
                    Stats.NumPlayersMbSucceeded = 0;
                    Stats.NumPlayersEliminated = 0;
                    Stats.NumPlayersPsEliminated = 0;
                    Stats.NumPlayersXbEliminated = 0;
                    Stats.NumPlayersSwEliminated = 0;
                    Stats.NumPlayersPcEliminated = 0;
                    Stats.NumPlayersMbEliminated = 0;
                    Stats.IsLastPlayedRoundStillPlaying = false;
                    Stats.LastPlayedRoundStart = null;
                    Stats.LastPlayedRoundEnd = null;

                    if (!Stats.IsConnectedToServer) {
                        Stats.IsConnectedToServer = true;
                        Stats.ConnectedToServerDate = line.Date;
                    }

                    if ((DateTime.UtcNow - Stats.ConnectedToServerDate).TotalMinutes <= 40) {
                        this.gameStateWatcher.Start();
                    }
                }

                if (logRound.Info != null) {
                    if (logRound.Info.End == DateTime.MinValue) {
                        logRound.Info.End = line.Date;
                    }
                    logRound.Info.Playing = false;
                }

                this.threadLocalVariable.Value.currentRoundId = line.Line.Substring(index + 30);
                this.threadLocalVariable.Value.currentRoundStart = line.Date;

                logRound.FindingPosition = false;

                logRound.PlayersInfo = new RoundInfo();
                logRound.CountingPlayers = true;
                logRound.GetCurrentPlayerID = true; // But currently useless for FinalBeans because playerID of the "local" player is unknown
                logRound.CurrentPlayerID = 0;       // Set to "0" because it is unknown
            } else if (logRound.CountingPlayers && line.Line.IndexOf("[CameraDirector] Adding Spectator target", StringComparison.OrdinalIgnoreCase) != -1) {
                //
                // "[CameraDirector] Adding Spectator target" in FinalBeans logs means a "remote" player is present (playing -OR- spectating, we can't tell...)
                //
                logRound.PlayersInfo.Players++;
                int playerId = int.Parse(line.Line.Substring(line.Line.IndexOf("playerID: ", StringComparison.OrdinalIgnoreCase) + 10));
                string platformId = line.Line.Substring(line.Line.IndexOf("(", StringComparison.OrdinalIgnoreCase) + 1, line.Line.IndexOf(")", StringComparison.OrdinalIgnoreCase) - line.Line.IndexOf("(", StringComparison.OrdinalIgnoreCase) - 1);
                switch (platformId) {
                    case "ps4":
                        logRound.PlayersInfo.PlayersPs4++;
                        if (line.Date > Stats.LastRoundLoad && !Stats.ReadyPlayerIds.ContainsKey(playerId)) {
                            Stats.ReadyPlayerIds.Add(playerId, "ps4");
                        }
                        break;
                    case "ps5":
                        logRound.PlayersInfo.PlayersPs5++;
                        if (line.Date > Stats.LastRoundLoad && !Stats.ReadyPlayerIds.ContainsKey(playerId)) {
                            Stats.ReadyPlayerIds.Add(playerId, "ps5");
                        }
                        break;
                    case "xb1":
                        logRound.PlayersInfo.PlayersXb1++;
                        if (line.Date > Stats.LastRoundLoad && !Stats.ReadyPlayerIds.ContainsKey(playerId)) {
                            Stats.ReadyPlayerIds.Add(playerId, "xb1");
                        }
                        break;
                    case "xsx":
                        logRound.PlayersInfo.PlayersXsx++;
                        if (line.Date > Stats.LastRoundLoad && !Stats.ReadyPlayerIds.ContainsKey(playerId)) {
                            Stats.ReadyPlayerIds.Add(playerId, "xsx");
                        }
                        break;
                    case "switch":
                        logRound.PlayersInfo.PlayersSw++;
                        if (line.Date > Stats.LastRoundLoad && !Stats.ReadyPlayerIds.ContainsKey(playerId)) {
                            Stats.ReadyPlayerIds.Add(playerId, "switch");
                        }
                        break;
                    case "win":
                        logRound.PlayersInfo.PlayersPc++;
                        if (line.Date > Stats.LastRoundLoad && !Stats.ReadyPlayerIds.ContainsKey(playerId)) {
                            Stats.ReadyPlayerIds.Add(playerId, "win");
                        }
                        break;
                    case "android":
                        logRound.PlayersInfo.PlayersAndroid++;
                        if (line.Date > Stats.LastRoundLoad && !Stats.ReadyPlayerIds.ContainsKey(playerId)) {
                            Stats.ReadyPlayerIds.Add(playerId, "android");
                        }
                        break;
                    case "ios":
                        logRound.PlayersInfo.PlayersIos++;
                        if (line.Date > Stats.LastRoundLoad && !Stats.ReadyPlayerIds.ContainsKey(playerId)) {
                            Stats.ReadyPlayerIds.Add(playerId, "ios");
                        }
                        break;
                    case "bots":
                        logRound.PlayersInfo.PlayersBots++;
                        if (line.Date > Stats.LastRoundLoad && !Stats.ReadyPlayerIds.ContainsKey(playerId)) {
                            Stats.ReadyPlayerIds.Add(playerId, "bots");
                        }
                        break;
                    default:
                        logRound.PlayersInfo.PlayersEtc++;
                        if (line.Date > Stats.LastRoundLoad && !Stats.ReadyPlayerIds.ContainsKey(playerId)) {
                            Stats.ReadyPlayerIds.Add(playerId, "etc");
                        }
                        break;
                }
            } else if (logRound.CountingPlayers && line.Line.IndexOf("[CameraDirector].UseCloseShot, current camera target type is Intro", StringComparison.OrdinalIgnoreCase) != -1) {
                //
                // "[CameraDirector].UseCloseShot, current camera target type is Intro" in FinalBeans logs means YOU will play
                //
                logRound.PlayersInfo.Participating = true;
                logRound.PlayersInfo.Players++;
                logRound.PlayersInfo.PlayersPc++;
                if (line.Date > Stats.LastRoundLoad && !Stats.ReadyPlayerIds.ContainsKey(logRound.CurrentPlayerID)) {
                    Stats.ReadyPlayerIds.Add(logRound.CurrentPlayerID, "win");
                }
            } else if (line.Line.IndexOf("[StateGameLoading].OnIntroCamerasComplete", StringComparison.OrdinalIgnoreCase) != -1) {
                logRound.CountingPlayers = false;
                logRound.GetCurrentPlayerID = false;
            } else if (line.Line.IndexOf("[GameSession] Changing state from Precountdown to Countdown", StringComparison.OrdinalIgnoreCase) != -1) {
                if (line.Date > Stats.LastPrecountdownStateDate) {
                    Stats.LastPrecountdownStateDate = line.Date;
                    Stats.PrecountdownDate = line.Date;
                }
            } else if (line.Line.IndexOf("[MatchLifecycle] Match started showId=", StringComparison.OrdinalIgnoreCase) != -1) {
                this.threadLocalVariable.Value.currentShowNameId = line.Line.Substring(line.Line.IndexOf("showId=", StringComparison.OrdinalIgnoreCase) + 7);
            } else if (line.Line.IndexOf("[RoundManager] OnGameStarted", StringComparison.OrdinalIgnoreCase) != -1) {
                string showId = this.threadLocalVariable.Value.currentShowNameId;
                string showName = Multilingual.GetShowName(showId);

                logRound.Info = new RoundInfo {
                    PrivateLobby = this.threadLocalVariable.Value.isPrivateLobby,
                    InParty = this.threadLocalVariable.Value.currentlyInParty,
                    // ShowName = this.GetShowNameFromLastGamesDir(this.threadLocalVariable.Value.lastGameDate),
                    // ShowNameId = this.GetShowNameFromLastGamesDir(this.threadLocalVariable.Value.lastGameDate, true),
                    ShowName = !string.IsNullOrEmpty(showName) ? showName : (!string.IsNullOrWhiteSpace(showId) ? showId : (Stats.SelectedShowIndex == 0 ? Multilingual.GetShowName("main_show") : Multilingual.GetShowName("ltm_show"))),
                    ShowNameId = !string.IsNullOrWhiteSpace(showId) ? showId : (Stats.SelectedShowIndex == 0 ? "main_show" : "ltm_show"),
                    RoundId = this.threadLocalVariable.Value.currentRoundId,
                    Start = this.threadLocalVariable.Value.currentRoundStart
                };

                if (logRound.PlayersInfo != null) {
                    logRound.Info.Participating = logRound.PlayersInfo.Participating;
                    logRound.Info.Players = logRound.PlayersInfo.Players;
                    logRound.Info.PlayersPs4 = logRound.PlayersInfo.PlayersPs4;
                    logRound.Info.PlayersPs5 = logRound.PlayersInfo.PlayersPs5;
                    logRound.Info.PlayersXb1 = logRound.PlayersInfo.PlayersXb1;
                    logRound.Info.PlayersXsx = logRound.PlayersInfo.PlayersXsx;
                    logRound.Info.PlayersSw = logRound.PlayersInfo.PlayersSw;
                    logRound.Info.PlayersPc = logRound.PlayersInfo.PlayersPc;
                    logRound.Info.PlayersAndroid = logRound.PlayersInfo.PlayersAndroid;
                    logRound.Info.PlayersIos = logRound.PlayersInfo.PlayersIos;
                    logRound.Info.PlayersBots = logRound.PlayersInfo.PlayersBots;
                    logRound.Info.PlayersEtc = logRound.PlayersInfo.PlayersEtc;
                    logRound.PlayersInfo = null;
                }

                logRound.Info.Name = this.VerifiedRoundId(logRound.Info.RoundId);

                logRound.Info.IsTeam = this.IsTeamException(logRound.Info.RoundId);

                if (this.IsRealFinalRound(logRound.Info.RoundId, logRound.Info.ShowNameId)) {
                    logRound.Info.IsFinal = true;
                } else if (this.IsModeException(logRound.Info.RoundId, logRound.Info.ShowNameId)) {
                    logRound.Info.IsFinal = this.IsModeFinalException(logRound.Info.RoundId);
                } else {
                    logRound.Info.IsFinal = this.StatsForm.StatLookup.TryGetValue(logRound.Info.Name, out LevelStats levelStats) && levelStats.IsFinal;
                }

                round.Add(logRound.Info);

                logRound.Info.Round = !Stats.EndedShow ? round.Count : Stats.SavedRoundCount + round.Count;

                Stats.IsShowStarting = false;
            } else if (logRound.Info != null && line.Line.IndexOf("[GameSession] Changing state from Countdown to Playing", StringComparison.OrdinalIgnoreCase) != -1) {
                if (line.Date > Stats.LastPrecountdownStateDate) {
                    Stats.LastPrecountdownStateDate = line.Date;
                    Stats.PrecountdownDate = DateTime.MinValue;
                }
                logRound.Info.Start = line.Date;
                logRound.Info.Playing = true;
            } else if (logRound.Info != null && !string.IsNullOrEmpty(this.playerName) && (line.Line.IndexOf($"[ROUNDMANAGER] Player {this.playerName} has qualified", StringComparison.OrdinalIgnoreCase) != -1
                                                                                           || line.Line.IndexOf($"[ROUNDMANAGER] Player {this.playerName} has won", StringComparison.OrdinalIgnoreCase) != -1)) {
                logRound.Info.Finish = logRound.Info.End == DateTime.MinValue ? line.Date : logRound.Info.End;
                if (line.Date > Stats.LastRoundLoad && !Stats.SucceededPlayerNames.Contains(this.playerName)) {
                    Stats.SucceededPlayerNames.Add(this.playerName);
                    Stats.NumPlayersSucceeded++;
                    Stats.NumPlayersPcSucceeded++;
                    this.UpdatePersonalBestLog(logRound.Info);
                }
                if (line.Line.LastIndexOf($" has won", StringComparison.OrdinalIgnoreCase) != -1) {
                    logRound.Info.Position = 1;
                    logRound.Info.Tier = 1;
                    logRound.Info.Crown = true;
                } else {
                    logRound.FindingPosition = true;
                }
            } else if (line.Date > Stats.LastRoundLoad && (index = line.Line.IndexOf("[ROUNDMANAGER] Player ", StringComparison.OrdinalIgnoreCase)) != -1 &&
                                                           (line.Line.LastIndexOf(" has qualified", StringComparison.OrdinalIgnoreCase) != -1
                                                            || line.Line.LastIndexOf(" has won", StringComparison.OrdinalIgnoreCase) != -1)) {
                int index2 = line.Line.LastIndexOf(" has ", StringComparison.OrdinalIgnoreCase);
                string remotePlayerName = line.Line.Substring(index + 22, index2 - (index + 22));
                if (!Stats.SucceededPlayerNames.Contains(remotePlayerName)) {
                    Stats.SucceededPlayerNames.Add(remotePlayerName);
                    Stats.NumPlayersSucceeded++;
                    Stats.NumPlayersPcSucceeded++;
                }
            } else if (line.Line.IndexOf("[ROUNDMANAGER] Loading round", StringComparison.OrdinalIgnoreCase) != -1
                       || line.Line.EndsWith(": Playing", StringComparison.OrdinalIgnoreCase)
                       || line.Line.EndsWith(": Succeeded", StringComparison.OrdinalIgnoreCase)
                       || line.Line.EndsWith(": Failed", StringComparison.OrdinalIgnoreCase)
                       || line.Line.EndsWith(": NotParticipating", StringComparison.OrdinalIgnoreCase)) {
                //
                // "[GameSession] Changing state from Playing to GameOver" line is not present in FinalBeans logs
                //
                // "[ROUNDMANAGER] - Loading round" is used in FinalBeans logs to possibly indicate a round end (ONLY WHEN GAME STARTED WITH 1 PLAYER!)
                //
                // "Playing" | "Succeeded" | "Failed | "NotParticipating" are also (sometimes) used to indicate a round end
                //
                if (line.Date > Stats.LastRoundLoad) {
                    Stats.LastRoundLoad = line.Date;
                    if (Stats.InShow && Stats.LastPlayedRoundStart.HasValue && !Stats.LastPlayedRoundEnd.HasValue) {
                        Stats.LastPlayedRoundEnd = line.Date;
                    }
                    Stats.IsLastPlayedRoundStillPlaying = false;
                }
                if (logRound.Info != null) {
                    if (logRound.Info.End == DateTime.MinValue) {
                        logRound.Info.End = line.Date;
                    }
                    logRound.Info.Playing = false;
                }
            } else if (logRound.Info != null && !Stats.EndedShow && logRound.FindingPosition && (index = line.Line.IndexOf("[ClientGameSession] NumPlayersAchievingObjective=")) != -1) {
                int position = int.Parse(line.Line.Substring(index + 49));
                if (position > 0) {
                    logRound.FindingPosition = false;
                    logRound.Info.Position = position;
                }
            } else if (logRound.Info != null && !Stats.EndedShow && line.Line.IndexOf("[MatchLifecycle] Local player eliminated", StringComparison.OrdinalIgnoreCase) != -1) {
                Stats.SavedRoundCount = logRound.Info.Round;

                if (logRound.Info.End == DateTime.MinValue) {
                    Stats.LastPlayedRoundStart = logRound.Info.Start;
                    Stats.IsLastPlayedRoundStillPlaying = true;
                    logRound.Info.End = line.Date;
                }
                logRound.Info.Playing = false;
                //
                // No " == [CompletedEpisodeDto] ==" (show summary) info in FinalBeans logs
                //
                // "RecordEscapeDuringAGame" setting MUST always be 'true' to save game stats
                //
                if (!string.IsNullOrEmpty(this.playerName) && this.StatsForm.CurrentSettings.RecordEscapeDuringAGame) {
                    DateTime showStart = DateTime.MinValue;
                    DateTime showEnd = logRound.Info.End;
                    for (int i = 0; i < round.Count; i++) {
                        if (i == 0) {
                            showStart = round[i].Start;
                        }
                        round[i].ShowStart = showStart;
                        round[i].ShowEnd = showEnd;
                        round[i].Playing = false;
                        // round[i].Round = i + 1;
                        if (i == (round.Count - 1)) {
                            for (int j = i; j >= 0; j--) {
                                if (string.IsNullOrEmpty(round[j].Name)) {
                                    round.RemoveAt(j);
                                    continue;
                                }
                                //
                                // Qualification info is not always present in FinalBeans logs (mostly in "Survival" rounds)
                                // So no "Finish" time set when it happens (it's an issue!)
                                //
                                // Manually set "Finish" time / set "Qualified" flag to 'true' for any previous round
                                //
                                for (int k = j; k > 0; k--) {
                                    if (!round[k - 1].Finish.HasValue) {
                                        round[k - 1].Finish = round[k - 1].End;
                                    }
                                    round[k - 1].Qualified = true;
                                }
                            }
                            if (round.Count > 0) {
                                round[round.Count - 1].LastRound = true;
                            }
                            //
                            // No "SessionId" info in FinalBeans logs
                            //
                            // this.StatsForm.UpdateServerConnectionLog(this.threadLocalVariable.Value.currentSessionId, false);
                            logRound.Info = null;
                            Stats.EndedShow = true;
                            return true;
                        }
                    }
                }
                logRound.Info = null;
                Stats.EndedShow = true;
            }
            //
            // Not present in FinalBeans logs
            //
            /*
            } else if (!Stats.EndedShow && line.Line.IndexOf("[FG_UnityInternetNetworkManager] Client connected to Server", StringComparison.OrdinalIgnoreCase) != -1) {
                if (!Stats.IsConnectedToServer) {
                    Stats.IsConnectedToServer = true;
                    Stats.ConnectedToServerDate = line.Date;
                    int ipIndex = line.Line.IndexOf("IP:", StringComparison.OrdinalIgnoreCase) + 3;
                    Stats.LastServerIp = line.Line.Substring(ipIndex);
                }
            } else if ((index = line.Line.IndexOf("[HandleSuccessfulLogin] Session: ", StringComparison.OrdinalIgnoreCase)) != -1) {
                this.threadLocalVariable.Value.currentSessionId = line.Line.Substring(index + 33);
                if ((DateTime.UtcNow - Stats.ConnectedToServerDate).TotalMinutes <= 40) {
                    this.UpdateServerConnectionLog(this.threadLocalVariable.Value.currentSessionId, this.threadLocalVariable.Value.selectedShowId);
                }
            } else if (logRound.Info != null && logRound.CountingPlayers && (line.Line.IndexOf("[ClientGameManager] Finalising spawn", StringComparison.OrdinalIgnoreCase) != -1 || line.Line.IndexOf("[ClientGameManager] Added player ", StringComparison.OrdinalIgnoreCase) != -1)) {
                logRound.Info.Players++;
            } else if (logRound.Info != null && logRound.GetCurrentPlayerID && line.Line.IndexOf("[ClientGameManager] Handling bootstrap for local player FallGuy", StringComparison.OrdinalIgnoreCase) != -1 && (index = line.Line.IndexOf("playerID = ", StringComparison.OrdinalIgnoreCase)) != -1) {
                logRound.GetCurrentPlayerID = false;
                int prevIndex = line.Line.IndexOf(",", index + 11);
                logRound.CurrentPlayerID = int.Parse(line.Line.Substring(index + 11, prevIndex - index - 11));
            } else if (logRound.Info != null && line.Line.IndexOf($"HandleServerPlayerProgress PlayerId={logRound.CurrentPlayerID} is succeeded=", StringComparison.OrdinalIgnoreCase) != -1) {
                if (line.Line.IndexOf("succeeded=True", StringComparison.OrdinalIgnoreCase) != -1) {
                    logRound.Info.Finish = logRound.Info.End == DateTime.MinValue ? line.Date : logRound.Info.End;
                    if (line.Date > Stats.LastRoundLoad && !Stats.SucceededPlayerIds.Contains(logRound.CurrentPlayerID)) {
                        Stats.SucceededPlayerIds.Add(logRound.CurrentPlayerID);
                        Stats.NumPlayersSucceeded++;
                        if (Stats.ReadyPlayerIds.TryGetValue(logRound.CurrentPlayerID, out string platformId)) {
                            switch (platformId) {
                                case "ps4":
                                case "ps5":
                                    Stats.NumPlayersPsSucceeded++; break;
                                case "xb1":
                                case "xsx":
                                    Stats.NumPlayersXbSucceeded++; break;
                                case "switch":
                                    Stats.NumPlayersSwSucceeded++; break;
                                case "win":
                                    Stats.NumPlayersPcSucceeded++; break;
                                case "android":
                                case "ios":
                                    Stats.NumPlayersMbSucceeded++; break;
                            }
                        }
                        this.UpdatePersonalBestLog(logRound.Info);
                    }
                    logRound.FindingPosition = true;
                } else if (line.Line.IndexOf("succeeded=False", StringComparison.OrdinalIgnoreCase) != -1) {
                    if (line.Date > Stats.LastRoundLoad && !Stats.EliminatedPlayerIds.Contains(logRound.CurrentPlayerID)) {
                        Stats.EliminatedPlayerIds.Add(logRound.CurrentPlayerID);
                        Stats.NumPlayersEliminated++;
                        if (Stats.ReadyPlayerIds.TryGetValue(logRound.CurrentPlayerID, out string platformId)) {
                            switch (platformId) {
                                case "ps4":
                                case "ps5":
                                    Stats.NumPlayersPsEliminated++; break;
                                case "xb1":
                                case "xsx":
                                    Stats.NumPlayersXbEliminated++; break;
                                case "switch":
                                    Stats.NumPlayersSwEliminated++; break;
                                case "win":
                                    Stats.NumPlayersPcEliminated++; break;
                                case "android":
                                case "ios":
                                    Stats.NumPlayersMbEliminated++; break;
                            }
                        }
                    }
                    logRound.FindingPosition = true;
                }
            } else if (line.Date > Stats.LastRoundLoad && (index = line.Line.IndexOf("HandleServerPlayerProgress PlayerId=", StringComparison.OrdinalIgnoreCase)) != -1) {
                if (line.Line.IndexOf("succeeded=True", StringComparison.OrdinalIgnoreCase) != -1) {
                    int prevIndex = line.Line.IndexOf(" ", index + 36);
                    int playerId = int.Parse(line.Line.Substring(index + 36, prevIndex - index - 36));
                    if (!Stats.SucceededPlayerIds.Contains(playerId)) {
                        Stats.SucceededPlayerIds.Add(playerId);
                        Stats.NumPlayersSucceeded++;
                        if (Stats.ReadyPlayerIds.TryGetValue(playerId, out string platformId)) {
                            switch (platformId) {
                                case "ps4":
                                case "ps5":
                                    Stats.NumPlayersPsSucceeded++; break;
                                case "xb1":
                                case "xsx":
                                    Stats.NumPlayersXbSucceeded++; break;
                                case "switch":
                                    Stats.NumPlayersSwSucceeded++; break;
                                case "win":
                                    Stats.NumPlayersPcSucceeded++; break;
                                case "android":
                                case "ios":
                                    Stats.NumPlayersMbSucceeded++; break;
                            }
                        }
                    }
                } else if (line.Line.IndexOf("succeeded=False", StringComparison.OrdinalIgnoreCase) != -1) {
                    int prevIndex = line.Line.IndexOf(" ", index + 36);
                    int playerId = int.Parse(line.Line.Substring(index + 36, prevIndex - index - 36));
                    if (!Stats.EliminatedPlayerIds.Contains(playerId) && Stats.ReadyPlayerIds.ContainsKey(playerId)) {
                        Stats.EliminatedPlayerIds.Add(playerId);
                        Stats.NumPlayersEliminated++;
                        if (Stats.ReadyPlayerIds.TryGetValue(playerId, out string platformId)) {
                            switch (platformId) {
                                case "ps4":
                                case "ps5":
                                    Stats.NumPlayersPsEliminated++; break;
                                case "xb1":
                                case "xsx":
                                    Stats.NumPlayersXbEliminated++; break;
                                case "switch":
                                    Stats.NumPlayersSwEliminated++; break;
                                case "win":
                                    Stats.NumPlayersPcEliminated++; break;
                                case "android":
                                case "ios":
                                    Stats.NumPlayersMbEliminated++; break;
                            }
                        }
                    }
                }
            } else if (line.Line.IndexOf(" == [CompletedEpisodeDto] ==", StringComparison.OrdinalIgnoreCase) != -1) {
                if (logRound.Info == null || Stats.EndedShow) return false;

                Stats.SavedRoundCount = logRound.Info.Round;
                Stats.EndedShow = true;

                if (logRound.Info.End == DateTime.MinValue) {
                    Stats.LastPlayedRoundStart = logRound.Info.Start;
                    Stats.IsLastPlayedRoundStillPlaying = true;
                    logRound.Info.End = line.Date;
                }
                logRound.Info.Playing = false;

                RoundInfo roundInfo = null;
                StringReader sr = new StringReader(line.Line);
                string detail;
                bool foundRound = false;
                int maxRound = 0;
                DateTime showStart = DateTime.MinValue;
                int questKudos = 0;
                while ((detail = sr.ReadLine()) != null) {
                    if (detail.IndexOf("[Round ", StringComparison.OrdinalIgnoreCase) == 0) {
                        foundRound = true;
                        int roundNum = detail[7] - 0x30 + 1;
                        string roundName = detail.Substring(11, detail.Length - 12);

                        if (roundNum - 1 < round.Count) {
                            if (roundNum > maxRound) {
                                maxRound = roundNum;
                            }

                            roundInfo = round[roundNum - 1];

                            if (string.IsNullOrEmpty(roundInfo.Name)) {
                                return false;
                            }

                            if (string.Equals(roundInfo.ShowNameId, "wle_mrs_shuffle_show") || string.Equals(roundInfo.ShowNameId, "wle_shuffle_discover") || string.Equals(roundInfo.ShowNameId, "wle_mrs_shuffle_show_squads")) {
                                if (round.Count > 1 && roundNum != round.Count) {
                                    roundInfo.IsFinal = false;
                                }
                                roundName = this.StatsForm.ReplaceLevelIdInShuffleShow(roundInfo.ShowNameId, roundName);
                            }

                            if (!string.Equals(roundInfo.Name, roundName, StringComparison.OrdinalIgnoreCase)) {
                                return false;
                            }

                            roundInfo.VerifyName();

                            if (roundNum == 1) {
                                showStart = roundInfo.Start;
                            }
                            roundInfo.ShowStart = showStart;
                            roundInfo.Playing = false;
                            roundInfo.Round = roundNum;
                        } else {
                            return false;
                        }

                        if (roundInfo.End == DateTime.MinValue) {
                            roundInfo.End = line.Date;
                        }
                        if (roundInfo.Start == DateTime.MinValue) {
                            roundInfo.Start = roundInfo.End;
                        }
                    } else if (foundRound) {
                        if (detail.IndexOf("> Qualified: ", StringComparison.OrdinalIgnoreCase) == 0) {
                            char qualified = detail[13];
                            roundInfo.Qualified = qualified == 'T';
                        } else if (detail.IndexOf("> Position: ", StringComparison.OrdinalIgnoreCase) == 0) {
                            roundInfo.Position = int.Parse(detail.Substring(12));
                        } else if (detail.IndexOf("> Team Score: ", StringComparison.OrdinalIgnoreCase) == 0) {
                            roundInfo.Score = int.Parse(detail.Substring(14));
                        } else if (detail.IndexOf("> Kudos: ", StringComparison.OrdinalIgnoreCase) == 0) {
                            roundInfo.Kudos += int.Parse(detail.Substring(9));
                        } else if (detail.IndexOf("> Bonus Tier: ", StringComparison.OrdinalIgnoreCase) == 0 && detail.Length == 15) {
                            char tier = detail[14];
                            roundInfo.Tier = roundInfo.Qualified ? tier - 0x30 + 1 : 0;
                        } else if (detail.IndexOf("> Bonus Kudos: ", StringComparison.OrdinalIgnoreCase) == 0) {
                            roundInfo.Kudos += int.Parse(detail.Substring(15));
                        }
                    } else {
                        if (detail.IndexOf("> Kudos: ", StringComparison.OrdinalIgnoreCase) == 0) {
                            questKudos = int.Parse(detail.Substring(9));
                            //> Fame:, > Crowns:, > CurrentCrownShards:
                        }
                    }
                }

                if (round.Count > maxRound) {
                    return false;
                }

                if (roundInfo != null) {
                    roundInfo.Kudos += questKudos;
                }

                DateTime showEnd = logRound.Info.End;
                for (int i = 0; i < round.Count; i++) {
                    round[i].ShowEnd = showEnd;
                }

                if (logRound.Info.Qualified) {
                    logRound.Info.Crown = true;
                    logRound.InLoadingGameScreen = false;
                    logRound.CountingPlayers = false;
                    logRound.GetCurrentPlayerID = false;
                    logRound.FindingPosition = false;
                }
                logRound.Info = null;
                return true;
            }
            */
            return false;
        }
    }
}
