using System;

namespace ZipTest.Db.Model
{
    /// <summary>
    /// Represents DB model for Users
    /// </summary>
    public class Users
    {
        /// <summary>
        /// Gets or sets the UserId.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the monthly salary of the user.
        /// </summary>
        public double MonthlySalary { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public double MonthlyExpenses { get; set; }

        /// <summary>
        /// Gets or sets the last modified datetime.
        /// </summary>
        public DateTime LastModifiedDt { get; set; }

        /// <summary>
        /// Gets or sets the created datetime.
        /// </summary>
        public DateTime CreatedDt { get; set; }

        /// <summary>
        /// User associated to this account.
        /// </summary>
        public virtual Accounts Account { get; set; }

    }
}
