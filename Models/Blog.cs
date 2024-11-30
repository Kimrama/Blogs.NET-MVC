using System.ComponentModel.DataAnnotations;

namespace blog.Models;

public class Blog {

    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = null!;
    [Required]
    public string Content { get; set; } = null!;

}