namespace DataAccessAPI.Models
{
    public class Portfolio
    {
        public int PortfolioId { get; set; }
        public int UserAccountId { get; set; }
        
        public Portfolio(int portfolioId, int userAccountId)
        {
            PortfolioId = portfolioId;
            UserAccountId = userAccountId;
        }

        public static List<Portfolio> GetPortfolios() => new List<Portfolio>
        {
            new Portfolio(1234, 5678),
            new Portfolio(1111, 2222),
            new Portfolio(3333, 4444),
            new Portfolio(5555, 6666)
        };
    }
}
