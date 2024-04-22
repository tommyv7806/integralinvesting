using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessWebAPI.Models
{
    public class PortfolioStock
    {
        [Key]
        public int PortfolioStockId { get; set; }

        // Foregin key properties
        public int PortfolioId { get; set; }
        public Portfolio? Portfolio { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Symbol { get; set; } = string.Empty;

        [Column(TypeName = "decimal(8, 2)")]
        public decimal CurrentPrice { get; set; } = 0m;

        [Column(TypeName = "decimal(8, 2)")]
        public decimal PurchasePrice { get; set; } = 0m;

        public int PurchaseQuantity { get; set; } = 0;
    }
}
