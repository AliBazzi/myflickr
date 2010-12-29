using System;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// Extension Methods for MachineTags.
    /// </summary>
    public static class SynchronousMachineTags
    {
        /// <summary>
        /// Return a list of unique predicates, optionally limited by a given namespace.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="machineTags">Instance.</param>
        /// <param name="nameSpace">Limit the list of predicates returned to those that have the following namespace.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>PredicatesCollection Object.</returns>
        public static PredicatesCollection GetPredicates(this MachineTags machineTags, string nameSpace = null, Nullable<int> perPage = null,
            Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<PredicatesCollection> FSP = new FlickrSynchronousPrmitive<PredicatesCollection>();

            Action<object, EventArgs<PredicatesCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            machineTags.GetPredicatesCompleted += new EventHandler<EventArgs<PredicatesCollection>>(handler);
            FSP.Token = machineTags.GetPredicatesAsync(nameSpace,perPage,page);
            FSP.WaitForAsynchronousCall();
            machineTags.GetPredicatesCompleted -= new EventHandler<EventArgs<PredicatesCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Fetch recently used (or created) machine tags values.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="machineTags">Instance.</param>
        /// <param name="nameSpace">A namespace that all values should be restricted to.</param>
        /// <param name="predicate">A predicate that all values should be restricted to.</param>
        /// <param name="addedSince">Only return machine tags values that have been added since this timestamp, in epoch seconds. </param>
        /// <returns>ValuesCollection Object.</returns>
        public static ValuesCollection GetRecentValues(this MachineTags machineTags, string nameSpace = null, string predicate = null, string addedSince = null)
        {
            FlickrSynchronousPrmitive<ValuesCollection> FSP = new FlickrSynchronousPrmitive<ValuesCollection>();

            Action<object, EventArgs<ValuesCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            machineTags.GetRecentValuesCompleted += new EventHandler<EventArgs<ValuesCollection>>(handler);
            FSP.Token = machineTags.GetRecentValuesAsync(nameSpace, predicate,addedSince);
            FSP.WaitForAsynchronousCall();
            machineTags.GetRecentValuesCompleted -= new EventHandler<EventArgs<ValuesCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Return a list of unique values for a namespace and predicate.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="machineTags">Instance.</param>
        /// <param name="nameSpace">The namespace that all values should be restricted to.</param>
        /// <param name="predicate">The predicate that all values should be restricted to.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>ValuesCollection Object.</returns>
        public static ValuesCollection GetValues(this MachineTags machineTags, string nameSpace, string predicate, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<ValuesCollection> FSP = new FlickrSynchronousPrmitive<ValuesCollection>();

            Action<object, EventArgs<ValuesCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            machineTags.GetValuesCompleted += new EventHandler<EventArgs<ValuesCollection>>(handler);
            FSP.Token = machineTags.GetValuesAsync(nameSpace,predicate,perPage,page);
            FSP.WaitForAsynchronousCall();
            machineTags.GetValuesCompleted -= new EventHandler<EventArgs<ValuesCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Return a list of unique namespace and predicate pairs, optionally limited by predicate or namespace, in alphabetical order.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="machineTags">Instance.</param>
        /// <param name="nameSpace">Limit the list of pairs returned to those that have the following namespace.</param>
        /// <param name="predicate">Limit the list of pairs returned to those that have the following predicate.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>PairsCollection Object.</returns>
        public static PairsCollection GetPairs(this MachineTags machineTags, string nameSpace = null, string predicate = null,
            Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<PairsCollection> FSP = new FlickrSynchronousPrmitive<PairsCollection>();

            Action<object, EventArgs<PairsCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            machineTags.GetPairsCompleted += new EventHandler<EventArgs<PairsCollection>>(handler);
            FSP.Token = machineTags.GetPairsAsync(nameSpace, predicate, perPage, page);
            FSP.WaitForAsynchronousCall();
            machineTags.GetPairsCompleted -= new EventHandler<EventArgs<PairsCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Return a list of unique namespaces, optionally limited by a given predicate, in alphabetical order.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="machineTags">Instance.</param>
        /// <param name="predicate">Limit the list of namespaces returned to those that have the following predicate.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>NamespacesCollection Object.</returns>
        public static NamespacesCollection GetNamespaces(this MachineTags machineTags, string predicate = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<NamespacesCollection> FSP = new FlickrSynchronousPrmitive<NamespacesCollection>();

            Action<object, EventArgs<NamespacesCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            machineTags.GetNamespacesCompleted += new EventHandler<EventArgs<NamespacesCollection>>(handler);
            FSP.Token = machineTags.GetNamespacesAsync(predicate, perPage, page);
            FSP.WaitForAsynchronousCall();
            machineTags.GetNamespacesCompleted -= new EventHandler<EventArgs<NamespacesCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }
    }
}
