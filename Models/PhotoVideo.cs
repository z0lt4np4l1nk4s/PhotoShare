using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PhotoShare.Models
{
    public class PhotoVideo
    {
        [Key]
        public int ID { get; set; }
        public string Naziv { get; set; }
        [Display(Name = "Vrijeme objave")]
        public DateTime DatumObjave { get; set; }
        public string Path { get; set; }
        public bool isSlika { get; set; }
        [NotMapped]
        public HttpPostedFileBase PhotoVideoFile { get; set; }
    }
}