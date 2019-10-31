using System;
using System.Collections.Generic;

namespace ZipTest.Models.Response
{
    /// <summary>
    /// Users list
    /// </summary>
    public class UserCollection
    {
        /// <summary>
        /// Gets or sets the total number of returned users.
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// Gets or sets the page number of returned users.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the page size of returned users.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the page count of returned users.
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the collection of results.
        /// </summary>
        public IEnumerable<UserBase> Users { get; set; } = Array.Empty<UserBase>();
    }
}
