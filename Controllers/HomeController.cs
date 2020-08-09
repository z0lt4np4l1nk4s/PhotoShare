using PhotoShare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoShare.Controllers
{
    public class HomeController : Controller
    {
        PhotoVideoDBContext db = new PhotoVideoDBContext();
        public ActionResult Index()
        {
            ViewBag.PostTags = db.PhotoVideoTag;
            ViewBag.Tag = db.Tag;
            return View(db.PhotoVideo.ToList());
        }

        [HttpPost]
        public ActionResult Index(string search, string searchBy)
        {
            ViewBag.PostTags = db.PhotoVideoTag;
            ViewBag.Tag = db.Tag;
            List<PhotoVideo> pvList = new List<PhotoVideo>();
            switch(searchBy)
            {
                case "Tag":
                    foreach (PhotoVideoTag photoVideoTag in db.PhotoVideoTag)
                    {
                        if(db.Tag.Single(x => x.ID == photoVideoTag.TagID).Naziv.ToLower().StartsWith(search))
                        {
                            pvList.Add(db.PhotoVideo.Single(x => x.ID == photoVideoTag.PhotoVideoID));
                        }
                    }
                    break;
                case "Datum":
                    break;
                default:
                    pvList = db.PhotoVideo.Where(x => x.Naziv.ToLower().StartsWith(search.ToLower())).ToList();
                    break;
            }
            return View(pvList);
        }
    }
}