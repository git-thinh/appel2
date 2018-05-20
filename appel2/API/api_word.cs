using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using YoutubeExplode;

namespace appel
{
    public class api_word : api_base, IAPI
    {
        public bool Open { set; get; } = false;

        static bool m_flag_analytic_content = false;
        static System.Threading.Timer timer_Content = null;
        static bool m_flag_analytic_word = false;
        static System.Threading.Timer timer_Word = null;

        static ConcurrentDictionary<long, string> dicMediaContent = null;
        static ConcurrentDictionary<long, string[]> dicMediaClause = null;
        static ConcurrentDictionary<long, oWordCount[]> dicMediaWord = null;

        static ConcurrentDictionary<string, int> dicWordCounter = null;
        static ConcurrentDictionary<string, bool> dicWordAnalytic = null;

        static ConcurrentDictionary<string, string> dicWordPronunce = null;
        static ConcurrentDictionary<string, string> dicWordMeanVi = null;

        static ConcurrentDictionary<string, List<string>> dicWordMedia = null;
        static ConcurrentDictionary<string, List<string>> dicWordStruct = null;

        public void Init()
        {
            dicMediaContent = new ConcurrentDictionary<long, string>();
            dicMediaWord = new ConcurrentDictionary<long, oWordCount[]>();
            dicMediaClause = new ConcurrentDictionary<long, string[]>();

            dicWordCounter = new ConcurrentDictionary<string, int>();
            dicWordAnalytic = new ConcurrentDictionary<string, bool>();

            dicWordPronunce = new ConcurrentDictionary<string, string>();
            dicWordMeanVi = new ConcurrentDictionary<string, string>();
            dicWordMedia = new ConcurrentDictionary<string, List<string>>();
            dicWordStruct = new ConcurrentDictionary<string, List<string>>();

            //if (timer_Content == null)
            //{
            //    timer_Content = new System.Threading.Timer(new System.Threading.TimerCallback((obj) =>
            //    {
            //        f_word_analytic_CONTENT();
            //    }), null, 100, 100);
            //}

            //if (timer_Word == null)
            //{
            //    timer_Word = new System.Threading.Timer(new System.Threading.TimerCallback((obj) =>
            //    {
            //        f_word_analytic_WORD();
            //    }), null, 100, 100);
            //}
        }

        public msg Execute(msg m)
        {
            if (m == null || Open == false) return m;
            switch (m.KEY)
            {
                case _API.WORD_KEY_INITED:
                    break;
                case _API.WORD_KEY_ANALYTIC:
                    #region
                    if (true)
                    {
                        long mediaId = (long)m.Input;
                        oMedia mi = api_media.f_media_getInfo(mediaId);
                        if (mi != null)
                        {
                            string content = mi.Title;
                            if (!string.IsNullOrEmpty(mi.Description))
                                content += Environment.NewLine + mi.Description;

                            if (mi.Keywords != null && mi.Keywords.Count > 0)
                                content += Environment.NewLine + string.Join(" ", mi.Keywords);

                            //var lw1 = f_analytic_wordXml(mi.SubtileEnglish);
                            //var ls1 = f_render_Sentence(lw1);
                            //string text = string.Empty;
                            //foreach (var se in ls1) text += se.TimeStart + ": " + se.Words + Environment.NewLine;
                            string text = mi.SubtileEnglish;
                            if (!string.IsNullOrEmpty(text))
                            {
                                text = Regex.Replace(text, @"<[^>]*>", String.Empty);
                                text = HttpUtility.HtmlDecode(text);
                                text = text.Replace('\r', ' ').Replace('\n', ' ');
                                string[] a = text.Split(new char[] { '.' })
                                    .Select(x => x.Trim())
                                    .Where(x => x != string.Empty)
                                    .ToArray();
                                text = string.Join(Environment.NewLine, a);
                                text = text.Replace("?", "?" + Environment.NewLine);
                                a = text.Split(new char[] { '\r', '\n' })
                                   .Select(x => x.Trim())
                                   .Where(x => x != string.Empty)
                                   .ToArray();
                                text = string.Empty;
                                foreach (string ti in a)
                                    if (ti[ti.Length - 1] == '?') text += ti + Environment.NewLine;
                                    else text += ti + "." + Environment.NewLine;


                            }

                            content += Environment.NewLine + Environment.NewLine + Environment.NewLine + text;
                            dicMediaContent.TryAdd(mediaId, content);

                            m.Output.Ok = true;
                            m.Output.Data = text;
                            response_toMain(m);

                            string[] ts = content.Split(new char[] { '\r', '\n', '.', ':', '-', ',' })
                                .Select(x => x.Trim())
                                .Where(x => x != string.Empty && x.IndexOf(' ') != -1)
                                .Distinct()
                                .ToArray();
                            if (!dicMediaClause.ContainsKey(mediaId))
                                dicMediaClause.TryAdd(mediaId, ts);

                            string temp = Regex.Replace(content, "[^0-9a-zA-Z]+", " ").ToLower();
                            temp = Regex.Replace(temp, "[ ]{2,}", " ").ToLower();
                            oWordCount[] wc = temp.Split(new char[] { '\r', '\n', ' ' })
                                .Where(x => x != string.Empty)
                                .Select(x => x.ToLower())
                                .GroupBy(x => x)
                                .Select(x => new oWordCount() { count = x.Count(), word = x.Key })
                                .ToArray();

                            if (!dicMediaWord.ContainsKey(mediaId))
                                dicMediaWord.TryAdd(mediaId, wc);

                            string word = string.Empty;
                            for (int i = 0; i < wc.Length; i++)
                            {
                                word = wc[i].word;
                                if (dicWordCounter.ContainsKey(word))
                                {
                                    int count = 0;
                                    dicWordCounter.TryGetValue(word, out count);
                                    dicWordCounter.TryUpdate(word, count + wc[i].count, count);
                                }
                                else
                                {
                                    dicWordCounter.TryAdd(word, wc[i].count);
                                }

                                if (!dicWordAnalytic.ContainsKey(word))
                                    dicWordAnalytic.TryAdd(word, false);
                            }

                        } // end analytic
                    }
                    break;
                    #endregion
            }
            return m;
        }

        public void Close()
        {
            if (timer_Content != null) timer_Content.Dispose();
        }

        public static List<oCaptionWord> f_analytic_wordFileXml(string file_xml)
        {
            XDocument xdoc = XDocument.Load(file_xml);
            List<oCaptionWord> listWord = new List<oCaptionWord>();
            foreach (var p in xdoc.Descendants("p"))
            {
                var its = p.Descendants("s").Select(x => new oCaptionWord(x)).ToArray();
                if (its.Length > 0)
                {
                    int tt = 0, dd = 0;
                    string t = p.Attribute("t").Value, d = p.Attribute("d").Value;
                    if (!string.IsNullOrEmpty(t)) int.TryParse(t, out tt);
                    if (!string.IsNullOrEmpty(d)) int.TryParse(d, out dd);
                    foreach (var it in its) it.TimeStart += tt;
                    listWord.AddRange(its);
                }
            }

            return listWord;
        }

        public static List<oCaptionWord> f_analytic_wordXml(string data_xml)
        {
            XDocument xdoc = XDocument.Parse(data_xml);
            List<oCaptionWord> listWord = new List<oCaptionWord>();
            foreach (var p in xdoc.Descendants("p"))
            {
                var its = p.Descendants("s").Select(x => new oCaptionWord(x)).ToArray();
                if (its.Length > 0)
                {
                    int tt = 0, dd = 0;
                    string t = p.Attribute("t").Value, d = p.Attribute("d").Value;
                    if (!string.IsNullOrEmpty(t)) int.TryParse(t, out tt);
                    if (!string.IsNullOrEmpty(d)) int.TryParse(d, out dd);
                    foreach (var it in its) it.TimeStart += tt;
                    listWord.AddRange(its);
                }
            }

            return listWord;
        }

        public static List<oCaptionSentence> f_render_Sentence(List<oCaptionWord> listWord)
        {
            List<oCaptionSentence> listSen = new List<oCaptionSentence>();
            oCaptionWord ci = null;
            oCaptionSentence si = new oCaptionSentence();
            string wi = string.Empty, wii = string.Empty;
            for (var i = 0; i < listWord.Count; i++)
            {
                ci = listWord[i];
                wi = ci.Word.Trim().ToLower();

                if (i == 0)
                {
                    si = new oCaptionSentence();
                    si.TimeStart = ci.TimeStart;
                    si.ListIndex.Add(i);
                    continue;
                }

                if (wi == "i" || wi == "we" || wi == "you" || wi == "they" || wi == "he" || wi == "she" || wi == "it"
                    || wi == "i'm" || wi == "we're" || wi == "you're" || wi == "they're" || wi == "he's" || wi == "she's" || wi == "it's"
                    || wi == "how" || wi == "where" || wi == "what" || wi == "whom" || wi == "who" || wi == "which")
                {
                    bool sub = false;
                    wii = listWord[i - 1].Word.ToLower();
                    if (i > 0 &&
                        (wii == "so" || wii == "and" || wii == "if" || wii == "when" || wii == "because"))
                    {
                        sub = true;
                        si.ListIndex.RemoveAt(si.ListIndex.Count - 1);
                    }

                    var ws = listWord.Where((x, id) => si.ListIndex.Any(y => y == id)).Select(x => x.Word).ToArray();
                    si.Words = string.Join(" ", ws);
                    listSen.Add(si);

                    si = new oCaptionSentence();
                    si.TimeStart = ci.TimeStart;
                    if (sub) si.ListIndex.Add(i - 1);
                    si.ListIndex.Add(i);
                }
                else
                {
                    si.ListIndex.Add(i);
                }
            }

            return listSen;
        }























        public static void f_word_addContentAnaltic(long mediaId, string content)
        {
            if (!string.IsNullOrEmpty(content))
                dicMediaContent.TryAdd(mediaId, content);
        }

        private void f_word_analytic_WORD()
        {
            if (m_flag_analytic_word == false)
            {
                if (dicMediaContent.Count > 0)
                {
                    m_flag_analytic_word = true;
                    string word = string.Empty;
                    //f_notificationToMain(new msg() { API = _API.MSG_ANALYTIC_WORD, Log = word });
                    m_flag_analytic_word = false;
                }
            }
        }

        private void f_word_analytic_CONTENT()
        {
            if (m_flag_analytic_content == false)
            {
                if (dicMediaContent.Count > 0)
                {
                    m_flag_analytic_content = true;

                    string content = string.Empty;
                    long mediaId = dicMediaContent.Keys.Take(1).SingleOrDefault();
                    dicMediaContent.TryRemove(mediaId, out content);
                    if (!string.IsNullOrEmpty(content))
                    {
                        string[] ts = content.Split(new char[] { '\r', '\n', '.', ':', '-', ',' })
                            .Select(x => x.Trim())
                            .Where(x => x != string.Empty && x.IndexOf(' ') != -1)
                            .Distinct()
                            .ToArray();
                        if (!dicMediaClause.ContainsKey(mediaId))
                            dicMediaClause.TryAdd(mediaId, ts);

                        string temp = Regex.Replace(content, "[^0-9a-zA-Z]+", " ").ToLower();
                        temp = Regex.Replace(temp, "[ ]{2,}", " ").ToLower();
                        oWordCount[] wc = temp.Split(new char[] { '\r', '\n', ' ' })
                            .Where(x => x != string.Empty)
                            .Select(x => x.ToLower())
                            .GroupBy(x => x)
                            .Select(x => new oWordCount() { count = x.Count(), word = x.Key })
                            .ToArray();

                        if (!dicMediaWord.ContainsKey(mediaId))
                            dicMediaWord.TryAdd(mediaId, wc);

                        string word = string.Empty;
                        for (int i = 0; i < wc.Length; i++)
                        {
                            word = wc[i].word;
                            if (dicWordCounter.ContainsKey(word))
                            {
                                int count = 0;
                                dicWordCounter.TryGetValue(word, out count);
                                dicWordCounter.TryUpdate(word, count + wc[i].count, count);
                            }
                            else
                            {
                                dicWordCounter.TryAdd(word, wc[i].count);
                            }

                            if (!dicWordAnalytic.ContainsKey(word))
                                dicWordAnalytic.TryAdd(word, false);
                        }

                        string tit = api_media.f_media_getTitle(mediaId);
                        notification_toMain(new msg() { API = _API.MSG_ANALYTIC_CONTENT, Log = tit, Output = new msgOutput() { Data = mediaId, Ok = true } });
                    }

                    m_flag_analytic_content = false;
                }
            }
        }
    }
}
