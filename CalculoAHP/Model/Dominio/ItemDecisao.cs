using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP.Model.Dominio
{
    public class ItemDecisao
    {
        [JsonConstructor]
        public ItemDecisao(string nome, double peso)
        {
            Nome = nome;
            Peso = peso;
        }

        public string Nome
        {
            get;
            set;
        }

        public double Peso
        {
            get;
            set;
        }
    }
}
