using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class TopicDO
    {
        public Int64 TopicID { get; set; }
        public string Topic { get; set; }
        public int Count { get; set; }
    }
}
