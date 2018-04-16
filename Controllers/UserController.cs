using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bank_account.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace bank_account.Controllers
{
    public class UserController : Controller
    {
        private BankContext _context;
    
        public UserController(BankContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                User CheckExist = _context.Users.SingleOrDefault(user => user.Email == model.Email);
                if(CheckExist == null)
                {
                    User newUser = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Password = model.Password,
                        Balance = 0, 
                    };
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                    _context.Add(newUser);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("userid", newUser.UserId);
                    return Redirect($"/account/{newUser.UserId}");          
                }
                else
                {
                    TempData["email_exists"] = "That email has already been registered";
                }
            }
            return View("Index");
        }
        [HttpGet]
        [Route("user/login")]
        public IActionResult UserLogin()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string Email, string Password)
        {
            User Returned = _context.Users.SingleOrDefault(user => user.Email == Email);
            if(Returned != null && Password != null)
            {
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(Returned, Returned.Password, Password))
                {
                    HttpContext.Session.SetInt32("userid", Returned.UserId);
                    return Redirect($"/account/{Returned.UserId}");
                }
                else
                {
                    @TempData["passError"] = "Incorrect password entered";
                }
            }
            else
            {
                @TempData["emailReg"] = "Please register this email before logging in";                
            }
            return RedirectToAction("UserLogin");
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
