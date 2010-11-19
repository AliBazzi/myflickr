using System;
namespace MyFlickr.Rest
{
    /// <summary>
    /// represents the Permissions and User information that are Granted during authentication
    /// </summary>
    public class AuthenticationTokens
    {
        /// <summary>
        /// Flickr API application key
        /// </summary>
        public string ApiKey { get; private set; }

        /// <summary>
        /// A shared secret for the api key that is issued by flickr
        /// </summary>
        public string SharedSecret { get; private set; }

        /// <summary>
        /// the Authentication Token
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// The Access Permission
        /// </summary>
        public AccessPermission AccessPermission { get; private set; }

        /// <summary>
        /// The User ID
        /// </summary>
        public string UserID { get; private set; }

        /// <summary>
        /// the UserName
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// the Full Name of the User
        /// </summary>
        public string FullName { get; private set; }

        internal AuthenticationTokens(string apiKey , string sharedSecret,string token, AccessPermission accessPermission
            , string userID, string userName, string fullName)
        {
            this.AccessPermission = accessPermission;
            this.Token = token;
            this.FullName = fullName;
            this.UserID = userID;
            this.UserName = userName;
            this.ApiKey = apiKey;
            this.SharedSecret = sharedSecret;
        }
    }

    public static class AuthenticationTokensExtensions
    {
        public static User CreateUserInstance(this AuthenticationTokens authinticationTokens)
        {
            return new User(authinticationTokens);
        }

        internal static void ValidateGrantedPermission(this AuthenticationTokens authenticationTokens, AccessPermission accessPermission)
        {
            if (authenticationTokens.AccessPermission < accessPermission)
            {
                throw new InvalidOperationException(string.Format("The Called method needs {0} permission and you have {1} permission"
                    , accessPermission.ToString() , authenticationTokens.AccessPermission.ToString()));
            }
        }
    }
}
