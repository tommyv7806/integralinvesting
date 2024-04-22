using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IntegralInvesting.Models
{
    public class UserFundViewModel : IModel
    {
        [Key]
        public int UserFundId { get; set; }

        [Required]
        [MaxLength(200)]
        public string UserId { get; set; }

        [Range(0, 999999999.99)]
        [DisplayName("Amount To Transfer")]
        public decimal? CurrentTransferAmount { get; set; }

        [DisplayName("Current Funds")]
        public decimal CurrentFunds { get; set; }

        [MaxLength(200)]
        public string? CurrentTransferAccount { get; set; }
    }
}
