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
        //public static string currentView="list";
        public ActionResult Index()
        {
            ViewBag.PostTags = db.PhotoVideoTag;
            ViewBag.Tag = db.Tag;
            var items = db.PhotoVideo.OrderByDescending(x => x.ID).Take(5);
            return View(items.ToList());
        }

        public ActionResult Slika(string view, string search, string searchBy, int? page, string sortBy, string viewChange)
        {
            ViewBag.PostTags = db.PhotoVideoTag;
            ViewBag.Tag = db.Tag;
            ViewBag.Naziv = sortBy == "naziv" ? "naziv desc" : "naziv";
            ViewBag.Datum = sortBy == "stariji" ? "noviji" : "stariji";
            if (view == null) view = "grid";
            if (viewChange != null)
            {
                ViewBag.view = viewChange == "list" ? "grid" : "list";
            }
            else { ViewBag.view = view; }
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
            else
            {
                search = null;
                searchBy = null;
            }
            switch (sortBy)
            {
                case "naziv":
                    slikaList = slikaList.OrderBy(x => x.Naziv).AsQueryable();
                    break;
                case "naziv desc":
                    slikaList = slikaList.OrderByDescending(x => x.Naziv).AsQueryable();
                    break;
                case "stariji":
                    slikaList = slikaList.OrderBy(x => x.DatumObjave).AsQueryable();
                    break;
                case "noviji":
                    slikaList = slikaList.OrderByDescending(x => x.DatumObjave).AsQueryable();
                    break;
                default:
                    slikaList = slikaList.OrderBy(x => x.ID).ToList().AsQueryable();
                    break;

            }
            if (ViewBag.view == "grid")
            {
                return View(slikaList.ToPagedList(page ?? 1, 12));
            }
            return View(slikaList.ToPagedList(page ?? 1, 5));
        }

        public ActionResult Video(string view, string search, string searchBy, int? page, string sortBy, string viewChange)
        {
            ViewBag.PostTags = db.PhotoVideoTag;
            ViewBag.Tag = db.Tag;
            ViewBag.Naziv = sortBy == "naziv" ? "naziv desc" : "naziv";
            ViewBag.Datum = sortBy == "stariji" ? "noviji" : "stariji";
            if (view == null)
            {
                view = "grid";
            }
            if (viewChange != null)
            {
                ViewBag.view = viewChange == "list" ? "grid" : "list"; 
            }
            else
            {
                ViewBag.view = view;
            }
            var videoList = db.PhotoVideo.Where(x => x.isSlika == false).ToList().AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                view = Request.QueryString["view"];
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
                        videoList = videoList.Where(x => x.Naziv.ToLower().StartsWith(search.ToLower()) && x.isSlika == false).AsQueryable();
                        break;
                }
            }
            switch (sortBy)
            {
                case "naziv":
                    videoList = videoList.OrderBy(x => x.Naziv).AsQueryable();
                    break;
                case "naziv desc":
                    videoList = videoList.OrderByDescending(x => x.Naziv).AsQueryable();
                    break;
                case "stariji":
                    videoList = videoList.OrderBy(x => x.DatumObjave).AsQueryable();
                    break;
                case "noviji":
                    videoList = videoList.OrderByDescending(x => x.DatumObjave).AsQueryable();
                    break;
                default:
                    videoList = videoList.OrderBy(x => x.ID).ToList().AsQueryable();
                    break;

            }
            if (ViewBag.view == "grid")
            {
                return View(videoList.ToPagedList(page ?? 1, 12));
            }
            return View(videoList.ToPagedList(page ?? 1, 5));

        }
    }
}