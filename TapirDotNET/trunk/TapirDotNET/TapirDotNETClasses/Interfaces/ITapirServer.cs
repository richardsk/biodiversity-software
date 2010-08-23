using System;
using System.Collections.Generic;
using System.Text;

namespace TapirDotNET
{
    public interface ITapirServer
    {
        TpResponse ProcessRequest(TpRequest req);
    }
}

