using CalculoAHP.Domain.Enum;
using CalculoAHP.Model.Modelos.Registro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoAHP.ViewModel.Messages
{
    public class FinalizarRegistroMessage
    {
        public TipoFinalizacaoEnum Tipo
        {
            get;
            set;
        }
    }
}
