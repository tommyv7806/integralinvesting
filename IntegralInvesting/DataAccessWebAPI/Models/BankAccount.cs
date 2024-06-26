﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessWebAPI.Models
{
    public class BankAccount
    {
        [Key]
        public int BankAccountId { get; set; }

        [Required]
        [MaxLength(200)]
        public string UserId { get; set; }

        [Required]
        [MaxLength(60)]
        public string AccountName { get; set; }
    }
}
