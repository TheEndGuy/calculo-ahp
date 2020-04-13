using CalculoAHP.Domain.Enum;
using CalculoAHP.Model.Dominio;
using CalculoAHP.Model.Modelos.Registro;
using CalculoAHP.ViewModel.Messages;
using CalculoAHP.ViewModel.Modelos;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CalculoAHP.ViewModel.MainScreens
{
    public class RegistroModeloMain : ViewModelBase
    {
        private ViewModelBase m_currentViewModel;

        public RegistroModeloMain()
        {
            ModeloRegistro = new Modelo();
            ModoEdicao = false;

            Messenger.Default.Register<SwitchViewMessage>(this, (switchViewMessage) =>
            {
                SwitchView(switchViewMessage.ModeloViewModel);
            });

            Messenger.Default.Register<FinalizarRegistroMessage>(this, (finalizarRegistroMessage) =>
            {
                FinalizarRegistro(finalizarRegistroMessage.Tipo);
            });

            Messenger.Default.Register<IniciarEdicaoModeloMessage>(this, (iniciarEdicaoMessage) =>
            {
                IniciarEdicao(iniciarEdicaoMessage.Modelo);
            });
        }

        public Modelo ModeloRegistro
        {
            get;
            set;
        }

        public bool ModoEdicao
        {
            get;
            set;
        }

        private string m_imagem;

        public string Imagem
        {
            get { return m_imagem; }
            set
            {
                if (m_imagem == value)
                    return;

                m_imagem = value;
                RaisePropertyChanged("Imagem");
            }
        }

        private string m_title;

        public string Title
        {
            get { return m_title; }
            set
            {
                if (m_title == value)
                    return;

                m_title = value;
                RaisePropertyChanged("Title");
            }
        }

        public ViewModelBase CurrentRegisterStep
        {
            get { return m_currentViewModel; }
            set
            {
                if (m_currentViewModel == value)
                    return;

                m_currentViewModel = value;
                RaisePropertyChanged("CurrentRegisterStep");
            }
        }

        private IModel CurrentScreen
        {
            get { return CurrentRegisterStep as IModel; }
        }

        private ICommand m_close;

        public ICommand Close
        {
            get { return m_close ?? (m_close = new RelayCommand(CommandReturn)); }
            set { m_close = value; }
        }

        private void CommandReturn()
        {
            MessageBoxResult result = MessageBox.Show("Cuidado ! Caso tenha feito alguma mudança elas não serão salvas. Deseja mesmo sair ?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
                return;
            
            if (ModoEdicao)
                ModoEdicao = false;

            ModeloRegistro = new Modelo();
            CurrentRegisterStep.Cleanup();

            WindowManager.Instance.CloseModelCreate();
        }

        public void SwitchView(RegistroModeloEnum viewModel)
        {
            if (viewModel == RegistroModeloEnum.NONE)
                return;

            if(!ModoEdicao && (Imagem == null || !Imagem.Equals(@"/Imagens/criacao-modelo.png")))
            {
                Imagem = @"/Imagens/criacao-modelo.png";
                Title = "F01002 - Cadastrar Modelos";
            }
            
            if (CurrentScreen != null)
                CurrentScreen.Save(ModeloRegistro); // salva os dados anteriores

            if (viewModel == RegistroModeloEnum.TELA_REGISTRO_UM)
                CurrentRegisterStep = ServiceLocator.Current.GetInstance<ModeloRegistroViewModel>();

            if (viewModel == RegistroModeloEnum.TELA_REGISTRO_DOIS)
                CurrentRegisterStep = ServiceLocator.Current.GetInstance<ModeloSelecaoViewModel>();

            if (CurrentScreen != null)
                CurrentScreen.Load(ModeloRegistro); // carrega os dados na próxima tela
        }

        private void IniciarEdicao(Modelo modeloEdicao)
        {
            if (Imagem == null || !Imagem.Equals(@"\Imagens\editar-modelo.png"))
            {
                Imagem = @"\Imagens\editar-modelo.png";
                Title = "F01003 - Editar Modelos";
            }
                
            ModoEdicao = true;
            ModeloRegistro = modeloEdicao.Clone() as Modelo;

            CurrentRegisterStep = ServiceLocator.Current.GetInstance<ModeloRegistroViewModel>();
            CurrentScreen.Load(ModeloRegistro);
        }

        private void FinalizarRegistro(TipoFinalizacaoEnum tipoFinalizacao)
        {
            if (tipoFinalizacao == TipoFinalizacaoEnum.LANCAMENTO)
                return;

            if (!ModoEdicao)
                ModeloRegistro.Id = DataSaveManager.Instance.GetModeloNextId();

            CurrentScreen.Save(ModeloRegistro);

            if (!ModoEdicao)
                DataSaveManager.Instance.SalvarModelo(ModeloRegistro);
            else
                DataSaveManager.Instance.AtualizarModelo(ModeloRegistro);

            ModoEdicao = false;
            Messenger.Default.Send(new InserirModeloMessage() { Modelo = ModeloRegistro });

            ModeloRegistro = new Modelo();
            CurrentRegisterStep.Cleanup();
        }
    }
}
