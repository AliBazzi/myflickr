using System;
using System.Collections.Generic;
using MyFlickr.Core;
using MyFlickr.Rest;
using MyFlickr.Rest.Synchronous;
namespace APITest
{
    class Program
    {
        static void Main(string[] args)
        {
            var res = new Authenticator("fa8abd96654642183131486ba3ae9695", "e820b2d523ce547e").CheckToken("fgdgfdgfdgdfgdg");
            //Authenticator.GetFrobCompleted += new EventHandler<EventArgs<GetFrobResult>>(
            //    (obj, e) => 
            //    {
            //        Authenticator.GetAuthenticationTokenCompleted+=new EventHandler<EventArgs<GetTokenResult>>(
            //            (sender,arg)=>
            //                Console.WriteLine(arg.Result.AuthinticationTokens.Token));
            //        Console.WriteLine(e.Result.Frob);
            //        System.Diagnostics.Process.Start(e.Result.Url.ToString());
            //        Console.ReadLine();
            //        Authenticator.GetTokenAsync("fa8abd96654642183131486ba3ae9695", "e820b2d523ce547e", e.Result.Frob);
            //    });
            //var res = Authenticator.GetFrobAsync("fa8abd96654642183131486ba3ae9695", "e820b2d523ce547e", AccessPermission.Read);
            //1 - get frob 
            //2 - build the URL , and waits authorization ...

            //var frob = "";
            //FlickrCore.IntiateRequest((element) =>
            //    {
            //        Console.WriteLine(element.Element("frob").Value);
            //        frob = element.Element("frob").Value;
            //        System.Diagnostics.Process.Start(
            //            UriHelper.CalculateRedirectionUrl("e820b2d523ce547e",new Parameter("api_key", "fa8abd96654642183131486ba3ae9695"),
            //        new Parameter("perms", "write"),
            //        new Parameter("frob", element.Element("frob").Value)).ToString());
            //    }, e => { Console.WriteLine(e.Message); },
            //    "e820b2d523ce547e",
            //    new Parameter("method","flickr.auth.getFrob"), new Parameter("api_key", "fa8abd96654642183131486ba3ae9695") );

           

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
