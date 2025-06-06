using System.ComponentModel.DataAnnotations;

namespace Project_JS.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Ім’я користувача є обов’язковим")]
        [Display(Name = "Ім’я користувача")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Пароль є обов’язковим")]
        [MinLength(8, ErrorMessage = "Пароль має містити щонайменше 8 символів")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запам’ятати мене")]
        public bool RememberMe { get; set; }
    }
}
