namespace MyFlickr.Rest
{
    public class AuthinticationTokens
    {
        public string Token { get; private set; }
        public AccessPermission AccessPermission { get; private set; }
        public string UserID { get; private set; }
        public string UserName { get; private set; }
        public string FullName { get; private set; }

        internal AuthinticationTokens(string token, AccessPermission accessPermission
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
