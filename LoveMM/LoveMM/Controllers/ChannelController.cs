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
    [AdminAuthorization]
    public class ChannelController : Controller
    {
        private static string ChannelBaseDirectory = "~/Pictures/Channels/";
        private LoveMMContext dbContext = new LoveMMContext();

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<Channel> channels = dbContext.Channels.Where(t => t.IsDelete == false).OrderBy(t => t.OrderNum);
            return View(channels);
        }

        [HttpGet]
        public ActionResult Create()
        {
            Channel channel = new Channel();
            return View(channel);
        }

        [HttpPost]
        public ActionResult Create(Channel channel)
        {
            if (ModelState.IsValid)
            {
                channel.CTime = DateTime.Now;
                channel.IsDelete = false;
                channel.HitCount = 0;
                Channel dbChannel = dbContext.Channels.Add(channel);
                dbContext.SaveChanges();
                string directory = Server.MapPath(ChannelBaseDirectory + dbChannel.Id);
                FileHelper.CreateDirectory(directory);
                FileHelper.CreateDirectory(directory + "/Thumbnail");
                return RedirectToAction("Index");
            }
            return View(channel);
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            Channel channel = dbContext.Channels.Find(id);
            return View(channel);
        }

        [HttpPost]
        public ActionResult Edit(Channel channel)
        {
            if (ModelState.IsValid)
            {
                Channel OldChannel = dbContext.Channels.Find(channel.Id);
                OldChannel.ChannelName = channel.ChannelName;
                OldChannel.IsValid = channel.IsValid;
                OldChannel.OrderNum = channel.OrderNum;
                OldChannel.Remark = channel.Remark;
                dbContext.Entry(OldChannel).State = EntityState.Modified;
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(channel);
        }

        [HttpGet]
        public ActionResult Delete(int id = 0)
        {
            Channel channel = dbContext.Channels.Find(id);
            return View(channel);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Channel channel = dbContext.Channels.Find(id);
            channel.IsDelete = true;
            dbContext.Entry(channel).State = EntityState.Modified;
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
