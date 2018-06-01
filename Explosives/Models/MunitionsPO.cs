using System;
using System.ComponentModel.DataAnnotations;

namespace Explosives.Models
{
    public class MunitionsPO
    {

        //Declaring all object properties for Munitions.
        public Int64 MunitionID { get; set; }

        [Display(Name = "Munition")]
        [StringLength(50)]
        [Required]
        public string Munition { get; set; }

        [Display(Name = "Description")]
        [StringLength(500)]
        [Required]
        public string Description { get; set; }

        public Int64 TopicID { get; set; }

    }
}