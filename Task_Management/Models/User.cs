using System;
using System.Collections.Generic;

namespace TodoApi.Models
{
    public class User
    {
        public Guid Id { get; set; } //شناسه یکتا برای id
        public string Username { get; set; } //یوزرنیم
        public string PasswordHash { get; set; } // پسورد

        // Navigation property
        public ICollection<TodoItem> TodoItems { get; set; } //  هر کاربر میتواند چند تسک داشته باشد بعدا در کوئری کمک میکند 
    } 
}
