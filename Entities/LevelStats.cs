using System;
using System.Collections.Generic;
using System.Drawing;
using LiteDB;
namespace FinalBeansStats {
    public class RoundInfo : IComparable<RoundInfo> {
        public ObjectId ID { get; set; }
        public int Profile { get; set; }
        public string RoundId { get; set; }
        public string Name { get; set; }
        public bool LastRound { get; set; }
        public int ShowID { get; set; }
        public string ShowName { get; set; }
        public string ShowNameId { get; set; }
        public int Round { get; set; }
        public bool Participating { get; set; }
        public int Position { get; set; }
        public int? Score { get; set; }
        public int Tier { get; set; }
        public bool Qualified { get; set; }
        public int Kudos { get; set; }
        public int Players { get; set; }
        public int PlayersPs4 { get; set; }
        public int PlayersPs5 { get; set; }
        public int PlayersXb1 { get; set; }
        public int PlayersXsx { get; set; }
        public int PlayersSw { get; set; }
        public int PlayersPc { get; set; }
        public int PlayersAndroid { get; set; }
        public int PlayersIos { get; set; }
        public int PlayersBots { get; set; }
        public int PlayersEtc { get; set; }
        public bool InParty { get; set; }
        public bool IsFinal { get; set; }
        public bool IsTeam { get; set; }
        public bool PrivateLobby { get; set; }
        public DateTime Start { get; set; } = DateTime.MinValue;
        public DateTime End { get; set; } = DateTime.MinValue;
        public DateTime? Finish { get; set; } = null;
        public bool Crown { get; set; }

        public DateTime StartLocal;
        public DateTime EndLocal;
        public DateTime? FinishLocal;
        public DateTime ShowStart = DateTime.MinValue;
        public DateTime ShowEnd = DateTime.MinValue;
        public bool Playing;
        private bool setLocalTime;

        public void ToLocalTime() {
            if (this.setLocalTime) return;

            this.setLocalTime = true;

            this.StartLocal = this.Start.ToLocalTime();
            this.EndLocal = this.End.ToLocalTime();
            if (this.Finish.HasValue) {
                this.FinishLocal = this.Finish.Value.ToLocalTime();
            }
        }

        public override string ToString() {
            return $"{this.Name}: Round={this.Round} Position={this.Position} Duration={this.End - this.Start} Kudos={this.Kudos}";
        }

        public override bool Equals(object obj) {
            return obj is RoundInfo info
                   && info.End == this.End
                   && info.Finish == this.Finish
                   && info.InParty == this.InParty
                   && info.Kudos == this.Kudos
                   && info.Players == this.Players
                   && info.PlayersPs4 == this.PlayersPs4
                   && info.PlayersPs5 == this.PlayersPs5
                   && info.PlayersXb1 == this.PlayersXb1
                   && info.PlayersXsx == this.PlayersXsx
                   && info.PlayersSw == this.PlayersSw
                   && info.PlayersPc == this.PlayersPc
                   && info.PlayersAndroid == this.PlayersAndroid
                   && info.PlayersIos == this.PlayersIos
                   && info.PlayersBots == this.PlayersBots
                   && info.PlayersEtc == this.PlayersEtc
                   && info.Position == this.Position
                   && info.Qualified == this.Qualified
                   && info.Round == this.Round
                   && info.Score == this.Score
                   && info.ShowID == this.ShowID
                   && info.Start == this.Start
                   && info.Tier == this.Tier
                   && string.Equals(info.Name, this.Name);
        }

        public override int GetHashCode() {
            return Name.GetHashCode() ^ ShowID ^ Round;
        }

        public int CompareTo(RoundInfo other) {
            int showCompare = ShowID.CompareTo(other.ShowID);
            return showCompare != 0 ? showCompare : Round.CompareTo(other.Round);
        }
    }

    public class LevelStats {
        public static Dictionary<string, LevelStats> ALL = new Dictionary<string, LevelStats>(StringComparer.OrdinalIgnoreCase) {

            // FALL GUYS ROUNDS

            { "round_biggestfan",                                 new LevelStats("round_biggestfan", "Big Fans", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_big_fans_icon, Properties.Resources.round_big_fans_big_icon)},
            { "round_satellitehoppers",                           new LevelStats("round_satellitehoppers", "Cosmic Highway", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_cosmic_highway_icon, Properties.Resources.round_cosmic_highway_big_icon)},
            { "round_gauntlet_02",                                new LevelStats("round_gauntlet_02", "Dizzy Heights", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_dizzy_heights_icon, Properties.Resources.round_dizzy_heights_big_icon)},
            { "round_door_dash",                                  new LevelStats("round_door_dash", "Door Dash", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_door_dash_icon, Properties.Resources.round_door_dash_big_icon)},
            { "round_iceclimb",                                   new LevelStats("round_iceclimb", "Freezy Peak", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_freezy_peak_icon, Properties.Resources.round_freezy_peak_big_icon)},
            { "round_dodge_fall",                                 new LevelStats("round_dodge_fall", "Fruit Chute", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_fruit_chute_icon, Properties.Resources.round_fruit_chute_big_icon)},
            { "round_see_saw_360",                                new LevelStats("round_see_saw_360", "Full Tilt", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_full_tilt_icon, Properties.Resources.round_full_tilt_big_icon)},
            { "round_chompchomp",                                 new LevelStats("round_chompchomp", "Gate Crash", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_gate_crash_icon, Properties.Resources.round_gate_crash_big_icon)},
            { "round_gauntlet_01",                                new LevelStats("round_gauntlet_01", "Hit Parade", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_hit_parade_icon, Properties.Resources.round_hit_parade_big_icon)},
            { "round_gauntlet_04",                                new LevelStats("round_gauntlet_04", "Knight Fever", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_knight_fever_icon, Properties.Resources.round_knight_fever_big_icon)},
            { "round_drumtop",                                    new LevelStats("round_drumtop", "Lily Leapers", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_lily_leapers_icon, Properties.Resources.round_lily_leapers_big_icon)},
            { "round_gauntlet_08",                                new LevelStats("round_gauntlet_08", "Party Promenade", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_party_promenade_icon, Properties.Resources.round_party_promenade_big_icon)},
            { "round_pipedup",                                    new LevelStats("round_pipedup", "Pipe Dream", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_pipe_dream_icon, Properties.Resources.round_pipe_dream_big_icon)},
            { "round_follow_the_line",                            new LevelStats("round_follow_the_line", "Puzzle Path", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_puzzle_path_icon, Properties.Resources.round_puzzle_path_big_icon)},
            { "round_tunnel_race",                                new LevelStats("round_tunnel_race", "Roll On", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_roll_on_icon, Properties.Resources.round_roll_on_big_icon)},
            { "round_see_saw",                                    new LevelStats("round_see_saw", "See Saw", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_see_saw_icon, Properties.Resources.round_see_saw_big_icon)},
            { "round_shortcircuit",                               new LevelStats("round_shortcircuit", "Short Circuit", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_short_circuit_icon, Properties.Resources.round_short_circuit_big_icon)},
            { "round_gauntlet_06",                                new LevelStats("round_gauntlet_06", "Skyline Stumble", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_skyline_stumble_icon, Properties.Resources.round_skyline_stumble_big_icon)},
            { "round_lava",                                       new LevelStats("round_lava", "Slime Climb", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_slime_climb_icon, Properties.Resources.round_slime_climb_big_icon)},
            { "round_gauntlet_10",                                new LevelStats("round_gauntlet_10", "Space Race", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_space_race_icon, Properties.Resources.round_space_race_big_icon)},
            { "round_short_circuit_2",                            new LevelStats("round_short_circuit_2", "Speed Circuit", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_speed_circuit_icon, Properties.Resources.round_speed_circuit_big_icon)},
            { "round_slide_chute",                                new LevelStats("round_slide_chute", "Speed Slider", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_speed_slider_icon, Properties.Resources.round_speed_slider_big_icon)},
            { "round_starlink",                                   new LevelStats("round_starlink", "Starchart", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_starchart_icon, Properties.Resources.round_starchart_big_icon)},
            { "round_slimeclimb_2",                               new LevelStats("round_slimeclimb_2", "The Slimescraper", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_the_slimescraper_icon, Properties.Resources.round_the_slimescraper_big_icon)},
            { "round_gauntlet_03",                                new LevelStats("round_gauntlet_03", "The Whirlygig", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_the_whirlygig_icon, Properties.Resources.round_the_whirlygig_big_icon)},
            { "round_tip_toe",                                    new LevelStats("round_tip_toe", "Tip Toe", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_tip_toe_icon, Properties.Resources.round_tip_toe_big_icon)},
            { "round_gauntlet_09",                                new LevelStats("round_gauntlet_09", "Track Attack", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_track_attack_icon, Properties.Resources.round_track_attack_big_icon)},
            { "round_gauntlet_07",                                new LevelStats("round_gauntlet_07", "Treetop Tumble", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_treetop_tumble_icon, Properties.Resources.round_treetop_tumble_big_icon)},
            { "round_gauntlet_05",                                new LevelStats("round_gauntlet_05", "Tundra Run", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_tundra_run_icon, Properties.Resources.round_tundra_run_big_icon)},
            { "round_wall_guys",                                  new LevelStats("round_wall_guys", "Wall Guys", LevelType.Race, BestRecordType.Fastest, false, 1, Properties.Resources.round_wall_guys_icon, Properties.Resources.round_wall_guys_big_icon)},

            { "round_airtime",                                    new LevelStats("round_airtime", "Airtime", LevelType.Hunt, BestRecordType.Fastest, false, 1, Properties.Resources.round_airtime_icon, Properties.Resources.round_airtime_big_icon)},
            { "round_bluejay",                                    new LevelStats("round_bluejay", "Bean Hill Zone", LevelType.Hunt, BestRecordType.Fastest, false, 1, Properties.Resources.round_bean_hill_zone_icon, Properties.Resources.round_bean_hill_zone_big_icon)},
            { "round_hoops_revenge",                              new LevelStats("round_hoops_revenge", "Bounce Party", LevelType.Hunt, BestRecordType.Fastest, false, 1, Properties.Resources.round_bounce_party_icon, Properties.Resources.round_bounce_party_big_icon)},
            { "round_king_of_the_hill",                           new LevelStats("round_king_of_the_hill", "Bubble Trouble", LevelType.Hunt, BestRecordType.Fastest, false, 1, Properties.Resources.round_bubble_trouble_icon, Properties.Resources.round_bubble_trouble_big_icon)},
            { "round_1v1_button_basher",                          new LevelStats("round_1v1_button_basher", "Button Bashers", LevelType.Hunt, BestRecordType.Fastest, false, 1, Properties.Resources.round_button_bashers_icon, Properties.Resources.round_button_bashers_big_icon)},
            { "round_ffa_button_bashers",                         new LevelStats("round_ffa_button_bashers", "Frantic Factory", LevelType.Hunt, BestRecordType.Fastest, false, 1, Properties.Resources.round_frantic_factory_icon, Properties.Resources.round_frantic_factory_big_icon)},
            { "round_slippy_slide",                               new LevelStats("round_slippy_slide", "Hoop Chute", LevelType.Hunt, BestRecordType.Fastest, false, 1, Properties.Resources.round_hoop_chute_icon, Properties.Resources.round_hoop_chute_big_icon)},
            { "round_hoops_blockade_solo",                        new LevelStats("round_hoops_blockade_solo", "Hoopsie Legends", LevelType.Hunt, BestRecordType.Fastest, false, 1, Properties.Resources.round_hoopsie_legends_icon, Properties.Resources.round_hoopsie_legends_big_icon)},
            { "round_follow-the-leader",                          new LevelStats("round_follow-the-leader", "Leading Light", LevelType.Hunt, BestRecordType.Fastest, false, 1, Properties.Resources.round_leading_light_icon, Properties.Resources.round_leading_light_big_icon)},
            { "round_penguin_solos",                              new LevelStats("round_penguin_solos", "Pegwin Pool Party", LevelType.Hunt, BestRecordType.Fastest, false, 1, Properties.Resources.round_pegwin_pool_party_icon, Properties.Resources.round_pegwin_pool_party_big_icon)},
            { "round_skeefall",                                   new LevelStats("round_skeefall", "Ski Fall", LevelType.Hunt, BestRecordType.Fastest, false, 1, Properties.Resources.round_ski_fall_icon, Properties.Resources.round_ski_fall_big_icon)},
            { "round_tail_tag",                                   new LevelStats("round_tail_tag", "Tail Tag", LevelType.Hunt, BestRecordType.Fastest, false, 1, Properties.Resources.round_tail_tag_icon, Properties.Resources.round_tail_tag_big_icon)},
            { "round_1v1_volleyfall",                             new LevelStats("round_1v1_volleyfall", "Volleyfall", LevelType.Hunt, BestRecordType.HighScore, false, 1, Properties.Resources.round_volleyfall_icon, Properties.Resources.round_volleyfall_big_icon)},

            { "round_fruitpunch",                                 new LevelStats("round_fruitpunch", "Big Shots", LevelType.Survival, BestRecordType.Longest, false, 1, Properties.Resources.round_big_shots_icon, Properties.Resources.round_big_shots_big_icon)},
            { "round_blastballruins",                             new LevelStats("round_blastballruins", "Blastlantis", LevelType.Survival, BestRecordType.Longest, false, 1, Properties.Resources.round_blastlantis_icon, Properties.Resources.round_blastlantis_big_icon)},
            { "round_block_party",                                new LevelStats("round_block_party", "Block Party", LevelType.Survival, BestRecordType.Longest, false, 1, Properties.Resources.round_block_party_icon, Properties.Resources.round_block_party_big_icon)},
            { "round_hoverboardsurvival",                         new LevelStats("round_hoverboardsurvival", "Hoverboard Heroes", LevelType.Survival, BestRecordType.Fastest, false, 1, Properties.Resources.round_hoverboard_heroes_icon, Properties.Resources.round_hoverboard_heroes_big_icon)},
            { "round_hoverboardsurvival2",                        new LevelStats("round_hoverboardsurvival2", "Hyperdrive Heroes", LevelType.Survival, BestRecordType.Fastest, false, 1, Properties.Resources.round_hyperdrive_heroes_icon, Properties.Resources.round_hyperdrive_heroes_big_icon)},
            { "round_jump_club",                                  new LevelStats("round_jump_club", "Jump Club", LevelType.Survival, BestRecordType.Longest, false, 1, Properties.Resources.round_jump_club_icon, Properties.Resources.round_jump_club_big_icon)},
            { "round_tunnel",                                     new LevelStats("round_tunnel", "Roll Out", LevelType.Survival, BestRecordType.Longest, false, 1, Properties.Resources.round_roll_out_icon, Properties.Resources.round_roll_out_big_icon)},
            { "round_snowballsurvival",                           new LevelStats("round_snowballsurvival", "Snowball Survival", LevelType.Survival, BestRecordType.Longest, false, 1, Properties.Resources.round_snowball_survival_icon, Properties.Resources.round_snowball_survival_big_icon)},
            { "round_robotrampage_arena_2",                       new LevelStats("round_robotrampage_arena_2", "Stompin' Ground", LevelType.Survival, BestRecordType.Longest, false, 1, Properties.Resources.round_stompin_ground_icon, Properties.Resources.round_stompin_ground_big_icon)},
            { "round_spin_ring",                                  new LevelStats("round_spin_ring", "The Swiveller", LevelType.Survival, BestRecordType.Longest, false, 1, Properties.Resources.round_the_swiveller_icon, Properties.Resources.round_the_swiveller_big_icon)},

            { "round_match_fall",                                 new LevelStats("round_match_fall", "Perfect Match", LevelType.Logic, BestRecordType.Longest, false, 1, Properties.Resources.round_perfect_match_icon, Properties.Resources.round_perfect_match_big_icon)},
            { "round_pixelperfect",                               new LevelStats("round_pixelperfect", "Pixel Painters", LevelType.Logic, BestRecordType.Fastest, false, 1, Properties.Resources.round_pixel_painters_icon, Properties.Resources.round_pixel_painters_big_icon)},
            { "round_fruit_bowl",                                 new LevelStats("round_fruit_bowl", "Sum Fruit", LevelType.Logic, BestRecordType.Longest, false, 1, Properties.Resources.round_sum_fruit_icon, Properties.Resources.round_sum_fruit_big_icon)},

            { "round_basketfall",                                 new LevelStats("round_basketfall", "Basketfall", LevelType.Team, BestRecordType.HighScore, false, 1, Properties.Resources.round_basketfall_icon, Properties.Resources.round_basketfall_big_icon)},
            { "round_egg_grab",                                   new LevelStats("round_egg_grab", "Egg Scramble", LevelType.Team, BestRecordType.HighScore, false, 1, Properties.Resources.round_egg_scramble_icon, Properties.Resources.round_egg_scramble_big_icon)},
            { "round_egg_grab_02",                                new LevelStats("round_egg_grab_02", "Egg Siege", LevelType.Team, BestRecordType.HighScore, false, 1, Properties.Resources.round_egg_siege_icon, Properties.Resources.round_egg_siege_big_icon)},
            { "round_fall_ball",                                  new LevelStats("round_fall_ball", "Fall Ball", LevelType.Team, BestRecordType.HighScore, false, 1, Properties.Resources.round_fall_ball_icon, Properties.Resources.round_fall_ball_big_icon)},
            { "round_ballhogs",                                   new LevelStats("round_ballhogs", "Hoarders", LevelType.Team, BestRecordType.HighScore, false, 1, Properties.Resources.round_hoarders_icon, Properties.Resources.round_hoarders_big_icon)},
            { "round_hoops",                                      new LevelStats("round_hoops", "Hoopsie Daisy", LevelType.Team, BestRecordType.HighScore, false, 1, Properties.Resources.round_hoopsie_daisy_icon, Properties.Resources.round_hoopsie_daisy_big_icon)},
            { "round_jinxed",                                     new LevelStats("round_jinxed", "Jinxed", LevelType.Team, BestRecordType.Fastest, false, 1, Properties.Resources.round_jinxed_icon, Properties.Resources.round_jinxed_big_icon)},
            { "round_chicken_chase",                              new LevelStats("round_chicken_chase", "Pegwin Pursuit", LevelType.Team, BestRecordType.HighScore, false, 1, Properties.Resources.round_pegwin_pursuit_icon, Properties.Resources.round_pegwin_pursuit_big_icon)},
            { "round_territory_control",                          new LevelStats("round_territory_control", "Power Trip", LevelType.Team, BestRecordType.HighScore, false, 1, Properties.Resources.round_power_trip_icon, Properties.Resources.round_power_trip_big_icon)},
            { "round_rocknroll",                                  new LevelStats("round_rocknroll", "Rock 'n' Roll", LevelType.Team, BestRecordType.Fastest, false, 1, Properties.Resources.round_rock_n_roll_icon, Properties.Resources.round_rock_n_roll_big_icon)},
            { "round_snowy_scrap",                                new LevelStats("round_snowy_scrap", "Snowy Scrap", LevelType.Team, BestRecordType.Fastest, false, 1, Properties.Resources.round_snowy_scrap_icon, Properties.Resources.round_snowy_scrap_big_icon)},
            { "round_conveyor_arena",                             new LevelStats("round_conveyor_arena", "Team Tail Tag", LevelType.Team, BestRecordType.Fastest, false, 1, Properties.Resources.round_team_tail_tag_icon, Properties.Resources.round_team_tail_tag_big_icon)},

            { "round_invisibeans",                                new LevelStats("round_invisibeans", "Sweet Thieves", LevelType.Invisibeans, BestRecordType.Fastest, false, 1, Properties.Resources.round_sweet_thieves_icon, Properties.Resources.round_sweet_thieves_big_icon)},
            { "round_pumpkin_pie",                                new LevelStats("round_pumpkin_pie", "Treat Thieves", LevelType.Invisibeans, BestRecordType.Fastest, false, 1, Properties.Resources.round_treat_thieves_icon, Properties.Resources.round_treat_thieves_big_icon)},

            { "round_blastball_arenasurvival",                    new LevelStats("round_blastball_arenasurvival", "Blast Ball", LevelType.Survival, BestRecordType.Longest, true, 1, Properties.Resources.round_blast_ball_icon, Properties.Resources.round_blast_ball_big_icon)},
            { "round_fall_mountain_hub_complete",                 new LevelStats("round_fall_mountain_hub_complete", "Fall Mountain", LevelType.Race, BestRecordType.Fastest, true, 1, Properties.Resources.round_fall_mountain_icon, Properties.Resources.round_fall_mountain_big_icon)},
            { "round_floor_fall",                                 new LevelStats("round_floor_fall", "Hex-A-Gone", LevelType.Survival, BestRecordType.Longest, true, 1, Properties.Resources.round_hex_a_gone_icon, Properties.Resources.round_hex_a_gone_big_icon)},
            { "round_hexaring",                                   new LevelStats("round_hexaring", "Hex-A-Ring", LevelType.Survival, BestRecordType.Longest, true, 1, Properties.Resources.round_hex_a_ring_icon, Properties.Resources.round_hex_a_ring_big_icon)},
            { "round_hexsnake",                                   new LevelStats("round_hexsnake", "Hex-A-Terrestrial", LevelType.Survival, BestRecordType.Longest, true, 1, Properties.Resources.round_hex_a_terrestrial_icon, Properties.Resources.round_hex_a_terrestrial_big_icon)},
            { "round_jump_showdown",                              new LevelStats("round_jump_showdown", "Jump Showdown", LevelType.Survival, BestRecordType.Longest, true, 1, Properties.Resources.round_jump_showdown_icon, Properties.Resources.round_jump_showdown_big_icon)},
            { "round_kraken_attack",                              new LevelStats("round_kraken_attack", "Kraken Slam", LevelType.Survival, BestRecordType.Longest, true, 1, Properties.Resources.round_kraken_slam_icon, Properties.Resources.round_kraken_slam_big_icon)},
            { "round_crown_maze",                                 new LevelStats("round_crown_maze", "Lost Temple", LevelType.Race, BestRecordType.Fastest, true, 1, Properties.Resources.round_lost_temple_icon, Properties.Resources.round_lost_temple_big_icon)},
            { "round_tunnel_final",                               new LevelStats("round_tunnel_final", "Roll Off", LevelType.Survival, BestRecordType.Longest, true, 1, Properties.Resources.round_roll_off_icon, Properties.Resources.round_roll_off_big_icon)},
            { "round_royal_rumble",                               new LevelStats("round_royal_rumble", "Royal Fumble", LevelType.Hunt, BestRecordType.Fastest, true, 1, Properties.Resources.round_royal_fumble_icon, Properties.Resources.round_royal_fumble_big_icon)},
            { "round_thin_ice",                                   new LevelStats("round_thin_ice", "Thin Ice", LevelType.Survival, BestRecordType.Longest, true, 1, Properties.Resources.round_thin_ice_icon, Properties.Resources.round_thin_ice_big_icon)},
            { "round_tiptoefinale",                               new LevelStats("round_tiptoefinale", "Tip Toe Finale", LevelType.Race, BestRecordType.Fastest, true, 1, Properties.Resources.round_tip_toe_finale_icon, Properties.Resources.round_tip_toe_finale_big_icon)},

            // FINALBEANS ROUNDS

            { "round_hallow_heights",                             new LevelStats("round_hallow_heights", "Hallow Heights", LevelType.Race, BestRecordType.Fastest, false, 2, Properties.Resources.round_block_party_icon, Properties.Resources.round_block_party_big_icon)},
            { "round_bowling",                                    new LevelStats("round_bowling", "Bean Ball Bowling", LevelType.Hunt, BestRecordType.Fastest, false, 2, Properties.Resources.round_block_party_icon, Properties.Resources.round_block_party_big_icon)},
            { "round_zombean",                                    new LevelStats("round_zombean", "Zombeanland", LevelType.Survival, BestRecordType.Longest, false, 2, Properties.Resources.round_block_party_icon, Properties.Resources.round_block_party_big_icon)},
            { "round_hexaghoul",                                  new LevelStats("round_hexaghoul", "Hex-A-Ghoul", LevelType.Survival, BestRecordType.Longest, true, 2, Properties.Resources.round_block_party_icon, Properties.Resources.round_block_party_big_icon)}
        };

        public Image RoundIcon { get; set; }
        public Image RoundBigIcon { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Qualified { get; set; }
        public int Gold { get; set; }
        public int Silver { get; set; }
        public int Bronze { get; set; }
        public int Played { get; set; }
        public int Kudos { get; set; }
        public TimeSpan Fastest { get; set; }
        public TimeSpan Longest { get; set; }
        public int AveKudos { get { return this.Kudos / Math.Max(1, this.Played); } }
        public TimeSpan AveDuration { get { return TimeSpan.FromSeconds((int)this.Duration.TotalSeconds / Math.Max(1, this.Played)); } }
        public TimeSpan AveFinish { get { return TimeSpan.FromSeconds((double)this.FinishTime.TotalSeconds / Math.Max(1, this.FinishedCount)); } }
        public LevelType Type;
        public BestRecordType BestRecordType;
        public bool IsFinal;
        public int TimeLimitSeconds;

        public TimeSpan Duration;
        public TimeSpan FinishTime;
        public List<RoundInfo> Stats;
        public int Season;
        public int FinishedCount;

        public LevelStats(string levelId, string levelName, LevelType type, BestRecordType recordType, bool isFinal, int season, Image roundIcon, Image roundBigIcon) {
            this.Id = levelId;
            this.Name = levelName;
            this.Type = type;
            this.BestRecordType = recordType;
            this.Season = season;
            this.IsFinal = isFinal;
            this.RoundIcon = roundIcon;
            this.RoundBigIcon = roundBigIcon;
            this.Stats = new List<RoundInfo>();
            this.Clear();
        }

        public void Clear() {
            this.Qualified = 0;
            this.Gold = 0;
            this.Silver = 0;
            this.Bronze = 0;
            this.Played = 0;
            this.Kudos = 0;
            this.FinishedCount = 0;
            this.Duration = TimeSpan.Zero;
            this.FinishTime = TimeSpan.Zero;
            this.Fastest = TimeSpan.Zero;
            this.Longest = TimeSpan.Zero;
            this.Stats.Clear();
        }

        public void Increase(RoundInfo stat, bool isLinkedCustomShow) {
            if (!stat.PrivateLobby || isLinkedCustomShow) {
                this.Played++;
                this.Duration += stat.End - stat.Start;
                switch (stat.Tier) {
                    case (int)QualifyTier.Gold:
                        this.Gold++;
                        break;
                    case (int)QualifyTier.Silver:
                        this.Silver++;
                        break;
                    case (int)QualifyTier.Bronze:
                        this.Bronze++;
                        break;
                }

                this.Kudos += stat.Kudos;
                this.Qualified += stat.Qualified ? 1 : 0;
            }

            TimeSpan finishTime = stat.Finish.GetValueOrDefault(stat.Start) - stat.Start;
            if (stat.Finish.HasValue && finishTime.TotalSeconds > 1.1) {
                if (!stat.PrivateLobby || isLinkedCustomShow) {
                    this.FinishedCount++;
                    this.FinishTime += finishTime;
                }
                if (this.Fastest == TimeSpan.Zero || this.Fastest > finishTime) {
                    this.Fastest = finishTime;
                }
                if (this.Longest < finishTime) {
                    this.Longest = finishTime;
                }
            }
        }

        public void Add(RoundInfo stat) {
            this.Stats.Add(stat);
        }

        public override string ToString() {
            return $"{this.Name}: {this.Qualified} / {this.Played}";
        }
    }
}
