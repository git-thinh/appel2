using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace appel
{

    [ProtoContract]
    public class oAppSize
    {
        [ProtoMember(1)]
        public int top { set; get; }

        [ProtoMember(2)]
        public int left { set; get; }

        [ProtoMember(3)]
        public int left_width { set; get; }

        [ProtoMember(4)]
        public int right_width { set; get; }

        [ProtoMember(5)]
        public int width { set; get; }

        [ProtoMember(6)]
        public int height { set; get; }

        public oAppSize()
        {
            top = 40;
            left = 0;
            left_width = 250;
            right_width = 350;
            width = 999;
            height = 600;
        }
    }

    [ProtoContract]
    public class oSetting
    {
        [ProtoMember(1)]
        public List<string> list_folder { set; get; }
        
        [ProtoMember(2)]
        public oAppSize app_size { set; get; }

        [ProtoMember(3)]
        public long node_opening { get; set; }

        public List<long> list_package { set; get; }
        public List<long> list_book { set; get; }

        public oSetting()
        {
            list_folder = new List<string>();
            list_package = new List<long>();
            list_book = new List<long>();
            app_size = new oAppSize();
            node_opening = 0;
        }
    }
}
