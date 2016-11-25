using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebsiteDocSach.Models;

namespace WebsiteDocSach.Controllers.NguoiDungControllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang

        QLBanSachDataContext data = new QLBanSachDataContext();
        public ActionResult Index()
        {
            return View();
        }

        //Lấy giỏ hàng trong session["GioHang"]
        public List<GioHang> layGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        //Thêm hàng vào giỏ hàng tương ứng với mã mặt hàng
        public ActionResult ThemGioHang(int sidsach, string strURL)
        {
            List<GioHang> lstGioHang = layGioHang();
            GioHang dsSanPham = lstGioHang.Find(n => n.sidsach == sidsach);
            if (dsSanPham == null)
            {
                dsSanPham = new GioHang(sidsach);
                lstGioHang.Add(dsSanPham);
                ViewBag.thongbaothem = "Bạn đã đưa vào giỏ.";
                return Redirect(strURL);
            }
            else
            { 
                dsSanPham.iSoLuong++;
                ViewBag.thongbaothem = "Bạn đã đưa vào giỏ.";
                return Redirect(strURL);
            }
        }

        //Tính tổng số lượng hàng trong giỏ hàng
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }

        //Tính tổng tiền trong giỏ hàng
        private double TongTien()
        {
            double iTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongTien = lstGioHang.Sum(n => n.dThanhTien);
            }
            return iTongTien;
        }

        //dang sách hàng trong giỏ hàng
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = layGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "SachStore");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView(lstGioHang);
        }
         

        //dang sách hàng trong giỏ hàng
        public ActionResult GioHangLayoutNguoiDung()
        {
            try
            {
                List<GioHang> lstGioHang = layGioHang();
                if (lstGioHang.Count == 0)
                {
                    ViewBag.TongSoLuongmoi = TongSoLuong();
                }
                ViewBag.TongSoLuongmoi = TongSoLuong();
                ViewBag.TongTienmoi = TongTien();
                return PartialView(lstGioHang);
            }
            catch (Exception error)
            {
                ViewBag.TongSoLuongmoi = TongSoLuong();
                return RedirectToAction("Index", "SachStore");
            }
        }

        //Xóa hàng trong giỏ hàng
        public ActionResult XoaGioHang(int id)
        {
            //Lấy giỏ hàng có trong session["GioHang"]
            List<GioHang> lstGioHang = layGioHang();
            //Kiểm tra hàng trong giỏ
            GioHang layHangHoa = lstGioHang.SingleOrDefault(n => n.sidsach == id);
            if (layHangHoa != null)
            {
                lstGioHang.RemoveAll(n => n.sidsach == id);
                return RedirectToAction("GioHang");
            }

            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "DoGoStore");
            }
            return RedirectToAction("GioHang");
        }

        //Cập nhật giỏ hàng
        public ActionResult CapNhatGioHang(FormCollection frmCollection, int id)
        {
            //Lấy giỏ hàng từ Session["GioHang"]
            List<GioHang> lstGioHang = layGioHang();
            //Kiểm tra sản phẩm đã có trong giỏ
            GioHang sp = lstGioHang.SingleOrDefault(n => n.sidsach == id);
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(frmCollection["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        //Xóa tất cả sản phầm có trong giỏ hàng
        public ActionResult XoaTatCaGioHang()
        {
            //Lấy giỏ hàng từ session giỏ hàng
            List<GioHang> lstGioHang = layGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "DoGoStore");
        }

        [HttpGet]
        //Đặt hàng
        public ActionResult DatHang()
        {
            //Kiểm tra khách hàng đã đăng nhập chưa
            if (Session["Taikhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            //Kiểm tra hàng trong giỏ hàng
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "DoGoStore");
            }

            //Lấy giỏ hàng từ sesion["GioHang"]
            List<GioHang> lstGioHang = layGioHang();
            ViewBag.TongSoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DatHang(FormCollection frmCollection)
        {
            try
            { 
                if (ModelState.IsValid)
                {

                    if (Session["Taikhoan"] == null || Session["TaiKhoan"].ToString() == "")
                    {
                        return RedirectToAction("chon", "NguoiDung", new { thongbao = "Chua-Dang-Nhap"});
                    }
                    else
                    {
                        //Thêm đơn hàng
                        var hoTenNguoiNhan = frmCollection["HoTenNguoiNgan"];
                        var diaChiNguoiNhan = frmCollection["DiaChiNguoiNhan"];
                        var sdtNguoiNhan = frmCollection["SDTNguoiNhan"];
                        var ngayGiao = String.Format("{0:MM/dd/yyyy}", frmCollection["NgayGiao"]);

                        donhang dondathang = new donhang();
                        khachhang khachhang = (khachhang)Session["TaiKhoan"];
                        List<GioHang> lstGioHang = layGioHang();
                        //thông tin khách hàng đặt hàng
                        dondathang.idkh = khachhang.idkh;
                        dondathang.ngaylap = DateTime.Now;
                        dondathang.ngaygiaohang = DateTime.Parse(ngayGiao);
                        dondathang.tinhtranggiao = false;
                        dondathang.tinhtrangthanhtoan = false;
                        dondathang.hinhthucthanhtoan = false;
                        //Người nhận
                        dondathang.hotennguoinhan = hoTenNguoiNhan;
                        dondathang.diachinguoinhan = diaChiNguoiNhan;
                        dondathang.sodienthoainguoinhan = sdtNguoiNhan;
                        data.donhangs.InsertOnSubmit(dondathang);
                        data.SubmitChanges();
                        //Chi chi tiết đơn hàng
                        foreach (var hang in lstGioHang)
                        {
                            chitietdh chitietdonhang = new chitietdh();
                            chitietdonhang.iddh = dondathang.iddh;
                            chitietdonhang.idsach = hang.sidsach;
                            chitietdonhang.soluong = hang.iSoLuong;
                            chitietdonhang.dongia = (decimal)hang.dDonGia;
                            data.chitietdhs.InsertOnSubmit(chitietdonhang);
                        }
                        data.SubmitChanges();
                        Session["GioHang"] = null;
                    } 
                }
                return RedirectToAction("XacNhanDonHang", "GioHang");
            }
            catch (Exception error)
            {
                return RedirectToAction("XacNhanDonHang", "GioHang");
            }
        }

        public ActionResult XacNhanDonHang()
        {
            try
            {  
                
                return View();
            }
            catch (Exception error)
            {
                return RedirectToAction("Index", "SachStore");
            } 
        }


        public ActionResult DanhSachSachDaDat()
        {
            if (Session["Taikhoan"] != null)
            {
                int makh = int.Parse(Session["MaTKKH"].ToString());
                var giohangKH = data.TenSachLinkTaiTheoDonHang(makh);
                return View(giohangKH);
            } 
            else
            {
                return RedirectToAction("Index", "SachStore");
            }
        } 
    }
}