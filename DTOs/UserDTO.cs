using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Todo.DTOs;

public record UserLoginDTO
{
   [Required]
   [JsonPropertyName("userName")]
   [MinLength(3)]
   [MaxLength(255)]

    public string UserName { get; set; }

    [Required]
   [JsonPropertyName("password")]
   
   [MaxLength(255)]

    public string Password { get; set; }
}

public record UserLoginResDTO
{
     [JsonPropertyName("token")]
    public string Token { get; set; }

 [JsonPropertyName("username")]
    public string UserName { get; set; }

 [JsonPropertyName("id")]
    public int Id { get; set; }
}