namespace MyFlickr.Core
{
    public sealed class NoReply
    {
        private NoReply() { }

        public static readonly NoReply Empty = new NoReply();
    }
}
