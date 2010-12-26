using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents the Methods that exist in flickr.machineTags namespace
    /// </summary>
    public class MachineTags
    {
        private readonly string apiKey;

        /// <summary>
        /// Create MavhineTags Object
        /// </summary>
        /// <param name="apiKey">The API Key of your Application</param>
        public MachineTags(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("apiKey");
            this.apiKey = apiKey;
        }

        /// <summary>
        /// Return a list of unique predicates, optionally limited by a given namespace.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="nameSpace">Limit the list of predicates returned to those that have the following namespace.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetPredicatesAsync(string nameSpace = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetPredicatesCompletedEvent(new EventArgs<PredicatesCollection>(token, new PredicatesCollection(elm.Element("predicates")))), 
                e => this.InvokeGetPredicatesCompletedEvent(new EventArgs<PredicatesCollection>(token,e)), null,
                new Parameter("api_key", this.apiKey), new Parameter("method", "flickr.machinetags.getPredicates"), 
                new Parameter("namespace", nameSpace), new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        /// <summary>
        /// Fetch recently used (or created) machine tags values.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="nameSpace">A namespace that all values should be restricted to.</param>
        /// <param name="predicate">A predicate that all values should be restricted to.</param>
        /// <param name="addedSince">Only return machine tags values that have been added since this timestamp, in epoch seconds. </param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetRecentValuesAsync(string nameSpace = null ,string predicate = null ,string addedSince = null)
        {
            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetRecentValuesCompletedEvent(new EventArgs<ValuesCollection>(token, new ValuesCollection(elm.Element("values")))),
                e => this.InvokeGetRecentValuesCompletedEvent(new EventArgs<ValuesCollection>(token, e)), null,
                new Parameter("api_key", this.apiKey), new Parameter("method", "flickr.machinetags.getRecentValues"), 
                new Parameter("namespace", nameSpace), new Parameter("predicate", predicate), new Parameter("added_since", addedSince));

            return token;
        }

        /// <summary>
        /// Return a list of unique values for a namespace and predicate.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="nameSpace">The namespace that all values should be restricted to.</param>
        /// <param name="predicate">The predicate that all values should be restricted to.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetValuesAsync(string nameSpace, string predicate, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            if (string.IsNullOrEmpty(nameSpace))
                throw new ArgumentException("nameSpace");
            if (string.IsNullOrEmpty(predicate))
                throw new ArgumentException("predicate");

            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetValuesCompletedEvent(new EventArgs<ValuesCollection>(token, new ValuesCollection(elm.Element("values")))),
                e => this.InvokeGetValuesCompletedEvent(new EventArgs<ValuesCollection>(token, e)), null,
                new Parameter("api_key", this.apiKey), new Parameter("method", "flickr.machinetags.getValues"), new Parameter("namespace", nameSpace),
                new Parameter("predicate", predicate), new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        /// <summary>
        /// Return a list of unique namespace and predicate pairs, optionally limited by predicate or namespace, in alphabetical order.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="nameSpace">Limit the list of pairs returned to those that have the following namespace.</param>
        /// <param name="predicate">Limit the list of pairs returned to those that have the following predicate.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetPairsAsync(string nameSpace = null, string predicate = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetPairsCompletedEvent(new EventArgs<PairsCollection>(token, new PairsCollection(elm.Element("pairs")))),
                e => this.InvokeGetPairsCompletedEvent(new EventArgs<PairsCollection>(token, e)), null,
                new Parameter("api_key", this.apiKey), new Parameter("method", "flickr.machinetags.getPairs"), new Parameter("namespace", nameSpace),
                new Parameter("predicate", predicate), new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        /// <summary>
        /// Return a list of unique namespaces, optionally limited by a given predicate, in alphabetical order.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="predicate">Limit the list of namespaces returned to those that have the following predicate.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetNamespacesAsync(string predicate = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetNamespacesCompletedEvent(new EventArgs<NamespacesCollection>(token, new NamespacesCollection(elm.Element("namespaces")))),
                e => this.InvokeGetNamespacesCompletedEvent(new EventArgs<NamespacesCollection>(token, e)), null,
                new Parameter("api_key", this.apiKey), new Parameter("method", "flickr.machinetags.getNamespaces"),
                new Parameter("predicate", predicate), new Parameter("per_page", perPage), new Parameter("page", page));

            return token; 
        }

        #region Events
        private void InvokeGetNamespacesCompletedEvent(EventArgs<NamespacesCollection> args)
        {
            if (this.GetNamespacesCompleted != null)
            {
                this.GetNamespacesCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<NamespacesCollection>> GetNamespacesCompleted;
        private void InvokeGetPairsCompletedEvent(EventArgs<PairsCollection> args)
        {
            if (this.GetPairsCompleted != null)
            {
                this.GetPairsCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<PairsCollection>> GetPairsCompleted;
        private void InvokeGetValuesCompletedEvent(EventArgs<ValuesCollection> args)
        {
            if (this.GetValuesCompleted != null)
            {
                this.GetValuesCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<ValuesCollection>> GetValuesCompleted;
        private void InvokeGetRecentValuesCompletedEvent(EventArgs<ValuesCollection> args)
        {
            if (this.GetRecentValuesCompleted != null)
            {
                this.GetRecentValuesCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<ValuesCollection>> GetRecentValuesCompleted;
        private void InvokeGetPredicatesCompletedEvent(EventArgs<PredicatesCollection> args)
        {
            if (this.GetPredicatesCompleted != null)
            {
                this.GetPredicatesCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<PredicatesCollection>> GetPredicatesCompleted;
        #endregion
    }

    /// <summary>
    /// represents a Collection of Predicates
    /// </summary>
    public class PredicatesCollection : IEnumerable<Predicate>
    {
        private XElement data;

        public PredicatesCollection(XElement element)
        {
            this.data = element;
            this.Page = int.Parse(element.Attribute("page").Value);
            this.PerPage = int.Parse(element.Attribute("perpage").Value);
            this.Total = int.Parse(element.Attribute("total").Value);
            this.Pages = int.Parse(element.Attribute("pages").Value);
        }

        /// <summary>
        /// the Current page
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// the total number of pages
        /// </summary>
        public int Pages { get; private set; }

        /// <summary>
        /// the number of predicates per page
        /// </summary>
        public int PerPage { get; private set; }

        /// <summary>
        /// the total number of predicates
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// enumerable of Predicates
        /// </summary>
        public IEnumerable<Predicate> Predicates { get { return this.data.Elements("predicate").Select(pred => new Predicate(pred)); } }

        public IEnumerator<Predicate> GetEnumerator()
        {
            foreach (var pred in this.Predicates)
                yield return pred;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// represents a Predicate Info
    /// </summary>
    public class Predicate
    {
        internal Predicate(XElement element)
        {
            this.NameSpaces = int.Parse(element.Attribute("namespaces").Value);
            this.Usage = int.Parse(element.Attribute("usage").Value);
            this.Value = element.Value;
        }

        /// <summary>
        /// the Number of Usage of this predicate
        /// </summary>
        public int Usage { get; private set; }

        /// <summary>
        /// the Number of the Namespaces
        /// </summary>
        public int NameSpaces { get; private set; }

        /// <summary>
        /// the Content of the Predicate
        /// </summary>
        public string Value { get; private set; }
    }

    /// <summary>
    /// represents a Collection of Values
    /// </summary>
    public class ValuesCollection : IEnumerable<Value>
    {
        private XElement data;

        internal ValuesCollection(XElement element)
        {
            this.data = element;
            this.Page = int.Parse(element.Attribute("page").Value);
            this.Pages = int.Parse(element.Attribute("pages").Value);
            this.PerPage = int.Parse(element.Attribute("perpage").Value);
            this.Total = int.Parse(element.Attribute("total").Value);
            this.Namespace = element.Attribute("namespace").Value;
            this.Predicate = element.Attribute("predicate").Value;
        }

        /// <summary>
        /// the Current page
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// the total number of pages
        /// </summary>
        public int Pages { get; private set; }

        /// <summary>
        /// the number of values per page
        /// </summary>
        public int PerPage { get; private set; }

        /// <summary>
        /// the total number of values
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// the Namespace
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        /// the predicate
        /// </summary>
        public string Predicate { get; private set; }

        /// <summary>
        /// Enumerable of Value Objects
        /// </summary>
        public IEnumerable<Value> Values { get { return this.data.Elements("value").Select(val => new Value(val)); } }

        public IEnumerator<Value> GetEnumerator()
        {
            foreach (var val in this.Values)
                yield return val;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// represents a Value Info
    /// </summary>
    public class Value
    {
        internal Value(XElement element)
        {
            this.Usage = int.Parse(element.Attribute("usage").Value);
            this.NameSpace = element.Attribute("namespace")!= null ? element.Attribute("namespace").Value : null ;
            this.Predicate = element.Attribute("predicate")!= null ? element.Attribute("predicate").Value : null;
            this.Content = element.Value;
            this.FirstDate = element.Attribute("first_added")!= null ? new Nullable<DateTime>(double.Parse(element.Attribute("first_added").Value).ToDateTimeFromUnix()) : null;
            this.LastDate = element.Attribute("last_added")!=null ? new Nullable<DateTime>(double.Parse(element.Attribute("last_added").Value).ToDateTimeFromUnix()): null;
        }

        /// <summary>
        /// the Number of Usage
        /// </summary>
        public int Usage { get; private set; }

        /// <summary>
        /// the Namespace , Could Be Null
        /// </summary>
        public string NameSpace { get; private set; }

        /// <summary>
        /// the Predicate , Could Be Null
        /// </summary>
        public string Predicate { get; private set; }

        /// <summary>
        /// first date of addition , Could Be Null
        /// </summary>
        public Nullable<DateTime> FirstDate { get; private set; }

        /// <summary>
        /// last date of Addition , Could Be Null
        /// </summary>
        public Nullable<DateTime> LastDate { get; private set; }

        /// <summary>
        /// the Content of the Value
        /// </summary>
        public string Content { get; private set; }
    }

    /// <summary>
    /// represents a Collection of Pairs
    /// </summary>
    public class PairsCollection : IEnumerable<Pair>
    {
        private XElement data;

        internal PairsCollection(XElement element)
        {
            this.data = element;
            this.Page = int.Parse(element.Attribute("page").Value);
            this.Pages = int.Parse(element.Attribute("pages").Value);
            this.PerPage = int.Parse(element.Attribute("perpage").Value);
            this.Total = int.Parse(element.Attribute("total").Value);
        }

        /// <summary>
        /// the Current page
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// the total number of pages
        /// </summary>
        public int Pages { get; private set; }

        /// <summary>
        /// the number of pairs per page
        /// </summary>
        public int PerPage { get; private set; }

        /// <summary>
        /// the total number of pairs
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// Enumerable of Pair Objects
        /// </summary>
        public IEnumerable<Pair> Pairs { get { return this.data.Elements("pair").Select(pair => new Pair(pair)); } }

        public IEnumerator<Pair> GetEnumerator()
        {
            foreach (var pair in this.Pairs)
                yield return pair;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// represents Pair Info
    /// </summary>
    public class Pair
    {
        internal Pair(XElement element)
        {
            this.NameSpace = element.Attribute("namespace").Value;
            this.Predicate = element.Attribute("predicate").Value;
            this.Value = element.Value;
            this.Usage = int.Parse(element.Attribute("usage").Value);
        }
        /// <summary>
        /// the Namespace
        /// </summary>
        public string NameSpace { get; private set; }

        /// <summary>
        /// the Predicate
        /// </summary>
        public string Predicate { get; private set; }

        /// <summary>
        /// the Number of Usage
        /// </summary>
        public int Usage { get; private set; }

        /// <summary>
        /// the Value
        /// </summary>
        public string Value { get; private set; }
    }

    /// <summary>
    /// represents a Collection of Namespaces
    /// </summary>
    public class NamespacesCollection : IEnumerable<Namespace>
    {
        private XElement data;

        internal NamespacesCollection(XElement element)
        {
            this.data = element;
            this.Page = int.Parse(element.Attribute("page").Value);
            this.Pages = int.Parse(element.Attribute("pages").Value);
            this.PerPage = int.Parse(element.Attribute("perpage").Value);
            this.Total = int.Parse(element.Attribute("total").Value);
        }

        /// <summary>
        /// the Current page
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// the total number of pages
        /// </summary>
        public int Pages { get; private set; }

        /// <summary>
        /// the number of namespaces per page
        /// </summary>
        public int PerPage { get; private set; }

        /// <summary>
        /// the total number of namespaces
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// Enumerable of Namespace Objects
        /// </summary>
        public IEnumerable<Namespace> Namespaces { get { return this.data.Elements("namespace").Select(ns => new Namespace(ns)); } }

        public IEnumerator<Namespace> GetEnumerator()
        {
            foreach (var ns in this.Namespaces)
                yield return ns;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// represents Namespace Info
    /// </summary>
    public class Namespace
    {
        internal Namespace(XElement element)
        {
            this.Predicates = int.Parse(element.Attribute("predicates").Value);
            this.Usage = int.Parse(element.Attribute("usage").Value);
            this.Value = element.Value;
        }

        /// <summary>
        /// the Number of Predicates
        /// </summary>
        public int Predicates { get; private set; }

        /// <summary>
        /// the number of Usage
        /// </summary>
        public int Usage { get; private set; }

        /// <summary>
        /// the Value of Namespace
        /// </summary>
        public string Value { get; private set; }
    }
}
