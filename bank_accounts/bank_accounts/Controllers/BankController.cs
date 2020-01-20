using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using bank_accounts.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace bank_accounts.Controllers
{
    public class BankController : Controller
    {
        private BankContext _context;
        public BankController(BankContext context)
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
                return Redirect ($"/Dashboard/{loggedUser.UserId}");
            };
            return View("Index");
        }
        [HttpGet]
        [Route("/login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [Route("LoginUser")]
        public IActionResult LoginUser(LoginUser returningUser)
        {
            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            User loginUser = _context.Users.Where(user => user.Email == returningUser.Email).SingleOrDefault();
            if(loginUser == null)
            {
                ModelState.AddModelError("Email", "Invalid Email/Passowrd");
            }
            else if(hasher.VerifyHashedPassword(returningUser,loginUser.Password, returningUser.Password) == 0)
            {
                ModelState.AddModelError("Email", "Invalid Email/Passowrd");
            }
            if(!ModelState.IsValid)
            {
                return View("Login");
            }
            HttpContext.Session.SetInt32("id", loginUser.UserId);
            return Redirect ($"/Dashboard/{loginUser.UserId}");
        }
        [HttpGet]
        [Route("Dashboard/{user_id}")]
        public IActionResult Dashboard(int user_id)
        {
            if(HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index");
            }
            User theUser = _context.Users.Include( u => u.Transactions).Where(u => u.UserId == user_id).SingleOrDefault();
            return View( theUser);
        }
        [HttpPost]
        [Route("Dashboard/MakeTransaction")]
        public IActionResult MakeTransaction(int transaction_amount)
        {
            int? user_id = HttpContext.Session.GetInt32("id");
            User user = _context.Users.Where(u => u.UserId == user_id).SingleOrDefault();
            int current_balance = _context.Users.Where(u => u.UserId == user_id).Select(u => u.Balance).SingleOrDefault();
            if(transaction_amount < 0 && Math.Abs(transaction_amount) > current_balance)
            {
                // Could you use modelstate to do this?
                ViewBag.Error = null;
                TempData["error"] = "Can not withdraw that amount, withdrawal amount must be less than or equal to current balance";
                return Redirect ($"/Dashboard/{user.UserId}");
            }
            Transaction newTransaction = new Transaction
            {
                Amount = transaction_amount,
                UserId = (int)user_id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            user.Balance += transaction_amount;
            _context.Transactions.Add(newTransaction);
            _context.SaveChanges();
            return Redirect ($"/Dashboard/{user.UserId}");
        }
    }
}
