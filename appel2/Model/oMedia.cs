using Newtonsoft.Json;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public bool hasImage = false;

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
    }
}
