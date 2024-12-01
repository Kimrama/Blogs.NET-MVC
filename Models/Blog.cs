using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blog.Models;

public class Blog {

    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = null!;
    [Required]
    public string Content { get; set; } = null!;

    [Required]
    public int UserId {get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set;} = null!;

}