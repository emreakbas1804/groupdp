using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace webUi.Models
{
    public class RegisterModel
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }
        
        
    }

    
}
