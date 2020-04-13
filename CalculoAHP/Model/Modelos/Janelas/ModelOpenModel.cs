using CalculoAHP.Model.Dominio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP.Model.Modelos.Janelas
{
    public class ModelOpenModel : ModelBase
    {
        public ModelOpenModel(Modelo modelo)
        {
            Modelo = modelo;
            Lancamentos = new ObservableCollection<Lancamento>(DataSaveManager.Instance.GetLancamentosModelo(modelo));
        }

        private Lancamento m_selectedItem;

        public Lancamento SelectedItem
        {
            get { return m_selectedItem; }
            set { m_selectedItem = value; OnPropertyChanged("SelectedItem"); }
        }

        private Modelo m_modelo;

        public Modelo Modelo
        {
            get { return m_modelo; }
            set { m_modelo = value; OnPropertyChanged("Modelo"); }
        }

        private ObservableCollection<Lancamento> m_lancamentos;

        public ObservableCollection<Lancamento> Lancamentos
        {
            get { return m_lancamentos; }
            set { m_lancamentos = value; OnPropertyChanged("Lancamentos"); }
        }

        public void RemoveLancamento()
        {
            var lancamentoRemove = Lancamentos.FirstOrDefault(entry => entry.Id == SelectedItem.Id);

            if (lancamentoRemove == null)
                return;

            DataSaveManager.Instance.RemoverLancamento(lancamentoRemove);
            Lancamentos.Remove(lancamentoRemove);
        }
    }
}
