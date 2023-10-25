using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using 智能垃圾分类系统.Common;
using 智能垃圾分类系统.DataAccess;
using 智能垃圾分类系统.Model;

namespace 智能垃圾分类系统.ViewModel
{
    class LoginViewModel
    {
        public CommandBase CloseCommand { get; set; }
        public CommandBase LoginCommand { get; set; }
        public LoginModel LoginModel { get; set; }
        public LoginViewModel()
        {
            CloseCommand = new CommandBase(CloseWindow, func);

            LoginCommand = new CommandBase(Login, func);

            LoginModel = new LoginModel();
            LoginModel.UserName = "username1";
            LoginModel.Password = "password1";
            LoginModel.ValidationCode = "anv2";
            LoginModel.ErrorMessage = "";
        }
        public void CloseWindow(object o)
        {
            (o as Window).Close();
        }

        public void Login(object o)
        {
            LoginModel.ShowProgress = Visibility.Visible;
            LoginModel.ErrorMessage = "";

            if (string.IsNullOrEmpty(LoginModel.UserName))
            {
                LoginModel.ErrorMessage = "用户名不能为空!";
                LoginModel.ShowProgress = Visibility.Collapsed;

                return;
            }
            if (string.IsNullOrEmpty(LoginModel.Password))
            {
                LoginModel.ErrorMessage = "密码不能为空!";
                LoginModel.ShowProgress = Visibility.Collapsed;

                return;
            }
            if (string.IsNullOrEmpty(LoginModel.ValidationCode))
            {
                LoginModel.ErrorMessage = "验证码不能为空!";
                LoginModel.ShowProgress = Visibility.Collapsed;

                return;
            }
            if (LoginModel.ValidationCode.ToLower() != "anv2")
            {
                LoginModel.ErrorMessage = "验证码输入不正确！";
                LoginModel.ShowProgress = Visibility.Collapsed;
                return;
            }

            Task.Run(new Action(() =>
            {
                try
                {
                    var user = aliDataAccess.GetInstance().CheckUserInfo(LoginModel.UserName, LoginModel.Password);
                    if (user == null)
                    {
                        throw new Exception("登录失败！用户名或密码错误！");
                    }

                    //若查询到当前用户的信息，则用一个变量保存当前用户的信息
                    GlobalValues.UserInfo = user;

                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        (o as Window).DialogResult = true;
                    }));
                }
                catch (Exception ex)
                {

                    LoginModel.ErrorMessage = ex.Message;
                }
                finally
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        LoginModel.ShowProgress = Visibility.Collapsed;
                    }));
                }
            }));
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
