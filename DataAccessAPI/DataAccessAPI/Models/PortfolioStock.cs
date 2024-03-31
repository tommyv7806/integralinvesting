namespace DataAccessAPI.Models
{
    public class PortfolioStock
    {
        public int PortfolioStockId { get; set; }
        public int PortfolioId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public float CurrentPrice { get; set; }
        public float PurchasePrice { get; set; }
        public int NumberOfShares { get; set; }
        public float TotalAmount => CurrentPrice * NumberOfShares;
        

        public PortfolioStock(int portfolioStockId, int portfolioId, string name, string symbol, float currentPrice, float purchasePrice, int numberOfShares)
        {
            PortfolioStockId = portfolioStockId;
            PortfolioId = portfolioId;
            Name = name;
            Symbol = symbol;
            CurrentPrice = currentPrice;
            PurchasePrice = purchasePrice;
            NumberOfShares = numberOfShares;
        }

        public static List<PortfolioStock> GetPortfolioStocks() => new List<PortfolioStock>
        {
            new PortfolioStock(1111, 2222, "Apple Inc", "AAPL", 171.48f, 164.33f, 12),
            new PortfolioStock(3333, 4444, "Alphabet Inc", "GOOG", 152.26f, 131.87f, 4),
            new PortfolioStock(5555, 6666, "Microsoft Corp", "MSFT", 420.72f, 243.62f, 2)
        };
    }
}
