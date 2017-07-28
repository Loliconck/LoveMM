using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LoveMM.Models
{
    //public class PictureContext : DbContext
    //{
    //    public PictureContext()
    //        : base("LoveMMConnection")
    //    {

    //    }
    //    public DbSet<Picture> Pictures { get; set; }
    //}

    [Table("Picture")]
    public class Picture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "频道ID")]
        public int ChannelId { get; set; }

        [Display(Name = "相册ID")]
        public int AlbumId { get; set; }

        [StringLength(300)]
        [Display(Name = "图片名称")]
        public string PicName { get; set; }

        [Display(Name = "是否缩略图")]
        public bool IsThumbnail { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "添加时间")]
        public DateTime CTime { get; set; }

        [Display(Name = "是否删除")]
        public bool IsDelete { get; set; }

        public Album Album { get; set; }
    }
}