using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntegralInvesting.Models
{
    public class UserFundViewModel
    {
        [Key]
        public int UserFundId { get; set; }

        [Required]
        [MaxLength(200)]
        public string UserId { get; set; }

        public float CurrentFunds { get; set; }
    }
}
