using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using MailKit.Net.Smtp;
using SalesProject.Data;
using SalesProject.Models.DTOs;
using SalesProject.Models.Domain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
//using Microsoft.AspNetCore.Identity.Data;
namespace SalesProject.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _config;
    private readonly SalesDbContext _context;

    public AuthController(UserManager<IdentityUser> userManager, IConfiguration config, SalesDbContext context   )
    {
        _userManager = userManager;
        _config = config;
        _context = context;
    }

    //[HttpPost("register/{email}/{password}")]
    //public async Task<IActionResult> Register(string email, string password)
    //{
    //    var userExists = await _userManager.FindByEmailAsync(email);
    //    if (userExists != null)
    //        return BadRequest("User already exists.");

    //    IdentityUser newUser = new IdentityUser()
    //    {
    //        UserName = email,
    //        Email = email
    //    };

    //    var result = await _userManager.CreateAsync(newUser, password);
    //    if (!result.Succeeded)
    //        return BadRequest(result.Errors);

    //    var emailCode = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
    //    await SendEmailAsync(email, emailCode);

    //    return Ok("Thanks for your registration, kindly check your email for confirmation code.");
    //}
    private string GenerateConfirmationCode()
    {
        Random random = new Random();
        return random.Next(100000, 999999).ToString(); // Tạo mã 6 số
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        // Kiểm tra xem vai trò có hợp lệ không
        if (request.Role != UserRole.Customer && request.Role != UserRole.Admin)
        {
            return BadRequest("Invalid role selected.");
        }

        var userExists = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (userExists != null)
            return BadRequest("User already exists.");

        // ✅ Tạo mã xác nhận 6 số
        var emailConfirmationCode = GenerateConfirmationCode();

        var newUser = new Users
        {
            Email = request.Email,
            FullName = request.FullName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password), // ✅ Hash mật khẩu
            Role = request.Role, // ✅ Lưu vai trò người dùng
            IsActive = false,
            EmailConfirmationCode = emailConfirmationCode
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        // ✅ Gửi mã xác nhận qua email
        await SendEmailAsync(request.Email, emailConfirmationCode);

        return Ok("Registration successful. Please check your email to confirm your account.");
    }


    private async Task SendEmailAsync(string email, string emailCode)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse("jay.schroeder29@ethereal.email"));
        message.To.Add(MailboxAddress.Parse("jay.schroeder29@ethereal.email"));
        message.Subject = "Xác nhận Email";

        var emailBody = new BodyBuilder
        {
            HtmlBody = $@"
                <html>
                <body>
                    <p>Dear {email},</p>
                    <p>Thank you for registering with us. Please verify your email address using the code below:</p>
                    <h2>{emailCode}</h2>
                    <p>If you did not request this, please ignore this email.</p>
                    <br>
                    <p>Best regards,</p>
                    <p><strong>Netcode-Hub</strong></p>
                </body>
                </html>"
        };

        message.Body = emailBody.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.ethereal.email", 587, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync("jay.schroeder29@ethereal.email", "Z6pRG8HkHmJJ2wjXYd");
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    //[HttpPost("confirmation/{email}/{code}")]
    //public async Task<IActionResult> Confirmation(string email, string code)
    //{
    //    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(code))
    //        return BadRequest("Invalid parameters provided.");

    //    var user = await _userManager.FindByEmailAsync(email);
    //    if (user == null)
    //        return BadRequest("Invalid email provided.");

    //    var result = await _userManager.ConfirmEmailAsync(user, code);
    //    if (!result.Succeeded)
    //        return BadRequest("Invalid confirmation code.");

    //    return Ok("Email confirmed successfully. You can now login.");
    //}



    [HttpPost("confirmation/{email}/{code}")]
    public async Task<IActionResult> Confirmation(string email, string code)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(code))
            return BadRequest("Invalid parameters provided.");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            return BadRequest("Invalid email provided.");

        if (user.EmailConfirmationCode != code)
            return BadRequest("Invalid confirmation code.");

        user.IsActive = true;
        user.EmailConfirmationCode = null;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return Ok("Email confirmed successfully. You can now login.");
    }

    //[HttpPost("Login/{email}/{password}")]
    //public async Task<IActionResult> Login(string email, string password)
    //{
    //    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
    //    {
    //        return BadRequest();

    //    }

    //    var user = await _userManager.FindByEmailAsync(email);
    //    bool isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user!);
    //    if (!isEmailConfirmed)
    //    {
    //        return BadRequest($"Email: {email} is not confirmed yet");   
    //    }

    //    return Ok(new[] { "Login sucessful", GenerateToken(user) });


    //}

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return BadRequest("Invalid email or password.");

        if (!user.IsActive)
            return BadRequest("Account is not confirmed yet. Please check your email.");

        var token = GenerateJwt(user);

        return Ok(new
        {
            Message = "Login successful",
            Token = token,
            Role = user.Role.ToString()
        });
    }


    [HttpPost("signin-google")]
    public async Task<IActionResult> GoogleLogin(GoogleLoginRequest request)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.Credential);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.GoogleId == payload.Subject || u.Email == payload.Email);

            if (user == null)
            {
                user = new Users
                {
                    GoogleId = payload.Subject,
                    Email = payload.Email,
                    FullName = payload.Name,
                    AvatarUrl = payload.Picture,
                    IsActive = true,
                    Role = UserRole.Customer
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            else if (string.IsNullOrEmpty(user.GoogleId))
            {
                user.GoogleId = payload.Subject;
                user.AvatarUrl ??= payload.Picture;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }

            var token = GenerateJwt(user);

            return Ok(new LoginResponse
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName
            });
        }
        catch (InvalidJwtException ex)
        {
            return BadRequest(new { message = "Invalid Google Credential", error = ex.Message });
        }
    }

    //private string GenerateJwt(Users user)
    //{
    //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
    //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    //    var claims = new List<Claim>
    //    {
    //        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
    //        new(ClaimTypes.Email, user.Email),
    //        new(ClaimTypes.Name, user.FullName),
    //        new(ClaimTypes.Role, user.Role.ToString())
    //    };

    //    var token = new JwtSecurityToken(
    //        issuer: _config["Jwt:Issuer"],
    //        audience: _config["Jwt:Audience"],
    //        claims: claims,
    //        expires: DateTime.UtcNow.AddDays(7),
    //        signingCredentials: credentials
    //    );

    //    return new JwtSecurityTokenHandler().WriteToken(token);
    //}
    //private string GenerateToken(IdentityUser? user)
    //{

    //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
    //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


    //    var claims = new []
    //    {
    //      new Claim(JwtRegisteredClaimNames.Sub ,user!.Id),
    //        new Claim(JwtRegisteredClaimNames.Email ,user!.Email!),

    //    };

    //    var token = new JwtSecurityToken(
    //        issuer: _config["Jwt:Issuer"],
    //        audience: _config["Jwt:Audience"],
    //        claims: claims,
    //        expires: DateTime.UtcNow.AddDays(7),
    //        signingCredentials: credentials
    //    );

    //    return new JwtSecurityTokenHandler().WriteToken(token);
    //}

    private string GenerateJwt(Users user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
    {
        //new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //new(ClaimTypes.Email, user.Email),
        //new(ClaimTypes.Name, user.FullName),
        //new(ClaimTypes.Role, user.Role.ToString()) // ✅ Thêm Role vào claims
          new("id", user.Id.ToString()),            
        new("email", user.Email),               
        new("fullName", user.FullName),          
        new("role", user.Role.ToString())
    };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
