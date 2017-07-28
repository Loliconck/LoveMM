using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LoveMM.Models
{
    [Table("Channel")]
    public class Channel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "频道名称")]
        [StringLength(50)]
        public string ChannelName { get; set; }

        [Display(Name = "频道排序")]
        public int OrderNum { get; set; }

        [Display(Name = "是否有效")]
        public bool IsValid { get; set; }

        [Display(Name = "点击数")]
        public int HitCount { get; set; }

        [Display(Name = "是否专题")]
        public bool IsSpecial { get; set; }

        [Display(Name = "频道图片")]
        public string ThumbnailName { get; set; }

        [Display(Name = "备注")]
        [StringLength(500)]
        public string Remark { get; set; }

        [Display(Name = "创建时间")]
        [DataType(DataType.DateTime)]
        public DateTime CTime { get; set; }

        [Display(Name = "是否删除")]
        public bool IsDelete { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}