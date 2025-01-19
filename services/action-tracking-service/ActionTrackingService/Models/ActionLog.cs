namespace ActionTrackingService.Models
{
    public class ActionLog
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string ActionType { get; set; } // e.g., "click", "view", "favorite"
        public string TargetId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}