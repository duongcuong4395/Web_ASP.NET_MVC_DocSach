using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDocSach.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebsiteDocSach.Controllers.HomeQuanTriController
{
    public class HomeController : Controller
    {
        QLBanSachDataContext db = new QLBanSachDataContext();
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sach(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(db.saches.ToList().OrderBy(n => n.idsach).ToPagedList(pageNumber, pageSize));
        }

        //Tao moi sach
        [HttpGet]
        public ActionResult Taomoisach()
        {
            ViewBag.idnxb = new SelectList(db.nxbs.ToList().OrderBy(n => n.tennxb), "idnxb", "tennxb");
            ViewBag.idchude = new SelectList(db.chudes.ToList().OrderBy(n => n.tenchude), "idchude", "tenchude");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Taomoisach(sach sach, HttpPostedFileBase fileupload)
        {
            ViewBag.maloai = new SelectList(db.nxbs.ToList().OrderBy(n => n.tennxb), "idnxb", "tennxb");
            ViewBag.macd = new SelectList(db.chudes.ToList().OrderBy(n => n.tenchude), "idchude", "tenchude");
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
                    var path = Path.Combine(Server.MapPath("~/Anhbia"), fileName);
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
                    db.saches.InsertOnSubmit(sach);
                    db.SubmitChanges();
                }
                return RedirectToAction("Sach");
            }
        }
        //Chi tiet sach
        public ActionResult Chitietsach(int id)
        {
            //lay ra doi tuong theo ma
            sach sach = db.saches.SingleOrDefault(n => n.idsach == id);
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
        public ActionResult Xoasach(int id)
        {
            //lay ra doi tuong theo ma
            sach sach = db.saches.SingleOrDefault(n => n.idsach == id);
            ViewBag.idsach = sach.idsach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        [HttpPost, ActionName("Xoasach")]
        public ActionResult Xacnhanxoasach(int id)
        {
            //lay ra doi tuong theo ma
            sach sach = db.saches.SingleOrDefault(n => n.idsach == id);
            ViewBag.idsach = sach.idsach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.saches.DeleteOnSubmit(sach);
            db.SubmitChanges();
            return RedirectToAction("sach");
        }
        //Chinh sua sach
        [HttpGet]
        public ActionResult Suasach(int id)
        {
            //lay ra doi tuong theo ma
            sach sach = db.saches.SingleOrDefault(n => n.idsach == id);
            ViewBag.idsach = sach.idsach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.idnxb = new SelectList(db.nxbs.ToList().OrderBy(n => n.tennxb), "idnxb", "tennxb");
            ViewBag.idchude = new SelectList(db.chudes.ToList().OrderBy(n => n.tenchude), "idchude", "tenchude");
            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasach(sach s, HttpPostedFileBase fileupload, FormCollection f)
        {
            ViewBag.idnxb = new SelectList(db.nxbs.ToList().OrderBy(n => n.tennxb), "idnxb", "tennxb");
            ViewBag.idchude = new SelectList(db.chudes.ToList().OrderBy(n => n.tenchude), "idchude", "tenchude");
            sach smoi = db.saches.First(n => n.idsach == s.idsach);
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
                    var path = Path.Combine(Server.MapPath("~/Anhbia"), fileName);
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
                    db.SubmitChanges();
                }
                return RedirectToAction("sach");
            }
        }
        public ActionResult Chude(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(db.chudes.ToList().OrderBy(n => n.idchude).ToPagedList(pageNumber, pageSize));
        }
        //Tao moi chu de
        [HttpGet]
        public ActionResult Taomoicd()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Taomoicd(chude cd)
        {
            if (cd.tenchude == null)
            {
                ViewBag.Thongbao = "Không được để trống";
            }
            db.chudes.InsertOnSubmit(cd);
            db.SubmitChanges();
            return RedirectToAction("Chude");
        }
        //Xoa chu de
        [HttpGet]
        public ActionResult Xoacd(int id)
        {
            //lay ra doi tuong can xoa
            chude cd = db.chudes.SingleOrDefault(n => n.idchude == id);
            ViewBag.macd = cd.idchude;
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(cd);
        }
        [HttpPost, ActionName("Xoacd")]
        public ActionResult Xacnhanxoacd(int id)
        {
            //lay ra doi tuong 
            chude cd = db.chudes.SingleOrDefault(n => n.idchude == id);
            ViewBag.macd = cd.idchude;
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.chudes.DeleteOnSubmit(cd);
            db.SubmitChanges();
            return RedirectToAction("Chude");
        }

        //Sua chu de
        [HttpGet]
        public ActionResult Suacd(int id)
        {
            //lay ra doi tuong can 
            var cd = db.chudes.First(n => n.idchude == id);
            return View(cd);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suacd(int id, FormCollection f)
        {
            var chude = db.chudes.First(n => n.idchude == id);
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
                db.SubmitChanges();
            }
            return RedirectToAction("Chude");
        }
        public ActionResult Nxb(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(db.nxbs.ToList().OrderBy(n => n.idnxb).ToPagedList(pageNumber, pageSize));
        }
        //Tao moi nxb
        [HttpGet]
        public ActionResult Taomoinxb()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Taomoinxb(nxb nxb)
        {
            if ( nxb== null)
            {
                ViewBag.Thongbao = "Không được để trống";
            }
            db.nxbs.InsertOnSubmit(nxb);
            db.SubmitChanges();
            return RedirectToAction("Nxb");
        }
        //Xoa nxb
        [HttpGet]
        public ActionResult Xoanxb(int id)
        {
            //lay ra doi tuong can xoa
            nxb nxb = db.nxbs.SingleOrDefault(n => n.idnxb == id);
            ViewBag.idnxb = nxb.idnxb;
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nxb);
        }
        [HttpPost, ActionName("Xoanxb")]
        public ActionResult Xacnhanxoanxb(int id)
        {
            //lay ra doi tuong 
            nxb nxb = db.nxbs.SingleOrDefault(n => n.idnxb == id);
            ViewBag.idnxb = nxb.idnxb;
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.nxbs.DeleteOnSubmit(nxb);
            db.SubmitChanges();
            return RedirectToAction("Nxb");
        }
        //Chinh sua nxb
        [HttpGet]
        public ActionResult Suanxb(int id)
        {
            //lay ra doi tuong can 
            var cd = db.nxbs.First(n => n.idnxb == id);
            return View(cd);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suanxb(int id, FormCollection f)
        {
            var nxb = db.nxbs.First(n => n.idnxb == id);
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
                db.SubmitChanges();
            }
            return RedirectToAction("nxb");
        }
        public ActionResult Tacgia(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(db.tacgias.ToList().OrderBy(n => n.idtacgia).ToPagedList(pageNumber, pageSize));
        }
        //Tao moi tac gia
        [HttpGet]
        public ActionResult Taomoitg()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Taomoitg(tacgia tg)
        {
            if (tg.hotentg == null)
            {
                ViewBag.Thongbao = "Không được để trống";
            }
            db.tacgias.InsertOnSubmit(tg);
            db.SubmitChanges();
            return RedirectToAction("Tacgia");
        }
        //Xoa tac gia
        [HttpGet]
        public ActionResult Xoatg(int id)
        {
            //lay ra doi tuong can xoa
            tacgia cd = db.tacgias.SingleOrDefault(n => n.idtacgia == id);
            ViewBag.idtacgia = cd.idtacgia;
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(cd);
        }
        [HttpPost, ActionName("Xoatg")]
        public ActionResult Xacnhanxoatg(int id)
        {
            //lay ra doi tuong 
            tacgia cd = db.tacgias.SingleOrDefault(n => n.idtacgia == id);
            ViewBag.macd = cd.idtacgia;
            if (cd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.tacgias.DeleteOnSubmit(cd);
            db.SubmitChanges();
            return RedirectToAction("Tacgia");
        }
        //Chinh sua tac gia
        [HttpGet]
        public ActionResult Suatg(int id)
        {
            //lay ra doi tuong can 
            var cd = db.tacgias.First(n => n.idtacgia == id);
            return View(cd);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suatg(int id, FormCollection f)
        {
            var tacgia = db.tacgias.First(n => n.idtacgia == id);
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
                db.SubmitChanges();
            }
            return RedirectToAction("Tacgia");
        }
        public ActionResult Lienhe(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(db.lienhes.ToList().OrderBy(n => n.malienhe).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Donhang(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(db.donhangs.ToList().OrderBy(n => n.iddh).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Chitietdonhang(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(db.chitietdhs.ToList().OrderBy(n => n.iddh).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Khachhang(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(db.khachhangs.ToList().OrderBy(n => n.idkh).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Quantri(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(db.QuanTris.ToList().OrderBy(n => n.idadmin).ToPagedList(pageNumber, pageSize));
        }
    }
}