// Models/Review.cs
using ProjectManagement.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectManagement.Domain.Entities
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // Оценка от 1 до 5

        [MaxLength(500)]
        public string Comment { get; set; } // Текст отзыва

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Дата создания

        // Внешние ключи
        public int ApplicationId { get; set; }
        public int OrderId { get; set; } // Связь с заказом
        public int UserId { get; set; } // Связь с пользователем

        //[JsonIgnore]
        //public RentalApplication Application { get; set; }

        // Навигационные свойства
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}