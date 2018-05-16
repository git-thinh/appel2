using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace appel
{
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
    }
}
