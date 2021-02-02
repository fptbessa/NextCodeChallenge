using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NextCodeChallenge.Models.Entities;

namespace NextCodeChallenge.Controllers
{
    public class UsersController : Controller
    {
        private readonly NextCodeChallengeDbContext _context;

        public UsersController(NextCodeChallengeDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public IActionResult Index()
        {
            return View();
        }

        // POST: Users/Login

        public ActionResult Login(string Username, string Password)
        {
            if (ModelState.IsValid)
            {
                var dbUser = _context.Users.FirstOrDefault(ele => ele.Username.Equals(Username) && ele.Password.Equals(Password));

                if (dbUser != null)
                {
                    HttpContext.Session.SetInt32("UserId", dbUser.UserId);
                    HttpContext.Session.SetString("Username", dbUser.Username);
                    TempData["LoginMessage"] = "Logged in.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error: User credentials not found.";
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
