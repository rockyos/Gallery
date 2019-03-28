using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreTest.Models;
using CoreTest.Repository;
using CoreTest.Services;
using CoreTest.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreTest.Controllers
{
    public class AccountController : Controller
    {
        protected UnitOfWork UnitOfWork { get; }
        public AccountController(PhotoContext context, UnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await (await UnitOfWork.UserRepository.GetAllAsync()).FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.Email); 
                    return RedirectToAction("Index", "Photos");
                }
                ModelState.AddModelError("", "Wrong login/password");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await (await UnitOfWork.UserRepository.GetAllAsync()).FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    await UnitOfWork.UserRepository.InsertAsync(new User { Email = model.Email, Password = model.Password });
                    await UnitOfWork.SubmitChangesAsync();
                    await Authenticate(model.Email); 
                    return RedirectToAction("Index", "Photos");
                } 
                ModelState.AddModelError("", "Wrong login/password");
            }
            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}