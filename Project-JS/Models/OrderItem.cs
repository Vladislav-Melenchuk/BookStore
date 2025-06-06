namespace Project_JS.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Book Product { get; set; }
        public string Title { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }  
    }
}
