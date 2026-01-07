using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Models;


namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/auth")] // مسیر همانطور که خواسته شده 
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context; // دسترسی به دیتابیس 
            _configuration = configuration; // خواندن jwt از appsetting
        }






        [HttpPost("register")] // متد ثبت نام 
        public async Task<IActionResult> Register(RegisterUserDto dto) // هر جایی await هست باید async باشد در اینجا دو بار استفاده شده یکی زمانی که منتظر میماند تا تکراری بودن کاربر چک شود و یکی زمانی که تغییرات میخواهد ثبت شود
        {
            // بررسی تکراری بودن نام کاربری
            var exists = await _context.Users.AnyAsync(u => u.Username == dto.Username);
            if (exists)
                return BadRequest("Username already exists"); // اگر وجود داشته باشد که پیغام مناسب را میدهد 

            var user = new User
            { // اگر کاربر وجود نداشت کاربر جدیدی اضافه میکند 
                Id = Guid.NewGuid(),
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password) // اینجا پسورد را با استفاده از bcrypy هش کردیم 
                // در جایی که کار میکردم  سمت فرانت هش شده برای من فرستاده میشد
            };

            _context.Users.Add(user); //  کاربر را به دیتابیس اضافه میکنیم
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");

        }






        [HttpPost("login")] // متد لاگین
        public async Task<IActionResult> Login(LoginUserDto dto)
        { 
            var user = await _context.Users // منتظر میماند تا کاربر از دیتابیس پیدا شود 
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null) // اگر پیدا نشد یعنی  وجود نداشته یا یوزرنیم اشتباه 
                return Unauthorized("Invalid username");

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash); // اینجا مقدار پسورد را هش میکند و با مقدار داخل دیتابیس مقایسه میکند
            if (!isPasswordValid)
                return Unauthorized("Invalid password"); // اگر اشتباه وارد کند خطا میدهد
            // البته روش درست نباید بگوییم دقیقا یوزرنیم نادرست است یا پسورد
            var token = GenerateJwtToken(user); // تابع ساختن jwt که در پایین نوشته شده این یک توکن است که با همین کابر صحت سنجی میشود 

            return Ok(new
            {
                token = token // توکن را بر میگرداند در درخواست های بعدی با همین توکن کاربر ها از هم تشخیص داده میشوند 
            });
        }





        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(jwtSettings["ExpiryMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }

}
