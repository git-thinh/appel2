using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace appel
{
    public class api_word_LocalStore : api_base, IAPI
    {
        static readonly object _lock = new object();
        static Dictionary<string, oWord> dicWord = new Dictionary<string, oWord>();

        public static oWordContent f_analytic_Text(string s)
        {
            string[] sentences = s.Split(new char[] { '\n', '\r', '.' }).Where(x => x != string.Empty).ToArray();

            string text = Regex.Replace(s, "[^0-9a-zA-Z]+", " ").ToLower();
            text = Regex.Replace(text, "[ ]{2,}", " ").ToLower();
            oWordCount[] aword = text.Split(' ').Where(x => x.Length > 3)
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .Select(x => new oWordCount() { word = x.Key, count = x.Count() })
                .ToArray();

            return new oWordContent() { sentences = sentences, words = aword };
        }


        public msg Execute(msg m)
        {
            if (m == null || m.Input == null) return m;
            oWordContent wo = (oWordContent)m.Input;
            m.KEY = string.Empty;
            m.Output.Ok = false;

            string[] a_con = wo.words.Select(x => x.word.ToLower()).Distinct().ToArray();

            string[] a_local = new string[] { };
            lock (_lock)
                a_local = dicWord.Keys.ToArray();

            a_local = a_local.Where(x => a_con.Any(o => o == x)).ToArray();

            string[] a_down;

            if (a_local.Length == 0)
                a_down = a_con;
            else
                a_down = a_local.Where(x => !a_con.Any(o => o == x)).ToArray();

            if (a_down.Length > 0)
                app.postMessageToService(new msg() { API = _API.WORD_DOWNLOAD, Input = a_down });

            m.Output.Ok = true;
            m.Output.Data = a_con;

            return m;
        }
        public void Close() { }
    }
}
