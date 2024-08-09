using UploadImageMVCTest.Models.Entities;
using UploadImageMVCTest.Models.ViewModel;

namespace UploadImageMVCTest.Service
{
    public interface IUserService
    {
        List<User> GetUsers();
        User? GetUserById(int userId);
        Task AddUserAsync(UserAdded modelUser);
        UserAdded GetUserToUpdate(int userId);
        Task EditUserAsync(UserAdded modelUser);
        Task DeleteUserAsync(int userId);
        Task<User> DeleteProfilePictureAsync(User user);
    }
}
