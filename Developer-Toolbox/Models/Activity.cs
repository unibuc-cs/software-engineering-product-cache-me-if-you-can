using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Developer_Toolbox.Models
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Description required!")]
        public string? Description { get; set; }

        public int? ReputationPoints { get; set; }

    }
}
