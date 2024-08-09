using System.ComponentModel.DataAnnotations;
using UploadImageMVCTest.Helper;
using UploadImageMVCTest.Models.Entities;

namespace UploadImageMVCTest.Models.ViewModel
{
    public class UserAdded : User
    {
        [Display(Name = "Choose a profile picture visible to others"), ]
        [Required(ErrorMessage = "You have to add a profile picture")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" })]
        [MaxFileSize(2 * 1024 * 1024)]
        public IFormFile? Image { get; set; }
    }
}
