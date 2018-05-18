using Newtonsoft.Json;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appel
{
    [ProtoContract]
    public class oMedia
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        
        [ProtoMember(2)]
        public string Author { get; set; }
        
        [ProtoMember(3)]
        public int UploadDate { get; set; }
        
        [ProtoMember(4)]
        public string Title { get; set; }
        
        [ProtoMember(5)]
        public string Description { get; set; }
        
        [ProtoMember(6)]
        public int Duration { get; set; }

        [ProtoMember(7)]
        public List<string> Keywords { get; set; }

        [ProtoMember(8)]
        public string PathMp3_Local { get; set; }

        [ProtoMember(9)]
        public string PathMp3_GoogleDriver { get; set; }

        [ProtoMember(10)]
        public string PathMp3_YoutubeID { get; set; }

        [ProtoMember(11)]
        public string PathMp4_Local { get; set; }

        [ProtoMember(12)]
        public string PathMp4_GoogleDriver { get; set; }

        [ProtoMember(13)]
        public string PathMp4_YoutubeID { get; set; }

        [JsonIgnore]
        public string URLs { get; set; }

        public oMedia() { }
    }
}
