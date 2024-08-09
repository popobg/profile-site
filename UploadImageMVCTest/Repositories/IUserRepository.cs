using UploadImageMVCTest.Models.Entities;
using UploadImageMVCTest.Models.ViewModel;

namespace UploadImageMVCTest.Repositories
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        User? GetUserById(int userId);
        void AddUser(UserAdded userAdded);
        void EditUser(UserAdded userAdded);
        void DeleteUser(User user);
    }
}
