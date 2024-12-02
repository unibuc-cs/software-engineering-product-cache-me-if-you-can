using System.ComponentModel.DataAnnotations;

namespace Developer_Toolbox.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public virtual ICollection<QuestionTag>? QuestionTags { get; set; }

        [Required(ErrorMessage ="The name field is required!")]
        public string? Name { get; set; }

        public virtual ICollection<BadgeTag>? BadgeTags { get; set; }

    }
}
