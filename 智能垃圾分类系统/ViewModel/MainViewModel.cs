using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using 智能垃圾分类系统.Common;
using 智能垃圾分类系统.Model;

namespace 智能垃圾分类系统.ViewModel
{
    public class MainViewModel : NotifyBase
    {
        public UserModel UserInfo { get; set; }

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; OnPropertyChanged(); }
        }

        private FrameworkElement _mainContent;

        public FrameworkElement MainContent
        {
            get { return _mainContent; }
            set { _mainContent = value; OnPropertyChanged(); }
        }

        public CommandBase NavChangedCommand { get; set; }

        public MainViewModel()
        {
            UserInfo = new UserModel();
            this.NavChangedCommand = new CommandBase(new Action<object>(DoNavChanged), new Func<object, bool>((o) => true));
            /*this.NavChangedCommand.DoExecute = new Action<object>(DoNavChanged);
            this.NavChangedCommand.DoCanExecute = new Func<object, bool>((o) => true);*/

            //这是为了在窗口首次加载时避免内容框空白的情况
            DoNavChanged("FirstPageView");
        }

        private void DoNavChanged(object obj)
        {
            Type type = Type.GetType("智能垃圾分类系统.View." + obj.ToString());
            //对反射得到的类的默认构造函数访问（无参）
            ConstructorInfo cti = type.GetConstructor(System.Type.EmptyTypes);
            //方法调用
            this.MainContent = (FrameworkElement)cti.Invoke(null);
        }
    }
}
