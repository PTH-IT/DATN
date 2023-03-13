using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOANTOTNGHIEP.Models.kmeans
{
    public class start
    {
        public static void phancum()
        {
            DB db = new DB();
            var thongtinbaitaptuluanlophoc = db.TTBaiTapTLs.Select(x => x).ToList().OrderBy(y => y.NgayNop).ToList();
            var thongtintailieulophoc = db.documents.Select(x => x).ToList().OrderBy(y => y.Ngaydang).ToList();
            List<DOANTOTNGHIEP.Models.Modelcreate.TaiLieuCheckDaoVan> tailieucheck = new List<Modelcreate.TaiLieuCheckDaoVan>();
            foreach (var tailieu in thongtinbaitaptuluanlophoc)
            {
                DOANTOTNGHIEP.Models.Modelcreate.TaiLieuCheckDaoVan tl = new Modelcreate.TaiLieuCheckDaoVan();
                tl.Tenfile = tailieu.Tenfile;
                tl.Thoigian = tailieu.NgayNop;
                tl.NguoiSoHuu = tailieu.NguoiNop;
                tl.NoiLuu = tailieu.NoiLuu;
                tl.Datafile = tailieu.Datafile;
                tailieucheck.Add(tl);
            }
            foreach (var tailieu in thongtintailieulophoc)
            {
                DOANTOTNGHIEP.Models.Modelcreate.TaiLieuCheckDaoVan tl = new Modelcreate.TaiLieuCheckDaoVan();
                tl.Tenfile = tailieu.Ten;
                tl.Thoigian = tailieu.Ngaydang;
                tl.NguoiSoHuu = tailieu.Nguoisohuu;
                tl.NoiLuu = tailieu.Vitriluu;
                tl.Datafile = tailieu.Noidung;
                tailieucheck.Add(tl);
            }

            List<DocumentVector> vSpace = VectorSpaceModel.ProcessDocumentCollection(tailieucheck.Select(x=>x.Datafile).ToList());
            int totalIteration = 0;
            List<Centroid> resultSet = DocumnetClustering.PrepareDocumentCluster(tailieucheck.Count, vSpace, ref totalIteration);
            string msg = string.Empty;
            int count = 1;
            foreach (Centroid c in resultSet)
            {
                msg += String.Format("------------------------------[ CLUSTER {0} ]-----------------------------{1}", count, System.Environment.NewLine);
                foreach (DocumentVector document in c.GroupedDocument)
                {
                    msg += document.Content + System.Environment.NewLine;
                    if (c.GroupedDocument.Count > 1)
                    {
                        msg += String.Format("{0}-------------------------------------------------------------------------------{0}", System.Environment.NewLine);
                    }
                }
                msg += "-------------------------------------------------------------------------------" + System.Environment.NewLine;
                count++;
            }

        }
    }
}