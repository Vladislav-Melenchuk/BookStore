using System.Collections.Generic;

namespace Project_JS.Models
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public List<Order> Orders { get; set; }
    }
}
