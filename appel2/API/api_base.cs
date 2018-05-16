using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appel
{
    public class api_base
    {
        static readonly object _lock = new object();
        static Queue<msg> cache = new Queue<msg>();
        static System.Threading.Timer timer = null;
        static IFORM fom = null;

        public api_base()
        {
            if (fom == null)
                fom = app.get_Main();
            if (timer == null)
            {
                timer = new System.Threading.Timer(new System.Threading.TimerCallback((obj) =>
                {
                    lock (_lock)
                    {
                        if (cache.Count > 0)
                        {
                            msg m = cache.Dequeue();
                            if (fom != null) fom.api_responseMsg(null, new threadMsgEventArgs(m));
                        }
                    }
                }), fom, 100, 100);
            }
        }

        public void f_responseToMain(msg m)
        {
            lock (_lock) cache.Enqueue(m);
        }

        public void f_api_Inited(msg m)
        {
            if (fom != null) fom.api_initMsg(m);
        }
    }
}
