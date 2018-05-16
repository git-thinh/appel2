using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appel
{ 
    public class api_fetch : api_base, IAPI
    {
        static readonly object _lock = new object();
        static Dictionary<string, List<string>> dicUrl = new Dictionary<string, List<string>>();

        public msg Execute(msg m)
        {
            if (m == null || m.Input == null) return m;


            //HttpWebRequest w = (HttpWebRequest)WebRequest.Create(new Uri(url));
            //w.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.186 Safari/537.36";
            //w.BeginGetResponse(asyncResult =>
            //{
            //    HttpWebResponse rs = (HttpWebResponse)w.EndGetResponse(asyncResult); //add a break point here 
            //    StreamReader sr = new StreamReader(rs.GetResponseStream(), System.Text.Encoding.UTF8);
            //    s = sr.ReadToEnd();
            //    sr.Close();
            //    rs.Close();
            //    s = HttpUtility.HtmlDecode(s);
            //}, w);

            m.Output.Ok = true;
            m.Output.Data = null;
            return m;
        }

        public void Close()
        {
        }
    }
}
