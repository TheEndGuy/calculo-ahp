using CalculoAHP.Enum;
using CalculoAHP.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CalculoAHP.Model.Modelos.Registro
{
    public class CriterioChoiceItem : ModelBase
    {
        public CriterioChoiceItem(string nomePrimeiroCriterio, string nomeSegundoCriterio)
        {
            Id = Guid.NewGuid();
            CriterioId = Guid.NewGuid();
            PrimeiroCriterio = nomePrimeiroCriterio;
            SegundoCriterio = nomeSegundoCriterio;
            SelectedCriterio = CriterioOptionsEnum.CRITERIO_UM;
            SelectedOption = RadioOptionsEnum.IMPORTANCIA_1;
        }

        public CriterioChoiceItem(string nomePrimeiroCriterio, string nomeSegundoCriterio, CriterioOptionsEnum criterioOption, RadioOptionsEnum selectedOption)
        {
            Id = Guid.NewGuid();
            CriterioId = Guid.NewGuid();
            PrimeiroCriterio = nomePrimeiroCriterio;
            SegundoCriterio = nomeSegundoCriterio;
            SelectedCriterio = criterioOption;
            SelectedOption = selectedOption;
        }

        public Guid Id
        {
            get;
            private set;
        }

        public Guid CriterioId
        {
            get;
            private set;
        }

        public string PrimeiroCriterio
        {
            get;
            set;
        }

        public string SegundoCriterio
        {
            get;
            set;
        }

        private CriterioOptionsEnum m_selectedCriterio;

        public CriterioOptionsEnum SelectedCriterio
        {
            get { return m_selectedCriterio; }
            set
            {
                m_selectedCriterio = value;
                OnPropertyChanged("SelectedCriterio");
            }
        }

        private RadioOptionsEnum m_selectedOption;

        public RadioOptionsEnum SelectedOption
        {
            get { return m_selectedOption; }
            set
            {
                m_selectedOption = value;
                OnPropertyChanged("SelectedOption");
            }
        }
    }
}
