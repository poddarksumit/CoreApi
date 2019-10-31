using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZipTest.Models.Request
{
    /// <summary>
    /// Request model for getting user.
    /// </summary>
    public class GetUserRequest
    {
        /// <summary>
        /// Gets or Sets emailAddress.
        /// </summary>
        public string EmailAddress { get; set; }
       
        /// <summary>
        /// Gets or Sets id of the user.
        /// </summary>
        public Guid Id { get; set; }
    }
}
