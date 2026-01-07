using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/todos")]
    [Authorize] // فقط کاربران لاگین‌شده
    public class TodosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TodosController(AppDbContext context)
        {
            _context = context; // دسترسی به دیتابیس 
        }
        private Guid GetUserId() // userid را از روی توکن پیدا میکند در تمامی  متد ها این تابع استفاده میشود 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            return Guid.Parse(userId);
        }


        [HttpPost]
        public async Task<IActionResult> Todos(CreateTodoDto dto) // اینجا تسک جدید میسازیم
        {
            var userId = GetUserId();

            var todo = new TodoItem
            {    //دیتا ها را وارد میکنیم 
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                IsDone = false,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };

            _context.TodoItems.Add(todo); //به دیتابیس اضافه میکنیم 
            await _context.SaveChangesAsync();

            return Ok(todo);
        }


        [HttpGet]  // این متد تمام تسک هارا بر اساس زمان مرتب سازی میکند
        public async Task<IActionResult> Todos()
        {
            var userId = GetUserId();

            var todos = await _context.TodoItems
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return Ok(todos);
        }

    }
}
