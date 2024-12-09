namespace Developer_Toolbox.Models
{
    public class BadgeTag
    {
        public int? BadgeId { get; set; }
        public int? TagId { get; set; }
        public virtual Tag? Tag { get; set; }
        public virtual Badge? Badge { get; set; }

        public BadgeTag(int? badgeId, int? tagId) 
        {
            BadgeId = badgeId;
            TagId = tagId;
        }
    }
}
