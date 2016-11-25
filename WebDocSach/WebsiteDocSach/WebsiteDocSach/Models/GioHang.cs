using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteDocSach.Models;

namespace WebsiteDocSach.Models
{
    public class GioHang
    {
        QLBanSachDataContext data = new QLBanSachDataContext();


        public int sidsach { set; get; }

        public string sTenHangHoa { set; get; }

        public string sMoTa { set; get; }

        public string sAnhBia { set; get; }

        public Double dDonGia { set; get; }

        public Double dKHuyenMai { set; get; }

        public Double dGiaMoi { set; get; }

        public int iSoLuong { set; get; }

        public string sLinkThanhToan { set; get; }

        public string slinkTaiSach { set; get; }

        public Double dThanhTien
        {
            get
            {
                if (dGiaMoi == 0 || dGiaMoi < dDonGia)
                {
                    return iSoLuong * dGiaMoi;

                }
                else
                {
                    return iSoLuong * dDonGia;
                }
            }
        }

        public GioHang(int idsach)
        {
            sidsach = idsach;
            sach hang = data.saches.Single(n => n.idsach == sidsach);
            sTenHangHoa = hang.tensach;
            sAnhBia = hang.anhbia;
            sMoTa = hang.mota;
            sLinkThanhToan = hang.LinkThanhToan;
            slinkTaiSach = hang.linktai;
            dDonGia = double.Parse(hang.gia.ToString());
            dGiaMoi = double.Parse(hang.GiaMoi.ToString());
            iSoLuong = 1;
        }

    }
}