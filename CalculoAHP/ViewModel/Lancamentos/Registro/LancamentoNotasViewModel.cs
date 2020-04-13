using CalculoAHP.Domain;
using CalculoAHP.Domain.Enum;
using CalculoAHP.Model.Lancamentos.Registro;
using CalculoAHP.ViewModel.Lancamentos;
using CalculoAHP.ViewModel.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CalculoAHP.ViewModel
{
    public class LancamentoNotasViewModel : ViewModelBase, ILancamento
    {
        public LancamentoNotasViewModel()
        {
            Model = new LancamentoNotasModel();
        }

        public LancamentoNotasModel Model
        {
            get;
            set;
        }

        public ICommand ReturnScreen
        {
            get { return new RelayCommand(() =>
            {
                    Messenger.Default.Send(new SwitchViewMessage
                    {
                        ModeloViewModel = RegistroModeloEnum.NONE,
                        LancamentoViewModel = RegistroLancamentoEnum.TELA_REGISTRO_UM
                    });
                });
            }
        }

        private ICommand m_finishRegister;

        public ICommand FinishRegister
        {
            get { return m_finishRegister ?? (m_finishRegister = new RelayCommand(Finalizar)); }
        }

        private void Finalizar()
        {
            Messenger.Default.Send(new FinalizarRegistroMessage() { Tipo = TipoFinalizacaoEnum.LANCAMENTO });
            WindowManager.Instance.CloseLancamentoCreate();
        }

        public ICommand Decisao
        {
            get { return new RelayCommand(() => { Messenger.Default.Send(new GerarDecisaoMessage()); }); }
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
                case Key.F1:
                    Messenger.Default.Send(new SwitchViewMessage
                    {
                        ModeloViewModel = RegistroModeloEnum.NONE,
                        LancamentoViewModel = RegistroLancamentoEnum.TELA_REGISTRO_UM
                    });
                    break;
                case Key.F2:
                    Messenger.Default.Send(new GerarDecisaoMessage());
                    break;
                case Key.F3:
                    Finalizar();
                    break;
                default:
                    break;
            }
        }

        public void Load(Lancamento lancamento)
        {
            if (lancamento.Elementos.Count == 0)
                return;
            
            Model.Items = new ObservableCollection<AlternativaChoiceItem>(lancamento.Elementos.Select(entry => new AlternativaChoiceItem(entry.Nome, entry.Criterios)));
            Model.ColumnCollection.Clear();
            Model.SetColumns();
        }

        public void Save(Lancamento lancamento)
        {
            foreach (var alternativa in Model.Items)
            {
                var elementoLancamento = lancamento.Elementos.FirstOrDefault(entry => entry.Nome.Equals(alternativa.Nome));

                if (elementoLancamento == null)
                    continue;

                elementoLancamento.Criterios = alternativa.CriterioAlternativa;
            }
        }

        public override void Cleanup()
        {
            Model.Items.Clear();
            Model.ColumnCollection.Clear();
        }
    }
}
