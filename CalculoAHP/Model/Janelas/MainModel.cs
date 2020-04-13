using CalculoAHP.Domain;
using CalculoAHP.Model.Dominio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP.Model.Modelos
{
    public class MainModel : ModelBase
    {
        public MainModel()
        {
            ModelosCollection = new ObservableCollection<Modelo>(DataSaveManager.Instance.GetModelos());
        }

        private ObservableCollection<Modelo> m_modelosCollection;

        public ObservableCollection<Modelo> ModelosCollection
        {
            get { return m_modelosCollection; }
            set { m_modelosCollection = value; OnPropertyChanged("ModelosCollection"); }
        }

        public void RemoveModel(Modelo modelRemocao)
        {
            var modelRemove = ModelosCollection.FirstOrDefault(entry => entry.Id == modelRemocao.Id);

            if (modelRemove == null)
                return;

            DataSaveManager.Instance.RemoverModelo(modelRemove);
            ModelosCollection.Remove(modelRemocao);
        }
    }
}
