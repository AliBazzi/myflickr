using System;
using System.Collections.Generic;
using MyFlickr.Rest;

namespace APITest
{
    class Program
    {
        static void Main(string[] args)
        {
            //1 - get frob 
            //2 - build the URL , and waits authorization ...

            var frob = "";
            FlickrCore.IntiateRequest(new List<Parameter>() { new Parameter("method","flickr.auth.getFrob"),
                                                                 new Parameter("api_key", "fa8abd96654642183131486ba3ae9695") },
                                                                 "e820b2d523ce547e",
                (element) =>
                {
                    Console.WriteLine(element.Element("frob").Value);
                    frob = element.Element("frob").Value;
                    System.Diagnostics.Process.Start(
                        UriHelper.CalculateRedirectionUrl(new List<Parameter>()
                    {new Parameter("api_key", "fa8abd96654642183131486ba3ae9695"),
                    new Parameter("perms", "write"),
                    new Parameter("frob", element.Element("frob").Value)},
                    "e820b2d523ce547e").ToString());
                }, e => { Console.WriteLine(e.Message); });

           

            ////3 - get the token
            ////flickr.auth.getToken

            //Console.ReadLine();
            //FlickrRequest.IntiateRequest(new List<Parameter>() { new Parameter("method","flickr.auth.getToken"),
            //                                                     new Parameter("api_key", "fa8abd96654642183131486ba3ae9695"),
            //                                                     new Parameter("frob", frob) },
            //                                                     "e820b2d523ce547e",
            //    (element) =>
            //    {
            //        Console.WriteLine(element.Element("auth").Element("token").Value);
            //    }, e => { Console.WriteLine(e.Message); });

            Console.ReadLine();
        }
    }
}
