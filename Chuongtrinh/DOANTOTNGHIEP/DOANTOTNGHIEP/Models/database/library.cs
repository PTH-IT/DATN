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
using System.Data.Entity.Migrations;
using DOANTOTNGHIEP.Models.api;
using System.IO;
using System.Net.Http;
using System.Web.Script.Serialization;

namespace DOANTOTNGHIEP.Models.database
{
    public class library
    {
       public static async Task<Library> SaveLibrary( HttpPostedFileBase file)
        {
            var location = await SaveFileLibrary(file);
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
            library.Image = location.Image;
            db.Libraries.Add(library);
            db.SaveChanges();
            return library;
        }
       public static async Task<Library> UpdateLibrary(HttpPostedFileBase file , Library inputLibrary)
        {
            var location = await SaveFileLibrary(file);
            if (location == null)
            {
                return null;

            }
            DB db = new DB();
            var library = inputLibrary;
            library.Name = file.FileName;
            library.Location = location.Location;
            library.NgayUpdate = DateTime.Now;
            library.Noidung = location.Noidung;
            db.Libraries.AddOrUpdate(library);
            db.SaveChanges();
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
            documentpdf.MaLop = Convert.ToInt64(malop);
            documentpdf.IDLibrary = id;
            db.documents.Add(documentpdf);
            db.SaveChanges();

        }
        public static string getimagepdf(string filepdf)
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
            string path = HostingEnvironment.MapPath("~/Content/document/" + extension);
            emf.Save(path, ImageFormat.Png);
            return "/Content/document/" + extension;

        }
        public static async Task<upload> SaveFileLibrary(HttpPostedFileBase file)
        {

            HttpClient httpClient = new HttpClient();

            // Set the base address of the API
            httpClient.BaseAddress = new Uri("https://example.com/api/");

            // Create a new multipart form data content
            var content = new MultipartFormDataContent();

            // Add the file to the form data content
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target); // generates problem in this line
            byte[] data = target.ToArray();
            var fileContent = new ByteArrayContent(data);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
            content.Add(fileContent, "file", file.FileName);

            // Call the API and get the response
            HttpResponseMessage response = await httpClient.PostAsync("http://localhost:1909/api/upload", content);

            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                string result = await response.Content.ReadAsStringAsync();
                var serializer = new JavaScriptSerializer();
                var responseObj = serializer.Deserialize<dynamic>(result);
                upload responseupload = new upload();
                responseupload.Location = responseObj["Location"];
                responseupload.Noidung = responseObj["Data"];
                responseupload.Image = getimagepdf(responseObj["Location"]);
                return responseupload;
            }
            else
            {
                Console.WriteLine($"Error calling API: {response.StatusCode}");
            }
            return null;

        }
    }
}