using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDocSach.Models;

using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebsiteDocSach.Controllers.QuanTriControllers
{
    public class QuanTriController : Controller
    {
        // GET: QuanTri
        QLBanSachDataContext data = new QLBanSachDataContext();

        private int laykhachhang()
        {
            return data.khachhangs.Count();  
        }

        private int layhoadon()
        {
            return data.donhangs.Count();
        }

        private int layThuDanhGia()
        {
            return data.KHDanhGias.Count();
        }

        private int laySach()
        {
            return data.saches.Count();
        }

        //Trang Này quản lý 
        public ActionResult Index()
        {
            if (Session["ADMIN"] != null)
            {
                ViewBag.slKhachHang = laykhachhang();
                ViewBag.slHoaDon = layhoadon();
                ViewBag.slThuDanhGia = layThuDanhGia();
                ViewBag.slSach = laySach();
                return View();
            }
            else
            { 
                return RedirectToAction("DangNhap", "Admin", false);
            }
        }


        public ActionResult khDSThich()
        {
            var danhsach = data.TenKHSLThich();
            return Json(danhsach, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DsSLSachTheoChuDe()
        {
            var danhsach = data.sachTheoTheLoai();
            return Json(danhsach, JsonRequestBehavior.AllowGet);
        }
 
        public ActionResult SoNguoiXem()
        {
            var sl = data.NguoiXems.ToList();
            return Json(sl, JsonRequestBehavior.AllowGet);
        }


        //Bắt đầu quản lý DataBase
        public ActionResult Sach(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(data.saches.ToList().OrderBy(n => n.idsach).ToPagedList(pageNumber, pageSize));
        }

        //Tao moi sach
        [HttpGet]
        public ActionResult ThemSachMoi()
        {
            ViewBag.idnxb = new SelectList(data.nxbs.ToList().OrderBy(n => n.tennxb), "idnxb", "tennxb");
            ViewBag.idchude = new SelectList(data.chudes.ToList().OrderBy(n => n.tenchude), "idchude", "tenchude");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemSachMoi(sach sach, HttpPostedFileBase fileupload)
        {
            ViewBag.maloai = new SelectList(data.nxbs.ToList().OrderBy(n => n.tennxb), "idnxb", "tennxb");
            ViewBag.macd = new SelectList(data.chudes.ToList().OrderBy(n => n.tenchude), "idchude", "tenchude");
            //kiem tra duong dan
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //them vao csdl
            else
            {
                if (ModelState.IsValid)
                {
                    //luu ten file, them thu vien system.io
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //luu duong dan 
                    var path = Path.Combine(Server.MapPath("/Images/AnhBia"), fileName);
                    //kiem tra anh da ton tai chua
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        //luu hinh anh vao duong dan
                        fileupload.SaveAs(path);
                    }
                    sach.anhbia = fileName;
                    //luu vao csdl
                    data.saches.InsertOnSubmit(sach);
                    data.SubmitChanges();
                }
                return RedirectToAction("Sach");
            }
        }

        //Chi tiet sach
        public ActionResult ChiTietSach(int id)
        {
            //lay ra doi tuong theo ma
            sach sach = data.saches.SingleOrDefault(n => n.idsach == id);
            ViewBag.idsach = sach.idsach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        //Xoa sach
        [HttpGet]
        public ActionResult XoaSach(int id)
        {
            //lay ra doi tuong theo ma
            sach sach = data.saches.SingleOrDefault(n => n.idsach == id);
            ViewBag.idsach = sach.idsach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpPost, ActionName("XoaSach")]
        public ActionResult Xacnhanxoasach(int id)
        {
            //lay ra doi tuong theo ma
            sach sach = data.saches.SingleOrDefault(n => n.idsach == id);
            ViewBag.idsach = sach.idsach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.saches.DeleteOnSubmit(sach);
            data.SubmitChanges();
            return RedirectToAction("Sach");
        }

        [HttpGet]
        public ActionResult SuaSach(int id)
        {
            //lay ra doi tuong theo ma
            sach sach = data.saches.SingleOrDefault(n => n.idsach == id);
            ViewBag.idsach = sach.idsach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.idnxb = new SelectList(data.nxbs.ToList().OrderBy(n => n.tennxb), "idnxb", "tennxb");
            ViewBag.idchude = new SelectList(data.chudes.ToList().OrderBy(n => n.tenchude), "idchude", "tenchude");
            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSach(sach s, HttpPostedFileBase fileupload, FormCollection f)
        {
            ViewBag.idnxb = new SelectList(data.nxbs.ToList().OrderBy(n => n.tennxb), "idnxb", "tennxb");
            ViewBag.idchude = new SelectList(data.chudes.ToList().OrderBy(n => n.tenchude), "idchude", "tenchude");
            sach smoi = data.saches.First(n => n.idsach == s.idsach);
            int nxb = int.Parse(f["idnxb"]);
            int cd = int.Parse(f["idchude"]);
            string mota = f["mota"];
            string anhbia = f["anhbia"];
            decimal gia = decimal.Parse(f["gia"]);
            string noidung = f["noidung"];
            string linktai = f["linktai"];
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //them vao csdl
            else
            {
                if (ModelState.IsValid)
                {
                    //luu ten file, them thu vien system.io
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //luu duong dan 
                    var path = Path.Combine(Server.MapPath("/Images/AnhBia"), fileName);
                    //kiem tra anh da ton tai chua
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        //luu hinh anh vao duong dan
                        fileupload.SaveAs(path);
                    }
                    smoi.anhbia = fileName;
                    smoi.tensach = s.tensach;
                    smoi.idchude = cd;
                    smoi.idnxb = nxb;
                    smoi.mota = mota;
                    smoi.gia = gia;
                    smoi.noidung = noidung;
                    smoi.linktai = linktai;
                    UpdateModel(smoi);
                    data.SubmitChanges();
                }
                return RedirectToAction("Sach");
            }
        }

        //Bây giờ tới chủ đề
        public ActionResult ChuDe(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(data.chudes.ToList().OrderBy(n => n.idchude).ToPagedList(pageNumber, pageSize));
        }

        //Tao moi chu de
        [HttpGet]
        public ActionResult ThemMoiChuDe()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiChuDe(chude cd)
        {
            if (cd.tenchude == null)
            {
                ViewBag.Thongbao = "Không được để trống";
            }
            data.chudes.InsertOnSubmit(cd);
            data.SubmitChanges();
            return RedirectToAction("ChuDe");
        }

        //Xoa chu de
        [HttpGet]
        public ActionResult XoaChuDe(int id)
        {
            //lay ra doi tuong can xoa
            chude cd = data.chudes.SingleOrDefault(n => n.idchude == id);
            ViewBag.macd = cd.idchude;
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(cd);
        }

        [HttpPost, ActionName("XoaChuDe")]
        public ActionResult Xacnhanxoacd(int id)
        {
            //lay ra doi tuong 
            chude cd = data.chudes.SingleOrDefault(n => n.idchude == id);
            ViewBag.macd = cd.idchude;
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.chudes.DeleteOnSubmit(cd);
            data.SubmitChanges();
            return RedirectToAction("ChuDe");
        }

        //Sua chu de
        [HttpGet]
        public ActionResult SuaChuDe(int id)
        {
            //lay ra doi tuong can 
            var cd = data.chudes.First(n => n.idchude == id);
            return View(cd);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaChuDe(int id, FormCollection f)
        {
            var chude = data.chudes.First(n => n.idchude == id);
            var cd = f["tencd"];
            chude.idchude = id;
            if (string.IsNullOrEmpty(cd))
            {
                ViewData["Loi"] = "Không được để trống";
            }
            else
            {
                chude.tenchude = cd;
                UpdateModel(chude);
                data.SubmitChanges();
            }
            return RedirectToAction("ChuDe");
        }

        //Bây giờ tới Nhà xuất bản
        public ActionResult NhaXuatBan(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(data.nxbs.ToList().OrderBy(n => n.idnxb).ToPagedList(pageNumber, pageSize));
        }

        //Tao moi nxb
        [HttpGet]
        public ActionResult ThemNhaXuatBan()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemNhaXuatBan(nxb nxb)
        {
            if (nxb == null)
            {
                ViewBag.Thongbao = "Không được để trống";
            }
            data.nxbs.InsertOnSubmit(nxb);
            data.SubmitChanges();
            return RedirectToAction("NhaXuatBan");
        }

        //Xoa nxb
        [HttpGet]
        public ActionResult XoaNhaXuatBan(int id)
        {
            //lay ra doi tuong can xoa
            nxb nxb = data.nxbs.SingleOrDefault(n => n.idnxb == id);
            ViewBag.idnxb = nxb.idnxb;
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nxb);
        }
        [HttpPost, ActionName("XoaNhaXuatBan")]
        public ActionResult Xacnhanxoanxb(int id)
        {
            //lay ra doi tuong 
            nxb nxb = data.nxbs.SingleOrDefault(n => n.idnxb == id);
            ViewBag.idnxb = nxb.idnxb;
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.nxbs.DeleteOnSubmit(nxb);
            data.SubmitChanges();
            return RedirectToAction("NhaXuatBan");
        }

        //Chinh sua nxb
        [HttpGet]
        public ActionResult SuaNhaXuatBan(int id)
        {
            //lay ra doi tuong can 
            var cd = data.nxbs.First(n => n.idnxb == id);
            return View(cd);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaNhaXuatBan(int id, FormCollection f)
        {
            var nxb = data.nxbs.First(n => n.idnxb == id);
            var cd = f["tennxb"];
            nxb.idnxb = id;
            if (string.IsNullOrEmpty(cd))
            {
                ViewData["Loi"] = "Không được để trống";
            }
            else
            {
                nxb.tennxb = cd;
                UpdateModel(nxb);
                data.SubmitChanges();
            }
            return RedirectToAction("NhaXuatBan");
        }

        //Bây giờ tới tác giả
        public ActionResult TacGia(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(data.tacgias.ToList().OrderBy(n => n.idtacgia).ToPagedList(pageNumber, pageSize));
        }

        //Tao moi tac gia
        [HttpGet]
        public ActionResult ThemMoiTacGia()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiTacGia(tacgia tg)
        {
            if (tg.hotentg == null)
            {
                ViewBag.Thongbao = "Không được để trống";
            }
            data.tacgias.InsertOnSubmit(tg);
            data.SubmitChanges();
            return RedirectToAction("TacGia");
        }

        //Xoa tac gia
        [HttpGet]
        public ActionResult XoaTacGia(int id)
        {
            //lay ra doi tuong can xoa
            tacgia cd = data.tacgias.SingleOrDefault(n => n.idtacgia == id);
            ViewBag.idtacgia = cd.idtacgia;
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(cd);
        }
        [HttpPost, ActionName("XoaTacGia")]
        public ActionResult Xacnhanxoatg(int id)
        {
            //lay ra doi tuong 
            tacgia cd = data.tacgias.SingleOrDefault(n => n.idtacgia == id);
            ViewBag.macd = cd.idtacgia;
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.tacgias.DeleteOnSubmit(cd);
            data.SubmitChanges();
            return RedirectToAction("TacGia");
        }

        //Chinh sua tac gia
        [HttpGet]
        public ActionResult SuaTacGia(int id)
        {
            //lay ra doi tuong can 
            var cd = data.tacgias.First(n => n.idtacgia == id);
            return View(cd);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaTacGia(int id, FormCollection f)
        {
            var tacgia = data.tacgias.First(n => n.idtacgia == id);
            var cd = f["hotentg"];
            var ts = f["tieusu"];
            tacgia.idtacgia = id;
            if (string.IsNullOrEmpty(cd))
            {
                ViewData["Loi"] = "Không được để trống";
            }
            else
            {
                tacgia.hotentg = cd;
                tacgia.tieusu = ts;
                UpdateModel(tacgia);
                data.SubmitChanges();
            }
            return RedirectToAction("TacGia");
        }

        //một số thành phần phụ khác
        public ActionResult Lienhe(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(data.lienhes.ToList().OrderBy(n => n.malienhe).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Donhang(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(data.donhangs.ToList().OrderBy(n => n.iddh).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Chitietdonhang(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(data.chitietdhs.ToList().OrderBy(n => n.iddh).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Khachhang(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(data.khachhangs.ToList().OrderBy(n => n.idkh).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Quantri(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(data.QuanTris.ToList().OrderBy(n => n.idadmin).ToPagedList(pageNumber, pageSize));
        }


        public ActionResult LienHeKhachhang()
        {
            return View();
        }
          
        
    }
}