using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Developer_Toolbox.Models
{
    public class Badge
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title required!")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Description required!")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The image of the badge is mandatory")]
        public string? Image { get; set; } = "/img/badges/default.png";

        [Required(ErrorMessage = "Target activity required!")]
        public int? TargetActivityId { get; set; }
        public virtual Activity? TargetActivity { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? TargetActivities { get; set; }

        [Required(ErrorMessage = "Number of times the user must complete the activity is required!")]
        public int? TargetNoOfTimes { get; set; }

        public virtual ICollection<Tag>? TargetTags { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? TargetTagsItems { get; set; }

        public int? TargetCategoryId { get; set; }
        public virtual Category? TargetCategory { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? TargetCategories { get; set; }

        public virtual string? TargetLevel { get; set; }

        public virtual ICollection<ApplicationUser>? Users { get; set; }

    }
}
