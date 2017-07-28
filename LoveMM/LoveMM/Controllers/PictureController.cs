using LoveMM.Common;
using LoveMM.DAL;
using LoveMM.Filters;
using LoveMM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoveMM.Controllers
{
    /// <summary>
    /// 图片控制器
    /// </summary>
    [AdminAuthorization]
    public class PictureController : Controller
    {
        private static readonly string basePhotoPath = "/Pictures/Channels/";
        private LoveMMContext dbContext = new LoveMMContext();

        public ActionResult Index(int id)
        {
            Album album = dbContext.Albums.Find(id);
            IQueryable<Picture> model = dbContext.Pictures.Where(t => t.AlbumId == id);
            ViewBag.Album = album;
            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            Picture picture = dbContext.Pictures.Find(id);
            if (picture == null)
            {
                return Json(new { result = false });
            }
            try
            {
                picture.IsDelete = true;
                dbContext.Entry(picture).State = EntityState.Modified;
                dbContext.SaveChanges();
                return Json(new { result = true });
            }
            catch
            {
                return Json(new { result = false });
            }
        }

        [HttpPost]
        public JsonResult SetCover(int id)
        {
            Picture picture = dbContext.Pictures.Find(id);
            if (picture == null)
            {
                return Json(new { result = false });
            }
            try
            {
                Picture thumbnail = dbContext.Pictures.FirstOrDefault(t => t.Id == id && t.IsThumbnail == true);
                if (thumbnail != null)
                {
                    thumbnail.IsThumbnail = false;
                    dbContext.Entry(thumbnail).State = EntityState.Modified;
                }
                picture.IsThumbnail = true;
                dbContext.Entry(picture).State = EntityState.Modified;
                dbContext.SaveChanges();
                return Json(new { result = true });
            }
            catch
            {
                return Json(new { result = false });
            }
        }

        [HttpPost]
        public JsonResult CancelDelete(int id)
        {
            Picture picture = dbContext.Pictures.Find(id);
            if (picture == null)
            {
                return Json(new { result = false });
            }
            try
            {
                picture.IsDelete = false;
                dbContext.Entry(picture).State = EntityState.Modified;
                dbContext.SaveChanges();
                return Json(new { result = true });
            }
            catch
            {
                return Json(new { result = false });
            }
        }

        /// <summary>
        /// 相册图片上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AlbumPhotoUpload(HttpPostedFileBase img, int channelId, int albumId)
        {
            if (img == null || string.IsNullOrEmpty(img.FileName) || img.ContentLength == 0)
            {
                return new HttpNotFoundResult("上传文件未找到");
            }
            string relativePath = basePhotoPath + channelId + "/" + albumId + "/";
            string relativeThumbnailPath = relativePath + "Thumbnail/";
            string path = Server.MapPath(relativePath);
            string thumbnailPath = Server.MapPath(relativeThumbnailPath);
            PhotoUploadHelper photoHelper = new PhotoUploadHelper();
            Tuple<bool, string> result = photoHelper.UploadPhoto(img, path);
            if (result.Item1)
            {
                //生成缩略图
                photoHelper.MakeThumbnail(img, thumbnailPath, result.Item2, 233, 349, "CUT");

                Picture pic = new Picture();
                pic.AlbumId = albumId;
                pic.ChannelId = channelId;
                pic.CTime = DateTime.Now;
                pic.IsDelete = false;
                pic.PicName = result.Item2;
                dbContext.Pictures.Add(pic);
                dbContext.SaveChanges();
                return Json(new { result = result.Item1, relativeThumbnailPath, fileName = result.Item2 });
            }
            return Json(new { result = false, relativeThumbnailPath = "", fileName = "" });
        }

        /// <summary>
        /// 频道封面图片上传
        /// </summary>
        /// <param name="img"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChannelPhotoUpload(HttpPostedFileBase img, int channelId)
        {
            if (img == null || string.IsNullOrEmpty(img.FileName) || img.ContentLength == 0)
            {
                return new HttpNotFoundResult("上传文件未找到");
            }
            string relativePath = basePhotoPath + channelId + "/Thumbnail/";
            string path = Server.MapPath(relativePath);
            PhotoUploadHelper photoHelper = new PhotoUploadHelper();
            string extextension = FileHelper.GetFileExtension(img.FileName);//获取文件扩展名
            string name = Guid.NewGuid().ToString("N") + extextension;//生成新文件名
            //生成缩略图
            photoHelper.MakeThumbnail(img, path, name, 745, 340);
            Channel channel = dbContext.Channels.Find(channelId);
            channel.ThumbnailName = name;
            dbContext.SaveChanges();//更新频道的封面图片名称
            return Json(new { result = name });
        }
    }
}
