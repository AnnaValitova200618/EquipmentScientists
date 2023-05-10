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
    public class MainWindowVM
    {
        private Scientist scientist;

        public string Login { get; set; }
        public string Password { get; set; }
        public CustomCommand LoginL { get; set; }
        public Scientist Scientist { get => scientist; set => scientist = value; }

        public MainWindowVM(Window window, System.Windows.Controls.PasswordBox password)
        {
            

            LoginL = new CustomCommand(() =>
            {
                try
                {
                    Scientist = DBInstance.GetInstance().Scientists.FirstOrDefault(s => s.Login == Login && s.Password == HashPass.GetPass(password.Password));

                    if (Scientist == null)
                    {
                        MessageBox.Show("Пользователя не существует");
                        return;
                    }
                    else
                    {
                        switch (Scientist.IdPosition)
                        {
                            case 1:
                                new Administrator_Window(Scientist).Show();
                                window.Close();
                                break;
                            case 2:
                                new Responsible_Window(Scientist).Show();
                                window.Close();
                                break;
                            case 3:
                                new Scientist_Worker_Window(Scientist).Show();
                                window.Close();
                                break;
                        }
                        
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка с БД");
                }


            });

        }
    }
}
