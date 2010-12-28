using System;

namespace MyFlickr.Core
{
    internal class WebClient 
    {
        private System.Net.WebClient wc;

        private WebClient()
        {
            this.wc = new System.Net.WebClient() { Encoding = System.Text.UnicodeEncoding.UTF8 };
        }

        public WebClient(Action<System.Net.DownloadStringCompletedEventArgs> callBack)
            :this()
        {
            if (callBack == null)
                throw new ArgumentNullException("callBack");

            this.wc.DownloadStringCompleted += new System.Net.DownloadStringCompletedEventHandler((o,e) => { callBack.Invoke(e); ((System.Net.WebClient)o).Dispose(); });
        }

        public WebClient(Action<System.Net.UploadStringCompletedEventArgs> callBack)
            : this()
        {
            if (callBack == null)
                throw new ArgumentNullException("callBack");

            this.wc.UploadStringCompleted += new System.Net.UploadStringCompletedEventHandler((o, e) => { callBack.Invoke(e); ((System.Net.WebClient)o).Dispose(); });
        }
        public Uri InitiateGetRequestAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            this.wc.DownloadStringAsync(uri);
            return uri;
        }

        public Uri InitiatePostRequestAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            this.wc.UploadStringAsync(uri,string.Empty);
            return uri;
        }
    }
}
