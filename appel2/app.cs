using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using YoutubeExplode;

namespace appel
{
    public class app
    {
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

        static fPlayer player;

        public static IFORM get_Main() {
            return player;
        }

        public static void postMessageToService(msg m) {

        }

        public static void RUN()
        {
            //string videoId = "xHRpbjTZ9zU";
            //var _client = new YoutubeClient();
            //// Get data
            //var Video = _client.GetVideoAsync(videoId);
            //var Channel = _client.GetVideoAuthorChannelAsync(videoId);
            //var MediaStreamInfos = _client.GetVideoMediaStreamInfosAsync(videoId);
            //var ClosedCaptionTrackInfos = _client.GetVideoClosedCaptionTrackInfosAsync(videoId);

            player = new fPlayer();
            player.Shown += (se, ev) => {
                string path = string.Empty;
                path = @"http://localhost:7777/?type=mp4";
                //path = @"http://localhost:7777/?type=m4a";
                //path = @"http://localhost:7777/?type=mp3";
                player.f_play(path);
            };

            Application.EnableVisualStyles();
            Application.Run(player);
        }
    }

    class Program
    { 
        [STAThread]
        static void Main(string[] args) {
            app.RUN();
        }
    }
}
