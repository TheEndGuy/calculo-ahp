using CalculoAHP.Domain.Enum;
using CalculoAHP.Model.Lancamentos.Registro;
using CalculoAHP.ViewModel.Lancamentos;
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

namespace CalculoAHP.ViewModel
{
    public class LancamentoAlternativaViewModel : ViewModelBase, ILancamento
    {
        public LancamentoAlternativaViewModel()
        {
            Model = new LancamentoAlternativaModel();

            Dialog = new DialogViewModel("AlternativaDialog");
            Dialog.OnCreateEvent += AdicionarAlternativa;
        }

        public DialogViewModel Dialog
        {
            get;
            set;
        }

        public LancamentoAlternativaModel Model
        {
            get;
            set;
        }

        private ICommand m_openDialog;

        public ICommand OpenDialog
        {
            get { return m_openDialog ?? (m_openDialog = new RelayCommand(Dialog.Execute)); }
        }

        private ICommand m_removeAlternativa;

        public ICommand RemoveAlternativa
        {
            get { return m_removeAlternativa ?? (m_removeAlternativa = new RelayCommand(RemoverAlternativa)); }
        }

        private ICommand m_nexScreen;

        public ICommand NextScreen
        {
            get { return m_nexScreen ?? (m_nexScreen = new RelayCommand(ChangeScreen)); }
        }

        private SnackbarMessageQueue m_customMessage;

        public SnackbarMessageQueue CustomMessage
        {
            get { return m_customMessage ?? (m_customMessage = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(2000))); }
        }

        public ICommand OnKeyPressed
        {
            get { return new RelayCommand<KeyEventArgs>(KeyDown); }
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
                // Remover alternativa
                case Key.F2:
                    RemoverAlternativa();
                    break;
                // Abrir dialog
                case Key.F3:
                    Dialog.Execute();
                    break;
                default:
                    break;
            }
        }

        private void AdicionarAlternativa(string alternativaCreate)
        {
            if (Model.ListaAlternativas.Any(entry => entry.Equals(alternativaCreate)))
            {
                CustomMessage.Enqueue("Alternativa já existente na lista.", true);
                return;
            }

            Model.AdicionarAlternativa(alternativaCreate);
        }

        private void RemoverAlternativa()
        {
            if (Model.AlternativaSelecionada == null || Model.AlternativaSelecionada.Equals("") || !Model.ListaAlternativas.Contains(Model.AlternativaSelecionada))
            {
                CustomMessage.Enqueue("Por favor, selecione uma alternativa para remoção.", true);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Deseja mesmo excluir a alternativa " + Model.AlternativaSelecionada + " ?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
                return;

            Model.RemoverAlternativa();
        }

        private void ChangeScreen()
        {
            if (Model.Nome == null || Model.Nome.Equals(""))
            {
                CustomMessage.Enqueue("Por favor, preencha o campo 'Nome da decisão'.", true);
                return;
            }

            if (Model.Descricao == null || Model.Descricao.Equals(""))
            {
                CustomMessage.Enqueue("Por favor, preencha o campo 'Descrição da decisão'.", true);
                return;
            }

            if (Model.ListaAlternativas.Count == 0)
            {
                CustomMessage.Enqueue("Por favor, insira pelo menos uma alternativa.", true);
                return;
            }

            if (Dialog.IsOpened())
                Dialog.Close();

            Messenger.Default.Send(new SwitchViewMessage
            {
                ModeloViewModel = RegistroModeloEnum.NONE,
                LancamentoViewModel = RegistroLancamentoEnum.TELA_REGISTRO_DOIS
            });
        }

        public void Load(Lancamento lancamento)
        {
            Model.Nome = lancamento.Nome;
            Model.Descricao = lancamento.Descricao;
            Model.ListaAlternativas.Clear();

            foreach (var elemento in lancamento.Elementos)
                Model.ListaAlternativas.Add(elemento.Nome);
        }

        public void Save(Lancamento lancamento)
        {
            lancamento.Nome = Model.Nome;
            lancamento.Descricao = Model.Descricao;

            if(lancamento.Elementos.Count == 0)
                lancamento.CriarElementos(Model.ListaAlternativas.ToArray());
            
            else
            {
                // removemos velhos elementos
                foreach (var element in lancamento.Elementos.ToList())
                {
                    if (Model.ListaAlternativas.Any(entry => entry.Equals(element.Nome)))
                        continue;

                    lancamento.Elementos.Remove(element);
                }

                // adicionamos novos elementos
                foreach(var elemento in Model.ListaAlternativas)
                {
                    if (lancamento.Elementos.Any(entry => entry.Nome.Equals(elemento)))
                        continue;

                    lancamento.CriarElemento(elemento);
                }
            }
        }

        public override void Cleanup()
        {
            if (Model == null)
                return;

            Model.Nome = "";
            Model.Descricao = "";
            Model.AlternativaSelecionada = "";
            Model.ListaAlternativas.Clear();
        }
    }
}
