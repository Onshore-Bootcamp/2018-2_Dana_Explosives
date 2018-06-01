using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class MunitionsDO
    {
        //Declaring Munition properties
        public Int64 MunitionID { get; set; }
        public string Munition { get; set; }
        public string Description { get; set; }
        public Int64 TopicID { get; set; }
    }
}
