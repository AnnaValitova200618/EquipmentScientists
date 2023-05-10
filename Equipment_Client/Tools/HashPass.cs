using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Equipment_Client.Tools
{
    public static class HashPass
    {
        public static string GetPass(string p)
        {
            byte[] hash = Encoding.UTF8.GetBytes(p);
            byte[] hashbytes = MD5.Create().ComputeHash(hash);
            return Encoding.UTF8.GetString(hashbytes);
        }
    }
}
