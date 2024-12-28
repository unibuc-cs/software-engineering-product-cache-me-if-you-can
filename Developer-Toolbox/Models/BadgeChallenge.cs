namespace Developer_Toolbox.Models
{
    public class BadgeChallenge
    {
        public int? BadgeId { get; set; }
        public int? WeeklyChallengeId { get; set; }
        public virtual WeeklyChallenge? WeeklyChallenge { get; set; }
        public virtual Badge? Badge { get; set; }

        public BadgeChallenge(int? badgeId, int? weeklyChallengeId)
        {
            BadgeId = badgeId;
            WeeklyChallengeId = weeklyChallengeId;
        }
    }
}
