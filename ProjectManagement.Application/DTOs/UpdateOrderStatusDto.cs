﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.DTOs
{
    public class UpdateOrderStatusDto
    {
        //[Required]
        //public int OrderId { get; set; }
        public string Status { get; set; }
    }
}
