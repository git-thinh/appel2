using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace appel
{
    //Used for WM_COPYDATA for string messages
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpData;
    }

    public class msgOutput
    {
        public bool Ok = false;
        public int Total = 0;
        public object Data { set; get; } 
    }

    public class msg
    {
        public string API = string.Empty;

        public string KEY = string.Empty;

        public string Log = string.Empty;

        public object Input { set; get; }
        public msgOutput Output { set; get; }
        public msg() {
            Output = new msgOutput();
        }

        public static msg create_Key(string key, object data)
        {
            return new msg() { Input = data, KEY = key };
        }
    }

}
