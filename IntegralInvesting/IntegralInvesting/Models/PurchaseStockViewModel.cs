﻿namespace IntegralInvesting.Models
{
    public class PurchaseStockViewModel
    {
        public DateTime Timestamp { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
        public string TestString { get; set; } = string.Empty;
    }
}
