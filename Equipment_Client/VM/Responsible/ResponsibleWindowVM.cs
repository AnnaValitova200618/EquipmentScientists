using Equipment_Client.Tools;
using Equipment_Client.Views;
using Equipment_Client.Views.Responsible;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Equipment_Client.VM.Responsible
{
    public class ResponsibleWindowVM : BaseVM
    {
        private Page currentPage;

       
        public CustomCommand OpenRequestEquipment { get; set; }
        public CustomCommand OpenDiagramm { get; set; }
        public CustomCommand OpenReports { get; set; }
        public CustomCommand OpenCabinet { get; set; }
        public CustomCommand Back { get; set; }
        public Page CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                Signal();
            }
        }

        public ResponsibleWindowVM(Models.Scientist scientist, System.Windows.Window window)
        {
            CurrentPage = new CabinetResponsible(scientist);

            
            OpenRequestEquipment = new CustomCommand(() =>
            {
                CurrentPage = new RequestEquipment(scientist);
            });
            OpenDiagramm = new CustomCommand(() =>
            {
                CurrentPage = new Diagramm(scientist);
            });
            OpenReports = new CustomCommand(() =>
            {
                CurrentPage = new ReportsPage(scientist, this);
            });
            OpenCabinet = new CustomCommand(() =>
            {
                CurrentPage = new CabinetResponsible(scientist);
            });
            Back = new CustomCommand(() =>
            {
                new MainWindow().Show();
                window.Close();
            });
        }
    }
}
