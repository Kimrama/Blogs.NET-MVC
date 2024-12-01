using Microsoft.AspNetCore.Mvc;
using blog.Models;
using blog.Data;

namespace blog.Controllers {
    public class UsersController : Controller {
        private readonly AppDbContext _db;

        public UsersController(AppDbContext db) {
            _db = db;
        }
        public IActionResult Index() {
            return View();
        }
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterJson([FromBody] User user) {
            if (ModelState.IsValid) {

                if (_db.Users.Any(dbUser => dbUser.Username == user.Username)) {
                    ModelState.AddModelError("Username", "This username is already exits.");
                    return BadRequest(ModelState);
                }
                _db.Users.Add(user);
                _db.SaveChanges();
                return Ok("User registered successfully");
            }
            return BadRequest(ModelState);
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (_db.Users.Any(dbUser => dbUser.Username == user.Username))
            {
                ModelState.AddModelError("Username", "This username is already taken.");
                return View(user); 
            }

            _db.Users.Add(user);
            _db.SaveChanges();
            return RedirectToAction("Index", "Users");
        }
        
        [HttpPost]
        public IActionResult LoginJson([FromBody] User user) {
            var resultUser = _db.Users.FirstOrDefault(dbUser => dbUser.Username == user.Username && dbUser.Password == user.Password);
            if (resultUser == null) {
                return Unauthorized("Invalid username or password");
            }
            return Ok($"{resultUser.Username}");
        }

        [HttpPost]
        public IActionResult Login([FromForm] User user) {
            var resultUser = _db.Users.FirstOrDefault(dbUser => dbUser.Username == user.Username && dbUser.Password == user.Password);
            if (resultUser == null) {
                return Unauthorized("Invalid username or password");
            }
            return Ok($"{resultUser.Username}");
        }

    }
}