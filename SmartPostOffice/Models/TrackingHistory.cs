using SmartPostOffice.Models.Enums;
namespace SmartPostOffice.Models
{
    public class TrackingHistory
    {
        public int Id { get; set; }
        public int ServiceRequestId { get; set; }
        public ServiceRequest ServiceRequest { get; set; } = null!;
        public RequestStatus Status { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? UpdatedByOfficerId { get; set; }
        public string? Notes { get; set; }
    }
}
