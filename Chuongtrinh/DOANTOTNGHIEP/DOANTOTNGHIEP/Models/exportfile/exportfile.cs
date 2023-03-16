using DOANTOTNGHIEP.Modelcreate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOANTOTNGHIEP.Models;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Fields.OMath;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Pdf.Tables;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Web.Hosting;
using Spire.Pdf.General.Find;
using Spire.Pdf.Texts;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Spire.Pdf.Annotations;
using Spire.Pdf.ColorSpace;
using Spire.Pdf.Actions;

namespace DOANTOTNGHIEP.Models.exportfile
{
    public class exportfile
    {
        public static string exceldsdiembaitap(List<DOANTOTNGHIEP.Models.Modelcreate.BaiTap> dsdiem)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "Họ tên";

            Sheet.Cells["B1"].Value = "Email";

            Sheet.Cells["C1"].Value = "Ngày nộp";

            Sheet.Cells["D1"].Value = "Ngày hết hạn";

            Sheet.Cells["E1"].Value = "Trạng thái";

            Sheet.Cells["F1"].Value = "Điểm";

            Sheet.Cells["A1:f1"].Style.Font.Color.SetColor(Color.White);
            Sheet.Cells["A1:f1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            Sheet.Cells["A1:f1"].Style.Fill.BackgroundColor.SetColor(Color.Black);
            Sheet.Cells["A1:f1"].Style.Font.Size = 16;

            if (dsdiem != null && dsdiem.Count > 0)
            {



                int row = 2;
                foreach (var item in dsdiem)
                {

                    Sheet.Cells[string.Format("A{0}", row)].Value = item.TaiKhoan.Ho + " " + item.TaiKhoan.Ten;
                    Sheet.Cells[string.Format("B{0}", row)].Value = item.TaiKhoan.Email;

                    string ngaynop = "";
                    string ngayketthuc = "";
                    if (item.NgayNop != null)
                    {
                        ngaynop = item.NgayNop.Value.ToString(string.Format("HH:mm:ss dd/MM/yyyy"));
                    }
                    if (item.BaiTaps.ThoiGianKetThuc != null)
                    {
                        ngayketthuc = item.BaiTaps.ThoiGianKetThuc.Value.ToString(string.Format("HH:mm:ss dd/MM/yyyy"));
                    }

                    Sheet.Cells[string.Format("C{0}", row)].Value = ngaynop;
                    Sheet.Cells[string.Format("D{0}", row)].Value = ngayketthuc;

                    string trangthai = "";
                    if (item.NgayNop == null)
                    {
                        trangthai = "Chưa Nộp";
                    }
                    else if (item.NgayNop != null)
                    {
                        if (item.BaiTaps.ThoiGianKetThuc >= item.NgayNop)
                        {
                            trangthai = "Đã nộp";
                        }
                        else if (item.BaiTaps.ThoiGianKetThuc < item.NgayNop)
                        {
                            trangthai = "Nộp muộn";
                        }
                    }
                    Sheet.Cells[string.Format("E{0}", row)].Value = trangthai;
                   

                    Sheet.Cells[string.Format("F{0}", row)].Value = item.Diem.ToString();


                    Sheet.Cells[string.Format("A{0}:F{0}", row)].Style.Font.Color.SetColor(Color.Black);
                    if (trangthai.Equals("Đã nộp"))
                    {
                        Sheet.Cells[string.Format("E{0}", row)].Style.Font.Color.SetColor(Color.Black);
                    }
                    else
                    {
                        Sheet.Cells[string.Format("E{0}", row)].Style.Font.Color.SetColor(Color.Red);

                    }
                    Sheet.Cells[string.Format("A{0}:F{0}", row)].Style.Font.Size = 16;
                    row++;

                }
                string path = HostingEnvironment.MapPath("~/Content/file/" + dsdiem[0].BaiTaps.MaLop.ToString() + "_" + dsdiem[0].BaiTaps.MaBaiTap.ToString() + ".xlsx");
                string path1 = "/Content/file/" + dsdiem[0].BaiTaps.MaLop.ToString() + "_" + dsdiem[0].BaiTaps.MaBaiTap.ToString() + ".xlsx";
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                Sheet.Cells["A:AZ"].AutoFitColumns();
                Ep.SaveAs(path);
                return path1;
            }
            return "";
            
        }
        public static string exceldsdiem(List<Diem> dsdiem, string malop)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();
            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "Họ tên";

            Sheet.Cells["B1"].Value = "Email";

            Sheet.Cells["C1"].Value = "Số lượng bài đã nộp";

            Sheet.Cells["D1"].Value = "Số lượng bài trễ";

            Sheet.Cells["E1"].Value = "Số lượng bài chưa nộp";

            Sheet.Cells["F1"].Value = "Số lượng bài đã chấm";

            Sheet.Cells["G1"].Value = "Điểm";


            Sheet.Cells["A1:G1"].Style.Font.Color.SetColor(Color.White);
            Sheet.Cells["A1:G1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            Sheet.Cells["A1:G1"].Style.Fill.BackgroundColor.SetColor(Color.Black);
            Sheet.Cells["A1:G1"].Style.Font.Size = 16;

            int row = 2;
            foreach (var item in dsdiem)
            {


                Sheet.Cells[string.Format("A{0}", row)].Value = item.Hoten;

                Sheet.Cells[string.Format("B{0}", row)].Value = item.email;

                Sheet.Cells[string.Format("C{0}", row)].Value = item.soluongbaitap.ToString();

                Sheet.Cells[string.Format("D{0}", row)].Value = item.soluongtre.ToString();
               

                Sheet.Cells[string.Format("E{0}", row)].Value = item.soluongchuanop.ToString();

                Sheet.Cells[string.Format("F{0}", row)].Value = item.soluongbaicodiem.ToString();

                Sheet.Cells[string.Format("G{0}", row)].Value = item.diem.ToString();


                Sheet.Cells[string.Format("A{0}:G{0}", row)].Style.Font.Color.SetColor(Color.Black);
                Sheet.Cells[string.Format("A{0}:G{0}", row)].Style.Font.Size = 16;
                row++;
            }
            string path = HostingEnvironment.MapPath("~/Content/file/" + malop + ".xlsx");
            string path1 = "/Content/file/" + malop + ".xlsx";
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            Sheet.Cells["A:AZ"].AutoFitColumns();
            Ep.SaveAs(path);
            return path1;
        }
        public static string pdfdsdiembaitap(List<DOANTOTNGHIEP.Models.Modelcreate.BaiTap> dsdiem, string malop)
        {
            if (dsdiem != null && dsdiem.Count > 0)
            {
                DB db = new DB();

                var lophoc = db.LopHocs.SingleOrDefault(x => x.MaLop.ToString().Equals(malop));
                PdfDocument doc = new PdfDocument();
                PdfSection sec = doc.Sections.Add();
                sec.PageSettings.Width = PdfPageSize.A4.Width;
                PdfPageBase page = sec.Pages.Add();
                float y = 10;
                PdfBrush brush1 = PdfBrushes.Black;


                PdfTrueTypeFont font1 = new PdfTrueTypeFont(new Font("Arial Unicode MS", 24f, FontStyle.Bold), true);
                PdfStringFormat format1 = new PdfStringFormat(PdfTextAlignment.Center);

                page.Canvas.DrawString("BẢNG ĐIỂM BÀI TẬP LỚP " + lophoc.TenLop.ToUpper(), font1, brush1, page.Canvas.ClientSize.Width / 2, y, format1);
                page.Canvas.DrawString("CHỦ ĐỀ :" + dsdiem[0].BaiTaps.ChuDe.ToUpper(), font1, brush1, page.Canvas.ClientSize.Width / 2, y + 20, format1);

                y = y + font1.MeasureString("Country List", format1).Height;

                y = y + 5;


                PdfTable table = new PdfTable();
                table.Style.CellPadding = 1;

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Họ tên");
                dataTable.Columns.Add("Email");
                dataTable.Columns.Add("Ngày nộp");
                dataTable.Columns.Add("Ngày hết hạn");
                dataTable.Columns.Add("Trạng thái");
                dataTable.Columns.Add("Điểm");
                for (int i = 2; i <= dsdiem.Count + 1; i++)
                {
                    string trangthai = "";
                    if (dsdiem[i - 2].NgayNop == null)
                    {
                        trangthai = "Chưa Nộp";
                    }
                    else if (dsdiem[i - 2].NgayNop != null)
                    {
                        if (dsdiem[i - 2].BaiTaps.ThoiGianKetThuc >= dsdiem[i - 2].NgayNop)
                        {
                            trangthai = "Đã nộp";
                        }
                        else if (dsdiem[i - 2].BaiTaps.ThoiGianKetThuc < dsdiem[i - 2].NgayNop)
                        {
                            trangthai = "Nộp muộn";
                        }
                    }
                    string ngaynop = "";
                    string ngayketthuc = "";
                    if (dsdiem[i - 2].NgayNop != null)
                    {
                        ngaynop = dsdiem[i - 2].NgayNop.Value.ToString(string.Format("HH:mm:ss dd/MM/yyyy"));
                    }
                    if (dsdiem[i - 2].BaiTaps.ThoiGianKetThuc != null)
                    {
                        ngayketthuc = dsdiem[i - 2].BaiTaps.ThoiGianKetThuc.Value.ToString(string.Format("HH:mm:ss dd/MM/yyyy"));
                    }
                    dataTable.Rows.Add(new string[] { dsdiem[i - 2].TaiKhoan.Ho + " " + dsdiem[i - 2].TaiKhoan.Ten, dsdiem[i - 2].TaiKhoan.Email
                    , ngaynop, ngayketthuc, trangthai, dsdiem[i - 2].Diem.ToString() });



                }
                table.DataSource = dataTable;
                table.Columns[0].Width = 15;
                table.Columns[1].Width = 15;
                table.Columns[5].Width = 3;
                table.Style.ShowHeader = true;
                table.Style.HeaderStyle.StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                table.Style.HeaderStyle.Font = new PdfTrueTypeFont(new Font("Arial Unicode MS", 10f), true);
                table.Style.DefaultStyle.Font = new PdfTrueTypeFont(new Font("Arial Unicode MS", 8f), true);
                foreach (PdfColumn col in table.Columns)
                {


                    col.StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);

                }

                table.Style.HeaderStyle.BackgroundBrush = PdfBrushes.Gray;


                table.Draw(page, new RectangleF(0, 90, 500, 70));
                string path = HostingEnvironment.MapPath("~/Content/file/" + dsdiem[0].BaiTaps.MaLop.ToString() + "_" + dsdiem[0].BaiTaps.MaBaiTap.ToString() + ".pdf");

                string path1 = "/Content/file/" + dsdiem[0].BaiTaps.MaLop.ToString() + "_" + dsdiem[0].BaiTaps.MaBaiTap.ToString() + ".pdf";
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                doc.SaveToFile(path);
                return path1;
            }
            return "";
        }
        public static string pdfdsdiem(List<Diem> dsdiem, string malop)
        {
            DB db = new DB();

            var lophoc = db.LopHocs.SingleOrDefault(x => x.MaLop.ToString().Equals(malop));
            PdfDocument doc = new PdfDocument();
            PdfSection sec = doc.Sections.Add();
            sec.PageSettings.Width = PdfPageSize.A4.Width;
            PdfPageBase page = sec.Pages.Add();
            float y = 10;
            PdfBrush brush1 = PdfBrushes.Black;


            PdfTrueTypeFont font1 = new PdfTrueTypeFont(new Font("Arial Unicode MS", 24f, FontStyle.Bold), true);
            PdfStringFormat format1 = new PdfStringFormat(PdfTextAlignment.Center);

            page.Canvas.DrawString("BẢNG ĐIỂM LỚP " + lophoc.TenLop.ToUpper(), font1, brush1, page.Canvas.ClientSize.Width / 2, y, format1);

            y = y + font1.MeasureString("Country List", format1).Height;

            y = y + 5;


            PdfTable table = new PdfTable();
            table.Style.CellPadding = 1;

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Họ tên");
            dataTable.Columns.Add("Email");
            dataTable.Columns.Add("Số lượng bài đã nộp");
            dataTable.Columns.Add("Số lượng bài trễ");
            dataTable.Columns.Add("Số lượng bài chưa nộp");
            dataTable.Columns.Add("Số lượng bài đã chấm");
            dataTable.Columns.Add("Điểm");
            for (int i = 2; i <= dsdiem.Count + 1; i++)
            {
                dataTable.Rows.Add(new string[] { dsdiem[i - 2].Hoten, dsdiem[i - 2].email, dsdiem[i - 2].soluongbaitap.ToString()
                    , dsdiem[i - 2].soluongtre.ToString(), dsdiem[i - 2].soluongchuanop.ToString(), dsdiem[i - 2].soluongbaicodiem.ToString(),
            dsdiem[i - 2].diem.ToString()});

            }
            table.DataSource = dataTable;
            table.Columns[0].Width = 15;
            table.Columns[1].Width = 20;
            table.Columns[6].Width = 8;
            table.Style.ShowHeader = true;
            table.Style.HeaderStyle.BackgroundBrush = PdfBrushes.Gray;

            table.Style.HeaderStyle.StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
            table.Style.HeaderStyle.Font = new PdfTrueTypeFont(new Font("Arial Unicode MS", 10f), true);
            table.Style.DefaultStyle.Font = new PdfTrueTypeFont(new Font("Arial Unicode MS", 8f), true);
            foreach (PdfColumn col in table.Columns)
            {


                col.StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);

            }

            table.Style.HeaderStyle.BackgroundBrush = PdfBrushes.Gray;

            table.Draw(page, new RectangleF(0, 90, 500, 800));

            string path = HostingEnvironment.MapPath("~/Content/file/" + lophoc.MaLop.ToString() + ".pdf");
            string path1 = "/Content/file/" + lophoc.MaLop.ToString() + ".pdf";
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            doc.SaveToFile(path);
            return path1;

        }


        
        public static string updatepdfdaovan(string linkfile ,string noiluu, List<DOANTOTNGHIEP.Models.Modelcreate.detaildaovan> resultcaudaovan, string option)
        {


            /*string[] mamau = {"",};*/
            List<DOANTOTNGHIEP.Modelcreate.filedaovan> tonghopfile = new List<DOANTOTNGHIEP.Modelcreate.filedaovan>();

            string linkfilePdf = HostingEnvironment.MapPath(noiluu);
            string filePdf = HostingEnvironment.MapPath(linkfile);
            PdfDocument pdfSource = new PdfDocument(filePdf);
            List<string> aa = new List<string>();
            List<string> aa1 = new List<string>();

            foreach (var s in resultcaudaovan)
            {
               if(tonghopfile.SingleOrDefault(xx=>xx.linkfile.Equals(s.locationfilecompare)) == null){
                    DOANTOTNGHIEP.Modelcreate.filedaovan listfile = new DOANTOTNGHIEP.Modelcreate.filedaovan();
                    listfile.linkfile = s.locationfilecompare;
                    Random r = new Random();
                    while (true)
                    {
                        var red = r.Next(0, 255);
                        var green = r.Next(0, 255);
                        var blue = r.Next(0, 255);
                        if (tonghopfile.SingleOrDefault(xx => xx.red.Equals(red) && xx.green.Equals(green) && xx.blue.Equals(blue) ) == null)
                        {
                            listfile.red = red;
                            listfile.green = green;
                            listfile.blue = blue;
                            break;
                        }
                    }
                    tonghopfile.Add(listfile);
                }
                
            }
            // settings


            // load PDF file

            // search for text and retrieve all found text positions


            foreach (var file in resultcaudaovan)
            {
                /*if(tonghopfile.Select(x => x.linkfile.ToString().Contains(file.locationfilecompare.ToString())).Count == 0)
                {

                }*/
                var mau = tonghopfile.SingleOrDefault(xx => xx.linkfile.Equals(file.locationfilecompare));
                if (file.per >0)
                {
                    if (file.Keywor1.Replace(" ", "").Length > 0)
                    {
                       
                        foreach (var item in file.sentenceKeyword)
                        {
                            var s = item.Replace("\r", " ").Replace("  ", " ");
                            if(s.Replace(" ","").Length > 0)
                            {
                                

                                foreach(var ss in item.Replace("\r", "").Split('\n'))
                                {
                                    string strRegex = string.Format(@"{0}", ss.TrimEnd(' ').TrimStart(' '));
                                
                                    if(strRegex.Length > 0)
                                    {
                                        for (var i = 0; i < pdfSource.Pages.Count; i++)
                                        {
                                            PdfTextFind[] finds = pdfSource.Pages[i].FindText(strRegex, Spire.Pdf.General.Find.TextFindParameter.IgnoreCase).Finds;

                                            if (finds.Length > 0)
                                            {
                                                foreach (PdfTextFind textCurrent in finds)
                                                {
                                                    textCurrent.ApplyHighLight(Color.FromArgb(mau.red,mau.green,mau.blue));
                                                }
                                            }
                                        }
                                    }

                                }

                            }
                        }
                        
                    }
                }
            }



            PdfSection sec = pdfSource.Sections.Add();
            sec.PageSettings.Width = PdfPageSize.A4.Width;
            PdfPageBase page = sec.Pages.Add();
            float y = 10;
            PdfBrush brush1 = PdfBrushes.Black;


            PdfTrueTypeFont font = new PdfTrueTypeFont(new Font("Arial Unicode MS", 16f, FontStyle.Bold), true);
            PdfTrueTypeFont font2 = new PdfTrueTypeFont(new Font("Arial Unicode MS", 12f, FontStyle.Bold), true);
            PdfStringFormat format = new PdfStringFormat(PdfTextAlignment.Center);

            String label = "File So sánh trong "+option;
            float x = page.Canvas.ClientSize.Width / 2;
            page.Canvas.DrawString(label, font, PdfBrushes.Black, x, y, format);
            y = y + font.MeasureString(label).Height;

            x = 10;
            foreach(var filesosanh in tonghopfile)
            {
                string domainName = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                String url1 = domainName +filesosanh.linkfile;
                String text = filesosanh.linkfile;
                PdfTextWebLink link2 = new PdfTextWebLink();
                link2.Text = text;
                link2.Url = url1;
                link2.Font = font2;
                link2.Brush = PdfBrushes.Black;
                link2.DrawTextWebLink(page.Canvas, new PointF(x, y));
                y = y + font.MeasureString(text).Height;


                PdfTextFind[] finds = page.FindText(text, Spire.Pdf.General.Find.TextFindParameter.IgnoreCase).Finds;

                if (finds.Length > 0)
                {
                    foreach (PdfTextFind textCurrent in finds)
                    {
                        textCurrent.ApplyHighLight(Color.FromArgb(filesosanh.red, filesosanh.green, filesosanh.blue));
                    }
                }
            }
            


            pdfSource.SaveToFile(linkfilePdf.Replace(".pdf", "-highlight.pdf"));
            return noiluu.Replace(".pdf", "-highlight.pdf");
        }

        public static string getdatapdf(string filepdf)
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(HostingEnvironment.MapPath(filepdf));
            StringBuilder buffer = new StringBuilder();
            IList<Image> images = new List<Image>();
            foreach (PdfPageBase page in doc.Pages)
            {
                buffer.Append(page.ExtractText());
            }
            doc.Close();
            var noidung = buffer.ToString();
            return noidung;
        }
    }
}