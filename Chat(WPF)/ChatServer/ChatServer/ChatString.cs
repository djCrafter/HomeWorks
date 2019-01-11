using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class ChatString
    {
        public DateTime Time { get; set; }
        public string Str { get; set; }
        

        public ChatString(DateTime time, string str)
        {
            Time = time;
            Str = str;
        }

        public override string ToString()
        {
            return Time.ToLongTimeString() + ' ' + Str;
        }
    }
}
