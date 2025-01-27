using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Developer_Toolbox.Models
{
    public class LockedExercise
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Learning path required!")]
        public int? LearningPathId { get; set; }
        public virtual LearningPath? LearningPath { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? LearningPaths { get; set; }

        public virtual ICollection<Solution>? Solutions { get; set; }

        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        [Required(ErrorMessage = "Title required!")]
        [StringLength(100, ErrorMessage = "Title can't have more than 50 characters!")]
        [MinLength(3, ErrorMessage = "Title can't have less than 3 characters!")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Description required!")]
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
        [Required(ErrorMessage = "Summary required!")]
        public string? Summary { get; set; }
        [Required(ErrorMessage = "Restrictions required!")]
        public string? Restrictions { get; set; }
        [Required(ErrorMessage = "Examples required!")]
        public string? Examples { get; set; }
        [Required(ErrorMessage = "Level of difficulty required!")]
        public string? Difficulty { get; set; }
        [Required(ErrorMessage = "Test cases required")]
        public string? TestCases { get; set; }

        [Required(ErrorMessage = "Instructions required")]
        public string? Instructions { get; set; }

        public virtual ICollection<LockedSolution>? LockedSolutions { get; set; }
    


}
}
