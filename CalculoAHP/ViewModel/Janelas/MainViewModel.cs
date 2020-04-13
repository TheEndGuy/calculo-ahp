using CalculoAHP.Domain.Enum;
using CalculoAHP.Model.Modelos;
using CalculoAHP.ViewModel.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CalculoAHP.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Model = new MainModel();

            Messenger.Default.Register<InserirModeloMessage>(this, (inserirModeloMessage) =>
            {
                InserirModelo(inserirModeloMessage.Modelo);
            });
        }

        public MainModel Model
        {
            get;
            set;
        }

        private ICommand m_openModel;
        
        public ICommand OpenModel
        {
            get { return m_openModel ?? (m_openModel = new RelayCommand<object>(CommandOpen)); }
            set { m_openModel = value; }
        }

        private ICommand m_createModel;

        public ICommand CreateModel
        {
            get { return m_createModel ?? (m_createModel = new RelayCommand(CommandCreate)); }
            set { m_createModel = value; }
        }

        private ICommand m_editModel;

        public ICommand EditModel
        {
            get { return m_editModel ?? (m_createModel = new RelayCommand<object>(CommandEdit)); }
            set { m_editModel = value; }
        }

        private ICommand m_modeloRemove;

        public ICommand RemoveModel
        {
            get { return m_modeloRemove ?? (m_modeloRemove = new RelayCommand<object>(CommandRemove)); }
            set { m_modeloRemove = value; }
        }

        private ICommand m_information;

        public ICommand Information
        {
            get { return m_information ?? (m_information = new RelayCommand(OpenInformation)); }
            set { m_information = value; }
        }

        private void OpenInformation()
        {
            WindowManager.Instance.ShowInformationWindow();
        }

        private void CommandRemove(object model)
        {
            MessageBoxResult result = MessageBox.Show("Deseja mesmo apagar o modelo '" + (model as Modelo).Nome + "' ? \nIsso apagará todas as decisões relacionadas a ele.", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
                return;

            Model.RemoveModel(model as Modelo);
        }

        private void CommandEdit(object model)
        {
            WindowManager.Instance.ShowModelEdit(model as Modelo);
        }

        private void CommandCreate()
        {
            WindowManager.Instance.ShowModelCreate();
        }

        private void CommandOpen(object model)
        {
            WindowManager.Instance.ShowModelOpen(model as Modelo);
        }

        private void InserirModelo(Modelo modeloInsercao)
        {
            // Edição
            if (Model.ModelosCollection.Any(entry => entry.Id == modeloInsercao.Id))
            {
                var oldModelo = Model.ModelosCollection.FirstOrDefault(entry => entry.Id == modeloInsercao.Id);

                if (oldModelo == null)
                    return;

                Model.ModelosCollection.Remove(oldModelo);
                Model.ModelosCollection.Add(modeloInsercao);
            }

            // Inserção
            else
                Model.ModelosCollection.Add(modeloInsercao);
        }
    }
}