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

        static bool m_word_Analytic = false;
        static System.Threading.Timer timer = null;
        static ConcurrentDictionary<long, string> dicMediaContent = null;
        static ConcurrentDictionary<long, oWordCount[]> dicMediaWord = null;

        static ConcurrentDictionary<string, int> dicWordCounter = null;
        static ConcurrentDictionary<string, bool> dicWordAnalytic = null;

        static ConcurrentDictionary<string, string> dicPronunce = null;
        static ConcurrentDictionary<string, string> dicMeanVi = null;
        static ConcurrentDictionary<string, List<string>> dicMedia = null;
        static ConcurrentDictionary<string, List<string>> dicSentence = null;
        static ConcurrentDictionary<string, List<string>> dicStruct = null;

        public void Init()
        {
            dicMediaWord = new ConcurrentDictionary<long, oWordCount[]>();
            dicMediaContent = new ConcurrentDictionary<long, string>();

            dicWordCounter = new ConcurrentDictionary<string, int>();
            dicWordAnalytic = new ConcurrentDictionary<string, bool>();

            dicPronunce = new ConcurrentDictionary<string, string>();
            dicMeanVi = new ConcurrentDictionary<string, string>();
            dicMedia = new ConcurrentDictionary<string, List<string>>();
            dicSentence = new ConcurrentDictionary<string, List<string>>();
            dicStruct = new ConcurrentDictionary<string, List<string>>();

            if (timer == null)
            {
                timer = new System.Threading.Timer(new System.Threading.TimerCallback((obj) =>
                {
                    f_word_analyticContent();
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
            if (timer != null) timer.Dispose();
        }

        public static void f_word_addContentAnaltic(long mediaId, string content)
        {
            if (!string.IsNullOrEmpty(content))
                dicMediaContent.TryAdd(mediaId, content);
        }

        private void f_word_analyticContent()
        {
            if (m_word_Analytic == false)
            {
                if (dicMediaContent.Count > 0)
                {
                    m_word_Analytic = true;

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


                    }

                    m_word_Analytic = false;
                }
            }
        }
    }
}
