using System;

namespace ZipTest.Models.Response
{
    /// <summary>
    /// The model for user information.
    /// </summary>
    public class User : UserBase
    {
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
