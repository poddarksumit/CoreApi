using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZipTest.Db.Model
{
    /// <summary>
    /// Represents DB model for Accounts
    /// </summary>
    public class Accounts
    {
        /// <summary>
        /// Gets or sets the AccountId.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Gets or sets the reference id of User.
        /// </summary>
        public int AccountUserId { get; set; }

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
        public virtual Users User { get; set; }

    }
}
