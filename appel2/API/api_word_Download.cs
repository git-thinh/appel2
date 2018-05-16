using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appel
{
    public class api_word_Download : api_base, IAPI
    {
        static readonly object _lock = new object();
        static List<string> listWordDownload = new List<string>() { };

        public msg Execute(msg m)
        {
            if (m == null || m.Input == null) return m;
            string[] a = (string[])m.Input;

            m.KEY = string.Empty;
            m.Output.Ok = false;

            ////////https://s3.amazonaws.com/audio.oxforddictionaries.com/en/mp3/you_gb_1.mp3
            ////////https://ssl.gstatic.com/dictionary/static/sounds/oxford/you--_gb_1.mp3


            ////////https://ssl.gstatic.com/dictionary/static/sounds/20160317/hello--_us_1.mp3
            ////////https://ssl.gstatic.com/dictionary/static/sounds/20160317/you--_us_1.mp3

            ////////https://ssl.gstatic.com/dictionary/static/sounds/20160317/ok--_us_1.mp3
            ////////https://ssl.gstatic.com/dictionary/static/sounds/20160317/you--_gb_1.mp3
            ////////https://ssl.gstatic.com/dictionary/static/sounds/20160317/ok--_gb_1.mp3

            //////if (!NetworkInterface.GetIsNetworkAvailable())
            //////{
            //////    // Network does not available.

            //////}

            //////var ping = new System.Net.NetworkInformation.Ping();
            //////var result = ping.Send("www.google.com");
            //////if (result.Status == System.Net.NetworkInformation.IPStatus.Success)
            //////{

            //////}



            //////////string temp = HttpUtility.UrlEncode(input.Replace(" ", "---"));
            ////////string temp = HttpUtility.UrlEncode(input);
            //////////temp = temp.Replace("-----", "%20");

            ////////string url = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", temp, "en|vi");

            ////////string s = String.Empty;
            ////////using (WebClient webClient = new WebClient())
            ////////{
            ////////    webClient.Encoding = encoding;
            ////////    s = webClient.DownloadString(url);
            ////////}
            ////////string ht = HttpUtility.HtmlDecode(s);

            ////////string result = String.Empty;
            ////////int p = s.IndexOf("id=result_box");
            ////////if (p > 0)
            ////////    s = "<span " + s.Substring(p, s.Length - p);
            ////////p = s.IndexOf("</div>");
            ////////if (p > 0)
            ////////{
            ////////    s = s.Substring(0, p);
            ////////    s = s.Replace("<br>", "¦");
            ////////    s = HttpUtility.HtmlDecode(s);
            ////////    result = Regex.Replace(s, @"<[^>]*>", String.Empty);
            ////////}
            ////////if (result != string.Empty)
            ////////{
            ////////    string[] rs = result.Split('¦').Select(x => x.Trim()).ToArray();
            ////////    m.Output = new MsgOutput() { Ok = true, Data = rs, Total = rs.Length };
            ////////    
            ////////    lock (lockResponse)
            ////////    {
            ////////        if (dicResponses.ContainsKey(m.Key))
            ////////            dicResponses[m.Key] = m;
            ////////        else
            ////////            dicResponses.Add(m.Key, m);
            ////////    }
            ////////}
            ////////else
            ////////{
            ////////    m.Output = new MsgOutput() { Data = "Can not translate" };
            ////////}

            ////////postMessageToFormUI(m.Key);

            m.Output.Ok = true;
            //m.Output.Data = wo.words;
            return m;
        }
        public void Close() { }
    }
}
