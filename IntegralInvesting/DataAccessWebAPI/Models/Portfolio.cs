using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace DataAccessWebAPI.Models
{
    public class Portfolio
    {
        public Portfolio() => PortfolioStocks = new List<PortfolioStock>();

        [Key]
        public int PortfolioId { get; set; }

        [Required]
        [MaxLength(200)]
        public string UserId { get; set; } = string.Empty;

        [ValidateNever]
        public ICollection<PortfolioStock> PortfolioStocks { get; set; }
    }
}
