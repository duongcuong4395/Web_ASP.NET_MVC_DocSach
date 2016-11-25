using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebsiteDocSach.Models;
using PagedList;
using PagedList.Mvc;

namespace WebsiteDocSach.Controllers.SachStoreControllers
{
    public class SachController : Controller
    {
        // GET: Sach 
        QLBanSachDataContext data = new QLBanSachDataContext();
        public ActionResult Index()
        {
            return View();
        }  

        public ViewResult XemChiTiet(int id)
        { 
            var NXBTacGiaThLoaiSach = data.tacGiaNXBTheoSach(id);
            if (NXBTacGiaThLoaiSach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            Session["masach"] = id;
            return View(NXBTacGiaThLoaiSach);
        } 

        //Thể loại sách
        public ActionResult TheLoaiSachPartial()
        {
            var tls = data.sachTheoTheLoai();
            return PartialView(tls);
        }

        public ActionResult SachTheoTheLoai(int idchude, int? page)
        {
            //Tạo biến Quy định sô sản phẩm trên 1 trang
            int pagesize = 12;
            //Tạo biến số sang
            int pagenum = (page ?? 1);

            chude cd = data.chudes.SingleOrDefault(n => n.idchude == idchude);
            // kiểm tra loại hàng tồn tại
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<sach> lsSach = data.saches.Where(n => n.idchude == idchude).OrderByDescending(n => n.tensach).ToList();
            if (lsSach.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy loại thàng nào";
            }
            ViewBag.TenTheLoai = cd.tenchude;
            ViewBag.idchude = cd.idchude;
            return PartialView( lsSach.ToPagedList(pagenum, pagesize));
        }

        [ChildActionOnly]
        //Thể loại sách
        public ActionResult TacGiaPartial()
        {
            var tacGia = data.sachTheoTacGia();
            return PartialView(tacGia);
        } 
         
        [ChildActionOnly]
        public ActionResult tacgiatheosoluong(int skip, int take)
        {
            List<tacgia> tg = data.tacgias.OrderByDescending(n => n.hotentg).Skip(skip).Take(take).ToList(); 
            return PartialView(tg.ToList());
        }

        public ActionResult SachTheoTacGia(int idtacgia, int? page)
        {
            //Tạo biến Quy định sô sản phẩm trên 1 trang
            int pagesize = 12;
            //Tạo biến số sang
            int pagenum = (page ?? 1);

            tacgia tg = data.tacgias.SingleOrDefault(n => n.idtacgia == idtacgia);
            // kiểm tra loại hàng tồn tại
            if (tg == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<sach> lsSach = (from laysach in data.saches
                                 join laytacgia in data.thamgias on laysach.idsach equals laytacgia.idsach
                                 where laytacgia.idtacgia == idtacgia
                                 select laysach).ToList();
            if (lsSach.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy loại thàng nào";
            }
            ViewBag.hotentg = tg.hotentg;
            ViewBag.idtacgia = tg.idtacgia;
            return PartialView(lsSach.ToPagedList(pagenum, pagesize));
        }

        //Nhà Xuất bản
        public ActionResult NhaXuatBanPartial()
        {
            var nxb = data.sachTheoNhaXuatBan();
            return PartialView(nxb);
        }

        public ActionResult SachTheoNXB(int idnxb, int? page)
        {
            //Tạo biến Quy định sô sản phẩm trên 1 trang
            int pagesize = 6;
            //Tạo biến số sang
            int pagenum = (page ?? 1);

            nxb nhaxuatban = data.nxbs.SingleOrDefault(n => n.idnxb == idnxb);
            // kiểm tra loại hàng tồn tại
            if (nhaxuatban == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            List<sach> lsSach = data.saches.Where(n => n.idnxb == idnxb).OrderByDescending(n => n.tensach).ToList();
            if (lsSach.Count == 0)
            {
                ViewBag.ThongBao = "Không tìm thấy loại thàng nào";
            }
            ViewBag.tennxb = nhaxuatban.tennxb;
            ViewBag.idnxb = nhaxuatban.idnxb;
            return PartialView(lsSach.ToPagedList(pagenum, pagesize));
        }


        [HttpPost]
        [ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult DanhGiaKhachHang( FormCollection frmDanhGia)
        {
            int masach = int.Parse(Session["masach"].ToString());
            try
            {
                if (Session["Taikhoan"] != null)
                {
                    
                    var NoiDung = frmDanhGia["NoiDungDanhGia"];
                    var CHatLuongSach = frmDanhGia["ChatLuong"];

                    if (ModelState.IsValid)
                    { 
                        KHDanhGia danhGiaKhachHang = new KHDanhGia();
                        danhGiaKhachHang.HoTen = Session["TenKhachHang"].ToString();
                        danhGiaKhachHang.Email = Session["EmailKH"].ToString();
                        danhGiaKhachHang.idsach = masach;
                        danhGiaKhachHang.NoiDung = NoiDung;
                        danhGiaKhachHang.ChatLuong = CHatLuongSach;
                        data.KHDanhGias.InsertOnSubmit(danhGiaKhachHang);
                        data.SubmitChanges();

                        ViewBag.thongBaoloi = "Cảm ơn bạn đã góp ý cho cuốn sách này."; 
                    }
                }
                else
                {
                    ViewBag.thongBaoloi = "Bạn Vẫn chưa đăng nhập không thể đánh giá."; 
                }
                return RedirectToAction("XemChiTiet", "Sach", new { id = masach });
            }
            catch (Exception error)
            {
                return RedirectToAction("XemChiTiet", "Sach", new { id = masach });
            }
        }



    }
}