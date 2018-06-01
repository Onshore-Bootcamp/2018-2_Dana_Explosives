using System;
using System.ComponentModel.DataAnnotations;

namespace Explosives.Models
{
    public class VideosPO
    {
        //Declaring all object properties for Videos
        public Int64 VideoID { get; set; }

        [Display(Name = "Video Name")]
        [StringLength(50)]
        [Required]
        public string VideoName { get; set; }

        public string VideoPath { get; set; }

        [Display(Name = "Video Description")]
        [StringLength(200)]
        [Required]
        public string VideoDescription { get; set; }

        public Int64 MunitionID { get; set; }

        public Int64 UserID { get; set; }

    }
}