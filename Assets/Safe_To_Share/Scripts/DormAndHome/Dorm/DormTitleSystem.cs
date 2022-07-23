using AvatarStuff.Holders;

namespace DormAndHome.Dorm
{
    public static class DormTitleSystem
    {
        public static string GeneralTitleAllFollowers = "Follower";

        static readonly ValueAndString[] NegativeOptions =
        {
            new("stranger", -10),
            new("adversary", -25),
            new("enemy", -50),
        };

        static readonly ValueAndString[] PositiveOptions =
        {
            new("companion", 50),
            new("friend", 25),
            new("acquaintance", 10),
        };

        public static void ChangeGeneralFollowerTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                return;
            GeneralTitleAllFollowers = newTitle;
        }

        public static string TitleConversion(this DormMate mate)
        {
            float aff = mate.RelationsShips.GetRelationShipWith(PlayerHolder.PlayerID).Affection;
            float sub = mate.RelationsShips.GetRelationShipWith(PlayerHolder.PlayerID).Submission;
            return aff >= 0 ? PositiveTree(aff, sub) : NegativeTree(aff, sub);
        }

        static string NegativeTree(float aff, float sub)
        {
            if (sub > 10)
                return NegSubTree(aff, sub);
            if (sub < -10)
                return NegDomTree(aff, sub);
            foreach (var option in NegativeOptions)
                if (aff > option.Value)
                    return option.Title;
            return string.Empty;
        }

        static string NegDomTree(float aff, float sub) => string.Empty;

        static string NegSubTree(float aff, float sub)
        {
            if (sub < 10)
                return "";
            if (sub < 25)
                return "";
            if (sub < 50)
                return "";
            return "jeff";
        }

        static string PositiveTree(float aff, float sub)
        {
            if (sub > 10)
                return PosSubTree(aff, sub);
            if (sub < -10)
                return PosDomTree(aff, sub);
            foreach (var option in PositiveOptions)
                if (aff > option.Value)
                    return option.Title;
            return "stranger";
        }

        static string PosDomTree(float aff, float sub) => string.Empty;

        static string PosSubTree(float aff, float sub) => string.Empty;

        readonly struct ValueAndString
        {
            public ValueAndString(string title, float value)
            {
                Title = title;
                Value = value;
            }

            public readonly string Title;
            public readonly float Value;
        }
    }
}