using Equipment_Client.DB;
using Equipment_Client.Models;
using Equipment_Client.Tools;
using Equipment_Client.Views.Scientist_Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Equipment_Client.VM.Scientist_Worker
{
    public class CabinetVM : BaseVM
    {
        private Booking selectBooking;

        public List<Booking> Bookings { get; set; }
        public Booking SelectBooking 
        {
            get => selectBooking;
            set
            {
                selectBooking = value;
                Signal();
            }
        }
        public Scientist Scientist { get; set; }
        public CustomCommand MakeReport {  get; set; }
        public CabinetVM(Scientist scientist, Scientist_WorkerVM scientist_WorkerVM)
        {
            Scientist = scientist;
            Bookings = DBInstance.GetInstance().Bookings
                .Where(s=>s.IdScientist == scientist.Id && s.Approved == 1 && s.Reports.Count == 0)
                .Include(s=>s.IdEquipmentNavigation)
                .Include(s=>s.IdPurposeOfUseNavigation)
                .Include(s=>s.Reports)
                .ToList();

            MakeReport = new CustomCommand(() =>
            {
                if(SelectBooking == null)
                {
                    MessageBox.Show("Необходимо выбрать бронирование");
                    return;
                }
                scientist_WorkerVM.CurrentPage = new ReportPage(SelectBooking, scientist, scientist_WorkerVM);
            });
        }
    }
}
