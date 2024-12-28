using System.ComponentModel.DataAnnotations;

namespace Developer_Toolbox.Models
{
    public class Solution
    {
        [Key]
        public int Id { get; set; }
        public String? SolutionCode { get; set; }
        public int? Score { get; set; }
        public int? ExerciseId { get; set; }
        public virtual Exercise? Exercise { get; set; }

        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        // Adăugăm un câmp pentru data creării soluției
        public DateTime? CreatedAt { get; set; } = DateTime.Now; // Se setează automat la data curentă

    }
}
