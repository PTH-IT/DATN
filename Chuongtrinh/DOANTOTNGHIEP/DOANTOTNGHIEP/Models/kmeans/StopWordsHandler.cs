using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOANTOTNGHIEP.Models.kmeans
{
    public class StopWordsHandler
    {
        //you can defined other stop word list here
        public static string[] stopWordsList = new string[] { "The" };

        public static Boolean IsStotpWord(string word)
        {
            if (stopWordsList.Contains(word))
                return true;
            else
                return false;
        }
    }
}