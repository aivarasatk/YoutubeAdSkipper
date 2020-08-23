using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SkipYoutubeAdSysTray
{
    public class NotifyIconViewModel
    {
        public NotifyIconViewModel()
        {
            ExitApplicationCommand = new DelegateCommand { CommandAction = () => Application.Current.Shutdown() };
        }
        public ICommand ExitApplicationCommand { get; set; }
    }

    public class DelegateCommand : ICommand
    {
        public Action CommandAction { get; set; }
        public Func<bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter)
        {
            CommandAction();
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc == null || CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
