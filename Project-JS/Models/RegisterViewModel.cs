using System.ComponentModel.DataAnnotations;

namespace Project_JS.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ім’я користувача є обов’язковим")]
        [MinLength(3, ErrorMessage = "Ім’я користувача має містити щонайменше 3 символи")]
        [RegularExpression(@"^[a-zA-Z0-9_-]+$", ErrorMessage = "Ім’я користувача може містити лише літери, цифри, тире та підкреслення")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Номер телефону є обов’язковим")]
        [RegularExpression(@"^\+380\d{9}$", ErrorMessage = "Невірний формат номера (має бути +380XXXXXXXXX)")]
        public string PhoneNumber { get; set; }

    

        [Required(ErrorMessage = "Пароль є обов’язковим")]
        [MinLength(8, ErrorMessage = "Пароль має містити щонайменше 8 символів")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
    }
}
