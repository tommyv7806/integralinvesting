using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessWebAPI.Models
{
    public class UserFund
    {
        [Key]
        public int UserFundId { get; set; }

        [Required]
        [MaxLength(200)]
        public string UserId { get; set; } = string.Empty;

        [Column(TypeName = "decimal(8, 2)")]
        public float CurrentFunds { get; set; } = 0;
    }
}
