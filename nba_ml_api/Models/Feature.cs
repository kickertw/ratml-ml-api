namespace nba_ml_api.Models
{
    public class Feature
    {
        public string Key { get; set; }
        public string Display { get; set; }
        public static string GetDisplayName(string key)
        {
            switch (key)
            {
                case "FGM":
                    return "Field Goals Made";
                //case "FGA":
                //    return "Field Goal Attempts";
                //    break;
                case "FG_PCT":
                    return "Field Goal %";
                case "FG3M":
                    return "3 Pts Made";
                //case "FG3A":
                //    return "";
                //    break;
                case "FG3_PCT":
                    return "3 Pt %";
                case "FTM":
                    return "Free Throws Made";
                //case "FTA":
                //    return "";
                //    break;
                case "FT_PCT":
                    return "Free Throw %";
                case "OREB":
                    return "Rebounds (Off)";
                case "DREB":
                    return "Rebounds (Def)";
                //case "REB":
                //    return "";
                //    break;
                case "AST":
                    return "Assists";

                case "STL":
                    return "Steals";

                case "BLK":
                    return "Blocks";

                case "TOV":
                    return "Turnovers";

                case "PF":
                    return "Fouls";
                default:
                    break;
            }

            return string.Empty;
        }
    }
}
