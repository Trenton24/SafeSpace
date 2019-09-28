using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SafeSpace.Models;

namespace SafeSpace.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.SetInt32("id", 0);
            List<Users> AllUsers = dbContext.users.ToList();
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(Users user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                System.Console.WriteLine(user.Password);
                bool result =
                user.Password.Any(c => char.IsLetter(c)) &&
                user.Password.Any(c => char.IsDigit(c)) &&
                user.Password.Any(c => char.IsPunctuation(c)) ||
                user.Password.Any(c => char.IsSymbol(c));
                if(result == false)
                {
                    ModelState.AddModelError("Password", "Password must contain at least 1 number, digit, and symbol");
                    return View("Index");
                }
            
                PasswordHasher<Users> Hasher = new PasswordHasher<Users>();
                user.Password = Hasher.HashPassword(user, user.Password);
                dbContext.Add(user);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("id", user.UserId);
                HttpContext.Session.SetString("name", user.FirstName);
                return Redirect("result");
            }
            return View("Index");
        }

        [HttpGet]
        [Route("/signin")]
        public IActionResult signin()
        {
            return View("login");
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginUser user)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.users.FirstOrDefault(u => u.Email == user.Email);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("login");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(user, userInDb.Password, user.Password);
                if(result == 0)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("login");
                }
                HttpContext.Session.SetInt32("id", userInDb.UserId);
                HttpContext.Session.SetString("name", userInDb.FirstName);
                return Redirect("result");
            }
            return View("login");
        }

        [HttpGet]
        [Route("result")]
        public IActionResult Result()
        {
            var test = HttpContext.Session.GetInt32("id");
            if(test <= 0){return Redirect("/");}
            ViewBag.UserId = HttpContext.Session.GetInt32("id");
            ViewBag.Name = HttpContext.Session.GetString("name");
            return View("dashboard");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [HttpGet("friends")]
        public IActionResult Friends()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("id");
            var loggedId = HttpContext.Session.GetInt32("id");
            ViewBag.Name = HttpContext.Session.GetString("name");
            List<Users> users = dbContext.users.OrderBy(a => a.FirstName).ToList();
            return View(users);
        }

        [HttpGet("/user/{UserId}")]
        public IActionResult Profile(int userid)
        {
            var username = dbContext.users.FirstOrDefault(u => u.UserId == userid);
            ViewBag.ProfileName = $"{username.FirstName} {username.LastName}";
            ViewBag.ProfileId = username.UserId;
            ViewBag.UserId = HttpContext.Session.GetInt32("id");
            ViewBag.Name = HttpContext.Session.GetString("name");
            System.Console.WriteLine(username.Pending);
            var tempid = HttpContext.Session.GetInt32("id");
            int id = (int)tempid;
            var model = new UserViewModel();
            model.Person = username;
            model.Pending = dbContext.users.Include(u => u.Requested).Where(u => u.UserId == userid).ToList();
            model.Accepted = dbContext.UserhasFriends.FirstOrDefault(u => u.SentById == id || u.RequestedId == id && u.SentById == username.UserId || u.RequestedId == username.UserId);
            return View(model);
        }
        
        [HttpGet("/Add/{ProfileId}")]
        public IActionResult AddFriend(int profileid)
        {
            int? temp = HttpContext.Session.GetInt32("id");
            dbContext.SaveChanges();
            return Redirect($"/user/{profileid}");
        }

        [HttpGet("/sendrequest/{ProfileId}")]
        public IActionResult SendRequest(int profileid)
        {
            var requesteduser = dbContext.users.FirstOrDefault(u => u.UserId == profileid);
            int? user = HttpContext.Session.GetInt32("id");
            var usertemp = (int)user;
            var sentuser = dbContext.users.FirstOrDefault(u => u.UserId == usertemp);
            var Request = new UserHaveFriends();
            Request.Requested = requesteduser;
            Request.RequestedId = requesteduser.UserId;
            Request.SentBy = sentuser;
            Request.SentById = sentuser.UserId;
            Request.Accepted = 1;
            // ^ use to display if request is sent or not / 1 = Request is Pending
            dbContext.Add(Request);
            dbContext.SaveChanges();
            return Redirect($"/user/{profileid}");
        }

        [HttpGet("/requests/{UserId}")]
        public IActionResult Requests(int userid)
        {
            ViewBag.Name = HttpContext.Session.GetString("name");
            int? user = HttpContext.Session.GetInt32("id");
            var usertemp = (int)user;
            var requests = dbContext.UserhasFriends.Include(u => u.SentBy).Where(u => u.RequestedId == usertemp && u.Accepted == 1).ToList();
            return View("friendrequests", requests);
        }

        [HttpGet("/accept/{SentId}")]
        public IActionResult Accept(int SentId)
        {
            int? user = HttpContext.Session.GetInt32("id");
            var usertemp = (int)user;
            var NewFriend = new Friends();
            NewFriend.user1Id = SentId;
            //user1 is always the user that sent the request
            NewFriend.user2Id = usertemp;
            //user2 is always the user that accepted the request
            dbContext.Add(NewFriend);
            UserHaveFriends delrequest = dbContext.UserhasFriends.FirstOrDefault(u => u.SentById == SentId && u.RequestedId == usertemp);
            delrequest.Accepted = 2;
            // ^ 2 = Request Accepted
            dbContext.UserhasFriends.Update(delrequest);
            dbContext.SaveChanges();
            return Redirect($"/requests/{usertemp}");
        }

        [HttpGet("/deny/{SentId}")]
        public IActionResult Deny(int SentId)
        {
            int? user = HttpContext.Session.GetInt32("id");
            var usertemp = (int)user;
            UserHaveFriends delrequest = dbContext.UserhasFriends.FirstOrDefault(u => u.SentById == SentId && u.RequestedId == usertemp);
            dbContext.UserhasFriends.Remove(delrequest);
            dbContext.SaveChanges();
            return Redirect($"/requests/{usertemp}");
        }

        [HttpGet("/friendlist/{UserId}")]
        public IActionResult Friendlist(int userid)
        {
            List<Friends> friendlist = dbContext.Friends.Where(u => u.user1Id == userid || u.user2Id == userid).ToList();
            List<Users> UserList = new List<Users>();
            foreach(Friends f in friendlist)
            {
                int temp = 0;
                if(f.user1Id != userid)
                {
                    temp = 1;
                }
                else if(f.user2Id != userid)
                {
                    temp = 2;
                }
                if(temp == 1)
                {
                    Users newuser = dbContext.users.FirstOrDefault(u => u.UserId == f.user1Id);
                    UserList.Add(newuser);
                }
                if(temp == 2)
                {
                    Users newuser = dbContext.users.FirstOrDefault(u => u.UserId == f.user2Id);
                    UserList.Add(newuser);
                }
            }
            return View(UserList);
        }

        [HttpGet("delete/{userid}")]
        public IActionResult DeleteUser(int userid)
        {
            var user = dbContext.users.FirstOrDefault(u => u.UserId == userid);
            dbContext.users.Remove(user);
            dbContext.SaveChanges();
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
