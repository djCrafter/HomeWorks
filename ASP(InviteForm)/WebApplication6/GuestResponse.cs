using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication6
{
    public class GuestResponse
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Пожайлуста укажите, придете ли вы")]
        public bool? WillAttend { get; set; }
    }
}