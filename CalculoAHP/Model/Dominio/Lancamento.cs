using CalculoAHP.Domain;
using CalculoAHP.Enum;
using CalculoAHP.Model.Dominio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP
{
    public class Lancamento : ICloneable
    {
        [JsonConstructor]
        public Lancamento(int id, string nome, DateTime dataLancamento, string descricao, EstadoLancamentoEnum estado, Modelo modelo, List<ElementoLancamento> elementos, List<ItemDecisao> itensDecisao)
        {
            Id = id;
            Nome = nome;
            DataLancamento = dataLancamento;
            Descricao = descricao;
            Estado = estado;
            Modelo = modelo;
            Elementos = elementos;
            ItensDecisao = itensDecisao;
        }

        public Lancamento()
        {
            Elementos = new List<ElementoLancamento>();
            ItensDecisao = new List<ItemDecisao>();
            Estado = EstadoLancamentoEnum.PROCESSANDO;
        }

        public Lancamento(string nomeLancamento, DateTime dataLancamento, string descricaoLancamento, List<ElementoLancamento> elementos, Modelo modeloLancamento)
        {
            Nome = nomeLancamento;
            DataLancamento = dataLancamento;
            Descricao = descricaoLancamento;
            Elementos = elementos;
            Modelo = modeloLancamento;
            Estado = EstadoLancamentoEnum.CONCLUIDO;
            ItensDecisao = new List<ItemDecisao>();
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

        [JsonIgnore]
        public string Data
        {
            get { return DataLancamento == new DateTime(01, 01, 0001) ? "Não finalizado" : DataLancamento.ToString("dd/MM/yyyy"); }
            set { Data = value; }
        }

        public DateTime DataLancamento
        {
            get;
            set;
        }

        public string Descricao
        {
            get;
            set;
        }

        public EstadoLancamentoEnum Estado
        {
            get;
            set;
        }

        public Modelo Modelo
        {
            get;
            set;
        }

        public List<ElementoLancamento> Elementos
        {
            get;
            set;
        }

        public List<ItemDecisao> ItensDecisao
        {
            get;
            set;
        }

        public void CriarElemento(string nomeElemento)
        {
            Elementos.Add(new ElementoLancamento(nomeElemento, Modelo.Criterios.Select(entry => new CriterioAlternativa(entry, 0)).ToList()));
        }
        
        public void CriarElementos(string[] listaNomes)
        {
            foreach (var nome in listaNomes)
                Elementos.Add(new ElementoLancamento(nome, Modelo.Criterios.Select(entry => new CriterioAlternativa(entry, 0)).ToList()));
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public void AssignFields(Lancamento lancamento)
        {
            Nome = lancamento.Nome;
            Descricao = lancamento.Descricao;
            Estado = lancamento.Estado;
            DataLancamento = lancamento.DataLancamento;
            Elementos = lancamento.Elementos;
        }
    }
}
