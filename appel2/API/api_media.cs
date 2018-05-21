using ProtoBuf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using YoutubeExplode;
using YoutubeExplode.Models;
using YoutubeExplode.Models.MediaStreams;

namespace appel
{
    public class api_media : api_base, IAPI
    {
        const string file_media = "minfo.bin";
        const string file_path = "mpath.bin";

        public bool Open { set; get; } = false;

        static ConcurrentDictionary<long, oMedia> dicMedia = null;
        static ConcurrentDictionary<long, oMediaPath> dicPath = null;


        public void Init()
        {
            dicMedia = new ConcurrentDictionary<long, oMedia>();
            dicPath = new ConcurrentDictionary<long, oMediaPath>();

            if (File.Exists(file_media))
                using (var file = File.OpenRead(file_media))
                    dicMedia = Serializer.Deserialize<ConcurrentDictionary<long, oMedia>>(file);

            if (File.Exists(file_path))
                using (var file = File.OpenRead(file_path))
                    dicPath = Serializer.Deserialize<ConcurrentDictionary<long, oMediaPath>>(file);

            //if (dicMedia.Count > 0)
            //{
            //    foreach (var kv in dicMedia)
            //    {
            //        string con = kv.Value.Title;
            //        if (!string.IsNullOrEmpty(kv.Value.Description))
            //            con += Environment.NewLine + kv.Value.Description;

            //        if (kv.Value.Keywords != null && kv.Value.Keywords.Count > 0)
            //            con += Environment.NewLine + string.Join(" ", kv.Value.Keywords);

            //        api_word.f_word_addContentAnaltic(kv.Key, con);
            //    }
            //}
            f_proxy_Start();
        }

        #region [ MEDIA ]

        public static oMedia f_media_getInfo(long mediaId)
        {
            oMedia m = null;
            dicMedia.TryGetValue(mediaId, out m);
            return m;
        }

        public static string f_media_getTitle(long mediaId)
        {
            string title = string.Empty;
            oMedia m = null;
            dicMedia.TryGetValue(mediaId, out m);
            if (m != null && !string.IsNullOrEmpty(m.Title)) title = m.Title;
            return title;
        }

        public static string f_media_getYoutubeID(long mediaId)
        {
            string url = string.Empty;
            oMediaPath p = null;
            if (dicPath.TryGetValue(mediaId, out p) && p != null)
                return p.YoutubeID;
            return string.Empty;
        }

        public static string f_media_fetchUriSource(long mediaId, MEDIA_TYPE type)
        {
            string url = string.Empty;
            oMediaPath p = null;
            if (dicPath.TryGetValue(mediaId, out p))
            {
                if (!string.IsNullOrEmpty(p.YoutubeID))
                {
                    switch (type)
                    {
                        case MEDIA_TYPE.MP4:
                            #region
                            if (!string.IsNullOrEmpty(p.PathMp4_Youtube))
                            {
                                url = p.PathMp4_Youtube;
                            }
                            else
                            {
                                var _client = new YoutubeClient();
                                //var Video = _client.GetVideoAsync(videoId);
                                //var Channel = _client.GetVideoAuthorChannelAsync(videoId);
                                var ms = _client.GetVideoMediaStreamInfosAsync(p.YoutubeID);
                                var ms_video = ms.Muxed.Where(x => x.Container == Container.Mp4).Take(1).SingleOrDefault();
                                var ms_audio = ms.Audio.Where(x => x.Container == Container.M4A).Take(1).SingleOrDefault();

                                if (ms_video != null)
                                {
                                    p.PathMp4_Youtube = ms_video.Url;
                                    url = p.PathMp4_Youtube;
                                }

                                if (ms_audio != null)
                                {
                                    p.PathMp3_Youtube = ms_audio.Url;
                                }
                            }
                            #endregion
                            break;
                        case MEDIA_TYPE.M4A:
                            #region
                            if (!string.IsNullOrEmpty(p.PathMp3_Youtube))
                            {
                                url = p.PathMp3_Youtube;
                            }
                            else
                            {
                                var _client = new YoutubeClient();
                                //var Video = _client.GetVideoAsync(videoId);
                                //var Channel = _client.GetVideoAuthorChannelAsync(videoId);
                                var ms = _client.GetVideoMediaStreamInfosAsync(p.YoutubeID);
                                var ms_video = ms.Muxed.Where(x => x.Container == Container.Mp4).Take(1).SingleOrDefault();
                                var ms_audio = ms.Audio.Where(x => x.Container == Container.M4A).Take(1).SingleOrDefault();

                                if (ms_video != null)
                                {
                                    p.PathMp4_Youtube = ms_video.Url;
                                }

                                if (ms_audio != null)
                                {
                                    p.PathMp3_Youtube = ms_audio.Url;
                                    url = p.PathMp3_Youtube;
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

        #endregion

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
                            foreach (var kv in dicMedia)
                            {
                                if (kv.Value.Title.ToLower().Contains(input)
                                    || kv.Value.Description.ToLower().Contains(input))
                                {
                                    if (count >= min && count < max)
                                        lsSearch.Add(kv.Key);
                                    count++;
                                }
                            }
                            resultSearch.TotalItem = dicMedia.Count;
                            resultSearch.PageSize = m.PageSize;
                            resultSearch.PageNumber = m.PageNumber;
                            resultSearch.CountResult = count;
                            resultSearch.MediaIds = lsSearch;

                            m.Counter = count;
                            m.Output.Ok = true;
                            m.Output.Data = resultSearch;
                            response_toMain(m);

                            //if (m.PageNumber == 1 && !string.IsNullOrEmpty(input))
                            //{
                            //    if (count > 0)
                            //    {
                            //        var m2 = m.clone();
                            //        m2.Input = input;
                            //        m2.Output.Data = resultSearch.clone();
                            //        response_toMain(m2);
                            //    }

                            //    var _client = new YoutubeClient();
                            //    List<Video> rs = _client.SearchVideosAsync(input);
                            //    bool hasUpdate = false;
                            //    int _cc = 1;
                            //    List<long> lsMediaID = new List<long>();
                            //    foreach (var v in rs)
                            //    {
                            //        oMedia me = new oMedia(v);
                            //        if (!dicMedia.ContainsKey(me.Id))
                            //        {
                            //            string videoId = v.Id;
                            //            oMediaPath pe = new oMediaPath(me.Id, videoId);

                            //            var cap = _client.GetVideoClosedCaptionTrackInfosAsync(videoId);
                            //            if (cap.Count > 0)
                            //            {
                            //                var sub = cap.Where(x => x.Language.Code == "en").Take(1).SingleOrDefault();
                            //                if (sub != null)
                            //                {
                            //                    me.SubtileEnglish = _client.GetStringAsync(sub.Url);

                            //                    dicMedia.TryAdd(me.Id, me);
                            //                    dicPath.TryAdd(me.Id, pe);

                            //                    f_image_loadInit(me.Id);

                            //                    if (_cc <= 10)
                            //                        lsMediaID.Add(me.Id);

                            //                    if (_cc == 10)
                            //                    {
                            //                        var m2 = m.clone();
                            //                        m2.Input = input;
                            //                        m2.Counter = _cc;
                            //                        m2.Output.Data = new oMediaSearchLocalResult() {
                            //                            CountResult = _cc,
                            //                            MediaIds = lsMediaID,
                            //                            TotalItem = _cc,
                            //                        };
                            //                        response_toMain(m2); 
                            //                    }

                            //                    if (hasUpdate == false) hasUpdate = true;
                            //                    _cc++; 
                            //                    notification_toMain(new appel.msg()
                            //                    {
                            //                        API = _API.MSG_MEDIA_SEARCH_RESULT,
                            //                        Log = string.Format("{0} - {1}: {2}", _cc, rs.Count, me.Title),
                            //                    });
                            //                }
                            //            }

                            //            //string con = me.Title;
                            //            //if (!string.IsNullOrEmpty(me.Description))
                            //            //    con += Environment.NewLine + me.Description;

                            //            //if (me.Keywords != null && me.Keywords.Count > 0)
                            //            //    con += Environment.NewLine + string.Join(" ", me.Keywords);

                            //            //api_word.f_word_addContentAnaltic(me.Id, con);

                            //            //f_image_loadInit(me.Id);
                            //        }
                            //    }
                            //    if (hasUpdate)
                            //    {
                            //        f_media_writeFile();
                            //        notification_toMain(new appel.msg()
                            //        {
                            //            API = _API.MSG_MEDIA_SEARCH_RESULT,
                            //            Log = string.Format("{0} - {1}: Complete search online", _cc, rs.Count),
                            //        });
                            //        Execute(m);
                            //    }
                            //}
                            //else
                            //{
                            //    response_toMain(m);
                            //}
                        }
                        break;

                    #endregion
                    case _API.MEDIA_KEY_SEARCH_ONLINE:
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

                            if (!string.IsNullOrEmpty(input))
                            {
                                var _client = new YoutubeClient();
                                List<Video> rs = _client.SearchVideosAsync(input);
                                bool hasUpdate = false;
                                int _cc = 1;
                                List<long> lsMediaID = new List<long>();
                                foreach (var v in rs)
                                {
                                    oMedia me = new oMedia(v);
                                    if (!dicMedia.ContainsKey(me.Id))
                                    {
                                        string videoId = v.Id;
                                        oMediaPath pe = new oMediaPath(me.Id, videoId);

                                        ////var cap = _client.GetVideoClosedCaptionTrackInfosAsync(videoId);
                                        ////if (cap.Count > 0)
                                        ////{
                                        ////    var sub = cap.Where(x => x.Language.Code == "en").Take(1).SingleOrDefault();
                                        ////    if (sub != null)
                                        ////    {
                                        //?????????????????????????????????
                                        //me.SubtileEnglish = _client.GetStringAsync(sub.Url);

                                        dicMedia.TryAdd(me.Id, me);
                                        dicPath.TryAdd(me.Id, pe);

                                        //////f_image_loadInit(me.Id);

                                        if (_cc <= 10)
                                            lsMediaID.Add(me.Id);

                                        if (_cc == 10)
                                        {
                                            var m2 = m.clone(m.Input);
                                            m2.KEY = _API.MEDIA_KEY_SEARCH_STORE;
                                            m2.Input = input;
                                            m2.Counter = _cc;
                                            m2.Output.Ok = true;
                                            m2.Output.Data = new oMediaSearchLocalResult()
                                            {
                                                CountResult = _cc,
                                                MediaIds = lsMediaID,
                                                TotalItem = _cc,
                                            };
                                            response_toMain(m2);
                                        }

                                        if (hasUpdate == false) hasUpdate = true;
                                        _cc++;
                                        notification_toMain(new appel.msg()
                                        {
                                            API = _API.MSG_MEDIA_SEARCH_RESULT,
                                            Log = string.Format("{0} - {1}: {2}", _cc, rs.Count, me.Title),
                                        });

                                        ////    }
                                        ////}

                                        //string con = me.Title;
                                        //if (!string.IsNullOrEmpty(me.Description))
                                        //    con += Environment.NewLine + me.Description;

                                        //if (me.Keywords != null && me.Keywords.Count > 0)
                                        //    con += Environment.NewLine + string.Join(" ", me.Keywords);

                                        //api_word.f_word_addContentAnaltic(me.Id, con);

                                        //f_image_loadInit(me.Id);
                                    }
                                }
                                if (hasUpdate)
                                {
                                    //f_media_writeFile();
                                    notification_toMain(new appel.msg()
                                    {
                                        API = _API.MSG_MEDIA_SEARCH_RESULT,
                                        Log = string.Format("{0} - {1}: Complete search online", _cc, rs.Count),
                                    });
                                    //Execute(m);
                                }
                            }
                        }
                        break;

                    #endregion
                    case _API.MEDIA_KEY_DOWNLOAD_PHOTO:
                        break;
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
                }
            }
            return m;
        }
        public void Close()
        {
            proxy_Close();
        }

        private void f_media_writeFile()
        {
            using (var file = File.Create(file_media))
                Serializer.Serialize<ConcurrentDictionary<long, oMedia>>(file, dicMedia);

            using (var file = File.Create(file_path))
                Serializer.Serialize<ConcurrentDictionary<long, oMediaPath>>(file, dicPath);
        }

        private void f_image_loadInit(long mediaId)
        {
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "photo");
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);

            string filename = Path.Combine(dir, mediaId.ToString() + ".jpg");
            if (!File.Exists(filename))
            {
                oMediaPath m;
                if (dicPath.TryGetValue(mediaId, out m))
                {
                    string imageUrl = string.Format("https://img.youtube.com/vi/{0}/default.jpg", m.YoutubeID);
                    try
                    {
                        WebClient client = new WebClient();
                        Stream stream = client.OpenRead(imageUrl);
                        Bitmap bitmap = new Bitmap(stream);

                        if (bitmap != null)
                            bitmap.Save(filename, ImageFormat.Jpeg);

                        stream.Flush();
                        stream.Close();
                        client.Dispose();
                    }
                    catch { }
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////
















        //var lw1 = api_youtube.f_analytic_wordFileXml("demo1.xml");
        //var ls1 = api_youtube.f_render_Sentence(lw1);

        //var lw2 = api_youtube.f_analytic_wordFileXml("demo2.xml");
        //var ls2 = api_youtube.f_render_Sentence(lw2);

        //string text = string.Empty;
        //foreach (var se in ls2) text += se.TimeStart + ": " + se.Words + Environment.NewLine;

        //string videoId = "RQPSzkMNwcw";
        //var _client = new YoutubeClient();
        //// Get data
        //var Video = _client.GetVideoAsync(videoId);
        //var Channel = _client.GetVideoAuthorChannelAsync(videoId);
        //var MediaStreamInfos = _client.GetVideoMediaStreamInfosAsync(videoId);
        //var ClosedCaptionTrackInfos = _client.GetVideoClosedCaptionTrackInfosAsync(videoId);
        //List<Video> video_result = _client.SearchVideosAsync("learn english subtitle");
        //string json = Newtonsoft.Json.JsonConvert.SerializeObject(video_result);

        //List<Video> video_result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Video>>(File.ReadAllText("videos.json"));

        //using (var file = File.Create("videos.bin"))
        //{
        //    Serializer.Serialize<List<Video>>(file, video_result);
        //} 
        //using (var file = File.OpenRead("videos.bin"))
        //{
        //    var lvs = Serializer.Deserialize<List<Video>>(file);

        //}


        //public static string f_get_uriProxy(string videoId, MEDIA_TYPE type)
        //{

        //    string uri = string.Empty;
        //    uri = api_media_Proxy.f_get_uriProxy(videoId, type);
        //    if (string.IsNullOrEmpty(uri))
        //    {
        //        var _client = new YoutubeClient();
        //        // Get data
        //        //var video = _client.GetVideoAsync(videoId);
        //        //var chanel = _client.GetVideoAuthorChannelAsync(videoId);
        //        var media = _client.GetVideoMediaStreamInfosAsync(videoId);
        //        //var caption = _client.GetVideoClosedCaptionTrackInfosAsync(videoId);

        //        api_media_Proxy.f_add_URL(videoId, media);

        //        uri = api_media_Proxy.f_get_uriProxy(videoId, type);
        //    }

        //    return uri;
        //}

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

}
