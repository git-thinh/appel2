using Newtonsoft.Json;
using ProtoBuf;
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

        public int PageNumber = 1;

        public int PageSize = 10;

        public int Counter = 0;

        public object Input { set; get; }

        public msgOutput Output { set; get; }
        public msg()
        {
            Output = new msgOutput();
        }

        public msg clone()
        {
            msg m = new msg()
            {
                API = this.API,
                KEY = this.KEY,
                Log = this.Log,
                Input = this.Input,
                PageNumber = this.PageNumber,
                PageSize = this.PageSize,
                Counter = this.Counter,
                Output = null,
            };
            //if (input != null)
            //{
            //    string json = JsonConvert.SerializeObject(input);
            //    Type type = input.GetType();
            //    m.Input = JsonConvert.DeserializeObject(json, type);
            //}
            //if (output != null)
            //{
            //    string json = JsonConvert.SerializeObject(output);
            //    m.Output = JsonConvert.DeserializeObject<msgOutput>(json);
            //    m.Output.Data = null;
            //}
            return m;
        }
    }

}
