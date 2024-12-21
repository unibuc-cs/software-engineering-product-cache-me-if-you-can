using System.ComponentModel.DataAnnotations;
using Developer_Toolbox.Models.CustomValidations;


namespace Developer_Toolbox.Models
{
    public class WeeklyChallenge
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The title field is required!")]
        [StringLength(100, ErrorMessage = "The title cannot exceed 100 characters")]
        [MinLength(5, ErrorMessage = "The title must be at least 5 characters long")]
        public string Title { get; set; }
        [Required(ErrorMessage = "The description field is required!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Level of difficulty required!")]
        public string? Difficulty { get; set; }

        [Required(ErrorMessage = "Reward points are required.")]
        [Range(10, 1000, ErrorMessage = "Reward points must be between 10 and 1000.")]
        public int RewardPoints { get; set; }
        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndDate { get; set; } = DateTime.Now;

        // Relație Many-to-Many cu Exercise
        //[MinimumCount(1, ErrorMessage = "At least one exercise is required.")]
        public virtual ICollection<WeeklyChallengeExercise>? WeeklyChallengeExercises { get; set; }
    }
}
