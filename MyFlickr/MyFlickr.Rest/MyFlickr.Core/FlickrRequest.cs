using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MyFlickr.Core
{
    public static class FlickrCore
    {
        public static Uri IntiateGetRequest(Action<XElement> downloadCallBack,Action<Exception> downloadErrorCallBack,string sharedSecret,params Parameter[] parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            if (downloadCallBack == null)
                throw new ArgumentNullException("callBack");
            if (downloadErrorCallBack == null)
                throw new ArgumentNullException("onErrorCallBack");

            return new WebClient( (System.Net.DownloadStringCompletedEventArgs e) => {
                if (e.Error == null)
                    XmlParsing(downloadCallBack, downloadErrorCallBack, e.Result);
                else
                    downloadErrorCallBack.Invoke(e.Error);
            }).InitiateGetRequestAsync(UriHelper.BuildUri(parameters, sharedSecret));
        }
        
        public static Uri InitiatePostRequest(Action<XElement> downloadCallBack, Action<Exception> downloadErrorCallBack, string sharedSecret, params Parameter[] parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            if (downloadCallBack == null)
                throw new ArgumentNullException("callBack");
            if (downloadErrorCallBack == null)
                throw new ArgumentNullException("onErrorCallBack");

            return new WebClient((System.Net.UploadStringCompletedEventArgs e) =>
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
