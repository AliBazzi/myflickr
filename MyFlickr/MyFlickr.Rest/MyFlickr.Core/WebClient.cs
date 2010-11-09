using System;

namespace MyFlickr.Core
{
    internal class WebClient 
    {
        private System.Net.WebClient wc;
        public WebClient(Action<System.Net.DownloadStringCompletedEventArgs> callBack)
        {
            if (callBack == null)
                throw new ArgumentNullException("callBack");

            this.wc = new System.Net.WebClient();
            this.wc.DownloadStringCompleted += new System.Net.DownloadStringCompletedEventHandler((obj, e) => callBack.Invoke(e));
        }
        public void InitiateRequestAsync(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            this.wc.DownloadStringAsync(uri);
        }
    }
}
