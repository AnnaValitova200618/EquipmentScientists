using Equipment_Client.Tools;
using Equipment_Client.Views;
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

        public CustomCommand OpenResponsibleEquipment { get; set; }
        public CustomCommand OpenRequestEquipment { get; set; }
        public CustomCommand OpenDiagramm { get; set; }
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
            CurrentPage = new ResponsibleEquipment(scientist);

            OpenResponsibleEquipment = new CustomCommand(() =>
            {
                CurrentPage = new ResponsibleEquipment(scientist);
            });
            OpenRequestEquipment = new CustomCommand(() =>
            {
                CurrentPage = new RequestEquipment(scientist);
            });
            OpenDiagramm = new CustomCommand(() =>
            {
                CurrentPage = new Diagramm(scientist);
            });
            Back = new CustomCommand(() =>
            {
                new MainWindow().Show();
                window.Close();
            });
        }
    }
}
