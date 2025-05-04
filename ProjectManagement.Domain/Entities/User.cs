// Models/User.cs
using Microsoft.AspNetCore.Identity;
using ProjectManagement.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectManagement.Domain.Entities
{
    public class User : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public string UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public override string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public new string Email { get; set; } = null!;



        [Required]
        [MaxLength(20)]
        public string Role { get; set; } // Роль: Client или admin

        //[JsonIgnore]
        //public List<RentalApplication> Applications { get; set; }

        // Навигационные свойства
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}