using CalculoAHP.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP.Model.Dominio
{
    public class ElementoLancamento
    {
        [JsonConstructor]
        public ElementoLancamento(string nome, List<CriterioAlternativa> criterios)
        {
            Nome = nome;
            Criterios = criterios;

            foreach (var criterio in Criterios.Where(entry => entry.Importancia == 0).ToList())
                criterio.Importancia = 1;
        }

        public ElementoLancamento(string nomeElemento)
        {
            Nome = nomeElemento;
        }

        //public ElementoLancamento(string nomeElemento, List<CriterioAlternativa> criteriosElemento)
        //{
        //    Nome = nomeElemento;
        //    Criterios = criteriosElemento;

        //    foreach (var criterio in Criterios.Where(entry => entry.Importancia == 0).ToList())
        //        criterio.Importancia = 1;
        //}

        public string Nome
        {
            get;
            set;
        }

        public List<CriterioAlternativa> Criterios
        {
            get;
            set;
        }
    }
}
