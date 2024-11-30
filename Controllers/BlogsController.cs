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


    [HttpPost]
    public IActionResult Create([FromBody] Blog blog) {
        System.Console.WriteLine("title", blog.Title);
        if (ModelState.IsValid) {
            _db.Blogs.Add(blog); 
            _db.SaveChanges();   
            return Ok("Blog added successfully!");
        }

        return BadRequest(ModelState);
    }


    public IActionResult Test() {
        return Ok("Hello Test");
    }


}