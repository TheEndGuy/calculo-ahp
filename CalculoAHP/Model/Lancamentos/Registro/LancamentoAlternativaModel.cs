using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP.Model.Lancamentos.Registro
{
    public class LancamentoAlternativaModel : ModelBase
    {
        public LancamentoAlternativaModel()
        {
            ListaAlternativas = new ObservableCollection<string>();
        }

        private string m_nomeLancamento;

        public string Nome
        {
            get { return m_nomeLancamento; }
            set { m_nomeLancamento = value; OnPropertyChanged("Nome"); }
        }

        private string m_descricaoLancamento;

        public string Descricao
        {
            get { return m_descricaoLancamento; }
            set { m_descricaoLancamento = value; OnPropertyChanged("Descricao"); }
        }

        private string m_alternativaSelecionada;

        public string AlternativaSelecionada
        {
            get { return m_alternativaSelecionada; }
            set { m_alternativaSelecionada = value; OnPropertyChanged("AlternativaSelecionada"); }
        }

        private ObservableCollection<string> m_listaAlternativas;

        public ObservableCollection<string> ListaAlternativas
        {
            get { return m_listaAlternativas; }
            set { m_listaAlternativas = value; OnPropertyChanged("ListaAlternativas"); }
        }

        public void AdicionarAlternativa(string alternativa)
        {
            ListaAlternativas.Add(alternativa);
        }

        public void RemoverAlternativa()
        {
            ListaAlternativas.Remove(AlternativaSelecionada);
        }
    }
}
