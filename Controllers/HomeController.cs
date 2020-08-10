using PhotoShare.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
            var items = db.PhotoVideo.OrderByDescending(x => x.ID).Take(5);
            return View(items.ToList());
        }

        public ActionResult Slika()
        {
            ViewBag.PostTags = db.PhotoVideoTag;
            ViewBag.Tag = db.Tag;
            return View(db.PhotoVideo.Where(x => x.isSlika == true).ToList());
        }

        [HttpPost]
        public ActionResult Slika(string search, string searchBy)
        {
            ViewBag.PostTags = db.PhotoVideoTag;
            ViewBag.Tag = db.Tag;
            List<PhotoVideo> slikaList = new List<PhotoVideo>();
            switch(searchBy)
            {
                case "Tag":
                    foreach (PhotoVideoTag photoVideoTag in db.PhotoVideoTag)
                    {
                        if(db.Tag.Single(x => x.ID == photoVideoTag.TagID).Naziv.ToLower().StartsWith(search))
                        {
                            slikaList.Add(db.PhotoVideo.Single(x => x.ID == photoVideoTag.PhotoVideoID));
                        }
                    }
                    break;
                case "Datum":
                    break;
                default:
                    slikaList = db.PhotoVideo.Where(x => x.Naziv.ToLower().StartsWith(search.ToLower())).ToList();
                    break;
            }
            return View(slikaList);
        }

        public ActionResult Video()
        {
            ViewBag.PostTags = db.PhotoVideoTag;
            ViewBag.Tag = db.Tag;
            return View(db.PhotoVideo.Where(x => x.isSlika == false).ToList());
        }

        [HttpPost]
        public ActionResult Video(string search, string searchBy)
        {
            ViewBag.PostTags = db.PhotoVideoTag;
            ViewBag.Tag = db.Tag;
            List<PhotoVideo> videoList = new List<PhotoVideo>();
            switch (searchBy)
            {
                case "Tag":
                    foreach (PhotoVideoTag photoVideoTag in db.PhotoVideoTag)
                    {
                        if (db.Tag.Single(x => x.ID == photoVideoTag.TagID).Naziv.ToLower().StartsWith(search))
                        {
                            videoList.Add(db.PhotoVideo.Single(x => x.ID == photoVideoTag.PhotoVideoID));
                        }
                    }
                    break;
                default:
                    videoList = db.PhotoVideo.Where(x => x.Naziv.ToLower().StartsWith(search.ToLower())).ToList();
                    break;
            }
            return View(videoList);
        }
    }
}