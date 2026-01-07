using System;
using System.Collections.Generic;

namespace TodoApi.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        // Navigation property
        public ICollection<TodoItem> TodoItems { get; set; }
    }
}
