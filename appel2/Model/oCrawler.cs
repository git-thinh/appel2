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
            return string.Format("{0} = {1}",url , text);
        }
    }

    public class oLinkSetting
    {
        public string Url { set; get; }
        public Dictionary<string, string> Settings { set; get; }

        public oLinkSetting()
        {
            Url = string.Empty;
            Settings = new Dictionary<string, string>();
        }
    }
}
