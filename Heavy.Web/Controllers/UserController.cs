using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heavy.Web.Models;
using Heavy.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Heavy.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }
        public async Task<IActionResult> index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }
        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(UserAddViewModel userAddViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userAddViewModel);
            }
            var user = new ApplicationUser
            {
                UserName = userAddViewModel.UserName,
                Email = userAddViewModel.Email
            };
            var result = await _userManager.CreateAsync(user, userAddViewModel.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(userAddViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "刪除發生錯誤");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "找不到此用戶");
            }
            return View("Index", await _userManager.Users.ToListAsync());
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id ,UserEditViewModel userEditViewModel)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            user.UserName = userEditViewModel.UserName;
            user.Email = userEditViewModel.Email;
            user.IdCardNo = userEditViewModel.IdCardNo;
            user.BirthDate = userEditViewModel.BirthDate;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "編輯錯誤");
            }
            return View("Index", await _userManager.Users.ToListAsync());
        }
    }
}
