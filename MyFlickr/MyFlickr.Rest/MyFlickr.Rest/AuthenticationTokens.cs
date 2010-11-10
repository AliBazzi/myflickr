namespace MyFlickr.Rest
{
    public class AuthenticationTokens
    {
        public string Token { get; private set; }
        public AccessPermission AccessPermission { get; private set; }
        public string UserID { get; private set; }
        public string UserName { get; private set; }
        public string FullName { get; private set; }

        internal AuthenticationTokens(string token, AccessPermission accessPermission
            , string userID, string userName, string fullName)
        {
            this.AccessPermission = accessPermission;
            this.Token = token;
            this.FullName = fullName;
            this.UserID = userID;
            this.UserName = userName;
        }
    }
}
