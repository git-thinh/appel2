using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace appel
{
    public class oCaptionWord
    {
        public int TimeStart { set; get; } 
        public string Word { set; get; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", this.TimeStart, this.Word);
        }

        public oCaptionWord() { }
        public oCaptionWord(XElement node) {
            Word = node.Value.Trim(); 
            string t = node.Attribute("t") == null ? string.Empty : node.Attribute("t").Value;
            if (!string.IsNullOrEmpty(t))
            {
                int tt = 0;
                int.TryParse(t, out tt);
                this.TimeStart = tt;
            }
        }
    }

    public class oCaptionSentence
    {
        public int Index { set; get; }
        public int TimeStart { set; get; }
        public int Delay { set; get; }
        public string Words { set; get; }
        public List<int> ListIndex = new List<int>();

        public override string ToString()
        {
            return string.Format("{0}|{1} - {2}: {3}", this.Index, this.TimeStart, this.Delay, this.Words);
        }
    }
}
