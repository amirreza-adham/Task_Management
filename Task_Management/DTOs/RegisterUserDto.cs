namespace TodoApi.DTOs
{
    public class RegisterUserDto //dto برای ثبت نام 
    { // واسطی بین کلاینت و مدل ها برای اینکه جزئیات مدل ها پنهان بماند 
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
