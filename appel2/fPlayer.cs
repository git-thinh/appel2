using AxWMPLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace appel
{
    public class fPlayer : Form, IFORM
    {
        private AxWindowsMediaPlayer m_media;
        private ControlTransparent m_modal;

        public fPlayer()
        {
            // FORM
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black;

            // MEDIA
            m_media = new AxWindowsMediaPlayer();
            m_media.Dock = DockStyle.None;
            m_media.Location = new Point(0, 0);
            m_media.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            m_media.Enabled = true;
            m_media.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(this.f_media_event_PlayStateChange);
            this.Controls.Add(m_media);

            // MODAL
            m_modal = new ControlTransparent();
            m_media.Dock = DockStyle.None;
            m_modal.Location = new Point(0, 0);
            m_modal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            m_modal.BackColor = Color.Black;
            m_modal.Opacity = 1;
            m_modal.MouseMove += f_form_move_MouseDown;
            m_modal.DoubleClick += (se, ev) =>
            {
                if (this.FormBorderStyle == FormBorderStyle.None)
                {
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    this.TopMost = false;
                    m_media.Ctlcontrols.pause();
                }
                else
                {
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.TopMost = true;
                    m_media.Ctlcontrols.play();
                }
            };
            this.Controls.Add(m_modal);

            // FORM SHOWN
            this.Shown += (se, ev) =>
            {
                m_media.Size = new Size(this.Width, this.Height);
                m_modal.Size = new Size(this.Width, this.Height);
                m_modal.BringToFront();


                m_media.uiMode = "none";
            };
        }

        #region [ FORM MOVE ]

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void f_form_move_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #endregion

        private void f_media_event_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            /**** Don't add this if you want to play it on multiple screens***** /
             * 
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                axWindowsMediaPlayer1.fullScreen = true;
            }
            /********************************************************************/

            if (m_media.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                //Application.Exit();
            }


            switch (m_media.playState)
            {
                case WMPLib.WMPPlayState.wmppsTransitioning:

                    break;
                case WMPLib.WMPPlayState.wmppsPlaying:

                    break;
            }


        }

        public void f_play(string path)
        {
            m_media.URL = path;
        }

        public void api_responseMsg(object sender, threadMsgEventArgs e)
        {
        }

        public void api_initMsg(msg m)
        {
        }
    }
}
