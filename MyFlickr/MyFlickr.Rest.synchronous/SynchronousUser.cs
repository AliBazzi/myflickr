using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyFlickr.Core;

namespace MyFlickr.Rest.Synchronous
{
    public static class SynchronousUser
    {
        public static ContactsList GetContactsList(this User user,ContactFilter contactFilter = ContactFilter.Both, int page = 1, int perPage = 1000)
        {
            FlickrSynchronousPrmitive<ContactsList> FSP = new FlickrSynchronousPrmitive<ContactsList>();

            Action<object, EventArgs<ContactsList>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetContactsListCompleted += new EventHandler<EventArgs<ContactsList>>(handler);
            FSP.Token = user.GetContactsListAsync(contactFilter, page, perPage);
            FSP.WaitForAsynchronousCall();
            user.GetContactsListCompleted -= new EventHandler<EventArgs<ContactsList>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        public static ContactsList GetPublicContactsList(this User user, string userID, int page = 1, int perPage = 1000)
        {
            FlickrSynchronousPrmitive<ContactsList> FSP = new FlickrSynchronousPrmitive<ContactsList>();

            Action<object, EventArgs<ContactsList>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetPublicContactsListCompleted += new EventHandler<EventArgs<ContactsList>>(handler);
            FSP.Token = user.GetPublicContactsListAsync(userID, page, perPage);
            FSP.WaitForAsynchronousCall();
            user.GetPublicContactsListCompleted -= new EventHandler<EventArgs<ContactsList>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        public static ContactsList GetPublicContactsList(this User user, int page = 1, int perPage = 1000)
        {
            return GetPublicContactsList(user, user.UserID, page, perPage);
        }
    }
}
