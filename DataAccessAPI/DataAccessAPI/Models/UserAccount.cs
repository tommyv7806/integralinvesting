namespace DataAccessAPI.Models
{
    public class UserAccount
    {
        public int UserAccountId { get; set; }
        public string UserName { get; set; }
        public float CurrentFunds { get; set; }

        public UserAccount(int userAccountId, string userName, float currentFunds)
        {
            UserAccountId = userAccountId;
            UserName = userName;
            CurrentFunds = currentFunds;
        }

        public static List<UserAccount> GetUserAccounts() => new List<UserAccount>
        {
            new UserAccount(1234, "userName01", 504.37f),
            new UserAccount(5678, "userName02", 783.23f)
        };
    }
}
