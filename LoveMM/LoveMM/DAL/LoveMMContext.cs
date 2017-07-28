using LoveMM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LoveMM.DAL
{
    public class LoveMMContext : DbContext
    {
        private readonly static string CONNECTION_STRING = "LoveMMConnection";

        public LoveMMContext()
            : base(CONNECTION_STRING)
        {

        }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Channel> Channels { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Channel>().HasMany(t => t.Albums).WithRequired(t => t.Channel).HasForeignKey(t => t.ChannelId);
            modelBuilder.Entity<Album>().HasMany(t => t.Pictures).WithRequired(t => t.Album).HasForeignKey(t => t.AlbumId);
        }
    }
}