using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOANTOTNGHIEP.Models.database
{
    public class tailieu
    {

        public static List<string> getkeyword(List<string>  file)
        {
            List<string> keyword =new List<string>();
            
            foreach (string s in file)
            {
                var key = s.Split(' ');

                foreach (string v in key)
                {
                    if (comparekeywordwithfile(v.Replace(" ","").Replace(".", "").Replace(",", ""), file))
                    {
                        keyword.Add(v);
                    }
                }


            }

            return keyword;
        }
        public static bool comparekeywordwithfile(string s,  List<string> files)
        {
            foreach (string v in files)
            {
                if (!s.ToLower().Contains(v.ToLower()))
                {
                    return false;

                }

            }
                return true;
        }
   
    

    }


}