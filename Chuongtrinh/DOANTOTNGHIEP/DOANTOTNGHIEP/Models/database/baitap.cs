using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using DOANTOTNGHIEP.Models.api;

namespace DOANTOTNGHIEP.Models.database
{
    public class baitap
    {
        public static BaiTapTN AddBaitapTN(long MaBaiTap , string TenDangNhap)
        {
            DB db = new DB();
            BaiTapTN btn = new BaiTapTN();
            btn.MaBaiTap = MaBaiTap;
            btn.NguoiNop = TenDangNhap;
            btn.NgayNop = DateTime.Now;
            db.BaiTapTNs.Add(btn);
            db.SaveChanges();
            return btn;
        }
        public static BaiTapTL AddBaitapTL(long MaBaiTap, string TenDangNhap)
        {

            DB db = new DB();
            BaiTapTL btl = new BaiTapTL();
            btl.MaBaiTap = MaBaiTap;
            btl.NguoiNop = TenDangNhap;
            btl.NgayNop = DateTime.Now;
            db.BaiTapTLs.Add(btl);
            db.SaveChanges();
            return btl;
        }
        public static async  Task<upload> SaveFileLibrary(HttpPostedFileBase file)
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

                // Check if the call was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string result = await response.Content.ReadAsStringAsync();
                var serializer = new JavaScriptSerializer();
                var responseObj = serializer.Deserialize<dynamic>(result);
                upload responseupload = new upload();
                responseupload.Location = responseObj["Location"];
                responseupload.Noidung = responseObj["Data"];
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