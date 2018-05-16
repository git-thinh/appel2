using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace appel
{
    public class fYoutubeIframe : Form
    {
        public fYoutubeIframe(string videoId)
        {
            //AxShockwaveFlash axShockwaveFlash = new AxShockwaveFlash()
            //{
            //    Dock = DockStyle.Fill
            //};
            //this.Controls.Add(axShockwaveFlash);
            //this.Shown += (se, ev) =>
            //{
            //    this.Width = app.m_item_width * 2;
            //    this.Height = app.m_item_height * 2;
                
            //    //Get url video
            //    //string url = txtUrl.Text.Replace("watch?v=", "v/");
            //    string url = "http://www.youtube.com/v/" + videoId;
            //    axShockwaveFlash.Movie = url;
            //    axShockwaveFlash.Play();

            //    //const string page = "<html><head><title></title></head><body>{0}</body></html>";
            //    //w.DocumentText = string.Format(page, "<iframe width=\"100%\" height=\"100%\" src=\"http://www.youtube.com/embed/" + videoId + "\" frameborder=\"0\" allowfullscreen></iframe>");

            //    //w.Navigate("https://www.youtube.com/embed/" + videoId);
            //};
        }


    }
}
