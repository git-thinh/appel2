using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appel
{
    public class api_word : api_base, IAPI
    {
        public bool Open { set; get; } = false;

        static bool m_word_Analytic = false;
        static System.Threading.Timer timer = null;
        static ConcurrentDictionary<long, string> dicMediaContent = null;

        static ConcurrentDictionary<string, string> dicPronunce = null;
        static ConcurrentDictionary<string, string> dicMeanVi = null;
        static ConcurrentDictionary<string, List<string>> dicMedia = null;
        static ConcurrentDictionary<string, List<string>> dicSentence = null;
        static ConcurrentDictionary<string, List<string>> dicStruct = null;

        public void Init()
        {
            dicMediaContent = new ConcurrentDictionary<long, string>();
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

                    }
                }
            }
        }
    }
}
