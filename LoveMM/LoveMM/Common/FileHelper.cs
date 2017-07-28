using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LoveMM.Common
{
    public class FileHelper
    {
        public static void CreateDirectory(string name)
        {
            if (!Directory.Exists(name))
            {
                Directory.CreateDirectory(name);
            }
        }

        /// <summary>
        /// 根据文件路径获取文件的扩展名
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>文件的扩展名</returns>
        public static string GetFileExtension(string path)
        {
            return Path.GetExtension(path);
        }
    }
}