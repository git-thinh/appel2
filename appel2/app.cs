using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.Collections.Concurrent;
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
        #region [ VARIABLE ]

        public const int m_item_width = 320;
        public const int m_app_width = m_item_width * 2 + 45;
        public const int m_item_height = 180;
        static fMain main;

        static ConcurrentDictionary<string, VideoInfo> m_dicVideo = null;
        static ConcurrentDictionary<string, IthreadMsg> dicService = null;
        static ConcurrentDictionary<string, msg> dicResponses = null;

        #endregion

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
            RuntimeTypeModel.Default.Add(typeof(DateTimeOffset), false).SetSurrogate(typeof(DateTimeOffsetSurrogate));

            m_dicVideo = new ConcurrentDictionary<string, VideoInfo>();
            dicResponses = new ConcurrentDictionary<string, msg>();
            dicService = new ConcurrentDictionary<string, IthreadMsg>();

            //dicService.TryAdd(_API.PROXY_MEDIA, new threadMsg(new api_proxy_Media()));

            //dicService.TryAdd(_API.WORD_LOAD_LOCAL, new threadMsg(new api_word_LocalStore()));
            //dicService.TryAdd(_API.SETTING_APP, new threadMsg(new api_settingApp()));
            //dicService.TryAdd(_API.FOLDER_ANYLCTIC, new threadMsg(new api_folder_Analytic()));
            //dicService.TryAdd(_API.CRAWLER, new threadMsg(new api_crawler()));
            //dicService.TryAdd(_API.YOUTUBE, new threadMsg(new api_youtube()));
            //dicService.TryAdd(_API.MP3, new threadMsg(new api_mp3()));



            main = new fMain();
            main.Shown += (se, ev) =>
            {
                main.Width = m_app_width;
                main.Height = m_item_height * 3 + 123;
                main.Top = (Screen.PrimaryScreen.WorkingArea.Height - main.Height) / 2;
                main.Left = (Screen.PrimaryScreen.WorkingArea.Width - main.Width) / 2;
            };

            api_proxy_Media.Start();
            Application.EnableVisualStyles();
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
