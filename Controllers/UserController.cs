// using Adn.DTOs;
// using Adn.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Adn.Repositories;
using Adn.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Todo.DTOs;
using User.Models;

namespace Adn.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    private readonly IUserRepository _user;

    private readonly IConfiguration _config;

    public UserController(ILogger<UserController> logger,
    IUserRepository user, IConfiguration config)
    {
        _logger = logger;
        _user = user;
        _config = config;

    }


    [HttpPost("login")]
    public async Task<ActionResult<UserLoginResDTO>> Login([FromBody] UserLoginDTO Data)
    {
        var existingUser = await _user.GetByUserName(Data.UserName);

        if (existingUser is null)
            return NotFound();

        if (existingUser.Password != Data.Password)
            return BadRequest("Incorrect Password");


        var token = Generate(existingUser);

        var res = new UserLoginResDTO
        {
            Id = existingUser.Id,
            UserName = existingUser.UserName,
            Token = token,
        };
        return Ok(res);

    }

    private string Generate(Users user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(TodoConstants.Id, user.Id.ToString()),

            new Claim(TodoConstants.Username, user.UserName),

        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }



}