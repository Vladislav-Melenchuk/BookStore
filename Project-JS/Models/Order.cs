using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project_JS.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<OrderItem> Items { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }  

        
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        [Display(Name = "Додаткові побажання")]
        public string? AdditionalNotes { get; set; }
    }
}
