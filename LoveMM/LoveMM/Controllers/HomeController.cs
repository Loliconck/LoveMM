using LoveMM.DAL;
using LoveMM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoveMM.Common;

namespace LoveMM.Controllers
{
    public class HomeController : Controller
    {
        LoveMMContext dbContext = new LoveMMContext();

        /// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = 60)]
        public PartialViewResult _HomeHeadNav(int channelId)
        {
            List<Channel> channels = dbContext.Channels.Where(t => t.IsDelete == false && t.IsValid && t.ChannelName != "置顶" && t.IsSpecial == false).OrderByDescending(t => t.Id).Take(8).ToList();
            ViewBag.ChannelId = channelId;
            return PartialView("_HomeHeadNav", channels);
        }

        /// <summary>
        /// 首页置顶相册
        /// </summary>
        /// <returns></returns>
        public PartialViewResult _HomeTop()
        {
            Channel channel = dbContext.Channels.FirstOrDefault(t => t.IsDelete == false && t.IsValid && t.ChannelName == "置顶");
            List<Album> model = null;
            if (channel != null)
            {
                model = dbContext.Albums.Where(t => !t.IsDelete && t.IsValid && t.ChannelId == channel.Id).OrderByDescending(t => t.Id).Take(5).ToList();
            }
            ViewBag.Channel = channel ?? new Channel();
            return PartialView("_HomeTop", model);
        }

        /// <summary>
        /// 最近更新的相册的部分视图
        /// </summary>
        /// <returns></returns>
        public PartialViewResult _HomeLatestAlbums()
        {
            List<Album> model = dbContext.Albums.Where(t => !t.IsDelete && t.IsValid && t.Channel.IsSpecial == false).OrderByDescending(t => t.Id).Take(10).ToList();
            return PartialView("_HomeLatestAlbums", model);
        }

        /// <summary>
        /// 首页网友浏览
        /// </summary>
        /// <returns></returns>
        public PartialViewResult _HomeUsersBrowse()
        {
            List<Album> model = dbContext.Albums.Where(t => t.IsDelete == false && t.IsValid && t.Channel.IsSpecial == false).OrderByDescending(t => t.HitCount).Take(5).ToList();
            return PartialView("_HomeUsersBrowse", model);
        }

        /// <summary>
        /// 首页推荐美图
        /// </summary>
        /// <returns></returns>
        public PartialViewResult _HomeRecommedAlbums()
        {
            Channel channel = dbContext.Channels.FirstOrDefault(t => t.IsDelete == false && t.IsValid && t.ChannelName == "推荐美图");
            List<Album> model = null;
            if (channel != null)
            {
                model = dbContext.Albums.Where(t => !t.IsDelete && t.IsValid && t.ChannelId == channel.Id).OrderByDescending(t => t.Id).Take(10).ToList();
            }
            return PartialView("_HomeRecommedAlbums", model);
        }

        /// <summary>
        /// 频道部分视图
        /// </summary>
        /// <returns></returns>
        public PartialViewResult _HomeChannels()
        {
            List<Channel> model = dbContext.Channels.Where(t => !t.IsDelete && t.IsValid && t.ChannelName != "置顶" && t.ChannelName != "推荐美图" && t.IsSpecial == false).OrderByDescending(t => t.OrderNum).Take(8).ToList();
            return PartialView("_HomeChannels", model);
        }

        /// <summary>
        /// 频道二级页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[OutputCache(Duration = 3600, VaryByParam = "id;pageIndex")]
        public ActionResult Channel(int id, int pageIndex = 1)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            Channel channel = dbContext.Channels.Find(id);
            channel.HitCount = channel.HitCount + 1;
            dbContext.SaveChanges();
            PagingBase<Album> model = new PagingBase<Album>();
            model.PageIndex = pageIndex;
            model.PagingData = dbContext.Albums.Where(t => !t.IsDelete && t.IsValid && t.ChannelId == id).OrderByDescending(t => t.Id).Skip(model.PageSize * (model.PageIndex - 1)).Take(model.PageSize).ToList();
            model.TotalCount = dbContext.Albums.Where(t => !t.IsDelete && t.IsValid && t.ChannelId == id).Count();
            ViewBag.Channel = channel;
            return View(model);
        }

        /// <summary>
        /// 频道页推荐相册（4个）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[OutputCache(Duration = 3600, VaryByParam = "id")]
        public PartialViewResult _ChannelRecommedAlbums(int id)
        {
            List<Album> albums = dbContext.Albums.Where(t => !t.IsDelete && t.IsValid && t.ChannelId == id).OrderByNewId().Take(4).ToList();
            return PartialView("_ChannelRecommedAlbums", albums);
        }

        /// <summary>
        /// 相册页面
        /// </summary>
        /// <param name="id">相册ID</param>
        /// <returns></returns>
        public ActionResult Album(int id, int pageIndex = 1)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            Album album = dbContext.Albums.Find(id);
            album.HitCount = album.HitCount + 1;
            dbContext.SaveChanges();
            PagingBase<Picture> model = new PagingBase<Picture>();
            model.PageIndex = pageIndex;
            model.PageSize = 5;
            model.PagingData = dbContext.Pictures.Where(t => !t.IsDelete && t.AlbumId == id).OrderByDescending(t => t.Id).Skip(model.PageSize * (model.PageIndex - 1)).Take(model.PageSize).ToList();
            model.TotalCount = dbContext.Pictures.Where(t => !t.IsDelete && t.AlbumId == id).Count();
            album.Channel = dbContext.Channels.Find(album.ChannelId);
            ViewBag.Album = album;

            #region 获取当前相册的上一条和下一条相册

            ViewBag.PreviousAlbum = dbContext.Albums.Where(t => t.Id < id && t.ChannelId == album.ChannelId).OrderByDescending(t => t.Id).FirstOrDefault();
            ViewBag.NextAlbum = dbContext.Albums.Where(t => t.Id > id && t.ChannelId == album.ChannelId).OrderBy(t => t.Id).FirstOrDefault();

            #endregion

            return View("Album", model);
        }

        /// <summary>
        /// 相册页随机推荐的相关相册
        /// </summary>
        /// <returns></returns>
        //[OutputCache(Duration = 3600, VaryByParam = "channelId")]
        public PartialViewResult _RandomRecommedAlbums(int channelId)
        {
            List<Album> model = dbContext.Albums.Where(t => !t.IsDelete && t.IsValid && t.ChannelId == channelId && t.Pictures.Count() > 0).OrderByNewId().Take(5).ToList();
            return PartialView("_RandomRecommedAlbums", model);
        }

        /// <summary>
        /// 相册页热门推荐相册
        /// </summary>
        /// <returns></returns>
        public PartialViewResult _RecommedHotAlbums()
        {
            List<Album> model = dbContext.Albums.Where(t => !t.IsDelete && t.IsValid && t.Channel.IsSpecial == false).OrderByDescending(t => t.HitCount).Take(5).ToList();
            return PartialView("_RecommedHotAlbums", model);
        }
    }
}
