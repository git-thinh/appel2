using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using YoutubeExplode;
using YoutubeExplode.Models;

namespace appel
{
    public class app
    {
        public const int m_item_width = 320;
        public const int m_item_height = 180;
        static fMain main;
        static Dictionary<string, VideoInfo> m_dicVideo = new Dictionary<string, VideoInfo>();

        static app()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (se, ev) =>
            {
                Assembly asm = null;
                string comName = ev.Name.Split(',')[0];
                string resourceName = @"LIB\DLL\" + comName + ".dll";
                var assembly = Assembly.GetExecutingAssembly();
                resourceName = typeof(app).Namespace + "." + resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".");
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        //Debug.WriteLine(resourceName);
                    }
                    else
                    {
                        byte[] buffer = new byte[stream.Length];
                        using (MemoryStream ms = new MemoryStream())
                        {
                            int read;
                            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                                ms.Write(buffer, 0, read);
                            buffer = ms.ToArray();
                        }
                        asm = Assembly.Load(buffer);
                    }
                }
                return asm;
            };
        }

        public static void f_youtube_Open(string videoId, string title)
        {
            main.Cursor = Cursors.WaitCursor;

            string url = string.Empty;
            if (m_dicVideo.ContainsKey(videoId))
            {
                var mi = m_dicVideo[videoId];
                if (mi != null && mi.Media != null && mi.Media.Muxed.Count > 0)
                {
                    var mp4 = mi.Media.Muxed.Where(x => x.Container == YoutubeExplode.Models.MediaStreams.Container.Mp4).Take(1).SingleOrDefault();
                    if (mp4 != null)
                        url = mp4.Url;
                }
            }
            if (url == string.Empty)
            {
                var _client = new YoutubeClient();
                // Get data
                //var video = _client.GetVideoAsync(videoId);
                //var chanel = _client.GetVideoAuthorChannelAsync(videoId);
                var media = _client.GetVideoMediaStreamInfosAsync(videoId);
                //var caption = _client.GetVideoClosedCaptionTrackInfosAsync(videoId);
                if (media.Muxed.Count > 0)
                {
                    var mp4 = media.Muxed.Where(x => x.Container == YoutubeExplode.Models.MediaStreams.Container.Mp4).Take(1).SingleOrDefault();
                    if (mp4 != null)
                        url = mp4.Url;
                }

                if (!m_dicVideo.ContainsKey(videoId))
                    m_dicVideo.Add(videoId, new VideoInfo() { Media = media });
                else
                    m_dicVideo[videoId].Media = media;
            }

            if (url != string.Empty)
            {
                new fPlayer(url, title).Show();
            }
            else
                MessageBox.Show("Cannot open videoId: " + title);

            main.Cursor = Cursors.Default;
        }

        public static void RUN()
        {
            RuntimeTypeModel.Default.Add(typeof(DateTimeOffset), false).SetSurrogate(typeof(DateTimeOffsetSurrogate));

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

            main = new fMain();
            main.Shown += (se, ev) =>
            {
                main.Width = m_item_width * 2 + 45;
                main.Height = m_item_height * 5;
                main.Top = Screen.PrimaryScreen.WorkingArea.Height - (main.Height + 45);
                main.Left = Screen.PrimaryScreen.WorkingArea.Width - (main.Width + 45);

            };


            //string path = string.Empty;
            ////path = @"http://localhost:7777/?type=mp4";
            ////path = @"http://localhost:7777/?type=m4a";
            ////path = @"http://localhost:7777/?type=mp3";
            ////path = @"G:\_EL\Document\data_el2\media\files\video.mp4";
            ////path = @"G:\_EL\Document\data_el2\media\files\3.mp4";
            ////path = @"https://r3---sn-8qj-i5ols.googlevideo.com/videoplayback?source=youtube&mt=1526388841&mv=m&ms=au%2Crdu&ip=14.177.123.70&key=yt6&c=WEB&dur=8944.047&itag=22&pl=20&mime=video%2Fmp4&mm=31%2C29&sparams=dur%2Cei%2Cid%2Cinitcwndbps%2Cip%2Cipbits%2Citag%2Clmt%2Cmime%2Cmm%2Cmn%2Cms%2Cmv%2Cpl%2Cratebypass%2Crequiressl%2Csource%2Cexpire&mn=sn-8qj-i5ols%2Csn-nv47lnly&id=o-ALaMTgiUsCYKmGhqjlqpkbquWJoaYRFz17H8fIKLaGNX&expire=1526410530&ei=wtj6WsLLC4LeqQHBtryYBQ&ratebypass=yes&fvip=3&lmt=1519643541844596&initcwndbps=885000&requiressl=yes&ipbits=0&signature=0B9374EFB658C87E97146D8A0CF84ED69CB0BAA6.066A39ED106D0D44B8072978C7517096E8769286";
            ////path = @"https://r2---sn-8qj-i5ole.googlevideo.com/videoplayback?expire=1526411244&ipbits=0&signature=4B2CA3B9C5F2C008090EFF7193A26BE060C28BB8.60A7A60846B84DDFF903D73D18DDC7D6843D3D22&requiressl=yes&lmt=1520501837846008&ratebypass=yes&itag=22&c=WEB&key=yt6&mime=video%2Fmp4&id=o-AFfW8oloWYChJia0H715UyKcopAiwtD7l0MPGmi-6KHS&dur=120.534&pl=20&source=youtube&sparams=dur%2Cei%2Cid%2Cinitcwndbps%2Cip%2Cipbits%2Citag%2Clmt%2Cmime%2Cmm%2Cmn%2Cms%2Cmv%2Cpl%2Cratebypass%2Crequiressl%2Csource%2Cexpire&mv=m&initcwndbps=1008750&fvip=2&ms=au%2Crdu&ip=14.177.123.70&mm=31%2C29&mn=sn-8qj-i5ole%2Csn-npoeene6&ei=jNv6WrrxJMy04AKp24HwDw&mt=1526389548";
            ////path = @"demo2.mp4";
            //path = @"demo1.mp4";
            //fPlayer player = new fPlayer(path, "TEST"); 

            Application.EnableVisualStyles();
            //Application.Run(player);
            Application.Run(main);
        }

        #region

        public static IFORM get_Main()
        {
            return main;
        }

        public static void postMessageToService(msg m)
        {

        }

        public static void Exit()
        {
            //main.f_free_Resource();
            Application.Exit();
        }


        #endregion
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            app.RUN();
        }
    }
}
