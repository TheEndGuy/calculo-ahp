using CalculoAHP.Enum;
using CalculoAHP.Model.Dominio.Relatorios;
using CalculoAHP.Model.Modelos;
using CalculoAHP.Model.Modelos.Janelas;
using CalculoAHP.ViewModel.Messages;
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

namespace CalculoAHP.ViewModel.Modelos
{
    public class ModelOpenViewModel : ViewModelBase
    {
        public ModelOpenViewModel(Modelo model)
        {
            Model = new ModelOpenModel(model);

            Messenger.Default.Register<InserirLancamentoMessage>(this, (inserirLancamentoMessage) =>
            {
                InserirLancamento(inserirLancamentoMessage.Lancamento);
            });
        }

        public ModelOpenModel Model
        {
            get;
            set;
        }

        private ICommand m_return;

        public ICommand Return
        {
            get { return m_return ?? (m_return = new RelayCommand(CommandReturn)); }
            set { m_return = value; }
        }

        private ICommand m_create;

        public ICommand Create
        {
            get { return m_create ?? (m_create = new RelayCommand(CommandCreate)); }
            set { m_create = value; }
        }

        private ICommand m_update;

        public ICommand Update
        {
            get { return m_update ?? (m_update = new RelayCommand(CommandUpdate)); }
            set { m_update = value; }
        }

        private ICommand m_removeLancamento;

        public ICommand RemoveLancamento
        {
            get { return m_removeLancamento ?? (m_removeLancamento = new RelayCommand(RemoverLancamento)); }
        }

        private ICommand m_visualizarLancamento;

        public ICommand VisualizarLancamento
        {
            get { return m_visualizarLancamento ?? (m_visualizarLancamento = new RelayCommand(CommandVisualizar)); }
        }

        private SnackbarMessageQueue m_customMessage;

        public SnackbarMessageQueue CustomMessage
        {
            get { return m_customMessage ?? (m_customMessage = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(2000))); }
        }

        private void InserirLancamento(Lancamento lancamentoInsercao)
        {
            // Edição
            if (Model.Lancamentos.Any(entry => entry.Id == lancamentoInsercao.Id))
            {
                var oldLancamento = Model.Lancamentos.FirstOrDefault(entry => entry.Id == lancamentoInsercao.Id);

                if (oldLancamento == null)
                    return;

                Model.Lancamentos.Remove(oldLancamento);
                Model.Lancamentos.Add(lancamentoInsercao);
            }
                
            // Inserção
            else
                Model.Lancamentos.Add(lancamentoInsercao);
        }

        private void RemoverLancamento()
        {
            if (Model.SelectedItem == null || !Model.Lancamentos.Contains(Model.SelectedItem))
            {
                CustomMessage.Enqueue("Por favor, selecione um lançamento para remoção.", true);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Deseja mesmo apagar o lançamento '" + Model.SelectedItem.Nome + "' ?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
                return;

            Model.RemoveLancamento();
        }

        private void CommandUpdate()
        {
            if (Model.SelectedItem == null || !Model.Lancamentos.Contains(Model.SelectedItem))
            {
                CustomMessage.Enqueue("Por favor, selecione uma decisão para edição.", true);
                return;
            }

            if(Model.SelectedItem.Estado == Enum.EstadoLancamentoEnum.CONCLUIDO)
            {
                CustomMessage.Enqueue("Esta decisão já foi concluída, não é possível editá-la.", true);
                return;
            }

            WindowManager.Instance.ShowLancamentoEdit(Model.SelectedItem);
        }

        private void CommandVisualizar()
        {
            if (Model.SelectedItem == null || !Model.Lancamentos.Contains(Model.SelectedItem))
            {
                CustomMessage.Enqueue("Por favor, selecione uma decisão para visualização.", true);
                return;
            }

            if (Model.SelectedItem.Estado == Enum.EstadoLancamentoEnum.PROCESSANDO)
            {
                CustomMessage.Enqueue("Esta decisão ainda não foi gerada, não é possível visualiza-la.", true);
                return;
            }

            WindowManager.Instance.ShowLancamentoView(Model.SelectedItem);
        }

        private void CommandCreate()
        {
            WindowManager.Instance.ShowLancamentoCreate(Model.Modelo);
        }

        private void CommandReturn()
        {
            WindowManager.Instance.CloseModelOpen();
        }
    }
}