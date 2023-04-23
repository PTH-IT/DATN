using Spire.Pdf;
using Spire.Pdf.Exporting.XPS.Schema.Mc;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace DOANTOTNGHIEP.Models.database
{
    public class library
    {
       public static async Task<Library> SaveLibrary( HttpPostedFileBase file)
        {
            var location = await DOANTOTNGHIEP.Models.database.baitap.SaveFileLibrary(file);
            if (location == null)
            {
                return null;

            }
            DB db =new DB();
            Library library = new Library();
            library.Name = file.FileName;
            library.Location = location.Location;
            library.NgayThem = DateTime.Now;
            library.NgayUpdate = library.NgayThem;
            library.Noidung = location.Noidung;
            db.Libraries.Add(library);
            return library;
        }
        public static void SaveDocument(string tieude, string tendangnhap, string malop, string location, long id)
        {

            DB db =new DB();
            document documentpdf = new document();
          
            documentpdf.Ten = tieude;
            documentpdf.Nguoisohuu = tendangnhap;
            documentpdf.LuotTaiXuong = 0;
            documentpdf.Luotxem = 0;
            documentpdf.Image = getimagepdf(location, malop,tendangnhap);
            documentpdf.MaLop = Convert.ToInt64(malop);
            documentpdf.IDLibrary = id;
            db.documents.Add(documentpdf);
            db.SaveChanges();

        }
        public static string getimagepdf(string filepdf, string malop, string TenDangNhap)
        {

            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(HostingEnvironment.MapPath(filepdf));
            Image bmp = doc.SaveAsImage(0);
            Image emf = doc.SaveAsImage(0, Spire.Pdf.Graphics.PdfImageType.Metafile);
            Image zoomImg = new Bitmap((int)(emf.Size.Width * 2), (int)(emf.Size.Height * 2));
            using (Graphics g = Graphics.FromImage(zoomImg))
            {

                g.ScaleTransform(2.0f, 2.0f);
                g.DrawImage(emf, new Rectangle(new Point(0, 0), emf.Size), new Rectangle(new Point(0, 0), emf.Size), GraphicsUnit.Pixel);
            }
            string extension = DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            string path = HostingEnvironment.MapPath("~/Content/document/" + Models.crypt.Encrypt.encryptfoder(malop).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + Models.crypt.Encrypt.encryptfoder(TenDangNhap).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/") + extension;
            emf.Save(path, ImageFormat.Png);
            return "/Content/document/" + Models.crypt.Encrypt.encryptfoder(malop).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + Models.crypt.Encrypt.encryptfoder(TenDangNhap).Replace("+", "").Replace("=", "").Replace("-", "").Replace("_", "") + "/" + extension;

        }
    }
}