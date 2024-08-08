using System.ComponentModel.DataAnnotations;

namespace UploadImageMVCTest.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; } = string.Empty;

        // Store public Id for deletion
        public string? ProfileImagePublicId { get; set; } = string.Empty;
    }
}
