using System.ComponentModel.DataAnnotations;

namespace DataAccessWebAPI.Models
{
    public class BankAccount
    {
        [Key]
        public int BankAccountId { get; set; }

        [Required]
        public string UserId { get; set; }

        public float CurrentFunds { get; set; }

        [Required]
        [MaxLength(60)]
        public string AccountName { get; set; }
    }
}
