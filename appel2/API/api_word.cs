using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

            if (timer_Content == null)
            {
                timer_Content = new System.Threading.Timer(new System.Threading.TimerCallback((obj) =>
                {
                    f_word_analytic_CONTENT();
                }), null, 100, 100);
            }

            if (timer_Word == null)
            {
                timer_Word = new System.Threading.Timer(new System.Threading.TimerCallback((obj) =>
                {
                    f_word_analytic_WORD();
                }), null, 100, 100);
            }
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
                        string content = (string)m.Input;

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
                        f_notificationToMain(new msg() { API = _API.MSG_ANALYTIC_CONTENT, Log = tit, Output = new msgOutput() { Data = mediaId, Ok = true } });
                    }

                    m_flag_analytic_content = false;
                }
            }
        }
    }
}
