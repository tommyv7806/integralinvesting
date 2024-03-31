namespace DataAccessAPI.Models
{
    public class BankAccount
    {
        public int BankAccountId { get; set; }
        public int UserAccountId { get; set; }
        public float CurrentFunds { get; set; }
        public string AccountName { get; set; }

        public BankAccount(int bankAccountId, int userAccountId, float currentFunds, string accountName)
        {
            UserAccountId = userAccountId;
            BankAccountId = bankAccountId;
            CurrentFunds = currentFunds;
            AccountName = accountName;
        }

        public static List<BankAccount> GetBankAccounts() => new List<BankAccount>
        {
            new BankAccount(1234, 2222, 1621.90f, "Wells Fargo"),
            new BankAccount(5678, 3333, 902.72f, "Citi Bank"),
            new BankAccount(9101, 4444, 2345.67f, "Bank of America")
        };
    }
}
