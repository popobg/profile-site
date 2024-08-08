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
                await _userService.AddUser(modelUser);
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
        public async Task<IActionResult> Edit(UserAdded updatedUser)
        {
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
                await _userService.EditUser(updatedUser);
                return RedirectToAction("List");
            }
            catch (Exception)
            {
                // ViewError with a message?
                return NotFound();
            }


            return RedirectToAction("List");
        }
    }
}
