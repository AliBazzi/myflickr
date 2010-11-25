using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    public static class SynchronousPhotosSets
    {
        /// <summary>
        /// Returns the comments for a photoset.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="photoset"></param>
        /// <returns>Enumerable of comments</returns>
        public static IEnumerable<Comment> GetCommentsList(this PhotosSetBasic photoset)
        {
            FlickrSynchronousPrmitive<IEnumerable<Comment>> FSP = new FlickrSynchronousPrmitive<IEnumerable<Comment>>();

            Action<object, EventArgs<IEnumerable<Comment>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photoset.GetCommentsListCompleted += new EventHandler<EventArgs<IEnumerable<Comment>>>(handler);
            FSP.Token = photoset.GetCommentsListAsync();
            FSP.WaitForAsynchronousCall();
            photoset.GetCommentsListCompleted -= new EventHandler<EventArgs<IEnumerable<Comment>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Add a comment to a photoset.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoset"></param>
        /// <param name="text">Text of the comment.</param>
        /// <returns>the ID of the Added Comment</returns>
        public static string AddComment(this PhotosSetBasic photoset,string text)
        {
            FlickrSynchronousPrmitive<string> FSP = new FlickrSynchronousPrmitive<string>();

            Action<object, EventArgs<string>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photoset.AddCommentCompleted += new EventHandler<EventArgs<string>>(handler);
            FSP.Token = photoset.AddCommentAsync(text);
            FSP.WaitForAsynchronousCall();
            photoset.AddCommentCompleted -= new EventHandler<EventArgs<string>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Delete a photoset comment as the currently authenticated user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoset"></param>
        /// <param name="commentID">The id of the comment to delete from a photoset.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply DeleteComment(this PhotosSetBasic photoset, string commentID)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photoset.DeleteCommentCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photoset.DeleteCommentAsync(commentID);
            FSP.WaitForAsynchronousCall();
            photoset.DeleteCommentCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Edit the text of a comment as the currently authenticated user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoset"></param>
        /// <param name="commentID">The id of the comment to edit.</param>
        /// <param name="text">Update the comment to this text.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply EditComment(this PhotosSetBasic photoset, string commentID, string text)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photoset.EditCommentCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photoset.EditCommentAsync(commentID,text);
            FSP.WaitForAsynchronousCall();
            photoset.EditCommentCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }
    }
}
