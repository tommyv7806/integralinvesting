using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntegralInvesting.Models
{
    public class BankAccountViewModel : IModel
    {
        [Key]
        public int BankAccountId { get; set; }

        [Required]
        public string UserId { get; set; }

        [DisplayName("Current Funds")]
        public decimal CurrentFunds { get; set; }

        [Required]
        [MaxLength(60)]
        [DisplayName("Account Name")]
        public string AccountName { get; set; }
    }
}
