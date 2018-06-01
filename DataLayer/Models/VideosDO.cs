using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class VideosDO
    {
        public Int64 VideoID { get; set; }
        public string VideoName { get; set; }
        public string VideoPath { get; set; }
        public string VideoDescription { get; set; }
        public Int64 MunitionID { get; set; }
        public Int64 UserID { get; set; }
    }
}
