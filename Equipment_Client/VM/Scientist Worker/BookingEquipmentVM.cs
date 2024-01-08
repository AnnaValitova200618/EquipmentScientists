using Equipment_Client.DB;
using Equipment_Client.Models;
using Equipment_Client.Tools;
using Equipment_Client.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Equipment_Client.VM
{
    public class BookingEquipmentVM : BaseVM
    {
        private Equipment selectEquipment;
        private PurposeOfUse selectPurpose;
        public Booking Booking { get; set; } = new();
        public Scientist Scientist { get; set; }
        public string FIO { get; set; }
        //public List<Equipment> Equipments { get; set; }
        //public Equipment SelectEquipment
        //{
        //    get => selectEquipment;
        //    set
        //    {
                
        //        selectEquipment = value;
        //        if(selectEquipment == null)
        //        {
        //            FIO = null;
        //            Signal(nameof(FIO));
        //            return;
        //        }
        //        else
        //        {
        //            Booking.IdEquipment = SelectEquipment.Id;
        //            Scientist = DBInstance.GetInstance().Scientists.
        //                Where(s => s.Id == SelectEquipment.IdReponsibleScientists).FirstOrDefault();
        //            FIO = $"{Scientist.Firstname} {Scientist.Patronymic} {Scientist.Lastname}";
        //            Signal();
        //            Signal(nameof(Scientist));
        //            Signal(nameof(FIO));
        //        }
               
        //    }
        //}
        public List<PurposeOfUse> Purposes { get; set; }
        public PurposeOfUse SelectPurpose
        {
            get => selectPurpose;
            set
            {
                selectPurpose = value;
                Signal();
            }
        }
        public CustomCommand Save { get; set; }
        public CustomCommand CleanForm { get; set; }
        public BookingEquipmentVM(Scientist scientist, Equipment selectedEquipment, Scientist_WorkerVM scientist_WorkerVM)
        {
            try
            {
                Booking.IdEquipmentNavigation = selectedEquipment;
                Scientist = DBInstance.GetInstance().Scientists.Where(s => s.Id == selectedEquipment.IdReponsibleScientists).FirstOrDefault();
                FIO = $"{Scientist.Firstname} {Scientist.Patronymic} {Scientist.Lastname}";
                Signal(nameof(Scientist));
                Signal(nameof(FIO));
                //Equipments = DBInstance.GetInstance().Equipment.Where(s => s.IdStatus == 1 || s.IdStatus == 5 || s.IdStatus == 7).ToList();
                Purposes = DBInstance.GetInstance().PurposeOfUses.ToList();
            }
            catch
            {
                MessageBox.Show("Проблема с БД");
                return;
            }
            Save = new CustomCommand(() =>
            {
                DateTime interval = new DateTime();
                interval = interval.AddDays(6);

                if(Booking.DateStart == null || Booking.DateEnd == null || 
                    SelectPurpose == null)
                //SelectEquipment == null ||
                {
                    MessageBox.Show("Не все данные заполнены");
                    return;
                }
                if(Booking.DateEnd < Booking.DateStart)
                {
                    MessageBox.Show("Дата начала бронирования не может быть больше даты окончания бронирования");
                    return;
                }
                if (Booking.DateStart.Subtract(DateTime.Now).TotalDays < 6)
                {
                    MessageBox.Show("Дата начала бронирования должна быть больше сегодняшней даты на неделю");
                    return;
                }
                try
                {
                    //Booking.IdEquipment = SelectEquipment.Id;
                    
                    Booking.IdScientist = scientist.Id;
                    Booking.IdPurposeOfUse = SelectPurpose.Id;
                    Booking.Approved = 0;
                    DBInstance.GetInstance().Bookings.Add(Booking);
                    DBInstance.GetInstance().SaveChanges();

                   // CleanForm.Execute(null);
                    MessageBox.Show("Заявка на бронирование отправлена!");
                    scientist_WorkerVM.CurrentPage = new List_Equipment(scientist_WorkerVM, scientist);
                }
                catch
                {
                    MessageBox.Show("Проблемы с БД");
                    return;
                }
            });
            CleanForm = new CustomCommand(() =>
            {
                Booking = new();
                SelectPurpose = null;
                //SelectEquipment = null;
                Signal(nameof(SelectPurpose));
                Signal(nameof(Booking));
                //Signal(nameof(SelectEquipment));
                return;
            });
        }
    }
}
