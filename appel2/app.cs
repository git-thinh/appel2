 using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using YoutubeExplode;
using YoutubeExplode.Models;

namespace appel
{
    [PermissionSet(SecurityAction.LinkDemand, Name = "Everything"),
    PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    public class app
    {
        #region [ VARIABLE ]

        public const int m_item_width = 120;
        public const int m_item_height = 90;

        public const int m_box_width = 320;
        public const int m_box_height = 180;

        public const int m_app_width = m_box_width * 2 + 29;
        public const int m_app_height = 569;

        public const int m_player_width = 640;
        public const int m_player_height = 360;

        static fMain main;
        static fPlayer media;

        #endregion

        #region [ API ]

        static ConcurrentDictionary<string, IthreadMsg> dicService = null;
        static ConcurrentDictionary<string, msg> dicResponses = null;

        static ConcurrentQueue<msg> api_msg_queue = null;
        static System.Threading.Timer api_msg_timer = null;

        public static void postToAPI(msg m)
        {
            api_msg_queue.Enqueue(m);
        }

        public static void postToAPI(string api, string key, object input)
        {
            postToAPI(new msg() { API = api, KEY = key, Input = input });
        }

        private static void f_api_init()
        {
            api_msg_queue = new ConcurrentQueue<msg>();
            if (api_msg_timer == null)
                api_msg_timer = new System.Threading.Timer(new System.Threading.TimerCallback((obj) =>
                {
                    if (api_msg_queue.Count > 0)
                    {
                        msg m = null;
                        if (api_msg_queue.TryDequeue(out m) && m != null)
                        {
                            if (!string.IsNullOrEmpty(m.API) && dicService.ContainsKey(m.API))
                            {
                                IthreadMsg sv;
                                if (dicService.TryGetValue(m.API, out sv) && sv != null)
                                {
                                    ////new Thread(new ParameterizedThreadStart((object _sv) =>
                                    ////{
                                    ////    IthreadMsg so = (IthreadMsg)_sv;
                                    ////    so.Execute(m);
                                    ////})).Start(sv);
                                    sv.Execute(m);
                                }
                            }
                        }
                    }
                }), null, 100, 100);

            dicResponses = new ConcurrentDictionary<string, msg>();
            dicService = new ConcurrentDictionary<string, IthreadMsg>();

            //dicService.TryAdd(_API.WORD, new threadMsg(new api_word()));
            dicService.TryAdd(_API.MEDIA, new threadMsg(new api_media()));
        }

        #endregion

        #region [ MEDIA ]

        static void f_player_init()
        {
            media = new fPlayer();
            media.FormBorderStyle = FormBorderStyle.None;
            media.ShowInTaskbar = false;
            media.StartPosition = FormStartPosition.Manual;
            media.Location = new System.Drawing.Point(-2000, -2000);
            media.Width = 1;
            media.Shown += (se, ev) =>
            {
                media.f_init();
            };
            media.Show();
            media.Hide();
        }

        public static void f_player_Open(string url, string title)
        {
            media.Invoke((Action)(() =>
            {
                media.Show();
                media.ShowInTaskbar = true;
                media.open(url, title);
            }));
        }

        public static void f_player_Hide()
        {
            media.ShowInTaskbar = false;
            media.Hide();
        }

        public static void f_player_Close()
        {
            media.stop();
            media.ShowInTaskbar = false;
            media.Hide();
        }

        #endregion

        #region [ MAIN ]

        private static void main_Shown(object sender, EventArgs e)
        {
            app.postToAPI(_API.MEDIA, _API.MEDIA_KEY_SEARCH_STORE, string.Empty);
            //main.for_test();
        }

        private static void f_main_init()
        {
            main = new fMain();
            main.StartPosition = FormStartPosition.CenterScreen;
            main.Width = m_app_width;
            main.Height = m_app_height;
            main.Shown += main_Shown;
            main.FormClosing += main_Closing;

            Application.EnableVisualStyles();
            Application.Run(main);
        }

        private static void main_Closing(object sender, FormClosingEventArgs e)
        {
            if (main == null) return;

            //var confirmResult = MessageBox.Show("Are you sure to exit this application ?", "Confirm Exit!", MessageBoxButtons.YesNo);
            //if (confirmResult == DialogResult.Yes)
            //{
            foreach (var kv in dicService)
                if (kv.Value != null)
                    kv.Value.Stop();

            media.f_form_freeResource();
            media.Close();
            main.f_form_freeResource();

            Application.ExitThread();
            // wait for complete threads, free resource
            //Thread.Sleep(30);
            Application.Exit();
            //}
            //else
            //    e.Cancel = true;
        }


        public static IFORM get_Main()
        {
            return main;
        }

        #endregion

        #region [ APP ]

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

        public static void RUN()
        {
            // active SSL 1.1, 1.2, 1.3 for WebClient request HTTPS
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | (SecurityProtocolType)3072 | (SecurityProtocolType)0x00000C00 | SecurityProtocolType.Tls;
            // registry models to protofuf
            RuntimeTypeModel.Default.Add(typeof(DateTimeOffset), false).SetSurrogate(typeof(DateTimeOffsetSurrogate));

            f_api_init();
            f_player_init();
            f_main_init();
        }

        public static void Exit()
        {
            main.Close();
        }

        #endregion
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args) { app.RUN(); }
    }
}
