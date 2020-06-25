using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using loginregistration.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace loginregistration.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            List<User> AllUsers = dbContext.Users.ToList();
            return View();
        }
        [HttpGet("success")]
        public IActionResult Success()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return Redirect("/");
            }
            //ViewBag.UserId = (int) UserId;
            ViewBag.User = dbContext.Users.FirstOrDefault(u => u.UserId == (int)UserId);
            List<Transaction> transactions = dbContext.Users
                .Include(u => u.Transactions)
                .FirstOrDefault(u => u.UserId == (int) UserId)
                .Transactions;
            ViewBag.Transactions = transactions;
            return View("success");
        }

        [HttpPost("register")]
        public IActionResult Register(User newuser)
        {
            if(ModelState.IsValid)
            {
                User userMatchingEmail = dbContext.Users
                    .FirstOrDefault(u => u.Email == newuser.Email);
                if(userMatchingEmail != null)
                {
                //Manually add a ModelState error to the Email field, with provided
                //error message
                    ModelState.AddModelError("Email", "Email already in use!");
                    //return Redirect("/Index");
                }
                else{
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    newuser.Password = Hasher.HashPassword(newuser, newuser.Password);
                    dbContext.Users.Add(newuser);
                    dbContext.SaveChanges();
                    HttpContext.Session.SetInt32("UserId", newuser.UserId);
                    return Redirect("/success");

                }
        
                
            }
            return View("Index"); 
    

        }

        [HttpGet("loginpage")]
        public IActionResult loginpage()
        {
            return View("login");
        }

        [HttpPost("loginuser")]
        public IActionResult Login(LoginUser user)
        {
            if(ModelState.IsValid)
            {
                User userMatchingEmail = dbContext.Users
                    .FirstOrDefault(u => u.Email == user.LoginEmail);
                if(userMatchingEmail == null)
                {
                    ModelState.AddModelError("LoginEmail", "Unknown Email!");
                }
                else
                {
                    PasswordHasher<LoginUser> Hasher = new PasswordHasher<LoginUser>();
            
            // verify provided password against hash stored in db
                    var result = Hasher.VerifyHashedPassword(user, userMatchingEmail.Password, user.LoginPassword);
                    if(result == 0)
                    {
                        ModelState.AddModelError("LoginPassword", "Incorrect Password!");

                    }
                    else{
                        HttpContext.Session.SetInt32("UserId", userMatchingEmail.UserId);
                        return Redirect("/success");
                    }
                }
            }
            return View("login");
        }
        [HttpGet("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
        [HttpPost("create")]
        public IActionResult create(Transaction newtrans)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Index");
            }
            User user = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            ViewBag.Transactions = dbContext.Transactions.Include(t => t.accountholder)
                                .Where(t => t.UserId == userId)
                                .OrderByDescending(t => t.CreatedAt);
                            
            ViewBag.User = user;
            if(ModelState.IsValid)
            {
                if(user.Balance + newtrans.Amount < 0)
                {
                    ModelState.AddModelError("Amount", "You cannot withdraw more than your current balance!");
                    return View("success");
                }
                newtrans.UserId = (int) userId;
                dbContext.Transactions.Add(newtrans);
                user.Balance += newtrans.Amount;
                dbContext.SaveChanges();
                return Redirect("success");
            }
            return View("success");
        }
    }
            

            
    
}
