using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveMM.Models
{
    //public class AlbumContext : DbContext
    //{
    //    public AlbumContext()
    //        : base("LoveMMConnection")
    //    {

    //    }
    //    public DbSet<Album> Albums { get; set; }

    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Entity<Album>().HasMany(t => t.Pictures).WithRequired(t=>t.Album).HasForeignKey(t=>t.AlbumId);
    //    }
    //}

    [Table("Album")]
    public class Album
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "频道ID")]
        public int ChannelId { get; set; }

        [Display(Name = "相册名称")]
        [StringLength(50)]
        public string AlbumName { get; set; }

        [Display(Name = "点击数")]
        public int HitCount { get; set; }

        [Display(Name = "是否有效")]
        public bool IsValid { get; set; }

        [Display(Name = "创建时间")]
        [DataType(DataType.DateTime)]
        public DateTime CTime { get; set; }

        [Display(Name = "是否删除")]
        public bool IsDelete { get; set; }

        [Display(Name = "备注")]
        [StringLength(500)]
        public string Remark { get; set; }

        public Channel Channel { get; set; }

        public virtual ICollection<Picture> Pictures { get; set; }
    }
}