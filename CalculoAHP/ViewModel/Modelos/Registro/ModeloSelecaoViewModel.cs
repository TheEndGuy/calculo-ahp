using CalculoAHP.Domain;
using CalculoAHP.Domain.Enum;
using CalculoAHP.Enum;
using CalculoAHP.Model.Modelos.Registro;
using CalculoAHP.ViewModel.Messages;
using CalculoAHP.ViewModel.Modelos;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CalculoAHP.ViewModel
{
    public class ModeloSelecaoViewModel : ViewModelBase, IModel
    {
        public ModeloSelecaoViewModel()
        {
            Model = new ModeloSelecaoModel();
        }

        public ModeloSelecaoModel Model
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
                        ModeloViewModel = RegistroModeloEnum.TELA_REGISTRO_UM,
                        LancamentoViewModel = RegistroLancamentoEnum.NONE
                    });
            }); }
        }

        private ICommand m_finishRegister;

        public ICommand FinishRegister
        {
            get { return m_finishRegister ?? (m_finishRegister = new RelayCommand(Finalizar)); }
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
                        ModeloViewModel = RegistroModeloEnum.TELA_REGISTRO_UM,
                        LancamentoViewModel = RegistroLancamentoEnum.NONE
                    });
                    break;
                case Key.F2:
                    Finalizar();
                    break;
                default:
                    break;
            }
        }

        private void Finalizar()
        {
            Messenger.Default.Send(new FinalizarRegistroMessage() { Tipo = TipoFinalizacaoEnum.MODELO });
            WindowManager.Instance.CloseModelCreate();
        }

        public void Load(Modelo model)
        {
            Model.Items = new ObservableCollection<CriterioChoiceItem>();

            if (model.CriteriosItems.Count == 0)
            {
                Model.Items = new ObservableCollection<CriterioChoiceItem>(Model.CriarCriterioChoiceItem(model.Criterios));
                return;
            }

            foreach(var criterio in model.CriteriosItems)
            {
                if (!model.Criterios.Any(entry => entry.Nome.Equals(criterio.CriterioSelecionado.Nome)))
                    continue;

                if (!model.Criterios.Any(entry => entry.Nome.Equals(criterio.CriterioOpcao.Nome)))
                    continue;

                Model.Items.Add(new CriterioChoiceItem(criterio.CriterioOpcao.Nome, criterio.CriterioSelecionado.Nome, CriterioOptionsEnum.CRITERIO_DOIS, (RadioOptionsEnum)criterio.Importancia));
            }

            foreach (var criterio in Model.CriarCriterioChoiceItem(model.Criterios))
            {
                if (model.CriteriosItems.Any(entry => entry.Equals(criterio)))
                    continue;

                Model.Items.Add(criterio);
            }
        }

        public void Save(Modelo model)
        {
            model.CriteriosItems.Clear();
           
            foreach (var criterio in Model.Items)
            {
                var criterioSelecionado = criterio.SelectedCriterio == CriterioOptionsEnum.CRITERIO_UM ? new Criterio(criterio.PrimeiroCriterio) :
                                                                                                         new Criterio(criterio.SegundoCriterio);

                var segundoCriterio = criterio.PrimeiroCriterio.Equals(criterioSelecionado.Nome) ? new Criterio(criterio.SegundoCriterio) :
                                                                                                   new Criterio(criterio.PrimeiroCriterio);

                model.CriteriosItems.Add(new CalculoAHP.Model.Dominio.CriterioItem(criterioSelecionado, segundoCriterio, (int)criterio.SelectedOption));
            }
        }

        public override void Cleanup()
        {
            if (Model == null)
                return;

            Model.Items = new ObservableCollection<CriterioChoiceItem>();
        }
    }
}
    