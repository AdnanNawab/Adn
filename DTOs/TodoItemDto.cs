using System.ComponentModel.DataAnnotations;

namespace Adn.DTOs;

public record TodoItemCreateDTO
{

    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public string Title { get; set; }

    [Required]
    public int UserId { get; set; }
}

public record TodoItemUpdateDTO
{

    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public string Title { get; set; } = null;

    // [Required]
    public bool? IsComplete { get; set; } = null;

    [Required]
    public int TodoId { get; set; }
}