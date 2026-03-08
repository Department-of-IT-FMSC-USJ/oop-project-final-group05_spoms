using System.ComponentModel.DataAnnotations;

namespace SmartPostOffice.Models
{
    public class PostOfficeOfficer
    {
        public int Id { get; set; }

        [Required][StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required][EmailAddress][StringLength(150)]
        public string Email { get; set; } = string.Empty;  // login username

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(20)]
        public string OfficerId { get; set; } = string.Empty;  // e.g. OFF-001

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
