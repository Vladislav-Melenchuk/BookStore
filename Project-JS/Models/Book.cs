using System.ComponentModel.DataAnnotations;

namespace Project_JS.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва обов'язкова")]
        [Display(Name = "Назва")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Автор обов'язковий")]
        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Ціна обов'язкова")]
        [Range(1, 9999.99, ErrorMessage = "Ціна має бути в діапазоні від 1 до 9999")]
        [Display(Name = "Ціна (грн)")]
        public decimal Price { get; set; }

        [Display(Name = "Категорія")]
        public string Category { get; set; }

        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Display(Name = "Зображення (шлях)")]
        public string ImagePath { get; set; }
    }
}
