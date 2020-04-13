using CalculoAHP.Model.Lancamentos.Registro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CalculoAHP.ViewModel
{
    public class AlternativaItemConverter : IValueConverter
    {
        public AlternativaItemConverter(string nomeCriterio)
        {
            NomeCriterio = nomeCriterio;
        }

        public string NomeCriterio
        {
            get;
            set;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            if(value as AlternativaChoiceItem == null)
                return DependencyProperty.UnsetValue;

            if (!(value as AlternativaChoiceItem).CriterioAlternativa.Any(entry => entry.Criterio.Nome.Equals(NomeCriterio)))
                return DependencyProperty.UnsetValue;

            return (value as AlternativaChoiceItem).CriterioAlternativa.FirstOrDefault(entry => entry.Criterio.Nome.Equals(NomeCriterio)).Importancia;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return 1;

            return value;
        }
    }
}
