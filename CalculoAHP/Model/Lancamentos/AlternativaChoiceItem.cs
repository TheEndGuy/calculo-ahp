using CalculoAHP.Domain;
using CalculoAHP.Model.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP.Model.Lancamentos.Registro
{
    public class AlternativaChoiceItem : ModelBase
    {
        public AlternativaChoiceItem(string nomeAlternativa, List<CriterioAlternativa> listaCriterios)
        {
            Nome = nomeAlternativa;
            CriterioAlternativa = listaCriterios;
        }

        public string Nome
        {
            get;
            set;
        }

        public List<CriterioAlternativa> CriterioAlternativa
        {
            get;
            set;
        }
    }
}
