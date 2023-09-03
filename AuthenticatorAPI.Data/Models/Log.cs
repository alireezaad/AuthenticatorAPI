using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticatorAPI.Data.Models
{
    public class Log
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string RemoteOne { get; set; }

        [Required]
        public string RemoteTwo { get; set; }

        [Required]
        public int RemoteType { get; set; }

        [Required]
        public int DataId { get; set; }

        [Required]
        public int DataType { get; set; }

        [Required]
        public DateTime Date { get; set; }

    }
}
