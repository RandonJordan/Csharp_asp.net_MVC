using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ViewModelFun.Models;

namespace ViewModelFun.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            string coolString = " This is the string that you are getting!";
            return View("Index", coolString);
        }
        [HttpGet("numbers")]
        public IActionResult Numbers()
        {
            int[] numbersArray = new int[]{1,4,6,98,4,3 };


            return View("Numbers", numbersArray);
        }
        [HttpGet("users")]
        public IActionResult Users()
        {
            User newUser = new User()
            {
                FirstName = "Randon",
                LastName = "Jordan"
            };
            User otherUser = new User()
            {
                FirstName = "John",
                LastName = "Wick"
            };
            List<User> viewModel = new List<User>()
            {
                newUser, otherUser
            };

            return View("Users", viewModel);
        }
        [HttpGet("user")]
        public IActionResult User()
        {   
            User oneUser = new User()
            {
                FirstName = "Pizza",
                LastName = "Man"
            };
            return View(oneUser);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
