using System;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents the Method that exist in flickr.interestingness namespace.
    /// </summary>
    public class Interestingness
    {
        private readonly AuthenticationTokens authtkns;

        /// <summary>
        /// Create Interestingness Object.
        /// </summary>
        /// <param name="authenticationTokens">Authentication Tokens Object.</param>
        public Interestingness(AuthenticationTokens authenticationTokens)
        {
            if (authenticationTokens == null)
                throw new ArgumentNullException("authenticationTokens");
            this.authtkns = authenticationTokens;
        }

        /// <summary>
        /// Returns the list of interesting photos for the most recent day or a user-specified date.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="date">A specific date, formatted as YYYY-MM-DD, to return interesting photos for.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_z, url_l, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetListAsync(string date = null, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetListCompletedEvent(new EventArgs<PhotosCollection>(token, new PhotosCollection(this.authtkns, elm.Element("photos")))),
                e => this.InvokeGetListCompletedEvent(new EventArgs<PhotosCollection>(token, e)), this.authtkns.SharedSecret,
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token),
                new Parameter("method", "flickr.interestingness.getList"), new Parameter("date", date), new Parameter("extras", extras), 
                new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        private void InvokeGetListCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetListCompleted != null)
            {
                this.GetListCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetListAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PhotosCollection>> GetListCompleted;
    }
}
