using System.ComponentModel.DataAnnotations;

public class OrderFormViewModel
{
    [Required(ErrorMessage = "Вкажіть ваше ПІБ")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Адреса обов'язкова")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Телефон обов'язковий")]
    [RegularExpression(@"^\+?[0-9\s\-]{7,15}$", ErrorMessage = "Некоректний номер телефону")]
    public string PhoneNumber { get; set; }

    [EmailAddress(ErrorMessage = "Некоректна електронна адреса")]
    public string Email { get; set; }

    public string AdditionalNotes { get; set; }
}
