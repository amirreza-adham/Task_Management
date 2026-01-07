using System;

namespace TodoApi.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsDone { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign key
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
