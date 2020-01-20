using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using dojo_activity_center.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace dojo_activity_center.Controllers
{
    public class UserController : Controller
    {
        private DojoActivityContext _context;
        public UserController(DojoActivityContext context)
        {
            _context = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("RegisterUser")]
        public IActionResult RegisterUser(RegisterUser newUser)
        {
            if(_context.Users.Where(user => user.Email == newUser.Email).SingleOrDefault() != null)
            {
                ModelState.AddModelError("Email", "Email already in use");
            }
            PasswordHasher<RegisterUser> hasher = new PasswordHasher<RegisterUser>();
            if(ModelState.IsValid)
            {
                User theUser = new User
                {
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Email = newUser.Email,
                    Password = hasher.HashPassword(newUser, newUser.Password),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,          
                };
                User loggedUser = _context.Add(theUser).Entity;
                _context.SaveChanges();
                HttpContext.Session.SetInt32("id", loggedUser.UserId);
                return RedirectToAction ("Index", "Activity");
            };
            return View("Index");
        }
        [Route("LoginUser")]
        public IActionResult LoginUser(LoginUser returningUser)
        {
            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            User loginUser = _context.Users.Where(user => user.Email == returningUser.LogEmail).SingleOrDefault();
            if(loginUser == null)
            {
                ModelState.AddModelError("LogEmail", "Invalid Email/Password");
            }
            else if(hasher.VerifyHashedPassword(returningUser,loginUser.Password, returningUser.LogPassword) == 0)
            {
                ModelState.AddModelError("LogEmail", "Invalid Email/Passowrd");
            }
            if(!ModelState.IsValid)
            {
                return View("Index");
            }
            HttpContext.Session.SetInt32("id", loginUser.UserId);
            return RedirectToAction ("Index", "Activity");
        }
        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
