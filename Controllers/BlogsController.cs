using Microsoft.AspNetCore.Mvc;
using blog.Models;
using blog.Data;

namespace blog.Controllers;

public class BlogsController : Controller {

    private readonly AppDbContext _db;

    public BlogsController(AppDbContext db) {
        _db = db;
    }

    public IActionResult Index() {

        IEnumerable<Blog> blogs = _db.Blogs;
        return View(blogs);
    }
    public IActionResult Create() {
            return View();
    }

    [HttpPost]
    public IActionResult Create([FromForm] Blog blog) {
        
        if (ModelState.IsValid) {

            int editerId = int.Parse(HttpContext.Request.Cookies["UserId"] ?? "0");
            if (editerId == 0) {
                return Unauthorized();
            }
            blog.UserId = editerId;

            _db.Blogs.Add(blog); 
            _db.SaveChanges();   
            return RedirectToAction("Index", "Blogs");
        }

        return BadRequest(ModelState);
    }


    public IActionResult Test() {
        return Ok("Hello Test");
    }


}