namespace ZipTest.Models
{
    /// <summary>
    /// A reuqest model for getting list of results
    /// </summary>
    public class GetCollectionRequest
    {
        /// <summary>
        /// Gets or sets the page number for gettings results. Defaults to 1.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or sets number of results per page. Defaults to 10.
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
