using Equipment_Client.DB;
using Equipment_Client.Models;
using Equipment_Client.Tools;
using Equipment_Client.Views.Responsible;
using Equipment_Client.Views.Scientist_Worker;
using Equipment_Client.VM.Responsible;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Spire.Doc;
using System.Windows;
using Microsoft.Identity.Client;
using System.Windows.Controls;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System.Diagnostics;

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
        private bool block = true;
        private bool blockEnd = true;
        private Visibility visibilityBlock = Visibility.Visible;
        private Visibility visibilityResponsable = Visibility.Collapsed;
        private Visibility visibilityEnd = Visibility.Visible;
        private Visibility word = Visibility.Collapsed;
        public Booking Booking { get; set; }
        public Report Report { get; set; } = new();
        public Repair Repair { get; set; } = new();
        public ReplacementOfConsumable ReplacementOfConsumable { get; set; } = new();
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
        public bool Repair1 
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
                    Repair1 = true;
                    Signal(nameof(Repair1));
                }
                else
                {
                    Repair1 = false;
                    Signal(nameof(Repair1));
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
        public bool Block 
        {
            get => block;
            set
            {
                block = value;
                Signal();
            }
        }
        public bool BlockEnd 
        { 
            get => blockEnd;
            set
            {
                blockEnd = value;
                Signal();
            }
        }
        public Visibility VisibilityBlock 
        {
            get => visibilityBlock;
            set
            {
                visibilityBlock = value;
                Signal();
            }
        }
        public Visibility VisibilityResponsable 
        {
            get => visibilityResponsable;
            set
            {
                visibilityResponsable = value;
                Signal();
            }
        }
        public Visibility VisibilityEnd 
        {
            get => visibilityEnd;
            set
            {
                visibilityEnd = value;
                Signal();
            }
        }
        public Visibility Word 
        {
            get => word;
            set
            {
                word = value;
                Signal();
            }
        }
        public string Operators { get; set; }
        public List<Scientist> SelectOperators { get; set; }
        public ObservableCollection<byte[]> ImagesResponsable { get; set; } = new();
        public ObservableCollection<byte[]> Images { get; set; } = new ();
        public CustomCommand AddImage { get; set; }
        public CustomCommand Subscribe { get; set; }
        public CustomCommand Export { get; set; }
        public CustomCommand AddImageResponsable { get; set; }
        public CustomCommand SubscribeResponsable { get; set; }
        public CustomCommand OpenOperatorWindow { get; set; }

        OpenFileDialog openFileDialog = new OpenFileDialog()
        {
            Multiselect = true,
            Filter = "(*.png, *.jpg)|*.png;*.jpg",
        };
        

        public ReportVM(Booking selectBooking, Scientist scientist, Scientist_WorkerVM scientist_WorkerVM)
            //подпись научного
        {
            Booking = selectBooking;
            PlaceOfUses = DBInstance.GetInstance().PlaceOfUses.ToList();
            Works = DBInstance.GetInstance().TypeOfWorks.ToList();

            

            AddImage = new CustomCommand(() =>
            {
                
                if(openFileDialog.ShowDialog() == true)
                {
                    List<double>listfilesize = new();
                    
                    if (Images.Count + openFileDialog.FileNames.Count() > 3) 
                    {
                        MessageBox.Show("НАДО МЕНЬШЕ КАРТИНОК");
                        return;
                    }
                    foreach(var file in openFileDialog.FileNames)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        double filesize = ConvertBytesToMegabytes(fileInfo.Length);
                        listfilesize.Add(filesize);
                        double sumfilesize = listfilesize.Sum();

                        if (sumfilesize > 5)
                        {
                            MessageBox.Show("Фотографии слишком велики для моего маленького приложения");
                            return;
                        }

                        Images.Add(File.ReadAllBytes(file));
                    }

                    
                }
            });
            
            Subscribe = new CustomCommand(() =>
            {
                if (Report.Condition.IsNullOrEmpty() || Images.Count == 0 ||
                Report.NumberDocument.IsNullOrEmpty() ||
                Report.CharacteristicsWork.IsNullOrEmpty() || Report.DescriptionPlaceOfUse.IsNullOrEmpty() ||
                SelectPlaceOfUse == null || SelectWork == null)
                {
                    MessageBox.Show("Не все данные заполнены");
                    return;
                }
                if (Report.DateStartFact < Booking.DateStart || 
                   Report.DateStartFact > Report.DateEndFact ||
                   Report.DateStartFact > Booking.DateEnd)
                {
                    MessageBox.Show("Некорректно выбрана дата начала");
                    return;
                }
                if(Report.DateEndFact > Booking.DateEnd)
                {
                    MessageBox.Show("Некорректно выбрана дата окончания");
                    return;
                }
                if(Report.DateFirstUseEquipment < Report.DateStartFact ||
                   Report.DateFirstUseEquipment > Report.DateEndFact)
                {
                    MessageBox.Show("Некорректно выбрана дата первого использования");
                    return;
                }
                if(Report.DateLastUseEquipment < Report.DateFirstUseEquipment ||
                   Report.DateLastUseEquipment > Report.DateEndFact)
                {
                    MessageBox.Show("Некорректно выбрана дата последнего использования");
                    return;
                }
                if (RepairCheck == true && 
                   (Repair.DateStartDowntime < Report.DateStartFact ||
                    Repair.DateEndDowntime > Report.DateEndFact ||
                    Repair.DateStartDowntime > Repair.DateEndDowntime))
                {

                    MessageBox.Show("Некорректно выбраны даты простоя оборудования во время ремонта");
                    return;

                }
                try
                {
                    Report.IdBooking = Booking.Id;
                    Report.IdPlaceOfUse = SelectPlaceOfUse.Id;
                    Report.IdTypeOfWork = SelectWork.Id;
                    Report.DateSigningReportScientists = DateTime.Now;
                    Report.Operators = Operators;
                    foreach(var image in Images)
                    {
                        Report.FhotoPaths.Add(new FhotoPath() { Fhoto = image, IdScientist = scientist.Id}); 
                    }

                    DBInstance.GetInstance().Reports.Add(Report);
                    DBInstance.GetInstance().SaveChanges();

                    if (RepairCheck == true)
                    {
                        
                        Repair.IdReport = Report.Id;
                        DBInstance.GetInstance().Repairs.Add(Repair);
                    }
                    if(ConsumableCheck == true)
                    {
                        if(ReplacementOfConsumable.Description == null)
                        {
                            MessageBox.Show("Не все данные заполнены");
                            return;
                        }
                        ReplacementOfConsumable.IdReport = Report.Id;
                        DBInstance.GetInstance().ReplacementOfConsumables.Add(ReplacementOfConsumable);
                    }

                    DBInstance.GetInstance().SaveChanges();
                    MessageBox.Show("Отчёт сохранён успешно!");
                    scientist_WorkerVM.CurrentPage = new Cabinet(scientist, scientist_WorkerVM);
                }
                catch(Exception ex) 
                {
                    MessageBox.Show("Опаньки >:)");
                    return;
                }

            });
            OpenOperatorWindow = new CustomCommand(() =>
            {
                new OperatorWindow(this).ShowDialog();

                if(SelectOperators?.Count == 1)
                {
                    Operators = $"{SelectOperators[0].FIO}";
                    Signal(nameof(Operators));
                }
                else if (SelectOperators?.Count == 2)
                {
                    Operators = $"{SelectOperators[0].FIO}, {SelectOperators[1].FIO}";
                    Signal(nameof(Operators));
                }
                else if(SelectOperators?.Count == 3)
                {
                    Operators = $"{SelectOperators[0].FIO}, {SelectOperators[1].FIO}, {SelectOperators[2].FIO}";
                    Signal(nameof(Operators));
                }
                else
                {
                    Operators = "";
                    Signal(nameof(Operators));
                    return;
                }
                  
            });
        }

        public ReportVM(Report selectReport, Scientist scientist, Scientist_WorkerVM scientist_WorkerVM)
            //просмотр научного
        {
            try
            {
                Booking = selectReport.IdBookingNavigation;
                PlaceOfUses = DBInstance.GetInstance().PlaceOfUses.ToList();
                Works = DBInstance.GetInstance().TypeOfWorks.ToList();
                Report = selectReport;
                SelectPlaceOfUse = Report.IdPlaceOfUseNavigation;
                SelectWork = Report.IdTypeOfWorkNavigation;
                Repair = Report.Repairs.FirstOrDefault();
                ReplacementOfConsumable = Report.ReplacementOfConsumables.FirstOrDefault();
                Operators = Report?.Operators;

                if (selectReport != null)
                {
                    Block = false;
                    VisibilityBlock = Visibility.Collapsed;
                    
                    if (selectReport.ReplacementOfConsumables.Count != 0)
                    {
                        ConsumableCheck = true;
                        Consumable = false;
                    }
                    if (selectReport.Repairs.Count != 0)
                    {

                        RepairCheck = true;
                        Repair1 = false;
                    }
                }

                foreach (var image in selectReport.FhotoPaths.Where(s=>s.IdScientist == scientist.Id))
                {
                    Images.Add(image.Fhoto);
                }

                if (selectReport.DateSigningReportReponsibleScientists != null)
                {
                    VisibilityResponsable = Visibility.Visible;
                    BlockEnd = false;
                    VisibilityEnd = Visibility.Collapsed;
                    Word = Visibility.Visible;

                    foreach (var image in selectReport.FhotoPaths.Where(s => s.IdScientist == Report.
                    IdBookingNavigation.IdEquipmentNavigation.IdReponsibleScientistsNavigation.Id))
                    {
                        ImagesResponsable.Add(image.Fhoto);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Опаньки >:)");
                return;
            }
            Export = new CustomCommand(() =>
            {
                try
                {
                    DirectionCreator.Images.Clear();
                    DirectionCreator.ImagesResponsable.Clear();
                    DirectionCreator.GetDirections(Report);
                   
                }
                catch(Exception ex) 
                {
                    MessageBox.Show("Опаньки >:)");
                    return;
                }
            });
        }

        public ReportVM(Report selectReport, Scientist scientist, ResponsibleWindowVM responsibleWindowVM)
            //подпись ответственного и просмотр ответственного
        {
            try
            {
                Booking = selectReport.IdBookingNavigation;
                PlaceOfUses = DBInstance.GetInstance().PlaceOfUses.ToList();
                Works = DBInstance.GetInstance().TypeOfWorks.ToList();
                Report = selectReport;
                SelectPlaceOfUse = Report.IdPlaceOfUseNavigation;
                SelectWork = Report.IdTypeOfWorkNavigation;
                Repair = Report.Repairs.FirstOrDefault();
                ReplacementOfConsumable = Report.ReplacementOfConsumables.FirstOrDefault();
                Operators = Report?.Operators;

                foreach (var image in selectReport.FhotoPaths.Where(s => s.IdScientist == Report.
                    IdBookingNavigation.IdScientistNavigation.Id))
                {
                    Images.Add(image.Fhoto);
                }

                if (scientist.IdPosition == 2)
                {
                    Block = false;
                    VisibilityBlock = Visibility.Collapsed;
                    VisibilityResponsable = Visibility.Visible;

                    if (selectReport.ReplacementOfConsumables.Count != 0)
                    {
                        ConsumableCheck = true;
                        Consumable = false;
                    }
                    if (selectReport.Repairs.Count != 0)
                    {

                        RepairCheck = true;
                        Repair1 = false;
                    }
                }

                if (selectReport.DateSigningReportReponsibleScientists != null)
                {
                    VisibilityResponsable = Visibility.Visible;
                    BlockEnd = false;
                    VisibilityEnd = Visibility.Collapsed;
                    Word = Visibility.Visible;

                    foreach (var image in selectReport.FhotoPaths.Where(s => s.IdScientist == Report.
                    IdBookingNavigation.IdEquipmentNavigation.IdReponsibleScientistsNavigation.Id))
                    {
                        ImagesResponsable.Add(image.Fhoto);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Опаньки >:)");
                return;
            }
            AddImageResponsable = new CustomCommand(() =>
            {

                if (openFileDialog.ShowDialog() == true)
                {
                    List<double> listfilesize = new();

                    if (ImagesResponsable.Count + openFileDialog.FileNames.Count() > 3)
                    {
                        MessageBox.Show("НАДО МЕНЬШЕ КАРТИНОК");
                        return;
                    }
                    foreach (var file in openFileDialog.FileNames)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        double filesize = ConvertBytesToMegabytes(fileInfo.Length);
                        listfilesize.Add(filesize);
                        double sumfilesize = listfilesize.Sum();

                        if (sumfilesize > 5)
                        {
                            MessageBox.Show("Фотографии слишком велики для моего маленького приложения");
                            return;
                        }

                        ImagesResponsable.Add(File.ReadAllBytes(file));
                    }
                }
            });
            SubscribeResponsable = new CustomCommand(() =>
            {
                if(ImagesResponsable.Count == 0 || Report.StatusEquipment.IsNullOrEmpty()  || Report.DateReturn == null)
                {
                    MessageBox.Show("Не все данные заполнены");
                    return;
                }
                try
                {
                    if(Report.DateReturn < Report.DateLastUseEquipment)
                    {
                        MessageBox.Show("Дата возврата оборудования не может быть меньше даты последнего использования оборудования");
                        return;
                    }
                    if(Report.DateReturn > Report.DateEndFact.AddDays(7))
                    {
                        MessageBox.Show("Дата возврата на может быть больше чем на неделю даты фактического окончания бронирования");
                        return;
                    }
                    Report.DateSigningReportReponsibleScientists = DateTime.Now;

                    foreach (var image in ImagesResponsable)
                    {
                        Report.FhotoPaths.Add(new FhotoPath() { Fhoto = image, IdScientist = scientist.Id});
                    }
                    DBInstance.GetInstance().SaveChanges();
                    MessageBox.Show("Отчёт сохранён успешно!");
                    responsibleWindowVM.CurrentPage = new ReportsPage(scientist, responsibleWindowVM);
                }
                catch
                {
                    MessageBox.Show("Опаньки >:)");
                    return;
                }
                Export = new CustomCommand(() =>
                {
                    
                });
            });
        }

        private static void OpenPreviewFile(string path)
        {
            if (File.Exists(path) == false)
            {
                return;
            }
            Process.Start(path);
        }


        internal void RemoveImage(byte[] image)
        {
            Images.Remove(image);
        }
        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
    }
}
