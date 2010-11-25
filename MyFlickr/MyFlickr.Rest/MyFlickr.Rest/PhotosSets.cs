using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents a collection of photosets
    /// </summary>
    public class PhotoSetsCollection : IEnumerable<PhotosSet>
    {
        private XElement data;
        private readonly AuthenticationTokens authTkns;

        internal PhotoSetsCollection(AuthenticationTokens authTkns,XElement element)
        {
            this.authTkns = authTkns;
            this.data = element;
            this.PhotosSetsCount = element.Elements("photoset").Count();
        }

        /// <summary>
        /// the number of photosets in this collection
        /// </summary>
        public int PhotosSetsCount { get; private set; }

        /// <summary>
        /// the Photosets Objects
        /// </summary>
        public IEnumerable<PhotosSet> PhotoSets
        {
            get
            {
                return data.Elements("photoset").Select(elm => new PhotosSet(authTkns,elm));
            }
        }

        public IEnumerator<PhotosSet> GetEnumerator()
        {
            foreach (var photoset in this.PhotoSets)
                yield return photoset;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// represents photosSet Basic Information
    /// </summary>
    public class PhotosSetBasic
    {
        private readonly AuthenticationTokens authTkns;

        internal PhotosSetBasic(AuthenticationTokens authTkns,XElement elm)
        {
            this.authTkns = authTkns;
            this.ID = Int64.Parse(elm.Attribute("id").Value);
            this.Title = elm.Element("title") != null ? elm.Element("title").Value : elm.Attribute("title").Value ;
            this.Description = elm.Attribute("description") != null ?  elm.Attribute("description").Value : null;
        }

        /// <summary>
        /// the ID that identifies the photoset
        /// </summary>
        public Int64 ID { get; private set; }

        /// <summary>
        /// the title of the photoset
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// the description of the photoset , could be Null
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Returns the comments for a photoset.
        /// This method does not require authentication.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetCommentsListAsync()
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeGetCommentsListCompletedEvent(new EventArgs<IEnumerable<Comment>>(token,
                    elm.Element("comments").Elements("comment").Select(comment => new Comment(comment)))),
                e => this.InvokeGetCommentsListCompletedEvent(new EventArgs<IEnumerable<Comment>>(token, e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photosets.comments.getList"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photoset_id", this.ID));

            return token;
        }

        /// <summary>
        /// Add a comment to a photoset.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="text">Text of the comment.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token AddCommentAsync(string text)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeAddCommentCompletedEvent(new EventArgs<string>(token,elm.Element("comment").Attribute("id").Value)),
                e => this.InvokeAddCommentCompletedEvent(new EventArgs<string>(token,e)), this.authTkns.SharedSecret, 
                new Parameter("method", "flickr.photosets.comments.addComment"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photoset_id", this.ID), new Parameter("comment_text", text));

            return token;
        }

        /// <summary>
        /// Delete a photoset comment as the currently authenticated user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="commentID">The id of the comment to delete from a photoset.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token DeleteCommentAsync(string commentID)
        {
            if (string.IsNullOrEmpty(commentID))
                throw new ArgumentException("commentID");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeDeleteCommentCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)),
                e => this.InvokeDeleteCommentCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photosets.comments.deleteComment"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("comment_id", commentID));

            return token;
        }

        /// <summary>
        /// Edit the text of a comment as the currently authenticated user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="commentID">The id of the comment to edit.</param>
        /// <param name="text">Update the comment to this text.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token EditCommentAsync(string commentID, string text)
        {
            if (string.IsNullOrEmpty(commentID))
                throw new ArgumentException("commentID");
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("text");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeEditCommentCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)), 
                e => this.InvokeEditCommentCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photosets.comments.editComment"), new Parameter("api_key", this.authTkns.ApiKey), 
                new Parameter("auth_token", this.authTkns.Token), new Parameter("comment_id", commentID), new Parameter("comment_text", text));

            return token;
        }

        #region Events
        private void InvokeEditCommentCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.EditCommentCompleted != null)
            {
                this.EditCommentCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<NoReply>> EditCommentCompleted;
        private void InvokeDeleteCommentCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.DeleteCommentCompleted != null)
            {
                this.DeleteCommentCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<NoReply>> DeleteCommentCompleted;
        private void InvokeAddCommentCompletedEvent(EventArgs<string> args)
        {
            if (this.AddCommentCompleted != null)
            {
                this.AddCommentCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<string>> AddCommentCompleted;
        private void InvokeGetCommentsListCompletedEvent(EventArgs<IEnumerable<Comment>> args)
        {
            if (this.GetCommentsListCompleted != null)
            {
                this.GetCommentsListCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<IEnumerable<Comment>>> GetCommentsListCompleted;
        #endregion
    }

    /// <summary>
    /// represents a user PhotosSet
    /// </summary>
    public class PhotosSet : PhotosSetBasic
    {
        public PhotosSet(AuthenticationTokens authTkns,XElement elm)
            :base(authTkns,elm)
        {
            this.Primary = Int64.Parse(elm.Attribute("primary").Value);
            this.Secret = elm.Attribute("secret").Value;
            this.Server = int.Parse(elm.Attribute("server").Value);
            this.Farm = int.Parse(elm.Attribute("farm").Value);
            this.PhotosCount = int.Parse(elm.Attribute("photos").Value);
            this.VideosCount = int.Parse(elm.Attribute("videos").Value);
        }

        /// <summary>
        /// the ID of default photo in this photoset
        /// </summary>
        public Int64 Primary { get; private set; }

        /// <summary>
        /// the secret used to build the URL of the default photo in this photoset
        /// </summary>
        public string Secret { get; private set; }

        /// <summary>
        /// the number of server the default photo in this set resides on
        /// </summary>
        public int Server { get; private set; }

        /// <summary>
        /// the number of servers farm the default photo in this set resides on
        /// </summary>
        public int Farm { get; private set; }

        /// <summary>
        /// the number of photos contained in this photoset
        /// </summary>
        public int PhotosCount { get; private set; }

        /// <summary>
        /// the number of videos contained in this photoset
        /// </summary>
        public int VideosCount { get; private set; }
    }
}
