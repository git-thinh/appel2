using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appel
{
    [ProtoContract]
    public class oMediaPath
    {
        [ProtoMember(1)]
        public long Id { get; set; }

        [ProtoMember(2)]
        public string PathMp3_Local { get; set; }

        [ProtoMember(3)]
        public string PathMp3_GoogleDriver { get; set; }

        [ProtoMember(4)]
        public string YoutubeID { get; set; }

        [ProtoMember(5)]
        public string PathMp4_Local { get; set; }

        [ProtoMember(6)]
        public string PathMp4_GoogleDriver { get; set; }

        [ProtoMember(7)]
        public string PathMp4_Facebook { get; set; }


        public string PathMp3_Youtube { get; set; }
        public string PathMp4_Youtube { get; set; }

        public oMediaPath clone()
        {
            oMediaPath m = Serializer.DeepClone<oMediaPath>(this);
            return m;
        }

        public oMediaPath() { }
        public oMediaPath(long mediaId, string youtubeId) {
            Id = mediaId;
            YoutubeID = youtubeId;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Id, YoutubeID);
        }
    }
}
