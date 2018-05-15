using AxWMPLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace appel
{
    public class fPlayer: Form, IFORM
    {
        private AxWindowsMediaPlayer media;
        public fPlayer() {
            media = new AxWindowsMediaPlayer();
            media.Dock = DockStyle.Fill;
            media.Enabled = true;
            media.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(this.f_media_event_PlayStateChange);

            this.Controls.Add(media);
            this.Shown += (se, ev) => {
                media.uiMode = "none";
            };
        }

        private void f_media_event_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            /**** Don't add this if you want to play it on multiple screens***** /
             * 
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                axWindowsMediaPlayer1.fullScreen = true;
            }
            /********************************************************************/

            if (media.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                Application.Exit();
            }
        }

        public void f_play(string path) {
            media.URL = path;
        }

        public void api_responseMsg(object sender, threadMsgEventArgs e)
        { 
        }

        public void api_initMsg(msg m)
        { 
        }
    }
}
