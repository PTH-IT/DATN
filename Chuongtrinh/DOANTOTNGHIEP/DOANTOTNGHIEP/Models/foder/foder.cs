using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DOANTOTNGHIEP.Models.foder
{
    public class foder
    {
        public static void CreateFolder(string strPath)
        {
            
                if (Directory.Exists(strPath) == false)
                {
                    Directory.CreateDirectory(strPath);
                }
            
          
        }
    }
}