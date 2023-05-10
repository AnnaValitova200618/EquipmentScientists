using Equipment_Client.DB;
using Equipment_Client.Models;
using Equipment_Client.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Equipment_Client.Tools
{
    public static class CheckStatusEquipment
    {
        public static List<Equipment> CheckStatus()
        {
            var listApprovedBookings = DBInstance.GetInstance().Bookings
                .Where(s => s.Approved == 1 && 
                s.DateStart >= DateTime.Now.Date ||
                (s.DateStart < DateTime.Now.Date && s.DateEnd >= DateTime.Now.Date))
                .OrderBy(s=>s.DateStart).ToList(); //актуальные заявки

            var listEquipment = DBInstance.GetInstance().Equipment
                .Where(s=>s.IdStatus == 5 || s.IdStatus == 7 || s.IdStatus == 1 ).ToList();

            for (int i = 0; i < listEquipment.Count; i++)
            {
                var approvedBooking = listApprovedBookings.FirstOrDefault(s=>s.IdEquipment == listEquipment[i].Id);
                if(approvedBooking == null)
                {
                    listEquipment[i].IdStatus = 1;
                    continue;
                }
                if (approvedBooking.DateStart <= DateTime.Now.Date)
                {
                    approvedBooking.IdEquipmentNavigation.IdStatus = 7;
                }
                if (approvedBooking.DateStart > DateTime.Now.Date)
                {
                    approvedBooking.IdEquipmentNavigation.IdStatus = 5;
                }
            }

            DBInstance.GetInstance().SaveChanges();

            return DBInstance.GetInstance().Equipment
                .Include("IdTypeNavigation")
                .Include(s => s.IdReponsibleScientistsNavigation)
                .Include(s => s.IdStatusNavigation).ToList();
        }
    }
}
