namespace IntegralInvesting.Models
{
    public class StockSearchViewModel : IModel
    {
        public string Name { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string MarketOpen { get; set; } = string.Empty;
        public string MarketClose { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string MatchScore { get; set; } = string.Empty;
    }
}
