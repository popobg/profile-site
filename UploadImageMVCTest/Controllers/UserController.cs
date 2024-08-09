using Microsoft.AspNetCore.Mvc;
using UploadImageMVCTest.Models.Entities;
using UploadImageMVCTest.Models.ViewModel;
using UploadImageMVCTest.Service;

namespace UploadImageMVCTest.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult List()
        {
            return View(_userService.GetUsers());
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserAdded modelUser)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await _userService.AddUserAsync(modelUser);
                return RedirectToAction("List");
            }
            catch (Exception)
            {
                // ViewError with a message?
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            try
            {
                UserAdded modelUser = _userService.GetUserToUpdate(Id);

                if (modelUser is null) throw new Exception("The model user is null");

                return View(modelUser);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserAdded updatedUser, int Id)
        {
            updatedUser.Id = Id;

            if (!ModelState.IsValid)
            {
                if (updatedUser is null) throw new Exception("The model received is null");

                try
                {
                    UserAdded modelUser = _userService.GetUserToUpdate(updatedUser.Id);

                    if (modelUser is null) throw new Exception("The model user is null");

                    return View(modelUser);
                }
                catch (Exception)
                {
                    return NotFound();
                }
            }

            try
            {
                await _userService.EditUserAsync(updatedUser);
                return RedirectToAction("List");
            }
            catch (Exception)
            {
                // ViewError with a message?
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                await _userService.DeleteUserAsync(Id);

                return RedirectToAction("List");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProfilePicture(int Id)
        {
            try
            {
                User? user = _userService.GetUserById(Id);

                if (user is null) throw new Exception("No user found by this Id");

                await _userService.DeleteProfilePictureAsync(user);

                return RedirectToAction("Edit", new {Id});
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
