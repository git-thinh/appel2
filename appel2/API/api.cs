using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using ProtoBuf;
using System.Net;
using HtmlAgilityPack;
using System.Net.Sockets;
using System.Web;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using NAudio.Wave;
using YoutubeExplode.Models.MediaStreams;
using YoutubeExplode.Internal;
//using Fizzler.Systems.HtmlAgilityPack;

namespace appel
{
    public class _API
    {
        public const string MP3 = "MP3";
        public const string MP3_PLAY = "MP3_PLAY";

        public const string YOUTUBE = "YOUTUBE";
        public const string YOUTUBE_INFO = "YOUTUBE_INFO";

        public const string SETTING_APP = "SETTING_APP";
        public const string SETTING_APP_KEY_INT = "SETTING_APP_KEY_INT";
        public const string SETTING_APP_KEY_UPDATE_FOLDER = "SETTING_APP_KEY_UPDATE_FOLDER";
        public const string SETTING_APP_KEY_UPDATE_SIZE = "SETTING_APP_KEY_UPDATE_SIZE";
        public const string SETTING_APP_KEY_UPDATE_NODE_OPENING = "SETTING_APP_KEY_UPDATE_NODE_OPENING";

        public const string CRAWLER = "CRAWLER";
        public const string CRAWLER_KEY_REGISTER_PATH = "CRAWLER_KEY_REGISTER_PATH";
        public const string CRAWLER_KEY_REQUEST_LINK = "CRAWLER_KEY_REQUEST_LINK";
        public const string CRAWLER_KEY_CONVERT_HTML_TO_TEXT = "CRAWLER_KEY_CONVERT_HTML_TO_TEXT";
        public const string CRAWLER_KEY_CONVERT_PACKAGE_TO_HTML = "CRAWLER_KEY_CONVERT_PACKAGE_TO_HTML";
        public const string CRAWLER_KEY_CONVERT_PACKAGE_TO_TEXT = "CRAWLER_KEY_CONVERT_PACKAGE_TO_TEXT";
        public const string CRAWLER_KEY_COMPLETE = "CRAWLER_KEY_COMPLETE";

        public const string CONTENT = "CONTENT";
        public const string CONTENT_KEY_ANALYTIC = "CONTENT_KEY_ANALYTIC";
        public const string CONTENT_KEY_EDIT_ROLE = "CONTENT_KEY_EDIT_ROLE";


        public const string FOLDER_ANYLCTIC = "FOLDER_ANYLCTIC";


        public const string TRANSLATER = "TRANSLATOR";
        public const string WORD_LOAD_LOCAL = "WORD_LOAD_LOCAL";
        public const string WORD_DOWNLOAD = "WORD_DOWNLOAD";
    }

    public interface IFORM
    {
        void api_responseMsg(object sender, threadMsgEventArgs e);
        void api_initMsg(msg m);
    }

    public interface IAPI
    {
        msg Execute(msg msg);
        void Close();
    }

    public class api_base
    {
        static readonly object _lock = new object();
        static Queue<msg> cache = new Queue<msg>();
        static System.Threading.Timer timer = null;
        static IFORM fom = null;

        public api_base()
        {
            if (fom == null)
                fom = app.get_Main();
            if (timer == null)
            {
                timer = new System.Threading.Timer(new System.Threading.TimerCallback((obj) =>
                {
                    lock (_lock)
                    {
                        if (cache.Count > 0)
                        {
                            msg m = cache.Dequeue();
                            if (fom != null) fom.api_responseMsg(null, new threadMsgEventArgs(m));
                        }
                    }
                }), fom, 100, 100);
            }
        }

        public void f_responseToMain(msg m)
        {
            lock (_lock) cache.Enqueue(m);
        }

        public void f_api_Inited(msg m)
        {
            if (fom != null) fom.api_initMsg(m);
        }
    }

    public class api_nodeStore : api_base, IAPI
    {
        static readonly object _lock = new object();
        static Dictionary<long, oNode> dicItems = new Dictionary<long, oNode>();

        public static void Add(oNode node)
        {
            lock (_lock)
                dicItems.Add(node.id, node);
        }
        public static void Adds(oNode[] nodes)
        {
            lock (_lock)
            {
                for (int i = 0; i < nodes.Length; i++)
                    if (!dicItems.ContainsKey(nodes[i].id))
                        dicItems.Add(nodes[i].id, nodes[i]);
            }
        }

        public static oNode Get(long id)
        {
            lock (_lock)
                if (dicItems.ContainsKey(id))
                    return dicItems[id];
            return null;
        }

        public msg Execute(msg msg)
        {
            return msg;
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

    }

    public class api_fetch : api_base, IAPI
    {
        static readonly object _lock = new object();
        static Dictionary<string, List<string>> dicUrl = new Dictionary<string, List<string>>();

        public msg Execute(msg m)
        {
            if (m == null || m.Input == null) return m;


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

            m.Output.Ok = true;
            m.Output.Data = null;
            return m;
        }

        public void Close()
        {
        }
    }

    public class api_crawler : api_base, IAPI
    {
        static readonly object lockHtml = new object();
        static Dictionary<string, string> dicHtml = new Dictionary<string, string>();

        static readonly object lockUri = new object();
        static List<oLink> listUri = new List<oLink>();

        static int crawlCounter = 0;

        static readonly object lockContent = new object();
        static Dictionary<string, string> dicContent = new Dictionary<string, string>();
        static int contentCounter = 0;

        public api_crawler()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                | (SecurityProtocolType)3072
                | (SecurityProtocolType)0x00000C00
                | SecurityProtocolType.Tls;
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
                    url_input = (string)m.Input;
                    url_input = url_input.ToLower();
                    m.Log = "CRAWLER_KEY_REGISTER_PATH: " + url_input;

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

                            string url = rs.ResponseUri.ToString();
                            string[] url_crawled;
                            lock (lockHtml)
                            {
                                if (dicHtml.ContainsKey(url) == false)
                                {
                                    string htm_format = format_HTML(htm);
                                    dicHtml.Add(url, htm_format);
                                }
                                url_crawled = dicHtml.Keys.ToArray();
                            }

                            doc.LoadHtml(htm);

                            lock (lockUri)
                            {
                                var url_new = doc.DocumentNode
                                    .SelectNodes("//a")
                                    .Select(p => new oLink()
                                    {
                                        crawled = false,
                                        uri = url,
                                        url = p.GetAttributeValue("href", string.Empty).ToLower(),
                                        text = p.InnerText
                                    })
                                    .Where(x =>
                                            x.url.IndexOf(x.uri) == 0
                                            && !listUri.Any(z => z.url == x.url)
                                            && x.url != url
                                            && x.text != string.Empty)
                                    .GroupBy(x => x.url)
                                    .Select(x => x.First())
                                    .ToArray();

                                listUri.Add(new oLink() { crawled = true, url = url, uri = url, text = string.Empty });
                                if (url_new.Length > 0)
                                {
                                    listUri.AddRange(url_new);
                                    Execute(new msg() { API = _API.CRAWLER, KEY = _API.CRAWLER_KEY_REQUEST_LINK, Input = url_new[0] });
                                }
                            }
                        }
                    }, w);
                    #endregion
                    break;
                case _API.CRAWLER_KEY_REQUEST_LINK:
                    #region
                    Interlocked.Increment(ref crawlCounter);

                    oLink link = (oLink)m.Input;
                    m.Log = "CRAWLER_KEY_REQUEST_LINK: " + link.url;

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

                            string url = rs.ResponseUri.ToString();
                            string[] url_crawled;

                            lock (lockHtml)
                            {
                                if (dicHtml.ContainsKey(url) == false)
                                {
                                    string htm_format = format_HTML(htm);
                                    dicHtml.Add(url, htm_format);
                                    Interlocked.Increment(ref contentCounter);

                                    f_responseToMain(new msg() { API = _API.CRAWLER, KEY = _API.CRAWLER_KEY_COMPLETE, Log = m.KEY + ": " + contentCounter.ToString() + " = " + url });
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

                                if (url_new.Length > 0)
                                    listUri.AddRange(url_new);

                                var li = listUri.Where(x => x.crawled == false).Take(1).SingleOrDefault();
                                if (li == null || contentCounter == 50)
                                {
                                    f_responseToMain(new msg() { API = _API.CRAWLER, KEY = _API.CRAWLER_KEY_COMPLETE, Log = "FINISH CWRALER ..." });
                                    lock (lockHtml)
                                    {
                                        using (var file = File.Create("crawler.raw.bin"))
                                            Serializer.Serialize<Dictionary<string, string>>(file, dicHtml);
                                    }
                                }
                                else
                                {
                                    Execute(new msg() { API = _API.CRAWLER, KEY = _API.CRAWLER_KEY_REQUEST_LINK, Input = li });
                                }
                            }
                        }
                    }, w);
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

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(s);
            string tagName = string.Empty, tagVal = string.Empty;
            foreach (var node in doc.DocumentNode.SelectNodes("//*"))
            {
                if (node.InnerText == null || node.InnerText.Trim().Length == 0)
                {
                    node.Remove();
                    continue;
                }

                tagName = node.Name.ToUpper();
                if (tagName == "A")
                    tagVal = node.GetAttributeValue("href", string.Empty);
                else if (tagName == "IMG")
                    tagVal = node.GetAttributeValue("src", string.Empty);

                node.Attributes.RemoveAll();

                if (tagVal != string.Empty)
                {
                    if (tagName == "A") node.SetAttributeValue("href", tagVal);
                    else if (tagName == "IMG") node.SetAttributeValue("src", tagVal);
                }
            }

            si = doc.DocumentNode.OuterHtml;
            //string[] lines = si.Split(new char[] { '\r', '\n' }, StringSplitOptions.None).Where(x => x.Trim().Length > 0).ToArray();
            string[] lines = si.Split(new char[] { '\r', '\n' }, StringSplitOptions.None).Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
            si = string.Join(Environment.NewLine, lines);
            return si;
        }
    }

    public class api_settingApp : api_base, IAPI
    {
        const string file_name = "setting.bin";
        static readonly object _lock = new object();
        static oSetting _setting = new oSetting() { };

        public api_settingApp()
        {
            if (File.Exists(file_name))
            {
                lock (_lock)
                {
                    using (var file = File.OpenRead(file_name))
                    {
                        _setting = Serializer.Deserialize<oSetting>(file);
                        string path_pkg = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "package");
                        if (Directory.Exists(path_pkg))
                        {
                            var ns = Directory.GetFiles(path_pkg, "*.pkg")
                                .Select(x => new oNode()
                                {
                                    anylatic = false,
                                    name = Path.GetFileName(x).Substring(0, Path.GetFileName(x).Length - 4),
                                    content = string.Empty,
                                    path = x,
                                    title = Path.GetFileName(x).Substring(0, Path.GetFileName(x).Length - 4),
                                    type = oNodeType.PACKAGE,
                                })
                                .ToArray();
                            api_nodeStore.Adds(ns);
                            _setting.list_package = ns.Select(x => x.id).ToList();
                        }

                        string path_book = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "book");
                        if (Directory.Exists(path_book))
                        {
                            //Directory.GetFiles(path, "*.txt, *.html")
                            var ps = "*.txt|*.pdf|*.html|*.htm|*.doc|*.docx|*.xls|*.xlsx|*.ppt|*.pptx".Split('|')
                             .SelectMany(filter => System.IO.Directory.GetFiles(path_book, filter))
                             .Select(x => node_Parse(x)).Where(x => x != null).ToArray();
                            api_nodeStore.Adds(ps);
                            _setting.list_book = ps.Select(x => x.id).ToList();
                        }

                        _setting.list_folder = new List<string>() { @"E:\data_el2\articles-IT\w2ui" };

                        f_api_Inited(new msg() { API = _API.SETTING_APP, KEY = _API.SETTING_APP_KEY_INT });
                    }
                }
            }
        }

        private oNode node_Parse(string path_file)
        {
            if (!File.Exists(path_file)) return null;

            oNode node = new oNode();
            node.path = path_file;
            node.name = Path.GetFileName(path_file);

            switch (Path.GetExtension(path_file).ToLower())
            {
                case ".txt":
                    node.type = oNodeType.TEXT;
                    node.content = File.ReadAllText(path_file);
                    node.title = node.content.Split(new char[] { '\r', '\n' })[0];
                    break;
                case ".pdf":
                    node.type = oNodeType.PDF;
                    node.title = node.name.Substring(0, node.name.Length - 4).Trim();
                    break;
                case ".html":
                    node.type = oNodeType.HTML;
                    node.title = node.name.Substring(0, node.name.Length - 5).Trim();
                    break;
                case ".htm":
                    node.type = oNodeType.HTM;
                    node.title = node.name.Substring(0, node.name.Length - 4).Trim();
                    break;
                case ".doc":
                    node.type = oNodeType.DOC;
                    node.title = node.name.Substring(0, node.name.Length - 4).Trim();
                    break;
                case ".docx":
                    node.type = oNodeType.DOCX;
                    node.title = node.name.Substring(0, node.name.Length - 5).Trim();
                    break;
                case ".xls":
                    node.type = oNodeType.XLS;
                    node.title = node.name.Substring(0, node.name.Length - 4).Trim();
                    break;
                case ".xlsx":
                    node.type = oNodeType.XLSX;
                    node.title = node.name.Substring(0, node.name.Length - 5).Trim();
                    break;
                case ".ppt":
                    node.type = oNodeType.PPT;
                    node.title = node.name.Substring(0, node.name.Length - 4).Trim();
                    break;
                case ".pptx":
                    node.type = oNodeType.PPTX;
                    node.title = node.name.Substring(0, node.name.Length - 5).Trim();
                    break;
                default:
                    node = null;
                    break;
            }
            return node;
        }

        public static long[] get_package()
        {
            lock (_lock)
                return _setting.list_package.ToArray();
        }

        public static long[] get_book()
        {
            lock (_lock)
                return _setting.list_book.ToArray();
        }

        public static bool get_checkExistFolder(string fol)
        {
            lock (_lock)
                return _setting.list_folder.IndexOf(fol) != -1;
        }

        public static string[] get_listFolder()
        {
            lock (_lock)
                return _setting.list_folder.ToArray();
        }

        public static oNode get_nodeOpening()
        {
            lock (_lock)
                return api_nodeStore.Get(_setting.node_opening);
        }

        public msg Execute(msg m)
        {
            if (m == null || m.Input == null) return m;
            bool hasUpdate = false;

            switch (m.KEY)
            {
                case _API.SETTING_APP_KEY_UPDATE_FOLDER:
                    #region
                    string fol = (string)m.Input;
                    if (!string.IsNullOrEmpty(fol))
                    {
                        fol = fol.ToLower().Trim();
                        lock (_lock)
                        {
                            if (_setting.list_folder.IndexOf(fol) == -1)
                            {
                                _setting.list_folder.Add(fol);
                                hasUpdate = true;
                                app.postMessageToService(new msg() { API = _API.FOLDER_ANYLCTIC, Input = fol });
                            }
                        }
                    }
                    #endregion
                    break;
                case _API.SETTING_APP_KEY_UPDATE_NODE_OPENING:
                    oNode node = (oNode)m.Input;
                    lock (_lock)
                        _setting.node_opening = node.id;
                    hasUpdate = true;
                    break;
                case _API.SETTING_APP_KEY_UPDATE_SIZE:
                    oAppSize app_size = (oAppSize)m.Input;
                    lock (_lock)
                    {
                        _setting.app_size = app_size;
                        hasUpdate = true;
                    }
                    break;
            }

            //if (hasUpdate)
            //{
            //    using (var file = File.Create(file_name))
            //    {
            //        Serializer.Serialize<oSetting>(file, _setting);
            //    }
            //}
            m.Output.Ok = hasUpdate;
            m.Output.Data = hasUpdate;
            return m;
        }

        public void Close()
        {
            using (var file = File.Create(file_name))
            {
                Serializer.Serialize<oSetting>(file, _setting);
            }
        }
    }

    public class api_Translater : api_base, IAPI
    {
        public msg Execute(msg m)
        {
            if (m == null || m.Input == null) return m;
            string[] words = (string[])m.Input;
            Encoding encoding = Encoding.UTF7;
            string input = string.Join("\r\n", words);
            m.KEY = input;
            m.Output.Ok = false;

            ////string temp = HttpUtility.UrlEncode(input.Replace(" ", "---"));
            //string temp = HttpUtility.UrlEncode(input);
            ////temp = temp.Replace("-----", "%20");

            //string url = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", temp, "en|vi");

            //string s = String.Empty;
            //using (WebClient webClient = new WebClient())
            //{
            //    webClient.Encoding = encoding;
            //    s = webClient.DownloadString(url);
            //}
            //string ht = HttpUtility.HtmlDecode(s);

            //string result = String.Empty;
            //int p = s.IndexOf("id=result_box");
            //if (p > 0)
            //    s = "<span " + s.Substring(p, s.Length - p);
            //p = s.IndexOf("</div>");
            //if (p > 0)
            //{
            //    s = s.Substring(0, p);
            //    s = s.Replace("<br>", "¦");
            //    s = HttpUtility.HtmlDecode(s);
            //    result = Regex.Replace(s, @"<[^>]*>", String.Empty);
            //}
            //if (result != string.Empty)
            //{
            //    string[] rs = result.Split('¦').Select(x => x.Trim()).ToArray();
            //    m.Output = new MsgOutput() { Ok = true, Data = rs, Total = rs.Length };
            //    
            //    lock (lockResponse)
            //    {
            //        if (dicResponses.ContainsKey(m.Key))
            //            dicResponses[m.Key] = m;
            //        else
            //            dicResponses.Add(m.Key, m);
            //    }
            //}
            //else
            //{
            //    m.Output = new MsgOutput() { Data = "Can not translate" };
            //}

            //postMessageToFormUI(m.Key);

            m.Output.Ok = true;
            m.Output.Data = words;
            return m;
        }
        public void Close() { }
    }

    public class api_word_Download : api_base, IAPI
    {
        static readonly object _lock = new object();
        static List<string> listWordDownload = new List<string>() { };

        public msg Execute(msg m)
        {
            if (m == null || m.Input == null) return m;
            string[] a = (string[])m.Input;

            m.KEY = string.Empty;
            m.Output.Ok = false;

            ////////https://s3.amazonaws.com/audio.oxforddictionaries.com/en/mp3/you_gb_1.mp3
            ////////https://ssl.gstatic.com/dictionary/static/sounds/oxford/you--_gb_1.mp3


            ////////https://ssl.gstatic.com/dictionary/static/sounds/20160317/hello--_us_1.mp3
            ////////https://ssl.gstatic.com/dictionary/static/sounds/20160317/you--_us_1.mp3

            ////////https://ssl.gstatic.com/dictionary/static/sounds/20160317/ok--_us_1.mp3
            ////////https://ssl.gstatic.com/dictionary/static/sounds/20160317/you--_gb_1.mp3
            ////////https://ssl.gstatic.com/dictionary/static/sounds/20160317/ok--_gb_1.mp3

            //////if (!NetworkInterface.GetIsNetworkAvailable())
            //////{
            //////    // Network does not available.

            //////}

            //////var ping = new System.Net.NetworkInformation.Ping();
            //////var result = ping.Send("www.google.com");
            //////if (result.Status == System.Net.NetworkInformation.IPStatus.Success)
            //////{

            //////}



            //////////string temp = HttpUtility.UrlEncode(input.Replace(" ", "---"));
            ////////string temp = HttpUtility.UrlEncode(input);
            //////////temp = temp.Replace("-----", "%20");

            ////////string url = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", temp, "en|vi");

            ////////string s = String.Empty;
            ////////using (WebClient webClient = new WebClient())
            ////////{
            ////////    webClient.Encoding = encoding;
            ////////    s = webClient.DownloadString(url);
            ////////}
            ////////string ht = HttpUtility.HtmlDecode(s);

            ////////string result = String.Empty;
            ////////int p = s.IndexOf("id=result_box");
            ////////if (p > 0)
            ////////    s = "<span " + s.Substring(p, s.Length - p);
            ////////p = s.IndexOf("</div>");
            ////////if (p > 0)
            ////////{
            ////////    s = s.Substring(0, p);
            ////////    s = s.Replace("<br>", "¦");
            ////////    s = HttpUtility.HtmlDecode(s);
            ////////    result = Regex.Replace(s, @"<[^>]*>", String.Empty);
            ////////}
            ////////if (result != string.Empty)
            ////////{
            ////////    string[] rs = result.Split('¦').Select(x => x.Trim()).ToArray();
            ////////    m.Output = new MsgOutput() { Ok = true, Data = rs, Total = rs.Length };
            ////////    
            ////////    lock (lockResponse)
            ////////    {
            ////////        if (dicResponses.ContainsKey(m.Key))
            ////////            dicResponses[m.Key] = m;
            ////////        else
            ////////            dicResponses.Add(m.Key, m);
            ////////    }
            ////////}
            ////////else
            ////////{
            ////////    m.Output = new MsgOutput() { Data = "Can not translate" };
            ////////}

            ////////postMessageToFormUI(m.Key);

            m.Output.Ok = true;
            //m.Output.Data = wo.words;
            return m;
        }
        public void Close() { }
    }

    public class api_word_LocalStore : api_base, IAPI
    {
        static readonly object _lock = new object();
        static Dictionary<string, oWord> dicWord = new Dictionary<string, oWord>();

        public static oWordContent f_analytic_Text(string s)
        {
            string[] sentences = s.Split(new char[] { '\n', '\r', '.' }).Where(x => x != string.Empty).ToArray();

            string text = Regex.Replace(s, "[^0-9a-zA-Z]+", " ").ToLower();
            text = Regex.Replace(text, "[ ]{2,}", " ").ToLower();
            oWordCount[] aword = text.Split(' ').Where(x => x.Length > 3)
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .Select(x => new oWordCount() { word = x.Key, count = x.Count() })
                .ToArray();

            return new oWordContent() { sentences = sentences, words = aword };
        }


        public msg Execute(msg m)
        {
            if (m == null || m.Input == null) return m;
            oWordContent wo = (oWordContent)m.Input;
            m.KEY = string.Empty;
            m.Output.Ok = false;

            string[] a_con = wo.words.Select(x => x.word.ToLower()).Distinct().ToArray();

            string[] a_local = new string[] { };
            lock (_lock)
                a_local = dicWord.Keys.ToArray();

            a_local = a_local.Where(x => a_con.Any(o => o == x)).ToArray();

            string[] a_down;

            if (a_local.Length == 0)
                a_down = a_con;
            else
                a_down = a_local.Where(x => !a_con.Any(o => o == x)).ToArray();

            if (a_down.Length > 0)
                app.postMessageToService(new msg() { API = _API.WORD_DOWNLOAD, Input = a_down });

            m.Output.Ok = true;
            m.Output.Data = a_con;

            return m;
        }
        public void Close() { }
    }

    public class api_folder_Analytic : api_base, IAPI
    {
        static readonly object _lock = new object();
        static List<string> listWordDownload = new List<string>() { };

        public msg Execute(msg m)
        {
            if (m == null || m.Input == null) return m;
            string path = (string)m.Input;
            if (Directory.Exists(path))
            {

                m.KEY = string.Empty;
                m.Output.Ok = false;
            }

            m.Output.Ok = true;
            //m.Output.Data = wo.words;
            return m;
        }
        public void Close() { }
    }

    public class api_youtube : api_base, IAPI
    {
        public api_youtube()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                | (SecurityProtocolType)3072
                | (SecurityProtocolType)0x00000C00
                | SecurityProtocolType.Tls;
        }

        public void Close() { }

        public msg Execute(msg msg)
        {
            if (msg != null && msg.Input != null)
            {
                //string s, url, videoId;
                //HttpWebRequest w;
                //HtmlDocument doc;

                switch (msg.KEY)
                {
                    case _API.YOUTUBE_INFO:
                        ////////videoId = (string)msg.Input;
                        ////////url = string.Format("https://www.youtube.com/get_video_info?video_id={0}&el=embedded&sts=&hl=en", videoId);
                        ////////w = (HttpWebRequest)WebRequest.Create(new Uri(url));
                        //////////w.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36";
                        ////////w.BeginGetResponse(asyncResult =>
                        ////////{
                        ////////    HttpWebResponse rs = (HttpWebResponse)w.EndGetResponse(asyncResult); //add a break point here 
                        ////////    StreamReader sr = new StreamReader(rs.GetResponseStream(), Encoding.UTF8);
                        ////////    string query = sr.ReadToEnd();
                        ////////    sr.Close();
                        ////////    rs.Close();

                        ////////    oVideo video = null;
                        ////////    List<ClosedCaptionTrackInfo> listCaptionTrackInfo = new List<ClosedCaptionTrackInfo>();
                        ////////    #region [ VIDEO INFO - CAPTION - SUBTITLE ] 

                        ////////    if (!string.IsNullOrEmpty(query))
                        ////////    {
                        ////////        //query = HttpUtility.HtmlDecode(query);

                        ////////        //////////////////////////////////////////////////////
                        ////////        // GET VIDEO INFO

                        ////////        var videoInfo = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        ////////        var rawParams = query.Split('&');
                        ////////        foreach (var rawParam in rawParams)
                        ////////        {
                        ////////            var param = HttpUtility.UrlDecode(rawParam);

                        ////////            // Look for the equals sign
                        ////////            var equalsPos = param.IndexOf('=');
                        ////////            if (equalsPos <= 0)
                        ////////                continue;

                        ////////            // Get the key and value
                        ////////            var key = param.Substring(0, equalsPos);
                        ////////            var value = equalsPos < param.Length
                        ////////                ? param.Substring(equalsPos + 1)
                        ////////                : string.Empty;

                        ////////            // Add to dictionary
                        ////////            videoInfo[key] = value;
                        ////////        }

                        ////////        // Extract values
                        ////////        var title = videoInfo["title"];
                        ////////        var author = videoInfo["author"];
                        ////////        double length_seconds = 0;
                        ////////        double.TryParse(videoInfo["length_seconds"], out length_seconds);
                        ////////        TimeSpan duration = TimeSpan.FromSeconds(length_seconds);
                        ////////        long viewCount = 0;
                        ////////        long.TryParse(videoInfo["view_count"], out viewCount);
                        ////////        var keywords = videoInfo["keywords"].Split(',');

                        ////////        //////////////////////////////////////////////////////
                        ////////        // CAPTION - SUBTITLE

                        ////////        // Extract captions metadata
                        ////////        var playerResponseRaw = videoInfo["player_response"];
                        ////////        var playerResponseJson = JToken.Parse(playerResponseRaw);
                        ////////        var captionTracksJson = playerResponseJson.SelectToken("$..captionTracks").EmptyIfNull();

                        ////////        // Parse closed caption tracks 
                        ////////        foreach (var captionTrackJson in captionTracksJson)
                        ////////        {
                        ////////            // Extract values
                        ////////            var code = captionTrackJson["languageCode"].Value<string>();
                        ////////            var name = captionTrackJson["name"]["simpleText"].Value<string>();
                        ////////            var language = new Language(code, name);
                        ////////            var isAuto = captionTrackJson["vssId"].Value<string>()
                        ////////                .StartsWith("a.", StringComparison.OrdinalIgnoreCase);
                        ////////            var url_caption = captionTrackJson["baseUrl"].Value<string>();

                        ////////            // Enforce format
                        ////////            url_caption = SetQueryParameter(url_caption, "format", "3");

                        ////////            var closedCaptionTrackInfo = new ClosedCaptionTrackInfo(url_caption, language, isAuto);
                        ////////            listCaptionTrackInfo.Add(closedCaptionTrackInfo);
                        ////////        }

                        ////////        ///////////////////////////////////////////////////////////////
                        ////////        // GET VIDEO WATCH PAGE
                        ////////        using (WebClient webWatchPage = new WebClient())
                        ////////        {
                        ////////            webWatchPage.Encoding = Encoding.UTF8;
                        ////////            s = webWatchPage.DownloadString(string.Format("https://www.youtube.com/watch?v={0}&disable_polymer=true&hl=en", videoId));

                        ////////            s = Regex.Replace(s, @"<script[^>]*>[\s\S]*?</script>", string.Empty);
                        ////////            s = Regex.Replace(s, @"<style[^>]*>[\s\S]*?</style>", string.Empty);
                        ////////            s = Regex.Replace(s, @"<noscript[^>]*>[\s\S]*?</noscript>", string.Empty);
                        ////////            s = Regex.Replace(s, @"(?s)(?<=<!--).+?(?=-->)", string.Empty).Replace("<!---->", string.Empty);

                        ////////            //s = Regex.Replace(s, @"<noscript[^>]*>[\s\S]*?</noscript>", string.Empty);
                        ////////            //s = Regex.Replace(s, @"<noscript[^>]*>[\s\S]*?</noscript>", string.Empty);
                        ////////            //s = Regex.Replace(s, @"</?(?i:embed|object|frameset|frame|iframe|meta|link)(.|\n|\s)*?>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                        ////////            s = Regex.Replace(s, @"</?(?i:embed|object|frameset|frame|iframe|link)(.|\n|\s)*?>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);

                        ////////            // Load the document using HTMLAgilityPack as normal
                        ////////            doc = new HtmlDocument();
                        ////////            doc.LoadHtml(s);

                        ////////            // Fizzler for HtmlAgilityPack is implemented as the
                        ////////            // QuerySelectorAll extension method on HtmlNode
                        ////////            var watchPage = doc.DocumentNode;

                        ////////            // Extract values 
                        ////////            var uploadDate = watchPage.QuerySelector("meta[itemprop=\"datePublished\"]")
                        ////////                .GetAttributeValue("content", "1900-01-01")
                        ////////                .ParseDateTimeOffset("yyyy-MM-dd");
                        ////////            var likeCount = watchPage.QuerySelector("button.like-button-renderer-like-button").InnerText
                        ////////                .StripNonDigit().ParseLongOrDefault();
                        ////////            var dislikeCount = watchPage.QuerySelector("button.like-button-renderer-dislike-button").InnerText
                        ////////                .StripNonDigit().ParseLongOrDefault();
                        ////////            var description = watchPage.QuerySelector("p#eow-description").TextEx();

                        ////////            var statistics = new Statistics(viewCount, likeCount, dislikeCount);
                        ////////            var thumbnails = new ThumbnailSet(videoId);

                        ////////            video = new oVideo(videoId, author, uploadDate, title, description, thumbnails, duration, keywords, statistics);
                        ////////        }
                        ////////    }

                        ////////    #endregion                            
                        ////////    string vinfo = $"Id: {video.Id} | Title: {video.Title} | Author: {video.Author}";

                        ////////    //////////////////////////////////////////////////////////////////////////////////////////////

                        ////////    Channel channel = null;
                        ////////    PlayerContext playerContext = null;
                        ////////    #region [ MEDIA STREAM INFO SET - CHANNEL INFO ]

                        ////////    using (WebClient requestGetVideoEmbedPage = new WebClient())
                        ////////    {
                        ////////        requestGetVideoEmbedPage.Encoding = Encoding.UTF8;
                        ////////        string rawGetVideoEmbedPage = requestGetVideoEmbedPage.DownloadString(string.Format("https://www.youtube.com/embed/{0}?disable_polymer=true&hl=en", videoId));

                        ////////        //////s = Regex.Replace(s, @"<script[^>]*>[\s\S]*?</script>", string.Empty);
                        ////////        //////s = Regex.Replace(s, @"<style[^>]*>[\s\S]*?</style>", string.Empty);
                        ////////        //////s = Regex.Replace(s, @"<noscript[^>]*>[\s\S]*?</noscript>", string.Empty);
                        ////////        //////s = Regex.Replace(s, @"(?s)(?<=<!--).+?(?=-->)", string.Empty).Replace("<!---->", string.Empty);
                        ////////        ////////s = Regex.Replace(s, @"</?(?i:embed|object|frameset|frame|iframe|meta|link)(.|\n|\s)*?>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                        ////////        //////s = Regex.Replace(s, @"</?(?i:embed|object|frameset|frame|iframe|link)(.|\n|\s)*?>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                        ////////        //////// Load the document using HTMLAgilityPack as normal
                        ////////        //////doc = new HtmlDocument();
                        ////////        //////doc.LoadHtml(s);
                        ////////        // Fizzler for HtmlAgilityPack is implemented as the QuerySelectorAll extension method on HtmlNode
                        ////////        // var watchPage = doc.DocumentNode;

                        ////////        // Get embed page config
                        ////////        var part = rawGetVideoEmbedPage.SubstringAfter("yt.setConfig({'PLAYER_CONFIG': ").SubstringUntil(",'");
                        ////////        JToken configJson = JToken.Parse(part);

                        ////////        // Extract values
                        ////////        var sourceUrl = configJson["assets"]["js"].Value<string>();
                        ////////        var sts_value = configJson["sts"].Value<string>();

                        ////////        // Extract values
                        ////////        var channelPath = configJson["args"]["channel_path"].Value<string>();
                        ////////        var id = channelPath.SubstringAfter("channel/");
                        ////////        var title = configJson["args"]["expanded_title"].Value<string>();
                        ////////        var logoUrl = configJson["args"]["profile_picture"].Value<string>();

                        ////////        channel = new Channel(id, title, logoUrl);

                        ////////        // Check if successful
                        ////////        if (sourceUrl.IsBlank() || sts_value.IsBlank())
                        ////////            throw new Exception("Could not parse player context.");

                        ////////        // Append host to source url
                        ////////        sourceUrl = "https://www.youtube.com" + sourceUrl;
                        ////////        playerContext = new PlayerContext(sourceUrl, sts_value);
                        ////////    }

                        ////////    #endregion

                        ////////    //////////////////////////////////////////////////////////////////////////////////////////////

                        ////////    string sts = playerContext.Sts;
                        ////////    MediaStreamInfoSet streamInfoSet = null;
                        ////////    #region [ STREAM VIDEO INFO FROM EMBED/DETAIL PAGE ]

                        ////////    Dictionary<string, string> videoInfo_EmbeddedOrDetailPage = new Dictionary<string, string>();
                        ////////    using (WebClient requestGetVideoInfo = new WebClient())
                        ////////    {
                        ////////        requestGetVideoInfo.Encoding = Encoding.UTF8;
                        ////////        string rawGetVideoInfo = requestGetVideoInfo.DownloadString(
                        ////////            string.Format("https://www.youtube.com/get_video_info?video_id={0}&el={1}&sts={2}&hl=en", videoId, "embedded", sts));

                        ////////        // Get video info
                        ////////        videoInfo_EmbeddedOrDetailPage = SplitQuery(rawGetVideoInfo);

                        ////////        // If can't be embedded - try another value of el
                        ////////        if (videoInfo_EmbeddedOrDetailPage.ContainsKey("errorcode"))
                        ////////        {
                        ////////            var errorReason = videoInfo_EmbeddedOrDetailPage["reason"];
                        ////////            if (errorReason.Contains("&feature=player_embedded"))
                        ////////            {
                        ////////                string rawGetVideoInfo_DetailPage = string.Empty;
                        ////////                using (WebClient requestGetVideoInfo_DetailPage = new WebClient())
                        ////////                {
                        ////////                    requestGetVideoInfo_DetailPage.Encoding = Encoding.UTF8;
                        ////////                    rawGetVideoInfo_DetailPage = requestGetVideoInfo_DetailPage.DownloadString(
                        ////////                        string.Format("https://www.youtube.com/get_video_info?video_id={0}&el={1}&sts={2}&hl=en", videoId, "detailpage", sts));
                        ////////                }
                        ////////                videoInfo_EmbeddedOrDetailPage = SplitQuery(rawGetVideoInfo_DetailPage);
                        ////////            }
                        ////////        }

                        ////////        // Check error
                        ////////        if (videoInfo_EmbeddedOrDetailPage.ContainsKey("errorcode"))
                        ////////        {
                        ////////            var errorCode = videoInfo_EmbeddedOrDetailPage["errorcode"].ParseInt();
                        ////////            var errorReason = videoInfo_EmbeddedOrDetailPage["reason"];

                        ////////            throw new VideoUnavailableException(videoId, errorCode, errorReason);
                        ////////        }
                        ////////    }

                        ////////    // Check if requires purchase
                        ////////    if (videoInfo_EmbeddedOrDetailPage.ContainsKey("ypc_vid"))
                        ////////    {
                        ////////        var previewVideoId = videoInfo_EmbeddedOrDetailPage["ypc_vid"];
                        ////////        throw new Exception(string.Format("Video [{0}] requires purchase and cannot be processed." + Environment.NewLine + "Free preview video [{1}] is available.", videoId, previewVideoId));
                        ////////    }

                        ////////    streamInfoSet = GetVideoMediaStreamInfosAsync(videoInfo_EmbeddedOrDetailPage);
                        ////////    var streamInfo = streamInfoSet.Muxed.WithHighestVideoQuality();
                        ////////    var normalizedFileSize = NormalizeFileSize(streamInfo.Size);

                        ////////    #endregion

                        ////////    string vstreamInfo = $"Quality: {streamInfo.VideoQualityLabel} | Container: {streamInfo.Container} | Size: {normalizedFileSize}";

                        ////////    //////////////////////////////////////////////////////////////////////////////////////////////

                        ////////    ////// Compose file name, based on metadata
                        ////////    ////var fileExtension = streamInfo.Container.GetFileExtension();
                        ////////    ////var fileName = $"{video.Title}.{fileExtension}";
                        ////////    ////// Replace illegal characters in file name
                        ////////    //////fileName = fileName.Replace(Path.GetInvalidFileNameChars(), '_');
                        ////////    ////// Download video
                        ////////    ////Console.WriteLine($"Downloading to [{fileName}]...");
                        ////////    ////Console.WriteLine('-'.Repeat(100));
                        ////////    ////var progress = new Progress<double>(p => Console.Title = $"YoutubeExplode Demo [{p:P0}]");
                        ////////    ////await client.DownloadMediaStreamAsync(streamInfo, fileName, progress);
                        ////////}, w);
                        break;
                }
            }
            return msg;
        }


        #region [ function common ]


        private static MediaStreamInfoSet GetVideoMediaStreamInfosAsync(Dictionary<string, string> videoInfo)
        {
            // Prepare stream info collections
            var muxedStreamInfoMap = new Dictionary<int, MuxedStreamInfo>();
            var audioStreamInfoMap = new Dictionary<int, AudioStreamInfo>();
            var videoStreamInfoMap = new Dictionary<int, VideoStreamInfo>();

            // Resolve muxed streams
            var muxedStreamInfosEncoded = videoInfo.GetOrDefault("url_encoded_fmt_stream_map");
            if (muxedStreamInfosEncoded.IsNotBlank())
            {
                foreach (var streamEncoded in muxedStreamInfosEncoded.Split(","))
                {
                    var streamInfoDic = SplitQuery(streamEncoded);

                    // Extract values
                    var itag = streamInfoDic["itag"].ParseInt();
                    var url = streamInfoDic["url"];
                    var sig = streamInfoDic.GetOrDefault("s");

#if RELEASE
                    if (!MediaStreamInfo.IsKnown(itag))
                        continue;
#endif

                    //// Decipher signature if needed
                    //if (sig.IsNotBlank())
                    //{
                    //    new Exception("Decipher signature if needed");
                    //    //////var playerSource = GetVideoPlayerSourceAsync(playerContext.SourceUrl);
                    //    //////sig = playerSource.Decipher(sig);
                    //    //////url = SetQueryParameter(url, "signature", sig);
                    //}

                    // Probe stream and get content length
                    long contentLength = 0;

                    HttpWebRequest w = (HttpWebRequest)WebRequest.Create(url);
                    w.Method = "HEAD";
                    WebResponse rs = w.GetResponse();
                    contentLength = rs.ContentLength;
                    rs.Close();

                    ////HttpWebRequest w = (HttpWebRequest)WebRequest.Create(new Uri(url));
                    ////w.BeginGetResponse(asyncResult =>
                    ////{
                    ////    HttpWebResponse response = (HttpWebResponse)w.EndGetResponse(asyncResult);
                    ////    //add a break point here 
                    ////    // Some muxed streams can be gone
                    ////    if (response.StatusCode == HttpStatusCode.OK)
                    ////    {
                    ////        // Extract content length
                    ////        contentLength = response.ContentLength;
                    ////        response.Close();
                    ////    }
                    ////}, w);

                    //// Probe stream and get content length
                    //long contentLength;
                    //using (var response = await _httpClient.HeadAsync(url).ConfigureAwait(false))
                    //{
                    //    // Some muxed streams can be gone
                    //    if (response.StatusCode == HttpStatusCode.NotFound ||
                    //        response.StatusCode == HttpStatusCode.Gone)
                    //        continue;

                    //    // Ensure success
                    //    response.EnsureSuccessStatusCode();

                    //    // Extract content length
                    //    contentLength = response.Content.Headers.ContentLength ??
                    //                    throw new Exception("Could not extract content length of muxed stream.");
                    //}

                    var streamInfo = new MuxedStreamInfo(itag, url, contentLength);
                    muxedStreamInfoMap[itag] = streamInfo;
                }
            }

            // Resolve adaptive streams
            var adaptiveStreamInfosEncoded = videoInfo.GetOrDefault("adaptive_fmts");
            if (adaptiveStreamInfosEncoded.IsNotBlank())
            {
                foreach (var streamEncoded in adaptiveStreamInfosEncoded.Split(","))
                {
                    var streamInfoDic = SplitQuery(streamEncoded);

                    // Extract values
                    var itag = streamInfoDic["itag"].ParseInt();
                    var url = streamInfoDic["url"];
                    var sig = streamInfoDic.GetOrDefault("s");
                    var contentLength = streamInfoDic["clen"].ParseLong();
                    var bitrate = streamInfoDic["bitrate"].ParseLong();

#if RELEASE
                    if (!MediaStreamInfo.IsKnown(itag))
                        continue;
#endif

                    // Decipher signature if needed
                    if (sig.IsNotBlank())
                    {
                        new Exception("Decipher signature if needed");
                        //var playerSource = await GetVideoPlayerSourceAsync(playerContext.SourceUrl).ConfigureAwait(false);
                        //sig = playerSource.Decipher(sig);
                        //url = SetQueryParameter(url, "signature", sig);
                    }

                    // Check if audio
                    var isAudio = streamInfoDic["type"].Contains("audio/");

                    // If audio stream
                    if (isAudio)
                    {
                        var streamInfo = new AudioStreamInfo(itag, url, contentLength, bitrate);
                        audioStreamInfoMap[itag] = streamInfo;
                    }
                    // If video stream
                    else
                    {
                        // Parse additional data
                        var size = streamInfoDic["size"];
                        var width = size.SubstringUntil("x").ParseInt();
                        var height = size.SubstringAfter("x").ParseInt();
                        var resolution = new VideoResolution(width, height);
                        var framerate = streamInfoDic["fps"].ParseInt();
                        var qualityLabel = streamInfoDic["quality_label"];

                        var streamInfo = new VideoStreamInfo(itag, url, contentLength, bitrate, resolution, framerate,
                            qualityLabel);
                        videoStreamInfoMap[itag] = streamInfo;
                    }
                }
            }

            // Resolve dash streams
            var dashManifestUrl = videoInfo.GetOrDefault("dashmpd");
            if (dashManifestUrl.IsNotBlank())
            {
                // Parse signature
                var sig = Regex.Match(dashManifestUrl, @"/s/(.*?)(?:/|$)").Groups[1].Value;

                // Decipher signature if needed
                if (sig.IsNotBlank())
                {
                    new Exception("Decipher signature if needed");
                    //var playerSource = await GetVideoPlayerSourceAsync(playerContext.SourceUrl).ConfigureAwait(false);
                    //sig = playerSource.Decipher(sig);
                    //dashManifestUrl = SetRouteParameter(dashManifestUrl, "signature", sig);
                }

                // Get the manifest
                //var response = await _httpClient.GetStringAsync(dashManifestUrl).ConfigureAwait(false);
                string response = string.Empty;
                using (WebClient webWatchPage = new WebClient())
                {
                    webWatchPage.Encoding = Encoding.UTF8;
                    response = webWatchPage.DownloadString(dashManifestUrl);
                }

                var dashManifestXml = XElement.Parse(response).StripNamespaces();
                var streamsXml = dashManifestXml.Descendants("Representation");

                // Parse streams
                foreach (var streamXml in streamsXml)
                {
                    // Skip partial streams
                    if (streamXml.Descendants("Initialization").FirstOrDefault()?.Attribute("sourceURL")?.Value
                            .Contains("sq/") == true)
                        continue;

                    // Extract values
                    var itag = (int)streamXml.Attribute("id");
                    var url = (string)streamXml.Element("BaseURL");
                    var bitrate = (long)streamXml.Attribute("bandwidth");

#if RELEASE
                    if (!MediaStreamInfo.IsKnown(itag))
                        continue;
#endif

                    // Parse content length
                    var contentLength = Regex.Match(url, @"clen[/=](\d+)").Groups[1].Value.ParseLong();

                    // Check if audio stream
                    var isAudio = streamXml.Element("AudioChannelConfiguration") != null;

                    // If audio stream
                    if (isAudio)
                    {
                        var streamInfo = new AudioStreamInfo(itag, url, contentLength, bitrate);
                        audioStreamInfoMap[itag] = streamInfo;
                    }
                    // If video stream
                    else
                    {
                        // Parse additional data
                        var width = (int)streamXml.Attribute("width");
                        var height = (int)streamXml.Attribute("height");
                        var resolution = new VideoResolution(width, height);
                        var framerate = (int)streamXml.Attribute("frameRate");

                        var streamInfo = new VideoStreamInfo(itag, url, contentLength, bitrate, resolution, framerate);
                        videoStreamInfoMap[itag] = streamInfo;
                    }
                }
            }

            // Get the raw HLS stream playlist (*.m3u8)
            var hlsLiveStreamUrl = videoInfo.GetOrDefault("hlsvp");

            // Finalize stream info collections
            var muxedStreamInfos = muxedStreamInfoMap.Values.OrderByDescending(s => s.VideoQuality).ToList();
            var audioStreamInfos = audioStreamInfoMap.Values.OrderByDescending(s => s.Bitrate).ToList();
            var videoStreamInfos = videoStreamInfoMap.Values.OrderByDescending(s => s.VideoQuality).ToList();

            return new MediaStreamInfoSet(muxedStreamInfos, audioStreamInfos, videoStreamInfos, hlsLiveStreamUrl);
        }

        private static string SetQueryParameter(string url, string key, string value)
        {
            value = value ?? string.Empty;

            // Find existing parameter
            var existingMatch = Regex.Match(url, $@"[?&]({Regex.Escape(key)}=?.*?)(?:&|/|$)");

            // Parameter already set to something
            if (existingMatch.Success)
            {
                var group = existingMatch.Groups[1];

                // Remove existing
                url = url.Remove(group.Index, group.Length);

                // Insert new one
                url = url.Insert(group.Index, $"{key}={value}");

                return url;
            }
            // Parameter hasn't been set yet
            else
            {
                // See if there are other parameters
                var hasOtherParams = url.IndexOf('?') >= 0;

                // Prepend either & or ? depending on that
                var separator = hasOtherParams ? '&' : '?';

                // Assemble new query string
                return url + separator + key + '=' + value;
            }
        }

        private static string SetRouteParameter(string url, string key, string value)
        {
            value = value ?? string.Empty;

            // Find existing parameter
            var existingMatch = Regex.Match(url, $@"/({Regex.Escape(key)}/?.*?)(?:/|$)");

            // Parameter already set to something
            if (existingMatch.Success)
            {
                var group = existingMatch.Groups[1];

                // Remove existing
                url = url.Remove(group.Index, group.Length);

                // Insert new one
                url = url.Insert(group.Index, $"{key}/{value}");

                return url;
            }
            // Parameter hasn't been set yet
            else
            {
                // Assemble new query string
                return url + '/' + key + '/' + value;
            }
        }

        /// <summary>
        /// Turns file size in bytes into human-readable string
        /// </summary>
        private static string NormalizeFileSize(long fileSize)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            double size = fileSize;
            var unit = 0;

            while (size >= 1024)
            {
                size /= 1024;
                ++unit;
            }

            return $"{size:0.#} {units[unit]}";
        }

        /// <summary>
        /// Tries to parse video ID from a YouTube video URL.
        /// </summary>
        public static bool TryParseVideoId(string videoUrl, out string videoId)
        {
            videoId = default(string);

            if (string.IsNullOrEmpty(videoUrl))
                return false;

            // https://www.youtube.com/watch?v=yIVRs6YSbOM
            var regularMatch =
                Regex.Match(videoUrl, @"youtube\..+?/watch.*?v=(.*?)(?:&|/|$)").Groups[1].Value;
            if (!string.IsNullOrEmpty(regularMatch) && ValidateVideoId(regularMatch))
            {
                videoId = regularMatch;
                return true;
            }

            // https://youtu.be/yIVRs6YSbOM
            var shortMatch =
                Regex.Match(videoUrl, @"youtu\.be/(.*?)(?:\?|&|/|$)").Groups[1].Value;
            if (!string.IsNullOrEmpty(shortMatch) && ValidateVideoId(shortMatch))
            {
                videoId = shortMatch;
                return true;
            }

            // https://www.youtube.com/embed/yIVRs6YSbOM
            var embedMatch =
                Regex.Match(videoUrl, @"youtube\..+?/embed/(.*?)(?:\?|&|/|$)").Groups[1].Value;
            if (!string.IsNullOrEmpty(embedMatch) && ValidateVideoId(embedMatch))
            {
                videoId = embedMatch;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Verifies that the given string is syntactically a valid YouTube video ID.
        /// </summary>
        public static bool ValidateVideoId(string videoId)
        {
            if (string.IsNullOrEmpty(videoId))
                return false;

            if (videoId.Length != 11)
                return false;

            return !Regex.IsMatch(videoId, @"[^0-9a-zA-Z_\-]");
        }

        private static Dictionary<string, string> SplitQuery(string query)
        {
            var dic = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var rawParams = query.Split("&");
            foreach (var rawParam in rawParams)
            {
                var param = rawParam.UrlDecode();

                // Look for the equals sign
                var equalsPos = param.IndexOf('=');
                if (equalsPos <= 0)
                    continue;

                // Get the key and value
                var key = param.Substring(0, equalsPos);
                var value = equalsPos < param.Length
                    ? param.Substring(equalsPos + 1)
                    : string.Empty;

                // Add to dictionary
                dic[key] = value;
            }

            return dic;
        }

        #endregion

        #region

        //////private PlayerSource GetVideoPlayerSourceAsync(string sourceUrl)
        //////{
        //////    // Original code credit:
        //////    // https://github.com/flagbug/YoutubeExtractor/blob/3106efa1063994fd19c0e967793315f6962b2d3c/YoutubeExtractor/YoutubeExtractor/Decipherer.cs
        //////    // No copyright, MIT license

        //////    // Try to resolve from cache first
        //////    var playerSource = _playerSourceCache.GetOrDefault(sourceUrl);
        //////    if (playerSource != null)
        //////        return playerSource;

        //////    // Get player source code
        //////    var sourceRaw = await _httpClient.GetStringAsync(sourceUrl).ConfigureAwait(false);

        //////    // Find the name of the function that handles deciphering
        //////    var entryPoint = Regex.Match(sourceRaw, @"\""signature"",\s?([a-zA-Z0-9\$]+)\(").Groups[1].Value;
        //////    if (entryPoint.IsBlank())
        //////        throw new ParseException("Could not find the entry function for signature deciphering.");

        //////    // Find the body of the function
        //////    var entryPointPattern = @"(?!h\.)" + Regex.Escape(entryPoint) + @"=function\(\w+\)\{(.*?)\}";
        //////    var entryPointBody = Regex.Match(sourceRaw, entryPointPattern, RegexOptions.Singleline).Groups[1].Value;
        //////    if (entryPointBody.IsBlank())
        //////        throw new ParseException("Could not find the signature decipherer function body.");
        //////    var entryPointLines = entryPointBody.Split(";").ToArray();

        //////    // Identify cipher functions
        //////    string reverseFuncName = null;
        //////    string sliceFuncName = null;
        //////    string charSwapFuncName = null;
        //////    var operations = new List<ICipherOperation>();

        //////    // Analyze the function body to determine the names of cipher functions
        //////    foreach (var line in entryPointLines)
        //////    {
        //////        // Break when all functions are found
        //////        if (reverseFuncName.IsNotBlank() && sliceFuncName.IsNotBlank() && charSwapFuncName.IsNotBlank())
        //////            break;

        //////        // Get the function called on this line
        //////        var calledFuncName = Regex.Match(line, @"\w+\.(\w+)\(").Groups[1].Value;
        //////        if (calledFuncName.IsBlank())
        //////            continue;

        //////        // Find cipher function names
        //////        if (Regex.IsMatch(sourceRaw, $@"{Regex.Escape(calledFuncName)}:\bfunction\b\(\w+\)"))
        //////        {
        //////            reverseFuncName = calledFuncName;
        //////        }
        //////        else if (Regex.IsMatch(sourceRaw,
        //////            $@"{Regex.Escape(calledFuncName)}:\bfunction\b\([a],b\).(\breturn\b)?.?\w+\."))
        //////        {
        //////            sliceFuncName = calledFuncName;
        //////        }
        //////        else if (Regex.IsMatch(sourceRaw,
        //////            $@"{Regex.Escape(calledFuncName)}:\bfunction\b\(\w+\,\w\).\bvar\b.\bc=a\b"))
        //////        {
        //////            charSwapFuncName = calledFuncName;
        //////        }
        //////    }

        //////    // Analyze the function body again to determine the operation set and order
        //////    foreach (var line in entryPointLines)
        //////    {
        //////        // Get the function called on this line
        //////        var calledFuncName = Regex.Match(line, @"\w+\.(\w+)\(").Groups[1].Value;
        //////        if (calledFuncName.IsBlank())
        //////            continue;

        //////        // Swap operation
        //////        if (calledFuncName == charSwapFuncName)
        //////        {
        //////            var index = Regex.Match(line, @"\(\w+,(\d+)\)").Groups[1].Value.ParseInt();
        //////            operations.Add(new SwapCipherOperation(index));
        //////        }
        //////        // Slice operation
        //////        else if (calledFuncName == sliceFuncName)
        //////        {
        //////            var index = Regex.Match(line, @"\(\w+,(\d+)\)").Groups[1].Value.ParseInt();
        //////            operations.Add(new SliceCipherOperation(index));
        //////        }
        //////        // Reverse operation
        //////        else if (calledFuncName == reverseFuncName)
        //////        {
        //////            operations.Add(new ReverseCipherOperation());
        //////        }
        //////    }

        //////    return _playerSourceCache[sourceUrl] = new PlayerSource(operations);
        //////}


        ////////        /// <inheritdoc />
        ////////        public async Task DownloadMediaStreamAsync(MediaStreamInfo info, Stream output,
        ////////            IProgress<double> progress = null, CancellationToken cancellationToken = default(CancellationToken))
        ////////        {
        ////////            info.GuardNotNull(nameof(info));
        ////////            output.GuardNotNull(nameof(output));

        ////////            // Determine if stream is rate-limited
        ////////            var isRateLimited = !Regex.IsMatch(info.Url, @"ratebypass[=/]yes");

        ////////            // Download rate-limited streams in segments
        ////////            if (isRateLimited)
        ////////            {
        ////////                // Determine segment count
        ////////                const long segmentSize = 9_898_989; // this number was carefully devised through research
        ////////                var segmentCount = (int)Math.Ceiling(1.0 * info.Size / segmentSize);

        ////////                // Keep track of bytes copied for progress reporting
        ////////                var totalBytesCopied = 0L;

        ////////                for (var i = 0; i < segmentCount; i++)
        ////////                {
        ////////                    // Determine segment range
        ////////                    var from = i * segmentSize;
        ////////                    var to = (i + 1) * segmentSize - 1;

        ////////                    // Download segment
        ////////                    using (var input = await _httpClient.GetStreamAsync(info.Url, from, to).ConfigureAwait(false))
        ////////                    {
        ////////                        int bytesCopied;
        ////////                        do
        ////////                        {
        ////////                            // Copy
        ////////                            bytesCopied = await input.CopyChunkToAsync(output, cancellationToken).ConfigureAwait(false);

        ////////                            // Report progress
        ////////                            totalBytesCopied += bytesCopied;
        ////////                            progress?.Report(1.0 * totalBytesCopied / info.Size);
        ////////                        } while (bytesCopied > 0);
        ////////                    }
        ////////                }
        ////////            }
        ////////            // Download non-limited streams directly
        ////////            else
        ////////            {
        ////////                using (var input = await GetMediaStreamAsync(info).ConfigureAwait(false))
        ////////                    await input.CopyToAsync(output, progress, cancellationToken).ConfigureAwait(false);
        ////////            }
        ////////        }

        ////////#if NETSTANDARD2_0 || NET45 || NETCOREAPP1_0

        ////////        /// <inheritdoc />
        ////////        public async Task DownloadMediaStreamAsync(MediaStreamInfo info, string filePath,
        ////////            IProgress<double> progress = null, CancellationToken cancellationToken = default(CancellationToken))
        ////////        {
        ////////            filePath.GuardNotNull(nameof(filePath));

        ////////            using (var output = File.Create(filePath))
        ////////                await DownloadMediaStreamAsync(info, output, progress, cancellationToken).ConfigureAwait(false);
        ////////        }

        ////////#endif

        #endregion

    }

    public class api_mp3 : api_base, IAPI
    {
        public void Close() { }

        public msg Execute(msg m)
        {
            if (m == null || m.Input == null) return m;
            string path_file = string.Empty;

            switch (m.KEY) {
                case _API.MP3_PLAY:
                    path_file = (string)m.Input;

                    using (var ms = File.OpenRead(path_file))
                    using (var rdr = new Mp3FileReader(ms))
                    using (var wavStream = WaveFormatConversionStream.CreatePcmStream(rdr))
                    using (var baStream = new BlockAlignReductionStream(wavStream))
                    using (var waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                    {
                        waveOut.Init(baStream);
                        waveOut.Play();
                        while (waveOut.PlaybackState == PlaybackState.Playing)
                        {
                            Thread.Sleep(100);
                        }
                    }
                    break;
            }

            return m;
        }
    }
}
