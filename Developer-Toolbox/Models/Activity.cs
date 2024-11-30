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


        [Range(0, 100,
            ErrorMessage = "Reputation points gain must be a value between 0 and 100!")]
        public int? ReputationPoints { get; set; }

    }
}
