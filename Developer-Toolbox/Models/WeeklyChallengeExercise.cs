namespace Developer_Toolbox.Models
{
    public class WeeklyChallengeExercise
    {
        public int Id { get; set; }
        public int? WeeklyChallengeId { get; set; }
        public virtual WeeklyChallenge? WeeklyChallenge { get; set; }

        public int? ExerciseId { get; set; }
        public virtual Exercise? Exercise { get; set; }
    }
}
