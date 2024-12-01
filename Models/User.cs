using System.ComponentModel.DataAnnotations;

namespace blog.Models {
    public class User {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Password { get; set; } = null!;

        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    }
}