using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace appel
{
    public class oWordCount
    {
        public bool analytic = false;
        public string word { set; get; }
        public int count { set; get; }
    }

    public class oWordContent
    {
        public oWordCount[] words { set; get; }

        public string[] sentences { set; get; }
    }

    public class oWord
    {
        public string word { set; get; }

        public string mean_vi { set; get; }

        public List<string> sentences { set; get; }

        public string mean_oxford { set; get; }
        public string mean_cambridge { set; get; }
    } 
}
