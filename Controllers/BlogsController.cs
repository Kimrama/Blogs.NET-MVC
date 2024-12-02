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
    public IActionResult Create([FromForm] Blog blog, IFormFile? Image) {
        
        if (ModelState.IsValid) {

            int editerId = int.Parse(HttpContext.Request.Cookies["UserId"] ?? "0");
            if (editerId == 0) {
                return Unauthorized();
            }
            
            if(Image != null && Image.Length > 0) {

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder)) {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var uniqueFileName = $"{Guid.NewGuid()}_{Image.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }

                blog.ImagePath = $"/uploads/{uniqueFileName}";
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