using Equipment_Client.DB;
using Equipment_Client.Models;
using Equipment_Client.Tools;
using MaterialDesignColors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Equipment_Client.VM.Responsible
{
    public class RequestEquipmentVM : BaseVM
    {
        private List<Booking> bookings;
        private PurposeOfUse selectPurposeOfUse;
        private readonly Scientist scientist;

        public CustomCommand Approve { get; set; }
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

        public List<Booking> ApproveBookings { get; set; }
        public List<PurposeOfUse> PurposeOfUses { get; set; }
        public PurposeOfUse SelectPurposeOfUse 
        {
            get => selectPurposeOfUse;
            set
            {
                selectPurposeOfUse = value;
                GetBookings(scientist);
            }
        }

        

        public RequestEquipmentVM(Scientist scientist)
        {
            try
            {
                this.scientist = scientist;
                GetBookings(scientist);
                PurposeOfUses = DBInstance.GetInstance().PurposeOfUses.ToList();
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

            Approve = new CustomCommand(() =>
            {

                try
                {
                    var checklist = Bookings.Where(s => s.ApprovedID).ToList();
                    if (checklist.Count() > 0)
                    {
                        ApproveBookings = DBInstance.GetInstance().Bookings.Where(s => s.Approved != 0).ToList();//ищем подтверждённые заявки
                        int countBooking = checklist.Count();
                        List<Booking> wrongBooking = new List<Booking>(); // создаём коллекцию, где будем хранить заявки, которые пересекаются
                        for (int i = 0; i < countBooking; i++)
                        {
                            Booking booking1 = checklist[i];// берём первую строку

                            for (int j = 0; j < countBooking; j++)
                            {
                                Booking booking2 = checklist[j];//берём вторую строку

                                if (booking1.IdEquipment == booking2.IdEquipment && booking1.Id != booking2.Id)
                                {
                                    if (!(booking1.DateStart > booking2.DateEnd || booking1.DateEnd < booking2.DateStart))// сравниваем
                                                                                                                          // даты для обнаружения пересечений
                                    {
                                        wrongBooking.Add(booking1);
                                        wrongBooking.Add(booking2);
                                    }
                                }
                            }
                            var bookingApproved = ApproveBookings.FirstOrDefault(s =>
                                      s.IdEquipment == booking1.IdEquipment &&
                                      !(s.DateStart > booking1.DateEnd || s.DateEnd < booking1.DateStart));
                            if (bookingApproved != null)// сравниваем заявку с заявкой, которая уже подтвержденв
                            {
                                wrongBooking.Add(booking1);
                            }
                        }
                        wrongBooking.ForEach(s => s.ApprovedID = false);//обнуляем статусы тем, у чего были проставлены галочки,
                                                                        //но они не подошли
                        if (wrongBooking.Count() == 0)
                        {
                            MessageBox.Show("Все заявки одобрены");

                        }
                        if (wrongBooking.Count() > 0)
                        {
                            MessageBox.Show("Часть заявок были не одобрены");
                        }
                    }

                    DBInstance.GetInstance().SaveChanges();
                    GetBookings(scientist);
                }
                catch
                {
                    MessageBox.Show("Проблемы с БД");
                    return;
                }
            });
            
        }

        private void GetBookings(Scientist scientist)
        {
            DateTime dateTime = DateTime.Now;

            Bookings = DBInstance.GetInstance().Bookings
                .Include(s => s.IdEquipmentNavigation)
                .Include(s => s.IdScientistNavigation)
                .Include(s => s.IdPurposeOfUseNavigation)
                .Where(s => s.IdEquipmentNavigation.IdReponsibleScientists == scientist.Id)
                .Where(s => s.Approved == 0)
                .Where(s => s.DateStart.DayOfYear > dateTime.DayOfYear)
                .ToList();
            try
            {
                if (SelectPurposeOfUse != null)
                {
                        Bookings = DBInstance.GetInstance().Bookings
                    .Include(s => s.IdEquipmentNavigation)
                    .Include(s => s.IdScientistNavigation)
                    .Include(s => s.IdPurposeOfUseNavigation)
                    .Where(s => s.IdEquipmentNavigation.IdReponsibleScientists == scientist.Id)
                    .Where(s => s.Approved == 0)
                    .Where(s => s.DateStart.DayOfYear > dateTime.DayOfYear)
                    .Where(s => s.IdPurposeOfUse == SelectPurposeOfUse.Id)
                    .ToList();
                    Signal(nameof(Bookings));
                }
            }
            catch 
            {
                MessageBox.Show("Проблема с БД");
                return;
            }
            
        }
    }
}
