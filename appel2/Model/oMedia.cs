using Newtonsoft.Json;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using YoutubeExplode.Models;

namespace appel
{
    [ProtoContract]
    [ProtoInclude(13, typeof(oMediaPath))]
    public class oMedia
    {
        #region [ CONTRACTOR - ID ]

        public oMedia clone()
        {
            oMedia m = Serializer.DeepClone<oMedia>(this);
            return m;
        }

        public oMedia()
        {
        }

        public oMedia(string videoId)
        {
            Id = convert_id_bit_shifting(videoId);
        }

        long convert_id(string key)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(key);
            long result = BitConverter.ToInt64(bytes, 0);
            return result;
        }

        long convert_id_bit_shifting(string key)
        {
            long val = 0;
            for (int i = 3; i >= 0; i--)
            {
                val <<= 8;
                val += (int)key[i];
            }
            return val;
        }
        public override string ToString()
        {
            return string.Format("{0} - {1}", Id, Title);
        }

        #endregion

        #region [ MEMBER PROTO ]

        [ProtoMember(1)]
        public long Id { get; set; }

        [ProtoMember(2)]
        public string Author { get; set; }

        [ProtoMember(3)]
        public int UploadDate { get; set; }

        [ProtoMember(4)]
        public string Title { get; set; }

        [ProtoMember(5)]
        public string Description { get; set; }

        [ProtoMember(6)]
        public int DurationSecond { get; set; }

        [ProtoMember(7)]
        public List<string> Keywords = new List<string>();

        [ProtoMember(8)]
        public string SubtileEnglish { get; set; }

        [ProtoMember(9)]
        public bool Star { get; set; }

        [ProtoMember(10)]
        public List<string> Tags = new List<string>();

        [ProtoMember(11)]
        public List<string> Folder = new List<string>();

        [ProtoMember(12)]
        public long ViewCount = 1;

        [ProtoMember(13)]
        public oMediaPath Paths = new oMediaPath();

        #endregion

        #region [ SUBTITLE - CC ]

        private string _subtileEnglish_Text = string.Empty;
        private string[] _subtileEnglish_Sentences = null;

        public string[] SubtileEnglish_Sentence
        {
            get
            {
                if (_subtileEnglish_Sentences != null)
                    return _subtileEnglish_Sentences;

                string[] a = new string[] { };

                string text = this.SubtileEnglish_Text.ToLower()
                    .Replace('.', '|')
                    .Replace(" i i ", " i ")
                    .Replace('\r', ' ')
                    .Replace('\n', ' ')
                    .Replace("[music]", ".");

                text = Regex.Replace(text, "[^0-9a-zA-Z.'|]+", " ").ToLower();
                text = Regex.Replace(text, "[ ]{2,}", " ").ToLower();

                List<string> ls_questions = new List<string>() { "how", "where", "what", "whom", "who", "which", "when", "why" };

                string[] a_break_sentence = new string[] {
                        #region

                        "welcome to",
                        "there's",
                        "these're",
                        "there is",
                        "these are",
                        "there are",
                        "this is",
                        "please",
                        "my name",
                        "all these",
                        "these while",
                        "thank you",
                        "and then",
                        "how are",
                        "that you can",
                        "that you do",
                        "you can even",
                        "you could use",
                        "so you can",
                        "and you can",
                        "so you know",
                        "and you know",
                        "you know that",
                        "you only",
                        "you ask",
                        "that will",

                        " let ",
                        " if ",
                        " because ",
                        " let's ",
                        " but ",
                        " while ",

                        "how", "where", "what", "whom", "who", "which", "when", "why",
                        "i'm", "we're", "you're", "they're", "he's", "she's", "it's",
                        "i will", "we will", "you will", "they will", "he will", "she will", "it will",
                        "i'll", "we'll", "you'll", "they'll", "he'll", "she'll", "it'll",
                        "i've", "we've", "you've", "they've",
                        "i'd", "we'd", "you'd", "they'd",
                        "it was",
                        "it is",
                        " i "," we "," they "," he "," she ", // " it ",

                        #endregion
                    };
                foreach (string ai in a_break_sentence)
                    text = text.Replace(ai, ". " + ai);

                //return a = text
                //    .Split('.')
                //    .Select(x => x.Trim())
                //    .Where(x => x.Length > 0)
                //    .Select(x => x[0].ToString().ToUpper() + x.Substring(1))
                //    .ToArray();

                a = text.Split('.')
                    .Select(x => x.Trim())
                    .Where(x => x.Length > 0)
                    .ToArray();
                if (a[0] == "music") a[0] = string.Empty;

                List<int> ls_index_short = a.Select((x, k) => new oWordCount() { word = x, count = k, len = x.Split(' ').Length })
                    .Where(x =>
                        x.len < 3
                        || (x.len < 4 && (
                            x.word.EndsWith("was")
                            || x.word.EndsWith("did")
                        )))
                    .Select(x => x.count)
                    .ToList();

                string[] rs = new string[a.Length - 1];
                string si = string.Empty;
                for (int i = 0; i < a.Length - 1; i++)
                {
                    rs[i] = string.Empty;
                    if (ls_index_short.IndexOf(i) == -1)
                    {
                        if (ls_index_short.IndexOf(i - 1) != -1)
                            rs[i] = a[i - 1] + " ";

                        si = a[i];

                        if (si.EndsWith(" and so"))
                            si = si.Substring(0, si.Length - 7);                        
                        else if (si.EndsWith(" so") || si.EndsWith(" and"))
                            si = si.Substring(0, si.Length - 3).Trim();

                        if (ls_questions.IndexOf(si.Split(' ')[0]) != -1) si += " ?";

                        rs[i] += si;
                    }
                }

                a = rs
                    .Where(x => x.Length > 0)
                    .Select(x => x[0].ToString().ToUpper() + x.Substring(1))
                    .ToArray();
                _subtileEnglish_Sentences = a;

                return a;
            }//end get
        }

        public string SubtileEnglish_Text
        {
            get
            {
                if (!string.IsNullOrEmpty(_subtileEnglish_Text))
                    return _subtileEnglish_Text;

                string text = this.SubtileEnglish;
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

                    text = text
                        //string text = m.SubtileEnglish_Text.ToLower()
                        //.Replace("\r", string.Empty)
                        //.Replace("\n", string.Empty)
                        //.Replace('.', '|')
                        .Replace(" i i ", " i ")
                        .Replace('\r', ' ')
                        .Replace('\n', ' ')
                        .Replace("[music]", ".");

                    text = Regex.Replace(text, "[^0-9a-zA-Z.'|]+", " ").ToLower();
                    text = Regex.Replace(text, "[ ]{2,}", " ").ToLower();
                    //string.Join("." + Environment.NewLine, text.Split('.').Select(x => x.Trim()).ToArray());

                    text = string.Join(Environment.NewLine,
                        text.Split('.')
                        .Select(x => x.Trim())
                        .Where(x => x.Length > 0)
                        .Select(x => x[0].ToString().ToUpper() + x.Substring(1))
                        .ToArray());
                }

                _subtileEnglish_Text = text;
                return text;
            }
        }

        #endregion

        #region [ WORD - VOCABULARY ]

        public List<Tuple<string, string>> Vocabulary = new List<Tuple<string, string>>();
        //public List<oWordCount> Words = new List<oWordCount>();
        //public string Text = string.Empty;

        private List<oWordCount> _Words = new List<oWordCount>();
        public List<oWordCount> Words
        {
            get
            {
                if (_Words.Count == 0)
                {
                    _Words = this.f_get_Words();
                }
                return _Words;
            }
        }

        private List<oWordCount> f_get_Words()
        {
            string content = this.f_get_Text();

            string temp = Regex.Replace(content, "[^0-9a-zA-Z]+", " ").ToLower();
            temp = Regex.Replace(temp, "[ ]{2,}", " ").ToLower();
            return temp.Split(new char[] { '\r', '\n', ' ' })
                .Where(x => x != string.Empty)
                .Select(x => x.ToLower())
                .GroupBy(x => x)
                .Select(x => new oWordCount() { count = x.Count(), word = x.Key })
                //.OrderByDescending(x => x.count)
                .OrderBy(x => x.word)
                .ToList();
        }

        #endregion

        #region [ TEXT ]

        private string _Text = string.Empty;
        public string Text
        {
            get
            {
                if (_Text == string.Empty) _Text = this.f_get_Text();
                return _Text;
            }
        }

        private string f_get_Text()
        {
            oMedia mi = this;

            string content = mi.Title;
            if (!string.IsNullOrEmpty(mi.Description))
                content += Environment.NewLine + mi.Description;

            if (mi.Keywords != null && mi.Keywords.Count > 0)
                content += Environment.NewLine + string.Join(" ", mi.Keywords);

            content += Environment.NewLine +
                //"------------------------------------------------------------------" +
                Environment.NewLine + Environment.NewLine +
                mi.SubtileEnglish_Text;

            return content;
        }

        #endregion
    }
}
