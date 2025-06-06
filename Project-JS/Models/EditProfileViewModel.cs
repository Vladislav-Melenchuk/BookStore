using System.ComponentModel.DataAnnotations;

public class EditProfileViewModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    public string Email { get; set; }
}
