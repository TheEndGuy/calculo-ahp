using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP.Model.Modelos.Registro
{
    public class ModeloRegistroModel : ModelBase
    {
        public ModeloRegistroModel()
        {
            ListaCriterios = new ObservableCollection<string>();
        }

        private string m_nomeModelo;

        public string Nome
        {
            get { return m_nomeModelo; }
            set { m_nomeModelo = value; OnPropertyChanged("Nome"); }
        }

        private string m_descricaoModelo;

        public string Descricao
        {
            get { return m_descricaoModelo; }
            set { m_descricaoModelo = value; OnPropertyChanged("Descricao"); }
        }

        private string m_criterioSelecionado;

        public string CriterioSelecionado
        {
            get { return m_criterioSelecionado; }
            set { m_criterioSelecionado = value; OnPropertyChanged("CriterioSelecionado"); }
        }
        
        private ObservableCollection<string> m_listaCriterios;

        public ObservableCollection<string> ListaCriterios
        {
            get { return m_listaCriterios; }
            set { m_listaCriterios = value; OnPropertyChanged("ListaCriterios"); }
        }

        public void AdicionarCriterio(string criterio)
        {
            ListaCriterios.Add(criterio);
        }

        public void RemoverCriterio()
        {
            if (CriterioSelecionado == null)
                return;

            if (CriterioSelecionado.Equals(""))
                return;

            if (!ListaCriterios.Contains(CriterioSelecionado))
                return;

            ListaCriterios.Remove(CriterioSelecionado);
        }
    }
}
