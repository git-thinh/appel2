﻿using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using YoutubeExplode.Models.MediaStreams;

namespace appel
{
    public class api_media_Proxy : api_base, IAPI
    {
        static ConcurrentDictionary<string, string> m_dicProxy = null;
        static int m_port = 0;
        static HttpListener m_listener;
        static bool m_running = true;

        public api_media_Proxy()
        {
            Start();
        }

        void Start()
        {
            m_dicProxy = new ConcurrentDictionary<string, string>();

            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            m_port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();

            m_listener = new HttpListener();
            m_listener.Prefixes.Add("http://*:" + m_port + "/");
            m_listener.Start();
            //Console.WriteLine("Listening...");

            new Thread(new ParameterizedThreadStart((object lis) =>
            {
                HttpListener listener = (HttpListener)lis;
                while (m_running)
                {
                    try
                    {
                        var ctx = listener.GetContext();
                        if (ctx.Request.RawUrl == "/crossdomain.xml")
                        {
                            ctx.Response.ContentType = "text/xml";
                            string xml =
        @"<cross-domain-policy>
    <allow-access-from domain=""*.*"" headers=""SOAPAction""/>
    <allow-http-request-headers-from domain=""*.*"" headers=""SOAPAction""/> 
    <site-control permitted-cross-domain-policies=""master-only""/>
</cross-domain-policy>";
                            xml =
        @"<?xml version=""1.0""?>
<!DOCTYPE cross-domain-policy SYSTEM ""http://www.macromedia.com/xml/dtds/cross-domain-policy.dtd"">
<cross-domain-policy>
  <allow-access-from domain=""*"" />
</cross-domain-policy>";
                            byte[] bytes = Encoding.UTF8.GetBytes(xml);
                            ctx.Response.OutputStream.Write(bytes, 0, bytes.Length);
                            ctx.Response.OutputStream.Flush();
                            ctx.Response.OutputStream.Close();
                            ctx.Response.Close();
                        }
                        else
                        {
                            string key = ctx.Request.QueryString["key"];
                            if (!m_dicProxy.ContainsKey(key))
                            {
                                byte[] bytes = Encoding.UTF8.GetBytes(string.Format("Cannot find key: ", key));
                                ctx.Response.OutputStream.Write(bytes, 0, bytes.Length);
                                ctx.Response.OutputStream.Flush();
                                ctx.Response.OutputStream.Close();
                                ctx.Response.Close();
                            }
                            else
                                new Thread(new Relay(ctx).ProcessRequest).Start();
                        }
                    }
                    catch { }
                }
            })).Start(m_listener); 
        }
         
        public static string f_get_uriProxy(string mediaId, MEDIA_TYPE type)
        {
            string key = string.Format("{0}{1}", type, mediaId);
            if (m_dicProxy.ContainsKey(key))
                return string.Format("http://localhost:{0}/?key={1}", m_port, key);
            return string.Empty;
        }

        public static string f_get_uriSrcByKey(string key)
        {
            string uri = string.Empty;
            if (m_dicProxy.ContainsKey(key))
                m_dicProxy.TryGetValue(key, out uri);
            return uri;
        }

        public static void f_add_URL(string mediaId, MediaStreamInfoSet media)
        {
            string key_audio = "M4A" + mediaId;
            string key_video = "MP4" + mediaId;

            var au = media.Audio.Where(x => x.Container == Container.M4A).Take(1).SingleOrDefault();
            if (au != null)
                m_dicProxy.TryAdd(key_audio, au.Url);

            var vi = media.Muxed.Where(x => x.Container == Container.Mp4).Take(1).SingleOrDefault();
            if (vi != null)
                m_dicProxy.TryAdd(key_video, vi.Url);
        }

        public msg Execute(msg msg)
        {
            return msg;
        }

        public void Close()
        {
            m_listener.Stop();
            m_running = false;

            Thread.Sleep(10);
        }
    }

    public enum MEDIA_TYPE
    {
        AAC,
        M4A,
        MP3,
        MP4,
        WEB, // WEBM
    }

    public class Relay
    {
        private readonly HttpListenerContext originalContext;

        public Relay(HttpListenerContext originalContext)
        {
            this.originalContext = originalContext;
        }

        public void ProcessRequest()
        {
            string rawUrl = originalContext.Request.RawUrl;          
            string uri = string.Empty, key = string.Empty, type = "mp3";

            key = originalContext.Request.QueryString["key"];
            if (key.Length > 3) type = key.Substring(0, 3).ToLower();

            switch (type)
            {
                case "m4a":
                    uri = api_media_Proxy.f_get_uriSrcByKey(key);
                    break;
                case "mp4":
                    uri = api_media_Proxy.f_get_uriSrcByKey(key);
                    break;
                case "web": // webm
                    uri = api_media_Proxy.f_get_uriSrcByKey(key);
                    break;
                case "mp3":
                    uri = api_media_Proxy.f_get_uriSrcByKey(key);
                    break;
            }
            
            #region

            ////if (rawUrl == "/") rawUrl = "https://google.com.vn";  
            //if (rawUrl.Length > 3) type = rawUrl.Substring(rawUrl.Length - 3, 3).ToLower();
            //ConsoleUtilities.WriteRequest("Proxy receive a request for: " + rawUrl);

            //switch (type)
            //{
            //    case "m4a":
            //        uri = "https://r6---sn-jhjup-nbol.googlevideo.com/videoplayback?c=WEB&mn=sn-jhjup-nbol%2Csn-i3b7kn7d&mm=31%2C29&mv=m&mt=1526353247&signature=12D877BF10BDA8B8D9EB787F07C53C5E9B6BCDD2.CF50B6DE340882AC5453CBB0F10540B9D30F6E7B&ms=au%2Crdu&sparams=clen%2Cdur%2Cei%2Cgir%2Cid%2Cinitcwndbps%2Cip%2Cipbits%2Citag%2Ckeepalive%2Clmt%2Cmime%2Cmm%2Cmn%2Cms%2Cmv%2Cpcm2cms%2Cpl%2Crequiressl%2Csource%2Cexpire&ei=iU36WryBOpG-4gLMkY7YAw&ip=113.20.96.116&clen=3721767&keepalive=yes&id=o-AP0scLQEoW6xg_BDA4RnCp3bNCPg5y4hjvaLHhJnePWN&gir=yes&requiressl=yes&source=youtube&pcm2cms=yes&dur=234.289&pl=23&initcwndbps=463750&itag=140&ipbits=0&lmt=1510741503111392&expire=1526374890&key=yt6&mime=audio%2Fmp4&fvip=2";
            //        break;
            //    case "mp4":
            //        uri = "https://r6---sn-jhjup-nbol.googlevideo.com/videoplayback?key=yt6&signature=CD6655BD08EEDADA61255DE9638EADEBF9BC2DAB.640F4ED4573F543F7423F3C62699A7795A34C6AE&requiressl=yes&lmt=1510741625396835&source=youtube&dur=234.289&ipbits=0&c=WEB&initcwndbps=680000&mime=video%2Fmp4&pcm2cms=yes&sparams=dur%2Cei%2Cid%2Cinitcwndbps%2Cip%2Cipbits%2Citag%2Clmt%2Cmime%2Cmm%2Cmn%2Cms%2Cmv%2Cpcm2cms%2Cpl%2Cratebypass%2Crequiressl%2Csource%2Cexpire&id=o-AOpvYdf_hpR_jCGsytRQ4p_2uICpZxVqqewAyrpM1_U9&mm=31%2C29&mn=sn-jhjup-nbol%2Csn-i3beln7z&pl=23&ip=113.20.96.116&ei=mFD6WoeNDNOb4AK0squACA&ms=au%2Crdu&mt=1526354019&mv=m&ratebypass=yes&fvip=6&expire=1526375672&itag=22";
            //        break;
            //    case "ebm": // webm
            //        uri = "https://r6---sn-jhjup-nbol.googlevideo.com/videoplayback?gir=yes&key=yt6&signature=64933C7570840B48D0E3702A51200EF12DB71456.AA4398BD234730DA07841DAF7FDA6B7A2B341963&requiressl=yes&lmt=1510742527754463&source=youtube&dur=0.000&clen=15660856&ipbits=0&c=WEB&initcwndbps=680000&mime=video%2Fwebm&pcm2cms=yes&sparams=clen%2Cdur%2Cei%2Cgir%2Cid%2Cinitcwndbps%2Cip%2Cipbits%2Citag%2Clmt%2Cmime%2Cmm%2Cmn%2Cms%2Cmv%2Cpcm2cms%2Cpl%2Cratebypass%2Crequiressl%2Csource%2Cexpire&id=o-AOpvYdf_hpR_jCGsytRQ4p_2uICpZxVqqewAyrpM1_U9&mm=31%2C29&mn=sn-jhjup-nbol%2Csn-i3beln7z&pl=23&ip=113.20.96.116&ei=mFD6WoeNDNOb4AK0squACA&ms=au%2Crdu&mt=1526354019&mv=m&ratebypass=yes&fvip=6&expire=1526375672&itag=43";
            //        break;
            //    //case "mp3":
            //    default:
            //        uri = "https://drive.google.com/uc?export=download&id=1u2wJYTB-hVWeZOLLd9CxcA9KCLuEanYg";
            //        break;
            //}

            #endregion

            try
            {
                var relayRequest = (HttpWebRequest)WebRequest.Create(uri);
                relayRequest.KeepAlive = false;
                relayRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;
                relayRequest.UserAgent = this.originalContext.Request.UserAgent;
                var requestData = new RequestState(relayRequest, originalContext);

                switch (type)
                {
                    case "m4a":
                        requestData.context.Response.ContentType = "audio/x-m4a";
                        break;
                    case "mp4":
                        requestData.context.Response.ContentType = "video/mp4";
                        break;
                    case "web": // webm
                        requestData.context.Response.ContentType = "video/webm"; // audio/webm
                        break;
                    case "mp3":
                        requestData.context.Response.ContentType = "audio/mpeg";
                        break;
                }

                relayRequest.BeginGetResponse(ResponseCallBack, requestData);
            }
            catch { }
        }

        private static void ResponseCallBack(IAsyncResult asynchronousResult)
        {
            var requestData = (RequestState) asynchronousResult.AsyncState;
            ConsoleUtilities.WriteResponse("Proxy receive a response from " + requestData.context.Request.RawUrl);
            
            using (var responseFromWebSiteBeingRelayed = (HttpWebResponse) requestData.webRequest.EndGetResponse(asynchronousResult))
            {
                using (var responseStreamFromWebSiteBeingRelayed = responseFromWebSiteBeingRelayed.GetResponseStream())
                {
                    var originalResponse = requestData.context.Response; 


                    if (responseFromWebSiteBeingRelayed.ContentType.Contains("text/html"))
                    {
                        var reader = new StreamReader(responseStreamFromWebSiteBeingRelayed);
                        string html = reader.ReadToEnd();
                        //Here can modify html
                        byte[] byteArray = System.Text.Encoding.Default.GetBytes(html);
                        var stream = new MemoryStream(byteArray);
                        stream.CopyTo(originalResponse.OutputStream);
                    }
                    else
                    {
                        requestData.context.Response.ContentType = "video/mp4";
                        responseStreamFromWebSiteBeingRelayed.CopyTo(originalResponse.OutputStream);
                    }
                    originalResponse.OutputStream.Close();
                }
            }
        }
    }

    public static class ConsoleUtilities
    {
        public static void WriteRequest(string info)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(info);
            Console.ResetColor();
        }
        public static void WriteResponse(string info)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(info);
            Console.ResetColor();
        }
    }

    public class RequestState
    {
        public readonly HttpWebRequest webRequest;
        public readonly HttpListenerContext context;

        public RequestState(HttpWebRequest request, HttpListenerContext context)
        {
            webRequest = request;
            this.context = context;
        }
    }

}