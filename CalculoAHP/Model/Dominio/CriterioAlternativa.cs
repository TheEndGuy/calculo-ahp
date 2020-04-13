using CalculoAHP.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP.Model.Dominio
{
    public class CriterioAlternativa
    {
        public CriterioAlternativa()
        {
        }

        [JsonConstructor]
        public CriterioAlternativa(Criterio criterio, int importancia)
        {
            Criterio = criterio;
            Importancia = importancia;
        }

        public Criterio Criterio
        {
            get;
            set;
        }

        public int Importancia
        {
            get;
            set;
        }
    }
}
