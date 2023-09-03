using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticatorAPI.Data.Models
{
    public class Token
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Tocken { get; set; }

        [Required]
        public int DataId { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
