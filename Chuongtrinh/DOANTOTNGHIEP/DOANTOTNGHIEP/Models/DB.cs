using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DOANTOTNGHIEP.Models
{
    public partial class DB : DbContext
    {
        public DB()
            : base("name=DB")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<BaiTap> BaiTaps { get; set; }
        public virtual DbSet<BaiTapTL> BaiTapTLs { get; set; }
        public virtual DbSet<BaiTapTN> BaiTapTNs { get; set; }
        public virtual DbSet<CauHoi> CauHois { get; set; }
        public virtual DbSet<Chudetailieu> Chudetailieux { get; set; }
        public virtual DbSet<commentbaitap> commentbaitaps { get; set; }
        public virtual DbSet<commentnotification> commentnotifications { get; set; }
        public virtual DbSet<DapAn> DapAns { get; set; }
        public virtual DbSet<document> documents { get; set; }
        public virtual DbSet<FileBTTL> FileBTTLs { get; set; }
        public virtual DbSet<FileTB> FileTBs { get; set; }
        public virtual DbSet<KeywordLibrary> KeywordLibraries { get; set; }
        public virtual DbSet<KeywordTailieu> KeywordTailieux { get; set; }
        public virtual DbSet<Library> Libraries { get; set; }
        public virtual DbSet<Loimoi> Loimois { get; set; }
        public virtual DbSet<LopHoc> LopHocs { get; set; }
        public virtual DbSet<Mess> Messes { get; set; }
        public virtual DbSet<Plagiarism> Plagiarism { get; set; }
        public virtual DbSet<replycomment> replycomments { get; set; }
        public virtual DbSet<replycommentBT> replycommentBTs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }
        public virtual DbSet<ThanhVienLop> ThanhVienLops { get; set; }
        public virtual DbSet<ThongBao> ThongBaos { get; set; }
        public virtual DbSet<TTBaiTapTL> TTBaiTapTLs { get; set; }
        public virtual DbSet<TTBaiTapTN> TTBaiTapTNs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaiTap>()
                .HasMany(e => e.BaiTapTLs)
                .WithRequired(e => e.BaiTap)
                .HasForeignKey(e => e.MaBaiTap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BaiTap>()
                .HasMany(e => e.BaiTapTNs)
                .WithRequired(e => e.BaiTap)
                .HasForeignKey(e => e.MaBaiTap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BaiTap>()
                .HasMany(e => e.CauHois)
                .WithOptional(e => e.BaiTap)
                .HasForeignKey(e => e.MaBaiTap);

            modelBuilder.Entity<BaiTap>()
                .HasMany(e => e.commentbaitaps)
                .WithOptional(e => e.BaiTap)
                .HasForeignKey(e => e.MaBaiTap);

            modelBuilder.Entity<BaiTap>()
                .HasMany(e => e.ThongBaos)
                .WithOptional(e => e.BaiTap)
                .HasForeignKey(e => e.MaBaiTap);

            modelBuilder.Entity<BaiTap>()
                .HasMany(e => e.FileBTTLs)
                .WithOptional(e => e.BaiTap)
                .HasForeignKey(e => e.MaBT);

            modelBuilder.Entity<BaiTapTL>()
                .HasMany(e => e.TTBaiTapTLs)
                .WithOptional(e => e.BaiTapTL)
                .HasForeignKey(e => e.MaBaiNop);

            modelBuilder.Entity<BaiTapTN>()
                .HasMany(e => e.TTBaiTapTNs)
                .WithOptional(e => e.BaiTapTN)
                .HasForeignKey(e => e.MaBaiNop);

            modelBuilder.Entity<CauHoi>()
                .HasMany(e => e.DapAns)
                .WithOptional(e => e.CauHoi)
                .HasForeignKey(e => e.MaCauHoi);

            modelBuilder.Entity<CauHoi>()
                .HasMany(e => e.TTBaiTapTNs)
                .WithOptional(e => e.CauHoi)
                .HasForeignKey(e => e.MaCauHoi);

            modelBuilder.Entity<Chudetailieu>()
                .HasMany(e => e.KeywordTailieux)
                .WithOptional(e => e.Chudetailieu)
                .HasForeignKey(e => e.Machude);

            modelBuilder.Entity<Chudetailieu>()
                .HasMany(e => e.Libraries)
                .WithOptional(e => e.Chudetailieu)
                .HasForeignKey(e => e.MaNhom);

            modelBuilder.Entity<commentbaitap>()
                .HasMany(e => e.replycommentBTs)
                .WithOptional(e => e.commentbaitap)
                .HasForeignKey(e => e.MaComment);

            modelBuilder.Entity<commentnotification>()
                .HasMany(e => e.replycomments)
                .WithOptional(e => e.commentnotification)
                .HasForeignKey(e => e.Macomment);

            modelBuilder.Entity<DapAn>()
                .HasMany(e => e.TTBaiTapTNs)
                .WithOptional(e => e.DapAn)
                .HasForeignKey(e => e.MaDapAnluaChon);

            modelBuilder.Entity<Library>()
                .HasMany(e => e.documents)
                .WithOptional(e => e.Library)
                .HasForeignKey(e => e.IDLibrary);

            modelBuilder.Entity<Library>()
                .HasMany(e => e.FileBTTLs)
                .WithOptional(e => e.Library)
                .HasForeignKey(e => e.IDLibrary);

            modelBuilder.Entity<Library>()
                .HasMany(e => e.FileTBs)
                .WithOptional(e => e.Library)
                .HasForeignKey(e => e.IDLibrary);

            modelBuilder.Entity<Library>()
                .HasMany(e => e.TTBaiTapTLs)
                .WithOptional(e => e.Library)
                .HasForeignKey(e => e.IDLibrary);

            modelBuilder.Entity<LopHoc>()
                .HasMany(e => e.BaiTaps)
                .WithOptional(e => e.LopHoc)
                .HasForeignKey(e => e.MaLop);

            modelBuilder.Entity<LopHoc>()
                .HasMany(e => e.documents)
                .WithOptional(e => e.LopHoc)
                .HasForeignKey(e => e.MaLop);

            modelBuilder.Entity<LopHoc>()
                .HasMany(e => e.Loimois)
                .WithRequired(e => e.LopHoc)
                .HasForeignKey(e => e.MaLop)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LopHoc>()
                .HasMany(e => e.Messes)
                .WithOptional(e => e.LopHoc)
                .HasForeignKey(e => e.malop);

            modelBuilder.Entity<LopHoc>()
                .HasMany(e => e.ThanhVienLops)
                .WithRequired(e => e.LopHoc)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LopHoc>()
                .HasMany(e => e.ThongBaos)
                .WithOptional(e => e.LopHoc)
                .HasForeignKey(e => e.MaLop);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.BaiTapTLs)
                .WithOptional(e => e.TaiKhoan)
                .HasForeignKey(e => e.NguoiNop);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.BaiTapTNs)
                .WithOptional(e => e.TaiKhoan)
                .HasForeignKey(e => e.NguoiNop);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.commentbaitaps)
                .WithOptional(e => e.TaiKhoan)
                .HasForeignKey(e => e.Nguoidang);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.commentnotifications)
                .WithOptional(e => e.TaiKhoan)
                .HasForeignKey(e => e.Nguoidang);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.documents)
                .WithOptional(e => e.TaiKhoan)
                .HasForeignKey(e => e.Nguoisohuu);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.Libraries)
                .WithOptional(e => e.TaiKhoan)
                .HasForeignKey(e => e.NguoiAdd);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.Loimois)
                .WithRequired(e => e.TaiKhoan)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.LopHocs)
                .WithOptional(e => e.TaiKhoan)
                .HasForeignKey(e => e.NguoiTao);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.Messes)
                .WithOptional(e => e.TaiKhoan)
                .HasForeignKey(e => e.NguoiGui);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.Messes1)
                .WithOptional(e => e.TaiKhoan1)
                .HasForeignKey(e => e.NguoiNhan);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.replycomments)
                .WithOptional(e => e.TaiKhoan)
                .HasForeignKey(e => e.Nguoidang);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.replycommentBTs)
                .WithOptional(e => e.TaiKhoan)
                .HasForeignKey(e => e.Nguoidang);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.ThanhVienLops)
                .WithRequired(e => e.TaiKhoan)
                .HasForeignKey(e => e.Mathanhvien)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.ThongBaos)
                .WithOptional(e => e.TaiKhoan)
                .HasForeignKey(e => e.NguoiDang);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.TTBaiTapTLs)
                .WithOptional(e => e.TaiKhoan)
                .HasForeignKey(e => e.NguoiNop);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.TTBaiTapTNs)
                .WithOptional(e => e.TaiKhoan)
                .HasForeignKey(e => e.NguoiNop);

            modelBuilder.Entity<ThongBao>()
                .HasMany(e => e.commentnotifications)
                .WithOptional(e => e.ThongBao)
                .HasForeignKey(e => e.MaThongbao);

            modelBuilder.Entity<ThongBao>()
                .HasMany(e => e.FileTBs)
                .WithOptional(e => e.ThongBao)
                .HasForeignKey(e => e.Mathongbao);

            modelBuilder.Entity<TTBaiTapTL>()
                .HasMany(e => e.Plagiarism)
                .WithOptional(e => e.TTBaiTapTL)
                .HasForeignKey(e => e.Mafile);
        }
    }
}
