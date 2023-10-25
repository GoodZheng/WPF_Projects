using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 智能垃圾分类系统.Common;

namespace 智能垃圾分类系统.Model
{
    public class UserModel : NotifyBase
    {
        private string _avatar;

        public string Avatar
        {
            get { return _avatar; }
            set { _avatar = value; OnPropertyChanged(); }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
        }

        private int _gender;

        public int Gender
        {
            get { return _gender; }
            set { _gender = value; OnPropertyChanged(); }
        }

    }
}
