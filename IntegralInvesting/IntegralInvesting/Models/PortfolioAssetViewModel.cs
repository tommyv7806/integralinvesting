using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IntegralInvesting.Models
{
    public class PortfolioAssetViewModel
    {
        public PortfolioAssetViewModel()
        {
            PortfolioStocks = new List<PortfolioStockViewModel>();
            LastSevenDaysData = new List<StockTimeDetails>();
        }

        [Key]
        public int PortfolioAssetId { get; set; }

        // Foregin key properties
        public int PortfolioId { get; set; }
        public PortfolioViewModel? Portfolio { get; set; }

        [ValidateNever]
        public ICollection<PortfolioStockViewModel> PortfolioStocks { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Symbol { get; set; } = string.Empty;

        [Column(TypeName = "decimal(8, 2)")]
        public decimal CurrentPrice { get; set; } = 0m;

        [NotMapped]
        public int NumberOfShares { get; set; } = 0;

        [NotMapped]
        public int SellQuantity { get; set; } = 0;

        [NotMapped]
        public decimal SaleTotal { get; set; } = 0;

        [ValidateNever]
        [NotMapped]
        public ICollection<StockTimeDetails> LastSevenDaysData { get; set; }
    }
}
