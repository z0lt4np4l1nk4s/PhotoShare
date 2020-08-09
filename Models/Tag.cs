using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoShare.Models
{
    public class Tag
    {
        [Key]
        public int ID { get; set; }
        public string Naziv { get; set; }
    }
}