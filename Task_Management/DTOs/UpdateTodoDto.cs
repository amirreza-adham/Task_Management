namespace TodoApi.DTOs
{
    public class UpdateTodoDto // این مواردی که در آپدیت میتواند تغییر کند 
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsDone { get; set; }
    }
}
