using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace IntegralInvesting.Models
{
    public class PortfolioViewModel : IModel
    {
        public PortfolioViewModel() => PortfolioStocks = new List<PortfolioStockViewModel>();

        [Key]
        public int PortfolioId { get; set; }

        [Required]
        [MaxLength(200)]
        public string UserId { get; set; } = string.Empty;

        [ValidateNever]
        public ICollection<PortfolioStockViewModel> PortfolioStocks { get; set; }
    }
}
