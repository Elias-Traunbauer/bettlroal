using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bettlroal
{
    [Serializable]
    class NetworkData
    {
        public List<Message> msgs;

        public NetworkData ()
        {
            msgs = new List<Message>();
        }
    }
}
