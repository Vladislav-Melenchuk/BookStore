﻿namespace Project_JS.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public string? Email { get; set; }

    }
}
