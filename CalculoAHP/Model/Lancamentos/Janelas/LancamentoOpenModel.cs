using CalculoAHP.Model.Dominio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP.Model.Lancamentos.Janelas
{
    public class LancamentoOpenModel : ModelBase
    {
        public LancamentoOpenModel(Lancamento lancamentoView)
        {
            Lancamento = lancamentoView;
            Items = new ObservableCollection<ItemDecisao>(Lancamento.ItensDecisao.OrderByDescending(entry=> entry.Peso));
        }

        private Lancamento m_lancamento;

        public Lancamento Lancamento
        {
            get { return m_lancamento; }
            set { m_lancamento = value; OnPropertyChanged("Lancamento"); }
        }

        private ObservableCollection<ItemDecisao> m_itens;

        public ObservableCollection<ItemDecisao> Items
        {
            get { return m_itens; }
            set { m_itens = value; OnPropertyChanged("Items"); }
        }
    }
}
