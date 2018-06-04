using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace appel
{
    public class api_word : api_base, IAPI
    {
        static ConcurrentDictionary<string, string> dicWordLink = null;
        static ConcurrentDictionary<string, List<string>> dicPron = null;
        static List<string> listWord = null;
        static readonly object _lock = new object();
        const int page_size_default = 100;


        #region [ API ]

        public bool Open { set; get; } = false;
        
        public void Init()
        {
            dicWordLink = new ConcurrentDictionary<string, string>();
            listWord = new List<string>();

            var ws = Directory.GetFiles("words", "*.txt")
                .Select(x => Path.GetFileName(x).ToLower())
                .Select(x => x.Substring(0, x.Length - 4))
                .Distinct().ToArray();

            listWord.AddRange(ws);
            listWord.Sort();

            for (int i = 0; i < ws.Length; i++)
                dicWordLink.TryAdd(ws[i], string.Empty);

            dicPron = new ConcurrentDictionary<string, List<string>>();
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
                    break;
                    #endregion
            }
            return m;
        }

        public void Close()
        {
        }

        #endregion

        #region [ METHOD ]

        public static oWordCollectionResult f_get_Items(int page_number, int page_size = page_size_default)
        {
            int min = (page_number - 1) * page_size;
            lock (_lock)
            {
                if (listWord.Count > 0)
                {
                    if (listWord.Count > page_size_default)
                        return new oWordCollectionResult(listWord.Count, listWord.Count, page_number, page_size, listWord.Skip(min).Take(page_size).ToArray());
                    else
                        return new oWordCollectionResult(listWord.Count, listWord.Count, page_number, page_size, listWord.ToArray());
                }
            }
            return new oWordCollectionResult(0, 0, page_number, page_size, new string[] { });
        }

        public static oWordCollectionResult f_find_Items(string key, int page_number, int page_size = page_size_default)
        {
            if (string.IsNullOrWhiteSpace(key))
                return f_get_Items(page_number, page_size);

            int min = (page_number - 1) * page_size;
            string[] rs = new string[] { };

            lock (_lock)
            {
                if (listWord.Count > 0)
                    rs = listWord.Where(x => x.Contains(key)).ToArray();
            }

            if (rs.Length > page_size_default)
                return new oWordCollectionResult(listWord.Count, rs.Length, page_number, page_size, rs.Skip(min).Take(page_size).ToArray());
            else
                return new oWordCollectionResult(listWord.Count, rs.Length, page_number, page_size, rs);
        }

        #endregion

    }

    public class oPronExam
    {
        public string Pronunciation { set; get; }
        public List<string> Words { set; get; }
    }

    public class oWordResult
    {
        public oWordLink[] WordLink { get; set; }
        public string MeanVi { get; set; }
        public string Pronunciation { get; set; }
        public string Type { get; set; }
        public string[] UrlAudio { get; set; }

        public oWordResult()
        {
            WordLink = new oWordLink[] { };
            MeanVi = string.Empty;
            Pronunciation = string.Empty;
            Type = string.Empty;
            UrlAudio = new string[] { };
        }
    }

    public class oWordLink
    {
        public string Word { get; set; }
        public string MeanVi { get; set; }
        public string Pronunciation { get; set; }
        public string Type { get; set; }

        public oWordLink()
        {
            Word = string.Empty;
            MeanVi = string.Empty;
            Pronunciation = string.Empty;
            Type = string.Empty;
        }
    }

    public class oWordCollectionResult
    {
        public string[] Words { get; }
        public int Counter { get; }
        public int Total { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public int PageTotal { get; }

        public oWordCollectionResult(int total, int counter, int page_number, int page_size, string[] words)
        {
            Total = total;
            Counter = counter;
            PageNumber = page_number;
            PageSize = page_size;

            PageTotal = 0;
            if (counter > 0 && page_size > 0)
            {
                PageTotal = counter / page_size;
                if (counter % page_size == 0) PageTotal++;
                if (PageTotal == 0) PageTotal = 1;
            }

            Words = words;
        }
    }
}
