using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_Projects.Common;
using WPF_Projects.Model;

namespace WPF_Projects.ViewModel
{
    class LoginViewModel
    {
        public CommonBase CloseCommand { get; set; }
        public CommonBase LoginCommand { get; set; }
        public LoginModel LoginModel { get; set; }
        public LoginViewModel()
        {
            CloseCommand = new CommonBase(CloseWindow,func);

            LoginCommand = new CommonBase(Login,func);

            LoginModel = new LoginModel();
            LoginModel.UserName = "user";
            LoginModel.Password = "1234";
            LoginModel.ErrorMessage = "错啦！";

            // load the configuration file.
            var configBuilder = new ConfigurationBuilder().
               AddJsonFile("appsettings.json").Build();

            // get the section to read
            var configSection = configBuilder.GetSection("AppSettings");

            // get the configuration values in the section.
            var client_id = configSection["ID"] ?? null;

            LoginModel.ErrorMessage = client_id;
        }
        public void CloseWindow(object o)
        {
            (o as Window).Close();
        }

        public void Login(object o)
        {
            LoginModel.ErrorMessage = "";
            if(string.IsNullOrEmpty(LoginModel.UserName))
            {
                LoginModel.ErrorMessage = "用户名不能为空!";
                return;
            }
            if (string.IsNullOrEmpty(LoginModel.Password))
            {
                LoginModel.ErrorMessage = "密码不能为空!";
                return;
            }
            if (string.IsNullOrEmpty(LoginModel.ValidationCode))
            {
                LoginModel.ErrorMessage = "验证码不能为空!";
                return;
            }
        }

        /*Action<object> action = new Action<object>((o) =>
        {
            (o as Window).Close();
            //MessageBox.Show("关闭窗口！");
        });*/
        Func<object, bool> func = new Func<object, bool>((o) =>
        {
            return true;
        });
    }
}
