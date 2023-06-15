using DOANTOTNGHIEP.Models.api;
using Spire.Pdf.Exporting.XPS.Schema.Mc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;

namespace DOANTOTNGHIEP.Models.database
{
    public class daovan
    {

        static string host = "http://localhost:1909";
        public static async Task<bool> Apikiemtradaovanbaitap(string mabaitap, string malop)
        {
            HttpClient httpClient = new HttpClient();

            // Set the base address of the API
            httpClient.BaseAddress = new Uri("https://example.com/api/");

            // Create a new multipart form data content
            var content = new MultipartFormDataContent();

            // Add the file to the form data content
            

            // Call the API and get the response
            HttpResponseMessage response = await httpClient.PostAsync(host + "/api/baitap?mabaitap=" + mabaitap + "&malop" + malop, content);

            if (response.IsSuccessStatusCode)
            {
                
                return true;
            }
            else
            {
                Console.WriteLine($"Error calling API: {response.StatusCode}");
            }
            return false;
        }
        public static async Task<bool> Apikiemtradaovanlophoc(string mabaitap, string malop)
        {
            HttpClient httpClient = new HttpClient();

            // Set the base address of the API
            httpClient.BaseAddress = new Uri("https://example.com/api/");

            // Create a new multipart form data content
            var content = new MultipartFormDataContent();

            // Add the file to the form data content


            // Call the API and get the response
            HttpResponseMessage response = await httpClient.PostAsync(host + "/api/lophoc?mabaitap=" + mabaitap + "&malop" + malop, content);

            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {

                return true;
            }
            else
            {
                Console.WriteLine($"Error calling API: {response.StatusCode}");
            }
            return false;
        }

        public static async Task<bool> Apikiemtradaovanall(string mabaitap)
        {
            HttpClient httpClient = new HttpClient();

            // Set the base address of the API
            httpClient.BaseAddress = new Uri("https://example.com/api/");

            // Create a new multipart form data content
            var content = new MultipartFormDataContent();

            // Add the file to the form data content


            // Call the API and get the response
            HttpResponseMessage response = await httpClient.PostAsync(host + "/api/all?mabaitap=" + mabaitap , content);

            if (response.IsSuccessStatusCode)
            {

                return true;
            }
            else
            {
                Console.WriteLine($"Error calling API: {response.StatusCode}");
            }
            return false;
        }



        public static bool kiemtradaovanbaitap(string mabaitap, string malop)
        {
            DB db = new DB();
            var thongtinbaitap = db.TTBaiTapTLs.Where(x => x.BaiTapTL.MaBaiTap.ToString().Equals(mabaitap) ).ToList().OrderBy(y => y.Library.NgayUpdate).ToList();
            if (thongtinbaitap.Count > 0)
            {
                
                

                foreach (var thongtincankiemtra in thongtinbaitap)
                {
                    List<DOANTOTNGHIEP.Models.Modelcreate.daovan> checkdaovan = new List<DOANTOTNGHIEP.Models.Modelcreate.daovan>();
                    foreach (var bailam in thongtinbaitap)
                    {
                        if (thongtincankiemtra.Library.NgayUpdate > bailam.Library.NgayUpdate && thongtincankiemtra.NguoiNop != bailam.NguoiNop)
                        {

                            var kiemtradaovan = DOANTOTNGHIEP.Models.database.daovan.comparetwofilepdf(thongtincankiemtra, bailam.Library.Noidung,bailam.Library.Location);
                            checkdaovan.Add(kiemtradaovan);

                        }
                    }
                    List<List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan>> caudaovan = new List<List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan>>();
                    caudaovan.AddRange(checkdaovan.Select(x => x.sentence));
                    List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan> resultcaudaovan = new List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan>();

                    if (caudaovan.Count > 0)
                    {
                        for (int j = 0; j < caudaovan.ToArray()[0].Count; j++)
                        {
                            List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan> resultcaulist = new List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan>();
                            for (int i = 0; i < caudaovan.Count; i++)
                            {
                                resultcaulist.Add(caudaovan.ToArray()[i][j]);
                            }
                            resultcaudaovan.AddRange(resultcaulist.Select(x => x).OrderByDescending(y => y.per).Take(1));

                        }
                    }

                    if (resultcaudaovan.Count > 0)
                    {
                        Plagiarism plagiarism1 = new Plagiarism();
                        plagiarism1.Mafile = thongtincankiemtra.ID;
                        plagiarism1.Percents = resultcaudaovan.Select(x => x.per).ToArray().Sum() / resultcaudaovan.Count;
                        plagiarism1.Location = DOANTOTNGHIEP.Models.exportfile.exportfile.updatepdfdaovan(thongtincankiemtra.Library.Location, "/Content/daovan/" + thongtincankiemtra.Library.Name.Replace(".pdf", "-baitap.pdf"), resultcaudaovan, "bài Tập");
                        plagiarism1.Loaikiemtra = "Baitap";
                        db.Plagiarism.Add(plagiarism1);
                        db.SaveChanges();


                    }
                    if (checkdaovan.Count==0)
                    {

                        Plagiarism plagiarism1 = new Plagiarism();
                        plagiarism1.Mafile = thongtincankiemtra.ID;
                        plagiarism1.Percents = 0;
                        plagiarism1.Location = DOANTOTNGHIEP.Models.exportfile.exportfile.updatepdfdaovan(thongtincankiemtra.Library.Location, "/Content/daovan/" + thongtincankiemtra.Library.Name.Replace(".pdf", "-baitap.pdf"), resultcaudaovan, "bài Tập");
                        plagiarism1.Loaikiemtra = "Baitap";
                        db.Plagiarism.Add(plagiarism1);
                        db.SaveChanges();
                    }
                    /*tttl.Isplagiarism = true;*/
                    db.SaveChanges();

                }
                return true;


            }

            return false;

        }

        public static bool kiemtradaovanlophoc(string mabaitap, string malop)
        {
            DB db = new DB();
            var thongtinbaitap = db.TTBaiTapTLs.Where(x => x.BaiTapTL.MaBaiTap.ToString().Equals(mabaitap)).ToList().OrderBy(y => y.Library.NgayUpdate).ToList();
            var thongtinbaitaptuluanlophoc = db.TTBaiTapTLs.Where(x => x.BaiTapTL.BaiTap.LopHoc.ID.ToString().Equals(malop)).ToList().OrderBy(y => y.Library.NgayUpdate).ToList();
            var thongtintailieulophoc = db.documents.Where(x => x.MaLop.ToString().Equals(malop)).ToList().OrderBy(y => y.Library.NgayUpdate).ToList();
            if (thongtinbaitap.Count > 0)
            {


                List<DOANTOTNGHIEP.Models.Modelcreate.TaiLieuCheckDaoVan> tailieucheck = new List<Modelcreate.TaiLieuCheckDaoVan>();
                foreach (var tailieu in thongtinbaitaptuluanlophoc)
                {
                    DOANTOTNGHIEP.Models.Modelcreate.TaiLieuCheckDaoVan tl = new Modelcreate.TaiLieuCheckDaoVan();
                    tl.Tenfile = tailieu.Library.Name;
                    tl.Thoigian = tailieu.Library.NgayUpdate;
                    tl.NguoiSoHuu = tailieu.NguoiNop;
                    tl.NoiLuu = tailieu.Library.Location;
                    tl.Datafile = tailieu.Library.Noidung;
                    tailieucheck.Add(tl);
                }
                foreach (var tailieu in thongtintailieulophoc)
                {
                    DOANTOTNGHIEP.Models.Modelcreate.TaiLieuCheckDaoVan tl = new Modelcreate.TaiLieuCheckDaoVan();
                    tl.Tenfile = tailieu.Library.Name;
                    tl.Thoigian = tailieu.Library.NgayUpdate;
                    tl.NguoiSoHuu = tailieu.Library.NguoiAdd;
                    tl.NoiLuu = tailieu.Library.Location;
                    tl.Datafile = tailieu.Library.Noidung;
                    tailieucheck.Add(tl);
                }
                foreach (var thongtincankiemtra in thongtinbaitap)
                {
                    List<DOANTOTNGHIEP.Models.Modelcreate.daovan> checkdaovan = new List<DOANTOTNGHIEP.Models.Modelcreate.daovan>();
                    foreach (var bailam in tailieucheck.OrderBy(x => x.Thoigian))
                    {
                        if (thongtincankiemtra.Library.NgayUpdate > bailam.Thoigian && thongtincankiemtra.NguoiNop != bailam.NguoiSoHuu)
                        {

                            var kiemtradaovan = DOANTOTNGHIEP.Models.database.daovan.comparetwofilepdf(thongtincankiemtra, bailam.Datafile, bailam.NoiLuu);
                            checkdaovan.Add(kiemtradaovan);

                        }
                    }

                  


                    List<List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan>> caudaovan = new List<List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan>>();
                    caudaovan.AddRange(checkdaovan.Select(x => x.sentence));
                    List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan> resultcaudaovan = new List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan>();

                    if (caudaovan.Count > 0)
                    {
                        for (int j = 0; j < caudaovan.ToArray()[0].Count; j++)
                        {
                            List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan> resultcaulist = new List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan>();
                            for (int i = 0; i < caudaovan.Count; i++)
                            {
                                resultcaulist.Add(caudaovan.ToArray()[i][j]);
                            }
                            resultcaudaovan.AddRange(resultcaulist.Select(x => x).OrderByDescending(y => y.per).Take(1));

                        }
                    }
                    if (resultcaudaovan.Count > 0)
                    {
                        Plagiarism plagiarism1 = new Plagiarism();
                        plagiarism1.Mafile = thongtincankiemtra.ID;
                        plagiarism1.Percents = resultcaudaovan.Select(x => x.per).ToArray().Sum() / resultcaudaovan.Count;
                        plagiarism1.Location = DOANTOTNGHIEP.Models.exportfile.exportfile.updatepdfdaovan(thongtincankiemtra.Library.Location, "/Content/daovan/" + thongtincankiemtra.Library.Name.Replace(".pdf", "-lophoc.pdf"), resultcaudaovan , "lớp học");
                        plagiarism1.Loaikiemtra = "Lophoc";
                        db.Plagiarism.Add(plagiarism1);
                        db.SaveChanges();


                    }
                    if (checkdaovan.Count == 0)
                    {

                        Plagiarism plagiarism1 = new Plagiarism();
                        plagiarism1.Mafile = thongtincankiemtra.ID;
                        plagiarism1.Percents = 0;
                        plagiarism1.Location = DOANTOTNGHIEP.Models.exportfile.exportfile.updatepdfdaovan(thongtincankiemtra.Library.Location, "/Content/daovan/" + thongtincankiemtra.Library.Name.Replace(".pdf", "-lophoc.pdf"), resultcaudaovan, "lớp học");
                        plagiarism1.Loaikiemtra = "Lophoc";
                        db.Plagiarism.Add(plagiarism1);
                        db.SaveChanges();
                    }
                    /*tttl.Isplagiarism = true;*/
                    db.SaveChanges();

                }
                return true;


            }

            return false;

        }
        public static bool kiemtradaovanall(string mabaitap)
        {
            DB db = new DB();
            var thongtinbaitap = db.TTBaiTapTLs.Where(x => x.BaiTapTL.MaBaiTap.ToString().Equals(mabaitap)).ToList().OrderBy(y => y.Library.NgayUpdate).ToList();
            var thongtinbaitaptuluanlophoc = db.TTBaiTapTLs.Select(x => x).ToList().OrderBy(y => y.Library.NgayUpdate).ToList();
            var thongtintailieulophoc = db.documents.Select(x => x).ToList().OrderBy(y => y.Library.NgayUpdate).ToList();
            if (thongtinbaitap.Count > 0)
            {


                List<DOANTOTNGHIEP.Models.Modelcreate.TaiLieuCheckDaoVan> tailieucheck = new List<Modelcreate.TaiLieuCheckDaoVan>();
                foreach (var tailieu in thongtinbaitaptuluanlophoc)
                {
                    DOANTOTNGHIEP.Models.Modelcreate.TaiLieuCheckDaoVan tl = new Modelcreate.TaiLieuCheckDaoVan();
                    tl.Tenfile = tailieu.Library.Name;
                    tl.Thoigian = tailieu.Library.NgayUpdate;
                    tl.NguoiSoHuu = tailieu.Library.NguoiAdd;
                    tl.NoiLuu = tailieu.Library.Location;
                    tl.Datafile = tailieu.Library.Noidung;
                    tailieucheck.Add(tl);
                }
                foreach (var tailieu in thongtintailieulophoc)
                {
                    DOANTOTNGHIEP.Models.Modelcreate.TaiLieuCheckDaoVan tl = new Modelcreate.TaiLieuCheckDaoVan();
                    tl.Tenfile = tailieu.Library.Name;
                    tl.Thoigian = tailieu.Library.NgayUpdate;
                    tl.NguoiSoHuu = tailieu.Library.NguoiAdd;
                    tl.NoiLuu = tailieu.Library.Location;
                    tl.Datafile = tailieu.Library.Noidung;
                    tailieucheck.Add(tl);
                }

                foreach (var thongtincankiemtra in thongtinbaitap)
                {
                    List<DOANTOTNGHIEP.Models.Modelcreate.daovan> checkdaovan = new List<DOANTOTNGHIEP.Models.Modelcreate.daovan>();
                    foreach (var bailam in tailieucheck.OrderBy(x => x.Thoigian))
                    {
                        if (thongtincankiemtra.Library.NgayUpdate > bailam.Thoigian && thongtincankiemtra.NguoiNop != bailam.NguoiSoHuu)
                        {

                            var kiemtradaovan = DOANTOTNGHIEP.Models.database.daovan.comparetwofilepdf(thongtincankiemtra, bailam.Datafile, bailam.NoiLuu);
                            checkdaovan.Add(kiemtradaovan);

                        }
                    }



                    List<List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan>> caudaovan = new List<List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan>>();
                    caudaovan.AddRange(checkdaovan.Select(x => x.sentence));
                    List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan> resultcaudaovan = new List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan>();

                    if (caudaovan.Count > 0)
                    {
                        for (int j = 0; j < caudaovan.ToArray()[0].Count; j++)
                        {
                            List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan> resultcaulist = new List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan>();
                            for (int i = 0; i < caudaovan.Count; i++)
                            {
                                resultcaulist.Add(caudaovan.ToArray()[i][j]);
                            }
                            resultcaudaovan.AddRange(resultcaulist.Select(x => x).OrderByDescending(y => y.per).Take(1));

                        }
                    }
                    if (resultcaudaovan.Count > 0)
                    {
                        var xx = resultcaudaovan.Select(x => x.per).ToArray();
                        Plagiarism plagiarism1 = new Plagiarism();
                        plagiarism1.Mafile = thongtincankiemtra.ID;
                        plagiarism1.Percents = resultcaudaovan.Select(x => x.per).ToArray().Sum() / resultcaudaovan.Count;
                        plagiarism1.Location = DOANTOTNGHIEP.Models.exportfile.exportfile.updatepdfdaovan(thongtincankiemtra.Library.Location, "/Content/daovan/" + thongtincankiemtra.Library.Name.Replace(".pdf", "-all.pdf"), resultcaudaovan, "toàn bộ chương trình");
                        plagiarism1.Loaikiemtra = "all";
                        db.Plagiarism.Add(plagiarism1);
                        db.SaveChanges();


                    }
                    if (checkdaovan.Count == 0)
                    {

                        Plagiarism plagiarism1 = new Plagiarism();
                        plagiarism1.Mafile = thongtincankiemtra.ID;
                        plagiarism1.Percents = 0;
                        plagiarism1.Location = DOANTOTNGHIEP.Models.exportfile.exportfile.updatepdfdaovan(thongtincankiemtra.Library.Location, "/Content/daovan/" + thongtincankiemtra.Library.Name.Replace(".pdf", "-all.pdf"), resultcaudaovan , "toàn bộ chương trình");
                        plagiarism1.Loaikiemtra = "all";
                        db.Plagiarism.Add(plagiarism1);
                        db.SaveChanges();
                    }
                    /*tttl.Isplagiarism = true;*/
                    db.SaveChanges();

                }
                return true;


            }

            return false;

        }

        public static string[] getcau(string s)
        {
            List<string> list = new List<string>();
            string a = "";
            for (var i =0; i< s.Length; i++)
            {
                a = a + s[i];
                if (i + 1 < s.Length && i - 1 > 0)
                {
                    if (s[i] == '.' && s[i + 1] != '.' && s[i - 1] != '.')
                    {
                        if (a.Replace("\r", "").Replace("\n", "").Replace(" ", "").Length > 0)
                        {
                            list.Add(a);
                            a = "";
                        }
                    }
                    else if (i == s.Length - 1)
                    {
                        if (a.Replace("\r", "").Replace("\n", "").Replace(" ", "").Length > 0)
                        {
                            list.Add(a);
                            a = "";
                        }
                    }

                }else
                {
                    if (i == s.Length - 1)
                    {
                        if (a.Replace("\r", "").Replace("\n", "").Replace(" ", "").Length > 0)
                        {
                            list.Add(a);
                            a = "";
                        }
                        
                    }
                }
               
                
            }



            return list.ToArray();
        }



        public static DOANTOTNGHIEP.Models.Modelcreate.daovan comparetwofilepdf(TTBaiTapTL file1, string data2 , string link2)
        {
            DOANTOTNGHIEP.Models.Modelcreate.daovan listdv =new DOANTOTNGHIEP.Models.Modelcreate.daovan();
            listdv.sentence = new List<Modelcreate.detaildaovan>();

            var datafile1 = getcau(file1.Library.Noidung);
            var datafile2 = getcau(data2);
            List<float> per = new List<float>();
            foreach (var compare1 in datafile1)
            {
                DOANTOTNGHIEP.Models.Modelcreate.detaildaovan dv = new DOANTOTNGHIEP.Models.Modelcreate.detaildaovan();
                List<float> per1 = new List<float>();
                List<List<string>> checklist = new List<List<string>>();
                foreach (var compare2 in datafile2)
                {
                    var check = comparetsentence(compare1.TrimStart(' ').TrimEnd(' '), compare2.TrimStart(' ').TrimEnd(' '));
                    float percompare = check.Item2;
                    checklist.Add(check.Item1);
                    per1.Add(percompare);
                    if (percompare >= 100)
                    {
                        break;
                    }


                }
                var max = per1.Max();
                var indexmax= per1.IndexOf(max);
                var listsentence = checklist.GetRange(indexmax, 1).ToArray()[0];
                dv.per = max;
                dv.locationfilecompare = link2;
                dv.Keywor1 = compare1;
                dv.Keywor2 = datafile2[indexmax];
                dv.sentenceKeyword = listsentence;
                listdv.sentence.Add(dv);
                per.Add(max);

            }
            listdv.file1 = file1;
            listdv.per = per.Sum() / datafile1.Length;

            return listdv;
        }

        public static (List<string>,float) comparetsentence(string keyword1, string keyword2)
        {
            float percent = 0;
            List<string> sentence = new List<string>();
            if (keyword2.ToLower().Contains(keyword1.ToLower()))
            {
                percent = 100;
                sentence.Add(keyword1);
            }
            else
            {
                var check = comparetsentence1(keyword1.ToLower(), keyword2.ToLower());
                sentence = check.Item1;
                percent = check.Item2;
            }

            return (sentence,percent);

        }
        //so sánh loại bỏ từ 
        public static (List<string> ,float) comparetsentence1(string keyword1, string keyword2)
        {
            List<string> keyalike = new List<string>();
            List<float> pers = new List<float>();
            List<string> alike = new List<string>();
            List<int> indexalike = new List<int>();
            float percent = 0;
            if (keyword1.Replace("\r", " ").Replace("\n", " ").Replace("  ", " ").IndexOf(" ") > 1)
            {
                int indexs1 = 0;
                foreach (string s1 in keyword1.Replace("\r", " ").Replace("\n", " ").Replace("  ", " ").Split(' '))
                {

                    foreach (string s2 in keyword2.Replace("\r", " ").Replace("\n", " ").Replace("  ", " ").Split(' '))
                    {
                        if (s1.Equals(s2))
                        {
                            alike.Add(s1);
                            indexalike.Add(indexs1);
                            break;

                        }
                    }
                    indexs1 += s1.Length + 1;

                }
                
                string s = "";
                for (var i = 0; i < alike.Count; i++)
                {
                    if (i > 0)
                    {
                        if (indexalike[i - 1] + alike[i - 1].Length + 1 == indexalike[i])
                        {


                            if (i == alike.Count - 1 && (s + " " + alike[i]).Split(' ').Length >= 2 && keyword2.Contains(s + " " + alike[i]))
                            {
                                s = s + " " + alike[i];
                                keyalike.Add(s);
                                s = "";
                            }
                            else if (s.Split(' ').Length >= 2 && keyword2.Contains(s) && !keyword2.Contains(s + " " + alike[i]))
                            {
                                keyalike.Add(s);
                                s = alike[i];
                            }
                            else if (keyword2.Contains(s + " " + alike[i]))
                            {
                                if (s.Length == 0)
                                {
                                    s = alike[i];
                                }
                                else
                                {
                                    s = s + " " + alike[i];
                                }

                            }
                            else
                            {
                                s = alike[i];

                            }
                        }
                        else
                        {
                            if (s.Split(' ').Length >= 2 && keyword2.Contains(s))
                            {
                                keyalike.Add(s);


                            }
                            s = alike[i];

                        }

                    }
                    else
                    {
                        s = alike[i];
                    }
                }
                int numberalike = 0;
                foreach (var i in keyalike)
                {
                    numberalike += i.Split(' ').Length;
                }
                percent = Convert.ToSingle(Convert.ToSingle(numberalike * 100) / keyword1.Split(' ').Length);

            }


            return (keyalike,percent);

        }


    }
}