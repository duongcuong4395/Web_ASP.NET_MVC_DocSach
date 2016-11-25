using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebsiteDocSach.Models;

namespace WebsiteDocSach.Controllers.QuanTriControllers
{
    public class AdminController : Controller
    { 
        // GET: Admin
        QLBanSachDataContext data = new QLBanSachDataContext();


        //Trang này Quản lý toàn bộ Admin
        public ActionResult Index()
        { 
            return View();
        }


        [HttpGet]
        public ActionResult DangNhap()
        {
            if (Session["ADMIN"] != null)
            {
                 ViewBag.thongbaodangnhap = " ";
            }
            else
            {
                ViewBag.thongbaodangnhap = "loiChuaDangNhap";
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(FormCollection frmcollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var TenDangNhapAdmin = frmcollection["TenDangNhapAdmin"];
                    var MatKhauAdmin = frmcollection["MatKhauAdmin"];

                    QuanTri qt = data.QuanTris.Where(n => n.username == TenDangNhapAdmin && n.password == MatKhauAdmin).FirstOrDefault();
                    if (qt != null)
                    {
                        // ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                        Session["ADMIN"] = qt;
                        Session["username"] = qt.username;
                        Session["idadmin"] = qt.idadmin;
                        Session["idQuyen"] = qt.idQuyen; 
                        return RedirectToAction("Index", "QuanTri");
                    }
                    else
                    {
                        ViewBag.thongbaodangnhap = "Tên đăng nhập hoặc mật khẩu không đúng.";
                    }
                } 
                return View();
            }
            catch (Exception error)
            {
                return this.DangNhap();
            }
        } 

        public ActionResult MenuOptionTheoPhanQuyen()
        {
            return PartialView();
        }

        public ActionResult DangXuat()
        {
            Session.Abandon();
            return RedirectToAction("DangNhap", "Admin");
        }
         
        public ActionResult HoaHonKhachHang()
        {
            List<donhang> donDatHang = data.donhangs.ToList();
            if(donDatHang == null)
            {
                ViewBag.thongbaodonhang = "Chưa có khách đặt hàng.";
            }
            else
            {
                ViewBag.thongbaodonhang = "Hóa Đơn";
            }
            return PartialView(donDatHang);
        }

        public ActionResult chiTietHoaDon(int Mahoadon)
        {
            khachhang kh = (from khachhangmoi in data.khachhangs
                           join donhangmoi in data.donhangs on khachhangmoi.idkh equals donhangmoi.idkh
                           where (donhangmoi.iddh == Mahoadon)
                           select khachhangmoi).FirstOrDefault();
            ViewBag.khGuiHoTen = kh.hotenkh;
            ViewBag.khGuiDiachi = kh.diachi;
            ViewBag.khGuiSDT = kh.dienthoaikh;
            ViewBag.khGuiEmail = kh.email;

            donhang dh = data.donhangs.Where(m => m.iddh == Mahoadon).FirstOrDefault();
            ViewBag.khNhanHoTen = dh.hotennguoinhan;
            ViewBag.khNhanDiachi = dh.diachinguoinhan;
            ViewBag.khNhanSDT = dh.sodienthoainguoinhan; 

            var chiTietHD = data.ChiTietHoaDonKH(Mahoadon);
            return View(chiTietHD);
        }
    }
}