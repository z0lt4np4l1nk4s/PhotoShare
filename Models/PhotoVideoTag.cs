using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoShare.Models
{
    public class PhotoVideoTag
    {
        [Key]
        public int ID { get; set; }
        public int PhotoVideoID { get; set; }
        public PhotoVideo PhotoVideo { get; set; }
        public int TagID { get; set; }
        public Tag Tag { get; set; }
    }
}