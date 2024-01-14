using Equipment_Client.DB;
using Equipment_Client.Models;
using Equipment_Client.Tools;
using Equipment_Client.Views.Scientist_Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Equipment_Client.VM.Scientist_Worker
{
    public class ReportVM : BaseVM
    {
        private PlaceOfUse selectPlaceOfUse;
        private TypeOfWork selectWork;
        private bool repair = false;
        private bool repair1 = false;
        private bool consumable = false;
        private bool consumableCheck = false;

        public Booking Booking { get; set; }
        public Report Report { get; set; } = new();
        public List<PlaceOfUse> PlaceOfUses { get; set; }
        public PlaceOfUse SelectPlaceOfUse 
        {
            get => selectPlaceOfUse;
            set
            {
                selectPlaceOfUse = value;
                Signal();
            }
        }
        public List<TypeOfWork> Works { get; set; }
        public TypeOfWork SelectWork 
        {
            get => selectWork;
            set
            {
                selectWork = value;
                Signal();
            }
        }
        public bool Repair 
        {
            get => repair1;
            set
            {
                repair1 = value;
                Signal();
            }
        }
        public bool RepairCheck 
        {
            get => repair;
            set
            {
                repair = value;
                Signal();
                if (repair == true)
                {
                    Repair = true;
                    Signal(nameof(Repair));
                }
                else
                {
                    Repair = false;
                    Signal(nameof(Repair));
                }
            }
        }
        public bool Consumable 
        {
            get => consumable;
            set
            {
                consumable = value;
                Signal();
            }
        }
        public bool ConsumableCheck 
        {
            get => consumableCheck;
            set
            {
                consumableCheck = value;
                Signal();
                if (consumableCheck == true)
                {
                    Consumable = true;
                    Signal(nameof(Consumable));
                }
                else
                {
                    Consumable = false;
                    Signal(nameof(Consumable));
                }
            }
        }
        public CustomCommand AddImage { get; set; }
        public CustomCommand Subscribe { get; set; }
        public ReportVM(Booking selectBooking, Scientist scientist, Scientist_WorkerVM scientist_WorkerVM)
        {
            Booking = selectBooking;
            PlaceOfUses = DBInstance.GetInstance().PlaceOfUses.ToList();
            Works = DBInstance.GetInstance().TypeOfWorks.ToList();

            

            AddImage = new CustomCommand(() =>
            {

            });
            Subscribe = new CustomCommand(() =>
            {
                if(Report.Condition == null || Report.DateStartFact == null || 
                Report.DateEndFact == null || Report.DateFirstUseEquipment == null ||
                Report.NumberDocument == null || Report.NumberOfMeasurements == null ||
                Report.CharacteristicsWork == null || Report.DateLastUseEquipment == null || Report.DescriptionPlaceOfUse == null ||
                SelectPlaceOfUse == null || SelectWork == null)
                {
                    MessageBox.Show("Не все данные заполнены");
                    return;
                }
                try
                {
                    Report.IdBooking = Booking.Id;
                    Report.IdPlaceOfUse = SelectPlaceOfUse.Id;
                    Report.IdTypeOfWork = SelectWork.Id;
                    Report.DateSigningReportScientists = DateTime.Now;
                    DBInstance.GetInstance().Reports.Add(Report);
                    DBInstance.GetInstance().SaveChanges();
                    MessageBox.Show("Отчёт сохранён успешно!");
                    scientist_WorkerVM.CurrentPage = new Cabinet(scientist, scientist_WorkerVM);
                }
                catch 
                {
                    MessageBox.Show("Опаньки >:)");
                    return;
                }

            });
        }
    }
}
