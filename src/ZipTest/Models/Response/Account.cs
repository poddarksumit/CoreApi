namespace ZipTest.Models.Response
{
    /// <summary>
    /// The model for account details.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Gets or sets the AccountId.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// User associated to this account.
        /// </summary>
        public User User { get; set; }
    }
}
