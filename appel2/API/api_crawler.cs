using HtmlAgilityPack;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace appel
{

    public class api_crawler : api_base, IAPI
    {
        static readonly object lockHtml = new object();
        static Dictionary<string, string> dicHtml = new Dictionary<string, string>();

        static readonly object lockUri = new object();
        static List<oLink> listUri = new List<oLink>();

        static Dictionary<string, string> dicSettingCurrent = new Dictionary<string, string>();





        static int crawlCounter = 0;

        static readonly object lockContent = new object();
        static Dictionary<string, string> dicContent = new Dictionary<string, string>();
        static int contentCounter = 0;

        public bool Open { get; set; } = false;

        public api_crawler()
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 1000;
            // active SSL 1.1, 1.2, 1.3 for WebClient request HTTPS
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | (SecurityProtocolType)3072 | (SecurityProtocolType)0x00000C00 | SecurityProtocolType.Tls;
        }

        public void Init()
        {
        }
        public msg Execute(msg m)
        {
            if (m == null || m.Input == null) return m;
            string url_input = string.Empty, path_package;
            HttpWebRequest w;
            HtmlDocument doc = new HtmlDocument();
            switch (m.KEY)
            {
                case _API.CRAWLER_KEY_REGISTER_PATH:
                    #region
                    if (m.Input != null)
                    {
                        oLinkSetting olink = (oLinkSetting)m.Input;
                        url_input = olink.Url;
                        dicSettingCurrent = olink.Settings;

                        //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 
                        //    | (SecurityProtocolType)3072 
                        //    | (SecurityProtocolType)0x00000C00 
                        //    | SecurityProtocolType.Tls;

                        w = (HttpWebRequest)WebRequest.Create(new Uri(url_input));
                        w.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36";
                        w.BeginGetResponse(asyncResult =>
                        {
                            HttpWebResponse rs = (HttpWebResponse)w.EndGetResponse(asyncResult); //add a break point here 
                            StreamReader sr = new StreamReader(rs.GetResponseStream(), Encoding.UTF8);
                            string htm = sr.ReadToEnd();
                            sr.Close();
                            rs.Close();

                            if (!string.IsNullOrEmpty(htm))
                            {
                                htm = HttpUtility.HtmlDecode(htm);
                                htm = format_HTML(htm);

                                string url = rs.ResponseUri.ToString();
                                string[] url_crawled;
                                lock (lockHtml)
                                {
                                    if (dicHtml.ContainsKey(url) == false)
                                        dicHtml.Add(url, htm);
                                    url_crawled = dicHtml.Keys.ToArray();
                                }

                                doc.LoadHtml(htm);

                                lock (lockUri)
                                {
                                    string[] auri = url.Split('/');
                                    string uri_root = string.Join("/", auri.Where((x, k) => k < 3).ToArray());

                                    var url_new = doc.DocumentNode
                                        .SelectNodes("//a")
                                        .Select(p => new oLink()
                                        {
                                            crawled = false,
                                            uri = uri_root,
                                            url = p.GetAttributeValue("href", string.Empty).ToLower(),
                                            text = p.InnerText
                                        })
                                        .Where(x => x.url.Length > 1 && x.url[0] != '#')
                                        .ToArray();
                                    ;
                                    foreach (var li in url_new)
                                        if (li.url[0] == '/')
                                            li.url = uri_root + li.url;
                                        else if (li.url[0] != 'h')
                                            li.url = uri_root + "/" + li.url;

                                    var urls = url_new
                                         .Where(x =>
                                                 x.url.IndexOf(x.uri) == 0
                                                 && !listUri.Any(z => z.url == x.url)
                                                 && x.url != url
                                                 && x.text != string.Empty)
                                         .GroupBy(x => x.url)
                                         .Select(x => x.First())
                                         .ToArray();

                                    //string START_URL = string.Empty;
                                    //if (dicSettingCurrent.TryGetValue("START_URL", out START_URL) && !string.IsNullOrEmpty(START_URL))
                                    //url_new = url_new.Where(x => x.url.StartsWith(START_URL)).ToArray();

                                    listUri.Add(new oLink() { crawled = true, url = url, uri = url, text = string.Empty });
                                    if (urls.Length > 0)
                                    {
                                        listUri.AddRange(urls);
                                        Execute(new msg() { API = _API.CRAWLER, KEY = _API.CRAWLER_KEY_REQUEST_LINK, Input = urls[0] });
                                    }
                                }
                            }
                        }, w);
                    }
                    #endregion
                    break;
                case _API.CRAWLER_KEY_REQUEST_LINK:
                    #region
                    if (m.Input != null)
                    {
                        Interlocked.Increment(ref crawlCounter);

                        oLink link = (oLink)m.Input;
                        //m.Log = "CRAWLER_KEY_REQUEST_LINK: " + link.url;

                        //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 
                        //    | (SecurityProtocolType)3072 
                        //    | (SecurityProtocolType)0x00000C00 
                        //    | SecurityProtocolType.Tls;

                        w = (HttpWebRequest)WebRequest.Create(new Uri(link.url));
                        w.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36";
                        w.BeginGetResponse(asyncResult =>
                        {
                            HttpWebResponse rs = (HttpWebResponse)w.EndGetResponse(asyncResult); //add a break point here 
                            StreamReader sr = new StreamReader(rs.GetResponseStream(), Encoding.UTF8);
                            string htm = sr.ReadToEnd();
                            sr.Close();
                            rs.Close();

                            if (!string.IsNullOrEmpty(htm))
                            {
                                htm = HttpUtility.HtmlDecode(htm);
                                htm = format_HTML(htm);

                                string url = rs.ResponseUri.ToString();
                                string[] url_crawled;

                                lock (lockHtml)
                                {
                                    if (dicHtml.ContainsKey(url) == false)
                                    {
                                        dicHtml.Add(url, htm);
                                        Interlocked.Increment(ref contentCounter);

                                        response_toMain(new msg() { API = _API.CRAWLER, KEY = _API.CRAWLER_KEY_REQUEST_LINK, Log = m.KEY + ": " + contentCounter.ToString() + "|" + listUri.Count + " = " + url });
                                    }
                                    url_crawled = dicHtml.Keys.ToArray();
                                }

                                /////////////////////////////////////////
                                //f_responseToMain(m);

                                doc.LoadHtml(htm);

                                lock (lockUri)
                                {
                                    var url_new = doc.DocumentNode
                                        .SelectNodes("//a")
                                        .Select(p => new oLink()
                                        {
                                            uri = link.uri,
                                            crawled = false,
                                            url = p.GetAttributeValue("href", string.Empty).ToLower(),
                                            text = p.InnerText
                                        })
                                        .Where(x => x.url.Length > 1 && x.url[0] != '#')
                                        .ToArray(); 
                                    ;

                                    foreach (var lx in url_new)
                                        if (lx.url[0] == '/')
                                            lx.url = link.uri + lx.url;
                                        else if (lx.url[0] != 'h')
                                            lx.url = link.uri + "/" + lx.url;

                                    var urls = url_new
                                        .Where(x =>
                                                x.url.IndexOf(x.uri) == 0
                                                && !listUri.Any(z => z.url == x.url)
                                                && x.url != url
                                                && x.text != string.Empty)
                                        .GroupBy(x => x.url)
                                        .Select(x => x.First())
                                        .ToArray();

                                    //var div_con = doc.DocumentNode 
                                    //    .SelectNodes("//article")
                                    //    .ToArray();
                                    //if (div_con.Length > 0)
                                    //{
                                    //    string con = div_con[0].InnerHtml;
                                    //    lock (lockContent)
                                    //    {
                                    //        if (dicContent.ContainsKey(url) == false)
                                    //            dicContent.Add(url, con);
                                    //    }
                                    //    Interlocked.Increment(ref contentCounter);

                                    //    f_responseToMain(new msg() { API = _API.CRAWLER, KEY = _API.CRAWLER_KEY_REQUEST_LINK, Log = con });
                                    //}

                                    int index = listUri.IndexOf(link);
                                    if (index != -1) listUri[index].crawled = true;

                                    if (urls.Length > 0)
                                        listUri.AddRange(urls);

                                    string START_URL = string.Empty;
                                    if (dicSettingCurrent.TryGetValue("START_URL", out START_URL) && !string.IsNullOrEmpty(START_URL))
                                        listUri = listUri.Where(x => x.url.StartsWith(START_URL)).ToList();

                                    var li = listUri.Where(x => x.crawled == false).Take(1).SingleOrDefault();
                                    if (li == null)
                                    {
                                        Uri uri_ = new Uri(link.uri);
                                        string fi_name = uri_.Host + ".bin";
                                        if (File.Exists(fi_name)) File.Delete(fi_name);

                                        lock (lockHtml)
                                        {
                                            using (var file = File.Create(fi_name))
                                                Serializer.Serialize<Dictionary<string, string>>(file, dicHtml);
                                        }

                                        response_toMain(new msg() { API = _API.CRAWLER, KEY = _API.CRAWLER_KEY_REQUEST_LINK_COMPLETE, Input = listUri.Select(x => x.url).ToArray() });
                                    }
                                    else
                                    {
                                        Execute(new msg() { API = _API.CRAWLER, KEY = _API.CRAWLER_KEY_REQUEST_LINK, Input = li });
                                    }
                                }
                            }
                        }, w);
                    }

                    #endregion
                    break;
                case _API.CRAWLER_KEY_CONVERT_PACKAGE_TO_HTML:
                    #region
                    path_package = (string)m.Input;
                    if (!string.IsNullOrEmpty(path_package) && File.Exists(path_package))
                    {
                        var dicRaw = new Dictionary<string, string>();
                        var dicCon = new Dictionary<string, string>();
                        var list_XPath = new List<string>();

                        using (var fileStream = File.OpenRead(path_package))
                            dicRaw = Serializer.Deserialize<Dictionary<string, string>>(fileStream);

                        //foreach (var kv in dicRaw)
                        //{
                        //    string s = kv.Value;
                        //    doc = new HtmlDocument();
                        //    doc.LoadHtml(s);
                        //    foreach (var h1 in doc.DocumentNode.SelectNodes("//h1"))
                        //    {
                        //        //d1.Add(kv.Key, h1.ParentNode.InnerText);
                        //        //d2.Add(kv.Key, h1.ParentNode.ParentNode.InnerText);
                        //        //d3.Add(kv.Key, h1.ParentNode.ParentNode.ParentNode.InnerText);
                        //        list_XPath.Add(h1.XPath);
                        //        break;
                        //    }
                        //}

                        foreach (var kv in dicRaw)
                        {
                            string s = kv.Value, si = string.Empty;
                            doc = new HtmlDocument();
                            doc.LoadHtml(s);
                            var ns = doc.DocumentNode.SelectNodes("/html[1]/body[1]/div[3]/article[1]/div[1]/div[1]/div[1]/div[1]/article[1]");
                            if (ns != null && ns.Count > 0)
                            {
                                si = ns[0].InnerHtml;
                                dicCon.Add(kv.Key, si);
                            }
                        }

                        using (var file = File.Create("crawler.htm.bin"))
                            Serializer.Serialize<Dictionary<string, string>>(file, dicCon);

                    }
                    #endregion
                    break;
                case _API.CRAWLER_KEY_CONVERT_PACKAGE_TO_TEXT:
                    #region
                    path_package = (string)m.Input;
                    if (!string.IsNullOrEmpty(path_package) && File.Exists(path_package))
                    {
                        var dicRaw = new Dictionary<string, string>();
                        var dicText = new Dictionary<string, string>();

                        using (var fileStream = File.OpenRead(path_package))
                            dicRaw = Serializer.Deserialize<Dictionary<string, string>>(fileStream);

                        foreach (var kv in dicRaw)
                        {
                            string s = new htmlToText().ConvertHtml(kv.Value).Trim();
                            dicText.Add(kv.Key, s);
                        }

                        using (var file = File.Create("crawler.txt.bin"))
                            Serializer.Serialize<Dictionary<string, string>>(file, dicText);

                    }
                    #endregion
                    break;
            }

            m.Output.Ok = true;
            m.Output.Data = null;
            return m;
        }

        public void Close()
        {
        }

        public static string getHtml(string url)
        {
            string s = string.Empty;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | (SecurityProtocolType)3072 | SecurityProtocolType.Tls;

            //using (WebClient w = new WebClient())
            //{ 
            //    w.Encoding = Encoding.UTF7;
            //    s = w.DownloadString(url);
            //}

            //HttpWebRequest w = (HttpWebRequest)WebRequest.Create(url);
            //w.Method = "GET";
            //w.KeepAlive = false;
            //WebResponse rs = w.GetResponse();
            //StreamReader sr = new StreamReader(rs.GetResponseStream(), System.Text.Encoding.UTF8);
            //s = sr.ReadToEnd();
            //sr.Close();
            //rs.Close();

            //            var uri = new Uri(url);
            //            string req =
            //@"GET " + uri.PathAndQuery + @" HTTP/1.1
            //User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36
            //Host: " + uri.Host + @"
            //Accept: */*
            //Accept-Encoding: gzip, deflate
            //Connection: Keep-Alive 
            //";
            //            var requestBytes = Encoding.UTF8.GetBytes(req);
            //            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //            socket.Connect(uri.Host, 80);
            //            if (socket.Connected)
            //            {
            //                socket.Send(requestBytes);
            //                var responseBytes = new byte[socket.ReceiveBufferSize];
            //                socket.Receive(responseBytes);
            //                s = Encoding.UTF8.GetString(responseBytes);
            //            }
            //            s = HttpUtility.HtmlDecode(s);
            //result = CleanHTMLFromScript(result);

            //HttpWebRequest w = (HttpWebRequest)WebRequest.Create(new Uri(url));
            //w.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36";
            //w.BeginGetResponse(asyncResult =>
            //{
            //    HttpWebResponse rs = (HttpWebResponse)w.EndGetResponse(asyncResult); //add a break point here 
            //    StreamReader sr = new StreamReader(rs.GetResponseStream(), System.Text.Encoding.UTF8);
            //    s = sr.ReadToEnd();
            //    sr.Close();
            //    rs.Close();
            //    s = HttpUtility.HtmlDecode(s);
            //}, w);

            return s;
        }

        private static string format_HTML(string s)
        {
            string si = string.Empty;
            s = Regex.Replace(s, @"<script[^>]*>[\s\S]*?</script>", string.Empty);
            s = Regex.Replace(s, @"<style[^>]*>[\s\S]*?</style>", string.Empty);
            s = Regex.Replace(s, @"<noscript[^>]*>[\s\S]*?</noscript>", string.Empty);
            s = Regex.Replace(s, @"(?s)(?<=<!--).+?(?=-->)", string.Empty).Replace("<!---->", string.Empty);

            //s = Regex.Replace(s, @"<noscript[^>]*>[\s\S]*?</noscript>", string.Empty);
            //s = Regex.Replace(s, @"<noscript[^>]*>[\s\S]*?</noscript>", string.Empty);
            s = Regex.Replace(s, @"</?(?i:embed|object|frameset|frame|iframe|meta|link)(.|\n|\s)*?>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);


            string[] lines = s.Split(new char[] { '\r', '\n' }, StringSplitOptions.None).Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
            s = string.Join(Environment.NewLine, lines);
            return s;

            //HtmlDocument doc = new HtmlDocument();
            //doc.LoadHtml(s);
            //string tagName = string.Empty, tagVal = string.Empty;
            //foreach (var node in doc.DocumentNode.SelectNodes("//*"))
            //{
            //    if (node.InnerText == null || node.InnerText.Trim().Length == 0)
            //    {
            //        node.Remove();
            //        continue;
            //    }

            //    tagName = node.Name.ToUpper();
            //    if (tagName == "A")
            //        tagVal = node.GetAttributeValue("href", string.Empty);
            //    else if (tagName == "IMG")
            //        tagVal = node.GetAttributeValue("src", string.Empty);

            //    //node.Attributes.RemoveAll();
            //    node.Attributes.RemoveAll_NoRemoveClassName();

            //    if (tagVal != string.Empty)
            //    {
            //        if (tagName == "A") node.SetAttributeValue("href", tagVal);
            //        else if (tagName == "IMG") node.SetAttributeValue("src", tagVal);
            //    }
            //}

            //si = doc.DocumentNode.OuterHtml;
            ////string[] lines = si.Split(new char[] { '\r', '\n' }, StringSplitOptions.None).Where(x => x.Trim().Length > 0).ToArray();
            //string[] lines = si.Split(new char[] { '\r', '\n' }, StringSplitOptions.None).Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
            //si = string.Join(Environment.NewLine, lines);
            //return si;
        }

    }

}
