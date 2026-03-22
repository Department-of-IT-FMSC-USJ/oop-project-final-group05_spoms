using SmartPostOffice.Models.Enums;

namespace SmartPostOffice.Models
{
    public class StampTrackingHistory
    {
        public int Id { get; set; }

        // FK to StampOrder
        public int StampOrderId { get; set; }
        public StampOrder StampOrder { get; set; } = null!;

        public StampOrderStatus Status { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Which officer changed the status
        public int? UpdatedByOfficerId { get; set; }
        public string? Notes { get; set; }
    }
}