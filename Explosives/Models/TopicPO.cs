using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Explosives.Models
{
    public class TopicPO
    {      
        //Declaring all object properties for Munitions.

        public Int64 TopicID { get; set; }

        [Display(Name = "Topic")]
        [StringLength(30)]
        [Required]
        public string Topic { get; set; }
    }
}