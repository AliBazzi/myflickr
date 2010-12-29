using System;
using System.Collections.Generic;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// Extension Methods for Reflection.
    /// </summary>
    public static class SynchronousReflection
    {
        /// <summary>
        /// /// Returns a list of available flickr API methods.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="reflection">Instance.</param>
        /// <returns>Enumerable of Methods.</returns>
        public static IEnumerable<Method> GetMethods(this Reflection reflection)
        {
            FlickrSynchronousPrmitive<IEnumerable<Method>> FSP = new FlickrSynchronousPrmitive<IEnumerable<Method>>();

            Action<object, EventArgs<IEnumerable<Method>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            reflection.GetMethodsCompleted += new EventHandler<EventArgs<IEnumerable<Method>>>(handler);
            FSP.Token = reflection.GetMethodsAsync();
            FSP.WaitForAsynchronousCall();
            reflection.GetMethodsCompleted -= new EventHandler<EventArgs<IEnumerable<Method>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns information for a given flickr API method.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="reflection">Instance.</param>
        /// <param name="methodName">The name of the method to fetch information for.</param>
        /// <returns>MethodInfo Object.</returns>
        public static MethodInfo GetMethodInfo(this Reflection reflection, string methodName)
        {
            FlickrSynchronousPrmitive<MethodInfo> FSP = new FlickrSynchronousPrmitive<MethodInfo>();

            Action<object, EventArgs<MethodInfo>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            reflection.GetMethodInfoCompleted += new EventHandler<EventArgs<MethodInfo>>(handler);
            FSP.Token = reflection.GetMethodInfoAsync(methodName);
            FSP.WaitForAsynchronousCall();
            reflection.GetMethodInfoCompleted -= new EventHandler<EventArgs<MethodInfo>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }
    }
}
