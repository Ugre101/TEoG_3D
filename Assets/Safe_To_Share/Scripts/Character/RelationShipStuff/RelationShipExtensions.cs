﻿namespace Character.RelationShipStuff {
    public static class RelationShipExtensions {
        public const float FirstThreesHold = 10;
        public const float SecondThreesHold = 25;

        public static string DominanceTitle(this RelationShip relationsShip) {
            var sub = relationsShip.Submission;
            if (sub > FirstThreesHold)
                return SubmissionTree(sub);

            if (sub < -FirstThreesHold)
                return DomisionTree(sub);

            return "neutral";
        }

        static string DomisionTree(float sub) {
            if (sub < -SecondThreesHold)
                return "dominant";
            return "arrogant";
        }

        static string SubmissionTree(float sub) {
            if (sub > SecondThreesHold)
                return "submissive";
            return "meek";
        }

        public static string AffectionTitle(this RelationShip relationShip) {
            var affection = relationShip.Affection;
            if (affection > FirstThreesHold)
                return LikeTree(affection);

            if (affection < -FirstThreesHold)
                return DislikeTree(affection);
            return "neutral";
        }

        static string LikeTree(float affection) {
            if (affection > SecondThreesHold)
                return "friendly";
            return "acquaintance";
        }

        static string DislikeTree(float affection) {
            if (affection < -SecondThreesHold)
                return "dislike";
            return "indifferent";
        }
    }
}