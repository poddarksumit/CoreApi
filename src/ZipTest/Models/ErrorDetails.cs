using Newtonsoft.Json;
using System.Net;

namespace ZipTest.Models
{
    /// <summary>
    /// Model for errors.
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// Gets or Sets the status code of the error.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or Sets the message of the error.
        /// </summary>
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
