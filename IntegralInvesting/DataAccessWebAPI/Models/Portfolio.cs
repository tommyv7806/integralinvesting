using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace DataAccessWebAPI.Models
{
    public class Portfolio
    {
        public Portfolio() => PortfolioAssets = new List<PortfolioAsset>();

        [Key]
        public int PortfolioId { get; set; }

        [Required]
        [MaxLength(200)]
        public string UserId { get; set; } = string.Empty;

        [ValidateNever]
        public ICollection<PortfolioAsset> PortfolioAssets { get; set; }
    }
}
