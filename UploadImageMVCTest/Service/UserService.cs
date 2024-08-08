using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using UploadImageMVCTest.Models.Entities;
using UploadImageMVCTest.Models.ViewModel;
using UploadImageMVCTest.Repositories;

namespace UploadImageMVCTest.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly Cloudinary _cloudinary;

        public UserService(IUserRepository userRepository, Cloudinary cloudinary)
        {
            _userRepository = userRepository;
            _cloudinary = cloudinary;
        }

        public List<User> GetUsers() => _userRepository.GetUsers();

        public User GetUserById(int Id) => _userRepository.GetUserById(Id);

        public async Task AddUser(UserAdded modelUser)
        {
            if (modelUser.Image != null && modelUser.Image.Length > 0)
            {
                try
                {
                    modelUser = await UploadImageonCloudinary(modelUser);
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
            User user = _userRepository.GetUserById(userId);

            if (user == null) throw new Exception("No user found at this Id");

            UserAdded modelUser = new UserAdded()
            {
                Id = user.Id,
                Name = user.Name,
                ImageUrl = user.ProfilePictureUrl
            };

            return modelUser;
        }

        public async Task EditUser(UserAdded modelUser)
        {
            if (modelUser.Image != null && modelUser.Image.Length > 0)
            {
                try
                {
                    modelUser = await UploadImageonCloudinary(modelUser);
                }
                catch (Exception)
                {
                    throw new Exception("Fail to upload image on cloudinary");
                }
            }

            _userRepository.EditUser(modelUser);
        }

        private async Task<UserAdded> UploadImageonCloudinary(UserAdded modelUser)
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
                modelUser.ImageUrl = uploadResult.SecureUrl.ToString();
                modelUser.ProfileImagePublicId = uploadResult.PublicId;
            }
            else
            {
                throw new Exception("uploadResult status code not Ok, fail to upload image on cloudinary");
            }

            return modelUser;
        }
    }
}
