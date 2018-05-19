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
        public const int m_item_width = 320;
        public const int m_app_width = m_item_width * 2 + 45;
        public const int m_item_height = 180;
        static fMain main;

        static ConcurrentDictionary<string, IthreadMsg> dicService = null;
        static ConcurrentDictionary<string, msg> dicResponses = null;

        public static void f_postToAPI(string api, string key, object data)
        {
            if (dicService.ContainsKey(api))
            {
                IthreadMsg sv;
                if (dicService.TryGetValue(api, out sv))
                {
                    new Thread(new ParameterizedThreadStart((object _sv) =>
                    {
                        IthreadMsg so = (IthreadMsg)_sv;
                        so.Execute(new msg() { API = api, KEY = key, Input = data });
                    })).Start(sv);
                }

            }
        }

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
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            dicResponses = new ConcurrentDictionary<string, msg>();
            dicService = new ConcurrentDictionary<string, IthreadMsg>();

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            dicService.TryAdd(_API.MEDIA, new threadMsg(new api_media()));
            //dicService.TryAdd(_API.MEDIA_PROXY, new threadMsg(new api_media_Proxy()));


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            main = new fMain();
            main.Shown += main_Shown;
            main.FormClosing += main_Closing;
            Application.EnableVisualStyles();
            Application.Run(main);
        }

        public static void Exit()
        {
            //main.f_free_Resource();
            Application.Exit();
        }

        #region [ MAIN ]

        private static void main_Closing(object sender, FormClosingEventArgs e)
        {
            if (main == null) return;

            var confirmResult = MessageBox.Show("Are you sure to exit this application ?", "Confirm Exit!", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                foreach (var kv in dicService)
                    if (kv.Value != null)
                        kv.Value.Stop();

                main.f_form_freeResource();

                // wait for complete threads, free resource
                Thread.Sleep(200);

                //Application.ExitThread();
                //Application.Exit();
            }
            else
                e.Cancel = true;
        }

        private static void main_Shown(object sender, EventArgs e)
        {
            main.Width = m_app_width;
            main.Height = m_item_height * 3 + 123;
            main.Top = (Screen.PrimaryScreen.WorkingArea.Height - main.Height) / 2;
            main.Left = (Screen.PrimaryScreen.WorkingArea.Width - main.Width) / 2;
        }

        #endregion

        #region

        public static IFORM get_Main()
        {
            return main;
        }

        public static void postMessageToService(msg m)
        {

        }



        #endregion
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args) { app.RUN(); }
    }
}
