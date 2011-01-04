using System;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// Represents Search Method that exist in many Namespaces.
    /// </summary>
    public class Search
    {
        private readonly AuthenticationTokens authTkns;

        /// <summary>
        /// create an Instance of Search.
        /// </summary>
        /// <param name="authenticationTokens">Authentication Tokens object.</param>
        public Search(AuthenticationTokens authenticationTokens)
        {
            if (authenticationTokens == null)
                throw new ArgumentNullException("authenticationTokens");
            this.authTkns= authenticationTokens;
        }

        /// <summary>
        /// create an Instance of Search.
        /// </summary>
        ///<param name="apiKey">the API Key of your Application.</param>
        public Search(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("apiKey");
            this.authTkns = new AuthenticationTokens(apiKey, null, null, AccessPermission.None, null, null, null);
        }

        /// <summary>
        /// Return a list of photos matching some criteria. Only photos visible to the calling user will be returned. 
        /// To return private or semi-private photos, the caller must be authenticated with 'read' permissions, and have permission to view the photos. 
        /// Unauthenticated calls will only return public photos.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="userID">UserID that represents a Flickr User.</param>
        /// <param name="tags">A comma-delimited list of tags. Photos with one or more of the tags listed will be returned. You can exclude results that match a term by prepending it with a - character.</param>
        /// <param name="tagMode">Either 'any' for an OR combination of tags, or 'all' for an AND combination. Defaults to 'any' if not specified.</param>
        /// <param name="text">A free text search. Photos who's title, description or tags contain the text will be returned. You can exclude results that match a term by prepending it with a - character.</param>
        /// <param name="minUploadDate">Minimum upload date. Photos with an upload date greater than or equal to this value will be returned. The date can be in the form of a unix timestamp or mysql datetime.</param>
        /// <param name="maxUploadDate">Maximum upload date. Photos with an upload date less than or equal to this value will be returned. The date can be in the form of a unix timestamp or mysql datetime.</param>
        /// <param name="minTakenDate">Minimum taken date. Photos with an taken date greater than or equal to this value will be returned. The date can be in the form of a mysql datetime or unix timestamp.</param>
        /// <param name="maxTakenDate">Maximum taken date. Photos with an taken date less than or equal to this value will be returned. The date can be in the form of a mysql datetime or unix timestamp.</param>
        /// <param name="license">The license id for photos (for possible values see the flickr.photos.licenses.getInfo method). Multiple licenses may be comma-separated.</param>
        /// <param name="sortType">The order in which to sort returned photos. Deafults to date-posted-desc (unless you are doing a radial geo query, in which case the default sorting is by ascending distance from the point specified). The possible values are: date-posted-asc, date-posted-desc, date-taken-asc, date-taken-desc, interestingness-desc, interestingness-asc, and relevance.</param>
        /// <param name="privacyFilter">Return photos only matching a certain privacy level. This only applies when making an authenticated call to view photos you own. </param>
        /// <param name="bbox">A comma-delimited list of 4 values defining the Bounding Box of the area that will be searched.
        ///The 4 values represent the bottom-left corner of the box and the top-right corner, minimum_longitude, minimum_latitude, maximum_longitude, maximum_latitude.
        ///Longitude has a range of -180 to 180 , latitude of -90 to 90. Defaults to -180, -90, 180, 90 if not specified.
        ///Unlike standard photo queries, geo (or bounding box) queries will only return 250 results per page.
        ///Geo queries require some sort of limiting agent in order to prevent the database from crying. This is basically like the check against "parameterless searches" for queries without a geo component.
        ///A tag, for instance, is considered a limiting agent as are user defined min_date_taken and min_date_upload parameters — If no limiting factor is passed we return only photos added in the last 12 hours (though we may extend the limit in the future).</param>
        /// <param name="accuracy">Recorded accuracy level of the location information. Current range is 1-16 :
        ///* World level is 1
        ///* Country is ~3
        ///* Region is ~6
        ///* City is ~11
        ///* Street is ~16
        ///Defaults to maximum value if not specified.</param>
        /// <param name="saftyLevel">Safe search setting</param>
        /// <param name="contentType">Content Type setting</param>
        /// <param name="machineTags">Aside from passing in a fully formed machine tag, there is a special syntax for searching on specific properties :
        ///* Find photos using the 'dc' namespace : "machine_tags" => "dc:"
        ///* Find photos with a title in the 'dc' namespace : "machine_tags" => "dc:title="
        ///* Find photos titled "mr. camera" in the 'dc' namespace : "machine_tags" => "dc:title=\"mr. camera\"
        ///* Find photos whose value is "mr. camera" : "machine_tags" => "*:*=\"mr. camera\""
        ///* Find photos that have a title, in any namespace : "machine_tags" => "*:title="
        ///* Find photos that have a title, in any namespace, whose value is "mr. camera" : "machine_tags" => "*:title=\"mr. camera\""
        ///* Find photos, in the 'dc' namespace whose value is "mr. camera" : "machine_tags" => "dc:*=\"mr. camera\""
        ///Multiple machine tags may be queried by passing a comma-separated list. The number of machine tags you can pass in a single query depends on the tag mode (AND or OR) that you are querying with. "AND" queries are limited to (16) machine tags. "OR" queries are limited to (8).</param>
        /// <param name="machineTagMode">Either 'any' for an OR combination of tags, or 'all' for an AND combination. Defaults to 'any' if not specified.</param>
        /// <param name="groupID">The id of a group who's pool to search. If specified, only matching photos posted to the group's pool will be returned.</param>
        /// <param name="contacts">Search your contacts. Either 'all' or 'ff' for just friends and family. (Experimental)</param>
        /// <param name="woeID">A 32-bit identifier that uniquely represents spatial entities. (not used if bbox argument is present).
        ///Geo queries require some sort of limiting agent in order to prevent the database from crying. This is basically like the check against "parameterless searches" for queries without a geo component.
        ///A tag, for instance, is considered a limiting agent as are user defined min_date_taken and min_date_upload parameters ; If no limiting factor is passed we return only photos added in the last 12 hours (though we may extend the limit in the future).</param>
        /// <param name="placeID">A Flickr place id. (not used if bbox argument is present).
        ///Geo queries require some sort of limiting agent in order to prevent the database from crying. This is basically like the check against "parameterless searches" for queries without a geo component.
        ///A tag, for instance, is considered a limiting agent as are user defined min_date_taken and min_date_upload parameters ; If no limiting factor is passed we return only photos added in the last 12 hours (though we may extend the limit in the future).</param>
        /// <param name="mediaType">Filter results by media type. Possible values are all (default), photos or videos</param>
        /// <param name="hasGeo">Any photo that has been geotagged, or if the value is "0" any photo that has not been geotagged.
        ///Geo queries require some sort of limiting agent in order to prevent the database from crying. This is basically like the check against "parameterless searches" for queries without a geo component.
        ///A tag, for instance, is considered a limiting agent as are user defined min_date_taken and min_date_upload parameters ; If no limiting factor is passed we return only photos added in the last 12 hours (though we may extend the limit in the future).</param>
        /// <param name="geoContext">Geo context is a numeric value representing the photo's geotagginess beyond latitude and longitude. For example, you may wish to search for photos that were taken "indoors" or "outdoors". 
        /// Geo queries require some sort of limiting agent in order to prevent the database from crying. This is basically like the check against "parameterless searches" for queries without a geo component.
        ///A tag, for instance, is considered a limiting agent as are user defined min_date_taken and min_date_upload parameters ; If no limiting factor is passed we return only photos added in the last 12 hours (though we may extend the limit in the future).</param>
        /// <param name="latitude">A valid latitude, in decimal format, for doing radial geo queries.
        ///Geo queries require some sort of limiting agent in order to prevent the database from crying. This is basically like the check against "parameterless searches" for queries without a geo component.
        ///A tag, for instance, is considered a limiting agent as are user defined min_date_taken and min_date_upload parameters ; If no limiting factor is passed we return only photos added in the last 12 hours (though we may extend the limit in the future).</param>
        /// <param name="longitude">A valid longitude, in decimal format, for doing radial geo queries.
        ///Geo queries require some sort of limiting agent in order to prevent the database from crying. This is basically like the check against "parameterless searches" for queries without a geo component.
        ///A tag, for instance, is considered a limiting agent as are user defined min_date_taken and min_date_upload parameters ; If no limiting factor is passed we return only photos added in the last 12 hours (though we may extend the limit in the future).</param>
        /// <param name="radius">A valid radius used for geo queries, greater than zero and less than 20 miles (or 32 kilometers), for use with point-based geo queries. The default value is 5 (km).</param>
        /// <param name="radiosUnits">The unit of measure when doing radial geo queries. Valid options are "mi" (miles) and "km" (kilometers). The default is "km".</param>
        /// <param name="isCommons">Limit the scope of the search to only photos that are part of the Flickr Commons project. Default is false.</param>
        /// <param name="inGallery">Limit the scope of the search to only photos that are in a gallery? Default is false, search all photos.</param>
        /// <param name="isGetty">Limit the scope of the search to only photos that are for sale on Getty. Default is false.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token SearchPhotosAsync(string userID = null, string tags = null, Nullable<TagMode> tagMode = null, string text = null, string minUploadDate = null,
            string maxUploadDate = null, string minTakenDate = null, string maxTakenDate = null, string license = null, Nullable<SortType> sortType = null,
            Nullable<PrivacyFilter> privacyFilter = null, string bbox = null, Nullable<int> accuracy = null, Nullable<SafetyLevel> saftyLevel = null,
            Nullable<ContentType> contentType = null, string machineTags = null, Nullable<TagMode> machineTagMode = null, string groupID = null,
            string contacts = null, Nullable<int> woeID = null, string placeID = null, Nullable<MediaType> mediaType = null, Nullable<bool> hasGeo = null,
            Nullable<GeoContext> geoContext = null, Nullable<int> latitude = null, Nullable<int> longitude = null, Nullable<int> radius = null,
            Nullable<RadiusUnits> radiosUnits = null, Nullable<bool> isCommons = null, Nullable<bool> inGallery = null, Nullable<bool> isGetty = null,
            string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeSearchPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, new PhotosCollection(this.authTkns, elm.Element("photos")))),
                e => this.InvokeSearchPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.search"), new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("user_id", userID), new Parameter("tag", tags), new Parameter("tag_Mode", tagMode), new Parameter("text", text),
                new Parameter("min_upload_date", minUploadDate), new Parameter("max_upload_date", maxUploadDate), new Parameter("min_taken_date", minTakenDate),
                new Parameter("max_taken_date", maxTakenDate), new Parameter("license", license), new Parameter("sort_type", sortType),
                new Parameter("privacy_filter", privacyFilter), new Parameter("bbox", bbox), new Parameter("accuracy", accuracy),
                new Parameter("safe_search", saftyLevel), new Parameter("content_type", contentType), new Parameter("machine_tags", machineTags),
                new Parameter("machine_tag_mode", machineTagMode), new Parameter("group_id", groupID), new Parameter("contacts", contacts),
                new Parameter("woe_id", woeID), new Parameter("place_id", placeID), new Parameter("media", mediaType), new Parameter("has_geo", hasGeo),
                new Parameter("geo_context", geoContext), new Parameter("lat", latitude), new Parameter("lon", longitude), new Parameter("radius", radius),
                new Parameter("radius_units", radiosUnits), new Parameter("is_commons", isCommons), new Parameter("in_gallery", inGallery),
                new Parameter("is_getty", isGetty), new Parameter("extras", extras), new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        /// <summary>
        /// Return a user's NSID, given their email address.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="email">The email address of the user to find (may be primary or secondary).</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token FindPeopleByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("email");

            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeFindPeopleByEmailCompletedEvent(new EventArgs<Tuple<string,string>>(token,new Tuple<string,string>(
                    elm.Element("user").Attribute("nsid").Value,elm.Element("user").Element("username").Value))), 
                e => this.InvokeFindPeopleByEmailCompletedEvent(new EventArgs<Tuple<string,string>>(token,e)), this.authTkns.SharedSecret, 
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.people.findByEmail"), new Parameter("find_email", email));

            return token;
        }

        /// <summary>
        /// Return a user's NSID, given their username.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="username">The username of the user to lookup.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token FindPeopleByUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("username");

            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeFindPeopleByUsernameCompletedEvent(new EventArgs<Tuple<string, string>>(token, new Tuple<string, string>(
                    elm.Element("user").Attribute("nsid").Value, elm.Element("user").Element("username").Value))),
                e => this.InvokeFindPeopleByUsernameCompletedEvent(new EventArgs<Tuple<string, string>>(token, e)), this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.people.findByUsername"), new Parameter("username", username));

            return token;
        }

        #region Events
        private void InvokeFindPeopleByUsernameCompletedEvent(EventArgs<Tuple<string,string>> args)
        {
            if (this.FindPeopleByUsernameCompleted != null)
            {
                this.FindPeopleByUsernameCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when FindPeopleByUsernameAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<Tuple<string,string>>> FindPeopleByUsernameCompleted;
        private void InvokeFindPeopleByEmailCompletedEvent(EventArgs<Tuple<string,string>> args)
        {
            if (this.FindPeopleByEmailCompleted != null)
            {
                this.FindPeopleByEmailCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when FindPeopleByEmailAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<Tuple<string,string>>> FindPeopleByEmailCompleted;
        private void InvokeSearchPhotosCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.SearchPhotosCompleted != null)
            {
                this.SearchPhotosCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when SearchPhotosAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PhotosCollection>> SearchPhotosCompleted;
        #endregion
    }
}
