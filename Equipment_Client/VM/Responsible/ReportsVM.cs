using Equipment_Client.DB;
using Equipment_Client.Models;
using Equipment_Client.Tools;
using Equipment_Client.Views.Scientist_Worker;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Equipment_Client.VM.Responsible
{
    internal class ReportsVM : BaseVM
    {
        private Report selectReport;
        private Report selectCloseReport;

        public List<Report> Reports { get; set; }
        public Report SelectReport 
        {
            get => selectReport;
            set
            {
                selectReport = value;
                Signal();
            }
        }
        public List<Report> CloseReports { get; set; }
        public Report SelectCloseReport 
        {
            get => selectCloseReport;
            set
            {
                selectCloseReport = value;
                Signal();
            }
        }
        public CustomCommand LookReport { get; set; }
        public CustomCommand MakeReport { get; set; }
        public ReportsVM(Scientist scientist, ResponsibleWindowVM responsibleWindowVM)
        {
            Reports = DBInstance.GetInstance().Reports
                .Include(s => s.IdBookingNavigation.IdEquipmentNavigation)
                .Include(s => s.FhotoPaths)
                .Include(s => s.IdPlaceOfUseNavigation)
                .Include(s => s.IdTypeOfWorkNavigation)
                .Include(s => s.Repairs)
                .Include(s => s.ReplacementOfConsumables)
                .Where(s => s.IdBookingNavigation.IdEquipmentNavigation.IdReponsibleScientistsNavigation.Id == scientist.Id && 
                            s.DateSigningReportReponsibleScientists == null)
                .ToList();
            CloseReports = DBInstance.GetInstance().Reports
                .Include(s => s.IdBookingNavigation.IdEquipmentNavigation)
                .Include(s => s.FhotoPaths)
                .Include(s => s.IdPlaceOfUseNavigation)
                .Include(s => s.IdTypeOfWorkNavigation)
                .Include(s => s.Repairs)
                .Include(s => s.ReplacementOfConsumables)
                .Include(s=>s.IdBookingNavigation.IdScientistNavigation.IdLaboratotyNavigation.IdDepartmentNavigation)
                .Include(s=>s.IdBookingNavigation.IdScientistNavigation.IdLaboratotyNavigation)
                .Where(s => s.IdBookingNavigation.IdEquipmentNavigation.IdReponsibleScientistsNavigation.Id == scientist.Id &&
                            s.DateSigningReportReponsibleScientists != null)
                .ToList();
            MakeReport = new CustomCommand(() =>
            {
                if (SelectReport == null)
                {
                    MessageBox.Show("Необходимо выбрать бронирование");
                    return;
                }
                responsibleWindowVM.CurrentPage = new ReportPage(SelectReport, scientist, responsibleWindowVM);
            });
            LookReport = new CustomCommand(() =>
            {
                if(SelectCloseReport == null)
                {
                    MessageBox.Show("Необходимо выбрать бронирование");
                    return;
                }
                responsibleWindowVM.CurrentPage = new ReportPage(SelectCloseReport, scientist, responsibleWindowVM);
            });
        }
    }
}
