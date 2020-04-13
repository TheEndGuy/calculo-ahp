using CalculoAHP.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP.Model.Modelos.Registro
{
    public class ModeloSelecaoModel : ModelBase
    {
        public ModeloSelecaoModel()
        {
            Items = new ObservableCollection<CriterioChoiceItem>();
        }

        private ObservableCollection<CriterioChoiceItem> m_items;

        public ObservableCollection<CriterioChoiceItem> Items
        {
            get { return m_items; }
            set{ m_items = value; OnPropertyChanged("Items"); }
        }
        
        public List<CriterioChoiceItem> CriarCriterioChoiceItem(List<Criterio> listaCriterios)
        {
            List<CriterioChoiceItem> m_criteriosItens = new List<CriterioChoiceItem>();

            foreach (var criterion in listaCriterios)
            {   
                var restoCriterios = listaCriterios.Where(entry => listaCriterios.FindIndex(x => x == entry) > listaCriterios.FindIndex(x => x == criterion)).ToList();

                if (restoCriterios.Count <= 0)
                    continue;

                foreach (var criterioCombinacao in restoCriterios)
                    m_criteriosItens.Add(new CriterioChoiceItem(criterion.Nome, criterioCombinacao.Nome));
            }

            return m_criteriosItens;
        }
    }
}
