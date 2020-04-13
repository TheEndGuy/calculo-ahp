using CalculoAHP.Domain;
using CalculoAHP.Domain.Enum;
using CalculoAHP.ViewModel.Lancamentos.Janelas;
using CalculoAHP.ViewModel.MainScreens;
using CalculoAHP.ViewModel.Messages;
using CalculoAHP.ViewModel.Modelos;
using CalculoAHP.Views.Lancamentos.Janelas;
using CalculoAHP.Views.Modelo.Janelas;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP.ViewModel
{
    public class WindowManager
    {
        private WindowManager()
        {
        }

        private static WindowManager m_windowManager;

        public static WindowManager Instance
        {
            get { return m_windowManager ?? (m_windowManager = new WindowManager()); }
            set { m_windowManager = value; }
        }

        // open model
        private ModelOpenWindow ModelOpen
        {
            get;
            set;
        }

        // create - edit model
        private ModelCreateWindow ModelCreate
        {
            get;
            set;
        }

        // create - edit lancamento
        private LancamentoCreateWindow LancamentoCreate
        {
            get;
            set;
        }

        // view lancamento
        private LancamentoOpenWindow LancamentoOpen
        {
            get;
            set;
        }

        // information
        private InformationWindow InformationOpen
        {
            get;
            set;
        }

        public void ShowInformationWindow()
        {
            InformationOpen = new InformationWindow();
            InformationOpen.ShowDialog();
        }
        
        public void CloseInformationWindow()
        {
            if (InformationOpen == null)
                return;

            if (InformationOpen.IsEnabled)
                InformationOpen.Close();
        }

        #region Criação e edição de modelos

        public void ShowModelEdit(Modelo model)
        {
            ModelCreate = new ModelCreateWindow();

            Messenger.Default.Send(new IniciarEdicaoModeloMessage()
            {
                Modelo = model
            });

            ModelCreate.Focus();
            ModelCreate.ShowDialog();
        }

        public void ShowModelCreate()
        {
            ModelCreate = new ModelCreateWindow();

            Messenger.Default.Send(new SwitchViewMessage
            {
                ModeloViewModel = RegistroModeloEnum.TELA_REGISTRO_UM,
                LancamentoViewModel = RegistroLancamentoEnum.NONE
            });

            ModelCreate.ShowDialog();
        }

        public void CloseModelCreate()
        {
            if (ModelCreate == null)
                return;

            if (ModelCreate.IsEnabled)
                ModelCreate.Close();
        }

        #endregion

        #region Criação, edição e visualização dos lançamentos

        public void ShowLancamentoView(Lancamento lancamentoView)
        {
            LancamentoOpen = new LancamentoOpenWindow()
            {
                DataContext = new LancamentoOpenViewModel(lancamentoView)
            };

            LancamentoOpen.ShowDialog();
        }

        public void CloseLancamentoView()
        {
            if (LancamentoOpen == null)
                return;

            if (LancamentoOpen.IsEnabled)
                LancamentoOpen.Close();
        }

        public void ShowModelOpen(Modelo model)
        {
            ModelOpen = new ModelOpenWindow()
            {
                DataContext = new ModelOpenViewModel(model)
            };

            ModelOpen.ShowDialog();
        }

        public void CloseModelOpen()
        {
            if (ModelOpen == null)
                return;

            if (ModelOpen.IsEnabled)
                ModelOpen.Close();
        }

        public void ShowLancamentoEdit(Lancamento lancamentoEdit)
        {
            LancamentoCreate = new LancamentoCreateWindow();

            Messenger.Default.Send(new IniciarEdicaoLancamentoMessage()
            {
                Lancamento = lancamentoEdit
            });

            LancamentoCreate.ShowDialog();
        }

        public void ShowLancamentoCreate(Modelo modeloLancamento)
        {
            LancamentoCreate = new LancamentoCreateWindow();
            (LancamentoCreate.DataContext as RegistroLancamentoMain).LancamentoRegistro.Modelo = modeloLancamento;

            Messenger.Default.Send(new SwitchViewMessage
            {
                LancamentoViewModel = RegistroLancamentoEnum.TELA_REGISTRO_UM,
                ModeloViewModel = RegistroModeloEnum.NONE
            });

            LancamentoCreate.ShowDialog();
        }

        public void CloseLancamentoCreate()
        {
            if (LancamentoCreate == null)
                return;

            if (LancamentoCreate.IsEnabled)
                LancamentoCreate.Close();
        }

        #endregion
    }
}
