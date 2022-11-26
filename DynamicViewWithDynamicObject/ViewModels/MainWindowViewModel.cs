using CommunityToolkit.Mvvm.Input;
using DynamicViewWithDynamicObject.ExtensiveMethods;
using DynamicViewWithDynamicObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using RelayCommand = CommunityToolkit.Mvvm.Input.RelayCommand;

namespace DynamicViewWithDynamicObject.ViewModels
{
    public partial class MainWindowViewModel:BaseViewModel
    {
        private readonly Window mainWindow;
        private CustomObject? obje;

        public CustomObject CustomModule
        {
            get { return obje??new(Application.Current.Dispatcher); }
            set { SetProperty(ref obje, value); }
        }


        public MainWindowViewModel(Window a)
        {
            mainWindow = a;
            obje = new CustomObject(a.Dispatcher);
        }
        private string? newItemName;
        

        private object _newProp=new();

        public object NewProp
        {
            get { return _newProp; }
            set { SetProperty(ref _newProp, value); }
        }

        public string? NewItemName
        {
            get => newItemName;
            set => SetProperty(ref newItemName, value);
        }
        public ICommand AddElementCommand
        {
            get => new CommunityToolkit.Mvvm.Input.RelayCommand<object[]>((object[]? param) =>
            {
                if(param is not null&&NewItemName is not null &&NewItemName != string.Empty)
                {
                    var stcPropNames = (StackPanel)param[0];
                    var stcItems =(StackPanel) param[1];
                    var newTextbox = new TextBox()
                    {
                        Name = NewItemName
                    };
                    var newTextBlock = new TextBlock()
                    {
                        Text = NewItemName
                    };
                    CustomModule[NewItemName] = NewProp;
                    Binding myBinding = new();
                    myBinding.Source = CustomModule;
                    myBinding.Path = new PropertyPath(NewItemName);
                    myBinding.Mode = BindingMode.TwoWay;
                    myBinding.NotifyOnSourceUpdated = true;
                    myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    BindingOperations.SetBinding(newTextbox, TextBox.TextProperty, myBinding);
                    //NewItemName = null;
                    if (!stcItems.Children.Contains(newTextbox))
                    {
                        stcItems.Children.Add(newTextbox);
                    }
                    if (!stcPropNames.Children.Contains(newTextBlock))
                    {
                        stcPropNames.Children.Add(newTextBlock);
                    }
                }
                //if(param[0] is StackPanel a&& NewItemName is not null&& NewItemName is not null)
                //{
                //    
                //}
            });
        }
        public ICommand SerializeObjectCommand
        {
            get => new RelayCommand(() =>
            {
                var xx = JsonSerializer.Serialize(CustomModule, new JsonSerializerOptions()
                {
                    WriteIndented=true,
                    Converters = {new DynamicObjectConverter()}
                });
                var tq = CustomModule.CreateClass("TestModule");
                
                var qt = new List<object>()
                {
                    CustomModule.GetConverter(),
                    CustomModule.GetAttributes(),
                    CustomModule.GetProperties()
                };
                HelperMethods.WriteClassToCs(tq,"TestModule");
                var tt = JsonSerializer.Deserialize<Dictionary<string, object>>(xx);
                var qq = new CustomObject(tt);
            });
           
        }
    }
}
