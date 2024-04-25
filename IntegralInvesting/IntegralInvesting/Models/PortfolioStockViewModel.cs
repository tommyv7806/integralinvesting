using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IntegralInvesting.Models
{
    public class PortfolioStockViewModel : IModel
    {
        [Key]
        public int PortfolioStockId { get; set; }

        // Foregin key properties
        //public int PortfolioId { get; set; }
        //public PortfolioViewModel? Portfolio { get; set; }

        public int PortfolioAssetId { get; set; }
        public PortfolioAssetViewModel? PortfolioAsset { get; set; }

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

        [Column(TypeName = "decimal(8, 2)")]
        public decimal PurchaseTotal { get; set; } = 0m;
        public int PurchaseQuantity { get; set; } = 0;
    }
}
