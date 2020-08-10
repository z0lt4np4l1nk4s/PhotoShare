using PagedList;
using PhotoShare.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

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

        public ActionResult Slika(string search, string searchBy, int? page)
        {
            ViewBag.PostTags = db.PhotoVideoTag;
            ViewBag.Tag = db.Tag;
            var slikaList = db.PhotoVideo.Where(x => x.isSlika == true).ToList().AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                switch (searchBy)
                {
                    case "Tag":
                        List<PhotoVideo> list = new List<PhotoVideo>();
                        foreach (PhotoVideoTag photoVideoTag in db.PhotoVideoTag)
                        {
                            if (db.Tag.Single(x => x.ID == photoVideoTag.TagID).Naziv.ToLower().StartsWith(search))
                            {
                                list.Add(db.PhotoVideo.Single(x => x.ID == photoVideoTag.PhotoVideoID));
                            }
                        }
                        slikaList = list.Where(x => x.isSlika == true).AsQueryable();
                        break;
                    default:
                        slikaList = slikaList.Where(x => x.Naziv.ToLower().StartsWith(search.ToLower()) && x.isSlika == true);
                        break;
                }
            }
            return View(slikaList.ToPagedList(page ?? 1, 5));
        }

        public ActionResult Video(string search, string searchBy, int? page)
        {
            ViewBag.PostTags = db.PhotoVideoTag;
            ViewBag.Tag = db.Tag;
            var videoList = db.PhotoVideo.Where(x => x.isSlika == false).ToList().AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                switch (searchBy)
                {
                    case "Tag":
                        List<PhotoVideo> list = new List<PhotoVideo>();
                        foreach (PhotoVideoTag photoVideoTag in db.PhotoVideoTag)
                        {
                            if (db.Tag.Single(x => x.ID == photoVideoTag.TagID).Naziv.ToLower().StartsWith(search))
                            {
                                list.Add(db.PhotoVideo.Single(x => x.ID == photoVideoTag.PhotoVideoID));
                            }
                        }
                        videoList = list.Where(x => x.isSlika == false).AsQueryable();
                        break;
                    default:
                        videoList = videoList.Where(x => x.Naziv.ToLower().StartsWith(search.ToLower()) && x.isSlika == false).ToList().AsQueryable();
                        break;
                }
            }
            return View(videoList.ToPagedList(page ?? 1, 5));
        }
    }
}