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

        public List<Tuple<string, string>> Vocabulary = new List<Tuple<string, string>>();
        //public List<oWordCount> Words = new List<oWordCount>();
        //public string Text = string.Empty;

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

        //public oMedia(Video v)
        //{
        //    DurationSecond = (int)v.Duration.TotalSeconds;
        //    Title = v.Title;
        //    Description = v.Description;
        //    Keywords = v.Keywords;
        //    Author = v.Author;
        //    UploadDate = int.Parse(v.UploadDate.ToString("yyMMdd"));
        //}

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

        private string _Text = string.Empty;
        public string Text
        {
            get
            {
                if (_Text == string.Empty) _Text = this.f_get_Text();
                return _Text;
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
                .OrderByDescending(x => x.count)
                .ToList();
        }

        private string f_get_Text()
        {
            oMedia mi = this;

            string content = mi.Title;
            if (!string.IsNullOrEmpty(mi.Description))
                content += Environment.NewLine + mi.Description;

            if (mi.Keywords != null && mi.Keywords.Count > 0)
                content += Environment.NewLine + string.Join(" ", mi.Keywords);

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

            content += Environment.NewLine +
                "------------------------------------------------------------------"
                + Environment.NewLine + Environment.NewLine +
                text;

            return content;
        }
    }
}
