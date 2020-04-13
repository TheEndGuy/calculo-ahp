using CalculoAHP.Domain;
using CalculoAHP.Domain.Enum;
using CalculoAHP.Model.Modelos.Registro;
using CalculoAHP.ViewModel.Messages;
using CalculoAHP.ViewModel.Modelos;
using CalculoAHP.ViewModel.Modelos.Dialog;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CalculoAHP.ViewModel
{
    public class ModeloRegistroViewModel : ViewModelBase, IModel
    {
        public ModeloRegistroViewModel()
        {
            Model = new ModeloRegistroModel();

            Dialog = new DialogViewModel("CriterioDialog");
            Dialog.OnCreateEvent += AdicionarCriterio;
        }

        public ModeloRegistroModel Model
        {
            get;
            set;
        }

        public DialogViewModel Dialog
        {
            get;
            set;
        }

        private ICommand m_openDialog;

        public ICommand OpenDialog
        {
            get { return m_openDialog ?? (m_openDialog = new RelayCommand(Dialog.Execute)); }
        }

        private ICommand m_removeCriterio;

        public ICommand RemoveCriterio
        {
            get { return m_removeCriterio ?? (m_removeCriterio = new RelayCommand(RemoverCriterio)); }
        }

        private ICommand m_nexScreen;

        public ICommand NextScreen
        {
            get { return m_nexScreen ?? (m_nexScreen = new RelayCommand(ChangeScreen)); }
        }

        public ICommand OnKeyPressed
        {
            get { return new RelayCommand<KeyEventArgs>(KeyDown); }
        }
        
        private SnackbarMessageQueue m_customMessage;

        public SnackbarMessageQueue CustomMessage
        {
            get { return m_customMessage ?? (m_customMessage = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(2000))); }
        }
        
        public void KeyDown(KeyEventArgs e)
        {
            if (e == null)
                return;
            
            switch (e.Key)
            {
                // Mudar de tela
                case Key.F1:
                    ChangeScreen();
                    break;
                // Remover criterio
                case Key.F2:
                    RemoverCriterio();
                    break;
                // Abrir dialog
                case Key.F3:
                    Dialog.Execute();
                    break;
                default:
                    break;
            }
        }

        private void AdicionarCriterio(string criterioCreate)
        {
            if (Model.ListaCriterios.Any(entry => entry.Equals(criterioCreate)))
            {
                CustomMessage.Enqueue("Critério já existente na lista.", true);
                return;
            }

            Model.AdicionarCriterio(criterioCreate);
        }

        private void RemoverCriterio()
        {
            if (Model.CriterioSelecionado == null || Model.CriterioSelecionado.Equals("") || !Model.ListaCriterios.Contains(Model.CriterioSelecionado))
            {
                CustomMessage.Enqueue("Por favor, selecione um critério para remoção.", true);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Deseja mesmo excluir o critério " + Model.CriterioSelecionado + " ?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
                return;

            Model.RemoverCriterio();
        }

        private void ChangeScreen()
        {
            if (Model.Nome == null || Model.Nome.Equals(""))
            {
                CustomMessage.Enqueue("Por favor, preencha o campo 'Nome do modelo'.", true);
                return;
            }

            if (Model.Descricao == null || Model.Descricao.Equals(""))
            {
                CustomMessage.Enqueue("Por favor, preencha o campo 'Descrição do modelo'.", true);
                return;
            }

            if (Model.ListaCriterios.Count <= 1)
            {
                CustomMessage.Enqueue("Por favor, insira pelo menos dois critérios.", true);
                return;
            }

            if (Dialog.IsOpened())
                Dialog.Close();

            Messenger.Default.Send(new SwitchViewMessage
            {
                ModeloViewModel = RegistroModeloEnum.TELA_REGISTRO_DOIS,
                LancamentoViewModel = RegistroLancamentoEnum.NONE
            });
        }

        public void Load(Modelo model)
        {
            Model.Nome = model.Nome;
            Model.Descricao = model.Descricao;
            Model.ListaCriterios.Clear();

            foreach (var criterio in model.Criterios)
                Model.ListaCriterios.Add(criterio.Nome);
        }

        public void Save(Modelo model)
        {
            model.Nome = Model.Nome;
            model.Descricao = Model.Descricao;
            model.Criterios.Clear();

            foreach (var criterio in Model.ListaCriterios)
                model.Criterios.Add(new Criterio(criterio));
        }

        public override void Cleanup()
        {
            if (Model == null)
                return;

            Model.Nome = "";
            Model.Descricao = "";
            Model.CriterioSelecionado = "";
            Model.ListaCriterios.Clear();
        }
    }
}
