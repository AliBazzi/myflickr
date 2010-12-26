using System;
using System.Collections.Generic;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    public static class SynchronousCommons
    {
        /// <summary>
        /// Retrieves a list of the current Commons institutions.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="commons"></param>
        /// <returns>Enumerable of Institution Objects</returns>
        public static IEnumerable<Institution> GetInstitutions(this Commons commons)
        {
            FlickrSynchronousPrmitive<IEnumerable<Institution>> FSP = new FlickrSynchronousPrmitive<IEnumerable<Institution>>();

            Action<object, EventArgs<IEnumerable<Institution>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            commons.GetInstitutionsCompleted += new EventHandler<EventArgs<IEnumerable<Institution>>>(handler);
            FSP.Token = commons.GetInstitutionsAsync();
            FSP.WaitForAsynchronousCall();
            commons.GetInstitutionsCompleted -= new EventHandler<EventArgs<IEnumerable<Institution>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }
    }
}
