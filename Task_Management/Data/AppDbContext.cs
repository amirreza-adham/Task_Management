using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data
{
    public class AppDbContext : DbContext // کلاس مدیریت ارتباط با دیتابیس ، اجرای کوئری ها 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) // تابع کانستراکتور
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }
        // هر کدام از دیتاست های بالا نماینده آن جدول در دیتابیس است

        protected override void OnModelCreating(ModelBuilder modelBuilder) // پیکربندی جداول در زمان ساخته شدن هر رکورد
        {
            base.OnModelCreating(modelBuilder); // انجام تنظیمات پیش فرض 

           
            modelBuilder.Entity<User>() // برای یوزر ها بررسی میکند  که هیچ دو نام کاربری شبیه هم نباشد 
                        .HasIndex(u => u.Username)
                        .IsUnique();


            modelBuilder.Entity<TodoItem>() // اینجا بررسی میشود که طول عنوان حداکثر 100 کارکتر باشد و حتما باید این فیلد وارد شود 
                        .Property(t => t.Title)
                        .HasMaxLength(100)
                        .IsRequired();
        }
    }
}
