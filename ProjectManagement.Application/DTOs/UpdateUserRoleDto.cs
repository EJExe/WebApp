﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.DTOs
{
    public class UpdateUserRoleDto
    {
        //[Required]
        //public string UserId { get; set; }
        public string Role { get; set; }
    }
}
