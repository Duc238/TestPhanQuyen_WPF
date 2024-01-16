using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using TestPhanQuyen_WPF.Model;
namespace TestPhanQuyen_WPF.ViewModel
{
    public class MainViewModel:BaseViewModel
    {
        private string _UserName;
        public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }
        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }
        private ObservableCollection<UserRole_PhanQuyen> _List;
        public ObservableCollection<UserRole_PhanQuyen> List { get => _List; set { _List = value; OnPropertyChanged(); } }
        private UserRole_PhanQuyen _SelectedUserRole;
        public UserRole_PhanQuyen SelectedUserRole
        {
            get => _SelectedUserRole;
            set
            {
                _SelectedUserRole = value;
                OnPropertyChanged();
            }
        }
        private Users_PhanQuyen _SelectedItem;
        public Users_PhanQuyen SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    SelectedUserRole = SelectedItem.UserRole_PhanQuyen;
                }
            }
        }
        public MainViewModel()
        {
            List = new ObservableCollection<UserRole_PhanQuyen>(DataProvider.Instance.DB.UserRole_PhanQuyen);
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Login(p); });
            ExitCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Environment.Exit(0); });
        }

        private void Login(Window p)
        {
            if (p == null)
            {
                return;
            }
            MainWindow Mainwindow = new MainWindow();
            if (UserName == null && Password == null)
            {
                MessageBox.Show("Bạn chưa nhập User Name và Password");
                return;
            }
            string passEncode = MD5Hash(Base64Encode(Password));
            var accCount = DataProvider.Instance.DB.Users_PhanQuyen.Where(x => x.UserName == UserName && x.Password == passEncode).Count();
            if (accCount > 0)
            {
                if (SelectedUserRole == null)
                {
                    MessageBox.Show("Vui lòng chọn quyền");
                }
                Window window = nextWindow(SelectedUserRole.Id);
                window.Show();
                Mainwindow.Hide();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại");
            }
        }
        Window nextWindow(int role)
        {
            Window window = new Window();
            switch (role)
            {
                case (int)Keyword.Role.Admin:
                    window = new Admin();
                    break;
                case (int)Keyword.Role.Staff:
                    window = new Staff();
                    break;
            }
            return window;
        }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
