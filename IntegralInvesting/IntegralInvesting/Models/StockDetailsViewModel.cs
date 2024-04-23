namespace IntegralInvesting.Models
{
    public class StockDetailsViewModel : IModel
    {
        public string Symbol { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Price { get; set; }
        public int Volume { get; set; }
        public decimal PreviousClose { get; set; }

        public List<StockTimeDetails> StockTimeDetailsList { get; set; } = new List<StockTimeDetails>();
    }
}
