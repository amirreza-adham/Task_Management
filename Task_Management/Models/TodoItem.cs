using System;

namespace TodoApi.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; } // شناسه یکتا برای هر تسک
        public string Title { get; set; } //عنوان 
        public string? Description { get; set; } // توضیحات میتواند خالی باشد 
        public bool IsDone { get; set; } // انجام شده یا خیر
        public DateTime CreatedAt { get; set; } // زمان ساخته شدن این تسک

        // کلید خارجی 
        public Guid UserId { get; set; } // مربوط به کدام یوزر است 
        public User User { get; set; } // اگر بخواهیم اطلاعات کاربر را با هر تسک بعدا کامل برگردانیم همان بحث جوین 
    }
}
