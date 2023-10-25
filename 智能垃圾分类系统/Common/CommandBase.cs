using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace 智能垃圾分类系统.Common
{
    public class CommandBase : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public Action<object> execiteAction { get; set; }
        public Func<object, bool> DoCanExecuteFunc { get; set; }

        public CommandBase(Action<object> action,Func<object,bool> func)
        {
            execiteAction = action;
            DoCanExecuteFunc = func;
        }

        public bool CanExecute(object? parameter)
        {
            //return true;
            return DoCanExecuteFunc?.Invoke(parameter) == true;
        }

        public void Execute(object? parameter)
        {
            execiteAction?.Invoke(parameter);
        }
    }
}
