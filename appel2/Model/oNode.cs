using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace appel
{
    public enum oNodeType
    {
        NONE,
        TEXT,
        PDF,
        DOC,
        DOCX,
        PPT,
        PPTX,
        XLS,
        XLSX,
        HTM,
        HTML,
        FOLDER,
        PACKAGE,
        PACKAGE_ARTICLE,
        YOUTUBE,
        LINK,
        TAG,
    }
    
    [ProtoContract]
    public class oNode
    {
        [ProtoMember(1)]
        public long id { set; get; }

        [ProtoMember(2)]
        public string name { set; get; }

        [ProtoMember(3)]
        public string title { set; get; }

        string _path = string.Empty;
        [ProtoMember(4)]
        public string path
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _path = value.ToLower().Trim();
                    if (_path.EndsWith(".txt"))
                        type = oNodeType.TEXT;
                    else if (_path.EndsWith(".pdf"))
                        type = oNodeType.PDF;
                    else if (_path.EndsWith(".docx"))
                        type = oNodeType.DOCX;
                    else if (_path.EndsWith(".doc"))
                        type = oNodeType.DOC;
                    else if (_path.EndsWith(".XLSX"))
                        type = oNodeType.XLSX;
                    else if (_path.EndsWith(".XLS"))
                        type = oNodeType.XLS;
                    else if (_path.EndsWith(".pptx"))
                        type = oNodeType.PPTX;
                    else if (_path.EndsWith(".ppt"))
                        type = oNodeType.PPT;
                    //else if (Directory.Exists(_path))
                    //    type = oNodeType.FOLDER;
                }
            }
            get
            {
                return _path;
            }
        }

        [ProtoMember(5)]
        public bool anylatic { set; get; }

        [ProtoMember(6)]
        public oNodeType type { get; set; }

        public string content { get; set; }

        private static readonly Random getrandom = new Random();
        public oNode()
        {
            lock (getrandom) // synchronize
                id = long.Parse(DateTime.Now.AddMilliseconds(getrandom.Next(0, 999)).ToString("yyMMddHHmmssfff"));
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}; {2}", type, title, path);
        }
    }
}
