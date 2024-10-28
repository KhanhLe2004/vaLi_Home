using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VaLi_Home.Models.ProductModels;
using VaLi_Home.Models;

namespace VaLi_Home.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        QlbanVaLiContext db = new QlbanVaLiContext();
        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            IList<Product> lstproducts = new List<Product>();
            var sanPhams = db.TDanhMucSps.OrderBy(x => x.TenSp).ToList();
            foreach (var sanPham in sanPhams)
            {
                lstproducts.Add(new Product
                {
                    MaSp = sanPham.MaSp,
                    TenSp = sanPham.TenSp,
                    MaLoai = sanPham.MaLoai,
                    AnhDaiDien = sanPham.AnhDaiDien,
                    GiaNhoNhat = sanPham.GiaNhoNhat
                });
            }
            return lstproducts;
        }
        //[HttpGet("{maLoai}")]
        [HttpGet("GetProductsByCategory/{maLoai}")]
        public IEnumerable<Product> GetProductsByCategory(string maLoai)
        {
            IList<Product> lstproducts = new List<Product>();
            var sanPhams = db.TDanhMucSps.Where(x => x.MaLoai == maLoai).OrderBy(x => x.TenSp).ToList();
            foreach (var sanPham in sanPhams)
            {
                lstproducts.Add(new Product
                {
                    MaSp = sanPham.MaSp,
                    TenSp = sanPham.TenSp,
                    MaLoai = sanPham.MaLoai,
                    AnhDaiDien = sanPham.AnhDaiDien,
                    GiaNhoNhat = sanPham.GiaNhoNhat
                });
            }
            return lstproducts;
        }
        [HttpPost]
        public IActionResult ThemSanPham([FromBody] TDanhMucSp sanPham)
        {
            if (ModelState.IsValid)
            {
                db.TDanhMucSps.Add(sanPham);
                db.SaveChanges();
            }
            return Ok(sanPham);
        }
        [HttpDelete("{maSanPham}")]
        public IActionResult XoaSanPham(string maSanPham)
        {
            var sanPham = db.TDanhMucSps.Where(x => x.MaSp == maSanPham).SingleOrDefault();
            db.TDanhMucSps.Remove(sanPham);
            return Ok(sanPham);
        }
        [HttpPut("{maSanPham}")]
        public IActionResult SuaSanPham([FromBody] string maSanPham)
        {
            var sanPham = db.TDanhMucSps.Where(x => x.MaSp == maSanPham).SingleOrDefault();
            if (ModelState.IsValid)
            {
                db.TDanhMucSps.Update(sanPham);
                db.SaveChanges();
            }
            return Ok(sanPham);
        }
    }
}
