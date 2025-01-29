using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Developer_Toolbox.Models
{
    public class LearningPath
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Learning path required!")]
        public string Name { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public virtual ICollection<LockedExercise>? LockedExercises { get; set; }

        [Required(ErrorMessage = "Description of learning path is required!")]
        public string? Description { get; set; }

    }
}
