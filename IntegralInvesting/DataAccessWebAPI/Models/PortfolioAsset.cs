using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessWebAPI.Models
{
    public class PortfolioAsset
    {
        public PortfolioAsset() => PortfolioStocks = new List<PortfolioStock>();

        [Key]
        public int PortfolioAssetId { get; set; }

        // Foregin key properties
        public int PortfolioId { get; set; }
        public Portfolio? Portfolio { get; set; }

        [ValidateNever]
        public ICollection<PortfolioStock> PortfolioStocks { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Symbol { get; set; } = string.Empty;

        [Column(TypeName = "decimal(8, 2)")]
        public decimal CurrentPrice { get; set; } = 0m;

        public int NumberOfShares { get; set; } = 0;
    }
}
