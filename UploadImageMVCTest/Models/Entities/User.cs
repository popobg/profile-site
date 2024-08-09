using System.ComponentModel.DataAnnotations;

namespace UploadImageMVCTest.Models.Entities
{
    public class User
    {
        public int Id;

        [Required(ErrorMessage = "You must have a name")]
        [MaxLength(50, ErrorMessage = "Max 50 char")]
        public string Name { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; } = string.Empty;

        // Store public Id for deletion
        public string? ProfileImagePublicId { get; set; } = string.Empty;
    }
}
