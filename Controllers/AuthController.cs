using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/auth")]
public class AuthController : Controller{
    private readonly AppDbContext _context;//đọc dữ liệu hay ghi dữ liệu thông qua DbContext
    private readonly IConfiguration _config;//đọc dữ liệu trong file appsettings.json

    public AuthController(AppDbContext context, IConfiguration config){
        _config = config;
        _context = context;

    }
    //API đăng nhập
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request){
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        String hashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);//c1
        if(user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return Unauthorized();
        
        //Tạo token cho phiên đăng nhặp
        var token = GenerateJwtToken(user);
        
        //Trả về thông tin người dùng và token
        return Ok(new { Token=token, User = user});
    }

    //phương thức tạo token
    private String GenerateJwtToken(User user){
        var jwtKey = _config["JwtKey"] ?? throw new InvalidOperationException("JwtKey chưa đươc cấu hình");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new []{
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer : _config["Jwt:Issuer"],
            audience : _config["Jwt:Audience"],
            claims: claims,
            expires : DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ExprityInMinutes"])),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginRequest{
    public String Username { get; set; }
    public String Password { get; set; }
}