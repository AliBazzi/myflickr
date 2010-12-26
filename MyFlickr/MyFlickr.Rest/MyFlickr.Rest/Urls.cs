using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents the Methods that exist in flickr.urls namespace
    /// </summary>
    public class Urls
    {
        private readonly AuthenticationTokens authtkns;

        /// <summary>
        /// Create Urls object
        /// </summary>
        /// <param name="authenticationTokens">authentication Tokens object</param>
        public Urls(AuthenticationTokens authenticationTokens)
        {
            if (authenticationTokens == null )
		        throw new ArgumentNullException("authenticationTokens");
            this.authtkns = authenticationTokens;
        }

        /// <summary>
        /// Returns the url to a group's page.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="groupID">The NSID of the group to fetch the url for.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetGroupAsync(string groupID)
        {
            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetGroupCompletedEvent(new EventArgs<Uri>(token,new Uri(elm.Element("group").Attribute("url").Value))), 
                e => this.InvokeGetGroupCompletedEvent(new EventArgs<Uri>(token,e)), this.authtkns.SharedSecret,
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token), 
                new Parameter("method", "flickr.urls.getGroup"), new Parameter("group_id", groupID));

            return token;
        }

        /// <summary>
        /// Returns the url to a user's photos.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="userID">The NSID of the user to fetch the url for. If omitted, the calling user is assumed.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetUserPhotosAsync(string userID = null)
        {
            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetUserPhotosCompletedEvent(new EventArgs<Uri>(token, new Uri(elm.Element("user").Attribute("url").Value))),
                e => this.InvokeGetUserPhotosCompletedEvent(new EventArgs<Uri>(token, e)), this.authtkns.SharedSecret,
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token),
                new Parameter("method", "flickr.urls.getUserPhotos"), new Parameter("user_id", userID));

            return token;
        }

        /// <summary>
        /// Returns the url to a user's profile.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="userID">The NSID of the user to fetch the url for. If omitted, the calling user is assumed.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetUserProfileAsync(string userID = null)
        {
            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetUserProfileCompletedEvent(new EventArgs<Uri>(token, new Uri(elm.Element("user").Attribute("url").Value))),
                e => this.InvokeGetUserProfileCompletedEvent(new EventArgs<Uri>(token, e)), this.authtkns.SharedSecret,
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token),
                new Parameter("method", "flickr.urls.getUserProfile"), new Parameter("user_id", userID));

            return token;
        }

        /// <summary>
        /// Returns gallery , by url.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="url">The gallery's URL.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token LookupGalleryAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("url");

            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeLookupGalleryCompletedEvent(new EventArgs<Gallery>(token, new Gallery(this.authtkns,elm.Element("gallery")))),
                e => this.InvokeLookupGalleryCompletedEvent(new EventArgs<Gallery>(token, e)), this.authtkns.SharedSecret,
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token),
                new Parameter("method", "flickr.urls.lookupGallery"), new Parameter("url", url));

            return token;
        }

        /// <summary>
        /// Returns a group NSID, given the url to a group's page or photo pool.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="url">The url to the group's page or photo pool.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token LookupGroupAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("url");

            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeLookupGroupCompletedEvent(new EventArgs<Tuple<string, string>>
                    (token, new Tuple<string, string>(elm.Element("group").Attribute("id").Value,elm.Element("group").Element("groupname").Value))),
                e => this.InvokeLookupGroupCompletedEvent(new EventArgs<Tuple<string,string>>(token, e)), this.authtkns.SharedSecret,
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token),
                new Parameter("method", "flickr.urls.lookupGroup"), new Parameter("url", url));

            return token;
        }

        /// <summary>
        /// Returns a user NSID, given the url to a user's photos or profile.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="url">The url to the user's profile or photos page.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token LookupUserAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("url");

            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeLookupUserCompletedEvent(new EventArgs<Tuple<string, string>>
                    (token, new Tuple<string, string>(elm.Element("user").Attribute("id").Value, elm.Element("user").Element("username").Value))),
                e => this.InvokeLookupUserCompletedEvent(new EventArgs<Tuple<string, string>>(token, e)), this.authtkns.SharedSecret,
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token),
                new Parameter("method", "flickr.urls.lookupUser"), new Parameter("url", url));

            return token;
        }

        #region Events
        private void InvokeLookupUserCompletedEvent(EventArgs<Tuple<string,string>> args)
        {
            if (this.LookupUserCompleted != null)
            {
                this.LookupUserCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<Tuple<string,string>>> LookupUserCompleted;
        private void InvokeLookupGroupCompletedEvent(EventArgs<Tuple<string,string>> args)
        {
            if (this.LookupGroupCompleted != null)
            {
                this.LookupGroupCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<Tuple<string,string>>> LookupGroupCompleted;
        private void InvokeLookupGalleryCompletedEvent(EventArgs<Gallery> args)
        {
            if (this.LookupGalleryCompleted != null)
            {
                this.LookupGalleryCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<Gallery>> LookupGalleryCompleted;
        private void InvokeGetUserProfileCompletedEvent(EventArgs<Uri> args)
        {
            if (this.GetUserProfileCompleted != null)
            {
                this.GetUserProfileCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<Uri>> GetUserProfileCompleted;
        private void InvokeGetUserPhotosCompletedEvent(EventArgs<Uri> args)
        {
            if (this.GetUserPhotosCompleted != null)
            {
                this.GetUserPhotosCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<Uri>> GetUserPhotosCompleted;
        private void InvokeGetGroupCompletedEvent(EventArgs<Uri> args)
        {
            if (this.GetGroupCompleted != null)
            {
                this.GetGroupCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<Uri>> GetGroupCompleted;
        #endregion
    }
}
