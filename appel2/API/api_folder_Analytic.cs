using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace appel
{
    public class api_folder_Analytic : api_base, IAPI
    {
        static readonly object _lock = new object();
        static List<string> listWordDownload = new List<string>() { };

        public msg Execute(msg m)
        {
            if (m == null || m.Input == null) return m;
            string path = (string)m.Input;
            if (Directory.Exists(path))
            {

                m.KEY = string.Empty;
                m.Output.Ok = false;
            }

            m.Output.Ok = true;
            //m.Output.Data = wo.words;
            return m;
        }
        public void Close() { }
    }
}
