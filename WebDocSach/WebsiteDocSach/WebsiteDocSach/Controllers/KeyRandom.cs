using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace WebsiteDocSach.Controllers
{
    public class KeyRandom
    {
        public static string LayKey(int dodai)
        {
            byte[] randomArray = new byte[dodai];
            string randomString;
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomArray);
            randomString = Convert.ToBase64String(randomArray);
            return randomString;
        }
    }
}