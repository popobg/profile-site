using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net;
using UploadImageMVCTest.Models.Entities;
using UploadImageMVCTest.Models.ViewModel;
using UploadImageMVCTest.Repositories;

namespace UploadImageMVCTest.Service
{
    public class UserService : IUserService
    {
        // Repository
        private readonly IUserRepository _userRepository;
        // API Cloudinary
        private readonly Cloudinary _cloudinary;

        public UserService(IUserRepository userRepository, Cloudinary cloudinary)
        {
            _userRepository = userRepository;
            _cloudinary = cloudinary;
        }

        public List<User> GetUsers() => _userRepository.GetUsers();

        public User? GetUserById(int userId) => _userRepository.GetUserById(userId);

        public async Task AddUserAsync(UserAdded modelUser)
        {
            if (modelUser.Image != null && modelUser.Image.Length > 0)
            {
                try
                {
                    modelUser = await UploadImageCloudinaryAsync(modelUser);
                }
                catch (Exception)
                {
                    throw new Exception("Fail to upload image on cloudinary");
                }
            }

            _userRepository.AddUser(modelUser);
        }

        public UserAdded GetUserToUpdate(int userId)
        {
            User? user = _userRepository.GetUserById(userId);

            if (user == null) throw new Exception("No user found at this Id");

            UserAdded modelUser = new UserAdded()
            {
                Id = user.Id,
                Name = user.Name,
                ProfilePictureUrl = user.ProfilePictureUrl
            };

            return modelUser;
        }

        public async Task EditUserAsync(UserAdded modelUser)
        {
            if (modelUser.Image != null && modelUser.Image.Length > 0)
            {
                User? user = _userRepository.GetUserById(modelUser.Id);

                if (user is null) throw new Exception("No user found at this ID.");

                modelUser.ProfileImagePublicId = user.ProfileImagePublicId;

                try
                {
                    if (!string.IsNullOrEmpty(modelUser.ProfileImagePublicId))
                    {
                        modelUser = await DeleteImageCloudinaryAsync(modelUser);
                    }
                    modelUser = await UploadImageCloudinaryAsync(modelUser);
                }
                catch (Exception)
                {
                    throw new Exception("Fail to upload image on cloudinary");
                }
            }

            _userRepository.EditUser(modelUser);
        }

        public async Task DeleteUserAsync(int userId)
        {
            try
            {
                User? user = _userRepository.GetUserById(userId);

                if (user is null) throw new Exception("No user found with this Id");

                user = await DeleteProfilePictureAsync(user);

                _userRepository.DeleteUser(user);
            }
            catch (Exception)
            {
                throw new Exception("There was a problem during the deletion operation");
            }
        }

        public async Task<User> DeleteProfilePictureAsync(User user)
        {
            try
            {
                UserAdded modelUser = new UserAdded()
                {
                    Id = user.Id,
                    Name = user.Name,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                    ProfileImagePublicId = user.ProfileImagePublicId
                };

                if (!string.IsNullOrEmpty(user.ProfileImagePublicId))
                {
                    modelUser = await DeleteImageCloudinaryAsync(modelUser);
                    user.ProfilePictureUrl = null;
                    user.ProfileImagePublicId = null;
                }

                return modelUser;
            }
            catch (Exception)
            {
                throw new Exception("There was a problem during the deletion operation");
            }
        }

        private async Task<UserAdded> UploadImageCloudinaryAsync(UserAdded modelUser)
        {
            // unique file name, different then the one given by the user
            string fileName = Guid.NewGuid().ToString() + "_" + modelUser.Image.FileName;

            // Transformation : resize(150*150), crop = resize the image
            // to fill the 150*150 respecting the aspect ratio of the image,
            // gravity = focus the image on the face if it is found
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, modelUser.Image.OpenReadStream()),
                Transformation = new Transformation().Width(150).Height(150).Crop("fill").Gravity("face"),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true,
                Folder = "Test_profile_images"
            };

            // upload(file, options)
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                modelUser.ProfilePictureUrl = uploadResult.SecureUrl.ToString();
                modelUser.ProfileImagePublicId = uploadResult.PublicId;
            }
            else
            {
                throw new Exception("uploadResult status code not Ok, fail to upload image on cloudinary");
            }

            return modelUser;
        }

        private async Task<UserAdded> DeleteImageCloudinaryAsync(UserAdded modelUser)
        {
            var deletionParams = new DeletionParams(modelUser.ProfileImagePublicId);

            var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

            if (deletionResult.StatusCode == HttpStatusCode.OK)
            {
                // Remove the image URL and PublicId from the user
                modelUser.ProfilePictureUrl = null;
                modelUser.ProfileImagePublicId = null;
                return modelUser;
            }
            else
            {
                throw new Exception("deletionResult Status Code not Ok, can not delete the image on the cloudinary");
            }
        }
    }
}
