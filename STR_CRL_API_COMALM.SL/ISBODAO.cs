using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR_CRL_API_COMALM.SL
{
    public interface ISBODAO
    {
        int add(object obj);
        int update(object obj);
        object readAll();
        object read(string id);

    }
}
