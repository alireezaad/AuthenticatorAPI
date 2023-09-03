using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticatorAPI.Data.Models
{
    public class Customer
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int LockId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Password { get; set; }

        public bool IsBlock { get; set; }

        [Required]
        public int LastRemoteId { get; set; }
    }
}
