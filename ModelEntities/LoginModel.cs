using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelEntities
{
    public class LoginModel
    {
        public Guid CustomerId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Password Required!!")]        
        public string Password { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        
        public string ConfirmPassword { get; set; }
        public bool RememberMe { get; set; }
    }
}
