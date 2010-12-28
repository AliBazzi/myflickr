namespace MyFlickr.Core
{
    /// <summary>
    /// Represents NoReply that is Returned From Flickr when Calling the API Methods using POST Operation
    /// </summary>
    public sealed class NoReply
    {
        private NoReply() { }

        /// <summary>
        /// represents an Instance of the NoReply
        /// </summary>
        public static readonly NoReply Empty = new NoReply();
    }
}
