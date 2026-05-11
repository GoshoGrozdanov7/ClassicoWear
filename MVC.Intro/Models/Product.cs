using MVC.Intro.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC.Intro.Models
{
    public class Product
    {
        [DisplayName("Идентификатор")]
        public Guid Id { get; set; }

        [DisplayName("Наименование")]
        [Required(ErrorMessage = "Полето е задължително")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Дължината на името трябва да е между 3 и 50 символа")]
        [RegularExpression(@"^[A-Za-z\s\-]+$", ErrorMessage = "Името може да съдържа само букви, интервали и тирета")]
        public required string Name { get; set; }

        [DisplayName("Цена")]
        [Required(ErrorMessage = "Задължително е продуктът да има цена")]
        [Range(24.99,9999.99, ErrorMessage = "Цената трябва да е между 24.99 и 9999.99")]
        public decimal Price { get; set; }

        [DisplayName("Цвят")]
        [Required(ErrorMessage = "Задължително е да изберете цвят")]
        [StringLength(30, ErrorMessage = "Дължината на цвета трябва да е максимум 30 символа")]
        [RegularExpression(@"^[A-Za-zА-Яа-я\s\-]+$", ErrorMessage = "Цветът може да съдържа само букви, интервали и тирета")]
        public required string Color { get; set; }

        [DisplayName("Размер")]
        [Required(ErrorMessage = "Задължително е да изберете размер")]
        [StringLength(10, ErrorMessage = "Дължината на размера трябва да е максимум 10 символа")]
        [RegularExpression(@"^(XS|S|M|L|XL|XXL|((3[8-9]|4[0-7])(\.5)?|48))$", ErrorMessage = "Размерът трябва да е XS-XXL или между 38 и 48 през 0.5")]
        public required string Size { get; set; }
    }
}
