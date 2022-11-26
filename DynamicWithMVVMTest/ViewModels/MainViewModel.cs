using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DynamicWithMVVMTest.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using static DynamicWithMVVMTest.Models.DynamicModule;

namespace DynamicWithMVVMTest.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {

        }
        private string? newItemName;
        private MyDynObject? _dynModule;

        public MyDynObject MyProperty
        {
            get { return _dynModule ??= new(); }
            set { _dynModule = value; }
        }


        public string? NewItemName
        {
            get => newItemName;
            set => SetProperty(ref newItemName, value);
        }
        public ICommand AddElementCommand
        {
            get => new RelayCommand<object>((object? param) =>
            {
                if(param is Control)
                {
                    var xx = param;
                }
            });

        }
    }
}
