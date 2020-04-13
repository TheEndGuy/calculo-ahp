using CalculoAHP.Domain;
using CalculoAHP.Model.Dominio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP
{
    public class Modelo : ICloneable
    {
        [JsonConstructor]
        public Modelo(int id,string nome, string descricao, List<Criterio> criterios, List<CriterioItem> criteriosItems)
        {
            Id = id;
            Nome = nome;
            Descricao = descricao;
            Criterios = criterios;
            CriteriosItems = criteriosItems;
        }

        public Modelo()
        {
            CriteriosItems = new List<CriterioItem>();
            Criterios = new List<Criterio>();
        }

        public Modelo(string modeloNome, string modeloDescricao, List<CriterioItem> listaItems, List<Criterio> listaCriterios)
        {
            Nome = modeloNome;
            Descricao = modeloNome;
            CriteriosItems = listaItems;
            Criterios = listaCriterios;
        }
        
        public int Id
        {
            get;
            set;
        }

        public string Nome
        {
            get;
            set;
        }

        public string Descricao
        {
            get;
            set;
        }

        public List<Criterio> Criterios
        {
            get;
            set;
        }

        public List<CriterioItem> CriteriosItems
        {
            get;
            set;
        }
        
        public void AssignFields(Modelo model)
        {
            Nome = model.Nome;
            Descricao = model.Descricao;
            Criterios = model.Criterios;
            CriteriosItems = model.CriteriosItems;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
