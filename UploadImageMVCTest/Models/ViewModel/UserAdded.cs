using System.ComponentModel.DataAnnotations;
using UploadImageMVCTest.Helper;

namespace UploadImageMVCTest.Models.ViewModel
{
    public class UserAdded
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must have a name")]
        [MaxLength(50, ErrorMessage = "Max 50 char")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Choose a profile picture visible to others"), ]
        [Required(ErrorMessage = "You have to add a profile picture")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" })]
        [MaxFileSize(2 * 1024 * 1024)]
        public IFormFile? Image { get; set; }

        public string? ImageUrl { get; set; }

        public string? ProfileImagePublicId { get; set; } = string.Empty;
    }
}
