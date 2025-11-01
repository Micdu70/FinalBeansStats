using System.Drawing;
using MetroFramework;

namespace FinalBeansStats {
    internal static class LevelTypeBehavior {
        public static string LevelTitle(this LevelType type, bool isFinal) {
            if (isFinal) {
                return Multilingual.GetWord("level_detail_final");
            }
            switch (type) {
                case LevelType.Race:
                    return Multilingual.GetWord("level_detail_race");
                case LevelType.Hunt:
                    return Multilingual.GetWord("level_detail_hunt");
                case LevelType.Invisibeans:
                    return Multilingual.GetWord("level_detail_invisibeans");
                case LevelType.Survival:
                    return Multilingual.GetWord("level_detail_survival");
                case LevelType.Logic:
                    return Multilingual.GetWord("level_detail_logic");
                case LevelType.Team:
                    return Multilingual.GetWord("level_detail_team");
            }
            return "Unknown";
        }

        public static Color LevelDefaultColor(this LevelType type, bool isFinal) {
            if (isFinal) {
                return Color.FromArgb(251, 198, 0);
            }
            switch (type) {
                case LevelType.Race:
                    return Color.FromArgb(0, 236, 106);
                case LevelType.Hunt:
                    return Color.FromArgb(45, 101, 186);
                case LevelType.Invisibeans:
                    return Color.FromArgb(0, 0, 0);
                case LevelType.Survival:
                    return Color.FromArgb(184, 21, 213);
                case LevelType.Logic:
                    return Color.FromArgb(91, 181, 189);
                case LevelType.Team:
                    return Color.FromArgb(248, 82, 0);
            }
            return Color.DarkGray;
        }

        public static Color LevelBackColor(this LevelType type, bool isFinal, bool isTeam, int alpha) {
            if (isFinal) {
                return Color.FromArgb(alpha, 250, 195, 0);
            }
            if (isTeam) {
                return Color.FromArgb(alpha, 250, 80, 0);
            }
            switch (type) {
                case LevelType.Race:
                    return Color.FromArgb(alpha, 0, 235, 105);
                case LevelType.Survival:
                    return Color.FromArgb(alpha, 185, 20, 210);
                case LevelType.Hunt:
                    return Color.FromArgb(alpha, 45, 100, 190);
                case LevelType.Logic:
                    return Color.FromArgb(alpha, 90, 180, 190);
                case LevelType.Team:
                    return Color.FromArgb(alpha, 250, 80, 0);
                case LevelType.Invisibeans:
                    return Color.FromArgb(alpha, 0, 0, 0);
            }
            return Color.DarkGray;
        }

        public static Color LevelForeColor(this LevelType type, bool isFinal, bool isTeam, MetroThemeStyle theme = MetroThemeStyle.Default) {
            if (isFinal) {
                return Color.FromArgb(130, 100, 0);
            }
            if (isTeam) {
                return Color.FromArgb(130, 40, 0);
            }
            switch (type) {
                case LevelType.Race:
                    return Color.FromArgb(0, 130, 55);
                case LevelType.Survival:
                    return Color.FromArgb(110, 10, 130);
                case LevelType.Hunt:
                    return Color.FromArgb(30, 70, 130);
                case LevelType.Logic:
                    return Color.FromArgb(60, 120, 130);
                case LevelType.Team:
                    return Color.FromArgb(130, 40, 0);
                case LevelType.Invisibeans:
                    return theme == MetroThemeStyle.Light ? Color.FromArgb(0, 0, 0) : Color.DarkGray;
            }
            return Color.FromArgb(60, 60, 60);
        }
    }
}