using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CalculoAHP.ViewModel.Modelos.Dialog
{
    public class DialogViewModel : ViewModelBase
    {
        private string m_dialogType;
        private DialogHost m_dialogInstance;
        private UserControl m_internalDialog;

        public delegate void CreateValue(string criterioCreate);
        public event CreateValue OnCreateEvent;

        public DialogViewModel(string dialogType)
        {
            m_dialogType = dialogType;
        }

        private string m_internalValue;

        public string InternalValue
        {
            get { return m_internalValue; }
            set { m_internalValue = value; RaisePropertyChanged("InternalValue"); }
        }

        private ICommand m_createCriterio;

        public ICommand CreateValueCommand
        {
            get { return m_createCriterio ?? (m_createCriterio = new RelayCommand(OnCreateValue)); }
        }

        private ICommand m_closeDialog;

        public ICommand CloseDialogCommand
        {
            get { return m_closeDialog ?? (m_closeDialog = new RelayCommand(Close)); }
        }

        private UserControl View
        {
            get
            {
                return m_internalDialog ?? (m_internalDialog = new Views.Dialog.Dialog()
                {
                    DataContext = this
                });
            }
        }
        
        public bool IsOpened() => m_dialogInstance != null && m_dialogInstance.IsOpen;

        private bool IsValidValue() => InternalValue != null && !InternalValue.Equals("");

        public async void Execute()
        {
            if (IsOpened())
                return;

            await DialogHost.Show(View, m_dialogType, ExtendedOpenedEventHandler);
        }

        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            m_dialogInstance = eventargs.Source as DialogHost;
            BindingEvents();
        }

        public void BindingEvents()
        {
            m_dialogInstance.KeyDown += KeyDown;
        }
        
        public void UnbindingEvents()
        {
            m_dialogInstance.KeyDown -= KeyDown;
        }

        public void OnCreateValue()
        {
            if(IsValidValue())
                OnCreateEvent?.Invoke(InternalValue);
                
            Close();
        }

        public void Close()
        {
            UnbindingEvents();

            if (IsValidValue())
                InternalValue = "";

            if (m_dialogInstance == null)
                return;

            if (m_dialogInstance.IsOpen)
                m_dialogInstance.IsOpen = false;
        }

        public void KeyDown(object sender, KeyEventArgs e)
        {
            if (e == null)
                return;

            switch (e.Key)
            {
                case Key.Enter:
                    if (IsOpened())
                        OnCreateValue();
                    break;
                default:
                    break;
            }
        }
    }
}
