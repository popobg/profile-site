using Microsoft.AspNetCore.Razor.Language.Extensions;
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
                Name = "Rachid"
            }
        };

        public List<User> GetUsers() => _users;

        public User? GetUserById(int userId) => _users.Find(u => u.Id == userId);

        public void AddUser(UserAdded userAdded)
        {
            User newUser = new User
            {
                Id = _users.Max(u => u.Id) + 1,
                Name = userAdded.Name,
                ProfilePictureUrl = userAdded.ProfilePictureUrl
            };

            _users.Add(newUser);
        }

        public void EditUser(UserAdded userAdded)
        {
            if (_users.Find(u => u.Id == userAdded.Id) is User found && found != null)
            {
                found.Name = userAdded.Name;
                found.ProfilePictureUrl = userAdded.ProfilePictureUrl;
                found.ProfileImagePublicId = userAdded.ProfileImagePublicId;
            }
        }

        public void DeleteUser(User user)
        {
            User? userToRemove = _users.Find(u => u.Id == user.Id);
            _users.Remove(userToRemove);
        }
    }
}
