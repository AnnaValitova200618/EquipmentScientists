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
        private Visibility visibility = Visibility.Collapsed;
        private Visibility visibility2 = Visibility.Visible;

        public Visibility Visibility1
        {
            get => visibility;
            set
            {
                visibility = value;
                Signal();
            }
        }
        public Visibility Visibility2
        {
            get => visibility2;
            set
            {
                visibility2 = value;
                Signal();
            }
        }
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
        public CustomCommand Reset { get; set; }
        public CustomCommand Visible { get; set; }
        public CustomCommand Collapsed { get; set; }
        public CustomCommand Update { get; set; }
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
            Visible = new CustomCommand(() =>
            {
                Visibility2 = Visibility.Collapsed;
                Visibility1 = Visibility.Visible;
                Signal(nameof(Visibility1));
                Signal(nameof(Visibility2));
            });
            Collapsed = new CustomCommand(() =>
            {
                Visibility2 = Visibility.Visible;
                Visibility1 = Visibility.Collapsed;
                Signal(nameof(Visibility1));
                Signal(nameof(Visibility2));
            });
            Update = new CustomCommand(() =>
            {
                GetBooking();

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
