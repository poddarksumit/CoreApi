using System;

namespace ZipTest.Models.Response
{
    /// <summary>
    /// Base class for user details.
    /// </summary>
    public class UserBase
    {
        /// <summary>
        /// Gets or sets the id of the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string EmailAddress { get; set; }
    }
}
