using Microsoft.AspNetCore.Mvc;
using VaLi_Home.Models;
using VaLi_Home.Models.ProductModels;

namespace TestTpl7.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductAPIController : Controller
	{
		QlbanVaLiContext db=new QlbanVaLiContext();	
		[HttpGet]
		public IEnumerable<Product> GetAllProduct()
		{
			var sanPham = (from p in db.TDanhMucSps
						   select new Product
						   {
							   MaSp = p.MaSp,
							   TenSp = p.TenSp,
							   MaLoai = p.MaLoai,
							   AnhDaiDien = p.AnhDaiDien,
							   GiaNhoNhat = p.GiaNhoNhat,
						   }).ToList();
			return sanPham;
		}
		[HttpGet("{maloai}")]
		public IEnumerable<Product> GetProductByCatagory(string maloai)
		{
			var sanPham = (from p in db.TDanhMucSps
						   where p.MaLoai == maloai
						   select new Product
						   {
							   MaSp = p.MaSp,
							   TenSp = p.TenSp,
							   MaLoai = p.MaLoai,
							   AnhDaiDien = p.AnhDaiDien,
							   GiaNhoNhat = p.GiaNhoNhat,
						   }).ToList();
			return sanPham;
		}
	}
}
