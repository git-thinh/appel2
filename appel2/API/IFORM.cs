﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace appel
{
    public interface IFORM
    {
        void api_responseMsg(object sender, threadMsgEventArgs e);
        void f_form_freeResource();
    }
}
