using UploadImageMVCTest.Models.Entities;
using UploadImageMVCTest.Models.ViewModel;

namespace UploadImageMVCTest.Repositories
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        User GetUserById(int Id);
        void AddUser(UserAdded userAdded);
        void EditUser(UserAdded userAdded);
    }
}
