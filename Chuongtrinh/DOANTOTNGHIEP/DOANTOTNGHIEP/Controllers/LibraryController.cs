using HtmlAgilityPack;
/*using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using DOANTOTNGHIEP.Models;
using System.IO;
using DOANTOTNGHIEP.Models.GetData;
using Spire.Pdf;
using System.Drawing;
using System.Drawing.Imaging;
using DOANTOTNGHIEP.Modelcreate;
using System.Threading;
using OpenQA.Selenium.DevTools.V101.Debugger;
using System.Threading.Tasks;

namespace DOANTOTNGHIEP.Controllers
{
    public class LibraryController : Controller
    {
        public (DOANTOTNGHIEP.Models.TaiKhoan, DOANTOTNGHIEP.Models.ThanhVienLop) checkcookieuser(string malop)
        {
            DB db = new DB();
            var user = Request.Cookies["user"];
            if (user != null && user["TenDangNhap"].ToString().Length > 0 && user["Matkhau"].ToString().Length > 0)
            {
                var tendangnhap = Models.crypt.Encrypt.Decryptuser(user["TenDangNhap"].ToString());
                var matkhau = Models.crypt.Encrypt.Decryptuser(user["Matkhau"].ToString());
                var thanhvienlop = db.ThanhVienLops.SingleOrDefault(x => x.ID.ToString().Equals(malop) && x.Mathanhvien.Equals(tendangnhap));
                var TK = db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(tendangnhap) && x.MatKhau.Equals(matkhau));
                return (TK, thanhvienlop);

            }
            return (null, null);
        }
        public (bool, string, string, DOANTOTNGHIEP.Models.TaiKhoan, DOANTOTNGHIEP.Models.ThanhVienLop) checkCookie(string malop)
        {
            if (malop == null)
            {
                return (true, "Index", "TrangChu", null, null);
            }
            var cookie = checkcookieuser(malop);
            var user = cookie.Item1;
            var thanhvienlop = cookie.Item2;
            if (user == null)
            {
                return (true, "Login", "Login", user, thanhvienlop);
            }
            else if (user.ChucVu.ToLower().Contains("Admin".ToLower()))
            {
                return (true, "Index", "Admin", user, thanhvienlop);
            }
            else if (thanhvienlop == null)
            {
                return (true, "Index", "TrangChu", user, thanhvienlop);
            }
            return (false, "", "", user, thanhvienlop);
        }

        public static List<DOANTOTNGHIEP.Modelcreate.Tailieu> getfiletkb(string noidung)
        {
            List<DOANTOTNGHIEP.Modelcreate.Tailieu> tailieu = new List<Modelcreate.Tailieu>();
            /*ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            var options = new ChromeOptions();
            options.AddArgument("no-sandbox");
            // Chạy ngầm không pop up trình duyệt ra ngoài 
            options.AddArgument("headless");
            options.AddArgument("--window-position=-32000,-32000");
            options.AddArgument("--incognito");
            IWebDriver webDriver = new ChromeDriver(HostingEnvironment.MapPath("~/Content/chromdriver"), options);
            webDriver.Url = "https://kupdf.net/";
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            if(noidung.Length != 0)
            {
            var nd = webDriver.FindElement(By.XPath("//*[@id='keyword']"));
            nd.SendKeys(noidung);
            var bt = webDriver.FindElement(By.XPath("/html/body/div[4]/div/form/div/button"));
            bt.Click();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            }*/


            int trang = 1;

            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = true,
                OverrideEncoding = Encoding.UTF8
            };
            while (true)
            {
                string search = "https://kupdf.net";
                if (noidung.Length > 0)
                {
                    search = search + "/search/" + noidung + "/" + trang;
                }

                trang++;



                /*HtmlDocument document = htmlWeb.Load(webDriver.Url);*/
                HtmlDocument document = htmlWeb.Load(search);


                if (document.DocumentNode.SelectNodes("//div/div[@class='card']") == null) break;
                var threadItems = document.DocumentNode.SelectNodes("//div/div[@class='card']").ToList();
                foreach (var s in threadItems)
                {
                    DOANTOTNGHIEP.Modelcreate.Tailieu tl = new Modelcreate.Tailieu();
                    var c = s.SelectSingleNode(".//div[@class='card-image']/a/img");
                    c.Attributes["class"].Remove();/*
                c.Attributes.Append("background-size");
                c.SetAttributeValue("background-size", "cover");*/
                    c.Attributes.Append("style");
                    c.SetAttributeValue("style", "max-width:250;width:auto;height:auto;max-height:100%");
                    tl.anh = c.OuterHtml;
                    var a = s.SelectSingleNode(".//div[@class='card-header']/h3[@class='card-title']/a");
                    tl.ten = a.InnerHtml;
                    var b = s.SelectSingleNode(".//div[@class='card-footer']/a");
                    b.Attributes.Append("target");
                    b.SetAttributeValue("target", "_blank");
                    tl.duongdan = b.OuterHtml;
                    tailieu.Add(tl);
                }
                if (trang == 10 || noidung.Length == 0)
                {
                    break;
                }



            }




            return tailieu;
        }
        // GET: Library


        public void SaveKeyword(string noidung)
        {
            Thread t1 = new Thread(() =>
            {
                if (noidung.Replace("  ", "").Length > 0)
                {
                    DB db = new DB();
                    var checkkey = db.KeywordLibraries.Where(x => x.Keyword.ToLower().Equals(noidung.ToLower())).ToList();
                    if (checkkey.Count <= 0)
                    {
                        KeywordLibrary keyword = new KeywordLibrary();
                        keyword.Keyword = noidung.TrimEnd(' ');
                        db.KeywordLibraries.Add(keyword);
                        db.SaveChanges();

                    }

                }
            });


            t1.Start();
        }
        [HttpGet]
        public System.Web.Mvc.JsonResult getkeyword(string query)
        {
            DB db = new DB();
            var data = db.KeywordLibraries.Select(x => x.Keyword).ToArray();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index(string id)
        {
            ViewBag.malop = id;
            DB db = new DB();
            ViewBag.lophoc = db.LopHocs.SingleOrDefault(x => x.ID.ToString().Equals(id));
            var checkcookie = checkCookie(id);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            if (id == null)
            {
                return RedirectToAction("Index", "TrangChu");

            }
            var thanhvienlop = db.ThanhVienLops.SingleOrDefault(x => x.ID.ToString().Equals(id) && x.Mathanhvien.Equals(user.TenDangNhap));
            ViewBag.chucvu = thanhvienlop.ChucVu;
            return View();
        }
        public ActionResult Searchlibrary(string malop)
        {
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            SaveKeyword(Request.Form["noidungcantin"].ToString());
            var cauhoi = Request.Form["noidungcantin"].ToString().ToLower();

            List<document> documents = new List<document>();
            if (cauhoi.Replace("  ", "").Length == 0)
            {
                documents = db.documents.Where(x => x.MaLop.ToString().Equals(malop)).ToList();
            }
            List<Tailieu> tailieu = new List<Tailieu>();
            foreach (var filedoccument in documents)
            {
                Tailieu tl = new Tailieu();
                tl.ten = filedoccument.Ten;
                tl.anh = filedoccument.Library.Image;
                tl.duongdan = filedoccument.Library.Location;
                tailieu.Add(tl);

            }
            List<document> docs = new List<document>();
            List<string> keyseach = new List<string>();
            if (cauhoi.Replace("  ", "").Length > 0)
            {
                for (var i = 0; i < cauhoi.Replace("  ", " ").TrimEnd(' ').TrimStart(' ').Split(' ').ToList().Count; i++)
                {
                    var s = cauhoi.Replace("  ", " ").TrimEnd(' ').TrimStart(' ').Split(' ').ToArray()[i];
                    for (var j = i + 1; j < cauhoi.Replace("  ", " ").TrimEnd(' ').TrimStart(' ').Split(' ').ToList().Count; j++)
                    {
                        var v = s + " " + cauhoi.Replace("  ", " ").TrimEnd(' ').TrimStart(' ').Split(' ').ToArray()[j];
                        keyseach.Add(v);
                        s = v;
                    }

                }
                foreach (var v in cauhoi.Replace("  ", " ").TrimEnd(' ').TrimStart(' ').Split(' ').ToList())
                {
                    keyseach.Add(v);
                }

            }
            foreach (var v in keyseach.OrderByDescending(x => x.TrimEnd(' ').TrimStart(' ').Split(' ').Length))
            {
                documents = db.documents.Where(x => x.MaLop.ToString().Equals(malop) && x.Library.Noidung.Replace("  ", " ").Contains(v)).ToList();
                docs.AddRange(documents);
            }
            foreach (var filedoccument in docs)
            {
                if (tailieu.Where(x => x.duongdan.Equals(filedoccument.Library.Location)).ToList().Count == 0)
                {
                    Tailieu tl = new Tailieu();
                    tl.ten = filedoccument.Ten;
                    tl.anh = filedoccument.Library.Image;
                    tl.duongdan = filedoccument.Library.Location;
                    tailieu.Add(tl);

                }
            }
            ViewData["s"] = tailieu;
            return PartialView();
        }
        [HttpPost]
        public async Task<ActionResult> Uploaddocument(HttpPostedFileBase filedocumentupload, string malop)
        {
            ViewBag.malop = malop;
            DB db = new DB();
            var checkcookie = checkCookie(malop);
            if (checkcookie.Item1)
            {
                return RedirectToAction(checkcookie.Item2, checkcookie.Item3);
            }
            var user = checkcookie.Item4;
            ViewBag.user = user;
            if (malop == null) return RedirectToAction("Index", "TrangChu");
            string returnImagePath = string.Empty;
            string fileName;
            string Extension;
            string imageName;
            string imageSavePath;
            string tieude = Request.Form["titledocument"];
            if (filedocumentupload.ContentLength > 0)
            {
                fileName = Path.GetFileNameWithoutExtension(filedocumentupload.FileName);
                Extension = Path.GetExtension(filedocumentupload.FileName);
                imageName = fileName + DateTime.Now.ToString("yyyyMMddHHmmss");
                /*imageSavePath = Server.MapPath("~/Content/document/"+ Models.crypt.Encrypt.encryptfoder(malop).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + Models.crypt.Encrypt.encryptfoder(user.TenDangNhap).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/") + imageName +Extension;
                filedocumentupload.SaveAs(imageSavePath);*/
                var library = await DOANTOTNGHIEP.Models.database.library.SaveLibrary(filedocumentupload);

                if (library == null)
                {
                    return RedirectToAction("Index", "Library", new { id = malop });

                }
                DOANTOTNGHIEP.Models.database.library.SaveDocument(tieude, user.TenDangNhap, malop, library.Location, library.ID );

                return RedirectToAction("Index", "Library", new { id = malop });
            }



            return RedirectToAction("Index", "Library", new { id = malop });


        }

    }
}