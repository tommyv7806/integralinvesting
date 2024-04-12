using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IntegralInvesting.Models
{
    public class BankAccountViewModel
    {
        [Key]
        public int BankAccountId { get; set; }

        [Required]
        public string UserId { get; set; }

        [DisplayName("Current Funds")]
        public float CurrentFunds { get; set; }

        [Required]
        [MaxLength(60)]
        [DisplayName("Account Name")]
        public string AccountName { get; set; }
    }
}
