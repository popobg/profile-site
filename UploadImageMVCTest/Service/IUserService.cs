using UploadImageMVCTest.Models.Entities;
using UploadImageMVCTest.Models.ViewModel;

namespace UploadImageMVCTest.Service
{
    public interface IUserService
    {
        List<User> GetUsers();
        User GetUserById(int Id);
        Task AddUser(UserAdded modelUser);
        UserAdded GetUserToUpdate(int userId);
        Task<UserAdded> UploadImageonCloudinary(UserAdded modelUser);
        Task EditUser(UserAdded modelUser);
    }
}
