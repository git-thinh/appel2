using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appel
{
    public interface IAPI
    {
        msg Execute(msg msg);
        void Close();
    }
}
