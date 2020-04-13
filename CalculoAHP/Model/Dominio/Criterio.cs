using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP.Domain
{
    public class Criterio
    {
        [JsonConstructor]
        public Criterio(string nome)
        {
            Nome = nome;
        }

        public string Nome
        {
            get;
            set;
        }
    }
}
