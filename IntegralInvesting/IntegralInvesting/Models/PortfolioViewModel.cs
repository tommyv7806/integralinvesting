using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace IntegralInvesting.Models
{
    public class PortfolioViewModel : IModel
    {
        public PortfolioViewModel() => PortfolioAssets = new List<PortfolioAssetViewModel>();

        [Key]
        public int PortfolioId { get; set; }

        [Required]
        [MaxLength(200)]
        public string UserId { get; set; } = string.Empty;

        [ValidateNever]
        public ICollection<PortfolioAssetViewModel> PortfolioAssets { get; set; }
    }
}
