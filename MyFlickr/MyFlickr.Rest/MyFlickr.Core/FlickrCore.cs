using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MyFlickr.Core
{
    /// <summary>
    /// the Core Class that holds the necessary Tools that Enables you To Invoke Flickr API.
    /// </summary>
    public static class FlickrCore
    {
        /// <summary>
        /// get or set the Proxy to be Used when calling Flickr API.
        /// Note : the call to get; set ; is Unsafe .
        /// </summary>
        public static System.Net.IWebProxy Proxy { get; set; }

        /// <summary>
        /// Initiate Get HTTP Call Asynchronously.
        /// </summary>
        /// <param name="downloadCallBack">Call Back to Invoke when Downloading the Content of the Response Completes.</param>
        /// <param name="downloadErrorCallBack">Call Back to Invoke when Error Occurs.</param>
        /// <param name="sharedSecret">the Shared Secret of Your API Key on Flickr. can be Null.</param>
        /// <param name="parameters">set of parameters to Pass to Flickr.</param>
        /// <returns>Uri that will Be Invoked.</returns>
        public static Uri InitiateGetRequest(Action<XElement> downloadCallBack,Action<Exception> downloadErrorCallBack,string sharedSecret,params Parameter[] parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            if (downloadCallBack == null)
                throw new ArgumentNullException("callBack");
            if (downloadErrorCallBack == null)
                throw new ArgumentNullException("onErrorCallBack");

            return new WebClient(Proxy, (System.Net.DownloadStringCompletedEventArgs e) => {
                if (e.Error == null)
                    XmlParsing(downloadCallBack, downloadErrorCallBack, e.Result);
                else
                    downloadErrorCallBack.Invoke(e.Error);
            }).InitiateGetRequestAsync(UriHelper.BuildUri(parameters, sharedSecret));
        }

        /// <summary>
        /// Initiate POST HTTP Call Asynchronously.
        /// </summary>
        /// <param name="downloadCallBack">Call Back to Invoke when Downloading the Content of the Response Completes.</param>
        /// <param name="downloadErrorCallBack">Call Back to Invoke when Error Occurs.</param>
        /// <param name="sharedSecret">the Shared Secret of Your API Key on Flickr. can be Null.</param>
        /// <param name="parameters">set of parameters to Pass to Flickr.</param>
        /// <returns>Uri that will Be Invoked.</returns>
        public static Uri InitiatePostRequest(Action<XElement> downloadCallBack, Action<Exception> downloadErrorCallBack, string sharedSecret, params Parameter[] parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            if (downloadCallBack == null)
                throw new ArgumentNullException("callBack");
            if (downloadErrorCallBack == null)
                throw new ArgumentNullException("onErrorCallBack");

            return new WebClient(Proxy,(System.Net.UploadStringCompletedEventArgs e) =>
            {
                if (e.Error == null)
                    XmlParsing(downloadCallBack, downloadErrorCallBack, e.Result);
                else
                    downloadErrorCallBack.Invoke(e.Error);
            }).InitiatePostRequestAsync(UriHelper.BuildUri(parameters, sharedSecret));
        }
        
        private static void XmlParsing(Action<XElement> downloadCallBack, Action<Exception> downloadErrorCallBack, string content)
        {
            var root = XDocument.Parse(content);

            if (root.Element("rsp").Attribute("stat").Value == "fail")
                downloadErrorCallBack.Invoke(new FlickrException(root.Element("rsp")));
            else
                downloadCallBack.Invoke(root.Element("rsp"));
        }
    }
}
