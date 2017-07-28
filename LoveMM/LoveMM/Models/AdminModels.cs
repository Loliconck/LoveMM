using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoveMM.Models
{
    //public class AdminContext : DbContext
    //{
    //    public AdminContext()
    //        : base("LoveMMConnection")
    //    {

    //    }

    //    public DbSet<Admin> Admins { get; set; }
    //}

    [Table("Admin")]
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [DataType(DataType.Password), StringLength(100)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "最后登录时间")]
        public DateTime LastLoginTime { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "注册时间")]
        public DateTime CTime { get; set; }

        [Display(Name = "是否删除")]
        public bool IsDelete { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "用户名"), StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password), StringLength(100)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }
}