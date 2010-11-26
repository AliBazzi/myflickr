using System;
using System.Collections.Generic;
using System.Linq;
using MD5;

namespace MyFlickr.Core
{
    internal static class UriHelper
    {
        private static readonly string BaseServiceUrl = "http://api.flickr.com/services/rest/";
        private static readonly string AuthServiceUrl = "http://api.flickr.com/services/auth/";

        public static Uri BuildUri(IEnumerable<Parameter> parameters,string sharedSecret="")
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            string str = BaseServiceUrl + "?";

            foreach (var param in parameters.Where(param => !param.ShouldBeDropped))
                str += string.Format("{0}&",param);

            if (!string.IsNullOrEmpty(sharedSecret))
                str += GetSignture(sharedSecret,parameters);

            return new Uri(str);
        }

        public static Uri CalculateRedirectionUrl(string sharedSecret,params Parameter[] parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentException("sharedSecret");

            string str = AuthServiceUrl + "?";

            foreach (var param in parameters)
                str += string.Format("{0}&", param);

            str += GetSignture(sharedSecret, parameters);

            return new Uri(str);
        }

        private static string GetSignture(string sharedSecret,IEnumerable<Parameter> parameters)
        {
            string str = sharedSecret;

            foreach (var param in parameters.Where(param => !param.ShouldBeDropped).OrderBy(param => param.Name))
                str += param.ToString(true);

            return string.Format("api_sig={0}",str.Hash().GetHexString());
        }
    }
}
