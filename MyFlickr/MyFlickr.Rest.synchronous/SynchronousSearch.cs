using System;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// Extension Methods for Search.
    /// </summary>
    public static class SynchronousSearch
    {
        /// <summary>
        /// Return a list of photos matching some criteria. Only photos visible to the calling user will be returned. 
        /// To return private or semi-private photos, the caller must be authenticated with 'read' permissions, and have permission to view the photos. 
        /// Unauthenticated calls will only return public photos.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="search">Instance.</param>
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
        ///A tag, for instance, is considered a limiting agent as are user defined min_date_taken and min_date_upload parameters; If no limiting factor is passed we return only photos added in the last 12 hours (though we may extend the limit in the future).</param>
        /// <param name="placeID">A Flickr place id. (not used if bbox argument is present).
        ///Geo queries require some sort of limiting agent in order to prevent the database from crying. This is basically like the check against "parameterless searches" for queries without a geo component.
        ///A tag, for instance, is considered a limiting agent as are user defined min_date_taken and min_date_upload parameters ; If no limiting factor is passed we return only photos added in the last 12 hours (though we may extend the limit in the future).</param>
        /// <param name="mediaType">Filter results by media type. Possible values are all (default), photos or videos</param>
        /// <param name="hasGeo">Any photo that has been geotagged, or if the value is "0" any photo that has not been geotagged.
        ///Geo queries require some sort of limiting agent in order to prevent the database from crying. This is basically like the check against "parameterless searches" for queries without a geo component.
        ///A tag, for instance, is considered a limiting agent as are user defined min_date_taken and min_date_upload parameters; If no limiting factor is passed we return only photos added in the last 12 hours (though we may extend the limit in the future).</param>
        /// <param name="geoContext">Geo context is a numeric value representing the photo's geotagginess beyond latitude and longitude. For example, you may wish to search for photos that were taken "indoors" or "outdoors". 
        /// Geo queries require some sort of limiting agent in order to prevent the database from crying. This is basically like the check against "parameterless searches" for queries without a geo component.
        ///A tag, for instance, is considered a limiting agent as are user defined min_date_taken and min_date_upload parameters; If no limiting factor is passed we return only photos added in the last 12 hours (though we may extend the limit in the future).</param>
        /// <param name="latitude">A valid latitude, in decimal format, for doing radial geo queries.
        ///Geo queries require some sort of limiting agent in order to prevent the database from crying. This is basically like the check against "parameterless searches" for queries without a geo component.
        ///A tag, for instance, is considered a limiting agent as are user defined min_date_taken and min_date_upload parameters ; If no limiting factor is passed we return only photos added in the last 12 hours (though we may extend the limit in the future).</param>
        /// <param name="longitude">A valid longitude, in decimal format, for doing radial geo queries.
        ///Geo queries require some sort of limiting agent in order to prevent the database from crying. This is basically like the check against "parameterless searches" for queries without a geo component.
        ///A tag, for instance, is considered a limiting agent as are user defined min_date_taken and min_date_upload parameters; If no limiting factor is passed we return only photos added in the last 12 hours (though we may extend the limit in the future).</param>
        /// <param name="radius">A valid radius used for geo queries, greater than zero and less than 20 miles (or 32 kilometers), for use with point-based geo queries. The default value is 5 (km).</param>
        /// <param name="radiosUnits">The unit of measure when doing radial geo queries. Valid options are "mi" (miles) and "km" (kilometers). The default is "km".</param>
        /// <param name="isCommons">Limit the scope of the search to only photos that are part of the Flickr Commons project. Default is false.</param>
        /// <param name="inGallery">Limit the scope of the search to only photos that are in a gallery? Default is false, search all photos.</param>
        /// <param name="isGetty">Limit the scope of the search to only photos that are for sale on Getty. Default is false.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public static PhotosCollection SearchPhotos(this Search search, string userID = null, string tags = null, Nullable<TagMode> tagMode = null, string text = null,
            string minUploadDate = null, string maxUploadDate = null, string minTakenDate = null, string maxTakenDate = null, string license = null,
            Nullable<SortType> sortType = null, Nullable<PrivacyFilter> privacyFilter = null, string bbox = null, Nullable<int> accuracy = null,
            Nullable<SafetyLevel> saftyLevel = null, Nullable<ContentType> contentType = null, string machineTags = null, Nullable<TagMode> machineTagMode = null,
            string groupID = null, string contacts = null, Nullable<int> woeID = null, string placeID = null, Nullable<MediaType> mediaType = null,
            Nullable<bool> hasGeo = null, Nullable<GeoContext> geoContext = null, Nullable<int> latitude = null, Nullable<int> longitude = null,
            Nullable<int> radius = null, Nullable<RadiusUnits> radiosUnits = null, Nullable<bool> isCommons = null, Nullable<bool> inGallery = null,
            Nullable<bool> isGetty = null, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<PhotosCollection> FSP = new FlickrSynchronousPrmitive<PhotosCollection>();

            Action<object, EventArgs<PhotosCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            search.SearchPhotosCompleted += new EventHandler<EventArgs<PhotosCollection>>(handler);
            FSP.Token = search.SearchPhotosAsync(userID,tags,tagMode,text,minUploadDate,maxUploadDate,minTakenDate,maxTakenDate,license,sortType,privacyFilter,
                bbox,accuracy,saftyLevel,contentType,machineTags,machineTagMode,groupID,contacts,woeID,placeID,mediaType,hasGeo,geoContext,latitude,longitude);
            FSP.WaitForAsynchronousCall();
            search.SearchPhotosCompleted -= new EventHandler<EventArgs<PhotosCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Return a user's NSID, given their email address.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="search">Instance.</param>
        /// <param name="email">The email address of the user to find (may be primary or secondary).</param>
        /// <returns>Tuple that holds the UserID and UserName.</returns>
        public static Tuple<string, string> FindPeopleByEmail(this Search search,string email)
        {
            FlickrSynchronousPrmitive<Tuple<string, string>> FSP = new FlickrSynchronousPrmitive<Tuple<string, string>>();

            Action<object, EventArgs<Tuple<string, string>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            search.FindPeopleByEmailCompleted += new EventHandler<EventArgs<Tuple<string, string>>>(handler);
            FSP.Token = search.FindPeopleByEmailAsync(email);
            FSP.WaitForAsynchronousCall();
            search.FindPeopleByEmailCompleted -= new EventHandler<EventArgs<Tuple<string, string>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Return a user's NSID, given their username.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="search">Instance.</param>
        /// <param name="username">The username of the user to lookup.</param>
        /// <returns>Tuple that holds the UserID and UserName.</returns>
        public static Tuple<string, string> FindPeopleByUsername(this Search search, string username)
        {
            FlickrSynchronousPrmitive<Tuple<string, string>> FSP = new FlickrSynchronousPrmitive<Tuple<string, string>>();

            Action<object, EventArgs<Tuple<string, string>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            search.FindPeopleByUsernameCompleted += new EventHandler<EventArgs<Tuple<string, string>>>(handler);
            FSP.Token = search.FindPeopleByUsernameAsync(username);
            FSP.WaitForAsynchronousCall();
            search.FindPeopleByUsernameCompleted -= new EventHandler<EventArgs<Tuple<string, string>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }
    }
}
