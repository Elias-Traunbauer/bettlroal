using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bettlroal
{
    [Serializable]
    class Message
    {
        public DateTime date;
        public string sender;
        public string content;

        public Message()
        {
            date = DateTime.Now;
            sender = Form1.name;
        }
    }
}
