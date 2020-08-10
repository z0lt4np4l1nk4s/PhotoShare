using PhotoShare.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PhotoShare.Controllers
{
    public class PhotoVideoController : Controller
    {
        List<string> ImageExtensions = new List<string> { ".jpg", ".jpe", ".bmp", ".gif", ".png", ".jpeg", ".jfif" };
        PhotoVideoDBContext db = new PhotoVideoDBContext();
        // GET: PhotoVideo
        public ActionResult Create()
        {
            if (db.Tag.Count() != 0)
            {
                List<SelectListItem> list = new List<SelectListItem>();
                foreach(Tag t in db.Tag)
                {
                    SelectListItem selectListItem = new SelectListItem();
                    selectListItem.Value = t.ID.ToString();
                    selectListItem.Text = t.Naziv;
                    list.Add(selectListItem);
                }
                Session["Tag"] = list;
            }
            else
            {
                Session["Tag"] = null; 
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(PhotoVideo photoVideo, string[] tags, string tag)
        {
            if (tags == null && string.IsNullOrEmpty(tag))
            {
                return View(photoVideo);
            }
            else
            {
                List<string> text = new List<string>();
                StringBuilder sb = new StringBuilder();
                foreach (char c in tag)
                {
                    if (!char.IsDigit(c) && !char.IsLetter(c))
                    {
                        if (sb.ToString() != "")
                        {
                            text.Add(sb.ToString());
                        }
                        sb.Clear();
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
                text.Add(sb.ToString());
                photoVideo.Naziv = Path.GetFileNameWithoutExtension(photoVideo.PhotoVideoFile.FileName);
                if (ImageExtensions.Contains(Path.GetExtension(photoVideo.PhotoVideoFile.FileName)))
                {
                    photoVideo.isSlika = true;
                }
                else
                {
                    photoVideo.isSlika = false;
                }
                string fileName = photoVideo.Naziv + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(photoVideo.PhotoVideoFile.FileName);
                photoVideo.Path = "~/Files/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Files/"), fileName);
                photoVideo.PhotoVideoFile.SaveAs(fileName);
                photoVideo.DatumObjave = DateTime.Now;
                db.PhotoVideo.Add(photoVideo);
                db.SaveChanges();
                int id = db.PhotoVideo.Single(x => x.Path == photoVideo.Path).ID;
                AddTagToPost(id, tags, text);
                if (photoVideo.isSlika)
                {
                    return RedirectToAction("Slika", "Home");
                }
                else
                {
                    return RedirectToAction("Video", "Home");
                }
                
            }
        }

        private void AddTagToPost(int id, string[] tags, List<string> list)
        {
            PhotoVideoTag photoVideoTag = new PhotoVideoTag();
            photoVideoTag.PhotoVideo = db.PhotoVideo.Single(x => x.ID == id);
            if (tags != null)
            {
                if (tags.Length != 0)
                {
                    foreach (string s in tags)
                    {
                        photoVideoTag.Tag = db.Tag.Single(x => x.ID.ToString() == s);
                        db.PhotoVideoTag.Add(photoVideoTag);
                        db.SaveChanges();
                    }
                }
            }
            foreach(string s in list)
            {
                if (db.Tag.Count(x => x.Naziv == s) == 0)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        db.Tag.Add(new Tag { Naziv = s });
                        db.SaveChanges();
                        photoVideoTag.Tag = db.Tag.Single(x => x.Naziv.ToLower() == s.ToLower());
                        db.PhotoVideoTag.Add(photoVideoTag);
                    }
                }
            }
            db.SaveChanges();
        }
    }
}