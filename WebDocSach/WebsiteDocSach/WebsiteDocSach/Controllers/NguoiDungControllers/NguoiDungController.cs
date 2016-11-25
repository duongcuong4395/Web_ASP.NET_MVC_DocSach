using BotDetect.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebsiteDocSach.Models;
using System.Configuration;

//thuw vieenj dungf cho recaptcha
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Mail;

namespace WebsiteDocSach.Controllers.NguoiDungControllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung

        QLBanSachDataContext data = new QLBanSachDataContext();
         

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult chon(string thongbao)
        { 
            switch (thongbao)
            { 
                case "Trung-TK-Email":
                    {
                        ViewBag.chungchung = "* Tên đăng nhập hoặc mật khẩu đã tồn tại."; 
                        break;
                    }
                case "TK-Dang-Hoat-Dong":
                    {
                        ViewBag.chungchung = "* Tài khoản của bạn đang hoạt động, không thể đăng ký."; 
                        break;
                    }
                case "Sai-MK-Xac-Nhan":
                    {
                        ViewBag.chungchung = "* Nhập lại mật khẩu phải giống nhau."; 
                        break;
                    }
                case "Ten-TK-Khong-Ton-Tai":
                    {
                        ViewBag.chungchung = "* Tên đăng nhập không đúng.";
                        break;
                    }
                case "Sai-MK-Hien-Tai":
                    {
                        ViewBag.chungchung = "* Nhập sai mật khẩu hiện tại.";
                        break;
                    }
                case "Sai-TDN-MK":
                    {
                        ViewBag.chungchung = "* Tên đăng nhập hoặc mật khẩu không đúng";
                        break;
                    }
                case "Doi-MK-Thanh-Cong":
                    {
                        ViewBag.chungchung = "***** Bạn đã đổi mật khẩu thành công. ******";
                        break;
                    }
                case "Chua-Dang-Nhap":
                    {
                        ViewBag.chungchung = "* Bạn vẫn chưa đăng nhập.";
                        break;
                    }
                case "Sai-Thong-Tin":
                    {
                        ViewBag.chungchung = "* Nhập sai thông tin.";
                        break;
                    }
                case "Chieu-Dai-Ky-Tu":
                    {
                        ViewBag.chungchung = "* Độ dài 1 số chỗ vẫn chưa phù hợp.";
                        break;
                    }
                default :
                    {
                        ViewBag.chungchung = "******** Chào Mừng bạn tới trang Đăng ký/Đăng nhập  ***********"; 
                        break;
                    }
            }

            return View();
        } 

        //Post: Hàm DangKy(Post) nhận dữ liệu từ trang đăng ký và trhuwcj hiện việc tạo mới dữ liệu
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)] 
        public ActionResult DangKyNguoiDung(FormCollection formdangkyCollection, khachhang kh)
        {
            try
            {
                if (Session["Taikhoan"] == null)
                {
                    if (ModelState.IsValid)
                    {
                        //gán các giá trị ngừoi dùng nhập liệu cho cấc biến.
                        var Ho = formdangkyCollection["Ho"];
                        var Ten = formdangkyCollection["Ten"];
                        var HoTen = Ho + " " + Ten;
                        var TaiKhoan = formdangkyCollection["TenDangNhap"];
                        var MatKhau = formdangkyCollection["MatKhau"];
                        var MatKhauXacNhan = formdangkyCollection["NhapLaiMatKhau"];
                        var Email = formdangkyCollection["Email"];
                        var DienThoai = formdangkyCollection["SDT"];
                        var DiaChi = formdangkyCollection["DiaChi"];
                        var NgaySinh = formdangkyCollection["NgaySinh"];
                        var GioiTinh = formdangkyCollection["GioiTinh"];

                        //Kiểm tra lỗi khi đăng ký
                        if (data.khachhangs.Where(m => kh.taikhoan == TaiKhoan || m.email == Email).FirstOrDefault() != null)
                        {  
                            return RedirectToAction("chon", "NguoiDung", new { thongbao = "Trung-TK-Email" });
                        }

                        //kiểm tra mat khau vs mat khau nhap lai co trung nhau khong 
                        else if (MatKhau != MatKhauXacNhan)
                        {  
                            return RedirectToAction("chon", "NguoiDung", new { thongbao = "Sai-MK-Xac-Nhan" });
                        }

                        else if (MatKhau.Length < 6 || MatKhauXacNhan.Length < 6 || DiaChi.Length < 6 || DienThoai.Length < 6 || Email.Length < 6)
                        {
                            return RedirectToAction("chon", "NguoiDung", new { thongbao = "Chieu-Dai-Ky-Tu" });
                        }

                        else
                        { 
                            string key = KeyRandom.LayKey(6);
                            string mkm = MatKhau.ToString() + key;
                            string mk = MaHoaMD5(mkm);
                            kh.hotenkh = HoTen;
                            kh.taikhoan = TaiKhoan;
                            kh.password = mk;
                            kh.dienthoaikh = DienThoai;
                            kh.email = Email;
                            kh.diachi = DiaChi;
                            kh.ngaysinh = DateTime.Parse(NgaySinh.ToString());
                            kh.gioitinh = GioiTinh;
                            kh.maKHRieng = key;
                            kh.idQuyen = 7;
                            data.khachhangs.InsertOnSubmit(kh);
                            data.SubmitChanges();
                            Session["Taikhoan"] = kh;
                            Session["TenKhachHang"] = kh.hotenkh;
                            Session["TenTK"] = kh.taikhoan;
                            Session["MaTKKH"] = kh.idkh;
                            Session["EmailKH"] = kh.email;
                            return RedirectToAction("Index", "SachStore");
                        }
                    }
                }
                else
                {  
                    return RedirectToAction("chon", "NguoiDung", new {thongbao = "TK-Dang-Hoat-Dong" });
                }
                return RedirectToAction("chon", "NguoiDung", new { thongbao = "TK-Dang-Hoat-Dong" });
            }
            catch(Exception error)
            {
                return RedirectToAction("Chon", "NguoiDung", new { thongbao = "Sai-Thong-Tin" });
            } 
        } 


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhapmoi(FormCollection formDangNhapCollection)
        {
            try
            {
                if (Session["Taikhoan"] == null)
                {
                    if (ModelState.IsValid)
                    {
                        var tendangnhap = formDangNhapCollection["TenDangNhap"];
                        var matkhau = formDangNhapCollection["MatKhau"];

                        khachhang kh = data.khachhangs.Where(n => n.taikhoan == tendangnhap).FirstOrDefault();
                        if(kh == null)
                        { 
                            return RedirectToAction("chon", "NguoiDung", new { thongbao = "Ten-TK-Khong-Ton-Tai" });
                        }
                        else
                        {
                            string mkmmoi = matkhau.ToString() + kh.maKHRieng;
                            string mkmahoa = MaHoaMD5(mkmmoi);
                            khachhang kachmoi = data.khachhangs.Where(n => n.taikhoan == tendangnhap && n.password == mkmahoa).FirstOrDefault();
                            if (tendangnhap == null)
                            {
                                ViewData["txtTenDangNhap"] = "Tên đăng nhập không bỏ trống.";
                            }
                            else if (matkhau == null)
                            {
                                ViewData["txtMatKhau"] = "Mật khẩu không được bỏ trống.";
                            }
                            else if (kachmoi != null)
                            {
                                Session["Taikhoan"] = kachmoi;
                                Session["TenKhachHang"] = kachmoi.hotenkh;
                                Session["TenTK"] = kachmoi.taikhoan;
                                Session["MaTKKH"] = kachmoi.idkh; 
                                Session["EmailKH"] = kachmoi.email;
                                int sl = data.DSSachYeuThiches.Where(n => n.idkh == kachmoi.idkh).Count();
                                Session["makh"] = sl;
                                return RedirectToAction("Index", "SachStore");
                            }
                            else
                            { 
                                return RedirectToAction("chon", "NguoiDung", new { thongbao = "Sai-TDN-MK" });
                            }
                        } 
                    }
                }
                else
                { 
                    return RedirectToAction("chon", "NguoiDung", new { thongbao = "TK-Dang-Hoat-Dong" });
                }
                return RedirectToAction("chon", "NguoiDung", new { thongbao = "TK-Dang-Hoat-Dong" });
            }
            catch
            {
                return RedirectToAction("Chon", "NguoiDung", new { thongbao = "Sai-Thong-Tin" });
            }
        }

        [HttpGet] 
        public ActionResult QuenMatKhau()
        {
            
            if (Session["Taikhoan"] != null)
            {
                ViewBag.chung = "Tài khoản của bạn đang được sử dụng ko thể thực hiện QUÊN MẬT KHẨU nữa.";
                return RedirectToAction("Index", "SachStore");
            }
            else
            {
                ViewBag.chung = "Mời bạn chọn(click) vào ô ĐĂNG NHẬP.";
                return View();
            }  
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken] 
        public ActionResult QuenMatKhau(FormCollection formQuenMatKhauCollection)
        {
            try
            {
                if (Session["Taikhoan"] == null)
                {  
                    if (ModelState.IsValid )
                    { 
                        var tendangnhap = formQuenMatKhauCollection["TenDangNhap"];
                        var Email = formQuenMatKhauCollection["Email"];
                        ViewBag.matkhaumoi = " "; 

                        khachhang kh = data.khachhangs.Where(n => n.taikhoan == tendangnhap && n.email == Email).FirstOrDefault();
                        if (kh != null )
                        {
                            
                            //tạo ra một mật khẩu với 6 ký tự
                            string key = KeyRandom.LayKey(6);
                            //gép 2 key vs ma rieng cua khách hàng
                            string mkmmoi = key + kh.maKHRieng;
                            //mã hóa md5 
                            string mahoamatkhau = MaHoaMD5(mkmmoi);

                            ViewBag.matkhaumoi = key;
                            ViewBag.Thongbao = "Hệ thống đang Xác nhận tài khoản.....";
                            ViewBag.tb = "Mật khẩu của bạn đã được gửi trong mail.";
                            ViewBag.Thongbao2 = "Bạn Có thể kiểm tra mail của mình tại";
                            ViewBag.thongbao3 = "Đây";
                            ViewBag.mailKH = "";
                             
                            kh.password = mahoamatkhau;
                            UpdateModel(kh);
                            data.SubmitChanges(); 

                            string content = System.IO.File.ReadAllText(Server.MapPath("/Views/NguoiDung/GuiMailPage.html"));
                            content = content.Replace("{{TenKhachHang}}", kh.hotenkh);
                            //content = content.Replace("{{KhachHang}}", tendangnhap);
                            content = content.Replace("{{MatKhauMoi}}", key);

                            new GuiMail().GuiMailKH(Email, "Quên Mật khẩu", content);
                            
                        }
                        else
                        {
                            ViewBag.chung = "Tên đăng nhập hoặc Email không đúng";
                        }
                    }
                }
                else
                {
                    ViewBag.ThongBao = "Tài khoản của bạn đang hoạt động."; 
                }
                return this.QuenMatKhau();
            }
            catch
            {
                ViewBag.chung = "Một số chỗ vẫn chưa nhập thông tin.";
                return this.QuenMatKhau();
            }
            
        }



        [HttpGet]
        public ActionResult DoiMatKhau()
        {
            if (Session["Taikhoan"] != null)
            {
                ViewBag.chung = "Mời bạn chọn(click) vào ô ĐỔI MẬT KHẨU.";
                return RedirectToAction("Chon", "NguoiDung");
            }
            else
            {
                ViewBag.chung = "Bạn chưa đăng nhập tài khoản thì không thể đổi mật khẩu.";
                 
                return RedirectToAction("Chon", "NguoiDung");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DoiMatKhau(FormCollection formDoiMatKhauCollection)
        {
            try
            {
                if (Session["Taikhoan"] != null)
                {
                    var MatKhauCu = formDoiMatKhauCollection["MatKhauCu"];
                    var MatKhauMoi = formDoiMatKhauCollection["MatKhauMoi"];
                    var XacNhanMatKhau = formDoiMatKhauCollection["XacNhanMatKhau"]; 
                    
                    string mkm = MatKhauMoi.ToString(); 
                    //lấy tên tài khoản
                    string tk = Session["TenTK"].ToString();
                    //lấy tài khoản có tk == tentai koan
                    khachhang khhientai = data.khachhangs.Where(n => n.taikhoan == tk).FirstOrDefault();
                    //ghép mk nhập vào vs maKHRieng của khách hàng
                    string key = MatKhauCu + khhientai.maKHRieng;
                    //mã hóa
                    string mkmahoa = MaHoaMD5(key);
                    //lấy tài khoản có tk == tentai koan vs mk == mk mã hóa
                    khachhang kh = data.khachhangs.Where(n => n.taikhoan == tk && n.password == mkmahoa).FirstOrDefault();
                    //kiểm tra null
                    if (kh == null)
                    { 
                        return RedirectToAction("Chon", "NguoiDung", new { thongbao = "Sai-MK-Hien-Tai" });
                    }
                    else
                    {
                        //kiểm tra mk vs mat khau nhap lai
                        if (MatKhauMoi != XacNhanMatKhau)
                        { 
                            return RedirectToAction("Chon", "NguoiDung", new { thongbao = "Sai-MK-Xac-Nhan" });
                        }
                        else
                        {
                            if (ModelState.IsValid)
                            {
                                //Tạo ra 1 key với 4 ký tự bất kỳ.
                                string keymoi = KeyRandom.LayKey(4);
                                //ghép mk mới vs key mới
                                string mk2 = MatKhauMoi + keymoi;
                                //mã hóa
                                string mk = MaHoaMD5(mk2);
                                //gán giá trị
                                kh.password = mk;
                                kh.maKHRieng = keymoi;
                                UpdateModel(kh);
                                //lưu xuống database
                                data.SubmitChanges(); 
                                return RedirectToAction("Chon", "NguoiDung", new { thongbao = "Doi-MK-Thanh-Cong" });
                            }
                        } 
                    } 
                }
                else
                { 
                    return RedirectToAction("Chon", "NguoiDung", new { thongbao = "Chua-Dang-Nhap" });
                }
                return RedirectToAction("Chon", "NguoiDung", new { thongbao = "Chua-Dang-Nhap" });
            }
            catch (Exception error)
            { 
                return RedirectToAction("Chon", "NguoiDung", new { thongbao = "Sai-Thong-Tin" });
            } 
        }

        


        public ActionResult DangXuat()
        {
            Session.Abandon();
            return RedirectToAction("Index", "SachStore");
        }
         
        //hash chuoi ra md5
        public string MaHoaMD5(string chuoi)
        {
            string str_md5 = "";
            byte[] mang = System.Text.Encoding.UTF8.GetBytes(chuoi); 
            MD5CryptoServiceProvider my_md5 = new MD5CryptoServiceProvider();
            mang = my_md5.ComputeHash(mang); 
            foreach (byte b in mang)
            {
                str_md5 += b.ToString("X2");
            } 
            return str_md5;
        }

        public ActionResult taikhoanPartial()
        { 
            ViewBag.sl = Session["makh"];
            return PartialView();
        }

        private int laySoLuongThich(int idkh)
        {
            var sl = data.DSSachYeuThiches.Where(n => n.idkh == idkh).ToList();
            int soluong = sl.Count();
            return soluong;
        }



         

    }
}