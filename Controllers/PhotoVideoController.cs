using PhotoShare.Models;
using PhotoShare.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;

namespace PhotoShare.Controllers
{
    
    public class PhotoVideoController : Controller
    {
        List<string> ImageExtensions = new List<string> { ".jpg", ".jpe", ".bmp", ".gif", ".png", ".jpeg", ".jfif" };
        PhotoVideoDBContext db = new PhotoVideoDBContext();
        string fName;
        bool isEdit = false;

        [Authorize]
        public ActionResult Create()
        {
            isEdit = false;
            FillListBox(0);
            return View();
        }

        private void FillListBox(int id)
        {
            if (db.Tag.Count() != 0)
            {
                List<SelectListItem> list = new List<SelectListItem>();
                foreach (Tag t in db.Tag)
                {
                    SelectListItem selectListItem = new SelectListItem();
                    selectListItem.Value = t.ID.ToString();
                    selectListItem.Text = t.Naziv;
                    if (isEdit)
                    {
                        foreach (PhotoVideoTag pvt in db.PhotoVideoTag)
                        {
                            if (pvt.PhotoVideoID == id && pvt.TagID == t.ID)
                            {
                                selectListItem.Selected = true;
                                continue;
                            }
                        }
                    }
                    list.Add(selectListItem);
                }
                Session["Tag"] = list;
            }
            else
            {
                Session["Tag"] = null;
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(PhotoVideo photoVideo, string[] tags, string tag)
        {
            /*
            if (tags == null && string.IsNullOrEmpty(tag))
            {
                return View(photoVideo);
            }
            else
            {
                List<string> text = GetTagsFromText(tag);
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
                if (!string.IsNullOrEmpty(fName))
                {
                    photoVideo.Path = "~/Files/" + fName;
                }
                else
                {
                    photoVideo.Path = "~/Files/" + photoVideo.Naziv + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(photoVideo.PhotoVideoFile.FileName);
                }
                photoVideo.DatumObjave = DateTime.Now;
                db.PhotoVideo.Add(photoVideo);
                db.SaveChanges();
                int id = db.PhotoVideo.Single(x => x.Path == photoVideo.Path).ID;
                AddTagToPost(id, tags, text);
            */
            if (photoVideo.isSlika)
            {
                return RedirectToAction("Slika", "Home");
            }
            else
            {
                return RedirectToAction("Video", "Home");
            }

            //}
        }

        private List<string> GetTagsFromText(string tag)
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
            return text;
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
            foreach (string s in list)
            {
                if (db.Tag.Count(x => x.Naziv == s) == 0)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        db.Tag.Add(new Tag { Naziv = s.ToLower() });
                        db.SaveChanges();
                        photoVideoTag.Tag = db.Tag.Single(x => x.Naziv.ToLower() == s.ToLower());
                        db.PhotoVideoTag.Add(photoVideoTag);
                    }
                }
                else
                {
                    photoVideoTag.Tag = db.Tag.Single(x => x.Naziv == s);
                    db.PhotoVideoTag.Add(photoVideoTag);
                    db.SaveChanges();
                }
            }
            db.SaveChanges();
        }

        private void RemoveTagsFromPost(int id)
        {
            db.PhotoVideoTag.RemoveRange(db.PhotoVideoTag.Where(x => x.PhotoVideoID == id));
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            ViewBag.PostTags = db.PhotoVideoTag;
            ViewBag.Tag = db.Tag;
            return View(db.PhotoVideo.Single(x => x.ID == id));
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            ViewBag.PostTags = db.PhotoVideoTag;
            ViewBag.Tag = db.Tag;
            isEdit = true;
            FillListBox(id);
            return View(db.PhotoVideo.Single(x => x.ID == id));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(PhotoVideo photoVideo, string[] tags, string tag)
        {
            PhotoVideo pv = db.PhotoVideo.Single(x => x.ID == photoVideo.ID);
            if (tags == null && string.IsNullOrEmpty(tag))
            {
                return View(db.PhotoVideo.Single(x => x.ID == photoVideo.ID));
            }
            else
            {
                RemoveTagsFromPost(photoVideo.ID);
                AddTagToPost(photoVideo.ID, tags, GetTagsFromText(tag));
                if (pv.isSlika)
                {
                    return RedirectToAction("Slika", "Home");
                }
                else
                {
                    return RedirectToAction("Video", "Home");
                }
            }
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            ViewBag.PostTags = db.PhotoVideoTag;
            ViewBag.Tag = db.Tag;
            return View(db.PhotoVideo.Single(x => x.ID == id));
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhotoVideo photoVideo = db.PhotoVideo.Single(x => x.ID == id);
            if (System.IO.File.Exists(Request.MapPath(photoVideo.Path)))
            {
                System.IO.File.Delete(Request.MapPath(photoVideo.Path));
            }
            RemoveTagsFromPost(photoVideo.ID);
            db.PhotoVideo.Remove(photoVideo);
            db.SaveChanges();
            if (photoVideo.isSlika)
            {
                return RedirectToAction("Slika", "Home");
            }
            else
            {
                return RedirectToAction("Video", "Home");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage UploadFile()
        {
            foreach (string file in Request.Files)
            {
                var FileDataContent = Request.Files[file];
                if (FileDataContent != null && FileDataContent.ContentLength > 0)
                {
                    // take the input stream, and save it to a temp folder using the original file.part name posted
                    var stream = FileDataContent.InputStream;
                    var fileName = Path.GetFileName(FileDataContent.FileName);
                    var UploadPath = Server.MapPath("~/App_Data");
                    Directory.CreateDirectory(UploadPath);
                    string path = Path.Combine(UploadPath, fileName);
                    try
                    {
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        // Once the file part is saved, see if we have enough to merge it
                        Shared.Utils UT = new Shared.Utils();
                        UT.MergeFile(path);
                    }
                    catch (IOException ex)
                    {
                        // handle
                    }
                }
            }
            return new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("File uploaded.")
            };
        }
    }
}