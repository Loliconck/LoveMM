using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace LoveMM.Common
{
    public class PhotoUploadHelper
    {
        /// <summary>
        /// 图片上传大小限制，默认3M
        /// </summary>
        private int photoSizeLimit = 3 * 1024 * 1024;
        /// <summary>
        /// 图片文件格式数组，默认JPG,JPEG,PNG
        /// </summary>
        private string allowedFileExtensions = "jpg,jpeg,png";

        private string[] extensionsArr = null;
        public PhotoUploadHelper()
        {
            extensionsArr = allowedFileExtensions.Split(',');
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_photoSizeLimit">上传文件限制的大小（以字节为单位）</param>
        /// <param name="_allowedFileExtensions">上传文件限制的扩展名</param>
        public PhotoUploadHelper(int _photoSizeLimit, string _allowedFileExtensions)
        {
            this.photoSizeLimit = _photoSizeLimit;
            this.allowedFileExtensions = _allowedFileExtensions;
            extensionsArr = allowedFileExtensions.Split(',');
        }

        public int PhotoSizeLimit
        {
            get { return photoSizeLimit; }
            set { photoSizeLimit = value; }
        }

        public string AllowedFileExtensions
        {
            get { return allowedFileExtensions; }
            set
            {
                allowedFileExtensions = value;
                extensionsArr = allowedFileExtensions.Split(',');
            }
        }

        /// <summary>
        /// 验证图片文件的大小及各式
        /// </summary>
        /// <param name="img">图片文件</param>
        /// <returns></returns>
        public Tuple<bool, string> ValidatePhoto(HttpPostedFileBase img)
        {
            Tuple<bool, string> result = null;
            if (img == null || string.IsNullOrEmpty(img.FileName) || img.ContentLength == 0)
            {
                result = new Tuple<bool, string>(false, "上传文件为空");
                return result;
            }
            if (img.ContentLength > this.photoSizeLimit)
            {
                result = new Tuple<bool, string>(false, "上传文件不得大于" + this.photoSizeLimit / (1024 * 1024) + "M！");
                return result;
            }
            string extension = FileHelper.GetFileExtension(img.FileName);//获取文件扩展名
            extension = extension.Substring(extension.IndexOf('.') + 1).ToLower();
            if (!extensionsArr.Contains(extension))
            {
                result = new Tuple<bool, string>(false, "只允许上传后缀名为 " + this.allowedFileExtensions + " 的文件！");
                return result;
            }
            return new Tuple<bool, string>(true, "");
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="img">图片文件</param>
        /// <param name="relativePath">保存图片的相对路径</param>
        public Tuple<bool, string> UploadPhoto(HttpPostedFileBase img, string relativePath)
        {
            string extextension = FileHelper.GetFileExtension(img.FileName);//获取文件扩展名
            //string name = Guid.NewGuid().ToString("N") + extextension;//生成新文件名
            string name = DateTime.Now.ToString("yyyyMMddHHmmssfff") + extextension;//生成新文件名
            return UploadPhoto(img, name, relativePath);
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="img">图片文件</param>
        /// <param name="fileName">图片文件名</param>
        /// <param name="relativePath">保存图片的相对路径</param>
        public Tuple<bool, string> UploadPhoto(HttpPostedFileBase img, string fileName, string relativePath)
        {
            try
            {
                Tuple<bool, string> result = ValidatePhoto(img);
                if (!result.Item1)
                {
                    return result;
                }
                if (string.IsNullOrEmpty(relativePath))
                {
                    result = new Tuple<bool, string>(false, "上传文件保存路径错误");
                    return result;
                }
                img.SaveAs(relativePath + fileName);
                result = new Tuple<bool, string>(true, fileName);
                return result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary> 
        /// 从图片流中生成缩略图 
        /// </summary> 
        /// <param name="originalFile">源图</param> 
        /// <param name="thumbnailPath">缩略图保存路径</param>
        /// <param name="newName">缩略图保存用文件名</param>  
        /// <param name="width">缩略图宽度</param> 
        /// <param name="height">缩略图高度</param> 
        /// <param name="mode">生成缩略图的方式</param>     
        public void MakeThumbnail(HttpPostedFileBase img, string thumbnailPath, string name, int width = 278, int height = 180, string mode = "Cut")
        {
            Image originalImage = Image.FromStream(img.InputStream);
            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                 
                    break;
                case "W"://指定宽，高按比例                     
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
            }

            //新建一个bmp图片 
            Image bitmap = new Bitmap(towidth, toheight);

            //新建一个画板 
            Graphics g = Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            try
            {
                Stream stream = new MemoryStream();

                //以jpg格式保存缩略图
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

                stream.Position = 0;
                using (FileStream outStream = File.OpenWrite(thumbnailPath + name))
                {
                    byte[] buffer = new byte[stream.Length > 65536 ? 65536 : stream.Length];

                    int readedSize;
                    while ((readedSize = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outStream.Write(buffer, 0, readedSize);
                    }

                    outStream.Flush();
                    outStream.Close();
                }

                stream.Dispose();
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        /// <summary> 
        /// 从图片路径中生成缩略图 
        /// </summary> 
        /// <param name="originalImagePath">源图路径（物理路径）</param> 
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param> 
        /// <param name="width">缩略图宽度</param> 
        /// <param name="height">缩略图高度</param> 
        /// <param name="mode">生成缩略图的方式</param>    
        public void MakeThumbnailWithPhotoUrl(string originalImagePath, string thumbnailPath, string name, int width = 278, int height = 180, string mode = "Cut")
        {
            Image originalImage = null;
            try
            {
                string filepath = originalImagePath.Replace("/", "\\") + "\\" + name;
                if (File.Exists(filepath))
                {
                    originalImage = Image.FromFile(filepath);
                }
                else
                {
                    return;
                }

            }
            catch (FileNotFoundException ex)
            {
                if (originalImage != null)
                {
                    originalImage.Dispose();
                }
                throw ex;
            }

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                 
                    break;
                case "W"://指定宽，高按比例                     
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片 
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板 
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            try
            {
                Stream stream = new MemoryStream(); 
             
                //以jpg格式保存缩略图 
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

                stream.Position = 0;
                using (FileStream outStream = File.OpenWrite(thumbnailPath + name))
                {
                    byte[] buffer = new byte[stream.Length > 65536 ? 65536 : stream.Length];

                    int readedSize;
                    while ((readedSize = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outStream.Write(buffer, 0, readedSize);
                    }

                    outStream.Flush();
                    outStream.Close();
                }

                stream.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
    }
}