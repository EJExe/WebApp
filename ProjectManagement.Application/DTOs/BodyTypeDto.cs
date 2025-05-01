using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.DTOs
{
    public class BodyTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateBodyTypeDto
    {
        [Required]
        public int Id { get; set; }
        [Required] public string Name { get; set; }
    }

    public class UpdateBodyTypeDto
    {
        [Required]
        public int Id { get; set; } // Добавляем Id
        [Required]
        public string Name { get; set; }
    }
}
