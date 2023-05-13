using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equipment_Client.DB
{
    public class DBInstance
    {
        private static EquipmentContext instance;
        public static EquipmentContext GetInstance()
        {
            if (instance == null)
                instance = new EquipmentContext();
            return instance;


        }
    }
}
