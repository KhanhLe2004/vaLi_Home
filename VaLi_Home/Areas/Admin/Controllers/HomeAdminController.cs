using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VaLi_Home.Models;
using X.PagedList;

namespace VaLi_Home.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    public class HomeAdminController : Controller
    {
        QlbanVaLiContext db = new QlbanVaLiContext();
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("danhmucsanpham")]
        public IActionResult DanhMucSanPham(int? page)
        {
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstSanPham = db.TDanhMucSps.AsNoTracking().OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstSanPham, pageNumber, pageSize);
            return View(lst);
        }
        [Route("ThemSanPhamMoi")]
        [HttpGet]
        public IActionResult ThemSanPhamMoi()
        {
            ViewBag.MaChatLieu = new SelectList(db.TChatLieus.ToList(), "MaChatLieu", "ChatLieu");  
            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");  
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");  
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");  
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");  
            return View();
        }
        [Route("ThemSanPhamMoi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemSanPhamMoi(TDanhMucSp sanPham)
        {
            if(ModelState.IsValid)
            {
                db.TDanhMucSps.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham");
            }
            return View(sanPham);
        }
        [Route("SuaSanPham")]
        [HttpGet]
        public IActionResult SuaSanPham(string maSanPham)
        {
            ViewBag.MaChatLieu = new SelectList(db.TChatLieus.ToList(), "MaChatLieu", "ChatLieu");
            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");
            var sanPham=db.TDanhMucSps.Find(maSanPham);
            return View(sanPham);
        }
        [Route("SuaSanPham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaSanPham(TDanhMucSp sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State=EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham");
            }
            return View(sanPham);
        }
        [Route("XoaSanPham")]
        [HttpGet]
        public IActionResult XoaSanPham(string maSp)
        {
            TempData["Message"] = "";

            var CTSP = db.TChiTietSanPhams.Where(x => x.MaSp == maSp).Select(x => x.MaChiTietSp).ToList();

            var CTHDB = db.TChiTietHdbs.Where(x => CTSP.Contains(x.MaChiTietSp)).ToList();
            if (CTHDB.Any()) db.RemoveRange(CTHDB);
            var AnhCT = db.TAnhChiTietSps.Where(x => CTSP.Contains(x.MaChiTietSp)).ToList();
            if (AnhCT.Any()) db.RemoveRange(AnhCT);

            var chitietsp = db.TChiTietSanPhams.Where(x => x.MaSp == maSp);
            if (chitietsp.Any()) db.RemoveRange(chitietsp);

            var anhSanPhams = db.TAnhSps.Where(x => x.MaSp == maSp);
            if (anhSanPhams.Any()) db.RemoveRange(anhSanPhams);

            db.Remove(db.TDanhMucSps.Find(maSp));
            db.SaveChanges();
            TempData["Message"] = "San pham da duoc xoa";
            return RedirectToAction("DanhMucSanPham", "HomeAdmin");
        }
        //public IActionResult XoaSanPham(string maSp)
        //{
        //    TempData["Message"] = "";
        //    try
        //    {
        //        var chiTietSanPhamIds = db.TChiTietSanPhams.Where(x => x.MaSp == maSp).Select(x => x.MaChiTietSp).ToList();

        //        var chiTietHDBs = db.TChiTietHdbs.Where(x => chiTietSanPhamIds.Contains(x.MaChiTietSp)).ToList();
        //        if (chiTietHDBs.Any())
        //        {
        //            db.TChiTietHdbs.RemoveRange(chiTietHDBs);
        //        }
        //        var anhChiTietSPs = db.TAnhChiTietSps.Where(x => chiTietSanPhamIds.Contains(x.MaChiTietSp)).ToList();
        //        if (anhChiTietSPs.Any())
        //        {
        //            db.TAnhChiTietSps.RemoveRange(anhChiTietSPs);
        //        }
        //        var chiTietSanPhams = db.TChiTietSanPhams.Where(x => x.MaSp == maSp).ToList();
        //        if (chiTietSanPhams.Any())
        //        {
        //            db.TChiTietSanPhams.RemoveRange(chiTietSanPhams);
        //        }
        //        var anhSanPhams = db.TAnhSps.Where(x => x.MaSp == maSp).ToList();
        //        if (anhSanPhams.Any())
        //        {
        //            db.TAnhSps.RemoveRange(anhSanPhams);
        //        }
        //        var danhMucSP = db.TDanhMucSps.Find(maSp);
        //        if (danhMucSP != null)
        //        {
        //            db.TDanhMucSps.Remove(danhMucSP);
        //        }
        //        db.SaveChanges();
        //        TempData["Message"] = "Sản phẩm đã được xóa.";
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        //        TempData["Message"] = "Có lỗi xảy ra khi lưu thay đổi: " + innerMessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Message"] = "Có lỗi xảy ra: " + ex.Message;
        //    }

        //    return RedirectToAction("DanhMucSanPham", "HomeAdmin");
        //}



    }
}
