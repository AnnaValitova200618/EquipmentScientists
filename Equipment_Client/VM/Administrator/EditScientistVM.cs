using Equipment_Client.DB;
using Equipment_Client.Models;
using Equipment_Client.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Equipment_Client.VM.Administrator
{
    public class EditScientistVM : BaseVM
    {
        private Position selectPosition;
        private Visibility visibility = Visibility.Visible;
        private Laboratory selectLaboratory;
        private Visibility visibilityUpdatePassword = Visibility.Collapsed;
        private Visibility visibilityText = Visibility.Collapsed;

        public Visibility Visibility
        {
            get => visibility;
            set
            {
                visibility = value;
                Signal();
            }
        }
        public Visibility VisibilityUpdatePassword 
        {
            get => visibilityUpdatePassword;
            set
            {
                visibilityUpdatePassword = value;
                Signal();
            }
        }
        public Visibility VisibilityText 
        {
            get => visibilityText;
            set
            {
                visibilityText = value;
                Signal();
            }
        }

        public CustomCommand Save { get; set; }
        public CustomCommand UpdatePassword { get; set; }
        public Scientist Scientist { get; set; }
        public Position SelectPosition
        {
            get => selectPosition;
            set
            {
                selectPosition = value;
                Signal();
            }
        }
        public List<Position> Positions { get; set; }
        public Laboratory SelectLaboratory 
        {
            get => selectLaboratory;
            set
            {
                selectLaboratory = value;
                Signal();
            }
        }
        public List<Laboratory> Laboratores { get; set; }
        public string NewPassword { get; set; }
        public EditScientistVM(Scientist selectscientist, Window window)
        {
            try
            {
                Scientist = selectscientist;
                SelectPosition = Scientist.IdPositionNavigation;
                SelectLaboratory = Scientist.IdLaboratotyNavigation;
                Positions = DBInstance.GetInstance().Positions.ToList();
                Laboratores = DBInstance.GetInstance().Laboratories.ToList();

                if (Scientist.Id != 0)
                {
                    Visibility = Visibility.Collapsed;
                    VisibilityUpdatePassword = Visibility.Visible;
                }
            }
            catch
            {
                MessageBox.Show("Проблема с БД");
                return;
            }

            UpdatePassword = new CustomCommand(() =>
            {
                VisibilityText = Visibility.Visible;
            });
            Save = new CustomCommand(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(Scientist.Firstname) ||
                    string.IsNullOrEmpty(Scientist.Patronymic) ||
                    SelectPosition == null ||
                    string.IsNullOrEmpty(Scientist.Lastname) ||
                    string.IsNullOrEmpty(Scientist.Login) ||
                    string.IsNullOrEmpty(Scientist.Password) ||
                    SelectLaboratory == null)
                    {
                        MessageBox.Show("Необходимо заполнить все поля");
                        return;
                    }
                    if (Scientist.Id == 0)
                    {
                        var user = DBInstance.GetInstance().Scientists.FirstOrDefault(s => s.Login == Scientist.Login);

                        if (user != null)
                        {
                            MessageBox.Show("Пользователь с таким логином уже существует");
                            return;
                        }
                        DBInstance.GetInstance().Scientists.Add(
                            new Scientist
                            {
                                Login = Scientist.Login,
                                Firstname = Scientist.Firstname,
                                Patronymic = Scientist.Patronymic,
                                Lastname = Scientist.Lastname,
                                Password = HashPass.GetPass(Scientist.Password),
                                IdPosition = SelectPosition.Id,
                                IdLaboratoty = SelectLaboratory.Id,
                            });
                    }

                    if(NewPassword != null)
                    {
                        Scientist.Password = HashPass.GetPass(NewPassword);
                    }

                    Scientist.IdPosition = SelectPosition.Id;
                    Scientist.IdLaboratoty = SelectLaboratory.Id;
                    DBInstance.GetInstance().SaveChanges();
                    MessageBox.Show("Оке");
                    window.Close();
                }
                
                catch
                {
                    MessageBox.Show("Проблема с БД");
                }

            });
            
        }
    }
}
