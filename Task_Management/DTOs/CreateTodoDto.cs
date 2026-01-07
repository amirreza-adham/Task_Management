namespace TodoApi.DTOs
{
    public class CreateTodoDto // در اینجا هم نمیخواهیم کلاینت زمان ساخته شدن تسک، انجام شدن یا نشدن و همچنین آیدی را ببیند 
    {
        public string Title { get; set; }
        public string? Description { get; set; }
    }
}
