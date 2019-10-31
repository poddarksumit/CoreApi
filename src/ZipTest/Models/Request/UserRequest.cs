using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZipTest.Models.Request
{
    /// <summary>
    /// Request class for adding user
    /// </summary>
    public class UserRequest
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// Gets or sets the monthly salary of the user.
        /// </summary>
        public double MonthlySalary { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public double MonthlyExpenses { get; set; }
    }
}
