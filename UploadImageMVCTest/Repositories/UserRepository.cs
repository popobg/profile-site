using UploadImageMVCTest.Models.Entities;
using UploadImageMVCTest.Models.ViewModel;

namespace UploadImageMVCTest.Repositories
{
    public class UserRepository : IUserRepository
    {
        public static List<User> _users = new List<User>()
        {
            new User()
            {
                Id = 1,
                Name = "Rachid",
                ProfilePictureUrl = null
            }
        };

        public List<User> GetUsers() => _users;

        public User? GetUserById(int Id) => _users.Find(u => u.Id == Id);

        public void AddUser(UserAdded userAdded)
        {
            User newUser = new User
            {
                Name = userAdded.Name,
                ProfilePictureUrl = userAdded.ImageUrl
            };

            _users.Add(newUser);
        }

        public void EditUser(UserAdded userAdded)
        {
            if (_users.Find(u => u.Id == userAdded.Id) is User found && found != null)
            {
                found.Name = userAdded.Name;
                found.ProfilePictureUrl = userAdded.ImageUrl;
            }
        }
    }
}
