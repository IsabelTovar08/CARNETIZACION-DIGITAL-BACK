namespace Entity.DTOs.Notifications
{
    public class NotificationReceivedDto
    {
        public int StatusId { get; set; }
        public DateTime SendDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
} 