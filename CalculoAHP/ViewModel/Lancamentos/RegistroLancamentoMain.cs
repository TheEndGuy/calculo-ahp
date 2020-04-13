using CalculoAHP.Domain.Enum;
using CalculoAHP.Enum;
using CalculoAHP.Model.Dominio;
using CalculoAHP.Model.Dominio.Relatorios;
using CalculoAHP.Model.Modelos.Registro;
using CalculoAHP.ViewModel.Lancamentos;
using CalculoAHP.ViewModel.Messages;
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
    public class RegistroLancamentoMain : ViewModelBase
    {
        private ViewModelBase m_currentViewModel;

        public RegistroLancamentoMain()
        {
            LancamentoRegistro = new Lancamento();

            Messenger.Default.Register<SwitchViewMessage>(this, (switchViewMessage) =>
            {
                SwitchView(switchViewMessage.LancamentoViewModel);
            });

            Messenger.Default.Register<FinalizarRegistroMessage>(this, (finalizarRegistroMessage) =>
            {
                FinalizarRegistro(finalizarRegistroMessage.Tipo);
            });

            Messenger.Default.Register<IniciarEdicaoLancamentoMessage>(this, (iniciarEdicaoLancamentoMessage) =>
            {
                IniciarEdicao(iniciarEdicaoLancamentoMessage.Lancamento);
            });

            Messenger.Default.Register<GerarDecisaoMessage>(this, (gerarDecisaoMessage) =>
            {
                GerarDecisao();
            });
        }

        public Lancamento LancamentoRegistro
        {
            get;
            set;
        }

        public bool ModoEdicao
        {
            get;
            set;
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
        
        private ILancamento CurrentScreen
        {
            get { return CurrentRegisterStep as ILancamento; }
        }

        private ICommand m_close;

        public ICommand Close
        {
            get { return m_close ?? (m_close = new RelayCommand(CommandReturn)); }
            set { m_close = value; }
        }

        private void CommandReturn()
        {
            if (ModoEdicao)
                ModoEdicao = false;

            MessageBoxResult result = MessageBox.Show("Deseja salvar as alterações realizadas para continuar mais tarde ?", "Atenção", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            if (result == MessageBoxResult.Cancel)
                return;

            if (result == MessageBoxResult.Yes)
            {
                FinalizarRegistro(TipoFinalizacaoEnum.LANCAMENTO, false);

                WindowManager.Instance.CloseLancamentoCreate();
                return;
            }

            LancamentoRegistro = new Lancamento();
            CurrentRegisterStep.Cleanup();

            WindowManager.Instance.CloseLancamentoCreate();
        }

        public void SwitchView(RegistroLancamentoEnum viewModel)
        {
            if (viewModel == RegistroLancamentoEnum.NONE)
                return;

            if (!ModoEdicao && (Imagem == null || !Imagem.Equals(@"/Imagens/criar-lancamento.png")))
            {
                Imagem = @"/Imagens/criar-lancamento.png";
                Title = "F02002 - Cadastrar Decisões";
            }

            if (CurrentScreen != null)
                CurrentScreen.Save(LancamentoRegistro); // salva os dados anteriores

            if (viewModel == RegistroLancamentoEnum.TELA_REGISTRO_UM)
                CurrentRegisterStep = ServiceLocator.Current.GetInstance<LancamentoAlternativaViewModel>();

            if (viewModel == RegistroLancamentoEnum.TELA_REGISTRO_DOIS)
                CurrentRegisterStep = ServiceLocator.Current.GetInstance<LancamentoNotasViewModel>();

            if (CurrentScreen != null)
                CurrentScreen.Load(LancamentoRegistro); // carrega os dados na próxima tela
        }

        private void GerarDecisao()
        {
            CurrentScreen.Save(LancamentoRegistro);
            Algoritmo.Algoritmo.Instance.CalcularDecisao(LancamentoRegistro);

            WindowManager.Instance.ShowLancamentoView(LancamentoRegistro);
        }
        
        private void IniciarEdicao(Lancamento lancamentoEdicao)
        {
            if (Imagem == null || !Imagem.Equals(@"\Imagens\editar-lancamento.png"))
            {
                Imagem = @"\Imagens\editar-lancamento.png";
                Title = "F02003 - Editar Decisões";
            }
       
            ModoEdicao = true;
            LancamentoRegistro = lancamentoEdicao.Clone() as Lancamento;

            CurrentRegisterStep = ServiceLocator.Current.GetInstance<LancamentoAlternativaViewModel>();
            CurrentScreen.Load(LancamentoRegistro);
        }

        private void FinalizarRegistro(TipoFinalizacaoEnum tipoFinalizacao, bool conclusaoRegistro = true)
        {
            if (tipoFinalizacao == TipoFinalizacaoEnum.MODELO)
                return;

            if (!ModoEdicao)
                LancamentoRegistro.Id = DataSaveManager.Instance.GetLancamentoNextId();

            if (conclusaoRegistro)
            {
                Algoritmo.Algoritmo.Instance.CalcularDecisao(LancamentoRegistro);
                LancamentoRegistro.Estado = EstadoLancamentoEnum.CONCLUIDO;
                LancamentoRegistro.DataLancamento = DateTime.Now;
            }
            else
                LancamentoRegistro.Estado = Enum.EstadoLancamentoEnum.PROCESSANDO;

            CurrentScreen.Save(LancamentoRegistro);

            if (!ModoEdicao)
                DataSaveManager.Instance.SalvarLancamento(LancamentoRegistro);
            else
                DataSaveManager.Instance.AtualizarLancamento(LancamentoRegistro);

            ModoEdicao = false;
            Messenger.Default.Send(new InserirLancamentoMessage() { Lancamento = LancamentoRegistro });

            LancamentoRegistro = new Lancamento();
            CurrentRegisterStep.Cleanup();
        }
    }
}
