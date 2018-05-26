using Newtonsoft.Json.Linq;
using ProtoBuf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using YoutubeExplode;
using YoutubeExplode.Models;
using YoutubeExplode.Models.MediaStreams;
using YoutubeExplode.Internal;
using System.Text.RegularExpressions;
using System.Web;

namespace appel
{
    public class api_media : api_base, IAPI
    {
        #region [ VARIABLE ]

        static readonly string path_data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
        static readonly string file_media = Path.Combine(path_data, "media.bin");

        public bool Open { set; get; } = false;

        static ConcurrentDictionary<long, oMedia> dicMediaStore = null;
        static ConcurrentDictionary<long, Bitmap> dicMediaImage = null;
        static ConcurrentDictionary<long, oMedia> dicMediaSearch = null;
        static bool stop_search = false;
        static AutoResetEvent wait_search = null;

        #endregion

        #region [ API ]

        public void Init()
        {
            if (!Directory.Exists(path_data)) Directory.CreateDirectory(path_data);

            wait_search = new AutoResetEvent(false);
            dicMediaStore = new ConcurrentDictionary<long, oMedia>();
            dicMediaImage = new ConcurrentDictionary<long, Bitmap>();
            dicMediaSearch = new ConcurrentDictionary<long, oMedia>();

            if (File.Exists(file_media))
                using (var file = File.OpenRead(file_media))
                {
                    dicMediaStore = Serializer.Deserialize<ConcurrentDictionary<long, oMedia>>(file);
                    file.Close();
                }

            f_proxy_Start();
        }

        public msg Execute(msg m)
        {
            if (m != null && Open)
            {
                if (m.Output == null) m.Output = new msgOutput();
                switch (m.KEY)
                {
                    case _API.MEDIA_KEY_INITED:
                        #region  
                        break;
                    #endregion
                    case _API.MEDIA_KEY_PLAY_AUDIO:
                        #region
                        if (true)
                        {
                            long mediaId = (long)m.Input;
                            string urlSrc = f_media_fetchUriSource(mediaId, MEDIA_TYPE.M4A);
                            if (!string.IsNullOrEmpty(urlSrc))
                            {
                                m.Output.Ok = true;
                                m.Output.Data = f_proxy_getUriProxy(mediaId, MEDIA_TYPE.M4A);
                                response_toMain(m);
                            }
                            else
                            {
                                // cannot fetch uri
                            }
                        }
                        break;
                    #endregion
                    case _API.MEDIA_KEY_PLAY_VIDEO:
                        #region
                        if (true)
                        {
                            long mediaId = (long)m.Input;
                            string urlSrc = f_media_fetchUriSource(mediaId, MEDIA_TYPE.MP4);
                            if (!string.IsNullOrEmpty(urlSrc))
                            {
                                m.Output.Ok = true;
                                m.Output.Data = f_proxy_getUriProxy(mediaId, MEDIA_TYPE.MP4);
                                response_toMain(m);
                            }
                            else
                            {
                                // cannot fetch uri
                            }
                        }
                        break;
                    #endregion
                    case _API.MEDIA_KEY_TEXT_INFO:
                        #region
                        if (true)
                        {
                            long mediaId = (long)m.Input;
                            oMedia mi = null;
                            MEDIA_TAB tab = MEDIA_TAB.TAB_STORE;
                            if (m.Log != null) tab = (MEDIA_TAB)(int.Parse(m.Log));

                            if (tab == MEDIA_TAB.TAB_STORE)
                                dicMediaStore.TryGetValue(mediaId, out mi);
                            else if (tab == MEDIA_TAB.TAB_SEARCH)
                                dicMediaSearch.TryGetValue(mediaId, out mi);

                            if (mi != null)
                            {
                                m.Output.Ok = true;
                                m.Output.Data = mi.Text;
                                response_toMain(m);
                            }
                        }
                        break;
                    #endregion
                    case _API.MEDIA_KEY_WORD_LIST:
                        #region
                        if (true)
                        {
                            long mediaId = (long)m.Input;
                            oMedia mi = null;
                            MEDIA_TAB tab = MEDIA_TAB.TAB_STORE;
                            if (m.Log != null) tab = (MEDIA_TAB)(int.Parse(m.Log));

                            if (tab == MEDIA_TAB.TAB_STORE)
                                dicMediaStore.TryGetValue(mediaId, out mi);
                            else if (tab == MEDIA_TAB.TAB_SEARCH)
                                dicMediaSearch.TryGetValue(mediaId, out mi);

                            if (mi != null)
                            {
                                m.Output.Ok = true;
                                m.Output.Data = mi.Words;
                                response_toMain(m);
                            }
                        }
                        break;
                    #endregion
                    case _API.MEDIA_KEY_SEARCH_STORE:
                        #region 
                        if (true)
                        {
                            string input = (string)m.Input;
                            if (input == null) input = string.Empty;
                            oMediaSearchLocalResult resultSearch = new oMediaSearchLocalResult();

                            List<long> lsSearch = new List<long>();
                            int min = (m.PageNumber - 1) * m.PageSize,
                                max = m.PageNumber * m.PageSize,
                                count = 0;
                            foreach (var kv in dicMediaStore)
                            {
                                if (kv.Value.Title.ToLower().Contains(input)
                                    || kv.Value.Description.ToLower().Contains(input))
                                {
                                    if (count >= min && count < max)
                                        lsSearch.Add(kv.Key);
                                    count++;
                                }
                            }
                            resultSearch.TotalItem = dicMediaStore.Count;
                            resultSearch.PageSize = m.PageSize;
                            resultSearch.PageNumber = m.PageNumber;
                            resultSearch.CountResult = count;
                            resultSearch.MediaIds = lsSearch;

                            m.Counter = count;
                            m.Output.Ok = true;
                            m.Output.Data = resultSearch;
                            response_toMain(m);
                        }
                        break;
                    #endregion
                    case _API.MEDIA_KEY_DOWNLOAD_PHOTO:
                        break;

                    case _API.MEDIA_KEY_FILTER_BOOKMAR_STAR:
                        #region

                        if (true)
                        {
                            //string input = (string)m.Input;
                            //if (input == null) input = string.Empty;
                            oMediaSearchLocalResult resultSearch = new oMediaSearchLocalResult();

                            List<long> lsSearch = new List<long>();
                            int min = (m.PageNumber - 1) * m.PageSize,
                                max = m.PageNumber * m.PageSize,
                                count = 0;
                            foreach (var kv in dicMediaStore)
                            {
                                if (kv.Value.Star)
                                {
                                    if (count >= min && count < max)
                                        lsSearch.Add(kv.Key);
                                    count++;
                                }
                            }
                            resultSearch.TotalItem = dicMediaStore.Count;
                            resultSearch.PageSize = m.PageSize;
                            resultSearch.PageNumber = m.PageNumber;
                            resultSearch.CountResult = count;
                            resultSearch.MediaIds = lsSearch;

                            m.Counter = count;
                            m.Output.Ok = true;
                            m.Output.Data = resultSearch;
                            response_toMain(m);
                        }

                        break;
                    #endregion
                    case _API.MEDIA_KEY_UPDATE_BOOKMARK_STAR:
                        #region

                        if (m.Input != null)
                        {
                            long mediaId = (long)m.Input;
                            oMedia mi = null;
                            if (dicMediaStore.TryGetValue(mediaId, out mi) && mi != null)
                            {
                                if (mi.Star)
                                    mi.Star = false;
                                else
                                    mi.Star = true;
                                f_media_writeFile();

                                m.Log = string.Format("Update bookmark set be {0} for {1}: {2}", mi.Star, mi.Title, "SUCCESSFULLY");
                                notification_toMain(m);
                            }
                        }

                        break;
                    #endregion
                    case _API.MEDIA_KEY_UPDATE_LENGTH:
                        break;
                    case _API.MEDIA_KEY_UPDATE_INFO:
                        break;
                    case _API.MEDIA_KEY_UPDATE_CAPTION:
                        break;
                    //case _API.MEDIA_YOUTUBE_INFO:
                    #region

                    //string videoId = "RQPSzkMNwcw";
                    //var _client = new YoutubeClient();
                    //// Get data
                    //var Video = _client.GetVideoAsync(videoId);
                    //var Channel = _client.GetVideoAuthorChannelAsync(videoId);
                    //var MediaStreamInfos = _client.GetVideoMediaStreamInfosAsync(videoId);
                    //var ClosedCaptionTrackInfos = _client.GetVideoClosedCaptionTrackInfosAsync(videoId);
                    //List<Video> video_result = _client.SearchVideosAsync("learn english subtitle");

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
                    #endregion
                    //    break;

                    #region [ SEARCH ONLINE ]

                    case _API.MEDIA_KEY_PLAY_VIDEO_ONLINE:
                        #region
                        if (true)
                        {
                            long mediaId = (long)m.Input;
                            string urlSrc = f_search_fetchUriSource(mediaId, MEDIA_TYPE.MP4);
                            if (!string.IsNullOrEmpty(urlSrc))
                            {
                                m.Output.Ok = true;
                                m.Output.Data = f_proxy_getUriProxy(mediaId, MEDIA_TYPE.MP4) + "&online=true";
                                response_toMain(m);
                            }
                            else
                            {
                                // cannot fetch uri
                            }
                        }
                        break;
                    #endregion
                    case _API.MEDIA_KEY_SEARCH_ONLINE_CACHE_CLEAR:
                        #region
                        Bitmap bitremove = null;
                        foreach (long mid in dicMediaSearch.Keys)
                            dicMediaImage.TryRemove(mid, out bitremove);
                        dicMediaSearch.Clear();

                        notification_toMain(new msg() { API = m.API, KEY = m.KEY, Log = "Clear all cache of result search successfully!" });
                        Execute(new msg() { API = m.API, KEY = _API.MEDIA_KEY_SEARCH_ONLINE_CACHE, Input = string.Empty });

                        break;
                    #endregion
                    case _API.MEDIA_KEY_SEARCH_ONLINE_SAVE_TO_STORE:
                        #region
                        if (true)
                        {
                            long mediaId = (long)m.Input;
                            oMedia mi = null;
                            if (dicMediaSearch.TryGetValue(mediaId, out mi) && mi != null)
                            {
                                if (!dicMediaStore.ContainsKey(mediaId))
                                {
                                    dicMediaStore.TryAdd(mediaId, mi);

                                    oMedia del_mi;
                                    dicMediaSearch.TryRemove(mediaId, out del_mi);

                                    f_media_writeFile();

                                    if (dicMediaImage.ContainsKey(mediaId))
                                    {
                                        string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "photo");
                                        if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);
                                        string filename = Path.Combine(dir, mediaId.ToString() + ".jpg");
                                        if (!File.Exists(filename))
                                        {
                                            Bitmap bitmap = null;
                                            if (dicMediaImage.TryGetValue(mediaId, out bitmap) && bitmap != null)
                                                //bitmap.Save(filename, ImageFormat.Jpeg);
                                                dicMediaImage.TryRemove(mediaId, out bitmap);
                                        }
                                    }

                                    notification_toMain(new appel.msg()
                                    {
                                        API = _API.MSG_MEDIA_SEARCH_SAVE_TO_STORE,
                                        Log = string.Format("{0} save successfully!", mi.Title),
                                    });
                                }
                            }
                        }
                        break;
                    #endregion
                    case _API.MEDIA_KEY_TEXT_VIDEO_ONLINE:
                        #region
                        if (true)
                        {
                            long mediaId = (long)m.Input;
                            oMedia mi = null;
                            if (dicMediaSearch.TryGetValue(mediaId, out mi) && mi != null)
                            {
                                string content = mi.Title;
                                if (!string.IsNullOrEmpty(mi.Description))
                                    content += Environment.NewLine + mi.Description;

                                if (mi.Keywords != null && mi.Keywords.Count > 0)
                                    content += Environment.NewLine + string.Join(" ", mi.Keywords);

                                string text = mi.SubtileEnglish;
                                if (!string.IsNullOrEmpty(text))
                                {
                                    text = Regex.Replace(text, @"<[^>]*>", String.Empty);
                                    text = HttpUtility.HtmlDecode(text);
                                    text = text.Replace('\r', ' ').Replace('\n', ' ');
                                    string[] a = text.Split(new char[] { '.' })
                                        .Select(x => x.Trim())
                                        .Where(x => x != string.Empty)
                                        .ToArray();
                                    text = string.Join(Environment.NewLine, a);
                                    text = text.Replace("?", "?" + Environment.NewLine);
                                    a = text.Split(new char[] { '\r', '\n' })
                                       .Select(x => x.Trim())
                                       .Where(x => x != string.Empty)
                                       .ToArray();
                                    text = string.Empty;
                                    foreach (string ti in a)
                                        if (ti[ti.Length - 1] == '?') text += ti + Environment.NewLine;
                                        else text += ti + "." + Environment.NewLine;
                                }

                                content += Environment.NewLine +
                                    "------------------------------------------------------------------"
                                    + Environment.NewLine + Environment.NewLine +
                                    text;

                                m.Output.Ok = true;
                                m.Output.Data = content;
                                response_toMain(m);

                                Execute(new msg() { API = _API.MEDIA, KEY = _API.MEDIA_KEY_PLAY_VIDEO_ONLINE, Input = mediaId });
                            }
                        }
                        break;
                    #endregion
                    case _API.MEDIA_KEY_SEARCH_ONLINE_CACHE:
                        #region

                        if (true)
                        {
                            string input = (string)m.Input;
                            if (input == null) input = string.Empty;
                            oMediaSearchLocalResult resultSearch = new oMediaSearchLocalResult();

                            List<long> lsSearch = new List<long>();
                            int min = (m.PageNumber - 1) * m.PageSize,
                                max = m.PageNumber * m.PageSize,
                                count = 0;
                            foreach (var kv in dicMediaSearch)
                            {
                                if (kv.Value.Title.ToLower().Contains(input)
                                    || kv.Value.Description.ToLower().Contains(input))
                                {
                                    if (count >= min && count < max)
                                        lsSearch.Add(kv.Key);
                                    count++;
                                }
                            }
                            resultSearch.TotalItem = dicMediaStore.Count;
                            resultSearch.PageSize = m.PageSize;
                            resultSearch.PageNumber = m.PageNumber;
                            resultSearch.CountResult = count;
                            resultSearch.MediaIds = lsSearch;

                            m.Counter = count;
                            m.Output.Ok = true;
                            m.Output.Data = resultSearch;
                            response_toMain(m);
                        }

                        break;
                    #endregion
                    case _API.MEDIA_KEY_SEARCH_ONLINE_STOP:
                        #region
                        // wait_search.WaitOne();
                        // wait_search.Reset(); // Need to pause the background thread
                        // wait_search.Set(); // Need to continue the background thread
                        stop_search = true;
                        break;
                    #endregion
                    case _API.MEDIA_KEY_SEARCH_ONLINE_NEXT:
                        #region
                        if (true)
                        {
                            string input = (string)m.Input;
                            if (input == null) input = string.Empty;
                            List<long> lsSearch = new List<long>();

                            if (!string.IsNullOrEmpty(input))
                            {
                                var _client = new YoutubeClient();
                                bool hasCC_Subtitle = true;
                                int page_query = m.PageNumber;
                                long[] aIDs = new long[] { };

                                do
                                {
                                    if (stop_search) break;

                                    page_query++;
                                    aIDs = f_media_searchYoutubeOnline(_client, input, page_query, hasCC_Subtitle);
                                    lsSearch.AddRange(aIDs);

                                    //if (aIDs.Length > 0)
                                    //{
                                    //    notification_toMain(new appel.msg()
                                    //    {
                                    //        API = _API.MSG_MEDIA_SEARCH_RESULT,
                                    //        Log = string.Format("Page {0} -> Total items: {1}: Search online [ {2} ] ...", page_query, lsSearch.Count, input),
                                    //    });
                                    //}

                                    //if (page_query == 2)
                                    //{
                                    //    new Thread(new ParameterizedThreadStart((object so) =>
                                    //    {
                                    //        Execute(new msg()
                                    //        {
                                    //            API = _API.MEDIA,
                                    //            KEY = _API.MEDIA_KEY_SEARCH_ONLINE_CACHE,
                                    //            Input = so
                                    //        });
                                    //    })).Start(input);
                                    //}

                                }
                                while (aIDs.Length != 0);
                                stop_search = false;


                                notification_toMain(new appel.msg()
                                {
                                    API = _API.MSG_MEDIA_SEARCH_RESULT,
                                    Log = string.Format(">>> Search online [ {0} ] completed", input),
                                });

                                //m.KEY = _API.MEDIA_KEY_SEARCH_ONLINE_CACHE;
                                ////m_first.Input = input;
                                //m.Input = string.Empty;
                                //m.Counter = lsSearch.Count;
                                //m.Output.Ok = true;
                                //m.Output.Data = new oMediaSearchLocalResult()
                                //{
                                //    CountResult = lsSearch.Count,
                                //    MediaIds = lsSearch.Take(m.PageSize).ToList(),
                                //    TotalItem = lsSearch.Count,
                                //};
                                //response_toMain(m);
                            }
                        }
                        break;
                    #endregion
                    case _API.MEDIA_KEY_SEARCH_ONLINE:
                        #region 
                        if (true)
                        {
                            string input = (string)m.Input;
                            if (input == null) input = string.Empty;
                            List<long> lsSearch = new List<long>();

                            if (!string.IsNullOrEmpty(input))
                            {
                                var _client = new YoutubeClient();
                                bool hasCC_Subtitle = true;
                                int page_query = 1;
                                long[] aIDs = f_media_searchYoutubeOnline(_client, input, page_query, hasCC_Subtitle);
                                lsSearch.AddRange(aIDs);

                                //while (lsSearch.Count < 10)
                                //{
                                //    page_query++;
                                //    aIDs = f_media_searchYoutubeOnline(_client, input, page_query, hasCC_Subtitle);
                                //    lsSearch.AddRange(aIDs);
                                //}

                                var m_first = m.clone(m.Input);
                                m_first.KEY = _API.MEDIA_KEY_SEARCH_ONLINE_CACHE;
                                //m_first.Input = input;
                                m_first.Input = string.Empty;
                                m_first.Counter = lsSearch.Count;
                                m_first.Output.Ok = true;
                                m_first.Output.Data = new oMediaSearchLocalResult()
                                {
                                    CountResult = lsSearch.Count,
                                    MediaIds = lsSearch.Take(m.PageSize).ToList(),
                                    TotalItem = lsSearch.Count,
                                };
                                response_toMain(m_first);

                                new Thread(new ParameterizedThreadStart((object so) =>
                                {
                                    Execute(new msg()
                                    {
                                        API = _API.MEDIA,
                                        KEY = _API.MEDIA_KEY_SEARCH_ONLINE_NEXT,
                                        PageNumber = page_query,
                                        Input = so
                                    });
                                })).Start(input);
                            }
                        }
                        break;

                        #endregion

                        #endregion
                }
            }
            return m;
        }

        public void Close()
        {
            proxy_Close();
        }

        #endregion

        #region [ MEDIA ]

        public static oMedia f_media_getInfo(long mediaId)
        {
            oMedia m = null;
            dicMediaStore.TryGetValue(mediaId, out m);
            return m;
        }

        public static bool f_media_Exist(long mediaId)
        {
            return dicMediaStore.ContainsKey(mediaId);
        }

        public static string f_media_getTitle(long mediaId)
        {
            string title = string.Empty;
            oMedia m = null;
            dicMediaStore.TryGetValue(mediaId, out m);
            if (m != null && !string.IsNullOrEmpty(m.Title)) title = m.Title;
            return title;
        }

        public static string f_media_getText(long mediaId)
        {
            string text = string.Empty;
            oMedia m = null;
            dicMediaStore.TryGetValue(mediaId, out m);
            if (m != null && !string.IsNullOrEmpty(m.Text)) text = m.Text;
            return text;
        }

        public static string[] f_media_getSentences(long mediaId)
        {
            oMedia m = null;
            dicMediaStore.TryGetValue(mediaId, out m);
            if (m != null && !string.IsNullOrEmpty(m.Text))
            {
                string text = m.SubtileEnglish_Text.ToLower()
                    //.Replace("\r", string.Empty)
                    //.Replace("\n", string.Empty)
                    .Replace('.', '^')
                    .Replace(" i i ", " i ")
                    .Replace('\r', ' ')
                    .Replace('\n', ' ')
                    .Replace("[music]", ".");

                string[] a = new string[] { };
                if (!string.IsNullOrEmpty(text))
                {
                    //a = text.ToLower().Split(new string[] {
                    //        "so", "and", "if", "when", "because",
                    //        "i'm","we're","you're","they're","he's","she's","it's",
                    //        "how","where","what","whom","who","which",
                    //        "i","we","you","they","he","she","it",
                    //    }, StringSplitOptions.None);

                    List<string> ls_key = new List<string>() { 
                        "my name",
                        "thank you",
                        "at this",
                        "and then",
                        "how are",
                    };
                    string[] aw = new string[] {
                        " let ",
                        " if ", " because ",
                        "how", "where", "what", "whom", "who", "which", "when",
                        "i'm", "we're", "you're", "they're", "he's", "she's", "it's",
                        " i "," we "," they "," he "," she ",
                        " let's ",
                        " but ",
                        " while ",
                        " now ",
                        //" and ",
                        //" it ",
                        //" you ",
                        //"   ",
                    };
                    foreach (string ai in aw)
                        text = text.Replace(ai, ". " + ai);

                    a = text.Split('.')
                        .Select(x => x.Trim())
                        .Where(x => x != string.Empty)
                        //.Select(x => x[0].ToString().ToUpper() + x.Substring(1))
                        .ToArray();

                    List<int> li = new List<int>();
                    string si = string.Empty;
                    string[] wds;
                    string[] asen = new string[a.Length - 1];
                    for (int i = 0; i < a.Length - 1; i++)
                    {
                        asen[i] = string.Empty;
                        si = a[i].Trim();

                        if (ls_key.IndexOf(si) != -1) continue;

                        si = si.Replace('^', '.');
                        wds = si.Split(' ');

                        if (i > 0)
                        {
                            if ((ls_key.IndexOf(a[i - 1]) != -1)
                                ||( a[i - 1].IndexOf(' ') == -1 && li.IndexOf(i - 1) == -1))
                            {
                                asen[i] = a[i - 1];
                                li.Add(i);
                            }
                        }

                        if (wds.Length > 1)
                        {
                            if (asen[i] == string.Empty)
                                asen[i] = si;
                            else if (!si.StartsWith(asen[i]))
                                asen[i] += " " + si;
                        }
                    }
                    
                    string s1 = string.Join(Environment.NewLine, a);
                    string s2 = string.Join(Environment.NewLine, asen);
                }
                return a;
            }
            return new string[] { };
        }

        public static oWordCount[] f_media_getWords(long mediaId)
        {
            oMedia m = null;
            dicMediaStore.TryGetValue(mediaId, out m);
            if (m != null && !string.IsNullOrEmpty(m.Text))
                return m.Words.ToArray();
            return new oWordCount[] { };
        }

        public static string f_media_getYoutubeID(long mediaId)
        {
            string url = string.Empty;
            oMedia p = null;
            if (dicMediaStore.TryGetValue(mediaId, out p) && p != null && p.Paths != null)
                return p.Paths.YoutubeID;
            return string.Empty;
        }

        public static string f_media_fetchUriSource(long mediaId, MEDIA_TYPE type)
        {
            string url = string.Empty;
            oMedia p = null;
            if (dicMediaStore.TryGetValue(mediaId, out p) && p != null && p.Paths != null)
            {
                if (!string.IsNullOrEmpty(p.Paths.YoutubeID))
                {
                    switch (type)
                    {
                        case MEDIA_TYPE.MP4:
                            #region
                            if (!string.IsNullOrEmpty(p.Paths.PathMp4_Youtube))
                            {
                                url = p.Paths.PathMp4_Youtube;
                            }
                            else
                            {
                                var _client = new YoutubeClient();
                                //var Video = _client.GetVideoAsync(videoId);
                                //var Channel = _client.GetVideoAuthorChannelAsync(videoId);
                                var ms = _client.GetVideoMediaStreamInfosAsync(p.Paths.YoutubeID);
                                var ms_video = ms.Muxed.Where(x => x.Container == Container.Mp4).Take(1).SingleOrDefault();
                                var ms_audio = ms.Audio.Where(x => x.Container == Container.M4A).Take(1).SingleOrDefault();

                                if (ms_video != null)
                                {
                                    p.Paths.PathMp4_Youtube = ms_video.Url;
                                    url = p.Paths.PathMp4_Youtube;
                                }

                                if (ms_audio != null)
                                {
                                    p.Paths.PathMp3_Youtube = ms_audio.Url;
                                }
                            }
                            #endregion
                            break;
                        case MEDIA_TYPE.M4A:
                            #region
                            if (!string.IsNullOrEmpty(p.Paths.PathMp3_Youtube))
                            {
                                url = p.Paths.PathMp3_Youtube;
                            }
                            else
                            {
                                var _client = new YoutubeClient();
                                //var Video = _client.GetVideoAsync(videoId);
                                //var Channel = _client.GetVideoAuthorChannelAsync(videoId);
                                var ms = _client.GetVideoMediaStreamInfosAsync(p.Paths.YoutubeID);
                                var ms_video = ms.Muxed.Where(x => x.Container == Container.Mp4).Take(1).SingleOrDefault();
                                var ms_audio = ms.Audio.Where(x => x.Container == Container.M4A).Take(1).SingleOrDefault();

                                if (ms_video != null)
                                {
                                    p.Paths.PathMp4_Youtube = ms_video.Url;
                                }

                                if (ms_audio != null)
                                {
                                    p.Paths.PathMp3_Youtube = ms_audio.Url;
                                    url = p.Paths.PathMp3_Youtube;
                                }
                            }
                            #endregion
                            break;
                        case MEDIA_TYPE.MP3:
                            break;
                    }
                }
            }
            return url;
        }

        private void f_media_writeFile()
        {
            using (var file = File.Create(file_media))
                Serializer.Serialize<ConcurrentDictionary<long, oMedia>>(file, dicMediaStore);
        }

        public static Bitmap f_image_getCache(long mediaId)
        {
            Bitmap bitmap = null;
            if (dicMediaImage.TryGetValue(mediaId, out bitmap) && bitmap != null)
            {
                return bitmap;
            }
            else
            {
                oMedia m = null;
                if (dicMediaStore.TryGetValue(mediaId, out m) && m != null)
                {
                    string imageUrl = string.Format("https://img.youtube.com/vi/{0}/default.jpg", m.Paths.YoutubeID);
                    try
                    {
                        WebClient client = new WebClient();
                        Stream stream = client.OpenRead(imageUrl);
                        bitmap = new Bitmap(stream);
                        //if (bitmap != null) bitmap.Save(filename, ImageFormat.Jpeg); 
                        //stream.Flush();
                        stream.Close();
                        client.Dispose();
                        dicMediaImage.TryAdd(mediaId, bitmap);
                    }
                    catch { }
                }
            }
            return bitmap;
        }

        #endregion

        #region [ SEARCH ]

        public static oMedia f_search_getInfo(long mediaId)
        {
            oMedia m = null;
            dicMediaSearch.TryGetValue(mediaId, out m);
            return m;
        }

        public static Bitmap f_search_getPhoto(long mediaId)
        {
            Bitmap m = null;
            dicMediaImage.TryGetValue(mediaId, out m);
            return m;
        }

        public static string f_search_getYoutubeID(long mediaId)
        {
            string url = string.Empty;
            oMedia p = null;
            if (dicMediaSearch.TryGetValue(mediaId, out p) && p != null)
                return p.Paths.YoutubeID;
            return string.Empty;
        }

        public static string f_search_fetchUriSource(long mediaId, MEDIA_TYPE type)
        {
            string url = string.Empty;
            oMedia p = null;
            if (dicMediaSearch.TryGetValue(mediaId, out p) && p != null && p.Paths != null)
            {
                if (!string.IsNullOrEmpty(p.Paths.YoutubeID))
                {
                    switch (type)
                    {
                        case MEDIA_TYPE.MP4:
                            #region
                            if (!string.IsNullOrEmpty(p.Paths.PathMp4_Youtube))
                            {
                                url = p.Paths.PathMp4_Youtube;
                            }
                            else
                            {
                                var _client = new YoutubeClient();
                                //var Video = _client.GetVideoAsync(videoId);
                                //var Channel = _client.GetVideoAuthorChannelAsync(videoId);
                                var ms = _client.GetVideoMediaStreamInfosAsync(p.Paths.YoutubeID);
                                var ms_video = ms.Muxed.Where(x => x.Container == Container.Mp4).Take(1).SingleOrDefault();
                                var ms_audio = ms.Audio.Where(x => x.Container == Container.M4A).Take(1).SingleOrDefault();

                                if (ms_video != null)
                                {
                                    p.Paths.PathMp4_Youtube = ms_video.Url;
                                    url = p.Paths.PathMp4_Youtube;
                                }

                                if (ms_audio != null)
                                {
                                    p.Paths.PathMp3_Youtube = ms_audio.Url;
                                }
                            }
                            #endregion
                            break;
                        case MEDIA_TYPE.M4A:
                            #region
                            if (!string.IsNullOrEmpty(p.Paths.PathMp3_Youtube))
                            {
                                url = p.Paths.PathMp3_Youtube;
                            }
                            else
                            {
                                var _client = new YoutubeClient();
                                //var Video = _client.GetVideoAsync(videoId);
                                //var Channel = _client.GetVideoAuthorChannelAsync(videoId);
                                var ms = _client.GetVideoMediaStreamInfosAsync(p.Paths.YoutubeID);
                                var ms_video = ms.Muxed.Where(x => x.Container == Container.Mp4).Take(1).SingleOrDefault();
                                var ms_audio = ms.Audio.Where(x => x.Container == Container.M4A).Take(1).SingleOrDefault();

                                if (ms_video != null)
                                {
                                    p.Paths.PathMp4_Youtube = ms_video.Url;
                                }

                                if (ms_audio != null)
                                {
                                    p.Paths.PathMp3_Youtube = ms_audio.Url;
                                    url = p.Paths.PathMp3_Youtube;
                                }
                            }
                            #endregion
                            break;
                        case MEDIA_TYPE.MP3:
                            break;
                    }
                }
            }
            return url;
        }

        static WebClient client_img = new WebClient();
        long[] f_media_searchYoutubeOnline(YoutubeClient _client, string query, int page, bool hasCC_Subtitle = true)
        {
            string url = string.Format("https://www.youtube.com/search_ajax?style=json&search_query={0}&page={1}&hl=en", query, page);
            if (hasCC_Subtitle) url += "&sp=EgIoAQ%253D%253D";

            string raw = _client.GetStringAsync(url, false);

            // Get search results
            var searchResultsJson = JToken.Parse(raw);

            // Get videos
            var videosJson = searchResultsJson["video"].EmptyIfNull().ToArray();

            // Break if there are no videos
            if (!videosJson.Any()) return new long[] { };

            // Get all videos across pages
            var videos = new List<long>();

            // Parse videos
            foreach (var videoJson in videosJson)
            {
                // Basic info
                string videoId = videoJson["encrypted_id"].Value<string>();
                oMedia mi = new oMedia(videoId);
                mi.Paths.YoutubeID = videoId;

                if (!dicMediaStore.ContainsKey(mi.Id) && !dicMediaSearch.ContainsKey(mi.Id))
                {
                    var cap = _client.GetVideoClosedCaptionTrackInfosAsync(videoId);
                    if (cap.Count > 0)
                    {
                        var cen = cap.Where(x => x.Language.Code == "en").Take(1).SingleOrDefault();
                        if (cen != null)
                        {
                            mi.Tags.Add(query);
                            mi.SubtileEnglish = _client.GetStringAsync(cen.Url);

                            var videoAuthor = videoJson["author"].Value<string>();
                            //var videoUploadDate = videoJson["added"].Value<string>().ParseDateTimeOffset("M/d/yy");
                            var videoUploadDate = videoJson["added"].Value<string>().ParseDateTimeOffset();
                            var videoTitle = videoJson["title"].Value<string>();
                            var videoDuration = TimeSpan.FromSeconds(videoJson["length_seconds"].Value<double>());
                            var videoDescription = videoJson["description"].Value<string>().HtmlDecode();

                            // Keywords
                            var videoKeywordsJoined = videoJson["keywords"].Value<string>();
                            var videoKeywords = Regex
                                .Matches(videoKeywordsJoined, @"(?<=(^|\s)(?<q>""?))([^""]|(""""))*?(?=\<q>(?=\s|$))")
                                .Cast<Match>()
                                .Select(m => m.Value)
                                .Where(s => s.IsNotBlank())
                                .ToList();

                            //// Statistics
                            //var videoViewCount = videoJson["views"].Value<string>().StripNonDigit().ParseLong();
                            //var videoLikeCount = videoJson["likes"].Value<long>();
                            //var videoDislikeCount = videoJson["dislikes"].Value<long>();
                            //var videoStatistics = new Statistics(videoViewCount, videoLikeCount, videoDislikeCount);

                            mi.ViewCount = videoJson["views"].Value<string>().StripNonDigit().ParseLong();

                            mi.DurationSecond = (int)videoDuration.TotalSeconds;
                            mi.Title = videoTitle;
                            mi.Description = videoDescription;
                            mi.Keywords = videoKeywords;
                            mi.Author = videoAuthor;
                            mi.UploadDate = int.Parse(videoUploadDate.ToString("yyMMdd"));


                            if (!dicMediaImage.ContainsKey(mi.Id))
                            {
                                using (Stream stream = client_img.OpenRead(string.Format("https://img.youtube.com/vi/{0}/default.jpg", videoId)))
                                {
                                    Bitmap bitmap = new Bitmap(stream);
                                    dicMediaImage.TryAdd(mi.Id, bitmap);
                                }
                            }

                            videos.Add(mi.Id);

                            dicMediaSearch.TryAdd(mi.Id, mi);
                        }
                    }
                }
            }

            return videos.ToArray();
        }

        #endregion

        #region [ SUBTITLE - CC ]

        public static List<oCaptionWord> f_analytic_wordFileXml(string file_xml)
        {
            XDocument xdoc = XDocument.Load(file_xml);
            List<oCaptionWord> listWord = new List<oCaptionWord>();
            foreach (var p in xdoc.Descendants("p"))
            {
                var its = p.Descendants("s").Select(x => new oCaptionWord(x)).ToArray();
                if (its.Length > 0)
                {
                    int tt = 0, dd = 0;
                    string t = p.Attribute("t").Value, d = p.Attribute("d").Value;
                    if (!string.IsNullOrEmpty(t)) int.TryParse(t, out tt);
                    if (!string.IsNullOrEmpty(d)) int.TryParse(d, out dd);
                    foreach (var it in its) it.TimeStart += tt;
                    listWord.AddRange(its);
                }
            }

            return listWord;
        }

        public static List<oCaptionSentence> f_render_Sentence(List<oCaptionWord> listWord)
        {
            List<oCaptionSentence> listSen = new List<oCaptionSentence>();
            oCaptionWord ci = null;
            oCaptionSentence si = new oCaptionSentence();
            string wi = string.Empty, wii = string.Empty;
            for (var i = 0; i < listWord.Count; i++)
            {
                ci = listWord[i];
                wi = ci.Word.Trim().ToLower();

                if (i == 0)
                {
                    si = new oCaptionSentence();
                    si.TimeStart = ci.TimeStart;
                    si.ListIndex.Add(i);
                    continue;
                }

                if (wi == "i" || wi == "we" || wi == "you" || wi == "they" || wi == "he" || wi == "she" || wi == "it"
                    || wi == "i'm" || wi == "we're" || wi == "you're" || wi == "they're" || wi == "he's" || wi == "she's" || wi == "it's"
                    || wi == "how" || wi == "where" || wi == "what" || wi == "whom" || wi == "who" || wi == "which")
                {
                    bool sub = false;
                    wii = listWord[i - 1].Word.ToLower();
                    if (i > 0 &&
                        (wii == "so" || wii == "and" || wii == "if" || wii == "when" || wii == "because"))
                    {
                        sub = true;
                        si.ListIndex.RemoveAt(si.ListIndex.Count - 1);
                    }

                    var ws = listWord.Where((x, id) => si.ListIndex.Any(y => y == id)).Select(x => x.Word).ToArray();
                    si.Words = string.Join(" ", ws);
                    listSen.Add(si);

                    si = new oCaptionSentence();
                    si.TimeStart = ci.TimeStart;
                    if (sub) si.ListIndex.Add(i - 1);
                    si.ListIndex.Add(i);
                }
                else
                {
                    si.ListIndex.Add(i);
                }
            }

            return listSen;
        }

        #endregion

        #region [ PROXY ]

        static int m_port = 0;
        static HttpListener m_listener;
        static bool m_running = true;

        void f_proxy_Start()
        {
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
                            if (string.IsNullOrEmpty(key))
                            {
                                byte[] bytes = Encoding.UTF8.GetBytes(string.Format("Cannot find key: /?key=???"));
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

        string f_proxy_getUriProxy(long mediaId, MEDIA_TYPE type)
        {
            string key = string.Format("{0}{1}", type, mediaId);
            return string.Format("http://localhost:{0}/?key={1}", m_port, key);
        }

        void proxy_Close()
        {
            m_listener.Stop();
            m_running = false;
            Thread.Sleep(10);
        }

        #endregion
    }

    #region [ PROXY ]


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
            if (key.Length > 3)
            {
                type = key.Substring(0, 3).ToLower();
                key = key.Substring(3);
            }

            long mediaId = 0;
            long.TryParse(key, out mediaId);

            switch (type)
            {
                case "m4a":
                    uri = api_media.f_media_fetchUriSource(mediaId, MEDIA_TYPE.M4A);
                    break;
                case "mp4":
                    string online = originalContext.Request.QueryString["online"];
                    if (online == "true")
                        uri = api_media.f_search_fetchUriSource(mediaId, MEDIA_TYPE.MP4);
                    else
                        uri = api_media.f_media_fetchUriSource(mediaId, MEDIA_TYPE.MP4);
                    break;
                case "web": // webm
                    uri = api_media.f_media_fetchUriSource(mediaId, MEDIA_TYPE.WEB);
                    break;
                case "mp3":
                    uri = api_media.f_media_fetchUriSource(mediaId, MEDIA_TYPE.MP3);
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
            var requestData = (RequestState)asynchronousResult.AsyncState;
            ConsoleUtilities.WriteResponse("Proxy receive a response from " + requestData.context.Request.RawUrl);

            using (var responseFromWebSiteBeingRelayed = (HttpWebResponse)requestData.webRequest.EndGetResponse(asynchronousResult))
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
                        try
                        {
                            responseStreamFromWebSiteBeingRelayed.CopyTo(originalResponse.OutputStream);
                        }
                        catch { }
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

    #endregion

    public enum MEDIA_TYPE
    {
        AAC,
        M4A,
        MP3,
        MP4,
        WEB, // WEBM
    }

    public enum MEDIA_TAB
    {
        TAB_STORE = 1,
        TAB_SEARCH = 2,
    }
}
