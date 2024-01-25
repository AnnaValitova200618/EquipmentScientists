using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equipment_Client.Tools
{
    public class LoadImage
    {
        static string BaseDirectory = "ImageEquipment";
        public static string SaveImage(byte[] image) 
        {
            string pathdir = $"{BaseDirectory}\\{DateTime.Now.ToShortDateString()}";
            if (!Directory.Exists(pathdir))
            {
                Directory.CreateDirectory(pathdir);
            }
            string path =  $"{pathdir}\\{Path.GetRandomFileName()}";
            File.WriteAllBytes(path, image);
            return path;
        }
    } 
}
