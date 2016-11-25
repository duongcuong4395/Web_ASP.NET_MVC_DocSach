using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDocSach.Models;

using PagedList;
using PagedList.Mvc;

namespace WebsiteDocSach.Controllers.NguoiDungControllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem

        QLBanSachDataContext data = new QLBanSachDataContext();
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult KetQuaTimKiem(FormCollection frmCollection, int? page)
        {
            try
            {
                string chuoiTimKiem = frmCollection["txtTimKiem"].ToString();
                List<sach> lstHangHoa = data.saches.Where(n => n.tensach.Contains(chuoiTimKiem) || n.mota.Contains(chuoiTimKiem)).OrderByDescending(n => n.gia).ToList();
                //Phan trang
                int pageNumber = (page ?? 1);
                int pageSize = 9;

                ViewBag.chuoiTimKiem = chuoiTimKiem;
                //neu ket qua ko tim thay hang
                if (lstHangHoa.Count == 0)
                {
                    ViewBag.ThongBao = "Không tìm thấy hàng hóa nào.";
                }
                return View(lstHangHoa.OrderBy(n => n.gia).ToPagedList(pageNumber, pageSize));
            }
            catch(Exception error)
            {
                ViewBag.ThongBao = "Không tìm thấy hàng hóa nào.";
                return RedirectToAction("Index", "SachStore");
            } 
        }

        [HttpGet, ValidateInput(false)]
        public ActionResult KetQuaTimKiem(int? page, string chuoiTimKiem = " ")
        {
            try
            {
                ViewBag.chuoiTimKiem = chuoiTimKiem;
                List<sach> lstHangHoa = data.saches.Where(n => n.tensach.Contains(chuoiTimKiem) || n.mota.Contains(chuoiTimKiem)).ToList();

                //Phan trang
                int pageNumber = (page ?? 1);
                int pageSize = 9;

                //neu ket qua ko tim thay hang
                if (lstHangHoa.Count == 0)
                {
                    ViewBag.ThongBao = "Không tìm thấy hàng hóa nào.";
                }

                ViewBag.ThongBao = chuoiTimKiem;
                return View(lstHangHoa.OrderBy(n => n.gia).ToPagedList(pageNumber, pageSize)); 
            }
            catch(Exception error)
            {
                return RedirectToAction("Index", "SachStore");
            }
        }

       
        public ActionResult TimKiemTheoGia()
        {
            return PartialView();
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult TimKiemTheoGiaMoi(int? page, FormCollection formTimKiemCollection)
        {
            try
            {
                //Phan trang
                int pageNumber = (page ?? 1);
                int pageSize = 6;
                int gianho = 0;
                int giaLon = 0;

                var GiaNho = formTimKiemCollection["GiaNho"];
                var GiaLon = formTimKiemCollection["GiaLon"];
                if (GiaNho == "")
                {
                    gianho = 0;
                }
                else
                {
                    gianho = int.Parse(GiaNho.ToString()) * 1000;
                }

                if (GiaLon == "")
                {
                    giaLon = 0;
                }
                else
                {
                    giaLon = int.Parse(GiaLon.ToString()) * 1000;
                }
                List<sach> lstSach = data.saches.Where(n => n.GiaMoi >= gianho && n.GiaMoi <= giaLon).ToList();
                ViewBag.giaLon = GiaLon;
                ViewBag.gianho = GiaNho;
                if (giaLon < gianho)
                {
                    ViewBag.sanpham = "Giá sau phải nhỏ hơn hoặc bằng giá trước.";
                }
                else
                {
                    if (lstSach == null || lstSach.Count() == 0)
                    {
                        ViewBag.sanpham = "Không có Sách nào có giá từ " + String.Format("{0:0,0}", gianho) + " VND tới " + String.Format("{0:0,0}", giaLon) + " VND.";
                    }
                    else
                    {
                        ViewBag.sanpham = "Sách có giá từ " + String.Format("{0:0,0}", gianho) + " VND tới " + String.Format("{0:0,0}", giaLon) + " VND.";
                    }
                }
                return View(lstSach.OrderBy(n => n.GiaMoi).ToPagedList(pageNumber, pageSize));
            }
            catch(Exception error)
            {
                return RedirectToAction("Index", "SachStore");
            }
        }

        [HttpGet, ValidateInput(false)]
        public ActionResult TimKiemTheoGiaMoi(int? page, string gianho = " ", string gialon = " ")
        {
            try
            {
                //Phan trang
                int pageNumber = (page ?? 1);
                int pageSize = 6;
                int gn = 0;
                int gl = 0;
                gn = int.Parse(gianho) * 1000;
                gl = int.Parse(gialon) * 1000;

                List<sach> lstSach = data.saches.Where(n => n.GiaMoi >= gn && n.GiaMoi <= gl).ToList();
                ViewBag.giaLon = gialon;
                ViewBag.gianho = gianho;
                if (gl < gn)
                {
                    ViewBag.sanpham = "Giá sau phải nhỏ hơn hoặc bằng giá trước.";
                }
                else
                {
                    if (lstSach == null || lstSach.Count() == 0)
                    {
                        ViewBag.sanpham = "Không có sách nào có giá từ " + String.Format("{0:0,0}", gn) + " VND tới " + String.Format("{0:0,0}", gl) + " VND.";
                    }
                    else
                    {
                        ViewBag.sanpham = "Sách có giá từ " + String.Format("{0:0,0}", gn) + " VND tới " + String.Format("{0:0,0}", gl) + " VND.";
                    }
                }
                return View(lstSach.OrderBy(n => n.GiaMoi).ToPagedList(pageNumber, pageSize));
            }
            catch (Exception error)
            {
                return RedirectToAction("Index", "SachStore");
            }
        }
    }
}