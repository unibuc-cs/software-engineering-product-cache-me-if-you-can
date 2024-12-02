namespace Developer_Toolbox.Models
{
    public class UserBadge
    {
        public string? UserId { get; set; }
        public int? BadgeId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public virtual Badge? Badge { get; set; }
        public DateTime? ReceivedAt { get; set; }
    }
}
