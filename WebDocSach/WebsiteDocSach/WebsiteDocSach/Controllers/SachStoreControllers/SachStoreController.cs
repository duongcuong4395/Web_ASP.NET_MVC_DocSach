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
    public class SachStoreController : Controller
    {
        // GET: SachStore
        QLBanSachDataContext data = new QLBanSachDataContext();
        public ActionResult Index(int? page)
        {
            try
            {
                //kiem tra da co data trong table nguoi xem ko
                if (data.NguoiXems.Where(n => n.NgayXem == null).Take(1) == null)
                {
                    NguoiXem nx = new NguoiXem();
                    nx.SoNguoiXem = 1;
                    nx.NgayXem = String.Format("{0:dd/MM/yyyy}", DateTime.Now);
                    data.NguoiXems.InsertOnSubmit(nx);
                    data.SubmitChanges();
                }
                else
                {
                    string ngayhientai = String.Format("{0:dd/MM/yyyy}", DateTime.Now);
                    NguoiXem nx = data.NguoiXems.Where(n => n.NgayXem == ngayhientai).FirstOrDefault();
                    if (nx != null)
                    {
                        var songuoixemhientai = nx.SoNguoiXem;
                        int snht = int.Parse(songuoixemhientai.ToString());
                        int snx = snht + int.Parse(Session["online"].ToString());
                        nx.SoNguoiXem = snx;
                        nx.NgayXem = nx.NgayXem;
                        UpdateModel(nx);
                        data.SubmitChanges();
                    }
                    else
                    {
                        NguoiXem nguoixem = new NguoiXem();
                        nguoixem.SoNguoiXem = 1;
                        nguoixem.NgayXem = String.Format("{0:dd/MM/yyyy}", DateTime.Now);
                        data.NguoiXems.InsertOnSubmit(nguoixem);
                        data.SubmitChanges();
                    }
                }

                int pagesize = 6; 
                int pagenum = (page ?? 1);
                var sachmoi = Laysachmoi(9);
                return View(sachmoi.ToPagedList(pagenum, pagesize));
            }
            catch (Exception error)
            {
                ViewBag.ThongBao = " Hệ thống đang quá tải nên chúng tôi sẽ dời bạn đến trang đăng nhập";
                return RedirectToAction("chon", "NguoiDung");
            } 
        } 

        public ActionResult ThemSPYeuThich(int idsach, string strURL)
        {
            if (Session["Taikhoan"] != null)
            {
                int makh = data.khachhangs.Where(n => n.taikhoan == (Session["TenTK"]).ToString()).FirstOrDefault().idkh;
                DSSachYeuThich dsYeuThich = data.DSSachYeuThiches.Where(n => n.idsach == idsach && n.idkh == makh).FirstOrDefault();
                if (dsYeuThich == null)
                {
                    DSSachYeuThich danhsach = new DSSachYeuThich();
                    danhsach.idsach = idsach;
                    danhsach.idkh = makh;
                    data.DSSachYeuThiches.InsertOnSubmit(danhsach);
                    data.SubmitChanges();
                    int sl = data.DSSachYeuThiches.Where(n => n.idkh == makh).Count();
                    Session["makh"] = sl;
                    ViewBag.sl = Session["makh"];
                    ViewBag.ThongBao = "Đã thêm sản phẩm vào mục yêu thích.";
                }
                else
                {
                    ViewBag.ThongBao = "Sản phẩm này đã có trong danh mục yêu thích.";
                }
                return Redirect(strURL);
            }
            else
            {
                return RedirectToAction("chon", "NguoiDung");
            }
        }


        public ActionResult XoaSPYeuThich(int idsach, string strURL)
        {
            if (Session["Taikhoan"] != null)
            {
                int makh = data.khachhangs.Where(n => n.taikhoan == (Session["TenTK"]).ToString()).FirstOrDefault().idkh;
                DSSachYeuThich dsYeuThich = data.DSSachYeuThiches.Where(n => n.idsach == idsach && n.idkh == makh).FirstOrDefault();
                string tenhang = data.saches.Where(n => n.idsach == idsach).FirstOrDefault().tensach;

                if (dsYeuThich == null)
                {
                    ViewBag.ThongBao = "Sản phẩm này không nằm trong danh sách thích.";
                }
                else
                {
                    ViewBag.ThongBao = "Đã xóa sản phẩm: " + tenhang + " trong danh mục yêu thích.";
                    data.DSSachYeuThiches.DeleteOnSubmit(dsYeuThich);
                    data.SubmitChanges();
                    int sl = data.DSSachYeuThiches.Where(n => n.idkh == makh).Count();
                    Session["makh"] = sl;
                    ViewBag.sl = Session["makh"];
                }
                return Redirect(strURL);
            }
            else
            {
                return RedirectToAction("chon", "NguoiDung");
            }
        }

        [HttpGet]
        public ActionResult DShangYeuThich(int? page)
        {
            try
            {
                //Tạo biến Quy định sô sản phẩm trên 1 trang
                int pagesize = 9;
                //Tạo biến số sang
                int pagenum = (page ?? 1);

                int makh = data.khachhangs.Where(n => n.taikhoan == (Session["TenTK"]).ToString()).FirstOrDefault().idkh;
                var layhang = from dsthich in data.DSSachYeuThiches
                              join hanghoa in data.saches on dsthich.idsach equals hanghoa.idsach
                              where (dsthich.idkh == makh)
                              select hanghoa;
                if (layhang.Count() == 0)
                {
                    ViewBag.ThongBao = "Bạn vẫn chưa yêu thích sản phẩm nào.";
                }
                else
                {
                    ViewBag.ThongBao = "Sản phẩm bạn đã thích.";
                }
                return View(layhang.ToPagedList(pagenum, pagesize));
            }
            catch (Exception error)
            {
                return RedirectToAction("Index", "SachStore", false);
            }
           
        } 

        private List<sach> Laysachmoi(int count)
        {
            //Sap xe giam theo ngay cap nhat
            return data.saches.OrderByDescending(a => a.ngaycapnhat).Take(count).ToList();
        }

        private List<sach> LaySachMienPhi(int count)
        {
            //Sap xe giam theo ngay cap nhat
            return data.saches.Where(n => n.GiaMoi == 0).Take(count).ToList();
        }

        public ActionResult SachMienPhi()
        {
            var sachMienPhi = LaySachMienPhi(8);
            if (sachMienPhi == null)
            {
                ViewBag.thongbao = "Không có sách miễn phí trong kho.";
            }
            else
            {
                ViewBag.thongbao = " ";
            }
            return PartialView(sachMienPhi);
        }

        [HttpGet]
        public ActionResult LienHeCuaHang()
        {
            return View(data.lienhes.ToList());
        }


        public ActionResult soLuongNguoiXem()
        {
            try
            {
                var soluong = data.NguoiXems.Sum(row => row.SoNguoiXem);
                ViewBag.thongbao1 = "Số người đang Onlines: ";
                ViewBag.thongbao2 = "Lược người vào websites:";
                ViewBag.songuoidangonline = Session["count"].ToString();
                ViewBag.soNguoiOnline = soluong;
            }
            catch (Exception error)
            {
                ViewBag.thongbao2 = " ";
                ViewBag.soNguoiOnline = " ";
            }
            return PartialView();
        }
    }
}