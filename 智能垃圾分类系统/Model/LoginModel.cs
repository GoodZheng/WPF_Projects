using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using 智能垃圾分类系统.Common;

namespace 智能垃圾分类系统.Model
{
    internal class LoginModel : NotifyBase
    {
        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged();

            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        private string validationCode;
        public string ValidationCode
        {
            get { return validationCode; }
            set
            {
                validationCode = value;
            }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }

        private Visibility showProgress = Visibility.Collapsed;

        public Visibility ShowProgress
        {
            get { return showProgress; }
            set
            {
                showProgress = value;
                OnPropertyChanged();
                //LoginCommand.RaiseCanExecuteChanged();
            }
        }

    }
}
