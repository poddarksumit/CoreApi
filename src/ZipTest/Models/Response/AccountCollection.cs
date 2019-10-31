using System;
using System.Collections.Generic;

namespace ZipTest.Models.Response
{
    /// <summary>
    /// Accounts list
    /// </summary>
    public class AccountCollection
    {
        /// <summary>
        /// Gets or sets the total number of returned accounts.
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// Gets or sets the page number of returned accounts.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the page size of returned accounts.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the page count of returned accounts.
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the collection of results.
        /// </summary>
        public IEnumerable<Account> Accounts { get; set; } = Array.Empty<Account>();
    }
}
