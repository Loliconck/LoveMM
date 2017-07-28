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
    /// 相册控制器
    /// </summary>
    [AdminAuthorization]
    public class AlbumController : Controller
    {
        private static string ChannelBaseDirectory = "~/Pictures/Channels/";
        private LoveMMContext dbContext = new LoveMMContext();

        [HttpGet]
        public ActionResult Index(int id)
        {
            IQueryable<Album> list = dbContext.Albums.Where(t => t.ChannelId == id).OrderBy(t => t.Id);
            ViewBag.ChannelId = id;
            return View(list);
        }

        [HttpGet]
        public ActionResult Create(int id)
        {
            Album album = new Album();
            album.ChannelId = id;
            return View(album);
        }

        /// <summary>
        /// 创建相册
        /// </summary>
        /// <param name="album"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Album album)
        {
            if (ModelState.IsValid)
            {
                album.CTime = DateTime.Now;
                Album dbAlbum = dbContext.Albums.Add(album);
                dbContext.SaveChanges();
                string albumPath = Server.MapPath(ChannelBaseDirectory + album.ChannelId + "/" + dbAlbum.Id);
                FileHelper.CreateDirectory(albumPath);
                FileHelper.CreateDirectory(albumPath + "/Thumbnail");//创建缩略图文件夹
                return RedirectToAction("Index", new { id = album.ChannelId });
            }
            ModelState.AddModelError("CreateError", "创建相册出错。");
            return View(album);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Album album = dbContext.Albums.Find(id);
            return View(album);
        }

        [HttpPost]
        public ActionResult Edit(Album album)
        {
            if (ModelState.IsValid)
            {
                Album oldAlbum = dbContext.Albums.Find(album.Id);
                oldAlbum.AlbumName = album.AlbumName;
                oldAlbum.IsValid = album.IsValid;
                oldAlbum.Remark = album.Remark;
                dbContext.Entry(oldAlbum).State = EntityState.Modified;
                dbContext.SaveChanges();
                return RedirectToAction("Index", new { id = oldAlbum.ChannelId });
            }
            return View(album);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Album album = dbContext.Albums.Find(id);
            return View(album);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Album album = dbContext.Albums.Find(id);
            return View(album);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            Album album = dbContext.Albums.Find(id);
            album.IsDelete = true;
            dbContext.Entry(album).State = EntityState.Modified;
            dbContext.SaveChanges();
            return RedirectToAction("Index", new { id = album.ChannelId });
        }
    }
}
