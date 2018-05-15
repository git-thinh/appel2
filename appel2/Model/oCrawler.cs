using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace appel
{ 
    [ProtoContract]
    public class oLink
    {
        [ProtoMember(1)]
        public string uri { set; get; }

        [ProtoMember(2)]
        public string text { set; get; }

        [ProtoMember(3)]
        public string url { set; get; }

        [ProtoMember(4)]
        public bool crawled { set; get; }

        public override string ToString()
        {
            return string.Format("{0}:{1}", text, url);
        }
    }
}
