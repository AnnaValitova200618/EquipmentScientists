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
using System.Windows.Controls;

namespace Equipment_Client.VM.Scientist_Worker
{
    public class CabinetVM : BaseVM
    {
        private Booking selectBooking;
        private Report selectReport;
        private List<Equipment> equipments;
        private Models.Type selectType;

        public List<Models.Type> Types { get; set; }
        public Models.Type SelectType
        {
            get => selectType;
            set
            {
                selectType = value;
                Signal();
            }
        }
        public List<Status> Statuses { get; set; }
        public string Status { get; set; }

        public List<Equipment> Equipments
        {
            get => equipments;
            set
            {
                equipments = value;
                Signal();
            }
        }
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
        public Report SelectReport 
        {
            get => selectReport;
            set
            {
                selectReport = value;
                Signal();
            }
        }
        public List<Report> Reports { get; set; }
        public Scientist Scientist { get; set; }
        public CustomCommand MakeReport {  get; set; }
        public CustomCommand LookReport { get; set; }
        public CabinetVM(Scientist scientist, Scientist_WorkerVM scientist_WorkerVM)
        {
            try
            {
                Scientist = scientist;
                Bookings = DBInstance.GetInstance().Bookings
                    .Where(s => s.IdScientist == Scientist.Id && s.Approved == 1 && s.Reports.Count == 0 && s.DateEnd < DateTime.Now)
                    .Include(s => s.IdEquipmentNavigation)
                    .Include(s => s.IdPurposeOfUseNavigation)
                    .Include(s => s.Reports)
                    .ToList();
                Reports = DBInstance.GetInstance().Reports
                    .Where(s => s.IdBookingNavigation.IdScientist == Scientist.Id)
                    .Include(s => s.IdBookingNavigation.IdEquipmentNavigation)
                    .Include(s => s.FhotoPaths)
                    .Include(s => s.IdPlaceOfUseNavigation)
                    .Include(s => s.IdTypeOfWorkNavigation)
                    .Include(s => s.Repairs)
                    .Include(s => s.ReplacementOfConsumables)
                    .ToList();
            }
            catch
            {
                MessageBox.Show("Проблема с БД");
                return;
            }

            MakeReport = new CustomCommand(() =>
            {
                if(SelectBooking == null)
                {
                    MessageBox.Show("Необходимо выбрать бронирование");
                    return;
                }
                scientist_WorkerVM.CurrentPage = new ReportPage(SelectBooking, Scientist, scientist_WorkerVM);
            });
            LookReport = new CustomCommand(() =>
            {
                if (SelectReport == null)
                {
                    MessageBox.Show("Необходимо выбрать бронирование");
                    return;
                }
                scientist_WorkerVM.CurrentPage = new ReportPage(SelectReport, Scientist, scientist_WorkerVM);
            });
        }

        public CabinetVM(Scientist scientist)
        {
            try
            {
                Scientist = scientist;
                GetEquipments(Scientist);
                Types = DBInstance.GetInstance().Types.ToList();
                Statuses = DBInstance.GetInstance().Statuses.ToList();
            }
            catch
            {
                MessageBox.Show("Проблема с БД");
                return;
            }
        }

        public void Save(DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                DBInstance.GetInstance().SaveChanges();
            }
        }
        private List<Equipment> GetEquipments(Scientist scientist)
        {
            return Equipments = CheckStatusEquipment.CheckStatus()
                .Where(s => s.IdReponsibleScientists == scientist.Id).ToList();

        }
    }
}
