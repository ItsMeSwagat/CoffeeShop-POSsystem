using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace courseworkDB.Models
{
    public class PasswordProtectionModel
    {
        [Required]
        public string? Password { get; set; }
    }
}
