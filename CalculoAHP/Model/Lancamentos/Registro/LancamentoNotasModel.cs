using CalculoAHP.Domain;
using CalculoAHP.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace CalculoAHP.Model.Lancamentos.Registro
{
    public class LancamentoNotasModel : ModelBase
    {
        public LancamentoNotasModel()
        {
            ColumnCollection = new ObservableCollection<DataGridColumn>();
            Items = new ObservableCollection<AlternativaChoiceItem>();
        }

        private ObservableCollection<AlternativaChoiceItem> m_items;

        public ObservableCollection<AlternativaChoiceItem> Items
        {
            get { return m_items; }
            set { m_items = value; OnPropertyChanged("Items"); }
        }

        private ObservableCollection<DataGridColumn> m_columnCollection;

        public ObservableCollection<DataGridColumn> ColumnCollection
        {
            get { return m_columnCollection; }
            set { m_columnCollection = value; OnPropertyChanged("ColumnCollection"); }
        }

        private DataGridCellInfo m_cellInfo;

        public DataGridCellInfo CellInfo
        {
            get { return m_cellInfo; }
            set
            {
                m_cellInfo = value;
                OnPropertyChanged("CellInfo");
            }
        }

        public void SetColumns()
        {
            ColumnCollection.Add(new DataGridTextColumn()
            {
                Header = "Nome",
                Binding = new Binding("Nome")
            });

            foreach(var criterio in Items.First().CriterioAlternativa)
            {
                ColumnCollection.Add(new DataGridTemplateColumn()
                {
                    Header = criterio.Criterio.Nome,
                    CellTemplate = new DataTemplate()
                    {
                        VisualTree = CreateElement(criterio.Criterio.Nome)
                    }
                });
            }
        }

        private FrameworkElementFactory CreateElement(string nomeCriterio)
        {
            var frameworkElement = new FrameworkElementFactory(typeof(IntegerUpDown));

            Binding bindingElement = new System.Windows.Data.Binding(".")
            {
                Converter = new AlternativaItemConverter(nomeCriterio),
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            frameworkElement.AddHandler(IntegerUpDown.ValueChangedEvent, new RoutedPropertyChangedEventHandler<object>(IntegerValueChanged));

            frameworkElement.SetBinding(IntegerUpDown.ValueProperty, bindingElement);
            frameworkElement.SetValue(IntegerUpDown.MinimumProperty, 1); // Valor mínimo
            frameworkElement.SetValue(IntegerUpDown.MaximumProperty, 10); // Valor máximo
          
            return frameworkElement;
        }

        private void IntegerValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (sender == null)
                return;

            if (CellInfo.Column == null)
                return;
            
            var columnIndex = CellInfo.Column.DisplayIndex - 1;

            if (columnIndex == -1)
                return;

            if (ColumnCollection.Count - 1 < columnIndex)
                return;

            var item = Items.FirstOrDefault(entry => entry == CellInfo.Item as AlternativaChoiceItem);

            if (item == null)
                return;

            item.CriterioAlternativa[columnIndex].Importancia = Convert.ToInt32((sender as IntegerUpDown).Value);
        }
    }
}
