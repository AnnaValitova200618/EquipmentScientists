using Equipment_Client.DB;
using Equipment_Client.Models;
using Equipment_Client.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Equipment_Client.VM
{
    public class ListBookingEquipmentVM : BaseVM
    {
        private PurposeOfUse selectPurposeOfUse;
        private string search = "";
        private List<Booking> bookings;
        public string fio { get; set; }
        public CustomCommand Reset { get; set; }
        public List<Booking> Bookings 
        {
            get => bookings;
            set
            {
                bookings = value;
                Signal();
            }
        }
        public List<PurposeOfUse> PurposeOfUses { get; set; }
        public PurposeOfUse SelectPurposeOfUse
        {
            get => selectPurposeOfUse;
            set
            {
                selectPurposeOfUse = value;
                DoSearch();
            }
        }
        public string Search
        {
            get => search;
            set
            {
                search = value;
                DoSearch();
            }
        }
        private void DoSearch()
        {
            try
            {
                List<Booking> listBooking = GetBooking().Where(s => s.IdEquipmentNavigation.Name.Contains(Search)).ToList();
                if (SelectPurposeOfUse != null)
                {
                    listBooking = listBooking.Where(s => s.IdPurposeOfUse == SelectPurposeOfUse.Id).ToList();
                }
                Bookings = listBooking;
            }
            catch
            {
                MessageBox.Show("Проблема с БД");
                return;
            }
            
        }

        public ListBookingEquipmentVM()
        {
            try
            {
                
                GetBooking();
                PurposeOfUses = DBInstance.GetInstance().PurposeOfUses.ToList();

                
                GetBooking();
            }
            catch
            {
                MessageBox.Show("Проблема с БД");
                return;
            }
            

            Reset = new CustomCommand(() =>
            {
                SelectPurposeOfUse = null;
                Signal(nameof(SelectPurposeOfUse));
            });
        }

        private List<Booking> GetBooking()
        {
            return Bookings = DBInstance.GetInstance().Bookings
                            .Include(s => s.IdPurposeOfUseNavigation)
                            .Include(s => s.IdScientistNavigation)
                            .Where(s => s.Approved == 1 && s.DateEnd >= DateTime.Now.Date).ToList();
        }
    }
}
