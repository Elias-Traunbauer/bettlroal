using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bettlroal
{
    [Serializable]
    public class NetworkData
    {
        /// <summary>
        /// Default Type Message
        /// </summary>
        public DataType type;

        public List<ImageChunk> chunks;
        public List<Message> msgs;
        public int imageSize;

        public NetworkData ()
        {
            msgs = new List<Message>();
            type = DataType.Message;
        }

        public enum DataType
        {
            Message,
            ImageUpdate
        }
    }
}
